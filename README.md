# AudioStripper

AudioStripper is a Windows desktop app for extracting audio from a video using
FFmpeg. Select a video, choose your FFmpeg arguments, and run the conversion
without opening a command prompt. The default settings create an MP3 file beside
the source video.

## Features

- Select a video with the browse button or type its path.
- Edit and save FFmpeg argument templates with input/output macros.
- Generate MP3 output names automatically, adding numbered suffixes when needed.
- Choose a manual output path by using the `{o}` macro.
- View the full command, FFmpeg output, errors, and exit code in the console.
- Open the FFmpeg, input video, or output audio folder with its **F** button.
- Keep FFmpeg settings in `config.json` beside the application.
- Resize the window to expand the path fields and console.

## Get the app

In the GitHub repository, open **Actions > Build AudioStripper**, select a
successful run, and download `AudioStripper-win-x64-<run number>` from **Artifacts**.
Extract the entire archive and run `AudioStripper.exe`.

The GitHub Actions package targets Windows x64 and includes the .NET runtime,
FFmpeg, the default config, and FFmpeg license notices. Keep these files together.
Use a folder where you can save settings and write output files.

## Convert a video to MP3

1. Open `AudioStripper.exe`.
2. Check **ffmpeg location**. The default is `.\ffmpeg.exe`. To use another copy,
   click its **...** button and select `ffmpeg.exe`.
3. Click **...** beside **ffmpeg parameters** to open the editor. The default is:

   ```text
   -i "{i}" -vn -c:a libmp3lame -q:a 2 "{o:a}"
   ```

4. Select a file using **...** beside **Input Video File**, or type its full path.
5. Check the generated **Output File** path.
6. Click **Execute**. The console displays the full command and FFmpeg output.
   Exit code `0` indicates success; a nonzero code indicates an error.
7. Click **F** beside the output field to open its folder.

The default arguments omit video (`-vn`) and encode MP3 using `libmp3lame` with
quality setting `2`. Only one conversion runs at a time. FFmpeg runs without a
visible window, and the app remains responsive. Closing the app stops a running
conversion; an incomplete output file may remain.

## Parameter macros

Enter **arguments only** in the editor, starting with an option such as `-i`.
The application supplies the executable, replaces the macros, and adds
`-nostdin` so FFmpeg does not wait for keyboard input.

| Macro | Replaced with |
| --- | --- |
| `{i}` | The input video file's full path. |
| `{o}` | The output path you type into **Output File**. |
| `{o:a}` | An automatically generated MP3 path beside the input video. |

Every template must contain `{i}` and at least one of `{o}` or `{o:a}`. Macros
are case-sensitive. If a required macro is missing, **Save** explains what is
missing and leaves the editor open. Saved changes update the main window;
**Cancel** discards edits.

Use double quotes around path macros, as shown in the examples. The app handles
paths containing spaces. Do not add `ffmpeg` or `ffmpeg.exe` at the beginning of
the parameter string.

### Automatic output

With `{o:a}`, the output field is gray and read-only. Until an existing file
with a supported video extension is supplied, it displays:

```text
-- Path selected automatically --
```

For input `C:\Videos\clip.mp4`, the app proposes `C:\Videos\clip.mp3`. If that
name is taken, it tries `clip-1.mp3`, `clip-2.mp3`, and so on. Availability is
checked again when you click **Execute**, so the final name may change.

### Manual output

Use `{o}` in place of `{o:a}`:

```text
-i "{i}" -vn -c:a libmp3lame -q:a 2 "{o}"
```

The output field becomes editable. Type the full destination filename, such as
`C:\Audio\clip.mp3`; its parent folder must already exist. Choose an unused
filename to avoid FFmpeg's existing-file error. If both output macros appear,
automatic mode takes precedence, and both receive the same output path.

## Settings and portable paths

Selecting FFmpeg or saving parameters writes `config.json` beside the app.
Settings reload at startup. The FFmpeg location and parameters fields are
read-only on the main window; use their **...** buttons to change them.

FFmpeg selections inside the app's folder are stored relatively:

```text
AudioStripper.exe
ffmpeg.exe                 -> .\ffmpeg.exe
tools\ffmpeg.exe           -> .\tools\ffmpeg.exe
```

These paths resolve against the application folder, so you can move that folder
with its contents. FFmpeg selected outside that folder is saved as a full path.
Input and output paths are selected for each session and are not saved in config.

## Supported input paths

The file picker accepts `.mp4`, `.mkv`, `.avi`, `.mov`, `.wmv`, `.flv`, `.webm`,
`.m4v`, `.mpeg`, `.mpg`, `.ts`, `.mts`, `.m2ts`, `.3gp`, `.3g2`, `.vob`, `.ogv`,
`.mxf`, and `.asf` (case-insensitive).

The app checks that the file exists and has a supported extension. FFmpeg checks
the actual media during conversion. The video must contain an audio stream that
your selected FFmpeg build can decode.

## Build from source

Use Windows and the .NET 9 SDK, or Visual Studio with .NET 9 Windows Forms support.
From the project directory:

```powershell
dotnet build AudioStripper.sln --configuration Release
```

The first build downloads a pinned Windows x64 FFmpeg build from BtbN if
`ffmpeg.exe` is missing from the output folder. The download is SHA-256 verified
and cached under `obj/ffmpeg`. Existing output executables are preserved. The
project's default `config.json` is copied only when the output has no config.

For a standalone package with the .NET runtime included:

```powershell
dotnet publish AudioStripper.csproj --configuration Release --runtime win-x64 --self-contained true --output artifacts/AudioStripper-win-x64
./build/Test-Package.ps1 -OutputDirectory ./artifacts/AudioStripper-win-x64
```

GitHub Actions performs this packaging and conversion check on pushes and pull
requests. See [build instructions](build/README.md) for caching, workflow, and
dependency maintenance details.

## Troubleshooting

| Problem | What to check |
| --- | --- |
| FFmpeg cannot start | Select an existing `ffmpeg.exe`; check the executable path printed in the console. |
| Automatic output still shows the placeholder | Check that the input file exists and has a supported video extension. |
| Parameters cannot be saved | Include `{i}` and either `{o}` or `{o:a}`, using their exact spelling. |
| Conversion fails | Read the FFmpeg error in the console; check for an audio stream, supported codecs, and a writable output folder. |
| Settings cannot be saved | Check write permissions for the application folder and its `config.json`. |
| First build cannot download FFmpeg | Check network access to GitHub, or provide your own `ffmpeg.exe` in the build output folder. |

## FFmpeg notices

FFmpeg is a separate project: [ffmpeg.org](https://ffmpeg.org/). The automatically
downloaded build reports LGPL version 3 or later; its license and provenance
files accompany the executable. A separately supplied build may use a different
license. See [FFmpeg-NOTICE.txt](build/FFmpeg-NOTICE.txt) for build details and
corresponding-source considerations when redistributing FFmpeg.
