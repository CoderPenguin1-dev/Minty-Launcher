using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doom_Loader;

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
            "crispy-doom.exe;Crispy Doom\n" +
            "chocolate-doom.exe;Chocolate Doom\n" +
            "minty-doom.exe;Minty Doom\n" +
            "rum-and-raisin-doom.exe;R&&R Doom\n" +
            "pooch.exe;Pooch\n" +
            "prboom.exe;PrBoom\n" +
            "glboom.exe;GLBoom\n" +
            "woof.exe;Woof!\n" +
            "dsda-doom.exe;DSDA-Doom\n" +
            "nugget-doom.exe;Nugget Doom\n" +
            "prboom-plus.exe;PrBoom+\n" +
            "nyan-doom.exe;Nyan Doom\n" +
            "doom.exe;KEX Port\n" +
            "zdoom.exe;ZDoom\n" +
            "gzdoom.exe;GZDoom\n" +
            "lzdoom.exe;LZDoom\n" +
            "vkdoom.exe;VKDoom\n" +
            "zandronum.exe;Zandronum\n" +
            "zdaemon.exe;ZDaemon\n" +
            "odamex.exe;Odamex\n" +
            "doomretro.exe;Doom Retro\n" +
            "EDGE.exe;3DGE/EDGE\n" +
            "Doomsday.exe;Doomsday Engine");
    }

    public static void Complevel(string path)
    {
        File.WriteAllText($"{path}{ApplicationVariables.COMPLEVEL_FILE}",
            "Doom v1.9;2\n" +
            "Ultimate Doom;3\n" +
            "Final Doom;4\n" +
            "Boom v2.02;9\n" +
            "MBF;11\n" +
            "MBF21;21\n"
        );
    }
}