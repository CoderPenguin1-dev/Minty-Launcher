using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DiscordRPC;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging.Effects;

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
            // Check if the selected complevel is "None."
            if (complevelSelector.SelectedIndex == 0) ApplicationVariables.complevel = "-";
            else
            {
                string[] complevels = File.ReadAllLines($"{FindMintyLauncherFolder()}{ApplicationVariables.COMPLEVEL_FILE}");
                foreach (string entry in complevels)
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
                bool complevelFound = false;
                string[] complevels = File.ReadAllLines($"{FindMintyLauncherFolder()}{ApplicationVariables.COMPLEVEL_FILE}");
                foreach (string entry in complevels)
                {
                    string[] entryTokens = entry.Split(";");
                    if (entryTokens[1] == ApplicationVariables.complevel)
                    {
                        complevelSelector.SelectedItem = entryTokens[0];
                        complevelFound = true;
                    }
                }
                if (!complevelFound)
                {
                    MessageBox.Show($"Can't Find Complevel {ApplicationVariables.complevel}. Selecting no complevel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ApplicationVariables.complevel = "-";
                    CheckComplevel();
                }
            }
        }

        private void CheckPortDatabase()
        {
            bool dataFound = false;
            string path = FindMintyLauncherFolder();
            if (ApplicationVariables.sourcePort != string.Empty)
            {
                string[] portDatabase = File.ReadAllLines($"{path}{ApplicationVariables.PORTDATABASE_FILE}");
                foreach (string portData in portDatabase)
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
                if (!dataFound) portButton.Text = Path.GetFileNameWithoutExtension(ApplicationVariables.sourcePort);
            }
            else portButton.Text = "Select Port";
        }

        public static string FindMintyLauncherFolder()
        {
            // Try to find the portable settings folder.
            if (Path.Exists("MintyLauncher"))
                return "MintyLauncher\\";
            // If no portable settings folder is found in the CWD,
            // return the AppData folder.
            else
                return Environment.ExpandEnvironmentVariables("%appdata%\\MintyLauncher\\");
        }
        #endregion

        private void IWADChanged(object sender, EventArgs e)
        {
            try
            {
                if (iwadBox.SelectedIndex != 0)
                    ApplicationVariables.IWAD = $"{ApplicationVariables.IWADFolderPath}/{iwadBox.SelectedItem}";
                else ApplicationVariables.IWAD = string.Empty;
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

                // Reset dropdown.
                iwadBox.Items.Clear();
                iwadBox.Items.Add("None");

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

        private static string[] GetComplevels()
        {
            string path = FindMintyLauncherFolder();
            return File.ReadAllLines($"{path}{ApplicationVariables.COMPLEVEL_FILE}");
        }

        private void RefreshComplevels(object sender, EventArgs e)
        {
            // Check if the user already has a complevel selected.
            // Save it for later if they do.
            string complevel = null;
            if (complevelSelector.SelectedItem != null)
                complevel = complevelSelector.SelectedItem.ToString();

            // Reset dropdown.
            complevelSelector.Items.Clear();
            complevelSelector.Items.Add("None");

            string[] complevels = GetComplevels();

            foreach (string entry in complevels)
                complevelSelector.Items.Add(entry.Split(";")[0]);

            if (complevelSelector.Items.Contains(complevel))
                complevelSelector.SelectedItem = complevel;

            // Check for complevel number directly if it can't find it by name.
            else
            {
                bool foundComplevel = false;
                foreach (string entry in complevels)
                {
                    if (entry.Split(";")[1].Equals(ApplicationVariables.complevel))
                    {
                        complevelSelector.SelectedItem = entry.Split(";")[0];
                        foundComplevel = true;
                        break;
                    }
                }
                // Fallback to "None" if it can't find the complevel even after previous check.
                if (!foundComplevel)
                {
                    complevelSelector.SelectedIndex = 0;
                    ApplicationVariables.complevel = "-";
                    MessageBox.Show($"Can't Find Complevel {ApplicationVariables.complevel}. Selecting no complevel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (ApplicationVariables.rpcFilesShown == 0 || ApplicationVariables.externalFiles.Length == 0)
                    state = $"{Path.GetFileName(ApplicationVariables.IWAD)}";
                else
                {
                    state = $"{Path.GetFileName(ApplicationVariables.IWAD)} | ";
                    state += Path.GetFileName(ApplicationVariables.externalFiles[0]);
                    if (ApplicationVariables.externalFiles.Length > 1)
                    {
                        for (int i = 1; i < ApplicationVariables.externalFiles.Length; i++)
                        {
                            if (i == ApplicationVariables.rpcFilesShown) break;
                            state += $", {Path.GetFileName(ApplicationVariables.externalFiles[i])}";
                        }
                    }
                    if (ApplicationVariables.externalFiles.Length > ApplicationVariables.rpcFilesShown)
                        state += $", + {ApplicationVariables.externalFiles.Length - ApplicationVariables.rpcFilesShown} more";
                }

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
            string replaceWithDirectory;
            if (ApplicationVariables.useSourcePortDirectory) replaceWithDirectory = Environment.CurrentDirectory;
            else replaceWithDirectory = Path.GetDirectoryName(ApplicationVariables.sourcePort);


            // Extra Paramaters
            if (extraParamsTextBox.Text != "")
            {
                portArguments += ApplicationVariables.arguments
                    .Replace("*", replaceWithDirectory) // Check if there's any Minty CWD characters.
                    .Replace('\\', '/') + " "; // Replace all back-slashes with forward-slashes to prevent bug where it'll think it's an escape character.
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
                string workingDirectory;
                if (ApplicationVariables.useSourcePortDirectory) workingDirectory = Path.GetDirectoryName(ApplicationVariables.sourcePort);
                else workingDirectory = "";
                Process.Start(
                    new ProcessStartInfo(ApplicationVariables.sourcePort, portArguments)
                    {
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        WorkingDirectory = workingDirectory
                    }).WaitForExit();
                if (ApplicationVariables.closeOnPlay)
                {
                    if (ApplicationVariables.rpc) RPCClient.client.Dispose(); // Kill RPC Connection
                    Environment.Exit(0);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (ApplicationVariables.rpc) RPCClient.client.Dispose(); // Kill RPC Connection.
        }

        #region Presets
        private void LoadPreset(string path)
        {
            string[] args = File.ReadAllLines(path);

            // Complevel
            ApplicationVariables.complevel = args[2];
            CheckComplevel();

            extraParamsTextBox.Text = args[0]; // Arguments

            #region Sourceport and IWADs
            ApplicationVariables.sourcePort = Regex.Replace(args[1], @"[^\w\\.@: -]", string.Empty);
            CheckPortDatabase();
            if (args.Length == 5)
            {
                RefreshIWAD(new object(), new EventArgs());
                int iwadIndex = -1;
                for (int i = 0; i < iwadBox.Items.Count; i++)
                {
                    if (iwadBox.Items[i].ToString().Equals(args[4], StringComparison.CurrentCultureIgnoreCase))
                    {
                        iwadIndex = i;
                        break;
                    }
                }
                if (iwadIndex != -1)
                {
                    iwadBox.SelectedIndex = iwadIndex;
                    ApplicationVariables.IWAD = ApplicationVariables.IWADFolderPath + "\\" + args[4];
                }
                else
                {
                    iwadBox.SelectedIndex = 0;
                    ApplicationVariables.IWAD = string.Empty;
                }
            }
            else
            {
                iwadBox.SelectedIndex = 0;
                ApplicationVariables.IWAD = string.Empty;
            }
            #endregion

            // PWAD
            ApplicationVariables.externalFiles = [];
            if (args.Length >= 4)
            {
                if (args[3] != string.Empty)
                {
                    ApplicationVariables.externalFiles = args[3].Split(',');
                    if (ApplicationVariables.externalFiles.Length == 1) // I can't honestly remember why this if statement was put into here.
                    {                                                   // Will not remove to prevent it possibly breaking like a Jenga tower.
                        ApplicationVariables.externalFiles[0] = args[3];
                    }
                }
            }
        }

        private void LoadPresetComboBox(object sender, EventArgs e)
        {
            string path;

            if (!ApplicationVariables.customPreset || ApplicationVariables.useDefault && boot)
            {
                path = $"{FindMintyLauncherFolder()}Presets\\{loadPresetBox.SelectedItem}{ApplicationVariables.PRESET_EXTENSION}";
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
                file += "\n";
            }
            else file += "\n";

            if (ApplicationVariables.IWAD != null)
                file += Path.GetFileName(ApplicationVariables.IWAD);
            File.WriteAllText(path, file);
        }

        private void QuickSavePreset(object sender, MouseEventArgs e)
        {
            if (!ApplicationVariables.customPreset && e.Button == MouseButtons.Right)
            {
                if ((string)loadPresetBox.SelectedItem != null)
                {
                    string path = FindMintyLauncherFolder() + "Presets\\" + loadPresetBox.SelectedItem + ApplicationVariables.PRESET_EXTENSION;
                    SavePreset(path);
                    MessageBox.Show("Preset Saved", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No preset currently selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (preset.EndsWith(ApplicationVariables.PRESET_EXTENSION))
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

        // Checks to see if the play button should be active.
        private void UpdatePlayButton(object sender, EventArgs e)
        {
            if (portButton.Text != "Select Port" && ApplicationVariables.IWAD != string.Empty)
                playButton.Enabled = true;
            else playButton.Enabled = false;
        }

        // Settings, init complevel ComboBox and tooltips, and check command line arguments.
        // For --info and -i, check Program.cs
        private void AppDataInit(object sender, EventArgs e)
        {
            playButtonUpdateTimer.Start();
            iwadBox.Items.Add("None");
            iwadBox.SelectedIndex = 0;
            complevelSelector.Items.Add("None");
            complevelSelector.SelectedIndex = 0;
            string appdata = Environment.ExpandEnvironmentVariables("%appdata%\\");

            // Create Minty Launcher folder if neither the appdata or local settings folders exist.
            if (!Path.Exists($"{appdata}MintyLauncher\\") && !Path.Exists("MintyLauncher\\"))
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

            #region Convert Old Files (v5.x.x to v6.0.0)
            if (File.Exists(path + "settings.txt"))
            {
                var result = MessageBox.Show("Old Minty Launcher AppData folder detected." +
                    "\nDo you want to convert old Minty Launcher files?\n" +
                    "These files include your settings and presets." +
                    "\nYou'll lose no data, but these files will become incompatible with older versions." +
                    "\nYou must do this to use this version of Minty Launcher with your old settings and presets.",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    File.Copy($"{path}settings.txt", $"{path}{ApplicationVariables.SETTINGS_FILE}");
                    File.Delete($"{path}settings.txt");

                    // Convert old preset files.
                    DirectoryInfo presetsFolder = new(path + "Presets");
                    foreach (var file in presetsFolder.GetFiles())
                    {
                        string[] preset = File.ReadAllLines(file.FullName);
                        if (preset[2] == "0")
                        {
                            preset[2] = "-";
                            File.WriteAllLines(file.FullName, preset);
                        }
                    }

                    Generate.Complevel(path);

                    if (File.Exists(ApplicationVariables.PORTDATABASE_FILE))
                    {
                        result = MessageBox.Show("Do you want to move over your Port Database file?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            File.Copy(ApplicationVariables.PORTDATABASE_FILE, $"{path}{ApplicationVariables.PORTDATABASE_FILE}");
                            File.Delete(ApplicationVariables.PORTDATABASE_FILE);
                        }
                        else
                        {
                            Generate.PortDatabase(path);
                            // For some reason if this file exists, the complevel file can't be read. Don't ask me why.
                            // I can't even reproduce it, either.
                            File.Delete(ApplicationVariables.PORTDATABASE_FILE);
                        }
                    }
                    else Generate.PortDatabase(path);
                    MessageBox.Show("Conversion Complete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else Environment.Exit(0);
            }
            #endregion

            string[] settings = File.ReadAllLines($"{path}{ApplicationVariables.SETTINGS_FILE}");

            #region Settings
            ApplicationVariables.rpc = bool.Parse(settings[0]);
            ApplicationVariables.closeOnPlay = bool.Parse(settings[1]);
            ApplicationVariables.useSourcePortDirectory = bool.Parse(settings[2]);
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
            string[] complevels = GetComplevels();
            foreach (string complevel in complevels)
                complevelSelector.Items.Add(complevel.Split(";")[0]);
            complevelSelector.SelectedIndex = 0;
            #endregion

            // Check if there is the Default preset.
            if (File.Exists($"{path}Presets\\Default{ApplicationVariables.PRESET_EXTENSION}") && ApplicationVariables.useDefault)
            {
                // Load Default preset. Also add it to the list and select it.
                loadPresetBox.Items.Add("Default");
                loadPresetBox.SelectedItem = "Default";
                LoadPreset($"{path}Presets\\Default{ApplicationVariables.PRESET_EXTENSION}");
            }

            #region Tooltips
            toolTips.SetToolTip(iwadBox, "Select your desired IWAD.\nYou can change the IWAD folder in Settings.");
            toolTips.SetToolTip(pwadManagerButton, "Open the External File Manager.");
            toolTips.SetToolTip(complevelSelector, "Used to emulate a specific engine.\nOnly works for ports that support the -complevel parameter.");
            toolTips.SetToolTip(extraParamsTextBox, "Add in any extra parameters you may want.\nRight click to add a file path to the end of the text.");
            if (!ApplicationVariables.customPreset)
                toolTips.SetToolTip(savePresetButton, "Right click to quick-save currently selected preset.");
            #endregion

            // Command Line Arguments
            bool noGUI = false;
            bool usedIWAD = false;
            bool usedPreset = false;
            if (Environment.GetCommandLineArgs().Length > 0)
            {
                string[] args = Environment.GetCommandLineArgs();
                for (int i = 0; i < args.Length; i++)
                {
                    // Anything with i++ takes in an argument of its own. Put there to skip the argument on the next pass.
                    switch (args[i])
                    {
                        case "--preset-path":
                            i++;
                            loadPresetBox.Items.Add("External Preset");
                            loadPresetBox.SelectedItem = "External Preset";
                            LoadPreset(args[i]);
                            if (ApplicationVariables.IWAD != string.Empty) usedIWAD = true;
                            usedPreset = true;
                            break;

                        case "--generate-update-files": // May move into Program.cs at some point.
                            Generate.Complevel("deleteThis.");
                            Generate.PortDatabase("deleteThis.");

                            MessageBox.Show("Generated files.\n" +
                                "Please copy what you need from the `deleteThis.` files and put them in your actual files in your settings folder.\n" +
                                "Afterwards, please delete the `deleteThis.` files.",
                                "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Environment.Exit(0);
                            break;

                        case "--preset" or "-p":
                            i++;
                            RefreshPresetBox(sender, e);
                            int presetIndex = -1;
                            for (int x = 0; x < loadPresetBox.Items.Count; x++)
                            {
                                if (loadPresetBox.Items[x].ToString().Equals(args[i], StringComparison.CurrentCultureIgnoreCase))
                                {
                                    presetIndex = x;
                                    break;
                                }
                            }
                            if (presetIndex == -1)
                            {
                                MessageBox.Show("Given preset does not exist. Check to see if your capitalization is wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            loadPresetBox.SelectedIndex = presetIndex;
                            LoadPreset($"{path}Presets\\{loadPresetBox.SelectedItem}{ApplicationVariables.PRESET_EXTENSION}");
                            if (ApplicationVariables.IWAD != string.Empty) usedIWAD = true;
                            usedPreset = true;
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

                            int iwadIndex = -1;
                            // Why is x at 0? Well, I wanted the user to be able to specify "None" to outright tell a preset to not use an IWAD.
                            for (int x = 0; i < iwadBox.Items.Count; x++)
                            {
                                if (iwadBox.Items[x].ToString().Equals(args[i], StringComparison.CurrentCultureIgnoreCase))
                                {
                                    iwadIndex = x;
                                    break;
                                }
                            }
                            if (iwadIndex == -1)
                            {
                                MessageBox.Show("Given IWAD does not exist. Check to see if your capitalization is wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }

                            iwadBox.SelectedIndex = iwadIndex;
                            usedIWAD = true;
                            break;

                        case "--no-gui" or "-n":
                            if (!usedIWAD || !usedPreset)
                            {
                                MessageBox.Show("You must call --iwad and --preset or --preset-path prior to calling --no-gui.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            noGUI = true;
                            break;

                        case "--rpc":
                            i++;
                            if (!noGUI)
                            {
                                MessageBox.Show("--rpc must be called after --no-gui", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
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
                                MessageBox.Show("Invalid input for --rpc. Refer to USER.MD for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            break;

                        case "--rpc-files-shown":
                            i++;
                            if (!noGUI)
                            {
                                MessageBox.Show("--rpc-files-shown must be called after --no-gui", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
                            try
                            {
                                ApplicationVariables.rpcFilesShown = int.Parse(args[i]);
                                if (ApplicationVariables.rpcFilesShown > ApplicationVariables.MAX_RPC_FILES_SHOWN || ApplicationVariables.rpcFilesShown < 0)
                                    throw new Exception();
                            }
                            catch
                            {
                                MessageBox.Show("Invalid input for --rpc-files-shown. Refer to USER.MD for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
                            }
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

        private void PortChanged(object sender, EventArgs e)
        {
            if (portButton.Text == "Select Port")
                toolTips.SetToolTip(portButton, null);
            else
                toolTips.SetToolTip(portButton, portButton.Text + "\nRight click to aee all info.");
        }

        private void ShowFullPortInfo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (ApplicationVariables.sourcePort != string.Empty)
                {
                    MessageBox.Show($"Port Name: {portButton.Text}\n\n" +
                        $"Port Executable: {Path.GetFileName(ApplicationVariables.sourcePort)}\n\n" +
                        $"Port Executable Path: {ApplicationVariables.sourcePort}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No port selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}