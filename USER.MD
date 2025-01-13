# **The Minty Launcher**

Minty Launcher is a Doom launcher for Windows (tested on 10/11) that was made for my own wants and needs. I have decided to publish it and any updates I make to it as an alternative to launchers like ZDL or Doom Launcher. 

Want source code? [Here ya go!](https://github.com/PENGUINCODER1/Minty-Launcher)


# Features

* An easy-to-use External File Manager (as of 2.0.0)
* Preset Saving and Loading
    * Presets save
        * Complevel
        * PWADs
        * Extra Parameters
        * Sourceport used
    * Default Preset Support (as of 3.1.0)
        * Just name a preset “Default” in your Roaming AppData folder when saving and make sure the ***“Use ‘Default’ as default preset”*** option in the ***Settings ***menu is enabled!
* Discord Rich Presence (as of 2.5.0)
    * Shows
        * Sourceport used
        * IWAD and PWAD
            * Shows the PWAD filename if only one is loaded in
            * Shows “Multiple PWADs” if more than one are loaded in at once
        * Time elapsed
* The Port Database (as of 3.9.0)
    * Allows you to change the visual name of the port you are using in both the RPC and on the *“Select Port” *button (as of 3.10.0).


# Setting Up & Playing

For starters, if you chose the **Framework Dependent** option when downloading, you'll need to install the .NET 9.0 runtimes onto your system.

The setup process is overall simple. However, where the IWADs go is different depending on what version you’re using.


### **For Minty Launcher v4 and above**

Go into ***Settings* **and select the “*Set IWAD Folder” *button. In the dialog, select the folder your IWADs are in.


### **For Minty Launcher v3 and below**

Put your IWADs into the *same folder as your sourceport’s executable* so the launcher can detect the IWADs when you select the port.


### **Playing**

To launch into a game, open up Minty Launcher and press *“Select Port”* and navigate to where you have your port installed to and select the port’s executable. You then have to select which IWAD to use with the **IWAD List**, once you have done that, you may add in any maps/mods you wish to use by pressing the “*External Files”* button and using the controls in the manager. You can then hit the *“Play”* button.


# Descriptions of Each Setting

## Presets
* **Use “Default” as default preset**
    * If you have this setting enabled *and* have a preset saved in your Roaming AppData folder (the default place for presets) called “Default”, Minty Launcher will load that preset on startup.
        * **Requires restart of Minty Launcher to take effect.**


## Portability
* **Ask For Preset Location**
    * Mimics how older versions of Minty Launcher worked with presets.
        * Replaces the Preset List with a “Load Preset Button” which when clicked, asks you where the preset file is located. It replaces the “Save Preset” window with a similar dialog to what the new button shows.
        * **Requires restart of Minty Launcher to take effect.**
* **Make Settings Portable**
    * When clicked, it produces a “.Minty LauncherPortableSettings” file that replaces the settings file in your Minty Launcher Roaming AppData folder. This file *must* be in the same folder as the executable to be detected. Settings are both saved and read here.
        * The setting can be reversed by deleting, renaming, or moving the file.

## Window
* **Close On Play**
    * Despite its name, the setting closes the launcher after the port has been shut down.
* **Show Window After Quit**
    * Makes Minty Launcher the top window after the port has been shut down.
	
## Ungrouped
* **Discord Rich Presence **
    * Enables/Disables support for Discord RPC. Details on what it shows on your Discord profile are shown in Features.


# Versioning
The versioning of Minty Launcher is a **major.minor.patch.prerelease** system.

* **Major** mainly being massive overhauls in UI, compatibility-breaking feature additions, or what I consider to be major additions (like the addition of the PWAD Manager menu)

* **Minor** consisting of standard updates (like the addition of Discord Rich Presence)

* **Patch** being mainly bug fixes or rarely used to indicate very small updates

* **Prerelease** being an indicator if a version was meant to be shared for testing purposes before release. 0 meaning it’s a release version, 1 being a prerelease.