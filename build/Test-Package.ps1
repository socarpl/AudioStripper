[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $OutputDirectory
)

$ErrorActionPreference = 'Stop'
$outputPath = [System.IO.Path]::GetFullPath($OutputDirectory)
foreach ($file in @('AudioStripper.exe', 'AudioStripper.dll', 'AudioStripper.runtimeconfig.json',
    'ffmpeg.exe', 'config.json', 'FFmpeg-LICENSE.txt', 'FFmpeg-COPYING.GPLv3.txt', 'FFmpeg-NOTICE.txt')) {
    if (!(Test-Path -LiteralPath (Join-Path $outputPath $file) -PathType Leaf)) {
        throw "Package is missing '$file'."
    }
}

$config = Get-Content -LiteralPath (Join-Path $outputPath 'config.json') -Raw | ConvertFrom-Json
if ([string]::IsNullOrWhiteSpace($config.FfmpegLocation) -or [string]::IsNullOrWhiteSpace($config.FfmpegParameters)) {
    throw 'The packaged default config is missing FFmpeg settings.'
}
$runtimeConfig = Get-Content -LiteralPath (Join-Path $outputPath 'AudioStripper.runtimeconfig.json') -Raw | ConvertFrom-Json
if ($runtimeConfig.runtimeOptions.includedFrameworks) {
    throw 'The published package must use the installed .NET runtime (--self-contained false).'
}
$frameworks = @($runtimeConfig.runtimeOptions.frameworks) + @($runtimeConfig.runtimeOptions.framework)
if (!($frameworks | Where-Object { $_.name -eq 'Microsoft.WindowsDesktop.App' -and $_.version -like '9.*' })) {
    throw 'The package must reference the .NET 9 Windows Desktop Runtime.'
}
foreach ($runtimeFile in @('coreclr.dll', 'hostfxr.dll', 'hostpolicy.dll', 'System.Private.CoreLib.dll')) {
    if (Test-Path -LiteralPath (Join-Path $outputPath $runtimeFile)) {
        throw "The package contains a bundled runtime file '$runtimeFile'. Publish into a fresh output folder."
    }
}

# Test real encoding in a temporary folder, keeping fixtures out of the artifact.
$testDirectory = Join-Path ([System.IO.Path]::GetTempPath()) ('AudioStripper-smoke-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testDirectory | Out-Null
$ffmpeg = Join-Path $outputPath 'ffmpeg.exe'
$video = Join-Path $testDirectory 'sample video.mp4'
$audio = Join-Path $testDirectory 'sample audio.mp3'
& $ffmpeg -nostdin -hide_banner -loglevel error -f lavfi -i 'color=c=black:s=32x32:d=0.25' -f lavfi -i 'sine=frequency=440:duration=0.25' -c:v mpeg4 -c:a aac -shortest $video
if ($LASTEXITCODE -ne 0) { throw 'FFmpeg failed to create the video fixture.' }
& $ffmpeg -nostdin -hide_banner -loglevel error -i $video -vn -c:a libmp3lame -q:a 2 $audio
if ($LASTEXITCODE -ne 0 -or !(Test-Path -LiteralPath $audio) -or (Get-Item -LiteralPath $audio).Length -eq 0) {
    throw 'FFmpeg video-to-MP3 conversion failed.'
}
Write-Output 'Package checks and video-to-MP3 conversion passed.'

