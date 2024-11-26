namespace Factorio_Headless_Server_Tool
{
    partial class consoleLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            clearBTN = new Button();
            hideBTN = new Button();
            consoleTXT = new TextBox();
            SuspendLayout();
            // 
            // clearBTN
            // 
            clearBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            clearBTN.Location = new Point(691, 780);
            clearBTN.Name = "clearBTN";
            clearBTN.Size = new Size(75, 23);
            clearBTN.TabIndex = 5;
            clearBTN.Text = "Clear";
            clearBTN.UseVisualStyleBackColor = true;
            clearBTN.Click += clearBTN_Click;
            // 
            // hideBTN
            // 
            hideBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            hideBTN.Location = new Point(6, 780);
            hideBTN.Name = "hideBTN";
            hideBTN.Size = new Size(75, 23);
            hideBTN.TabIndex = 4;
            hideBTN.Text = "Hide";
            hideBTN.UseVisualStyleBackColor = true;
            hideBTN.Click += hideBTN_Click;
            // 
            // consoleTXT
            // 
            consoleTXT.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            consoleTXT.BackColor = Color.Black;
            consoleTXT.ForeColor = Color.FromArgb(209, 140, 71);
            consoleTXT.Location = new Point(6, 12);
            consoleTXT.Multiline = true;
            consoleTXT.Name = "consoleTXT";
            consoleTXT.ReadOnly = true;
            consoleTXT.ScrollBars = ScrollBars.Both;
            consoleTXT.Size = new Size(760, 762);
            consoleTXT.TabIndex = 6;
            consoleTXT.WordWrap = false;
            consoleTXT.TextChanged += consoleTXT_TextChanged;
            // 
            // consoleLog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 815);
            Controls.Add(consoleTXT);
            Controls.Add(clearBTN);
            Controls.Add(hideBTN);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "consoleLog";
            Text = "consoleLog";
            Load += consoleLog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button clearBTN;
        private Button hideBTN;
        private TextBox consoleTXT;
    }
}