# Factorio-Headless-Server-Tool

**Factorio Headless Server Tool** is a Windows-based utility designed to simplify the management of headless Factorio servers. With this tool, you can easily manage saves, configure settings, and start or stop your server without needing to navigate complex command-line options.

![Main Interface Screenshot](Factorio_Headless_Server_Tool-Screenshot.jpg)
![Udater_Interface Screenshot](Factorio_Headless_Server_Tool-Updater-Screenshot.jpg)
## Features

- **Start/Stop Server**: Easily start and stop your Factorio headless server with the click of a button.
- **Save Management**: View, load, and delete save files directly from the interface.
- **Server Configuration**: Customize server settings, such as server name, description, max players, and more.
- **IP Address Display**: Shows local and public IP addresses, with options to copy them to your clipboard.
- **User Preferences**: Save and load preferences like server port, username, password, and token.
- **Console Log**: View program logs in real-time with a dedicated console window.
- **Backup and Restore**: Create backups of save files and restore them as needed.
- **Achievements Patch Feature**: Unlock achievements on modified save files by bypassing restrictions that normally prevent achievements.
- **Automatic Update Check**: Automatically check for new Factorio server updates and notify users if a new version is available.
- **Download Manager with Progress**: Download the latest version with progress tracking, including the ability to cancel or hide the progress dialog.
- **Overwrite Handling**: Allows users to decide if they want to overwrite existing files when applying updates, avoiding repetitive confirmations.
- **Reapply Updates Option**: Reapply the latest update if it’s already downloaded.

## Installation

1. **Download** the compiled executable from the [Releases](https://github.com/coolshrimp/Factorio-Headless-Server-Tool/releases/) section.
2. **Run the executable**—no installation required.

## Usage

1. **Select Factorio Server Folder**: When prompted, choose your Factorio server directory.
   - If you don’t have a Factorio server, click the **Download** button in the tool to download `server.zip`.
   - Extract this file, then select the extracted Factorio server folder.
     
2. **Saves Folder**: Ensure your save files are located in the `saves` folder inside the Factorio server directory. The tool allows you to explore and manage these saves.

3. **Achievements Patch**: For modified saves where achievements have been disabled, you can use the **Achievements Patch Feature** to reactivate achievements. Note: This may be considered a form of modification and is generally intended for personal use only.

## Requirements

- **Windows OS** (Tested on Windows 10 and later)
- **Latest Official Factorio Server Files** Unzipped ([Download Factorio Server](https://www.factorio.com/download))
- **.NET 6.0 Desktop Runtime** or later ([Download .NET Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.35-windows-x64-installer))

## Troubleshooting

- **Settings Not Saving**: Ensure the tool is running with write permissions to save configuration files.
- **Server Not Starting**: Verify the Factorio server path and check for any missing dependencies.
- **Console Log Not Displayed**: Confirm the console log window is not minimized and is attached to the main form.
- **Achievements Not Unlocking**: Make sure the achievements patch is applied to a compatible save file and restart the server.
- **Auto Update Issues**: Verify that your username and token are entered correctly. Also, ensure your network connection is stable and that you have sufficient permissions to overwrite existing server files when applying updates.

## Version History

### v1.3.0

- **New Feature: Automatic Update Check with Single Overwrite Prompt**  
  Added an automatic update check feature with an improved overwrite prompt. Users are now asked **only once** if they want to overwrite existing files during an update, avoiding repetitive confirmations.

- **New Feature: Download Manager with Progress Tracking and Overwrite Options**  
  Enhanced the download manager for updates, including a progress dialog with options to **cancel** or **hide**. Additionally, users can choose to overwrite **all files at once** during an update.

- **New Feature: Reapply Updates Option**  
  If the latest version is already downloaded, users can now choose to **reapply the update** without downloading it again. This helps in situations where the previous update may not have been fully successful.

- **New Enhancement: Debugging Traces for Update Process**  
  Added detailed trace statements throughout the update process to improve debugging. These traces provide insight into version checking, file extraction, and the download process to help quickly identify issues.

- **Improved Update Extraction Flow**  
  The tool now extracts **only the contents** from the version directory inside the `.zip` archive, ensuring the update directory structure integrates seamlessly with the existing server folder.

### v1.1.0

- **New Feature: Automatic Update Check**  
  Added the ability to automatically check for new updates from the Factorio server. Users are notified when a new version is available.

- **New Feature: Download Manager with Progress Tracking**  
  Implemented a progress dialog for update downloads. Users can now track the download progress, with options to **cancel** or **hide** the dialog while the download proceeds.

- **New Feature: Overwrite Handling for Updates**  
  When extracting update files, users are prompted **only once** if they want to overwrite existing files, avoiding multiple confirmation dialogs.

- **Enhancement: Reapply Updates Option**  
  If the latest version is already downloaded, users can choose to **reapply the update** without downloading it again.

- **Enhanced Update Extraction**  
  The tool extracts **only the contents** from within the version directory of the `.zip` archive, ensuring the update directory structure integrates seamlessly.

- **Improvement: Debugging Traces Added**  
  Added detailed trace statements to aid in debugging and provide insight into the version checking, file extraction, and download processes.

### v1.0.0

- **Initial Release**  
  - **Start/Stop Server**: Easily start and stop your Factorio headless server.
  - **Save Management**: View, load, and delete save files.
  - **Server Configuration**: Customize settings like server name, description, max players, etc.
  - **IP Address Display**: Shows local and public IP addresses, with clipboard copy options.
  - **User Preferences**: Save/load settings like server port, username, password, and token.
  - **Console Log**: View real-time program logs.
  - **Backup and Restore**: Create backups of save files and restore them.
  - **Achievements Patch Feature**: Unlock achievements on modified save files.

## Acknowledgments

- [Factorio](https://factorio.com/) for their fantastic game.
- Community resources and documentation for Factorio headless servers.
- [FactorioSaveGameEnableAchievements](https://github.com/Rainson12/FactorioSaveGameEnableAchievements)
