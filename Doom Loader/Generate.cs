using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Generate
{
    public static void Settings(string path)
    {
        File.WriteAllLines($"{path}{ApplicationVariables.SETTINGS_FILE}", 
        [ 
            ApplicationVariables.rpc.ToString(),
            ApplicationVariables.closeOnPlay.ToString(),
            ApplicationVariables.topMost.ToString(),
            ApplicationVariables.useDefault.ToString(),
            ApplicationVariables.customPreset.ToString(),
            ApplicationVariables.rpcFilesShown.ToString(),
        ]);
    }

    public static void PortDatabase(string path)
    {
        File.WriteAllText($"{path}{ApplicationVariables.PORTDATABASE_FILE}", 
            "crispy-doom.exe;Crispy Doom\r\n" +
            "chocolate-doom.exe;Chocolate Doom\r\n" +
            "minty-doom.exe;Minty Doom\r\n" +
            "rum-and-raisin-doom.exe;R&&R Doom\r\n" +
            "pooch.exe;Pooch\r\n" +
            "prboom.exe;PrBoom\r\n" +
            "glboom.exe;GLBoom\r\n" +
            "woof.exe;Woof!\r\n" +
            "dsda-doom.exe;DSDA-Doom\r\n" +
            "nugget-doom.exe;Nugget Doom\r\n" +
            "prboom-plus.exe;PrBoom+\r\n" +
            "nyan-doom.exe;Nyan Doom\r\n" +
            "doom.exe;KEX Port\r\n" +
            "zdoom.exe;ZDoom\r\n" +
            "gzdoom.exe;GZDoom\r\n" +
            "lzdoom.exe;LZDoom\r\n" +
            "vkdoom.exe;VKDoom\r\n" +
            "zandronum.exe;Zandronum\r\n" +
            "zdaemon.exe;ZDaemon\r\n" +
            "odamex.exe;Odamex\r\n" +
            "doomretro.exe;Doom Retro\r\n" +
            "EDGE.exe;3DGE/EDGE\r\n" +
            "Doomsday.exe;Doomsday Engine");
    }

    public static void Complevel(string path)
    {
        File.WriteAllText($"{path}{ApplicationVariables.COMPLEVEL_FILE}",
            "Doom v1.9;2\r\n" +
            "Ultimate Doom;3\r\n" +
            "Final Doom;4\r\n" +
            "Boom v2.02;9\r\n" +
            "MBF;11\r\n" +
            "MBF21;21\r\n"
        );
    }
}