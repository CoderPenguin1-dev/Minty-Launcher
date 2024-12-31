# Minty Launcher
A C# WinForms Doom launcher I made as I hadn't liked others like ZDL or Doom Launcher.
This is build for my purposes and features will usually be added when I need/want them. I will, however, gladly take suggestions and implement them if possible within my lackluster skill level.

# Downloading
You can get prebuilt, Framework-Dependant Win86 bins from the Google Drive, as well as a premade port database file and the user manual. Due to limitations with WinForms, I am sadly unable to provide native Linux bins.
https://drive.google.com/drive/folders/1WFhlLlC_Ka0N-Fk6tTlCKcxksYwFRNg4?usp=sharing

# Building
No dependencies are required besides DiscordRichPresence (aka discord-rpc-csharp on GitHub) from NuGet and its dependencies as well as WinForms and .NET 6.
Open the solution in Visual Studio 2022.
Create a `Resources.resx` file in the `Properties` folder with a key called `DiscordAPI` and put in your own Discord application ID for its value.

In the Discord Developer Portal, make a new Rich Presence resource titled `icon`.

Build the solution from VS2022 from there.
