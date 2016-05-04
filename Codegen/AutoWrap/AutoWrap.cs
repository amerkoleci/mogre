using System;
using System.Windows.Forms;
using AutoWrap.Meta;

namespace AutoWrap
{
    public partial class AutoWrap : Form
    {
        private readonly Wrapper _wrapper;
        private bool _showHeaderFiles = true;

        public AutoWrap(Wrapper wrapper)
        {
            InitializeComponent();

            _wrapper = wrapper;
            wrapper.IncludeFileWrapped += (s, e) =>
                                              {
                                                  // Called when an include files has been wrapped (update progress).
                                                  bar.Value++;
                                                  bar.Refresh();
                                              };

            for (int i = 0; i < _wrapper.IncludeFiles.Count; i++)
            {
                _inputFilesList.Items.Add(_wrapper.IncludeFiles.Keys[i]);
            }
        }

        private void GenerateButtonClicked(object sender, EventArgs e)
        {
            _generateButton.Enabled = false;
            _showToggleButton.Enabled = false;
            bar.Visible = true;
            bar.Minimum = 0;
            bar.Maximum = _wrapper.IncludeFiles.Count;
            bar.Step = 1;
            bar.Value = 0;

            // Generate the C++/CLI source files
            _wrapper.GenerateCodeFiles();
            if (MessageBox.Show(this, "The source files were generated sucessfully.\nDo you want to quit now?", "Generation complete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
                return;
            }

            bar.Visible = false;
            _generateButton.Enabled = true;
            _showToggleButton.Enabled = true;
        }

        private void ToggleButtonClicked(object sender, EventArgs e)
        {
            _showHeaderFiles = !_showHeaderFiles;
            if (_showHeaderFiles)
                _showToggleButton.Text = "Show CPP File";
            else
                _showToggleButton.Text = "Show Header File";
            ShowCurrentFile();
        }

        private void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCurrentFile();
        }

        private void ShowCurrentFile()
        {
            if (_inputFilesList.SelectedItem == null)
                return;

            if (_showHeaderFiles)
                _sourceCodeField.Text = _wrapper.GenerateIncludeFileCodeForIncludeFile(_inputFilesList.SelectedItem.ToString());
            else
                _sourceCodeField.Text = _wrapper.GenerateCppFileCodeForIncludeFile(_inputFilesList.SelectedItem.ToString());
        }
    }
}