using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doom_Loader
{
    public partial class SavePreset : Form
    {
        public SavePreset()
        {
            InitializeComponent();
        }

        private void SavePresetButton(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                string path = $"{Main.FindMintyLauncherFolder()}Presets\\{textBox1.Text}{ApplicationVariables.PRESET_EXTENSION}";
                Main.SavePreset(path);
                ApplicationVariables.presetName = textBox1.Text;
                this.Close();
            }
        }

        private void SavePreset_Load(object sender, EventArgs e)
        {
            ApplicationVariables.presetName = string.Empty;
        }
    }
}
