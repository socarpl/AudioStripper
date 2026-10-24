using System.Text.Json;
using System.Diagnostics;
using System.ComponentModel;

namespace AudioStripper
{
    public partial class Form1 : Form
    {
        private static readonly string ConfigPath = Path.Combine(AppContext.BaseDirectory, "config.json");

        private bool _automaticOutput;
        private string _manualOutputPath = string.Empty;
        private Process? _ffmpegProcess;
        private bool _running;
        private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm", ".m4v",
            ".mpeg", ".mpg", ".ts", ".mts", ".m2ts", ".3gp", ".3g2", ".vob", ".ogv", ".mxf", ".asf"
        };

        public Form1()
        {
            InitializeComponent();
            Icon = AppIcon.Value;
            _button_FFMPEGFolder.Click += (_, _) => OpenContainingFolder(FfmpegPath.Resolve(_text_ffmpeglocation.Text), true);
            _button_InputVideoFolder.Click += (_, _) => OpenContainingFolder(_text_InputVideoFilePath.Text, true);
            _button_OutputAudioFolder.Click += (_, _) => OpenContainingFolder(_text_OutputAudioFIle.Text, false);
            _text_ffmpegParameters.TextChanged += OutputSettingsChanged;
            _text_InputVideoFilePath.TextChanged += OutputSettingsChanged;
            LoadConfig();
            UpdateOutputField();
        }

        private void OpenFfmpegWebsite(object? sender, EventArgs e)
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo("http://ffmpeg.org")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
            {
                MessageBox.Show(this, $"Could not open the FFmpeg website: {ex.Message}",
                    "Cannot open browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static string? GetContainingFolder(string filePath, bool requireExistingFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath)
                    || filePath == "-- Path selected automatically --"
                    || Path.EndsInDirectorySeparator(filePath))
                    return null;

                var fullPath = Path.GetFullPath(filePath);
                if (Directory.Exists(fullPath)
                    || Path.GetFileName(fullPath).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                    || (requireExistingFile && !File.Exists(fullPath)))
                    return null;

                var folder = Path.GetDirectoryName(fullPath);
                return Directory.Exists(folder) ? folder : null;
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or IOException
                or UnauthorizedAccessException)
            {
                return null;
            }
        }

        private void OpenContainingFolder(string filePath, bool requireExistingFile)
        {
            var folder = GetContainingFolder(filePath, requireExistingFile);
            if (folder == null)
            {
                MessageBox.Show(this, "Provide a valid file path with an existing containing folder first.",
                    "Cannot open folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Launch the folder through Windows, without a shell command string.
                using var process = Process.Start(new ProcessStartInfo(folder) { UseShellExecute = true });
            }
            catch (Exception ex) when (ex is Win32Exception or InvalidOperationException or IOException)
            {
                MessageBox.Show(this, $"Could not open folder: {ex.Message}", "Cannot open folder",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OutputSettingsChanged(object? sender, EventArgs e)
        {
            UpdateOutputField();
        }

        private void UpdateOutputField()
        {
            bool automatic = _text_ffmpegParameters.Text.Contains("{o:a}", StringComparison.Ordinal);
            if (automatic && !_automaticOutput)
                _manualOutputPath = _text_OutputAudioFIle.Text;

            _text_OutputAudioFIle.ReadOnly = automatic;
            _text_OutputAudioFIle.BackColor = automatic ? SystemColors.Control : SystemColors.Window;
            _text_OutputAudioFIle.ForeColor = automatic ? SystemColors.GrayText : SystemColors.WindowText;
            _button_AudioOutputFilePath.Enabled = !automatic;

            if (automatic)
            {
                var outputPath = GetAutomaticOutputPath(_text_InputVideoFilePath.Text);
                _text_OutputAudioFIle.Text = string.IsNullOrEmpty(outputPath)
                    ? "-- Path selected automatically --"
                    : outputPath;
            }
            else if (_automaticOutput)
            {
                _text_OutputAudioFIle.Text = _manualOutputPath;
            }

            _automaticOutput = automatic;
        }

        internal static string GetAutomaticOutputPath(string inputPath)
        {
            try
            {
                if (!IsValidVideoFilePath(inputPath))
                    return string.Empty;

                var fullInputPath = Path.GetFullPath(inputPath);
                if (Directory.Exists(fullInputPath)
                    || Path.GetFileName(fullInputPath).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    return string.Empty;

                var directory = Path.GetDirectoryName(fullInputPath)!;
                var name = Path.GetFileNameWithoutExtension(fullInputPath);
                var outputPath = Path.Combine(directory, name + ".mp3");
                int suffix = 1;

                // Also avoid using the input itself when it is already an MP3.
                while (File.Exists(outputPath) || Directory.Exists(outputPath)
                    || string.Equals(outputPath, fullInputPath, StringComparison.OrdinalIgnoreCase))
                {
                    outputPath = Path.Combine(directory, $"{name}-{suffix++}.mp3");
                }

                return outputPath;
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                // Input may be incomplete while the user is typing a path.
                return string.Empty;
            }
        }

        internal static bool IsValidVideoFilePath(string path)
        {
            return File.Exists(path) && VideoExtensions.Contains(Path.GetExtension(path));
        }

        private void SelectInputVideoFile(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select input video file",
                Filter = $"Video files|{string.Join(";", VideoExtensions.Select(extension => "*" + extension))}",
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };

            if (IsValidVideoFilePath(_text_InputVideoFilePath.Text))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(_text_InputVideoFilePath.Text));
                dialog.FileName = Path.GetFileName(_text_InputVideoFilePath.Text);
            }

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (!IsValidVideoFilePath(dialog.FileName))
            {
                MessageBox.Show(this, "Please select an existing video file with a supported video extension.",
                    "Invalid video file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TextChanged applies the same output naming rules as a typed path.
            _text_InputVideoFilePath.Text = dialog.FileName;
        }

        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return;

                var config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(ConfigPath));
                _text_ffmpeglocation.Text = config?.FfmpegLocation ?? string.Empty;
                _text_ffmpegParameters.Text = config?.FfmpegParameters ?? string.Empty;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
            {
                _text_Console.AppendText($"Could not load config: {ex.Message}{Environment.NewLine}");
            }
        }

        private void SelectFfmpeg(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select ffmpeg.exe",
                Filter = "FFmpeg executable (ffmpeg.exe)|ffmpeg.exe",
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };

            var currentPath = FfmpegPath.Resolve(_text_ffmpeglocation.Text);
            if (File.Exists(currentPath))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(currentPath);
                dialog.FileName = Path.GetFileName(currentPath);
            }

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            // A filename can still be typed manually despite the dialog's filter.
            if (!string.Equals(Path.GetFileName(dialog.FileName), "ffmpeg.exe", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Please select a file named ffmpeg.exe.", "Invalid selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var storedPath = FfmpegPath.ForConfig(dialog.FileName);
                SaveConfig(storedPath, _text_ffmpegParameters.Text);
                _text_ffmpeglocation.Text = storedPath;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"Could not save config: {ex.Message}", "Configuration error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void SaveConfig(string location, string parameters)
        {
            var config = new AppConfig
            {
                FfmpegLocation = location,
                FfmpegParameters = parameters
            };
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            var temporaryPath = ConfigPath + ".tmp";
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(config,
                new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temporaryPath, ConfigPath, overwrite: true);
        }

        private void EditFfmpegParameters(object? sender, EventArgs e)
        {
            using var editor = new FFmpegArgumentEditor(_text_ffmpegParameters.Text, parameters =>
            {
                try
                {
                    SaveConfig(_text_ffmpeglocation.Text, parameters);
                    _text_ffmpegParameters.Text = parameters;
                    return true;
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    MessageBox.Show(Form.ActiveForm ?? this, $"Could not save config: {ex.Message}",
                        "Configuration error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            });
            editor.ShowDialog(this);
        }

        private sealed class AppConfig
        {
            public string FfmpegLocation { get; set; } = string.Empty;
            public string FfmpegParameters { get; set; } = string.Empty;
        }

        private async void _button_RUN_Click(object sender, EventArgs e)
        {
            if (_running) return;

            try
            {
                var executable = FfmpegPath.Resolve(_text_ffmpeglocation.Text);
                var parameters = _text_ffmpegParameters.Text;
                var input = _text_InputVideoFilePath.Text;
                if (!File.Exists(executable)
                    || !string.Equals(Path.GetFileName(executable), "ffmpeg.exe", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Select an existing ffmpeg.exe first.");
                if (!IsValidVideoFilePath(input))
                    throw new ArgumentException("Select an existing video file first.");
                if (!FFmpegArgumentEditor.HasRequiredMacros(parameters))
                    throw new ArgumentException("Parameters must contain {i} and either {o} or {o:a}.");

                // Recheck collisions immediately before launching.
                UpdateOutputField();
                if (string.IsNullOrWhiteSpace(_text_OutputAudioFIle.Text))
                    throw new ArgumentException("Provide an output audio file path.");
                input = Path.GetFullPath(input);
                var output = Path.GetFullPath(_text_OutputAudioFIle.Text);
                if (string.Equals(input, output, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Input and output must be different files.");
                if (Directory.Exists(output) || !Directory.Exists(Path.GetDirectoryName(output)))
                    throw new ArgumentException("The output path must name a file in an existing folder.");

                var startInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                };
                foreach (var argument in FfmpegCommand.BuildArguments(parameters, input, output))
                    startInfo.ArgumentList.Add(argument);

                _running = true;
                SetRunControlsEnabled(false);
                _text_Console.Clear();
                AppendConsole($"Starting FFmpeg...{Environment.NewLine}");
                AppendConsole($"Command: {FfmpegCommand.FormatCommand(startInfo.FileName, startInfo.ArgumentList)}{Environment.NewLine}");

                using var process = new Process { StartInfo = startInfo };
                _ffmpegProcess = process;
                try
                {
                    process.Start();
                    process.StandardInput.Close();
                    await Task.WhenAll(
                        CopyToConsoleAsync(process.StandardOutput),
                        CopyToConsoleAsync(process.StandardError),
                        process.WaitForExitAsync());
                    AppendConsole($"{Environment.NewLine}FFmpeg exited with code {process.ExitCode}." + Environment.NewLine);
                }
                finally
                {
                    try
                    {
                        if (!process.HasExited) process.Kill(entireProcessTree: true);
                    }
                    catch (InvalidOperationException) { }
                    finally { _ffmpegProcess = null; }
                }
            }
            catch (Exception ex) when (ex is ArgumentException or IOException or UnauthorizedAccessException
                or Win32Exception or InvalidOperationException or NotSupportedException)
            {
                AppendConsole($"{Environment.NewLine}Error: {ex.Message}{Environment.NewLine}");
            }
            finally
            {
                _running = false;
                if (!IsDisposed && !Disposing) SetRunControlsEnabled(true);
            }
        }

        private async Task CopyToConsoleAsync(StreamReader reader)
        {
            var buffer = new char[4096];
            int count;
            while ((count = await reader.ReadAsync(buffer.AsMemory())) > 0)
                AppendConsole(new string(buffer, 0, count));
        }

        private void AppendConsole(string text)
        {
            // Async continuations resume on the WinForms UI thread.
            if (IsDisposed || Disposing) return;
            _text_Console.AppendText(text);
            _text_Console.SelectionStart = _text_Console.TextLength;
            _text_Console.ScrollToCaret();
        }

        private void SetRunControlsEnabled(bool enabled)
        {
            _button_RUN.Enabled = enabled;
            _button_FFMPEGLocationSelect.Enabled = enabled;
            _button_FFMPEGParameters.Enabled = enabled;
            _button_InputVideoFileSelect.Enabled = enabled;
            _text_InputVideoFilePath.Enabled = enabled;
            _text_OutputAudioFIle.Enabled = enabled;
            _button_AudioOutputFilePath.Enabled = enabled && !_automaticOutput;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.Cancel || _ffmpegProcess == null) return;
            try
            {
                if (!_ffmpegProcess.HasExited) _ffmpegProcess.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException) { }
            catch (Win32Exception ex)
            {
                e.Cancel = true;
                AppendConsole($"Could not stop FFmpeg: {ex.Message}{Environment.NewLine}");
            }
        }
    }
}
