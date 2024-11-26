using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Factorio_Headless_Server_Tool
{
    public partial class consoleLog : Form
    {
        private List<string> consoleLogMessages = new List<string>();
        private Form mainForm; // Reference to the main form

        public consoleLog()
        {
            InitializeComponent();
            // Set the text box to read-only to prevent user input
            consoleTXT.ReadOnly = true;

            // Attach the FormClosing event handler
            this.FormClosing += consoleLog_FormClosing;

            // Attach the Shown event handler to reposition when the form is shown
            this.Shown += consoleLog_Shown;
        }

        /// <summary>
        /// Writes a message to the console log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteToConsole(string message)
        {
            if (consoleTXT.IsDisposed || !consoleTXT.IsHandleCreated)
            {
                return;  // Exit if the control is no longer valid
            }

            if (consoleTXT.InvokeRequired)
            {
                consoleTXT.Invoke(new MethodInvoker(() => WriteToConsole(message)));
            }
            else
            {
                // Add message to the list and display in the text box
                message = message.Trim();  // Remove extra line breaks
                consoleLogMessages.Add(message);
                if (!consoleTXT.Text.EndsWith(Environment.NewLine))
                {
                    consoleTXT.AppendText(Environment.NewLine);
                }
                consoleTXT.AppendText(message + Environment.NewLine);
            }
        }

        /// <summary>
        /// Replays all log messages in the console text box.
        /// </summary>
        public void ReplayLogMessages()
        {
            if (consoleTXT.IsDisposed || !consoleTXT.IsHandleCreated)
            {
                Trace.WriteLine("Attempted to replay messages to a disposed or uncreated TextBox. Operation aborted.");
                return;  // Exit if the control is no longer valid
            }

            if (consoleTXT.InvokeRequired)
            {
                consoleTXT.Invoke(new MethodInvoker(() => ReplayLogMessages()));
            }
            else
            {
                consoleTXT.Clear();  // Clear current text and replay all messages
                foreach (string message in consoleLogMessages)
                {
                    consoleTXT.AppendText(message.Trim() + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Handles the form closing event to hide the form instead of closing it.
        /// </summary>
        private void consoleLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the close operation
                this.Hide();     // Hide the form instead
            }
        }

        /// <summary>
        /// Clears the console text box and stored messages.
        /// </summary>
        private void clearBTN_Click(object sender, EventArgs e)
        {
            consoleTXT.Clear();
            consoleLogMessages.Clear();
        }

        /// <summary>
        /// Hides the console log form.
        /// </summary>
        private void hideBTN_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void consoleTXT_TextChanged(object sender, EventArgs e)
        {
            // Handle any specific changes if needed when the text changes
        }

        /// <summary>
        /// Snaps the console log form to the right edge of the main form and matches its height.
        /// </summary>
        /// <param name="mainForm">The main form to snap to.</param>
        public void SnapToMainForm(Form mainForm)
        {
            if (mainForm == null)
                throw new ArgumentNullException(nameof(mainForm));

            this.mainForm = mainForm;

            // Set StartPosition to Manual
            this.StartPosition = FormStartPosition.Manual;

            // Set a fixed width for the console log window
            this.Width = 600; // Adjust as needed

            // Match the height of the main form
            this.Height = mainForm.Height;

            // Position the console form to the right of the main form
            PositionConsoleForm();

            // Set TopMost property if needed
            //this.TopMost = true;

            // Show the form
            this.Show();

            // Attach event handlers to keep console form in sync with main form movement
            mainForm.LocationChanged -= MainForm_LocationChanged; // Prevent multiple attachments
            mainForm.LocationChanged += MainForm_LocationChanged;

            mainForm.FormClosed -= MainForm_FormClosed; // Prevent multiple attachments
            mainForm.FormClosed += MainForm_FormClosed;
        }

        /// <summary>
        /// Positions the console log form to the right of the main form.
        /// </summary>
        private void PositionConsoleForm()
        {
            if (mainForm == null)
                return;

            // Calculate the position to align with the right edge of the main form
            int newX = mainForm.Location.X + mainForm.Width;
            int newY = mainForm.Location.Y;

            // Get screen working area to prevent going off-screen
            Rectangle workingArea = Screen.GetWorkingArea(mainForm);

            // Ensure the console form fits within the screen boundaries
            if (newX + this.Width > workingArea.Right)
            {
                newX = mainForm.Location.X - this.Width; // Place it to the left if not enough space
                if (newX < workingArea.Left) newX = workingArea.Left;
            }

            // Keep the Y-position aligned with the main form's top edge
            if (newY + this.Height > workingArea.Bottom)
            {
                newY = workingArea.Bottom - this.Height;
            }
            else if (newY < workingArea.Top)
            {
                newY = workingArea.Top;
            }

            this.Location = new Point(newX, newY);
        }


        /// <summary>
        /// Handles the main form's LocationChanged event to reposition the consoleLog form.
        /// </summary>
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            PositionConsoleForm();
        }

        /// <summary>
        /// Handles the main form's FormClosed event to close the consoleLog form as well.
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Shown event to reposition the consoleLog form whenever it is shown.
        /// </summary>
        private void consoleLog_Shown(object sender, EventArgs e)
        {
            PositionConsoleForm();
        }

        private void consoleLog_Load(object sender, EventArgs e)
        {
            // Any additional initialization if needed
        }
    }
}
