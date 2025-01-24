namespace Doom_Loader
{
    partial class Main
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            pwadManagerButton = new Button();
            portButton = new Button();
            sourcePortDialog = new OpenFileDialog();
            playButton = new Button();
            extraParamsTextBox = new TextBox();
            savePresetButton = new Button();
            savePresetDialog = new SaveFileDialog();
            openPresetDialog = new OpenFileDialog();
            rpcUpdateTimer = new System.Windows.Forms.Timer(components);
            settingsOpen = new Button();
            complevelSelector = new ComboBox();
            iwadBox = new ComboBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            customPresetButton = new Button();
            loadPresetBox = new ComboBox();
            toolTips = new ToolTip(components);
            fileImport = new OpenFileDialog();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // pwadManagerButton
            // 
            pwadManagerButton.BackColor = SystemColors.ControlLight;
            pwadManagerButton.Location = new Point(159, 118);
            pwadManagerButton.Margin = new Padding(4, 3, 4, 3);
            pwadManagerButton.Name = "pwadManagerButton";
            pwadManagerButton.Size = new Size(118, 23);
            pwadManagerButton.TabIndex = 3;
            pwadManagerButton.TabStop = false;
            pwadManagerButton.Text = "External Files";
            pwadManagerButton.UseVisualStyleBackColor = false;
            pwadManagerButton.Click += PWADManagerOpen;
            // 
            // portButton
            // 
            portButton.BackColor = SystemColors.ControlLight;
            portButton.Location = new Point(18, 118);
            portButton.Margin = new Padding(4, 3, 4, 3);
            portButton.Name = "portButton";
            portButton.Size = new Size(129, 23);
            portButton.TabIndex = 4;
            portButton.TabStop = false;
            portButton.Text = "Select Port";
            portButton.UseVisualStyleBackColor = false;
            portButton.Click += SelectPort;
            // 
            // sourcePortDialog
            // 
            sourcePortDialog.Filter = "Sourceport|*.exe";
            sourcePortDialog.Title = "Select Port";
            // 
            // playButton
            // 
            playButton.BackColor = SystemColors.ControlLight;
            playButton.ForeColor = SystemColors.ControlText;
            playButton.Location = new Point(12, 233);
            playButton.Margin = new Padding(4, 3, 4, 3);
            playButton.Name = "playButton";
            playButton.Size = new Size(183, 23);
            playButton.TabIndex = 6;
            playButton.TabStop = false;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = false;
            playButton.Click += Play;
            // 
            // extraParamsTextBox
            // 
            extraParamsTextBox.BackColor = SystemColors.Window;
            extraParamsTextBox.Location = new Point(7, 18);
            extraParamsTextBox.Margin = new Padding(4, 3, 4, 3);
            extraParamsTextBox.Name = "extraParamsTextBox";
            extraParamsTextBox.Size = new Size(253, 23);
            extraParamsTextBox.TabIndex = 11;
            extraParamsTextBox.TabStop = false;
            extraParamsTextBox.TextAlign = HorizontalAlignment.Center;
            extraParamsTextBox.TextChanged += ExtraParamsChanged;
            extraParamsTextBox.MouseDown += ExtraParametersInsertFilePath;
            // 
            // savePresetButton
            // 
            savePresetButton.BackColor = SystemColors.ControlLight;
            savePresetButton.ForeColor = SystemColors.ActiveCaptionText;
            savePresetButton.Location = new Point(69, 49);
            savePresetButton.Margin = new Padding(4, 3, 4, 3);
            savePresetButton.Name = "savePresetButton";
            savePresetButton.Size = new Size(124, 23);
            savePresetButton.TabIndex = 16;
            savePresetButton.TabStop = false;
            savePresetButton.Text = "Save Preset";
            savePresetButton.UseVisualStyleBackColor = false;
            savePresetButton.Click += SavePreset;
            // 
            // savePresetDialog
            // 
            savePresetDialog.Filter = "Minty Launcher Preset|*.mlPreset";
            savePresetDialog.Title = "Save Preset";
            // 
            // openPresetDialog
            // 
            openPresetDialog.Filter = "Minty Launcher Preset|*.mlPreset";
            openPresetDialog.Title = "Load Preset";
            // 
            // rpcUpdateTimer
            // 
            rpcUpdateTimer.Tick += UpdateRPCTimestamp;
            // 
            // settingsOpen
            // 
            settingsOpen.BackColor = SystemColors.ControlLight;
            settingsOpen.Location = new Point(202, 233);
            settingsOpen.Name = "settingsOpen";
            settingsOpen.Size = new Size(75, 23);
            settingsOpen.TabIndex = 18;
            settingsOpen.TabStop = false;
            settingsOpen.Text = "Settings";
            settingsOpen.UseVisualStyleBackColor = false;
            settingsOpen.Click += SettingsMenuOpen;
            // 
            // complevelSelector
            // 
            complevelSelector.BackColor = SystemColors.Control;
            complevelSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            complevelSelector.FormattingEnabled = true;
            complevelSelector.Items.AddRange(new object[] { "None", "Doom v1.9", "Ultimate Doom", "Final Doom", "Boom v2.02", "MBF", "MBF21" });
            complevelSelector.Location = new Point(5, 18);
            complevelSelector.Name = "complevelSelector";
            complevelSelector.Size = new Size(108, 23);
            complevelSelector.TabIndex = 19;
            complevelSelector.TabStop = false;
            complevelSelector.SelectedIndexChanged += ComplevelChanged;
            // 
            // iwadBox
            // 
            iwadBox.BackColor = SystemColors.Control;
            iwadBox.DropDownStyle = ComboBoxStyle.DropDownList;
            iwadBox.FormattingEnabled = true;
            iwadBox.Location = new Point(6, 18);
            iwadBox.Name = "iwadBox";
            iwadBox.Size = new Size(129, 23);
            iwadBox.TabIndex = 20;
            iwadBox.TabStop = false;
            iwadBox.DropDown += RefreshIWAD;
            iwadBox.SelectedIndexChanged += IWADChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(iwadBox);
            groupBox1.ForeColor = SystemColors.ControlText;
            groupBox1.Location = new Point(12, 9);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(141, 48);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "IWAD";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(complevelSelector);
            groupBox2.ForeColor = SystemColors.ControlText;
            groupBox2.Location = new Point(159, 9);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(118, 48);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "Complevel";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(extraParamsTextBox);
            groupBox3.ForeColor = SystemColors.ControlText;
            groupBox3.Location = new Point(12, 63);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(265, 48);
            groupBox3.TabIndex = 23;
            groupBox3.TabStop = false;
            groupBox3.Text = "Extra Parameters";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(customPresetButton);
            groupBox4.Controls.Add(loadPresetBox);
            groupBox4.Controls.Add(savePresetButton);
            groupBox4.ForeColor = SystemColors.ControlText;
            groupBox4.Location = new Point(12, 146);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(265, 76);
            groupBox4.TabIndex = 24;
            groupBox4.TabStop = false;
            groupBox4.Text = "Presets";
            // 
            // customPresetButton
            // 
            customPresetButton.BackColor = SystemColors.ControlLight;
            customPresetButton.Enabled = false;
            customPresetButton.ForeColor = SystemColors.ActiveCaptionText;
            customPresetButton.Location = new Point(69, 20);
            customPresetButton.Name = "customPresetButton";
            customPresetButton.Size = new Size(124, 23);
            customPresetButton.TabIndex = 25;
            customPresetButton.TabStop = false;
            customPresetButton.Text = "Load Preset";
            customPresetButton.UseVisualStyleBackColor = false;
            customPresetButton.Visible = false;
            customPresetButton.Click += LoadCustomPreset;
            // 
            // loadPresetBox
            // 
            loadPresetBox.BackColor = SystemColors.Control;
            loadPresetBox.DropDownStyle = ComboBoxStyle.DropDownList;
            loadPresetBox.FormattingEnabled = true;
            loadPresetBox.Location = new Point(7, 20);
            loadPresetBox.Name = "loadPresetBox";
            loadPresetBox.Size = new Size(252, 23);
            loadPresetBox.TabIndex = 25;
            loadPresetBox.TabStop = false;
            loadPresetBox.DropDown += RefreshPresetBox;
            loadPresetBox.SelectionChangeCommitted += LoadPreset;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(289, 265);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(playButton);
            Controls.Add(settingsOpen);
            Controls.Add(portButton);
            Controls.Add(pwadManagerButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "The Minty Launcher";
            Load += AppDataInit;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button pwadManagerButton;
        private Button portButton;
        private OpenFileDialog sourcePortDialog;
        private Button playButton;
        private TextBox extraParamsTextBox;
        private Button savePresetButton;
        private SaveFileDialog savePresetDialog;
        private OpenFileDialog openPresetDialog;
        private System.Windows.Forms.Timer rpcUpdateTimer;
        private Button settingsOpen;
        private ComboBox complevelSelector;
        private ComboBox iwadBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private ComboBox loadPresetBox;
        private Button customPresetButton;
        private ToolTip toolTips;
        private OpenFileDialog fileImport;
    }
}