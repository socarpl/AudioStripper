[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $OutputDirectory,
    [Parameter(Mandatory = $true)]
    [string] $CacheDirectory
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

function Get-Sha256 {
    param([Parameter(Mandatory = $true)][string] $Path)

    # Use .NET directly: Windows PowerShell launched by MSBuild may not be able
    # to discover Microsoft.PowerShell.Utility through an inherited module path.
    $algorithm = [System.Security.Cryptography.SHA256]::Create()
    try {
        $stream = [System.IO.File]::OpenRead($Path)
        try {
            $hash = $algorithm.ComputeHash($stream)
            return [System.BitConverter]::ToString($hash).Replace('-', '').ToLowerInvariant()
        }
        finally {
            $stream.Dispose()
        }
    }
    finally {
        $algorithm.Dispose()
    }
}

$outputPath = [System.IO.Path]::GetFullPath($OutputDirectory)
$executablePath = Join-Path $outputPath 'ffmpeg.exe'

# Respect an existing user-provided FFmpeg, without even accessing the network.
if (Test-Path -LiteralPath $executablePath -PathType Leaf) {
    Write-Output 'FFmpeg already exists; leaving it unchanged.'
    return
}

# Pinned monthly build: BtbN retains monthly releases for two years.
$release = 'autobuild-2026-09-30-13-08'
$archiveName = 'ffmpeg-n9.0.2-17-g2a571b6068-win64-lgpl-9.0.zip'
$expectedHash = '6b264b9e6019103f601d98c292bd332fd87acf1c5e941ddff4fb71760fe63432'
$downloadUrl = "https://github.com/BtbN/FFmpeg-Builds/releases/download/$release/$archiveName"
$cachePath = [System.IO.Path]::GetFullPath($CacheDirectory)
New-Item -ItemType Directory -Path $cachePath -Force | Out-Null
New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
$archivePath = Join-Path $cachePath $archiveName

if (!(Test-Path -LiteralPath $archivePath -PathType Leaf)) {
    $downloadPath = Join-Path $cachePath ([guid]::NewGuid().ToString('N') + '.download')
    try {
        Write-Output "Downloading pinned FFmpeg Windows x64 build ($release)..."
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        Invoke-WebRequest -Uri $downloadUrl -OutFile $downloadPath -UseBasicParsing -TimeoutSec 600
        if ((Get-Sha256 -Path $downloadPath) -ne $expectedHash) {
            throw 'Downloaded FFmpeg archive failed SHA-256 verification.'
        }
        Move-Item -LiteralPath $downloadPath -Destination $archivePath -Force
    }
    finally {
        if (Test-Path -LiteralPath $downloadPath) {
            Remove-Item -LiteralPath $downloadPath
        }
    }
}

if ((Get-Sha256 -Path $archivePath) -ne $expectedHash) {
    throw "Cached FFmpeg archive failed SHA-256 verification. Remove '$archivePath' and rebuild."
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
$archive = [System.IO.Compression.ZipFile]::OpenRead($archivePath)
$temporaryExecutable = Join-Path $outputPath ('ffmpeg-' + [guid]::NewGuid().ToString('N') + '.tmp')
try {
    $root = $archiveName.Substring(0, $archiveName.Length - 4)
    $binary = $archive.GetEntry("$root/bin/ffmpeg.exe")
    $license = $archive.GetEntry("$root/LICENSE.txt")
    if ($null -eq $binary -or $null -eq $license) {
        throw 'The verified FFmpeg archive does not contain the expected executable and license.'
    }

    [System.IO.Compression.ZipFileExtensions]::ExtractToFile($binary, $temporaryExecutable)
    [System.IO.Compression.ZipFileExtensions]::ExtractToFile($license, (Join-Path $outputPath 'FFmpeg-LICENSE.txt'), $true)
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'FFmpeg-COPYING.GPLv3.txt') -Destination $outputPath
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'FFmpeg-NOTICE.txt') -Destination $outputPath

    # Install only a complete, verified file; do not overwrite a concurrently supplied executable.
    if (!(Test-Path -LiteralPath $executablePath)) {
        [System.IO.File]::Move($temporaryExecutable, $executablePath)
    }
    Write-Output "FFmpeg is available at '$executablePath'."
}
finally {
    $archive.Dispose()
    if (Test-Path -LiteralPath $temporaryExecutable) {
        Remove-Item -LiteralPath $temporaryExecutable
    }
}
