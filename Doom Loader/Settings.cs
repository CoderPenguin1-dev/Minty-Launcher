using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doom_Loader
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used for rewriting the config file's settings.
        /// </summary>
        /// <param name="path"></param>
        private static void RewriteAppDataConfig(string path)
        {
            File.WriteAllLines(path, [ ApplicationVariables.rpc.ToString(),
                ApplicationVariables.closeOnPlay.ToString(),
                ApplicationVariables.topMost.ToString(),
                ApplicationVariables.useDefault.ToString(),
                ApplicationVariables.customPreset.ToString(),
                ApplicationVariables.IWADFolderPath.ToString(),
                ApplicationVariables.rpcFilesShown.ToString() ]);
        }

        /// <summary>
        /// Checks if the config file in use is either in Roaming AppData or in the CWD of Minty Launcher.
        /// </summary>
        /// <returns>Full path of the used config.</returns>
        private static string CheckForWhichConfig()
        {
            // Try to find the portable settings file.
            if (File.Exists("mintyLauncher.PortableSettings"))
                return "mintyLauncher.PortableSettings";
            // If no portable settings file is found in the CWD,
            // return the settings file found in Minty Launcher's folder in the user's Roaming AppData folder.
            else
                return Environment.ExpandEnvironmentVariables("%appdata%\\MintyLauncher\\settings.txt");
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            rcpBox.Checked = ApplicationVariables.rpc;
            closeBox.Checked = ApplicationVariables.closeOnPlay;
            topMostBox.Checked = ApplicationVariables.topMost;
            defaultBox.Checked = ApplicationVariables.useDefault;
            customPresetBox.Checked = ApplicationVariables.customPreset;
            rpcFilesTrackBar.Value = ApplicationVariables.rpcFilesShown;
            if (!rcpBox.Checked)
                rpcFilesTrackBar.Enabled = false;

            if (File.Exists("mintyLauncher.PortableSettings")) button2.Enabled = false; // Disable "Make Settings Portable" button if portable settings file already exists.

            #region Tooltips
            toolTips.SetToolTip(rcpBox, "Enable Discord Rich Presence intergration.");
            toolTips.SetToolTip(button2, "Creates a portable settings file in the same directory as Minty Launcher.");
            toolTips.SetToolTip(openPresetsLocation, "Open the AppData Presets folder.");
            toolTips.SetToolTip(closeBox, "Closes Minty Launcher after the port has been closed.");
            toolTips.SetToolTip(topMostBox, "Makes Minty Launcher the top window after the port has been closed.");
            toolTips.SetToolTip(customPresetBox, "Provide a file dialog when loading/saving presets.");
            toolTips.SetToolTip(defaultBox, @"Makes an AppData preset titled ""Default"" load on launch of Minty Launcher.");
            toolTips.SetToolTip(iwadFolderButton, ApplicationVariables.IWADFolderPath);
            #endregion
        }

        #region Settings
        private void DiscordRichPresence(object sender, EventArgs e)
        {
            ApplicationVariables.rpc = rcpBox.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
            if (ApplicationVariables.rpc)
                rpcFilesTrackBar.Enabled = true;
            else rpcFilesTrackBar.Enabled = false;
        }

        private void CloseOnPlay(object sender, EventArgs e)
        {
            ApplicationVariables.closeOnPlay = closeBox.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
        }

        private void ShowWindowAfterQuit(object sender, EventArgs e)
        {
            ApplicationVariables.topMost = topMostBox.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
        }

        private void DefaultPreset(object sender, EventArgs e)
        {
            ApplicationVariables.useDefault = defaultBox.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
        }

        private void CustomPresetLocation(object sender, EventArgs e)
        {
            ApplicationVariables.customPreset = customPresetBox.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
        }

        public void SetIWADFolder(object sender, EventArgs e)
        {
            if (iwadFolderDialog.ShowDialog() != DialogResult.Cancel)
            {
                ApplicationVariables.IWADFolderPath = iwadFolderDialog.SelectedPath;
                string path = CheckForWhichConfig();
                RewriteAppDataConfig(path);
                toolTips.SetToolTip(iwadFolderButton, ApplicationVariables.IWADFolderPath);
            }
        }

        private void RPCAmountOfFilesShown(object sender, EventArgs e)
        {
            ApplicationVariables.rpcFilesShown = rpcFilesTrackBar.Value;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
        }
        #endregion

        private void EnablePortableSettings(object sender, EventArgs e)
        {
            RewriteAppDataConfig("mintyLauncher.PortableSettings");
            button2.Enabled = false;
        }

        private void AboutMenu(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void ShowPresets(object sender, EventArgs e)
        {
            string appdata = Environment.ExpandEnvironmentVariables("%appdata%");
            Process.Start("explorer.exe", $"{appdata}\\MintyLauncher\\Presets");
        }
    }
}
