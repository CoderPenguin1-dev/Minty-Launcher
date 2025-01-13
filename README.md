![logo](Images/logo.png)
# The Minty Launcher
A C# WinForms Doom launcher I made as I hadn't liked others like ZDL or Doom Launcher.
This is build for my purposes and features will usually be added when I need/want them. I will, however, gladly take suggestions and implement them if possible within my lackluster skill level.

Minty Launcher only targets Windows, and only has been tested on Windows 10 and 11. Minty Launcher through Wine has mainly been untested.

# Downloading
You can also get Win64 and Win32 bins (both Framework-Dependant and Standalone) as well as the premade port database file [here on the repo](https://github.com/PENGUINCODER1/Minty-Launcher/releases). 

Note that there's a user manual you should read, titled `USER.MD`.

# Building
No dependencies are required besides DiscordRichPresence ([GitHub](https://github.com/Lachee/discord-rpc-csharp)) from NuGet and its dependencies as well as WinForms and .NET 9.

Open the solution in Visual Studio 2022.
Create a `Resources.resx` file in the `Properties` folder with a key called `DiscordAPI` and put in your own Discord application ID for its value.

In the Discord Developer Portal, make a new Rich Presence resource titled `icon`.

Build the solution from VS2022 from there.
