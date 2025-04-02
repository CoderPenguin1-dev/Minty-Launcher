using System.Collections.Generic;

public static class ApplicationVariables
{
    public static string presetName = string.Empty; // Used for the SavePreset window to change the Load Preset ComboBox selected item.
    public static string sourcePort = string.Empty;
    public static string[] externalFiles = [];
    public static string IWAD = string.Empty;
    public static string arguments = "";
    public static string complevel;
    public static int complevelIndex = 0;

    public const string SETTINGS_FILE = "mintyLauncher.settings";
    public const string PORTDATABASE_FILE = "mintyLauncher.portDatabase";
    public const string COMPLEVEL_FILE = "mintyLauncher.complevel";

    public static string[] complevels = [];

    // Settings
    public static string IWADFolderPath = string.Empty;
    public static bool closeOnPlay = false;
    public static bool rpc = true;
    public static bool topMost = false;
    public static bool useDefault = true;
    public static bool customPreset = false;
    public static int rpcFilesShown = 2;
}
