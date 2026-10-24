namespace AudioStripper
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            _label_InputVideoFilePath = new Label();
            label4 = new Label();
            _text_ffmpeglocation = new TextBox();
            _text_ffmpegParameters = new TextBox();
            _text_InputVideoFilePath = new TextBox();
            _text_OutputAudioFIle = new TextBox();
            _button_FFMPEGLocationSelect = new Button();
            _button_FFMPEGParameters = new Button();
            _button_AudioOutputFilePath = new Button();
            _button_InputVideoFileSelect = new Button();
            _button_FFMPEGFolder = new Button();
            _button_InputVideoFolder = new Button();
            _button_OutputAudioFolder = new Button();
            _gb_Console = new GroupBox();
            _text_Console = new TextBox();
            _button_RUN = new Button();
            _statusBar = new StatusStrip();
            _link_FfmpegLicense = new ToolStripStatusLabel();
            _gb_Console.SuspendLayout();
            _statusBar.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 36);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 0;
            label1.Text = "ffmpeg location";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 67);
            label2.Name = "label2";
            label2.Size = new Size(108, 15);
            label2.TabIndex = 1;
            label2.Text = "ffmpeg parameters";
            // 
            // _label_InputVideoFilePath
            // 
            _label_InputVideoFilePath.AutoSize = true;
            _label_InputVideoFilePath.Location = new Point(22, 100);
            _label_InputVideoFilePath.Name = "_label_InputVideoFilePath";
            _label_InputVideoFilePath.Size = new Size(89, 15);
            _label_InputVideoFilePath.TabIndex = 2;
            _label_InputVideoFilePath.Text = "Input Video File";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 129);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 3;
            label4.Text = "Output File";
            // 
            // _text_ffmpeglocation
            // 
            _text_ffmpeglocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _text_ffmpeglocation.Location = new Point(136, 31);
            _text_ffmpeglocation.Name = "_text_ffmpeglocation";
            _text_ffmpeglocation.ReadOnly = true;
            _text_ffmpeglocation.Size = new Size(475, 23);
            _text_ffmpeglocation.TabIndex = 4;
            // 
            // _text_ffmpegParameters
            // 
            _text_ffmpegParameters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _text_ffmpegParameters.Location = new Point(136, 64);
            _text_ffmpegParameters.Name = "_text_ffmpegParameters";
            _text_ffmpegParameters.ReadOnly = true;
            _text_ffmpegParameters.Size = new Size(475, 23);
            _text_ffmpegParameters.TabIndex = 5;
            // 
            // _text_InputVideoFilePath
            // 
            _text_InputVideoFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _text_InputVideoFilePath.Location = new Point(136, 93);
            _text_InputVideoFilePath.Name = "_text_InputVideoFilePath";
            _text_InputVideoFilePath.Size = new Size(475, 23);
            _text_InputVideoFilePath.TabIndex = 6;
            // 
            // _text_OutputAudioFIle
            // 
            _text_OutputAudioFIle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _text_OutputAudioFIle.Location = new Point(136, 126);
            _text_OutputAudioFIle.Name = "_text_OutputAudioFIle";
            _text_OutputAudioFIle.Size = new Size(475, 23);
            _text_OutputAudioFIle.TabIndex = 7;
            // 
            // _button_FFMPEGLocationSelect
            // 
            _button_FFMPEGLocationSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_FFMPEGLocationSelect.Location = new Point(617, 30);
            _button_FFMPEGLocationSelect.Name = "_button_FFMPEGLocationSelect";
            _button_FFMPEGLocationSelect.Size = new Size(41, 23);
            _button_FFMPEGLocationSelect.TabIndex = 8;
            _button_FFMPEGLocationSelect.Text = "...";
            _button_FFMPEGLocationSelect.UseVisualStyleBackColor = true;
            _button_FFMPEGLocationSelect.Click += SelectFfmpeg;
            // 
            // _button_FFMPEGParameters
            // 
            _button_FFMPEGParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_FFMPEGParameters.Location = new Point(617, 64);
            _button_FFMPEGParameters.Name = "_button_FFMPEGParameters";
            _button_FFMPEGParameters.Size = new Size(41, 23);
            _button_FFMPEGParameters.TabIndex = 13;
            _button_FFMPEGParameters.Text = "...";
            _button_FFMPEGParameters.UseVisualStyleBackColor = true;
            _button_FFMPEGParameters.Click += EditFfmpegParameters;
            // 
            // _button_AudioOutputFilePath
            // 
            _button_AudioOutputFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_AudioOutputFilePath.Location = new Point(617, 125);
            _button_AudioOutputFilePath.Name = "_button_AudioOutputFilePath";
            _button_AudioOutputFilePath.Size = new Size(41, 23);
            _button_AudioOutputFilePath.TabIndex = 9;
            _button_AudioOutputFilePath.Text = "...";
            _button_AudioOutputFilePath.UseVisualStyleBackColor = true;
            // 
            // _button_InputVideoFileSelect
            // 
            _button_InputVideoFileSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_InputVideoFileSelect.Location = new Point(617, 92);
            _button_InputVideoFileSelect.Name = "_button_InputVideoFileSelect";
            _button_InputVideoFileSelect.Size = new Size(41, 23);
            _button_InputVideoFileSelect.TabIndex = 10;
            _button_InputVideoFileSelect.Text = "...";
            _button_InputVideoFileSelect.UseVisualStyleBackColor = true;
            _button_InputVideoFileSelect.Click += SelectInputVideoFile;
            // 
            // _button_FFMPEGFolder
            // 
            _button_FFMPEGFolder.AccessibleName = "Open FFmpeg folder";
            _button_FFMPEGFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_FFMPEGFolder.Location = new Point(664, 30);
            _button_FFMPEGFolder.Name = "_button_FFMPEGFolder";
            _button_FFMPEGFolder.Size = new Size(41, 23);
            _button_FFMPEGFolder.TabIndex = 14;
            _button_FFMPEGFolder.Text = "F";
            _button_FFMPEGFolder.UseVisualStyleBackColor = true;
            // 
            // _button_InputVideoFolder
            // 
            _button_InputVideoFolder.AccessibleName = "Open input video folder";
            _button_InputVideoFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_InputVideoFolder.Location = new Point(664, 92);
            _button_InputVideoFolder.Name = "_button_InputVideoFolder";
            _button_InputVideoFolder.Size = new Size(41, 23);
            _button_InputVideoFolder.TabIndex = 15;
            _button_InputVideoFolder.Text = "F";
            _button_InputVideoFolder.UseVisualStyleBackColor = true;
            // 
            // _button_OutputAudioFolder
            // 
            _button_OutputAudioFolder.AccessibleName = "Open output audio folder";
            _button_OutputAudioFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _button_OutputAudioFolder.Location = new Point(664, 125);
            _button_OutputAudioFolder.Name = "_button_OutputAudioFolder";
            _button_OutputAudioFolder.Size = new Size(41, 23);
            _button_OutputAudioFolder.TabIndex = 16;
            _button_OutputAudioFolder.Text = "F";
            _button_OutputAudioFolder.UseVisualStyleBackColor = true;
            // 
            // _gb_Console
            // 
            _gb_Console.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gb_Console.Controls.Add(_text_Console);
            _gb_Console.Location = new Point(22, 222);
            _gb_Console.Name = "_gb_Console";
            _gb_Console.Size = new Size(683, 218);
            _gb_Console.TabIndex = 11;
            _gb_Console.TabStop = false;
            _gb_Console.Text = "Console";
            // 
            // _text_Console
            // 
            _text_Console.BackColor = Color.Black;
            _text_Console.BorderStyle = BorderStyle.FixedSingle;
            _text_Console.Dock = DockStyle.Fill;
            _text_Console.Font = new Font("Consolas", 10F);
            _text_Console.ForeColor = Color.Lime;
            _text_Console.Location = new Point(3, 19);
            _text_Console.Multiline = true;
            _text_Console.Name = "_text_Console";
            _text_Console.ReadOnly = true;
            _text_Console.ScrollBars = ScrollBars.Both;
            _text_Console.Size = new Size(677, 196);
            _text_Console.TabIndex = 0;
            _text_Console.WordWrap = false;
            // 
            // _button_RUN
            // 
            _button_RUN.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _button_RUN.Location = new Point(136, 164);
            _button_RUN.Name = "_button_RUN";
            _button_RUN.Size = new Size(475, 52);
            _button_RUN.TabIndex = 12;
            _button_RUN.Text = "Execute";
            _button_RUN.UseVisualStyleBackColor = true;
            _button_RUN.Click += _button_RUN_Click;
            // 
            // _statusBar
            // 
            _statusBar.Items.AddRange(new ToolStripItem[] { _link_FfmpegLicense });
            _statusBar.Location = new Point(0, 452);
            _statusBar.Name = "_statusBar";
            _statusBar.Size = new Size(729, 22);
            _statusBar.TabIndex = 17;
            // 
            // _link_FfmpegLicense
            // 
            _link_FfmpegLicense.ActiveLinkColor = Color.Blue;
            _link_FfmpegLicense.IsLink = true;
            _link_FfmpegLicense.LinkColor = Color.Blue;
            _link_FfmpegLicense.Name = "_link_FfmpegLicense";
            _link_FfmpegLicense.Size = new Size(107, 17);
            _link_FfmpegLicense.Text = "FFMPEG - LGPL 3.0";
            _link_FfmpegLicense.ToolTipText = "http://ffmpeg.org";
            _link_FfmpegLicense.VisitedLinkColor = Color.Blue;
            _link_FfmpegLicense.Click += OpenFfmpegWebsite;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(729, 474);
            Controls.Add(_statusBar);
            Controls.Add(_button_FFMPEGFolder);
            Controls.Add(_button_InputVideoFolder);
            Controls.Add(_button_OutputAudioFolder);
            Controls.Add(_button_RUN);
            Controls.Add(_gb_Console);
            Controls.Add(_button_InputVideoFileSelect);
            Controls.Add(_button_AudioOutputFilePath);
            Controls.Add(_button_FFMPEGLocationSelect);
            Controls.Add(_button_FFMPEGParameters);
            Controls.Add(_text_OutputAudioFIle);
            Controls.Add(_text_InputVideoFilePath);
            Controls.Add(_text_ffmpegParameters);
            Controls.Add(_text_ffmpeglocation);
            Controls.Add(label4);
            Controls.Add(_label_InputVideoFilePath);
            Controls.Add(label2);
            Controls.Add(label1);
            MinimumSize = new Size(650, 430);
            Name = "Form1";
            Text = "Audio Stripper";
            
            _gb_Console.ResumeLayout(false);
            _gb_Console.PerformLayout();
            _statusBar.ResumeLayout(false);
            _statusBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label _label_InputVideoFilePath;
        private Label label4;
        private TextBox _text_ffmpeglocation;
        private TextBox _text_ffmpegParameters;
        private TextBox _text_InputVideoFilePath;
        private TextBox _text_OutputAudioFIle;
        private Button _button_FFMPEGLocationSelect;
        private Button _button_FFMPEGParameters;
        private Button _button_AudioOutputFilePath;
        private Button _button_InputVideoFileSelect;
        private Button _button_FFMPEGFolder;
        private Button _button_InputVideoFolder;
        private Button _button_OutputAudioFolder;
        private GroupBox _gb_Console;
        private TextBox _text_Console;
        private Button _button_RUN;
        private StatusStrip _statusBar;
        private ToolStripStatusLabel _link_FfmpegLicense;
    }
}
