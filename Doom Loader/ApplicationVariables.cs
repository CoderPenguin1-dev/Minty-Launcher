using System.Collections.Generic;

public static class ApplicationVariables
{
    public static string presetName = string.Empty; // Used for the SavePreset window to change the Load Preset ComboBox selected item.
    public static string sourcePort = string.Empty;
    public static string[] externalFiles = [];
    public static string IWAD = string.Empty;
    public static string IWADFolderPath = string.Empty;
    public static string arguments = "";
    public static int complevel;

    // Settings
    public static bool closeOnPlay = false;
    public static bool rpc = true;
    public static bool topMost = false;
    public static bool useDefault = true;
    public static bool customPreset = false;
    public static int rpcFilesShown = 4;
}
