namespace Doom_Loader
{
    partial class About
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            version = new Label();
            label2 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(-49, 7);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(228, 203);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(129, 9);
            label1.Name = "label1";
            label1.Size = new Size(157, 30);
            label1.TabIndex = 1;
            label1.Text = "Minty Launcher";
            // 
            // version
            // 
            version.AutoSize = true;
            version.Location = new Point(131, 39);
            version.Name = "version";
            version.Size = new Size(75, 15);
            version.TabIndex = 2;
            version.Text = "Version X.X.X";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(129, 79);
            label2.Name = "label2";
            label2.Size = new Size(202, 75);
            label2.TabIndex = 3;
            label2.Text = "A launcher for id Tech 1 source ports,\r\nprimarily vanilla-style ports.\r\n\r\nProgrammed by CoderPenguin1.\r\n\r\n";
            // 
            // button1
            // 
            button1.Location = new Point(129, 178);
            button1.Name = "button1";
            button1.Size = new Size(178, 23);
            button1.TabIndex = 4;
            button1.Text = "View Credits && Legal Info";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 213);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(version);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "About";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            Load += About_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label version;
        private Label label2;
        private Button button1;
    }
}