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
Windows x64 package using the installed .NET 9 Windows Desktop Runtime (x64).
The .NET runtime libraries are excluded; FFmpeg remains included.

The job checks required package files and performs a real video-to-MP3 encoding
before uploading an artifact. In GitHub, open **Actions > Build AudioStripper >
the successful run > Artifacts** and download `AudioStripper-win-x64-<run number>`.
The artifact contains `AudioStripper-win-x64.zip`. Extract the application ZIP
and run `AudioStripper.exe` on a machine with the .NET 9 Windows Desktop Runtime
(x64). Artifacts are retained for 14 days.

To reproduce the packaging steps locally:

```powershell
dotnet restore AudioStripper.csproj --runtime win-x64
dotnet publish AudioStripper.csproj --configuration Release --runtime win-x64 --self-contained false --no-restore --output artifacts/AudioStripper-win-x64-framework-dependent
./build/Test-Package.ps1 -OutputDirectory ./artifacts/AudioStripper-win-x64-framework-dependent
Compress-Archive -Path ./artifacts/AudioStripper-win-x64-framework-dependent/* -DestinationPath ./artifacts/AudioStripper-win-x64.zip -CompressionLevel Optimal
```

Build and publish both provision missing FFmpeg/config files. Existing files are
preserved. Use a fresh publish folder when switching from self-contained output
so old runtime DLLs cannot remain in the package. Commit the workflow, project,
`build` scripts/notices, icon, sources, and default `config.json` to the repository.
No custom repository secrets are required.

## Publish a GitHub Release

After committing and pushing the updated workflow and sources, push a version tag:

```powershell
git tag v1.0.0
git push origin v1.0.0
```

Tags beginning with `v` trigger a release job after the build and package checks
succeed. That job downloads the tested ZIP, creates a GitHub Release for the
existing tag, and attaches `AudioStripper-win-x64.zip` under Assets. A rerun
updates the ZIP for that same release. Ordinary branch pushes, pull requests,
and manual workflow runs produce Actions artifacts without creating releases.
Only the release job receives `contents: write`; the build job stays read-only.

Use the repository's **Releases** link to download published versions. GitHub
Packages is not used for these desktop ZIP downloads. Releases and their assets
are separate from the 14-day Actions artifact retention.

If CI reports that `build/Ensure-Ffmpeg.ps1` does not exist, the build-support
files are missing from the GitHub checkout. Check the files committed to the
branch being built. Some Visual Studio ignore templates exclude directories
named `build`; this project's `.gitignore` explicitly includes the source
scripts. From your Git checkout, include them with `git add -f build/` and commit
and push them along with the workflow, project file, and `.gitignore`.
The workflow checks required files immediately after checkout so an incomplete
commit fails with a direct explanation before restoring or publishing.
