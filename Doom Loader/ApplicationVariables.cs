using System.Collections.Generic;

public static class ApplicationVariables
{
    public static string presetFilePath = string.Empty;
    public static string exe = string.Empty;
    public static string[] externalFiles = Array.Empty<string>();
    public static string IWAD = string.Empty;
    public static string IWADFolderPath = string.Empty;
    public static string arguments = "";
    public static int complevel;

    // Settings
    public static bool closeOnPlay = false;
    public static bool rcp = true;
    public static bool topMost = false;
    public static bool useDefault = true;
    public static bool customPreset = false;
}
