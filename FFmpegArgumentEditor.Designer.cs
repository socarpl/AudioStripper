namespace AudioStripper
{
    partial class FFmpegArgumentEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFmpegArgumentEditor));
            label1 = new Label();
            _text_Parameter = new TextBox();
            label2 = new Label();
            _button_save = new Button();
            _button_cancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 22);
            label1.Name = "label1";
            label1.Size = new Size(348, 75);
            label1.TabIndex = 0;
            label1.Text = resources.GetString("label1.Text");
            // 
            // _text_Parameter
            // 
            _text_Parameter.Location = new Point(124, 119);
            _text_Parameter.Name = "_text_Parameter";
            _text_Parameter.Size = new Size(381, 23);
            _text_Parameter.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 122);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 2;
            label2.Text = "ffmpeg parameter:";
            // 
            // _button_save
            // 
            _button_save.Location = new Point(430, 162);
            _button_save.Name = "_button_save";
            _button_save.Size = new Size(75, 23);
            _button_save.TabIndex = 3;
            _button_save.Text = "Save";
            _button_save.UseVisualStyleBackColor = true;
            _button_save.Click += SaveParameters;
            // 
            // _button_cancel
            // 
            _button_cancel.Location = new Point(349, 162);
            _button_cancel.Name = "_button_cancel";
            _button_cancel.Size = new Size(75, 23);
            _button_cancel.TabIndex = 4;
            _button_cancel.Text = "Cancel";
            _button_cancel.UseVisualStyleBackColor = true;
            _button_cancel.DialogResult = DialogResult.Cancel;
            // 
            // FFmpegArgumentEditor
            // 
            AcceptButton = _button_save;
            CancelButton = _button_cancel;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 197);
            Controls.Add(_button_cancel);
            Controls.Add(_button_save);
            Controls.Add(label2);
            Controls.Add(_text_Parameter);
            Controls.Add(label1);
            Name = "FFmpegArgumentEditor";
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            Text = "FFmpegArgumentEditor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox _text_Parameter;
        private Label label2;
        private Button _button_save;
        private Button _button_cancel;
    }
}
