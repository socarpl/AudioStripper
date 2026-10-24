# FFmpeg build dependency

A normal Windows build runs `Ensure-Ffmpeg.ps1` if the output folder does not
contain `ffmpeg.exe`. The script downloads the pinned BtbN Windows x64 static
LGPL build, verifies its SHA-256, and installs the executable and license notices
beside AudioStripper. The default config already uses `.\ffmpeg.exe`.

The first build needs internet access. Subsequent builds reuse the verified ZIP
under `obj/ffmpeg`; an existing output executable is always left untouched.
A failed download or checksum mismatch fails the build instead of installing an
unverified file. Design-time builds do not download anything.

The pinned release URL, archive filename, and expected SHA-256 are in
`Ensure-Ffmpeg.ps1`. Update all three together when changing versions. BtbN's
monthly release retention is two years, so the pin must be maintained; the
verified local cache also permits offline builds.

The downloaded build reports LGPL version 3 or later and supports `libmp3lame`.
Its notices accompany the binary. If distributing the output, also satisfy the
corresponding-source requirements for FFmpeg and its included dependencies;
the provenance links in `FFmpeg-NOTICE.txt` alone do not satisfy that requirement.

# GitHub Actions

`.github/workflows/build.yml` builds on every push and pull request. It can also
be started manually from the repository's Actions tab. It uses a Windows runner
and the .NET 9 SDK, caches the verified FFmpeg download, and publishes a Release
Windows x64 package with the .NET runtime included.

The job checks required package files and performs a real video-to-MP3 encoding
before uploading an artifact. In GitHub, open **Actions > Build AudioStripper >
the successful run > Artifacts** and download `AudioStripper-win-x64-<run number>`.
Extract the entire download and run `AudioStripper.exe`; a separate .NET runtime
installation is not required. Artifacts are retained for 14 days.

To reproduce the packaging steps locally:

```powershell
dotnet restore AudioStripper.csproj --runtime win-x64
dotnet publish AudioStripper.csproj --configuration Release --runtime win-x64 --self-contained true --no-restore --output artifacts/AudioStripper-win-x64
./build/Test-Package.ps1 -OutputDirectory ./artifacts/AudioStripper-win-x64
```

Build and publish both provision missing FFmpeg/config files. Existing files are
preserved. Actions only uploads a build artifact; it does not create a GitHub
Release. Commit the workflow, project, `build` scripts/notices, icon, sources, and
default `config.json` to the repository. No repository secrets are required.

If CI reports that `build/Ensure-Ffmpeg.ps1` does not exist, the build-support
files are missing from the GitHub checkout. Check the files committed to the
branch being built. Some Visual Studio ignore templates exclude directories
named `build`; this project's `.gitignore` explicitly includes the source
scripts. From your Git checkout, include them with `git add -f build/` and commit
and push them along with the workflow, project file, and `.gitignore`.
The workflow checks required files immediately after checkout so an incomplete
commit fails with a direct explanation before restoring or publishing.
