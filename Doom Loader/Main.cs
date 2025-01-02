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
                ApplicationVariables.exe = "\"" + sourcePortDialog.FileName + "\"";

                bool dataFound = false;
                if (File.Exists("mintyLauncher.portDatabase"))
                {
                    foreach (string portData in File.ReadAllLines("mintyLauncher.portDatabase"))
                    {
                        if (portData.StartsWith('#')) continue; // Check for comment lines.
                        string[] data = portData.Split(';');
                        if (Path.GetFileName(sourcePortDialog.FileName) == data[0])
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

        private void Play(object sender, EventArgs e)
        {
            #region Discord RPC
            if (ApplicationVariables.rcp)
            {
                RPCClient.Initialize();
                // State Setup
                string state;
                switch (ApplicationVariables.PWAD.Length)
                {
                    case 0:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)}";
                        break;
                    case 1:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | {Path.GetFileName(ApplicationVariables.PWAD[0])}";
                        break;
                    default:
                        state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | Multiple Files";
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
                        LargeImageKey = "icon"
                    }
                });
            }
            #endregion

            ProcessStartInfo startInfo = new(ApplicationVariables.exe)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            // Argument Setup
            string start = "";

            if (extraParamsTextBox.Text != "") start += ApplicationVariables.arguments + " "; // Extra Paramaters
            if (ApplicationVariables.complevel != 0) start += $"-complevel {ApplicationVariables.complevel} "; // Complevel
            // Check if PWAD was selected
            if (ApplicationVariables.PWAD.Length != 0)
            {
                start += $"-iwad \"{ApplicationVariables.IWAD}\" -file ";
                foreach (string pwad in ApplicationVariables.PWAD) start += $"\"{pwad}\" ";
            }
            else start += $"-iwad \"{ApplicationVariables.IWAD}\"";

            startInfo.Arguments = start; // Put Arguments Into startInfo

            // Start Process
            try
            {
                // Code from some Stack Overflow answer. Unable to find it, however.
                // Start the process with the info specified.
                // Call WaitForExit and then the using statement will close.
                using Process exeProcess = Process.Start(startInfo);
                exeProcess.WaitForExit();
                if (ApplicationVariables.closeOnPlay)
                {
                    if (ApplicationVariables.rcp) RPCClient.client.Dispose(); // Kill RPC Connection
                    Environment.Exit(0);
                }
            }
            catch { }
            if (ApplicationVariables.rcp) RPCClient.client.Dispose(); // Kill RPC Connection

            // Move window visible above all windows
            if (ApplicationVariables.topMost)
            {
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        #region Presets
        private void LoadPreset(object sender, EventArgs e)
        {
            string path;

            if (!ApplicationVariables.customPreset || ApplicationVariables.useDefault && boot)
            {
                path = $"%appdata%\\MintyLauncher\\Presets\\{loadPresetBox.SelectedItem}.mlPreset";
                path = Environment.ExpandEnvironmentVariables(path);
                boot = false;
            }
            else if (openPresetDialog.ShowDialog() != DialogResult.Cancel)
            {
                path = openPresetDialog.FileName;
            }
            else
            {
                return; // Should not end here under normal usage, hopefully
            }

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
            ApplicationVariables.exe = Regex.Replace(args[1], @"[^\w\\.@: -]", string.Empty);
            FileInfo port = new(ApplicationVariables.exe);
            string directory = port.Directory.ToString();
            bool dataFound = false;
            if (File.Exists("mintyLauncher.portDatabase"))
            {
                foreach (string portData in File.ReadAllLines("mintyLauncher.portDatabase"))
                {
                    if (portData.StartsWith('#')) continue; // Check for comments
                    string[] data = portData.Split(';');
                    if (Path.GetFileName(ApplicationVariables.exe) == data[0])
                    {
                        portButton.Text = $"{data[1]}";
                        dataFound = true;
                        break;
                    }
                }
            }
            if (!dataFound) portButton.Text = Path.GetFileNameWithoutExtension(ApplicationVariables.exe);
            #endregion

            // PWAD
            ApplicationVariables.PWAD = Array.Empty<string>();
            if (args.Length == 4)
            {
                ApplicationVariables.PWAD = args[3].Split(',');
                if (ApplicationVariables.PWAD.Length == 1) // I can't honestly remember why this if statement was put into here.
                {                                         // Will not remove to prevent it possibly breaking like a Jenga tower.
                    ApplicationVariables.PWAD[0] = args[3];
                }
            }
        }

        private void LoadCustomPreset(object sender, EventArgs e)
        {
            LoadPreset(sender, e);
        }

        // If changes are made here, also adapt changes to SavePreset.cs' save button function
        private void SavePreset(object sender, EventArgs e)
        {
            if (ApplicationVariables.customPreset)
            {
                if (savePresetDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string path = savePresetDialog.FileName;
                    path = Environment.ExpandEnvironmentVariables(path);

                    string file = @"";
                    file += $"{ApplicationVariables.arguments}\n";
                    file += $"{ApplicationVariables.exe}\n";
                    file += $"{ApplicationVariables.complevel}\n";

                    if (ApplicationVariables.PWAD.Length != 0)
                    {
                        if (ApplicationVariables.PWAD.Length == 1) file += $"{ApplicationVariables.PWAD[0]}";
                        else
                        {
                            for (int i = 0; i < ApplicationVariables.PWAD.Length - 1; i++)
                                file += $"{ApplicationVariables.PWAD[i]},";
                            file += ApplicationVariables.PWAD[^1];
                        }
                    }
                    File.WriteAllText(path, file);
                }
            }
            else new SavePreset().ShowDialog();
        }
        #endregion

        private void ExtraParamsChanged(object sender, EventArgs e)
        {
            ApplicationVariables.arguments = extraParamsTextBox.Text;
        }

        // Update the time since started for the Discord RPC.
        private void UpdateRPCTimestamp(object sender, EventArgs e)
        {
            RPCClient.client.UpdateStartTime();
        }

        private void AppDataInit(object sender, EventArgs e) // Loads in the settings. It also initalizes the complevel selector and tooltips.
        {
            string appdata = "%appdata%";
            appdata = Environment.ExpandEnvironmentVariables(appdata);

            // Create the Port Database file, usually only done on first run.
            if (!File.Exists("mintyLauncher.portDatabase"))
                File.WriteAllText("mintyLauncher.portDatabase", "# This is the Port Database file for The Minty Launcher" +
                    "\n# To create a new entry, do this for each port (one port per line!): [port filename].exe;[port name]" +
                    "\n# Example: gzdoom.exe;GZDoom" +
                    "\n# You can download official database updates on the Google Drive");

            if (Directory.Exists($"{appdata}\\MintyLauncher") || File.Exists("mintyLauncher.PortableSettings"))
            {
                // Settings
                try
                {
                    string[] lines = Array.Empty<string>();
                    if (File.Exists("mintyLauncher.PortableSettings")) 
                        lines = File.ReadAllLines("mintyLauncher.PortableSettings");
                    else lines = File.ReadAllLines($"{appdata}\\MintyLauncher\\settings.txt");

                    // Set the settings.
                    ApplicationVariables.rcp = bool.Parse(lines[0]);
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
                }
                catch
                {
                    File.WriteAllLines($"{appdata}\\MintyLauncher\\settings.txt", new string[] { ApplicationVariables.rcp.ToString(),
                        ApplicationVariables.closeOnPlay.ToString(),
                        ApplicationVariables.topMost.ToString(),
                        ApplicationVariables.useDefault.ToString(),
                        ApplicationVariables.customPreset.ToString()
                    });
                }

                // Presets
                if (!Directory.Exists($"{appdata}\\MintyLauncher\\Presets")) Directory.CreateDirectory($"{appdata}\\MintyLauncher\\Presets");
                else if (File.Exists($"{appdata}\\MintyLauncher\\Presets\\Default.mlPreset") && ApplicationVariables.useDefault) // Check if there is the Default preset.
                {
                    // Little hack to make loading the default preset work. Why did I do this?
                    loadPresetBox.Items.Add("Default");
                    loadPresetBox.SelectedItem = "Default";
                    LoadPreset(sender, e);
                }
            }
            else
            {
                Directory.CreateDirectory($"{appdata}\\MintyLauncher");
                File.WriteAllLines($"{appdata}\\MintyLauncher\\settings.txt", new string[] { ApplicationVariables.rcp.ToString(),
                    ApplicationVariables.closeOnPlay.ToString(),
                    ApplicationVariables.topMost.ToString(),
                    ApplicationVariables.useDefault.ToString(),
                    ApplicationVariables.customPreset.ToString()
                });
                Directory.CreateDirectory($"{appdata}\\MintyLauncher\\Presets");
            }

            // Tooltips
            complevelSelector.SelectedIndex = 0;
            toolTips.SetToolTip(iwadBox, "Change the IWAD folder in Settings");
            toolTips.SetToolTip(pwadManagerButton, "External File Manager");
            toolTips.SetToolTip(complevelSelector, "For ports that have compatibilty levels");
            toolTips.SetToolTip(extraParamsTextBox, "Right click to import file path to the end of the textbox");
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

        private void extraParamsTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (fileImport.ShowDialog() != DialogResult.Cancel)
                {
                    extraParamsTextBox.AppendText($"\"{fileImport.FileName}\"");
                }
            }
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
                    if (Path.GetExtension(wad).ToLower() == ".wad" || Path.GetExtension(wad).ToLower() == ".ipk3")
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
                if (error == DialogResult.Yes) new Settings().button3_Click(sender, e);
                else if (error == DialogResult.No) MessageBox.Show("Please set new path in settings.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}