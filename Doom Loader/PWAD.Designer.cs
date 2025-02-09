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
            button1 = new Button();
            button2 = new Button();
            addPWADDialog = new OpenFileDialog();
            button3 = new Button();
            button4 = new Button();
            toolTips = new ToolTip(components);
            SuspendLayout();
            // 
            // pwadList
            // 
            pwadList.FormattingEnabled = true;
            pwadList.HorizontalScrollbar = true;
            pwadList.Location = new Point(13, 13);
            pwadList.Name = "pwadList";
            pwadList.SelectionMode = SelectionMode.MultiExtended;
            pwadList.Size = new Size(244, 184);
            pwadList.TabIndex = 0;
            // 
            // button1
            // 
            button1.AccessibleName = "";
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.Location = new Point(13, 203);
            button1.Name = "button1";
            button1.Size = new Size(41, 39);
            button1.TabIndex = 1;
            button1.Text = "+";
            button1.UseVisualStyleBackColor = true;
            button1.Click += AddPWAD;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            button2.Location = new Point(60, 203);
            button2.Name = "button2";
            button2.Size = new Size(41, 39);
            button2.TabIndex = 2;
            button2.Text = "-";
            button2.UseVisualStyleBackColor = true;
            button2.Click += RemovePWAD;
            // 
            // addPWADDialog
            // 
            addPWADDialog.Filter = "All Supported Files|*.wad;*.pk3;*.deh;*bex|WADs|*.wad|PK3s|*.pk3|DeHackED|*.deh|BEX|*.bex|All Files|*.*";
            addPWADDialog.Multiselect = true;
            addPWADDialog.Title = "Add External File(s)";
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button3.Location = new Point(169, 203);
            button3.Name = "button3";
            button3.Size = new Size(41, 39);
            button3.TabIndex = 3;
            button3.Text = "↑";
            button3.UseVisualStyleBackColor = true;
            button3.Click += ReorderItemUp;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button4.Location = new Point(216, 203);
            button4.Name = "button4";
            button4.Size = new Size(41, 39);
            button4.TabIndex = 4;
            button4.Text = "↓";
            button4.UseVisualStyleBackColor = true;
            button4.Click += ReorderItemDown;
            // 
            // PWAD
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 252);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
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
        private Button button1;
        private Button button2;
        private OpenFileDialog addPWADDialog;
        private Button button3;
        private Button button4;
        private ToolTip toolTips;
    }
}