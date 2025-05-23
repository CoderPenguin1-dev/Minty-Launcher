# Minty Launcher User's Guide

You can obtain source code for Minty Launcher [here](https://github.com/PENGUINCODER1/Minty-Launcher).

To avoid confusion, please read this whole document before opening Minty Launcher for the first time.

Note this manual is updated to the latest commit, not the latest release. Information here may be different than the version you downloaded.

## 0. Definitions
Just a collection of simple definitions you may see throughout here, the launcher, or in other places relating to Doom.

**IWADS:** Your game WADs. Some examples include `DOOM.WAD` and `DOOM2.WAD`.

**PWADS:** Maps, mods, etc. Same file extension as IWADs, so be aware of the differences.

**Total Conversions (TCs):** A PWAD that completely replaces every sound, texture, and map, and possibly modifies the behaviour of the original IWADs.

**Complevel:** Usually indicated by a number, it's a parameter used in some source ports to emulate other source ports.

**Settings Preset:** Any Minty Launcher preset stored in your Settings folder.

**Settings Folder:** Either `%appdata\MintyLauncher` or `MintyLauncher` depending on if you've made your settings portable or not.

## 1. Setting Up
For Framework Dependent Only: Make sure you've installed the .NET 9.0 Runtimes before running.

You'll need to set where your IWADs folder is. It will usually prompt you to do so after opening Minty Launcher for the first time, but if it doesn't (or you wish to change it later), do one of these methods:
* Click on the *IWAD Selector Box.* (Use this if you deleted/moved a folder or if you opened the program for the first time.)
* Go into *Settings* and clicking on *Set IWADs Folder.* (Use this if you just wish to change the folder.)

With either method, you'll need to navigate to where the folder containing your IWADs is and then select that folder.

Then, you'll need to select a source port. On the main window, click on *Select Port* and navigate to the executable file of your desired port.
Now, select your desired IWAD and complevel and hit *Play* and it should boot you into the game!

## 2. The Interface

### 2a. The Main Window
The main window has several buttons and hidden gimmicks so it may be confusing at first.

The IWAD and Complevel selectors allow you to select which IWAD and source port you wish to emulate. Please note the latter is only usable in ports that support such features like DSDA-Doom.

The *Extra Parameters* box allows you to type in any extra parameters you wish to pass along to the source port, such as `-playdemo` to play a recorded demo.
You can also right-click the box to insert a file path easier.
You can also insert an `*` into the box to insert the alternate directory (details below). Example: `-record */demos/demo`.

*Use Source Port As CWD* Setting **ON**: It will use Minty Launcher's folder.

*Use Source Port As CWD* Setting **OFF**: It will use the source port's folder.

The *Select Port* button allows you to select which port you wish to use.
You can also right-click the button to give all information about the currently selected port. This includes the file path of the port.

The *External Files* button opens up the External File Manager window, which allows you to sort load orders of maps, mods, and dehacked/bex patches.

*Ask For Preset Location* Setting **OFF**: The Selector Box will allow you to quickly select from your saved presets.

*Ask For Preset Location* Setting **ON**: The *Load Preset* button will allow you to load in a preset.

The *Save Preset* button will allow you to save a preset. If the *Ask For Preset Location* setting is **OFF**, it will pop up with a small box that'll allow you to put in a preset name. These are saved in your Settings Folder.
If the *Ask For Preset Location* setting is **OFF**, you can right-click the button to quickly save the current preset.

### 2b. The External File Manager
You can drag files onto this window to quickly add them to your External File List. You can also hit the **+** button to add in an external file to the List as well.

If you need to see the file paths of each file, you can right-click the External File List. Selecting no files will show every file path. Selecting multiple files will show only those file paths.

You can select one or multiple file(s) to delete them with the **-** button.

You can reorder single files in the list with the Up and Down arrow buttons.

Note: The placement of `.deh` and `.bex` patches don't matter too much. They are always loaded before PWADs.
They're, however, loaded in the order they appear in the list.

## 3. Settings Overview
This covers what each setting in the Settings window does.

### 3a. Presets
These are settings relating to presets.

*Load "Default" Preset on launch*: 

	This will check for a preset named "Default" and load it automatically. It will only check the Settings Folder presets.
	
### 3b. Portability
These are settings related to being able to more easily use the same executable across multiple computers, like on a thumb drive.

*Ask For Preset Location*:

	This will replace the Load Preset Box with a button instead. Both the Load and Save buttons will pop up with a file dialog instead.

*Use Source Port As CWD*:

	This will use the source port's folder as the working directory when launching it. 
	This will also have any `*` in the Extra Parameters textbox insert Minty Launcher's folder when this setting is one. It will use the Source Port's folder when it's off.
	Recommended to keep this on.

*Make Settings Portable*:
	
	This will move all files from your appdata to a folder called MintyLauncher. The launcher will then read from this folder instead of appdata.
	
### 3c. Discord Rich Presence

*Enable RPC Integration*
	
	Enables the Rich Presence integration of Minty Launcher. 
	This will show information such as your source port, IWAD, and loaded files like PWADs.

*Amount Of Files Shown*

	The limit to how many individual file files can be shown on the Discord Activity. If it exceeds the limit, it will show "+ x more" at the end.
	Setting it to 0 disables this functionality.
	
### 3d. Ungrouped
	
*Set IWADs Folder*:

	This will allow you to change where your IWADs folder is located.
	
*Close On Play*:

	This will close the launcher after the source port has been closed.

## 4. Settings Files
You can access your settings files (as well as presets) by going into `Settings > Open Settings Folder`

### 4a. Port Database
Each entry is in this format: `source port file name`;`source port actual name`.
For example, GZDoom would be `gzdoom.exe;GZDoom`.
Entry placement and the input for the source port's actual name doesn't matter.
If your source port's actual name contains a &, double it up `&&`.

When you select a source port (or open a preset that contains a source port), it will first look through this file.
If an entry matches the source port executable name, it will replace the source port button and the Discord Activity source port description with the source port's actual name.

### 4b. Complevel
Each entry is in this format: `complevel name`;`complevel number`.
For example, Doom 2 would be `Doom v1.9;2`.
Entry placement dictates which order it will be shown in the complevel selector.

### 4c. Presets
All presets are in the `.mlPreset` file format. The format looks like this internally:
```
Parameters
Source Port Path
Complevel Number (or - if None)
File Path(s)
IWAD File Name
```
Each file path is separated by a comma (,).
When an option is left unchanged (with the exception of, complevel), it will remain blank in the preset as well.
	
## 5. Command-line Arguments
Note that all of your settings (and your Default preset, if enabled) are loaded before these are checked.

Note: Single-lettered arguments can't be combined together. They must be separated.

`--preset [Preset Name]` loads in the preset from your Settings Folder. Exclude the file extension from the name.

`--preset-path [Preset Path]` or `-p [Preset Path]` loads in a preset from the desired path. Must be an `.mlPreset` file.

`--iwad [IWAD File Name]` or `-w [IWAD File Name]`: Loads in the IWAD file from your IWADs folder. Must include the file extension.
You can also input "None" to deselect the IWAD from a preset.

`--iwad-folder [IWAD Folder Path]`: Set the IWAD folder path. Call this before `--iwad`.

`--no-gui` or `-n`: Start the selected IWAD and preset without a GUI. Must be called after `--iwad` and `--preset` or `--preset-path` have been called.

`--rpc [On/Off]`: Enables/disables the RPC integration. Must be called after `--no-gui` has been called.
* Valid inputs for enabling it are on, 1, true.
* Valid inputs for disabling are off, 0, false.
* Temporary.

`--rpc-files-shown [Amount Shown, 0-8]`: Description in section 3a.
* Temporary.

`--generate-update-files`: Creates two files, titled `deleteThis.mintyLauncher.portDatabase` and `deleteThis.mintyLauncher.complevel` 
so you can copy newly added information into your actual files in your Settings Folder.
* Will ignore every other argument if you have this, excluding `--info`.

`--info` or `-i`: shows the current version of Minty Launcher. 
* Will ignore every other argument if you have this.

