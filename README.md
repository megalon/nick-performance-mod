# NASB Performance Mod
This is a mod to improve performance in Nickelodeon All-Star Brawl on PC.

## Current Features

- Disable specific objects in each stage.

## 🚀 Installation

Grab the latest release [from the release tab.](https://github.com/megalon/nick-performance-mod/releases/latest) Not on Thunderstore yet.

## ℹ Usage

Currently, this mod loads JSON files from the folder `Nickelodeon All-Star Brawl\BepInEx\Performance Mod\Stages`

The mod includes a few files with the download. You can edit these files in a text editor to change which objects get disabled. You can even create your own file for any stage that doesn't have a config yet.

To find out the names of the objects in the scene, you can use the [Runtime Unity Editor](https://github.com/ManlyMarco/RuntimeUnityEditor).

Start a training match in the stage you want to adjust, then open the Runtime Unity Editor and follow the instructions below.

![performance-mod-diagram](https://user-images.githubusercontent.com/27714637/147719649-911d5963-5f02-413a-8fc8-53aafcc419c0.png)

Currently, only objects that are parented to the stage object are supported. Their transform heiarchy starts with

`game instance(Clone)/stage_{whatever}/`

Restart the game to load the updated files.

## 🔧 Developing

This project requires `Newtonsoft.Json.dll`

You can install it with `JsonDotNet` via [Slime Mod Manager.](https://github.com/legoandmars/SlimeModManager/releases)

### Setup

Clone the project, then create a file in the root of the project directory named:

`NickStageHazardRemover.csproj.user`

Here you need to set the `GameDir` property to match your install directory.

Example:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <GameDir>D:\SteamLibrary\steamapps\common\Nickelodeon All-Star Brawl</GameDir>
  </PropertyGroup>
</Project>
```

Now when you build the mod, it should resolve your references automatically, and the build event will copy the plugin into your `BepInEx\plugins` folder!
