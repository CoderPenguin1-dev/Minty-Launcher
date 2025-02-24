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

        private void ComplevelChanged(object sender, EventArgs e)
        {
            switch (complevelSelector.SelectedItem)
            {
                case "None":
                    ApplicationVariables.complevel = 0;
                    break;
                case "Doom v1.9":
                    ApplicationVariables.complevel = 2;
                    break;
                case "Ultimate Doom":
                    ApplicationVariables.complevel = 3;
                    break;
                case "Final Doom":
                    ApplicationVariables.complevel = 4;
                    break;
                case "Boom v2.02":
                    ApplicationVariables.complevel = 9;
                    break;
                case "MBF":
                    ApplicationVariables.complevel = 11;
                    break;
                case "MBF21":
                    ApplicationVariables.complevel = 21;
                    break;
            }
        }

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
                string IWAD = null;
                if (iwadBox.SelectedItem != null) IWAD = iwadBox.SelectedItem.ToString(); // Check if the user already has an IWAD selected
                                                                                          // Save it for later if they do.
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
            // Ask user to set the IWAD folder path if they either don't have one set or the one set is missing.
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
                ApplicationVariables.sourcePort = "\"" + sourcePortDialog.FileName + "\"";

                bool dataFound = false;
                if (File.Exists("mintyLauncher.portDatabase"))
                {
                    foreach (string portData in File.ReadAllLines("mintyLauncher.portDatabase"))
                    {
                        if (portData.StartsWith('#')) continue; // Check for comment lines.
                        string[] data = portData.Split(';');
                        if (string.Equals(Path.GetFileName(sourcePortDialog.FileName), data[0], StringComparison.CurrentCultureIgnoreCase))
                        {
                            portButton.Text = $"{data[1]}";
                            dataFound = true;
                            break;
                        }
                    }
                }
                if (!dataFound) portButton.Text = Path.GetFileNameWithoutExtension(sourcePortDialog.SafeFileName);
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

            if (extraParamsTextBox.Text != "") portArguments += ApplicationVariables.arguments + " "; // Extra Paramaters
            if (ApplicationVariables.complevel != 0) portArguments += $"-complevel {ApplicationVariables.complevel} "; // Complevel

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
                    }).WaitForExit();
                if (ApplicationVariables.closeOnPlay)
                {
                    if (ApplicationVariables.rpc) RPCClient.client.Dispose(); // Kill RPC Connection
                    Environment.Exit(0);
                }
            }
            catch { }
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
            switch (args[2])
            {
                case "0":
                    ApplicationVariables.complevel = 0;
                    complevelSelector.SelectedItem = "None";
                    break;
                case "2":
                    ApplicationVariables.complevel = 2;
                    complevelSelector.SelectedItem = "Doom v1.9";
                    break;
                case "3":
                    ApplicationVariables.complevel = 3;
                    complevelSelector.SelectedItem = "Ultimate Doom";
                    break;
                case "4":
                    ApplicationVariables.complevel = 4;
                    complevelSelector.SelectedItem = "Final Doom";
                    break;
                case "9":
                    ApplicationVariables.complevel = 9;
                    complevelSelector.SelectedItem = "Boom v2.02";
                    break;
                case "11":
                    ApplicationVariables.complevel = 11;
                    complevelSelector.SelectedItem = "MBF";
                    break;
                case "21":
                    ApplicationVariables.complevel = 21;
                    complevelSelector.SelectedItem = "MBF21";
                    break;

            }
            #endregion

            extraParamsTextBox.Text = args[0]; // Arguments

            #region Sourceport and IWADs
            ApplicationVariables.sourcePort = Regex.Replace(args[1], @"[^\w\\.@: -]", string.Empty);
            FileInfo port = new(ApplicationVariables.sourcePort);
            string directory = port.Directory.ToString();
            bool dataFound = false;
            if (File.Exists("mintyLauncher.portDatabase"))
            {
                foreach (string portData in File.ReadAllLines("mintyLauncher.portDatabase"))
                {
                    if (portData.StartsWith('#')) continue; // Check for comments
                    string[] data = portData.Split(';');
                    if (Path.GetFileName(ApplicationVariables.sourcePort) == data[0])
                    {
                        portButton.Text = $"{data[1]}";
                        dataFound = true;
                        break;
                    }
                }
            }
            if (!dataFound) portButton.Text = Path.GetFileNameWithoutExtension(ApplicationVariables.sourcePort);
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
                path = $"%appdata%\\MintyLauncher\\Presets\\{loadPresetBox.SelectedItem}.mlPreset";
                path = Environment.ExpandEnvironmentVariables(path);
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
                    string path = Environment.ExpandEnvironmentVariables($"%appdata%\\MintyLauncher\\Presets\\{loadPresetBox.SelectedItem}.mlPreset");
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
            string path = "%appdata%\\MintyLauncher\\Presets";
            path = Environment.ExpandEnvironmentVariables(path);
            string[] presets = Directory.GetFiles(path);

            foreach (string preset in presets)
            {
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
            string appdata = "%appdata%";
            appdata = Environment.ExpandEnvironmentVariables(appdata);

            // Create the Port Database file, usually only done on first run.
            if (!File.Exists("mintyLauncher.portDatabase"))
                File.WriteAllText("mintyLauncher.portDatabase", "# This is the Port Database file for The Minty Launcher" +
                    "\n# To create a new entry, do this for each port (one port per line!): [port filename].exe;[port name]" +
                    "\n# Example: gzdoom.exe;GZDoom" +
                    "\n# You can download official database updates on the GitHub Repo under each Release: https://github.com/PENGUINCODER1/Minty-Launcher");

            if (Directory.Exists($"{appdata}\\MintyLauncher") || File.Exists("mintyLauncher.PortableSettings"))
            {
                // Settings
                try
                {
                    string[] lines = [];
                    if (File.Exists("mintyLauncher.PortableSettings"))
                        lines = File.ReadAllLines("mintyLauncher.PortableSettings");
                    else lines = File.ReadAllLines($"{appdata}\\MintyLauncher\\settings.txt");

                    // Set the settings.
                    ApplicationVariables.rpc = bool.Parse(lines[0]);
                    ApplicationVariables.closeOnPlay = bool.Parse(lines[1]);
                    ApplicationVariables.topMost = bool.Parse(lines[2]);
                    ApplicationVariables.useDefault = bool.Parse(lines[3]);
                    ApplicationVariables.customPreset = bool.Parse(lines[4]);
                    if (ApplicationVariables.customPreset)
                    {
                        customPresetButton.Enabled = true;
                        customPresetButton.Visible = true;
                        loadPresetBox.Enabled = false;
                        loadPresetBox.Visible = false;
                    }
                    ApplicationVariables.IWADFolderPath = lines[5];
                    ApplicationVariables.rpcFilesShown = int.Parse(lines[6]);
                }
                catch
                {
                    File.WriteAllLines($"{appdata}\\MintyLauncher\\settings.txt", [ ApplicationVariables.rpc.ToString(),
                        ApplicationVariables.closeOnPlay.ToString(),
                        ApplicationVariables.topMost.ToString(),
                        ApplicationVariables.useDefault.ToString(),
                        ApplicationVariables.customPreset.ToString()
                    ]);
                }

                // Presets
                if (!Directory.Exists($"{appdata}\\MintyLauncher\\Presets")) Directory.CreateDirectory($"{appdata}\\MintyLauncher\\Presets"); //Check if there's the presets folder.
                else if (File.Exists($"{appdata}\\MintyLauncher\\Presets\\Default.mlPreset") && ApplicationVariables.useDefault) // Check if there is the Default preset.
                {
                    // Load Default preset. Also add it to the list and select it.
                    loadPresetBox.Items.Add("Default");
                    loadPresetBox.SelectedItem = "Default";
                    LoadPreset($"{appdata}\\MintyLauncher\\Presets\\Default.mlPreset");
                }
            }

            // No AppData folder detected, generate the folder.
            else
            {
                Directory.CreateDirectory($"{appdata}\\MintyLauncher");
                File.WriteAllLines($"{appdata}\\MintyLauncher\\settings.txt", [ ApplicationVariables.rpc.ToString(),
                    ApplicationVariables.closeOnPlay.ToString(),
                    ApplicationVariables.topMost.ToString(),
                    ApplicationVariables.useDefault.ToString(),
                    ApplicationVariables.customPreset.ToString()
                ]);
                // Prompt user to select IWADs folder path.
                var error = MessageBox.Show("IWADs Folder path missing.\nSet new path now?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (error == DialogResult.Yes) new Settings().SetIWADFolder(sender, e);
                else if (error == DialogResult.No) MessageBox.Show("Please set new path in settings.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.CreateDirectory($"{appdata}\\MintyLauncher\\Presets");
            }

            // Tooltips
            complevelSelector.SelectedIndex = 0;
            toolTips.SetToolTip(iwadBox, "Select your desired IWAD. You can change the IWAD folder in Settings.");
            toolTips.SetToolTip(pwadManagerButton, "Open the External File Manager.");
            toolTips.SetToolTip(complevelSelector, "Used to emulate a specific engine. Only works for ports that support the -complevel paremeter.");
            toolTips.SetToolTip(extraParamsTextBox, "Add in any extra parameters you may want. Right click to add a file path to the end of the text.");

            // Command Line Arguments
            bool noGUI = false;
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
                            LoadPreset($"{appdata}\\MintyLauncher\\Presets\\{args[i]}.mlPreset");
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