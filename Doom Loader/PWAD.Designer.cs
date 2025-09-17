namespace Doom_Loader
{
    partial class PWAD
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
            components = new System.ComponentModel.Container();
            pwadList = new ListBox();
            addItemButton = new Button();
            removeItemButton = new Button();
            addPWADDialog = new OpenFileDialog();
            reorderUpButton = new Button();
            reorderDownButton = new Button();
            toolTips = new ToolTip(components);
            SuspendLayout();
            // 
            // pwadList
            // 
            pwadList.FormattingEnabled = true;
            pwadList.HorizontalScrollbar = true;
            pwadList.IntegralHeight = false;
            pwadList.Location = new Point(13, 13);
            pwadList.MaximumSize = new Size(244, 184);
            pwadList.MinimumSize = new Size(244, 184);
            pwadList.Name = "pwadList";
            pwadList.SelectionMode = SelectionMode.MultiExtended;
            pwadList.Size = new Size(244, 184);
            pwadList.TabIndex = 0;
            pwadList.SelectedIndexChanged += CheckAmountSelected;
            pwadList.MouseDown += ShowExternalFilePaths;
            // 
            // addItemButton
            // 
            addItemButton.AccessibleName = "";
            addItemButton.BackgroundImage = Properties.Resources.plus;
            addItemButton.BackgroundImageLayout = ImageLayout.Center;
            addItemButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            addItemButton.Location = new Point(13, 203);
            addItemButton.Name = "addItemButton";
            addItemButton.Size = new Size(41, 39);
            addItemButton.TabIndex = 1;
            addItemButton.UseVisualStyleBackColor = true;
            addItemButton.MouseDown += AddPWAD;
            // 
            // removeItemButton
            // 
            removeItemButton.BackgroundImage = Properties.Resources.minus;
            removeItemButton.BackgroundImageLayout = ImageLayout.Center;
            removeItemButton.Enabled = false;
            removeItemButton.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            removeItemButton.Location = new Point(60, 203);
            removeItemButton.Name = "removeItemButton";
            removeItemButton.Size = new Size(41, 39);
            removeItemButton.TabIndex = 2;
            removeItemButton.UseVisualStyleBackColor = true;
            removeItemButton.Click += RemovePWAD;
            // 
            // addPWADDialog
            // 
            addPWADDialog.Filter = "DOOM Mod Files|*.wad;*.pk3;*.pke;*.pk7;*.deh;*bex|WADs|*.wad|PK3s|*.pk3|PKEs|*.pke|PK7s|*.pk7|DeHackED|*.deh|BEX|*.bex|All Files|*.*";
            addPWADDialog.Multiselect = true;
            addPWADDialog.Title = "Add External File(s)";
            // 
            // reorderUpButton
            // 
            reorderUpButton.BackgroundImage = Properties.Resources.arrow_up;
            reorderUpButton.BackgroundImageLayout = ImageLayout.Center;
            reorderUpButton.Enabled = false;
            reorderUpButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            reorderUpButton.Location = new Point(169, 203);
            reorderUpButton.Name = "reorderUpButton";
            reorderUpButton.Size = new Size(41, 39);
            reorderUpButton.TabIndex = 3;
            reorderUpButton.UseVisualStyleBackColor = true;
            reorderUpButton.Click += ReorderItemUp;
            // 
            // reorderDownButton
            // 
            reorderDownButton.BackgroundImage = Properties.Resources.arrow_down;
            reorderDownButton.BackgroundImageLayout = ImageLayout.Center;
            reorderDownButton.Enabled = false;
            reorderDownButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            reorderDownButton.Location = new Point(216, 203);
            reorderDownButton.Name = "reorderDownButton";
            reorderDownButton.Size = new Size(41, 39);
            reorderDownButton.TabIndex = 4;
            reorderDownButton.UseVisualStyleBackColor = true;
            reorderDownButton.Click += ReorderItemDown;
            // 
            // PWAD
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 252);
            Controls.Add(reorderDownButton);
            Controls.Add(reorderUpButton);
            Controls.Add(removeItemButton);
            Controls.Add(addItemButton);
            Controls.Add(pwadList);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PWAD";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "External File Manager";
            Load += ManagerSetup;
            DragDrop += PWADDragDrop;
            DragOver += PWADDragOver;
            ResumeLayout(false);
        }

        #endregion

        private ListBox pwadList;
        private Button addItemButton;
        private Button removeItemButton;
        private OpenFileDialog addPWADDialog;
        private Button reorderUpButton;
        private Button reorderDownButton;
        private ToolTip toolTips;
    }
}