# Modding For Dummies

Modding For Dummies is a mod template that allows you to create your very own TABS mod. It contains a simplified space for you to code factions, units, weapons, abilities, and clothing items.

It also supports custom models and icons.

### If you have trouble with the mod, or if you just want to chat, you can join the [TABS Mod Center](https://discord.gg/zrs44qyp7S).

### This mod is [**open source**](https://github.com/donkeyrat/MFD).

# How To Use

Below is a full tutorial on how to make your own mod, including installing Visual Studio, downloading and editing the mod template, and uploading your completed mod to Thunderstore.

## Installing Visual Studio

The first step in creating your own mod is downloading **Visual Studio**. You can find a link to download ***[here.](https://visualstudio.microsoft.com/)*** You will then scroll down to where it says **"Meet the Visual Studio family,"** and depending on your operating system you can either install *Visual Studio* or *Visual Studio for Mac*. Linux is unsupported for this tutorial.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079528373937778740/3TmS88ZlrP.png?ex=667beb65&is=667a99e5&hm=e7367ea7b18d613dabc80ea6ca3927bf5ca4d043340a46577eca9f267fd6111b&)

You will then hover over the **"Download Visual Studio"** button, and select **"Community 2022."** Afterwards, it will take you to a download page, and install a program called **"VisualStudioSetup.exe."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079531309661171803/screenshot2.png?ex=667bee20&is=667a9ca0&hm=9c39cb14953d33382a35c3288131aabe10fe198fc1f8b2399264dd768b5c5f6a&)

Open it up, and you'll be prompted with a screen asking if you would like to allow the program to make changes to your device. Select **"Yes."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079532462054592643/screenshot3.png?ex=667bef33&is=667a9db3&hm=758f3904c0e77f7caba5d72092ccc71fd19a8d7c1c16ebae9bdda3085e273429&)

After selecting "Yes," an installer window will then pop up. Select **"Continue,"** and proceed with the installation.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079532636713791614/screenshot4.png?ex=667bef5d&is=667a9ddd&hm=2f973a16efe810e16d95f7d6bb0bc39128fb19e212a68d0be9abb53ff863275c&)

Once you reach this screen, find the workload called **"Game development with Unity,"** and click the box to install it along with Visual Studio.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079532636713791614/screenshot4.png?ex=667bef5d&is=667a9ddd&hm=2f973a16efe810e16d95f7d6bb0bc39128fb19e212a68d0be9abb53ff863275c&)

Afterwards, click the **"Install"** button in the bottom-right corner of the installer. Bear in mind that you will need to have free space on your computer.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079534296265654312/screenshot6.png?ex=667bf0e9&is=667a9f69&hm=c52c71087c8d689cb2aceeb5db261163f4dffdb692d47f74a366fb4132dbcf21&)

It will then take you to a screen that should look something like this. You must wait for it to finish installing before proceeding. Make sure that **"Start after installation"** is turned on.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079534535131267112/screenshot7.png?ex=667bf121&is=667a9fa1&hm=c37da3a73e1eeb7ff3c7d88db3b031624651bf9b63cc2fc1fec5f024e4615a50&)

After it finishes installing, it should open up, and you will see a screen that looks something like this. If you already have a Microsoft account, you can sign in; if not, create one.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079535388370161736/screenshot8.png?ex=667bf1ed&is=667aa06d&hm=9449a02a0183a58de612c65a1fd746ca277b539f80f993d1849b40f71f59a337&)

You will see a screen that looks something like this. For now, don't do anything; just leave the program open.

## Using The Mod

The next section of the tutorial will be about installing the mod template, and opening it with Visual Studio.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079535910938497105/screenshot9.png?ex=667bf269&is=667aa0e9&hm=07a3a6f3d796fef0a9029799081431ed5b41a5da9739582342945963646a7fb3&)

First, navigate to the mod's [**Github**](https://github.com/donkeyrat/MFD). Afterwards, find the button that says **"Code,"** and click on it. It will open a drop-down menu, and you will then click **"Download ZIP."** 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079536853729935380/screenshot10.png?ex=667bf34a&is=667aa1ca&hm=a25c305a509788a5d02045adb64c2c0d0a0f03bd80da271b8da23d5e0bb9019a&)

This will download a copy of the mod files, which you can then put wherever you'd like in your computer, and unzip it. For the sake of the tutorial, I will be placing the zip on my desktop and unzipping it there.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079537069086490666/screenshot11.png?ex=667bf37e&is=667aa1fe&hm=58507163bc54c060766cc3bd12a6f3b574bc866f131500c94f4fc307f230ed44&)

Afterwards, return to Visual Studio, and click on the **"Open a project or solution"** button. 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079537845200486460/screenshot12.png?ex=667bf437&is=667aa2b7&hm=fac70f7ca17c419f4af8b9105c592d44acd2fcc4d6ef344ab37464ab9e195ff5&)

This will open up a new tab; navigate to where you placed your unzipped **"MFD-main"** folder, and open up the file named **"ModdingForDummies.sln."** Afterwards, click on the **"Open"** button.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079538251360129074/screenshot13.png?ex=667bf497&is=667aa317&hm=35e76dffe3a422942f6cc3e1fb31e60dec498686f9606e74e286a6879515f33d&)

Once it finishes opening, you will see a screen that looks like this. Click on the arrow to reveal the files.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079539087268126810/screenshot14.png?ex=667bf55f&is=667aa3df&hm=dd64fd17b863c5a6f26a1cda001b7f065b8105f7085168803bb7647dfeb26883&)

You can then double-click on the file named **"Main.cs,"** and it will open up.

### Adding References

This section of the tutorial is only if you open up **"Main.cs"**, and see any lines with red underlines. If this happens, it's either because you have manually renamed your TABS folder, or have TABS on Epic Games. *Bear in mind that this tutorial does not support the Microsoft Store version of TABS.*

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079539944118296676/screenshot15.png?ex=667bf62b&is=667aa4ab&hm=d8ba6ff6cbe78b9fe3d7ac5f3a8a48be2c7218e4f0dbcfbb15dce2977f8198ae&)

Open up the drop-down named **"References,"** and select everything in there besides **"Analyzers."** You can then right click one of the files, and click **"Remove."** This will remove all the current references.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079540984540561449/screenshot16.png?ex=667bf723&is=667aa5a3&hm=f6fd25761cd3e26fea3354a18faf95b1648d0aeed3fb786c627c66a2e86a532d&)

Next, right click the drop-down named **"ModdingForDummies,"** and then click **"Add,"** and then finally click **"Reference."**

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079541401714434200/screenshot17.png?ex=667bf787&is=667aa607&hm=0a8af15fc21212bcf50f485da71b89401250c586efa9f8db3386d101ed08aa27&)

Afterwards, it will open up a new menu. Click **"Browse,"** and it will open up a window. Locate your Totally Accurate Battle Simulator directory (where the .exe is), and then open the **"TotallyAccurateBattleSimulator_Data"** folder. In there will be a folder named **"Managed"**; open it, and you will find many program files.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079541938576953416/screenshot18.png?ex=667bf807&is=667aa687&hm=e43400ca0c9e0629d3c096d0a2108cecafe218bb52e2f764a34f7a601f3e17bf&)

In this folder, you will select every file **EXCEPT** for the ones that start with **"System"**. You will also not select the file named **"mscorslib"**. These files are built into Visual Studio, therefore they will cause issues if you reference them.

You will also need to navigate to the folder in your main TABS directory named **"BepInEx."** In it will be a folder named **"core."** Select every file in it besides the file named **"0Harmony20.dll."**

After selecting these files, click on the button that says **"Add,"** and you're good to go.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079542565000458240/screenshot19.png?ex=667bf89c&is=667aa71c&hm=c4a68b4021c7ed08fcff2ec091f0217d395d43e7b73a2173691942fa38b7a181&)

You will then return to this menu, and you can click the button that says **"OK."** This will cause Visual Studio to freeze for some time. Once it finishes loading, the red underlines will be gone! Hooray!

## Using The Mod Template

The **"Main.cs"** file contains two example units, and an example faction. This will assist you in making new units. Explaining the basics of Visual Studio and the coding language **C#** are beyond the scope of this tutorial. If you would like to research those on your own time, there are plenty of tutorials available online.

### Item Names

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079547936746246296/screenshot23.png?ex=667bfd9d&is=667aac1d&hm=7be44710703bc884d0a656f1c372767e0d459f4093dbadabc11f5d7aa0a161dd&)

This mod prints out item names in the folder the mod file is placed in. These files are placed in a folder named **"MFDPrints."** When you reference a unit, faction, weapon, prop, or anything else, you will refer to the names from these files. 

The names of each item typically corresponds to what they it is named in the Unit Creator. In the case of duplicate names, a number is added to the end of it to distinguish each item.

### Custom Models

This section of the tutorial is incomplete.

### Saving Changes

Pressing your "ctrl" key and "S" key at the same time will save each file individually.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079549760710656050/screenshot25.png?ex=667bff50&is=667aadd0&hm=e33bfe8f33f70921a001aa4bf69c7ddc6d7e642d6fc09652d43e45d6951cfc92&)

When you want to save your changes to the game, you must build the mod file. Return to the directory of your mod. Next, right click on the file **"ModdingForDummies,"** and click on **"Open with."** You will then select **"Notepad."** 

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079550022980485120/screenshot26.png?ex=667bff8e&is=667aae0e&hm=1fa88599e534b0ae1104a46d506abec6618e83115bc53876137a183baf9f3c12&)

Scroll all the way to the bottom; there, you will find where building the file is handled. It contains a path to a directory. By default, it leads to the Steam location of where TABS should be. If you renamed your TABS folder, have a Mac, or have TABS on Epic Games, this will need to be corrected to be your actual TABS directory. Ensure that your new directory leads to your **"plugins"** folder, in the **"BepInEx"** folder.

Afterwards, you can return to Visual Studio, press the "ctrl," "shift," and "B" keys on your keyboard all at once to build the file straight to your mod folder.

## Releasing Your Mod

Once you finish making a mod, there are a few things you must do first.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079544862501773332/screenshot20.png?ex=667bfac0&is=667aa940&hm=f88bd5041338ef9697780177cf938b220b3176aec0d1a3b484f3dce0276a9a9b&)

Firstly, be sure that you go to "Main.cs" and turn off **"DEV_MODE."** This will disable the prints, so that mods you make with Modding For Dummies does not generate useless files for players.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079546560758689842/screenshot21.png?ex=667bfc55&is=667aaad5&hm=360532a8fa7cf599207ce781353c7c4a2d680cde8a7013bc4a737bdf9b05552b&)

Next, navigate to the file named **"Launcher.cs,"** and make sure to fill out everything with your mod name.

![enter image description here](https://cdn.discordapp.com/attachments/651812532746584085/1079547015165399121/screenshot22.png?ex=667bfcc1&is=667aab41&hm=340902e57648d7ee31278a423c8347dfc964840746abd0fd686fb8e29aca91f9&)

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