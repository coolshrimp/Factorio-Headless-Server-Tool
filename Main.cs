using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.ComponentModel; // For CancelEventArgs
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace Factorio_Headless_Server_Tool
{
    public partial class Main : Form
    {
        int serverProcessID = 0;  // Holds the server process ID
        bool serverRunning = false;  // Track server state
        string saveFileName = "";  // Holds the save file name
        string currentDirectory = Directory.GetCurrentDirectory();  // Current working directory
        string[] ipAddresses = new string[2];  // Store local and public IPs
        string FACTORIO_IMAGE_PATH = "https://wiki.factorio.com/images/thumb/";  // Base path for images
        private consoleLog consoleForm;
        private HttpClient httpClient = new HttpClient();
        private DateTime? serverStartTime = null;
        private System.Windows.Forms.Timer statusUpdateTimer;
        private Process serverProcess;

        public Main()
        {
            InitializeComponent();
            InitializeSaveListView();
            consoleForm = new consoleLog();

            // Set up trace listeners to log to the custom console form
            AddTraceListeners();

            // Load saved settings and initial properties
            InitializeFormProperties();
        }


        private void AddTraceListeners()
        {
            // Set up trace listeners to log to the console and the custom console form
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new CustomTextBoxTraceListener(consoleForm));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeFactorioDataPath();
            InitializeIPAddress();
            InitializeContextMenu();
            InitializeServerStatus();

            // Position the console form but keep it hidden until needed
            consoleForm.SnapToMainForm(this);
            consoleForm.Hide();  // Hide on initialization
        }


        public class CustomTextBoxTraceListener : TraceListener
        {
            private readonly consoleLog _consoleForm;

            public CustomTextBoxTraceListener(consoleLog consoleForm)
            {
                _consoleForm = consoleForm;
            }

            public override void Write(string message) => _consoleForm.WriteToConsole(message);

            public override void WriteLine(string message) => _consoleForm.WriteToConsole(message + Environment.NewLine);
        }

        private void InitializeFactorioDataPath()
        {
            string dataPath = Properties.Settings.Default.factorioDataPath;
            Trace.WriteLine("Last used folder: " + dataPath);

            if (!string.IsNullOrEmpty(dataPath) && Directory.Exists(dataPath))
            {
                Trace.WriteLine("Last used folder found: " + dataPath);
                Load_Factorio_Folder();
            }
            else
            {
                Trace.WriteLine("Server folder not found: " + dataPath);
                Select_Factorio_Folder();
            }
        }

        private void InitializeFormProperties()
        {
            // Check if last location and size are set in settings
            if (Properties.Settings.Default.LastFormLocation != Point.Empty && Properties.Settings.Default.LastFormSize != Size.Empty)
            {
                // Restore the last known location and size
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Properties.Settings.Default.LastFormLocation;
                this.Size = Properties.Settings.Default.LastFormSize;
            }
            else
            {
                // Center the form if no previous location/size is saved
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            // Load saved settings for other properties
            portTXT.Text = Properties.Settings.Default.lastPort;
            userTXT.Text = Properties.Settings.Default.lastUsername;
            passTXT.Text = Properties.Settings.Default.lastPassword;
            tokenCheckBOX.Checked = Properties.Settings.Default.lastToken;
            AutoUpdateCheckBOX.Checked = Properties.Settings.Default.autoUpdate;

            // Log loaded settings
            Trace.WriteLine($"Loaded Preferences on Start: Username: {userTXT.Text}, Password: {(string.IsNullOrEmpty(passTXT.Text) ? "[empty]" : "[loaded]")}, Token: {tokenCheckBOX.Checked}");
        }

        private void InitializeSaveListView()
        {
            saveListView.View = View.Details;
            saveListView.FullRowSelect = true;
            saveListView.GridLines = true;
            saveListView.MultiSelect = false;

            // Set up columns with header details
            saveListView.Columns.Add("Save File", 200, HorizontalAlignment.Left);
            saveListView.Columns.Add("Save Date", 150, HorizontalAlignment.Left);

            // Handle row selection event
            saveListView.SelectedIndexChanged += saveListView_SelectedIndexChanged;
        }

        private void InitializeIPAddress()
        {
            ipAddresses[0] = GetLocalIPAddress();
            Trace.WriteLine("Local IP: " + ipAddresses[0]);

            ipAddresses[1] = GetPublicIPAddress();
            Trace.WriteLine("Public IP: " + ipAddresses[1]);

            // Display masked public IP in label
            ipLBL.Text = $"{ipAddresses[0]} / ***.***.***.***";
        }

        private void InitializeServerStatus()
        {
            ServerStatusTXT.Text = "Server is not running";
            statusUpdateTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // Update every second
            };
            statusUpdateTimer.Tick += (s, e) => UpdateServerStatus();
            statusUpdateTimer.Start();
        }

        private string GetLocalIPAddress()
        {
            // Attempt to retrieve first IPv4 address
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return string.Empty;
        }

        private string GetPublicIPAddress()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString("http://checkip.dyndns.org");
                    string publicIP = response.Substring(response.IndexOf(":") + 1).Trim();
                    return publicIP.Substring(0, publicIP.IndexOf("<"));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error fetching public IP: " + ex.Message);
                return string.Empty;
            }
        }

        private void InitializeContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Copy Local IP").Click += (s, e) => CopyToClipboard(ipAddresses[0], "Local IP");
            menu.Items.Add("Copy Remote IP").Click += (s, e) => CopyToClipboard(ipAddresses[1], "Public IP");
            ipLBL.ContextMenuStrip = menu;
        }

        private void CopyToClipboard(string text, string label)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
                Trace.WriteLine($"{label}: {text}");
            }
        }

        private bool IsValidFactorioFolder(string path)
        {
            // Check if the factorio.exe exists in the specified path
            string fullPath = Path.Combine(path, "bin", "x64", "factorio.exe");
            Trace.WriteLine($"Checking if valid Factorio folder: {fullPath}");
            bool exists = File.Exists(fullPath);
            Trace.WriteLine($"Is valid Factorio folder: {exists}");
            return exists;
        }

        private void Select_Factorio_Folder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK) // Check if user selected a folder
                {
                    string selectedPath = folderDialog.SelectedPath;
                    Trace.WriteLine($"Selected folder: {selectedPath}");

                    if (IsValidFactorioFolder(selectedPath)) // Check if selected folder contains necessary Factorio files
                    {
                        try
                        {
                            Properties.Settings.Default.factorioDataPath = selectedPath; // Save the valid Factorio path
                            Properties.Settings.Default.Save();
                            Trace.WriteLine("Load Folder set to: " + Properties.Settings.Default.factorioDataPath);
                            Load_Factorio_Folder(); // Load the saves from the selected folder
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("Error while setting factorio data path: " + ex.Message); // Log an error if path saving fails
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Selected folder is not a valid Factorio server folder: " + selectedPath);
                        MessageBox.Show("The selected folder is not a valid Factorio server folder.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void contextMenuSaves_Opening(object sender, CancelEventArgs e)
        {
            // This method runs when the context menu is about to open.
            // You can customize which items are enabled/disabled based on the selected item.
            Trace.WriteLine("Context menu opening. Checking if items are selected...");
            if (saveListView.SelectedItems.Count == 0)
            {
                Trace.WriteLine("No items selected. Canceling context menu.");
                e.Cancel = true; // Cancel the opening if no item is selected
            }
        }

        private void LoadSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveListView.SelectedItems.Count > 0)
            {
                string selectedSave = saveListView.SelectedItems[0].Text;
                string saveFilePath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", selectedSave);
                Trace.WriteLine($"Loading save: {selectedSave}");
                Load_Factorio_Save(saveFilePath);
            }
        }

        private void DeleteSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveListView.SelectedItems.Count > 0)
            {
                string selectedSave = saveListView.SelectedItems[0].Text;
                string saveFilePath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", selectedSave);

                DialogResult result = MessageBox.Show($"Are you sure you want to delete '{selectedSave}'?",
                                                      "Delete Confirmation",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(saveFilePath);
                        Trace.WriteLine($"Deleted save: {saveFilePath}");
                        Load_Factorio_Folder(); // Reload the saves list
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Error deleting save: {ex.Message}");
                        MessageBox.Show("Error deleting the save file. Please check the logs for details.");
                    }
                }
            }
        }

        private void BackupSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveListView.SelectedItems.Count > 0)
            {
                string selectedSave = saveListView.SelectedItems[0].Text;
                string saveFilePath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", selectedSave);
                string backupPath = saveFilePath + ".bak";

                try
                {
                    File.Copy(saveFilePath, backupPath, true);
                    Trace.WriteLine($"Backup created for save: {selectedSave}");
                    MessageBox.Show("Backup created successfully.");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error creating backup: {ex.Message}");
                    MessageBox.Show("Error creating the backup file. Please check the logs for details.");
                }
            }
        }

        private void Load_Factorio_Folder()
        {
            string factorioDataPath = Properties.Settings.Default.factorioDataPath;
            string savesFolder = Path.Combine(factorioDataPath, "saves");
            string serverEXE = Path.Combine(factorioDataPath, "bin", "x64", "Factorio.exe");

            if (!Directory.Exists(savesFolder))
            {
                Trace.WriteLine($"Directory {savesFolder} does not exist.");
                return;
            }

            // Set the loaded folder path in the TextBox to indicate the current directory
            loadedFolderTXT.Text = factorioDataPath;

            saveListView.Items.Clear(); // Clear previous items

            try
            {
                string[] zipFiles = Directory.GetFiles(savesFolder, "*.zip");

                foreach (string zipFile in zipFiles)
                {
                    string fileName = Path.GetFileName(zipFile);
                    DateTime lastModified = File.GetLastWriteTime(zipFile);

                    // Create a ListViewItem to represent a row
                    ListViewItem item = new ListViewItem(fileName);
                    item.SubItems.Add(lastModified.ToString("yyyy-MM-dd HH:mm:ss")); // Add Last Modified as a subitem

                    saveListView.Items.Add(item); // Add the row to the ListView
                }

                groupBox1.Text = $"{zipFiles.Length} Saves Found";
                Trace.WriteLine($"{zipFiles.Length} saves found and listed.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error loading saves: {ex.Message}");
            }

            try
            {
                Load_Factorio_Save(saveListView.Items[0].Text);
                saveListView.Items[0].Selected = true;
            }
            catch
            {
                Trace.WriteLine("No save files found to load.");
            }

            // Check for Factorio.exe and retrieve its version
            UpdateServerVersionLabel();

        }

        private void Load_Factorio_Save(string selectedSaveFileName)
        {
            string saveFilePath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", selectedSaveFileName); // Construct the full path for the selected save file
            Trace.WriteLine($"Loading save file: {saveFilePath}");
            if (!File.Exists(saveFilePath))
            {
                Trace.WriteLine($"Save file not found: {saveFilePath}");
                return; // Exit if the save file doesn't exist
            }

            saveFileName = selectedSaveFileName; // Store the selected save file name

            using (ZipArchive archive = ZipFile.OpenRead(saveFilePath)) // Open the ZIP file for reading
            {
                Trace.WriteLine($"Opened save archive: {saveFilePath}");
                LoadPreviewImage(archive); // Load the preview image from the save
                LoadServerSettings(archive); // Load the server settings from the save
            }

            PopulateServerDetailsUI(); // Populate the UI with the loaded server details
        }

        private void LoadPreviewImage(ZipArchive archive)
        {
            string previewfileName = Path.Combine(Path.GetFileNameWithoutExtension(saveFileName), "preview.jpg"); // Construct path to the preview image within the archive
            Trace.WriteLine($"Looking for preview image: {previewfileName}");
            var entry = archive.GetEntry(previewfileName); // Attempt to get the entry for the preview image

            if (entry != null) // If the entry exists, load the image
            {
                using (Stream stream = entry.Open()) // Open the entry for reading
                {
                    Trace.WriteLine("Loading preview image from save.");
                    Image image = Image.FromStream(stream); // Load the image from the stream
                    previewBox.Image = image; // Set the image in the preview box
                }
            }
            else
            {
                Trace.WriteLine("Preview image not found in save.");
            }
        }

        private void LoadServerSettings(ZipArchive archive)
        {
            string serverSettingsPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "bin", "x64", "Server-Settings.json");  // Local path to save server settings file
            string serversettingsfileName = Path.Combine(Path.GetFileNameWithoutExtension(saveFileName), "Server-Settings.json");  // Expected path in the ZIP archive

            // Normalize path for comparison with entries in the ZIP archive
            serversettingsfileName = serversettingsfileName.Replace("\\", "/").Trim();

            Trace.WriteLine($"Attempting to extract server settings: {serversettingsfileName}");

            // Attempt to extract the server settings file from the ZIP archive by looking for an exact match
            var entry = archive.Entries.FirstOrDefault(e => string.Equals(e.FullName, serversettingsfileName, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                entry.ExtractToFile(serverSettingsPath, true); // Extract the settings file from the save to a known location
                Trace.WriteLine($"Server settings extracted to: {serverSettingsPath}");
            }
            else
            {
                Trace.WriteLine($"Server settings file not found in archive: {serversettingsfileName}");
            }

            // Confirm if the extracted server settings file actually exists
            if (File.Exists(serverSettingsPath))
            {
                Trace.WriteLine("Server settings extraction successful. File exists at: " + serverSettingsPath);
            }
            else
            {
                Trace.WriteLine("Server settings extraction failed. File does not exist at: " + serverSettingsPath);
            }
        }

        private void PopulateServerDetailsUI()
        {
            string serverSettingsPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "bin", "x64", "Server-Settings.json");
            Trace.WriteLine($"Populating server details from: {serverSettingsPath}");

            if (!File.Exists(serverSettingsPath)) // Exit if the server settings file is not found
            {
                Trace.WriteLine("Server settings file not found.");
                return;
            }

            string json = File.ReadAllText(serverSettingsPath); // Read server settings JSON
            JObject serverSettingsObj;

            try
            {
                serverSettingsObj = JObject.Parse(json); // Parse JSON into an object
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error parsing the server settings: {ex.Message}"); // Log an error if JSON parsing fails
                return;
            }

            // Populate various UI components with data from the parsed JSON
            Trace.WriteLine("Populating UI with server settings.");
            serverTitleTXT.Text = (string)serverSettingsObj["name"];
            serverDescriptionRTXT.Text = (string)serverSettingsObj["description"];

            JArray tags = (JArray)serverSettingsObj["tags"];
            tagsTXT.Text = string.Join(",", tags.Select(t => t.ToString()));

            publicCheckBOX.Checked = (bool)serverSettingsObj["visibility"]["public"];
            lanCheckBOX.Checked = (bool)serverSettingsObj["visibility"]["lan"];
            userTXT.Text = Properties.Settings.Default.lastUsername;
            passTXT.Text = Properties.Settings.Default.lastPassword;
            tokenCheckBOX.Checked = Properties.Settings.Default.lastToken;
            commandsDDB.Text = (string)serverSettingsObj["allow_commands"];
            maxplayersTXT.Text = (string)serverSettingsObj["max_players"];
            afktimeTXT.Text = (string)serverSettingsObj["afk_autokick_interval"];
            autosaveslotsTXT.Text = (string)serverSettingsObj["autosave_slots"];
            autosaveintervalTXT.Text = (string)serverSettingsObj["autosave_interval"];
            maxuploadslotsTXT.Text = (string)serverSettingsObj["max_upload_slots"];
            maxuploadspeedTXT.Text = (string)serverSettingsObj["max_upload_in_kilobytes_per_second"];
            limitCheckBOX.Checked = (bool)serverSettingsObj["ignore_player_limit_for_returning_players"];
            autopauseCheckBOX.Checked = (bool)serverSettingsObj["auto_pause"];
            serverpassTXT.Text = (string)serverSettingsObj["game_password"];
            verificationCheckBOX.Checked = (bool)serverSettingsObj["require_user_verification"];

            if (!serverRunning) startBTN.Enabled = true; // Enable the start button if the server is not running
            serverDetailsPanel.Visible = true; // Make the server details panel visible
        }

        private bool IsFactorioServerRunning()
        {
            // Check if any "factorio" processes are currently running
            Process[] factorioProcesses = Process.GetProcessesByName("factorio");
            return factorioProcesses.Length > 0;
        }

        private void stopBTN_Click(object sender, EventArgs e)
        {
            StopServerProcess();
        }
        private void StopServerProcess()
        {
            try
            {
                if (serverProcessID != 0)
                {
                    Trace.WriteLine($"Attempting to stop server process with ID: {serverProcessID}");
                    Process process = Process.GetProcessById(serverProcessID);

                    if (!process.HasExited)
                    {
                        process.CloseMainWindow();
                        Trace.WriteLine("CloseMainWindow() called on server process.");

                        if (!process.WaitForExit(5000))
                        {
                            Trace.WriteLine("Process did not exit in time. Attempting to kill process...");
                            process.Kill();
                            Trace.WriteLine("Process killed forcefully.");
                        }
                        else
                        {
                            Trace.WriteLine("Process exited gracefully.");
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Process already exited or could not be found.");
                    }
                }
                else
                {
                    Trace.WriteLine("No valid server process ID found to stop.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error stopping server: {ex.Message}");
            }
            finally
            {
                serverRunning = false;
                startBTN.Enabled = true;
                stopBTN.Enabled = false;
                serverProcessID = 0;
                serverStartTime = null;
                UpdateServerStatus();
            }
        }

        private void StopAnyRunningFactorioServer()
        {
            try
            {
                Process[] factorioProcesses = Process.GetProcessesByName("factorio");
                foreach (Process process in factorioProcesses)
                {
                    if (!process.HasExited)
                    {
                        process.CloseMainWindow();
                        if (!process.WaitForExit(5000))
                        {
                            process.Kill();  // Force kill if it does not exit in time
                        }
                        Trace.WriteLine($"Stopped Factorio server process: {process.Id}");
                    }
                }

                serverRunning = false;
                startBTN.Enabled = true;
                stopBTN.Enabled = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error stopping Factorio server: {ex.Message}");
            }
            finally
            {
                UpdateServerStatus();
            }
        }

        private void startBTN_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Attempting to start Factorio server...");

            if (IsFactorioServerRunning())
            {
                Trace.WriteLine("A Factorio server is already running.");
                DialogResult result = MessageBox.Show("A Factorio server is already running. Would you like to stop it before launching a new one?",
                                                      "Server Already Running",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Trace.WriteLine("User opted to stop the running Factorio server before starting a new one.");
                    StopAnyRunningFactorioServer();
                }
                else
                {
                    Trace.WriteLine("User chose not to stop the running server. Start operation aborted.");
                    return;
                }
            }

            string factorioDataPath = Properties.Settings.Default.factorioDataPath;
            if (string.IsNullOrEmpty(factorioDataPath) || string.IsNullOrEmpty(saveFileName))
            {
                Trace.WriteLine("Factorio data path or save file name is not set. Start operation aborted.");
                MessageBox.Show("Please set the Factorio data path and select a save file before starting the server.",
                                "Missing Configuration",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string saveFilePath = Path.Combine(factorioDataPath, "saves", saveFileName);
            if (!File.Exists(saveFilePath))
            {
                Trace.WriteLine($"Save Not Found: {saveFilePath}. Start operation aborted.");
                MessageBox.Show($"The selected save file '{saveFileName}' was not found. Please verify the save file location.",
                                "Save File Not Found",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                Trace.WriteLine("Starting Factorio server process...");
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/k \"cd /d {Path.Combine(factorioDataPath, "bin", "x64")} && factorio.exe --start-server \"{saveFilePath}\" --server-settings \"Server-Settings.json\" --console-log \"{Path.Combine(factorioDataPath, "saves", "console.log")}\"\"",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };

                process.EnableRaisingEvents = true;
                process.Exited += (s, args) =>
                {
                    Trace.WriteLine($"Server process with ID {process.Id} has exited.");
                    serverRunning = false;
                    serverProcessID = 0;
                    serverStartTime = null;

                    this.Invoke((MethodInvoker)(() =>
                    {
                        startBTN.Enabled = true;
                        stopBTN.Enabled = false;
                        UpdateServerStatus();
                    }));
                };

                process.Start();

                serverProcessID = process.Id;
                serverStartTime = DateTime.Now;
                serverRunning = true;
                startBTN.Enabled = false;
                stopBTN.Enabled = true;

                Trace.WriteLine($"Server Started Successfully - ProcessID: {serverProcessID}");
                UpdateServerStatus();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error starting server: {ex.Message}");
                serverRunning = false;
                startBTN.Enabled = true;
                stopBTN.Enabled = false;
                MessageBox.Show("Failed to start the server. Check the logs for more details.",
                                "Server Start Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void UpdateServerStatus()
        {
            if (serverRunning && serverProcessID != 0 && serverStartTime.HasValue)
            {
                TimeSpan runtime = DateTime.Now - serverStartTime.Value;

                // Format the text as requested
                string line1 = $"Server is Running (Uptime: {runtime:hh\\:mm\\:ss})";
                string line2 = $"Process ID: {serverProcessID}";

                // Update the text box
                ServerStatusTXT.Text = $"{line1}\r\n{line2}";
            }
            else
            {
                ServerStatusTXT.Text = "Server is not running";
            }
        }

        private void CreatorLBL_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", "https://www.CoolshrimpModz.com");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error opening URL: {ex.Message}");
                MessageBox.Show("An error occurred while opening the URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            SaveUserPreferences();  // Save current settings before closing
            consoleForm?.Close();
            StopAnyRunningFactorioServer();
        }

        private void SaveUserPreferences()
        {
            Properties.Settings.Default.LastFormSize = this.Size;
            Properties.Settings.Default.LastFormLocation = this.Location;
            Properties.Settings.Default.lastUsername = userTXT.Text;
            Properties.Settings.Default.lastPassword = passTXT.Text;
            Properties.Settings.Default.lastToken = tokenCheckBOX.Checked;
            Properties.Settings.Default.lastPort = portTXT.Text;
            Properties.Settings.Default.autoUpdate = autopauseCheckBOX.Checked;


            // Explicitly save changes to ensure persistence
            Properties.Settings.Default.Save();

            // Log saved values for verification
            Trace.WriteLine($"User preferences saved: Username: {Properties.Settings.Default.lastUsername}, Password: {(string.IsNullOrEmpty(Properties.Settings.Default.lastPassword) ? "[empty]" : "[saved]")}, Token: {Properties.Settings.Default.lastToken}, Port: {Properties.Settings.Default.lastPort}");
        }

        private void loadBTN_Click(object sender, EventArgs e)
        {
            Select_Factorio_Folder();
            Load_Factorio_Folder();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Properties.Settings.Default.factorioDataPath);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error opening File Explorer: {ex.Message}");
                MessageBox.Show("An error occurred while opening File Explorer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adminListBTN_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Opened adminList Window");
            adminList adminList = new adminList();
            adminList.Tag = this;
            adminList.StartPosition = FormStartPosition.Manual;
            adminList.Location = this.Location;
            adminList.Show(this);
        }

        private void banListBTN_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Opened banList Window");
            banList banList = new banList();
            banList.Tag = this;
            banList.StartPosition = FormStartPosition.Manual;
            banList.Location = this.Location;
            banList.Show(this);
        }

        private void openSavesBTN_Click(object sender, EventArgs e)
        {
            try
            {
                string savesPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves");
                Process.Start("explorer.exe", savesPath);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error opening saves folder in File Explorer: {ex.Message}");
                MessageBox.Show("An error occurred while opening the saves folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showPassBTN_MouseDown(object sender, MouseEventArgs e)
        {
            passTXT.UseSystemPasswordChar = false;
        }

        private void showPassBTN_MouseUp(object sender, MouseEventArgs e)
        {
            passTXT.UseSystemPasswordChar = true;
        }

        private void resetallBTN_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset all server settings to default? This cannot be undone.", "Confirm", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) { return; }

            if (saveListView.SelectedItems.Count > 0)
            {
                string selectedSave = saveListView.SelectedItems[0].Text;

                // Delete existing server settings files
                string serverSettingsPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "bin\\x64\\Server-Settings.json");
                string serverBackupPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "bin\\x64\\Server-Settings.bak");

                if (File.Exists(serverSettingsPath)) { File.Delete(serverSettingsPath); }
                if (File.Exists(serverBackupPath)) { File.Delete(serverBackupPath); }

                // Reset server settings within the selected save ZIP
                string zipPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", selectedSave);

                if (File.Exists(zipPath))
                {
                    string fileInsertPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "data\\server-settings.example.json");

                    using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                    {
                        // Remove existing server settings from the archive
                        string entryPath = Path.Combine(Path.GetFileNameWithoutExtension(selectedSave), "Server-Settings.json").Replace("\\", "/");
                        ZipArchiveEntry existingEntry = archive.GetEntry(entryPath);
                        if (existingEntry != null)
                        {
                            existingEntry.Delete();
                        }

                        // Add default settings to the archive
                        archive.CreateEntryFromFile(fileInsertPath, entryPath);
                    }

                    // Copy the example settings to the server path
                    File.Copy(fileInsertPath, serverSettingsPath, true);
                    Trace.WriteLine("Settings reset to: " + fileInsertPath);

                    // Reload the selected save to reflect changes
                    Load_Factorio_Save(selectedSave);
                }
                else
                {
                    Trace.WriteLine("Selected save file does not exist: " + zipPath);
                    MessageBox.Show("The selected save file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Trace.WriteLine("No save file selected to reset settings.");
                MessageBox.Show("Please select a save file to reset settings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveBTN_Click(object sender, EventArgs e)
        {
            string factorioDataPath = Properties.Settings.Default.factorioDataPath;
            string serverFile = Path.Combine(factorioDataPath, "bin\\x64\\Server-Settings.json");
            string serverBackupFile = Path.Combine(factorioDataPath, "bin\\x64\\Server-Settings.bak");
            string saveFilePath = Path.Combine(factorioDataPath, "saves", saveFileName);
            bool patchAchievements = false;

            // Check if the "Enable Achievements" checkbox is checked and get confirmation before saving
            if (enableAchievementsCheckBox.Checked)
            {
                DialogResult result = MessageBox.Show(
                    "This will modify the save file to enable achievements. Note: This is technically considered cheating, as it bypasses the usual restrictions. Do you want to continue?",
                    "Enable Achievements Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    Trace.WriteLine("User declined to enable achievements.");
                    return; // Exit without saving if the user declines
                }

                // User confirmed, so set flag to patch achievements
                patchAchievements = true;
            }

            try
            {
                // Load current server settings from the JSON file
                JObject serverSettings = LoadServerSettings(serverFile);

                // Update the server settings based on UI input
                UpdateServerSettings(serverSettings);

                // Save the updated server settings to the JSON file
                SaveServerSettings(serverSettings, serverFile);

                // Backup the server settings to a separate file (optional)
                UpdateAndZipServerSettings(serverSettings, serverBackupFile);

                // Add the updated server settings back to the save ZIP file
                using (ZipArchive archive = ZipFile.Open(saveFilePath, ZipArchiveMode.Update))
                {
                    string entryName = Path.Combine(Path.GetFileNameWithoutExtension(saveFileName), "Server-Settings.json").Replace("\\", "/");
                    ZipArchiveEntry existingEntry = archive.GetEntry(entryName);
                    if (existingEntry != null)
                    {
                        existingEntry.Delete(); // Remove the old entry if it exists
                    }
                    archive.CreateEntryFromFile(serverFile, entryName); // Add the updated settings file back into the ZIP
                }

                Trace.WriteLine("Saved Server Settings and updated ZIP file");
                MessageBox.Show("Saved Server Settings and updated ZIP file");

                // Patch the save file for achievements if the user confirmed earlier
                if (patchAchievements)
                {
                    Trace.WriteLine("Patching save file to enable achievements...");
                    PatchSaveFile(saveFilePath);
                    MessageBox.Show("Save file patched successfully to enable achievements.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error saving server settings: {ex.Message}");
                MessageBox.Show("Error saving server settings. Check the logs for details.");
            }
        }

        private void portTXT_TextChanged(object sender, EventArgs e)
        {
            string configiniFile = Path.Combine(Properties.Settings.Default.factorioDataPath, "config", "config.ini");
            string serverPort = portTXT.Text;
            Properties.Settings.Default.lastPort = serverPort;  // Save the new port to settings
            Trace.WriteLine("Editing:" + configiniFile);

            if (!File.Exists(configiniFile)) return;  // Exit if the config file does not exist

            string[] lines = File.ReadAllLines(configiniFile);  // Read all lines from the INI file

            for (int i = 0; i < lines.Length; i++)  // Loop through each line to find the port setting
            {
                if (Regex.IsMatch(lines[i], @"^port=") || Regex.IsMatch(lines[i], @"^; port="))  // Look for the port setting (even commented out)
                {
                    lines[i] = lines[i].TrimStart(';').Trim();  // Remove any leading semicolon
                    lines[i] = "port=" + serverPort;  // Update the port value
                    Trace.WriteLine("Port set to:" + serverPort);
                    break;  // Exit the loop after updating the port
                }
            }

            File.WriteAllLines(configiniFile, lines);  // Write the updated lines back to the INI file
            Trace.WriteLine("Saved Port:" + configiniFile);
        }

        private void PatchSaveFile(string saveFilePath)
        {
            try
            {
                var fileProcessor = new FileProcessor();
                var extractDirectoryPath = Path.Combine(Path.GetDirectoryName(saveFilePath), Path.GetFileNameWithoutExtension(saveFilePath));

                Trace.WriteLine("Extracting save file to: " + extractDirectoryPath);
                ZipFile.ExtractToDirectory(saveFilePath, extractDirectoryPath, true);
                var files = Directory.GetFiles(extractDirectoryPath, "level.dat*", SearchOption.AllDirectories)
                                     .Where(x => !x.EndsWith(".datmetadata") && !x.EndsWith(".bin")).ToArray();

                foreach (var file in files)
                {
                    var result = fileProcessor.UnpackZLib(file);
                    string asciiString = Encoding.ASCII.GetString(result);

                    if (asciiString.Contains("command-ran"))
                    {
                        var position = asciiString.IndexOf("command-ran");
                        var findings = FindPattern(result, new byte[] { 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff });
                        var closest = findings.OrderBy(x => Math.Abs(position - x)).ToList();

                        if (closest.Count > 0)
                        {
                            if (result[closest[0] - 7] == 1)
                            {
                                // Achievements deactivated due to commands, reactivate them
                                result[closest[0] - 7] = 0;
                            }

                            if (result[closest[0] - 6] == 1)
                            {
                                // Achievements deactivated due to map editor, reactivate them
                                result[closest[0] - 6] = 0;
                            }

                            var packed = fileProcessor.PackZLib(result);
                            File.WriteAllBytes(file, packed); // Replace existing
                            Trace.WriteLine("Patched finding in save file.");
                        }
                        else
                        {
                            Trace.WriteLine("No relevant pattern found to patch in save file.");
                        }
                    }
                }

                Trace.WriteLine("Backing up original save file");
                File.Move(saveFilePath, saveFilePath + ".bak", true); // Backup original save
                Trace.WriteLine("Creating new save file");
                ZipFile.CreateFromDirectory(extractDirectoryPath, saveFilePath, CompressionLevel.Optimal, false);
                Trace.WriteLine("Cleaning up extracted directory");
                Directory.Delete(extractDirectoryPath, true);
                Trace.WriteLine("Save file patched successfully.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error patching save file: {ex.Message}");
            }
        }

        // Helper method to find patterns in byte arrays
        private List<int> FindPattern(byte[] array, byte[] pattern)
        {
            List<int> findings = new List<int>();
            if (array == null || pattern == null || array.Length == 0 || pattern.Length == 0 || pattern.Length > array.Length)
            {
                return findings;
            }

            for (int i = 0; i <= array.Length - pattern.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (array[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    findings.Add(i);
                }
            }

            return findings;
        }

        private JObject LoadServerSettings(string path)
        {
            string json = File.ReadAllText(path);
            return JObject.Parse(json);
        }

        private void UpdateServerSettings(JObject serverSettings)
        {
            // Server details
            serverSettings["name"] = serverTitleTXT.Text;
            serverSettings["description"] = serverDescriptionRTXT.Text;

            // Tags
            string[] tagArray = tagsTXT.Text.Split(',');
            JArray tags = JArray.FromObject(tagArray);
            serverSettings["tags"] = tags;

            // Visibility
            serverSettings["visibility"]["public"] = publicCheckBOX.Checked;
            serverSettings["visibility"]["lan"] = lanCheckBOX.Checked;

            // User Credentials
            serverSettings["username"] = userTXT.Text;

            if (tokenCheckBOX.Checked == false)
            {
                serverSettings["password"] = passTXT.Text;
                serverSettings["token"] = "";
            }
            else
            {
                serverSettings["token"] = passTXT.Text;
                serverSettings["password"] = "";
            }

            // Server Settings
            serverSettings["allow_commands"] = commandsDDB.Text;
            serverSettings["max_players"] = maxplayersTXT.Text;
            serverSettings["afk_autokick_interval"] = afktimeTXT.Text;
            serverSettings["autosave_slots"] = autosaveslotsTXT.Text;
            serverSettings["autosave_interval"] = autosaveintervalTXT.Text;
            serverSettings["max_upload_slots"] = maxuploadslotsTXT.Text;
            serverSettings["max_upload_in_kilobytes_per_second"] = maxuploadspeedTXT.Text;
            serverSettings["ignore_player_limit_for_returning_players"] = limitCheckBOX.Checked;
            serverSettings["auto_pause"] = autopauseCheckBOX.Checked;
            serverSettings["game_password"] = serverpassTXT.Text;
            serverSettings["require_user_verification"] = verificationCheckBOX.Checked;

            // Save user settings
            Properties.Settings.Default.lastUsername = userTXT.Text;
            Properties.Settings.Default.lastPassword = passTXT.Text;
            Properties.Settings.Default.lastToken = tokenCheckBOX.Checked;
            Properties.Settings.Default.Save();

            Trace.WriteLine($"Updated server settings and saved user preferences.");
        }

        private void SaveServerSettings(JObject serverSettings, string path)
        {
            File.WriteAllText(path, serverSettings.ToString());
        }

        private void UpdateAndZipServerSettings(JObject serverSettings, string backupPath)
        {
            // Remove sensitive data
            serverSettings["username"] = "";
            serverSettings["password"] = "";
            serverSettings["token"] = "";
            File.WriteAllText(backupPath, serverSettings.ToString());
            UpdateZipWithSettings(backupPath);
        }

        private void UpdateZipWithSettings(string backupPath)
        {
            string zipPath = Path.Combine(Properties.Settings.Default.factorioDataPath, "saves", saveFileName);
            if (File.Exists(zipPath))
            {
                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    string entryName = Path.Combine(Path.GetFileNameWithoutExtension(saveFileName), "Server-Settings.json").Replace("\\", "/");
                    ZipArchiveEntry existingEntry = archive.GetEntry(entryName);
                    if (existingEntry != null)
                    {
                        existingEntry.Delete();
                    }
                    archive.CreateEntryFromFile(backupPath, entryName);
                }
            }
        }

        private const int PREVIEW_FORM_WIDTH = 520;
        private const int PREVIEW_FORM_HEIGHT = 520;

        private void previewdescLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string serverDescription = serverDescriptionRTXT.Text;

            try
            {
                // Convert BBCode to HTML
                string convertedHtmlText = ConvertBBCodeToHtml(serverDescription);
                Trace.WriteLine("Converted HTML: \n" + convertedHtmlText);

                // Create the preview form
                using (Form previewForm = new Form())
                {
                    previewForm.Text = "Preview";
                    previewForm.Size = new Size(PREVIEW_FORM_WIDTH, PREVIEW_FORM_HEIGHT);
                    previewForm.FormBorderStyle = FormBorderStyle.FixedSingle;

                    // Use WebBrowser control for preview
                    using (WebBrowser webBrowser = new WebBrowser())
                    {
                        webBrowser.Dock = DockStyle.Fill;
                        previewForm.Controls.Add(webBrowser);

                        string htmlToRender = "<head><style>body { background-color: #403F40; color: #f2f2f2; font-family: \"Droid Sans\", sans-serif; font-size:14px } img { display: block; margin-bottom: 15px; }</style></head>" + convertedHtmlText;
                        webBrowser.DocumentText = htmlToRender;

                        previewForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error in previewdescLabel_LinkClicked: {ex.Message}");
                MessageBox.Show("An error occurred while generating the preview. Please check the input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertBBCodeToHtml(string bbcode)
        {
            var richTextMappings = new Dictionary<string, string>();

            // Load the colors from Color_List.txt
            var colors = LoadEmbeddedResource("Factorio_Headless_Server_Tool.Color_List.txt"); // Replace with your actual namespace.
            foreach (var color in colors)
            {
                richTextMappings[$"[color={color}]"] = $"<span style='color:{color}'>";
                richTextMappings[$"[/color]"] = "</span>";
            }

            // Base URL for images
            string baseUrl = "https://wiki.factorio.com/images/thumb/";

            // Load the items from Item_List.txt
            var items = LoadEmbeddedResource("Factorio_Headless_Server_Tool.Item_List.txt"); // Replace with your actual namespace.
            foreach (var item in items)
            {
                var formattedItemName = char.ToUpper(item[0]) + item.Substring(1).Replace("-", " ");
                var encodedItemName = Uri.EscapeDataString(formattedItemName); // Encoding the spaces as %20
                richTextMappings[$"[img=item.{item}]"] = $"<img src='{baseUrl}{encodedItemName}.png/28px-{encodedItemName}.png' alt='{formattedItemName}'>";
            }

            // Other general mappings can be added here, like the font tags
            richTextMappings["[font=default-bold]"] = "<span style='font-weight:bold'>";
            richTextMappings["[/font]"] = "</span>";
            richTextMappings["\n"] = "<br />";

            // Now, process the BBCode to HTML conversion using the richTextMappings dictionary
            foreach (var pair in richTextMappings)
            {
                bbcode = bbcode.Replace(pair.Key, pair.Value);
            }

            return bbcode;
        }

        private List<string> LoadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<string> lines = new List<string>();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        private const string IP_MASK = " ***.***.***.***";

        private void ipLBL_MouseEnter(object sender, EventArgs e)
        {
            ipLBL.Text = $"{ipAddresses[0]} / {ipAddresses[1]}";
        }

        private void ipLBL_MouseLeave(object sender, EventArgs e)
        {
            ipLBL.Text = $"{ipAddresses[0]} / {IP_MASK}";
        }

        private string BBcodeToRtf(string text)
        {
            // Create a StringBuilder to hold the RTF code
            StringBuilder rtf = new StringBuilder();

            // Add the RTF header
            rtf.Append(@"{\rtf1\ansi");

            // Split the text by the BB code tags
            string[] parts = Regex.Split(text, @"\[(.+?)\]");

            // Iterate through the parts of the text
            foreach (string part in parts)
            {
                switch (part)
                {
                    case "b":
                        rtf.Append(@"\b");
                        break;
                    case "/b":
                        rtf.Append(@"\b0");
                        break;
                    case "i":
                        rtf.Append(@"\i");
                        break;
                    case "/i":
                        rtf.Append(@"\i0");
                        break;
                    case "u":
                        rtf.Append(@"\ul");
                        break;
                    case "/u":
                        rtf.Append(@"\ulnone");
                        break;
                    default:
                        rtf.Append(part); // Add the text as-is
                        break;
                }
            }

            // Add the RTF footer
            rtf.Append(@"}");

            // Return the RTF code
            return rtf.ToString();
        }

        private void enableAchievementsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPreferences(); // Ensure preferences are saved when checkbox state changes
        }

        private void RefreshBTN_Click(object sender, EventArgs e)
        {
            Load_Factorio_Folder();
        }

        private void saveListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (saveListView.SelectedItems.Count > 0)
            {
                string selectedSave = saveListView.SelectedItems[0].Text;

                // Set "Enable Achievements" checkbox to unchecked by default
                enableAchievementsCheckBox.Checked = false;

                // Load the selected save file
                Load_Factorio_Save(selectedSave);
            }
        }

        private void logBTN_Click(object sender, EventArgs e)
        {
            if (consoleForm == null || consoleForm.IsDisposed)
            {
                consoleForm = new consoleLog();
                consoleForm.Hide();
            }

            if (!consoleForm.Visible)
            {
                consoleForm.SnapToMainForm(this); // Position the console to the right of the main form
                consoleForm.Show();
            }
            consoleForm.BringToFront();
        }

        private void userTXT_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.lastUsername = userTXT.Text;
            Properties.Settings.Default.Save();
        }

        private void passTXT_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.lastPassword = passTXT.Text ?? string.Empty;
            Properties.Settings.Default.Save();
        }

        private void tokenCheckBOX_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.lastToken = tokenCheckBOX.Checked;
            Properties.Settings.Default.Save();
        }

        private void loadedFolderTXT_TextChanged(object sender, EventArgs e)
        {

        }

        // Optimized version with reusable methods for version number checks
        private async void AutoUpdateCheckBOX_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autoUpdate = AutoUpdateCheckBOX.Checked;
            Properties.Settings.Default.Save();

            await CheckForUpdatesAsync();
        }

        public async Task CheckForUpdatesAsync()
        {
            if (AutoUpdateCheckBOX.Checked)
            {
                // Fetch the latest version available
                string latestVersion = await CheckLatestVersionNumber();

                if (string.IsNullOrEmpty(latestVersion))
                {
                    MessageBox.Show("Failed to check for updates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string currentVersion = CheckLocalVersionNumber();
                if (string.IsNullOrEmpty(currentVersion))
                {
                    MessageBox.Show("Unable to retrieve current version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Compare versions to determine if an update is available
                if (string.Compare(currentVersion, latestVersion, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    ShowUpdateDialog();
                }
                else
                {
                    MessageBox.Show("Your server files are up to date.", "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Trace.WriteLine("Auto-update is disabled by the user.");
            }
        }

        private async Task<string> CheckLatestVersionNumber()
        {
            try
            {
                // The URL to fetch available versions
                string url = "https://updater.factorio.com/get-available-versions";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        Trace.WriteLine($"Error fetching update data: {response.StatusCode}");
                        return string.Empty;
                    }

                    // Parse the response content as JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
                    JsonElement root = jsonDocument.RootElement;

                    // Assuming the package we care about is "core-linux_headless64"
                    if (root.TryGetProperty("core-linux_headless64", out JsonElement packageElement))
                    {
                        foreach (JsonElement versionElement in packageElement.EnumerateArray())
                        {
                            if (versionElement.TryGetProperty("stable", out JsonElement stableVersion))
                            {
                                string latestStableVersion = stableVersion.GetString();
                                Trace.WriteLine($"Latest Stable Version: {latestStableVersion}");
                                return latestStableVersion;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while checking updates: {ex.Message}");
            }
            return string.Empty;
        }

        private string CheckLocalVersionNumber()
        {
            string serverEXE = Path.Combine(Properties.Settings.Default.factorioDataPath, "bin", "x64", "factorio.exe");

            if (!File.Exists(serverEXE))
            {
                Trace.WriteLine("No Server EXE found in selected folder");
                return string.Empty;
            }

            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(serverEXE);
                return versionInfo.FileVersion;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error retrieving Factorio.exe version: {ex.Message}");
                return string.Empty;
            }
        }

        private async void UpdateServerVersionLabel()
        {
            string currentVersion = CheckLocalVersionNumber();
            if (string.IsNullOrEmpty(currentVersion))
            {
                ServerVersionLBL.Text = "No Server EXE found in selected folder";
                return;
            }

            string latestVersion = await CheckLatestVersionNumber();
            if (string.IsNullOrEmpty(latestVersion))
            {
                Invoke(new Action(() =>
                {
                    ServerVersionLBL.Text = $"Server Version: {currentVersion} (Error checking latest version)";
                    ServerVersionLBL.ForeColor = Color.Red;
                }));
                return;
            }

            // Update the label to indicate the version status
            Invoke(new Action(() =>
            {
                if (string.Compare(currentVersion, latestVersion, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    ServerVersionLBL.Text = $"Server Version: {currentVersion} (Update Available)";
                    ServerVersionLBL.ForeColor = Color.Orange;
                }
                else
                {
                    ServerVersionLBL.Text = $"Server Version: {currentVersion} (Latest Version)";
                    ServerVersionLBL.ForeColor = Color.Green;
                }
            }));
        }

        private void ShowUpdateDialog()
        {
            DialogResult result = MessageBox.Show(
                "An update is available. Would you like to update now?",
                "Update Available",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                PerformUpdate();
            }
            else
            {
                MessageBox.Show("Update postponed. You can check for updates later.", "Update Postponed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void downloadServerFileBTN_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the local server path is set; if not, default to current run folder
                string defaultPath = Properties.Settings.Default.factorioDataPath;
                if (string.IsNullOrEmpty(defaultPath))
                {
                    // Default to the current directory and create a new folder named "Factorio_Server"
                    defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Factorio_Server");
                    Properties.Settings.Default.factorioDataPath = defaultPath;
                    Properties.Settings.Default.Save();
                }

                // Check if the local server files are missing
                string serverEXEPath = Path.Combine(defaultPath, "bin", "x64", "factorio.exe");
                if (File.Exists(serverEXEPath))
                {
                    MessageBox.Show("The server files are already downloaded.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // No server EXE found, download the latest version
                string latestVersion = await CheckLatestVersionNumber();
                if (string.IsNullOrEmpty(latestVersion))
                {
                    MessageBox.Show("Unable to retrieve the latest version for download.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string build = "expansion";
                string distro = "win64-manual";
                string username = userTXT.Text;
                string token = passTXT.Text;

                // Validate user credentials
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Username and Token are required for the download.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Define output paths
                string outputDirectory = Path.Combine(defaultPath, "updates");
                string zipFilePath = Path.Combine(outputDirectory, "factorio_server_download.zip");

                // Create the updates directory if it does not exist
                Directory.CreateDirectory(outputDirectory);

                var downloadProgressForm = new DownloadProgressForm();
                downloadProgressForm.Show();

                // Check if the existing download file matches the latest version
                if (File.Exists(zipFilePath) && CheckIfExistingUpdateIsLatest(zipFilePath, latestVersion))
                {
                    DialogResult skipDownloadResult = MessageBox.Show(
                        "The latest version is already downloaded. Would you like to continue with the extraction?",
                        "Update Already Downloaded",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (skipDownloadResult == DialogResult.Yes)
                    {
                        // Extract on a background task to prevent UI locking
                        await Task.Run(() => ExtractAndUpdateFiles(zipFilePath, downloadProgressForm));
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Download skipped. You can continue later if needed.", "Download Skipped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        downloadProgressForm.Close(); // Close the progress form if user cancels the update
                        return;
                    }
                }

                // Download URL
                string url = $"https://www.factorio.com/get-download/{latestVersion}/{build}/{distro}?username={username}&token={token}";
                Trace.WriteLine(url);

                var cts = new CancellationTokenSource();

                try
                {
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Trace.WriteLine($"Error downloading server files: {response.StatusCode}");
                            MessageBox.Show("Failed to download the server files. Please check your credentials and internet connection.", "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            downloadProgressForm.Close(); // Close the progress form on failure
                            return;
                        }

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 81920, useAsync: true))
                        {
                            byte[] buffer = new byte[81920];
                            int bytesRead;
                            long totalBytes = response.Content.Headers.ContentLength ?? 0;
                            long downloadedBytes = 0;

                            // Update the file size information
                            downloadProgressForm.Invoke(new Action(() =>
                            {
                                downloadProgressForm.FileSizeLabel.Text = $"File Size: {downloadProgressForm.FormatBytes(totalBytes)}";
                            }));

                            // Asynchronous download to avoid UI blocking
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                            {
                                if (downloadProgressForm.IsCanceled)
                                {
                                    cts.Cancel();
                                    Trace.WriteLine("Download cancelled by user.");
                                    MessageBox.Show("Download has been cancelled.", "Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    downloadProgressForm.Close(); // Close the progress form on cancellation
                                    return;
                                }

                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                downloadedBytes += bytesRead;

                                // Update download progress asynchronously
                                int percentComplete = (int)((downloadedBytes / (double)totalBytes) * 100);
                                downloadProgressForm.Invoke(new Action(() =>
                                {
                                    downloadProgressForm.ProgressBar.Value = percentComplete;
                                    downloadProgressForm.ProgressLabel.Text = $"Downloading: {percentComplete}%";
                                    downloadProgressForm.FileSizeLabel.Text = $"Downloaded: {downloadProgressForm.FormatBytes(downloadedBytes)} / {downloadProgressForm.FormatBytes(totalBytes)}";
                                }));
                            }
                        }
                    }

                    // Extract and apply the downloaded server files on a background thread to keep the UI responsive
                    await Task.Run(() => ExtractAndUpdateFiles(zipFilePath, downloadProgressForm));

                    // Show success confirmation after initial download
                    MessageBox.Show("The server files have been successfully downloaded and set up.", "Download Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (OperationCanceledException)
                {
                    Trace.WriteLine("Operation canceled by user.");
                    MessageBox.Show("The download has been canceled.", "Download Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Exception occurred while downloading server files: {ex.Message}");
                    MessageBox.Show("An error occurred during the download. Please try again later.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    downloadProgressForm.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while attempting to download server files: {ex.Message}");
                MessageBox.Show("An error occurred while trying to initiate the download.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExtractAndUpdateFiles(string zipFilePath, DownloadProgressForm downloadProgressForm)
        {
            try
            {
                string extractPath = Properties.Settings.Default.factorioDataPath;
                Trace.WriteLine($"Starting extraction process to: {extractPath}");

                using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                {
                    // Find the root folder in the zip that matches the version naming pattern
                    string versionFolderName = archive.Entries
                        .Select(entry => entry.FullName.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)[0])
                        .Distinct()
                        .FirstOrDefault(name => name.StartsWith("Factorio_", StringComparison.OrdinalIgnoreCase));

                    if (string.IsNullOrEmpty(versionFolderName))
                    {
                        Trace.WriteLine("Unable to locate the version folder within the update archive.");
                        MessageBox.Show("Unable to locate the version folder within the update archive.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Trace.WriteLine($"Version folder found: {versionFolderName}");

                    int totalFiles = archive.Entries.Count(entry =>
                        entry.FullName.StartsWith(versionFolderName + "/", StringComparison.OrdinalIgnoreCase) ||
                        entry.FullName.StartsWith(versionFolderName + "\\", StringComparison.OrdinalIgnoreCase));

                    int extractedFiles = 0;

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Check if the process has been canceled
                        if (downloadProgressForm.IsCanceled)
                        {
                            Trace.WriteLine("Extraction cancelled by user.");
                            MessageBox.Show("Extraction has been cancelled.", "Extraction Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return; // Exit the extraction process
                        }

                        if (entry.FullName.StartsWith(versionFolderName + "/", StringComparison.OrdinalIgnoreCase) ||
                            entry.FullName.StartsWith(versionFolderName + "\\", StringComparison.OrdinalIgnoreCase))
                        {
                            string relativePath = entry.FullName.Substring(versionFolderName.Length + 1);
                            if (string.IsNullOrWhiteSpace(relativePath))
                            {
                                Trace.WriteLine($"Skipping the root version folder itself: {entry.FullName}");
                                continue;
                            }

                            string destinationPath = Path.Combine(extractPath, relativePath);
                            Trace.WriteLine($"Preparing to extract: {entry.FullName} to {destinationPath}");

                            string destinationDirectory = Path.GetDirectoryName(destinationPath);
                            if (!string.IsNullOrEmpty(destinationDirectory) && !Directory.Exists(destinationDirectory))
                            {
                                Directory.CreateDirectory(destinationDirectory);
                                Trace.WriteLine($"Created directory: {destinationDirectory}");
                            }

                            // Extract the file to the destination path
                            entry.ExtractToFile(destinationPath, overwrite: true);
                            Trace.WriteLine($"Successfully extracted: {entry.FullName} to {destinationPath}");

                            // Update the extraction progress
                            extractedFiles++;
                            int percentComplete = (int)((extractedFiles / (double)totalFiles) * 100);
                            downloadProgressForm.Invoke(new Action(() =>
                            {
                                downloadProgressForm.ProgressBar.Value = percentComplete;
                                downloadProgressForm.ProgressLabel.Text = $"Extracting Files: {percentComplete}%";
                                downloadProgressForm.FileSizeLabel.Text = $"Extracted: {extractedFiles} / {totalFiles} files";
                            }));
                        }
                    }
                }

                // Delete the zip file after successful extraction
                Trace.WriteLine($"Deleting zip file: {zipFilePath}");
                File.Delete(zipFilePath);

                Trace.WriteLine("The update has been successfully applied.");
                downloadProgressForm.Invoke(new Action(() =>
                {
                    downloadProgressForm.ProgressLabel.Text = "Extraction completed successfully.";
                }));
                MessageBox.Show("The update has been successfully applied.", "Update Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while extracting update: {ex.Message}");
                MessageBox.Show("An error occurred during the extraction process. Please try again later.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                downloadProgressForm.Invoke(new Action(() => downloadProgressForm.Close()));
            }
        }

        private bool CheckIfExistingUpdateIsLatest(string zipFilePath, string latestVersion)
        {
            try
            {
                // Open the zip file and get the first entry (folder name)
                using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                {
                    if (archive.Entries.Count > 0)
                    {
                        // Find the root folder in the zip that matches the version naming pattern
                        string versionFolderName = archive.Entries
                            .Select(entry => entry.FullName.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)[0])
                            .Distinct()
                            .FirstOrDefault(name => name.StartsWith("Factorio_", StringComparison.OrdinalIgnoreCase));

                        if (versionFolderName != null && versionFolderName.Equals($"Factorio_{latestVersion}", StringComparison.OrdinalIgnoreCase))
                        {
                            Trace.WriteLine($"Update folder matches the latest version: {latestVersion}");
                            return true; // Update file contains the expected version folder
                        }
                        else
                        {
                            Trace.WriteLine("The version folder does not match the expected latest version.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while checking existing update: {ex.Message}");
            }

            return false; // If the folder does not match or any error occurs, consider the update file not the latest
        }

        private async void PerformUpdate()
        {
            try
            {
                // Get the latest version number
                string latestVersion = await CheckLatestVersionNumber();
                if (string.IsNullOrEmpty(latestVersion))
                {
                    MessageBox.Show("Unable to retrieve the latest version for update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string build = "expansion";
                string distro = "win64-manual";
                string username = userTXT.Text;
                string token = passTXT.Text;

                // Validate user credentials
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Username and Token are required for the update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Define output paths
                string outputDirectory = Path.Combine(Properties.Settings.Default.factorioDataPath, "updates");
                string zipFilePath = Path.Combine(outputDirectory, "factorio_update.zip");

                // Create the updates directory if it does not exist
                Directory.CreateDirectory(outputDirectory);

                // Initialize the download progress form
                var downloadProgressForm = new DownloadProgressForm();
                downloadProgressForm.Show();

                // Check if update file already exists and if it matches the latest version
                if (File.Exists(zipFilePath) && CheckIfExistingUpdateIsLatest(zipFilePath, latestVersion))
                {
                    DialogResult reapplyUpdateResult = MessageBox.Show(
                        "The latest version is already downloaded. Would you like to reapply the update?",
                        "Update Already Downloaded",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (reapplyUpdateResult == DialogResult.Yes)
                    {
                        // Extract on a background task to prevent UI locking
                        await Task.Run(() => ExtractAndUpdateFiles(zipFilePath, downloadProgressForm));
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Update cancelled. You can reapply it later if needed.", "Update Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        downloadProgressForm.Close(); // Close the progress form if user cancels the update
                        return;
                    }
                }

                // Download URL
                string url = $"https://www.factorio.com/get-download/{latestVersion}/{build}/{distro}?username={username}&token={token}";
                Trace.WriteLine(url);

                var cts = new CancellationTokenSource();

                try
                {
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Trace.WriteLine($"Error downloading update: {response.StatusCode}");
                            MessageBox.Show("Failed to download the update. Please check your credentials and internet connection.", "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            downloadProgressForm.Close(); // Close the progress form on failure
                            return;
                        }

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 81920, useAsync: true))
                        {
                            byte[] buffer = new byte[81920];
                            int bytesRead;
                            long totalBytes = response.Content.Headers.ContentLength ?? 0;
                            long downloadedBytes = 0;

                            // Update the file size information
                            downloadProgressForm.Invoke(new Action(() =>
                            {
                                downloadProgressForm.FileSizeLabel.Text = $"File Size: {downloadProgressForm.FormatBytes(totalBytes)}";
                            }));

                            // Asynchronous download to avoid UI blocking
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                            {
                                if (downloadProgressForm.IsCanceled)
                                {
                                    cts.Cancel();
                                    Trace.WriteLine("Download cancelled by user.");
                                    MessageBox.Show("Download has been cancelled.", "Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    downloadProgressForm.Close(); // Close the progress form on cancellation
                                    return;
                                }

                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                downloadedBytes += bytesRead;

                                // Update download progress asynchronously
                                int percentComplete = (int)((downloadedBytes / (double)totalBytes) * 100);
                                downloadProgressForm.Invoke(new Action(() =>
                                {
                                    downloadProgressForm.ProgressBar.Value = percentComplete;
                                    downloadProgressForm.ProgressLabel.Text = $"Downloading: {percentComplete}%";
                                    downloadProgressForm.FileSizeLabel.Text = $"Downloaded: {downloadProgressForm.FormatBytes(downloadedBytes)} / {downloadProgressForm.FormatBytes(totalBytes)}";
                                }));
                            }
                        }
                    }

                    // Extract and apply the downloaded update on a background thread to keep the UI responsive
                    await Task.Run(() => ExtractAndUpdateFiles(zipFilePath, downloadProgressForm));

                    // Show success confirmation after initial update
                    MessageBox.Show("The application has been updated successfully.", "Update Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (OperationCanceledException)
                {
                    Trace.WriteLine("Operation canceled by user.");
                    MessageBox.Show("The update has been canceled.", "Update Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Exception occurred while performing update: {ex.Message}");
                    MessageBox.Show("An error occurred during the update. Please try again later.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    downloadProgressForm.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while performing update: {ex.Message}");
                MessageBox.Show("An error occurred during the update. Please try again later.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewStatsLBL_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User clicked to view matchmaking stats.");
            ShowMatchmakingPanel();
        }

        private async void ShowMatchmakingPanel()
        {
            Trace.WriteLine("ShowMatchmakingPanel: Started.");

            // Check if the server is running by looking for the process
            if (serverProcessID == 0)
            {
                Trace.WriteLine("ShowMatchmakingPanel: No server process found (serverProcessID is 0).");
                MessageBox.Show("No active servers found.", "Server Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process process = Process.GetProcessById(serverProcessID);
                if (process.HasExited)
                {
                    Trace.WriteLine("ShowMatchmakingPanel: Server process has exited.");
                    MessageBox.Show("No active servers found.", "Server Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (ArgumentException)
            {
                Trace.WriteLine("ShowMatchmakingPanel: Invalid server process ID or process does not exist.");
                MessageBox.Show("No active servers found.", "Server Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the username and token from the user text box or settings
            string username = userTXT.Text.Trim();
            string authToken = passTXT.Text.Trim();
            string serverIpAddress = ipAddresses[1].Trim();


            Trace.WriteLine($"ShowMatchmakingPanel: Username retrieved - '{username}', Server IP Address - '{serverIpAddress}'");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(serverIpAddress))
            {
                Trace.WriteLine("ShowMatchmakingPanel: Username, token, IP address, or port is empty.");
                MessageBox.Show("Please enter a valid username, IP address, port, and ensure you have a valid token.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare the matchmaking API URL
            string apiUrl = $"https://multiplayer.factorio.com/get-games?username={username}&token={authToken}";
            Trace.WriteLine($"ShowMatchmakingPanel: API URL prepared - https://multiplayer.factorio.com/get-games?username={username}&token=[Hidden]");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Trace.WriteLine("ShowMatchmakingPanel: Sending GET request to matchmaking API.");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        Trace.WriteLine($"ShowMatchmakingPanel: Failed to fetch matchmaking data. Status Code: {response.StatusCode}");
                        MessageBox.Show("Failed to fetch matchmaking data. Please make sure your username and token are correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Trace.WriteLine("ShowMatchmakingPanel: Successfully retrieved response from API.");

                    // Parse the JSON as an array since the response is an array of game objects
                    JArray matchmakingData = JArray.Parse(jsonResponse);
                    Trace.WriteLine($"ShowMatchmakingPanel: JSON Response parsed");

                    // Find the game by matching the host address
                    foreach (var game in matchmakingData)
                    {
                        string gameHostAddress = game["host_address"]?.ToString();
                        if (gameHostAddress != null && gameHostAddress.StartsWith(serverIpAddress))
                        {
                            // Extract all game details for this specific game
                            string gameId = game["game_id"]?.ToString() ?? "Unknown";
                            string serverName = game["name"]?.ToString() ?? "Unknown";
                            string description = game["description"]?.ToString() ?? "None";
                            string maxPlayers = game["max_players"]?.ToString() ?? "Unknown";
                            string gameVersion = game["application_version"]?["game_version"]?.ToString() ?? "Unknown";
                            string buildVersion = game["application_version"]?["build_version"]?.ToString() ?? "Unknown";
                            string buildMode = game["application_version"]?["build_mode"]?.ToString() ?? "Unknown";
                            string platform = game["application_version"]?["platform"]?.ToString() ?? "Unknown";
                            string gameTimeElapsed = game["game_time_elapsed"]?.ToString() ?? "Unknown";
                            string hasPassword = game["has_password"]?.ToString() == "true" ? "Yes" : "No";
                            string serverId = game["server_id"]?.ToString() ?? "Unknown";
                            string headlessServer = game["headless_server"]?.ToString() == "true" ? "Yes" : "No";
                            string hasMods = game["has_mods"]?.ToString() == "true" ? "Yes" : "No";
                            string modCount = game["mod_count"]?.ToString() ?? "Unknown";
                            string tags = game["tags"] != null ? string.Join(", ", game["tags"].ToObject<List<string>>()) : "None";

                            Trace.WriteLine($"ShowMatchmakingPanel: Extracted Details - Game ID: {gameId}, Server Name: {serverName}, Description: {description}, Game Version: {gameVersion}, Build Version: {buildVersion}, Build Mode: {buildMode}, Platform: {platform}, Max Players: {maxPlayers}, Game Time Elapsed: {gameTimeElapsed}, Password Protected: {hasPassword}, Server ID: {serverId}, Headless Server: {headlessServer}, Mods Present: {hasMods}, Mod Count: {modCount}, Tags: {tags}");

                            // Show the matchmaking details in a popup window
                            ShowMatchmakingPopup(gameId, serverName, description, maxPlayers, gameVersion, buildVersion, buildMode, platform, gameTimeElapsed, hasPassword, serverId, headlessServer, hasMods, modCount, tags);

                            return; // Exit the loop once the desired game is found
                        }
                    }

                    Trace.WriteLine("ShowMatchmakingPanel: No active matchmaking information found for this server address.");
                    MessageBox.Show("No active matchmaking information found for this server.", "Matchmaking Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception occurred while fetching matchmaking data: {ex.Message}");
                MessageBox.Show("An error occurred while fetching matchmaking data. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowMatchmakingPopup(string gameId, string serverName, string description, string maxPlayers, string gameVersion, string buildVersion, string buildMode, string platform, string gameTimeElapsed, string hasPassword, string serverId, string headlessServer, string hasMods, string modCount, string tags)
        {
            Trace.WriteLine("ShowMatchmakingPopup: Creating popup window to show matchmaking details.");

            // Create a new form to show the matchmaking details
            Form matchmakingForm = new Form
            {
                Text = "Matchmaking Details",
                Size = new Size(800, 500),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.WhiteSmoke
            };

            // Create a multiline text box to display the description separately
            TextBox descriptionBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(450, 420),
                Location = new Point(320, 20),
                Font = new Font("Arial", 10, FontStyle.Italic),
                Text = description.Replace("\n", "\r\n"), // Correctly replace line breaks for Windows Forms
                BorderStyle = BorderStyle.FixedSingle
            };

            // Create a label to display the matchmaking details
            Label matchmakingDetails = new Label
            {
                AutoSize = false,
                Size = new Size(280, 420),
                Location = new Point(20, 20),
                Font = new Font("Arial", 10, FontStyle.Regular),
                Text = $"Game ID: {gameId}\r\n" +
                       $"Server Name: {serverName}\r\n" +
                       $"Max Players: {maxPlayers}\r\n" +
                       $"Game Version: {gameVersion}\r\n" +
                       $"Build Version: {buildVersion}\r\n" +
                       $"Build Mode: {buildMode}\r\n" +
                       $"Platform: {platform}\r\n" +
                       $"Game Time Elapsed: {gameTimeElapsed}\r\n" +
                       $"Password Protected: {hasPassword}\r\n" +
                       $"Server ID: {serverId}\r\n" +
                       $"Headless Server: {headlessServer}\r\n" +
                       $"Mods Present: {hasMods}\r\n" +
                       $"Mod Count: {modCount}\r\n" +
                       $"Tags: {tags}",
                BorderStyle = BorderStyle.None
            };

            // Add the label and description box to the form
            matchmakingForm.Controls.Add(matchmakingDetails);
            matchmakingForm.Controls.Add(descriptionBox);

            // Show the form as a dialog
            matchmakingForm.ShowDialog();
            Trace.WriteLine("ShowMatchmakingPopup: Popup displayed.");
        }





    }
}
