using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factorio_Headless_Server_Tool
{
    public partial class DownloadProgressForm : Form
    {
        public ProgressBar ProgressBar { get; set; }
        public Button CancelButton { get; set; }
        public Label ProgressLabel { get; set; }
        public Label FileSizeLabel { get; set; }
        public bool IsCanceled { get; private set; } = false;

        public DownloadProgressForm()
        {
            InitializeComponent();
            InitializeDownloadProgressForm();
        }

        private void DownloadProgressForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeDownloadProgressForm()
        {
            this.Text = "Downloading/Extracting Update";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;

            // Initialize ProgressBar
            ProgressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Location = new Point(20, 20),
                Size = new Size(340, 25),
            };
            this.Controls.Add(ProgressBar);

            // Initialize ProgressLabel
            ProgressLabel = new Label
            {
                Text = "Progress: 0%",
                Location = new Point(20, 50),
                Size = new Size(340, 20),
            };
            this.Controls.Add(ProgressLabel);

            // Initialize FileSizeLabel
            FileSizeLabel = new Label
            {
                Text = "File Size: Calculating...",
                Location = new Point(20, 70),
                Size = new Size(340, 20),
            };
            this.Controls.Add(FileSizeLabel);

            // Initialize Cancel Button
            CancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(290, 100),
                Size = new Size(70, 25),
            };
            CancelButton.Click += (sender, e) => IsCanceled = true;
            this.Controls.Add(CancelButton);
        }

        public void UpdateDownloadProgress(long downloadedBytes, long totalBytes)
        {
            // Update the progress bar and text for download
            if (totalBytes > 0)
            {
                int percent = (int)((downloadedBytes / (double)totalBytes) * 100);
                ProgressBar.Value = percent;
                ProgressLabel.Text = $"Downloading: {percent}%";

                // Update the file size label
                FileSizeLabel.Text = $"Downloaded: {FormatBytes(downloadedBytes)} / {FormatBytes(totalBytes)}";
            }
        }

        public void UpdateExtractionProgress(int extractedFiles, int totalFiles)
        {
            // Update the progress bar and text for extraction
            if (totalFiles > 0)
            {
                int percent = (int)((extractedFiles / (double)totalFiles) * 100);
                ProgressBar.Value = percent;
                ProgressLabel.Text = $"Extracting Files: {percent}%";

                // Update the file size label to indicate extraction
                FileSizeLabel.Text = $"Extracted: {extractedFiles} / {totalFiles} files";
            }
        }

        // Changed this method from private to public so it can be accessed externally
        public string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
