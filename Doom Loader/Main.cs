using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using DiscordRPC;

namespace Doom_Loader
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private static bool boot = true; // Used for calling LoadPresets() in AppDataInit() to prevent some odd bug.

        private void ComplevelIndexChanged(object sender, EventArgs e)
        {
            if (complevelSelector.SelectedIndex == 0) ApplicationVariables.complevel = "-";
            else
            {
                foreach (string entry in ApplicationVariables.complevels)
                {
                    string[] entryTokens = entry.Split(";");
                    if (entryTokens[0] == (string)complevelSelector.SelectedItem)
                        ApplicationVariables.complevel = entryTokens[1];
                }
            }
        }

        #region Checkers
        private void CheckComplevel()
        {
            if (ApplicationVariables.complevel == "-")
                complevelSelector.SelectedIndex = 0;
            else
            {
                foreach (string entry in ApplicationVariables.complevels)
                {
                    string[] entryTokens = entry.Split(";");
                    if (entryTokens[1] == ApplicationVariables.complevel)
                    {
                        complevelSelector.SelectedItem = entryTokens[0];
                    }
                }
            }
        }

        private void CheckPortDatabase()
        {
            bool dataFound = false;
            string path = FindMintyLauncherFolder();
            foreach (string portData in File.ReadAllLines($"{path}{ApplicationVariables.PORTDATABASE_FILE}"))
            {
                if (portData.StartsWith('#')) continue; // Check for comment lines. Here only for legacy support.
                string[] data = portData.Split(';');
                if (string.Equals(Path.GetFileName(ApplicationVariables.sourcePort), data[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    portButton.Text = $"{data[1]}";
                    dataFound = true;
                    break;
                }
            }
            if (!dataFound) portButton.Text = Path.GetFileNameWithoutExtension(sourcePortDialog.SafeFileName);
        }

        public static string FindMintyLauncherFolder()
        {
            // Try to find the portable settings file.
            if (Path.Exists("MintyLauncher"))
                return "MintyLauncher\\";
            // If no portable settings file is found in the CWD,
            // return the settings file found in Minty Launcher's folder in the user's Roaming AppData folder.
            else
                return Environment.ExpandEnvironmentVariables("%appdata%\\MintyLauncher\\");
        }
        #endregion

        private void IWADChanged(object sender, EventArgs e)
        {
            try
            {
                ApplicationVariables.IWAD = $"{ApplicationVariables.IWADFolderPath}/{iwadBox.SelectedItem}";
            }
            catch { }
        }

        private void RefreshIWAD(object sender, EventArgs e)
        {
            try
            {
                // Check if the user already has an IWAD selected.
                // Save it for later if they do.
                string IWAD = null;
                if (iwadBox.SelectedItem != null) 
                    IWAD = iwadBox.SelectedItem.ToString();

                iwadBox.Items.Clear();
                string[] IWADs = Directory.GetFiles(ApplicationVariables.IWADFolderPath);

                // Find the IWADs from the folder path.
                foreach (string wad in IWADs)
                {
                    if (Path.GetExtension(wad).Equals(".wad", StringComparison.CurrentCultureIgnoreCase) || Path.GetExtension(wad).Equals(".ipk3", StringComparison.CurrentCultureIgnoreCase))
                    {
                        iwadBox.Items.Add(Path.GetFileName(wad));
                    }
                }
                // Reset the selected IWAD if the user had an IWAD selected.
                if (iwadBox.Items.Contains(IWAD)) iwadBox.SelectedItem = IWAD;
                else iwadBox.SelectedItem = null;
            }
            // Ask user to set the IWAD folder path if they
            // either don't have one set or the one set is missing.
            catch
            {
                var error = MessageBox.Show("IWADs Folder path missing.\nSet new path now?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (error == DialogResult.Yes) new Settings().SetIWADFolder(sender, e);
                else if (error == DialogResult.No) MessageBox.Show("Please set new path in settings.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #region External Windows
        private void PWADManagerOpen(object sender, EventArgs e)
        {
            new PWAD().ShowDialog();
        }

        private void SettingsMenuOpen(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }
        #endregion

        private void SelectPort(object sender, EventArgs e)
        {
            if (sourcePortDialog.ShowDialog() != DialogResult.Cancel)
            {
                ApplicationVariables.sourcePort = sourcePortDialog.FileName;
                CheckPortDatabase();
            }
        }

        // RPC Initialization, DEH/BEX patch sorting, and starting port with arguments.
        private void Play(object sender, EventArgs e)
        {
            #region Discord RPC
            if (ApplicationVariables.rpc)
            {
                RPCClient.Initialize();
                // State Setup
                string state;
                if (ApplicationVariables.externalFiles.Length > ApplicationVariables.rpcFilesShown && ApplicationVariables.rpcFilesShown != 0)
                    state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | Multiple Files";
                else if (ApplicationVariables.rpcFilesShown == 0)
                    state = $"{Path.GetFileName(ApplicationVariables.IWAD)}";
                else switch (ApplicationVariables.externalFiles.Length)
                {
                    case 0:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)}";
                        break;
                    case 1:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | {Path.GetFileName(ApplicationVariables.externalFiles[0])}";
                        break;
                    default:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | ";
                        foreach (string file in ApplicationVariables.externalFiles)
                        {
                            state += $"{Path.GetFileName(file)}, ";
                        }
                        state = state.Remove(state.Length - 2, 2);
                        break;
                } // Check for how many external files were loaded in

                RPCClient.client.SetPresence(new RichPresence()
                {
                    State = state,
                    Details = $"Port: {portButton.Text}",
                    Timestamps = new()
                    {
                        Start = Timestamps.Now.Start,
                    },
                    Assets = new Assets()
                    {
                        LargeImageKey = "icon",
                        LargeImageText = $"Minty Launcher v{GetType().Assembly.GetName().Version.Major}.{GetType().Assembly.GetName().Version.Minor}.{GetType().Assembly.GetName().Version.Build}"
                    },
                    Buttons =
                    [
                        new DiscordRPC.Button() { Label = "View On GitHub", Url = "https://github.com/PENGUINCODER1/Minty-Launcher" }
                    ]
                });
            }
            #endregion

            // Argument Setup
            string portArguments = "";

            // Extra Paramaters
            if (extraParamsTextBox.Text != "")
            {
                portArguments += ApplicationVariables.arguments
                    .Replace("*", Environment.CurrentDirectory) // Check if there's any Minty CWD characters.
                    // Replace all back-slashes with forward-slashes to prevent bug where it'll think it's an escape character.
                    .Replace('\\', '/') + " ";
            }
            if (ApplicationVariables.complevel != "-") portArguments += $"-complevel {ApplicationVariables.complevel} "; // Complevel

            // Check if there was a DeHacked patch
            List<string> extFileStore = []; // Used so the PWAD adder code doesn't need to iterate through the useless DEH and BEX files later.
            for (int i = 0; i < ApplicationVariables.externalFiles.Length; i++)
            {
                if (ApplicationVariables.externalFiles[i].EndsWith(".deh", StringComparison.CurrentCultureIgnoreCase))
                    portArguments += $"-deh \"{ApplicationVariables.externalFiles[i]}\" ";
                else if (ApplicationVariables.externalFiles[i].EndsWith(".bex", StringComparison.CurrentCultureIgnoreCase))
                    portArguments += $"-bex \"{ApplicationVariables.externalFiles[i]}\" ";
                else // Not an DEH/BEX patch? Shove it along with the external files.
                    extFileStore.Add(ApplicationVariables.externalFiles[i]);
            }

            // Check if PWAD was selected
            if (extFileStore.Count != 0)
            {
                portArguments += $"-iwad \"{ApplicationVariables.IWAD}\" -file ";
                foreach (string pwad in extFileStore) portArguments += $"\"{pwad}\" ";
            }
            else portArguments += $"-iwad \"{ApplicationVariables.IWAD}\"";

            // Start Port
            try
            {
                Process.Start(
                    new ProcessStartInfo(ApplicationVariables.sourcePort, portArguments)
                    {
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        WorkingDirectory = Path.GetDirectoryName(ApplicationVariables.sourcePort)
                    }).WaitForExit();
                if (ApplicationVariables.closeOnPlay)
                {
                    if (ApplicationVariables.rpc) RPCClient.client.Dispose(); // Kill RPC Connection
                    Environment.Exit(0);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (ApplicationVariables.rpc) RPCClient.client.Dispose(); // Kill RPC Connection

            // Move window visible above all windows
            if (ApplicationVariables.topMost)
            {
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        #region Presets
        private void LoadPreset(string path)
        {
            string[] args = File.ReadAllLines(path);

            #region Complevel
            ApplicationVariables.complevel = args[2];
            CheckComplevel();
            #endregion

            extraParamsTextBox.Text = args[0]; // Arguments

            #region Sourceport and IWADs
            ApplicationVariables.sourcePort = Regex.Replace(args[1], @"[^\w\\.@: -]", string.Empty);
            CheckPortDatabase();
            #endregion

            // PWAD
            ApplicationVariables.externalFiles = [];
            if (args.Length == 4)
            {
                ApplicationVariables.externalFiles = args[3].Split(',');
                if (ApplicationVariables.externalFiles.Length == 1) // I can't honestly remember why this if statement was put into here.
                {                                                   // Will not remove to prevent it possibly breaking like a Jenga tower.
                    ApplicationVariables.externalFiles[0] = args[3];
                }
            }
        }

        private void LoadPresetComboBox(object sender, EventArgs e)
        {
            string path;

            if (!ApplicationVariables.customPreset || ApplicationVariables.useDefault && boot)
            {
                path = $"{FindMintyLauncherFolder()}Presets\\{loadPresetBox.SelectedItem}.mlPreset";
                boot = false;
            }
            else
            {
                return; // Should not end here under normal usage, hopefully
            }

            LoadPreset(path);
        }

        private void LoadCustomPreset(object sender, EventArgs e)
        {
            if (openPresetDialog.ShowDialog() != DialogResult.Cancel)
            {
                LoadPreset(openPresetDialog.FileName);
            }
        }

        private void SavePresetButton(object sender, EventArgs e)
        {
            if (ApplicationVariables.customPreset)
            {
                if (savePresetDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string path = savePresetDialog.FileName;
                    path = Environment.ExpandEnvironmentVariables(path);

                    SavePreset(path);
                }
            }
            else
            {
                new SavePreset().ShowDialog();
                if (ApplicationVariables.presetName != string.Empty)
                {
                    loadPresetBox.Items.Add(ApplicationVariables.presetName);
                    loadPresetBox.SelectedItem = ApplicationVariables.presetName;
                }
            }
        }

        public static void SavePreset(string path)
        {
            string file = @"";
            file += $"{ApplicationVariables.arguments}\n";
            file += $"{ApplicationVariables.sourcePort}\n";
            file += $"{ApplicationVariables.complevel}\n";

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
        }

        private void QuickSavePreset(object sender, MouseEventArgs e)
        {
            try
            {
                if (!ApplicationVariables.customPreset && e.Button == MouseButtons.Right)
                {
                    string path = FindMintyLauncherFolder() + "Presets\\" + loadPresetBox.SelectedItem + ".mlPreset";
                    SavePreset(path);
                    MessageBox.Show("Preset Saved", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch 
            {
                MessageBox.Show("No preset currently selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshPresetBox(object sender, EventArgs e)
        {
            string presetName = null;
            if (loadPresetBox.SelectedItem != null) presetName = loadPresetBox.SelectedItem.ToString();
            loadPresetBox.Items.Clear();
            string path = $"{FindMintyLauncherFolder()}Presets";
            string[] presets = Directory.GetFiles(path);

            foreach (string preset in presets)
            {
                if (preset.EndsWith(".mlPreset"))
                    loadPresetBox.Items.Add(Path.GetFileNameWithoutExtension(preset));
            }
            if (loadPresetBox.Items.Contains(presetName)) loadPresetBox.SelectedItem = presetName;
            else loadPresetBox.SelectedItem = null;
        }
        #endregion

        private void ExtraParamsChanged(object sender, EventArgs e)
        {
            ApplicationVariables.arguments = extraParamsTextBox.Text;
        }

        // Update the time since started for the Discord RPC.
        private void UpdateRPCTimestamp(object sender, EventArgs e)
        {
            RPCClient.client.Invoke();
        }

        // Settings, init complevel ComboBox and tooltips, and check command line arguments.
        // For --info and -i, check Program.cs
        private void AppDataInit(object sender, EventArgs e)
        {
            string appdata = Environment.ExpandEnvironmentVariables("%appdata%\\");

            if (!Path.Exists(Environment.ExpandEnvironmentVariables($"{appdata}MintyLauncher\\")) && !Path.Exists("MintyLauncher\\"))
            {
                string folderPath = $"{appdata}\\MintyLauncher\\";
                Directory.CreateDirectory(folderPath);
                Generate.Settings(folderPath);
                // Prompt user to select IWADs folder path.
                var error = MessageBox.Show("IWADs Folder path missing.\nSet new path now?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (error == DialogResult.Yes) new Settings().SetIWADFolder(sender, e);
                else if (error == DialogResult.No) MessageBox.Show("Please set new path in settings.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.CreateDirectory($"{folderPath}Presets");
                Generate.PortDatabase(folderPath);
                Generate.Complevel(folderPath);
            }

            string path = FindMintyLauncherFolder();
            string[] settings = File.ReadAllLines($"{path}{ApplicationVariables.SETTINGS_FILE}");

            #region Settings
            ApplicationVariables.rpc = bool.Parse(settings[0]);
            ApplicationVariables.closeOnPlay = bool.Parse(settings[1]);
            ApplicationVariables.topMost = bool.Parse(settings[2]);
            ApplicationVariables.useDefault = bool.Parse(settings[3]);
            ApplicationVariables.customPreset = bool.Parse(settings[4]);
            if (ApplicationVariables.customPreset)
            {
                customPresetButton.Enabled = true;
                customPresetButton.Visible = true;
                loadPresetBox.Enabled = false;
                loadPresetBox.Visible = false;
            }
            ApplicationVariables.IWADFolderPath = settings[5];
            ApplicationVariables.rpcFilesShown = int.Parse(settings[6]);
            #endregion

            #region Complevels
            string[] complevels = File.ReadAllLines($"{path}{ApplicationVariables.COMPLEVEL_FILE}");
            ApplicationVariables.complevels = complevels;
            foreach (string complevel in complevels)
                complevelSelector.Items.Add(complevel.Split(";")[0]);
            complevelSelector.SelectedIndex = 0;
            #endregion
            
            // Check if there is the Default preset.
            if (File.Exists($"{path}Presets\\Default.mlPreset") && ApplicationVariables.useDefault) 
            {
                // Load Default preset. Also add it to the list and select it.
                loadPresetBox.Items.Add("Default");
                loadPresetBox.SelectedItem = "Default";
                LoadPreset($"{path}Presets\\Default.mlPreset");
            }

            #region Tooltips
            complevelSelector.SelectedIndex = 0;
            toolTips.SetToolTip(iwadBox, "Select your desired IWAD. You can change the IWAD folder in Settings.");
            toolTips.SetToolTip(pwadManagerButton, "Open the External File Manager.");
            toolTips.SetToolTip(complevelSelector, "Used to emulate a specific engine. Only works for ports that support the -complevel parameter.");
            toolTips.SetToolTip(extraParamsTextBox, "Add in any extra parameters you may want. Right click to add a file path to the end of the text.");
            #endregion

            // Command Line Arguments
            bool noGUI = false;
            bool usedIWAD = false;
            if (Environment.GetCommandLineArgs().Length > 0)
            {
                string[] args = Environment.GetCommandLineArgs();
                for (int i = 0; i < args.Length; i++)
                {
                    // Anything with i++ takes in an argument of its own. Put there to skip the argument on the next pass.
                    switch (args[i])
                    {
                        case "--preset-path" or "-p":
                            i++;
                            loadPresetBox.Items.Add("External Preset");
                            loadPresetBox.SelectedItem = "External Preset";
                            LoadPreset(args[i]);
                            break;

                        case "--preset":
                            i++;
                            RefreshPresetBox(sender, e);
                            // Here due to how ComboBoxes work with capitalization.
                            if (!loadPresetBox.Items.Contains(args[i]))
                            {
                                MessageBox.Show("Given preset does not exist. Check to see if your capitalization is wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            loadPresetBox.SelectedItem = args[i];
                            LoadPreset($"{path}Presets\\{args[i]}.mlPreset");
                            break;

                        case "--iwad-folder":
                            i++;
                            ApplicationVariables.IWADFolderPath = args[i];
                            if (!Path.Exists(ApplicationVariables.IWADFolderPath))
                            {
                                MessageBox.Show("Given IWAD Folder Path does not exist. Check to see if your capitalization is wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            if (usedIWAD)
                            {
                                MessageBox.Show("Specified new IWAD Folder Path after setting IWAD.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            break;

                        case "--iwad" or "-w":
                            i++;
                            RefreshIWAD(sender, e);
                            // Here due to how ComboBoxes work with capitalization.
                            if (!iwadBox.Items.Contains(args[i]))
                            {
                                MessageBox.Show("Given IWAD does not exist. Check to see if your capitalization is wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            iwadBox.SelectedItem = args[i];
                            usedIWAD = true;
                            break;

                        case "--no-gui":
                            noGUI = true;
                            break;

                        case "--no-gui-rpc" or "-n":
                            i++;
                            if (args[i] == "0"
                                || args[i].Equals("off", StringComparison.CurrentCultureIgnoreCase)
                                || args[i].Equals("false", StringComparison.CurrentCultureIgnoreCase))
                            {
                                ApplicationVariables.rpc = false;
                            }
                            else if (args[i] == "1"
                                || args[i].Equals("on", StringComparison.CurrentCultureIgnoreCase)
                                || args[i].Equals("true", StringComparison.CurrentCultureIgnoreCase))
                            {
                                ApplicationVariables.rpc = true;
                            }
                            else
                            {
                                MessageBox.Show("Invalid input for --no-gui-rpc. Refer to USER.MD for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }

                            noGUI = true;
                            break;
                    }
                }
            }
            if (noGUI)
            {
                Play(sender, e);
                this.Close();
            }
        }

        private void ExtraParametersInsertFilePath(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (fileImport.ShowDialog() != DialogResult.Cancel)
                {
                    extraParamsTextBox.AppendText($"\"{fileImport.FileName}\"");
                }
            }
        }
    }
}