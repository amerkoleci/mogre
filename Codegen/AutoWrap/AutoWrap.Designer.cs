namespace AutoWrap
{
    partial class AutoWrap
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoWrap));
            this._inputFilesList = new System.Windows.Forms.ListBox();
            this._sourceCodeField = new System.Windows.Forms.TextBox();
            this._generateButton = new System.Windows.Forms.Button();
            this._showToggleButton = new System.Windows.Forms.Button();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this._inputFilesList);
            groupBox1.Location = new System.Drawing.Point(9, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(264, 528);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Input Header Files";
            // 
            // _inputFilesList
            // 
            this._inputFilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._inputFilesList.FormattingEnabled = true;
            this._inputFilesList.Location = new System.Drawing.Point(16, 24);
            this._inputFilesList.Margin = new System.Windows.Forms.Padding(2);
            this._inputFilesList.Name = "_inputFilesList";
            this._inputFilesList.Size = new System.Drawing.Size(233, 485);
            this._inputFilesList.Sorted = true;
            this._inputFilesList.TabIndex = 1;
            this.tooltip.SetToolTip(this._inputFilesList, "Generate C++/CLI source code");
            this._inputFilesList.SelectedIndexChanged += new System.EventHandler(this.lstTypes_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            groupBox2.Controls.Add(this._sourceCodeField);
            groupBox2.Location = new System.Drawing.Point(279, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(609, 560);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Final Source Code";
            // 
            // _sourceCodeField
            // 
            this._sourceCodeField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._sourceCodeField.Location = new System.Drawing.Point(15, 24);
            this._sourceCodeField.Margin = new System.Windows.Forms.Padding(2);
            this._sourceCodeField.Multiline = true;
            this._sourceCodeField.Name = "_sourceCodeField";
            this._sourceCodeField.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._sourceCodeField.Size = new System.Drawing.Size(578, 520);
            this._sourceCodeField.TabIndex = 3;
            // 
            // _generateButton
            // 
            this._generateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this._generateButton.Location = new System.Drawing.Point(9, 545);
            this._generateButton.Margin = new System.Windows.Forms.Padding(2);
            this._generateButton.Name = "_generateButton";
            this._generateButton.Size = new System.Drawing.Size(67, 27);
            this._generateButton.TabIndex = 0;
            this._generateButton.Text = "Generate";
            this.tooltip.SetToolTip(this._generateButton, "Generate C++/CLI files");
            this._generateButton.UseVisualStyleBackColor = true;
            this._generateButton.Click += new System.EventHandler(this.GenerateButtonClicked);
            // 
            // _showToggleButton
            // 
            this._showToggleButton.Location = new System.Drawing.Point(147, 545);
            this._showToggleButton.Margin = new System.Windows.Forms.Padding(2);
            this._showToggleButton.Name = "_showToggleButton";
            this._showToggleButton.Size = new System.Drawing.Size(127, 27);
            this._showToggleButton.TabIndex = 2;
            this._showToggleButton.Text = "Show CPP File";
            this.tooltip.SetToolTip(this._showToggleButton, "Toggle between .h and .cpp files.");
            this._showToggleButton.UseVisualStyleBackColor = true;
            this._showToggleButton.Click += new System.EventHandler(this.ToggleButtonClicked);
            // 
            // bar
            // 
            this.bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.bar.Location = new System.Drawing.Point(11, 591);
            this.bar.Margin = new System.Windows.Forms.Padding(2);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(877, 19);
            this.bar.TabIndex = 5;
            this.bar.Visible = false;
            // 
            // tooltip
            // 
            this.tooltip.ShowAlways = true;
            // 
            // AutoWrap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 625);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.bar);
            this.Controls.Add(this._showToggleButton);
            this.Controls.Add(this._generateButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AutoWrap";
            this.Text = "AutoWrap";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _generateButton;
        private System.Windows.Forms.ListBox _inputFilesList;
        private System.Windows.Forms.Button _showToggleButton;
        private System.Windows.Forms.TextBox _sourceCodeField;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.ToolTip tooltip;
    }
}

