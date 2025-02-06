# The Minty Launcher User's Guide

You can obtain source code for Minty Launcher [here](https://github.com/PENGUINCODER1/Minty-Launcher)

## 1. Setting Up
For Framework Dependent Only: Make sure you've installed the .NET 9.0 Runtimes before running.

You'll need to set where your IWADs folder is. To do so, you can either click on the *IWAD Selector Box* or by going into *Settings* and clicking on *Set IWADs Folder.* 
With either method, you'll need to naviage to where the folder containing your IWADs are and then selecting that folder.

Then, you'll need to select a source port. On the main window, click on *Select Port* and navigate to the executable file of your desired port.
Now, select your desired IWAD and complevel and hit *Play* and it should boot you into the game!

Also, it'd be a good idea (however not required) to download `mintyLauncher.portDatabse` from the GitHub repository and sticking it in the same folder as your Minty Launcher executable.
This will swap out the name of the executable file of the source port over to something more pleasing to look at. You can also add in your own source ports or edit the list as you'd like.

## 2. The Interface

### 2a. The Main Window
The main window has several buttons and hidden gimmicks so it may be confusing at first.

The IWAD and Complevel selectors allow you to select which IWAD and engine you wish to emulate. Please note the latter is only usable in ports that supoort such feature like DSDA-Doom.

The *Extra Paramaters* box allows you to type in any extra parameters you wish to pass along to the source port, such as `-playdemo` to play a recorded demo.
You can also right-click the box to insert a file path easier.

The *Select Port* button allows you to select which port you wish to use.

The *External Files* button opens up the External File Manager window, which allows you to sort load orders of maps, mods, and dehacked/bex patches.

*Ask For Preset Location* Setting **OFF**: The Selector Box will allow you to quickly select from your saved presets.

*Ask For Preset Location* Setting **ON**: The *Load Preset* button will allow you to load in a preset.

The *Save Preset* button will allow you to save a preset. If the *Ask For Preset Location* setting is **OFF**, it will pop up with a small box that'll allow you to put in a preset name. These are saved in your AppData folder.

### 2b. The External File Manager
You can drag files onto this window to quickly add them to your External File list. You can also hit the **+** button to add in an external file to the list as well.

You can select multiple files at once to delete them with the **-** button. You can also select a single file.

You can reorder single files in the list with the Up and Down arrow buttons.

## 3. Settings Overview

### 3a. Presets
These are settings relating to presets.

*Load "Default" Preset on launch*: 

	This will check for a preset named "Default" and load it automatically. Please note it will only check the AppData folder.
	
### 3b. Portability
These are settings related to being able to more easily use the same executable across multiple computers, like on a thumb drive.

*Ask For Preset Location*:

	This will replace the Load Preset Box with a button instead. Both the Load and Save buttons will pop up with a file dialog instead.
	
*Make Settings Portable*:
	
	This will create a portable settings file in the directory of Minty Launcher's executable.
	
### 3c. Window
These are settings relating to Minty Launcher's main window.

*Close On Play*:

	This will close the launcher after the source port has been closed.
	
*Show Window On Quit*:

	This will make the window appear above all windows once the source port has been closed.
	
### 3d. Ungrouped

*Discord Rich Presence*:

	This will enable/disable the Discord RPC feature of the launcher.
	
*Set IWADs Folder*:

	This will allow you to change where your IWADs folder is located.
	
## 4. Command-line Arguments
`--preset [preset name]` loads in the preset from your AppData folder. Exclude the file extension from the name.

`--preset-path [path]` loads in a preset from the desired path. Must be an `.mlPreset` file.

`--iwad` loads in the IWAD file from your IWADs folder. Must include the file extension.

`--info` shows the current version of Minty Launcher.

`--no-gui` is meant to be combined with the above arguments. Automatically starts the sourceport from the preset with the desired IWAD without ever showing the GUI.

`--no-gui-rpc` is identical to `--no-gui` but allows you to override your RPC setting. Use [1, true, on] to enable RPC, [0, false, off] to disable RPC.

`--info` shows the current version of Minty Launcher.