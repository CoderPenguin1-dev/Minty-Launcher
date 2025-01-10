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

        // If changes are made here, also adapt changes to SavePreset() in Main.cs
        private void SavePresetButton(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                string path = $"%appdata%\\MintyLauncher\\Presets\\{textBox1.Text}.mlPreset";
                path = Environment.ExpandEnvironmentVariables(path);

                // Save extra paramaters, source port, and selected complevel.
                string file = @"";
                file += $"{ApplicationVariables.arguments}\n";
                file += $"{ApplicationVariables.exe}\n";
                file += $"{ApplicationVariables.complevel}\n";

                // Save the External File list to line index 5.
                if (ApplicationVariables.externalFiles.Length != 0)
                {
                    if (ApplicationVariables.externalFiles.Length == 1) file += $"{ApplicationVariables.externalFiles[0]}";
                    else
                    {
                        for (int i = 0; i < ApplicationVariables.externalFiles.Length - 1; i++)
                            file += $"{ApplicationVariables.externalFiles[i]},";
                        file += ApplicationVariables.externalFiles[^1];
                    }
                }
                File.WriteAllText(path, file);
                this.Close();
            }
        }
    }
}
