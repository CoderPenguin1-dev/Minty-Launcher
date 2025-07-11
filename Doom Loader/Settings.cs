using System.Diagnostics;

namespace Doom_Loader
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private static bool restart = false;

        /// <summary>
        /// Used for rewriting the config file's settings.
        /// </summary>
        /// <param name="path"></param>
        private static void RewriteAppDataConfig(string path)
        {
            File.WriteAllLines(path, [ ApplicationVariables.rpc.ToString(),
                ApplicationVariables.closeOnPlay.ToString(),
                ApplicationVariables.useSourcePortDirectory.ToString(),
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
            return Main.FindMintyLauncherFolder() + ApplicationVariables.SETTINGS_FILE;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            rcpBox.Checked = ApplicationVariables.rpc;
            closeBox.Checked = ApplicationVariables.closeOnPlay;
            defaultBox.Checked = ApplicationVariables.useDefault;
            customPresetBox.Checked = ApplicationVariables.customPreset;
            rpcFilesTrackBar.Value = ApplicationVariables.rpcFilesShown;
            useSourcePort.Checked = ApplicationVariables.useSourcePortDirectory;
            if (!rcpBox.Checked)
                rpcFilesTrackBar.Enabled = false;

            if (Path.Exists("MintyLauncher")) button2.Enabled = false; // Disable "Make Settings Portable" button if portable settings file already exists.

            #region Tooltips
            toolTips.SetToolTip(rcpBox, "Enable Discord Rich Presence intergration.");
            toolTips.SetToolTip(button2, "Moves all settings to a local folder called MintyLauncher. Will read/write settings in there.");
            toolTips.SetToolTip(openPresetsLocation, "Open the currently-used Minty Launcher folder.");
            toolTips.SetToolTip(closeBox, "Closes Minty Launcher after the port has been closed.");
            toolTips.SetToolTip(customPresetBox, "Provide a file dialog when loading/saving presets.");
            toolTips.SetToolTip(defaultBox, @"Makes an Settings Folder preset titled ""Default"" load on launch of Minty Launcher.");
            toolTips.SetToolTip(iwadFolderButton, ApplicationVariables.IWADFolderPath);
            toolTips.SetToolTip(rpcFilesTrackBar, "Sets how many individual files can be shown on the Discord Activity.");
            toolTips.SetToolTip(label2, "Sets how many individual files can be shown on the Discord Activity.");
            toolTips.SetToolTip(useSourcePort, "Use the source port's folder as the port's working directory\nWill have any asterisk in the Extra Parameters textbox insert Minty Launcher's working directory.");
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
            restart = true;
        }

        private void CloseOnPlay(object sender, EventArgs e)
        {
            ApplicationVariables.closeOnPlay = closeBox.Checked;
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
            restart = true;
        }

        internal void SetIWADFolder(object sender, EventArgs e)
        {
            if (iwadFolderDialog.ShowDialog() != DialogResult.Cancel)
            {
                ApplicationVariables.IWADFolderPath = iwadFolderDialog.SelectedPath;
                string path = CheckForWhichConfig();
                RewriteAppDataConfig(path);
                toolTips.SetToolTip(iwadFolderButton, ApplicationVariables.IWADFolderPath);
            }
        }

        private void UseSourcePortAsCWD(object sender, EventArgs e)
        {
            ApplicationVariables.useSourcePortDirectory = useSourcePort.Checked;
            string path = CheckForWhichConfig();
            RewriteAppDataConfig(path);
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
            string oldPath = Environment.ExpandEnvironmentVariables("%appdata%\\MintyLauncher\\"); // Didn't use CheckForWhichConfig() due to this always being in AppData.
            // Create settings folder.
            Directory.CreateDirectory("MintyLauncher");

            // Copy files
            File.Copy($"{oldPath}{ApplicationVariables.SETTINGS_FILE}", $"MintyLauncher\\{ApplicationVariables.SETTINGS_FILE}");
            File.Copy($"{oldPath}{ApplicationVariables.COMPLEVEL_FILE}", $"MintyLauncher\\{ApplicationVariables.COMPLEVEL_FILE}");
            File.Copy($"{oldPath}{ApplicationVariables.PORTDATABASE_FILE}", $"MintyLauncher\\{ApplicationVariables.PORTDATABASE_FILE}");

            // Copy presets.
            Directory.CreateDirectory("MintyLauncher\\Presets");
            foreach (string file in Directory.GetFiles($"{oldPath}Presets"))
                File.Copy(file, $"MintyLauncher\\Presets\\{Path.GetFileName(file)}");

            // Disable this button to prevent user from doing this whole process all over again.
            button2.Enabled = false;
        }

        private void AboutMenu(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void ShowPresets(object sender, EventArgs e)
        {
            string path = Main.FindMintyLauncherFolder();
            Process.Start("explorer.exe", $"{path}");
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (restart)
            {
                MessageBox.Show("Minty Launcher will now close due to one of your settings needing Minty Launcher to restart.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }
        }
    }
}
