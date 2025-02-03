<img src="Images/logo.png" width="250" height="250">

# The Minty Launcher
A C# WinForms Doom launcher I made as I hadn't liked others like ZDL or Doom Launcher.
This is build for my purposes and features will usually be added when I need/want them. I will, however, gladly take suggestions and implement them if possible within my lackluster skill level.

Minty Launcher only targets Windows, and only has been tested on Windows 10 and 11. Minty Launcher through Wine has mainly been untested.

## Useful Links
* [Downloads](https://github.com/PENGUINCODER1/Minty-Launcher/releases)
* [User's Guide](USER.MD)
* [DYNCOS License](LICENSE.MD)

# Downloading
You can also get Win64 and Win32 bins (both Framework-Dependant and Standalone) as well as the premade port database file [here on the repo](https://github.com/PENGUINCODER1/Minty-Launcher/releases). 

Note that there's a user manual you should read, titled `USER.MD`.


# Building
No dependencies are required besides [DiscordRichPresence](https://github.com/Lachee/discord-rpc-csharp) from NuGet and its dependencies as well as WinForms and .NET 9.

You can clone the repo by running the following.

	git clone https://github.com/PENGUINCODER1/Minty-Launcher.git

If you wish to replace the RPC ID, it's located in the `Doom Loader\Properties\Resources.resx` file.

Build Debug build with the following in the `Minty-Launcher/Doom Loader` folder.

	dotnet run

You should now have a sucessful build afterwards!

If you want a more streamline script to follow, you *could* also copy and run this.

	git clone https://github.com/PENGUINCODER1/Minty-Launcher.git && cd "Minty-Launcher/Doom Loader" && dotnet run