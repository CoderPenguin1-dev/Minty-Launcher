namespace Doom_Loader;

internal static class ApplicationVariables
{
    internal static string presetName = string.Empty; // Used for the SavePreset window to change the Load Preset ComboBox selected item.
    internal static string sourcePort = string.Empty;
    internal static string[] externalFiles = [];
    internal static string iwad = string.Empty;
    internal static string arguments = "";
    internal static string complevel;

    internal const string SETTINGS_FILE = "mintyLauncher.settings";
    internal const string PORTDATABASE_FILE = "mintyLauncher.portDatabase";
    internal const string COMPLEVEL_FILE = "mintyLauncher.complevel";
    internal const string PRESET_EXTENSION = ".mlPreset";

    /// <summary>
    /// // The maximum amount for the "RPC Files Shown" setting
    /// If you want to change the max for this setting, do it here, NOT the Settings Designer.
    /// </summary>
    internal const int MAX_RPC_FILES_SHOWN = 8;

    // Settings
    internal static string iwadFolderPath = string.Empty;
    internal static bool closeOnPlay = false;
    internal static bool rpc = true;
    internal static bool useSourcePortDirectory = true;
    internal static bool useDefault = true;
    internal static bool customPreset = false;
    internal static int rpcFilesShown = 2;
}
