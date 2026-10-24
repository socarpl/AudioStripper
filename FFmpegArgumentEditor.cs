using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioStripper
{
    public partial class FFmpegArgumentEditor : Form
    {
        private readonly Func<string, bool>? _saveParameters;

        public FFmpegArgumentEditor()
        {
            InitializeComponent();
            Icon = AppIcon.Value;
        }

        public FFmpegArgumentEditor(string parameters, Func<string, bool> saveParameters) : this()
        {
            _text_Parameter.Text = parameters;
            _saveParameters = saveParameters;
        }

        internal static bool HasRequiredMacros(string parameters)
        {
            return parameters.Contains("{i}", StringComparison.Ordinal)
                && (parameters.Contains("{o}", StringComparison.Ordinal)
                    || parameters.Contains("{o:a}", StringComparison.Ordinal));
        }

        private void SaveParameters(object? sender, EventArgs e)
        {
            if (!HasRequiredMacros(_text_Parameter.Text))
            {
                var missingMacros = new List<string>();
                if (!_text_Parameter.Text.Contains("{i}", StringComparison.Ordinal))
                    missingMacros.Add("Missing input macro: {i}.");
                if (!_text_Parameter.Text.Contains("{o}", StringComparison.Ordinal)
                    && !_text_Parameter.Text.Contains("{o:a}", StringComparison.Ordinal))
                    missingMacros.Add("Missing output macro: {o} or {o:a} (at least one is required).");

                var message = string.Join(Environment.NewLine, missingMacros)
                    + Environment.NewLine + Environment.NewLine
                    + "Parameters will not be saved until the missing macros are added.";
                MessageBox.Show(this, message,
                    "Missing required macros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _text_Parameter.Focus();
                return;
            }

            if (_saveParameters?.Invoke(_text_Parameter.Text) == false)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
