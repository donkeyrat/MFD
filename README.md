
# Modding For Dummies

Modding For Dummies is a mod template that allows you to create your very own TABS mod. It contains a simplified space for you to code factions, units, weapons, abilities, and clothing items.

It also supports custom models and icons.

### If you have trouble with the mod, or if you just want to chat, you can join the [TABS Mod Center](https://discord.gg/zrs44qyp7S).

### This mod is [**open source**](https://github.com/donkeyrat/MFD).

# How To Use

Below is a full tutorial on how to make your own mod, including installing Visual Studio, downloading and editing the mod template, and uploading your completed mod to Thunderstore.

## Installing Visual Studio

The first step in creating your own mod is downloading **Visual Studio**. You can find a link to download ***[here.](https://visualstudio.microsoft.com/)*** You will then scroll down to where it says **"Meet the Visual Studio family,"** and depending on your operating system you can either install *Visual Studio* or *Visual Studio for Mac*. Linux is unsupported for this tutorial.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079528373937778740/3TmS88ZlrP.png)

You will then hover over the **"Download Visual Studio"** button, and select **"Community 2022."** Afterwards, it will take you to a download page, and install a program called **"VisualStudioSetup.exe."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079531309661171803/screenshot2.png)

Open it up, and you'll be prompted with a screen asking if you would like to allow the program to make changes to your device. Select **"Yes."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079532462054592643/screenshot3.png)

After selecting "Yes," an installer window will then pop up. Select **"Continue,"** and proceed with the installation.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079533445862146048/screenshot4.png)

Once you reach this screen, find the workload called **"Game development with Unity,"** and click the box to install it along with Visual Studio.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079532636713791614/screenshot4.png)

Afterwards, click the **"Install"** button in the bottom-right corner of the installer. Bear in mind that you will need to have free space on your computer.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079534296265654312/screenshot6.png)

It will then take you to a screen that should look something like this. You must wait for it to finish installing before proceeding. Make sure that **"Start after installation"** is turned on.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079534535131267112/screenshot7.png)

After it finishes installing, it should open up, and you will see a screen that looks something like this. If you already have a Microsoft account, you can sign in; if not, create one.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079535388370161736/screenshot8.png)

You will see a screen that looks something like this. For now, don't do anything; just leave the program open.

## Using The Mod

The next section of the tutorial will be about installing the mod template, and opening it with Visual Studio.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079535910938497105/screenshot9.png)

First, navigate to the mod's [**Github**](https://github.com/donkeyrat/MFD). Afterwards, find the button that says **"Code,"** and click on it. It will open a drop-down menu, and you will then click **"Download ZIP."** 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079536853729935380/screenshot10.png)

This will download a copy of the mod files, which you can then put wherever you'd like in your computer, and unzip it. For the sake of the tutorial, I will be placing the zip on my desktop and unzipping it there.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079537069086490666/screenshot11.png)

Afterwards, return to Visual Studio, and click on the **"Open a project or solution"** button. 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079537845200486460/screenshot12.png)

This will open up a new tab; navigate to where you placed your unzipped **"MFD-main"** folder, and open up the file named **"ModdingForDummies.sln."** Afterwards, click on the **"Open"** button.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079538251360129074/screenshot13.png)

Once it finishes opening, you will see a screen that looks like this. Click on the arrow to reveal the files.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079539087268126810/screenshot14.png)

You can then double-click on the file named **"Main.cs,"** and it will open up.

### Adding References

This section of the tutorial is only if you open up **"Main.cs"**, and see any lines with red underlines. If this happens, it's either because you have manually renamed your TABS folder, or have TABS on Epic Games. *Bear in mind that this tutorial does not support the Microsoft Store version of TABS.*

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079539944118296676/screenshot15.png)

Open up the drop-down named **"References,"** and select everything in there besides **"Analyzers."** You can then right click one of the files, and click **"Remove."** This will remove all the current references.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079540984540561449/screenshot16.png)

Next, right click the drop-down named **"ModdingForDummies,"** and then click **"Add,"** and then finally click **"Reference."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079541401714434200/screenshot17.png)

Afterwards, it will open up a new menu. Click **"Browse,"** and it will open up a window. Locate your Totally Accurate Battle Simulator directory (where the .exe is), and then open the **"TotallyAccurateBattleSimulator_Data"** folder. In there will be a folder named **"Managed"**; open it, and you will find many program files.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079541938576953416/screenshot18.png)

In this folder, you will select every file **EXCEPT** for the ones that start with **"System"**. You will also not select the file named **"mscorslib"**. These files are built into Visual Studio, therefore they will cause issues if you reference them.

You will also need to navigate to the folder in your main TABS directory named **"BepInEx."** In it will be a folder named **"core."** Select every file in it besides the file named **"0Harmony20.dll."**

After selecting these files, click on the button that says **"Add,"** and you're good to go.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079542565000458240/screenshot19.png)

You will then return to this menu, and you can click the button that says **"OK."** This will cause Visual Studio to freeze for some time. Once it finishes loading, the red underlines will be gone! Hooray!

## Using The Mod Template

The **"Main.cs"** file contains two example units, and an example faction. This will assist you in making new units. Explaining the basics of Visual Studio and the coding language **C#** are beyond the scope of this tutorial. If you would like to research those on your own time, there are plenty of tutorials available online.

### Item Names

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079547936746246296/screenshot23.png)

This mod prints out item names in the folder the mod file is placed in. These files are placed in a folder named **"MFDPrints."** When you reference a unit, faction, weapon, prop, or anything else, you will refer to the names from these files. 

The names of each item typically corresponds to what they it is named in the Unit Creator. In the case of duplicate names, a number is added to the end of it to distinguish each item.

### Custom Models

This section of the tutorial is incomplete.

### Saving Changes

Pressing your "ctrl" key and "S" key at the same time will save each file individually.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079549760710656050/screenshot25.png)

When you want to save your changes to the game, you must build the mod file. Return to the directory of your mod. Next, right click on the file **"ModdingForDummies,"** and click on **"Open with."** You will then select **"Notepad."** 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079550022980485120/screenshot26.png)

Scroll all the way to the bottom; there, you will find where building the file is handled. It contains a path to a directory. By default, it leads to the Steam location of where TABS should be. If you renamed your TABS folder, have a Mac, or have TABS on Epic Games, this will need to be corrected to be your actual TABS directory. Ensure that your new directory leads to your **"plugins"** folder, in the **"BepInEx"** folder.

Afterwards, you can return to Visual Studio, press the "ctrl," "shift," and "B" keys on your keyboard all at once to build the file straight to your mod folder.

## Releasing Your Mod

Once you finish making a mod, there are a few things you must do first.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079544862501773332/screenshot20.png)

Firstly, be sure that you go to "Main.cs" and turn off **"DEV_MODE."** This will disable the prints, so that mods you make with Modding For Dummies does not generate useless files for players.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079546560758689842/screenshot21.png)

Next, navigate to the file named **"Launcher.cs,"** and make sure to fill out everything with your mod name.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079547015165399121/screenshot22.png)

Above is an example of how to fill it out. In it, the creator of the mod is named "Joe Biden," and the mod is named "American Military."

Lastly, you must go to the directory of your mod, and make sure that file that contains "ModdingForDummies" is replaced with your mod name. You must also open up files manually to find them; you can ignore the **"bin"** and **"obj"** folders, but everything else must be searched.

That's it! Your mod is now ready for release.

### Uploading To Thunderstore

This section of the tutorial is incomplete.

## Changelog

The mod's [**Github**](https://github.com/donkeyrat/MFD) is the only place to view the full changelog.

# 1.0.0

 - Initial release.

## Credits

This mod was made by the Shadow Modding Elite, a team of the greatest TABS modders.

__Maples - Coder__

__BD - Coder__

__Arargd - Coder__

__Fern - Coder__

__Harren Tonderen - Icon Artist__
