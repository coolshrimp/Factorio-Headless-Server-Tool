namespace Factorio_Headless_Server_Tool
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            CreatorLBL = new Label();
            groupBox1 = new GroupBox();
            saveListView = new ListView();
            contextMenuSaves = new ContextMenuStrip(components);
            titleLabel = new Label();
            groupBox2 = new GroupBox();
            serverDetailsPanel = new Panel();
            previewdescLabel = new LinkLabel();
            saveBTN = new Button();
            resetallBTN = new Button();
            label13 = new Label();
            serverDescriptionRTXT = new RichTextBox();
            groupBox4 = new GroupBox();
            showPassBTN = new Button();
            tokenCheckBOX = new CheckBox();
            label6 = new Label();
            passTXT = new TextBox();
            label5 = new Label();
            userTXT = new TextBox();
            tagsTXT = new TextBox();
            Settingsgroupebox = new GroupBox();
            enableAchievementsCheckBox = new CheckBox();
            label15 = new Label();
            maxuploadspeedTXT = new TextBox();
            label14 = new Label();
            limitCheckBOX = new CheckBox();
            maxuploadslotsTXT = new TextBox();
            commandsDDB = new ComboBox();
            label12 = new Label();
            label10 = new Label();
            afktimeTXT = new TextBox();
            label11 = new Label();
            maxplayersTXT = new TextBox();
            label9 = new Label();
            autosaveintervalTXT = new TextBox();
            autopauseCheckBOX = new CheckBox();
            label7 = new Label();
            autosaveslotsTXT = new TextBox();
            groupBox3 = new GroupBox();
            serverpassTXT = new TextBox();
            label8 = new Label();
            verificationCheckBOX = new CheckBox();
            lanCheckBOX = new CheckBox();
            publicCheckBOX = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            serverTitleTXT = new TextBox();
            previewBox = new PictureBox();
            stopBTN = new Button();
            loadBTN = new Button();
            downloadServerFileBTN = new Label();
            exploreFolderBTN = new Button();
            adminListBTN = new Button();
            banListBTN = new Button();
            openSavesBTN = new Button();
            portTXT = new TextBox();
            label2 = new Label();
            loadedFolderTXT = new TextBox();
            toolTip1 = new ToolTip(components);
            ipLBL = new Label();
            RefreshBTN = new Button();
            logBTN = new Button();
            startBTN = new Button();
            groupBox5 = new GroupBox();
            ViewStatsLBL = new Label();
            ServerStatusTXT = new TextBox();
            groupBox6 = new GroupBox();
            AutoUpdateCheckBOX = new CheckBox();
            ServerVersionLBL = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            serverDetailsPanel.SuspendLayout();
            groupBox4.SuspendLayout();
            Settingsgroupebox.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)previewBox).BeginInit();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // CreatorLBL
            // 
            CreatorLBL.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CreatorLBL.AutoSize = true;
            CreatorLBL.Cursor = Cursors.Hand;
            CreatorLBL.Location = new Point(586, 999);
            CreatorLBL.Margin = new Padding(4, 0, 4, 0);
            CreatorLBL.Name = "CreatorLBL";
            CreatorLBL.Size = new Size(102, 15);
            CreatorLBL.TabIndex = 3;
            CreatorLBL.Text = "Coolshrimp Modz";
            CreatorLBL.Click += CreatorLBL_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(saveListView);
            groupBox1.Controls.Add(titleLabel);
            groupBox1.Location = new Point(12, 117);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(404, 872);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Saves/Scenarios";
            // 
            // saveListView
            // 
            saveListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveListView.ContextMenuStrip = contextMenuSaves;
            saveListView.FullRowSelect = true;
            saveListView.GridLines = true;
            saveListView.Location = new Point(7, 40);
            saveListView.Name = "saveListView";
            saveListView.Size = new Size(392, 826);
            saveListView.Sorting = SortOrder.Ascending;
            saveListView.TabIndex = 2;
            saveListView.UseCompatibleStateImageBehavior = false;
            saveListView.View = View.Details;
            saveListView.SelectedIndexChanged += saveListView_SelectedIndexChanged;
            // 
            // contextMenuSaves
            // 
            contextMenuSaves.Name = "contextMenuSaves";
            contextMenuSaves.Size = new Size(61, 4);
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            titleLabel.Location = new Point(7, 16);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(90, 17);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Select a save.";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBox2.Controls.Add(serverDetailsPanel);
            groupBox2.Location = new Point(422, 117);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(522, 872);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Server Details";
            // 
            // serverDetailsPanel
            // 
            serverDetailsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            serverDetailsPanel.Controls.Add(previewdescLabel);
            serverDetailsPanel.Controls.Add(saveBTN);
            serverDetailsPanel.Controls.Add(resetallBTN);
            serverDetailsPanel.Controls.Add(label13);
            serverDetailsPanel.Controls.Add(serverDescriptionRTXT);
            serverDetailsPanel.Controls.Add(groupBox4);
            serverDetailsPanel.Controls.Add(tagsTXT);
            serverDetailsPanel.Controls.Add(Settingsgroupebox);
            serverDetailsPanel.Controls.Add(groupBox3);
            serverDetailsPanel.Controls.Add(label4);
            serverDetailsPanel.Controls.Add(label3);
            serverDetailsPanel.Controls.Add(serverTitleTXT);
            serverDetailsPanel.Controls.Add(previewBox);
            serverDetailsPanel.Location = new Point(5, 19);
            serverDetailsPanel.Name = "serverDetailsPanel";
            serverDetailsPanel.Size = new Size(511, 847);
            serverDetailsPanel.TabIndex = 2;
            serverDetailsPanel.Visible = false;
            // 
            // previewdescLabel
            // 
            previewdescLabel.AutoSize = true;
            previewdescLabel.Location = new Point(70, 47);
            previewdescLabel.Name = "previewdescLabel";
            previewdescLabel.Size = new Size(48, 15);
            previewdescLabel.TabIndex = 17;
            previewdescLabel.TabStop = true;
            previewdescLabel.Text = "Preview";
            previewdescLabel.LinkClicked += previewdescLabel_LinkClicked;
            // 
            // saveBTN
            // 
            saveBTN.Location = new Point(178, 572);
            saveBTN.Name = "saveBTN";
            saveBTN.Size = new Size(79, 27);
            saveBTN.TabIndex = 16;
            saveBTN.Text = "Save";
            saveBTN.UseVisualStyleBackColor = true;
            saveBTN.Click += saveBTN_Click;
            // 
            // resetallBTN
            // 
            resetallBTN.Location = new Point(10, 572);
            resetallBTN.Name = "resetallBTN";
            resetallBTN.Size = new Size(79, 27);
            resetallBTN.TabIndex = 15;
            resetallBTN.Text = "Reset All";
            resetallBTN.UseVisualStyleBackColor = true;
            resetallBTN.Click += resetallBTN_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(4, 351);
            label13.Name = "label13";
            label13.Size = new Size(33, 15);
            label13.TabIndex = 14;
            label13.Text = "Tags:";
            // 
            // serverDescriptionRTXT
            // 
            serverDescriptionRTXT.Location = new Point(4, 65);
            serverDescriptionRTXT.MaxLength = 1000;
            serverDescriptionRTXT.Name = "serverDescriptionRTXT";
            serverDescriptionRTXT.Size = new Size(502, 274);
            serverDescriptionRTXT.TabIndex = 0;
            serverDescriptionRTXT.Text = "";
            toolTip1.SetToolTip(serverDescriptionRTXT, "Description of the game that will appear in the listing");
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(showPassBTN);
            groupBox4.Controls.Add(tokenCheckBOX);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(passTXT);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(userTXT);
            groupBox4.Location = new Point(4, 377);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(253, 108);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Credentials";
            // 
            // showPassBTN
            // 
            showPassBTN.Location = new Point(202, 81);
            showPassBTN.Name = "showPassBTN";
            showPassBTN.Size = new Size(45, 23);
            showPassBTN.TabIndex = 12;
            showPassBTN.Text = "Show";
            showPassBTN.UseVisualStyleBackColor = true;
            showPassBTN.MouseDown += showPassBTN_MouseDown;
            showPassBTN.MouseUp += showPassBTN_MouseUp;
            // 
            // tokenCheckBOX
            // 
            tokenCheckBOX.AutoSize = true;
            tokenCheckBOX.Location = new Point(166, 62);
            tokenCheckBOX.Name = "tokenCheckBOX";
            tokenCheckBOX.Size = new Size(79, 19);
            tokenCheckBOX.TabIndex = 2;
            tokenCheckBOX.Text = "Use Token";
            toolTip1.SetToolTip(tokenCheckBOX, "Authentication token. May be used instead of 'password'.");
            tokenCheckBOX.UseVisualStyleBackColor = true;
            tokenCheckBOX.CheckedChanged += tokenCheckBOX_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 63);
            label6.Name = "label6";
            label6.Size = new Size(60, 15);
            label6.TabIndex = 11;
            label6.Text = "Password:";
            // 
            // passTXT
            // 
            passTXT.Location = new Point(6, 81);
            passTXT.Name = "passTXT";
            passTXT.Size = new Size(190, 23);
            passTXT.TabIndex = 10;
            toolTip1.SetToolTip(passTXT, "Your factorio.com login credentials. Required for games with visibility public");
            passTXT.UseSystemPasswordChar = true;
            passTXT.TextChanged += passTXT_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 19);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 9;
            label5.Text = "Username:";
            // 
            // userTXT
            // 
            userTXT.Location = new Point(6, 37);
            userTXT.Name = "userTXT";
            userTXT.Size = new Size(241, 23);
            userTXT.TabIndex = 8;
            toolTip1.SetToolTip(userTXT, "Your factorio.com login credentials. Required for games with visibility public");
            userTXT.TextChanged += userTXT_TextChanged;
            // 
            // tagsTXT
            // 
            tagsTXT.Location = new Point(43, 348);
            tagsTXT.MaxLength = 1000;
            tagsTXT.Name = "tagsTXT";
            tagsTXT.Size = new Size(456, 23);
            tagsTXT.TabIndex = 8;
            tagsTXT.Tag = "Tags for the game as it will appear in the game listing (Coma Seperate Each Tag)";
            // 
            // Settingsgroupebox
            // 
            Settingsgroupebox.Controls.Add(enableAchievementsCheckBox);
            Settingsgroupebox.Controls.Add(label15);
            Settingsgroupebox.Controls.Add(maxuploadspeedTXT);
            Settingsgroupebox.Controls.Add(label14);
            Settingsgroupebox.Controls.Add(limitCheckBOX);
            Settingsgroupebox.Controls.Add(maxuploadslotsTXT);
            Settingsgroupebox.Controls.Add(commandsDDB);
            Settingsgroupebox.Controls.Add(label12);
            Settingsgroupebox.Controls.Add(label10);
            Settingsgroupebox.Controls.Add(afktimeTXT);
            Settingsgroupebox.Controls.Add(label11);
            Settingsgroupebox.Controls.Add(maxplayersTXT);
            Settingsgroupebox.Controls.Add(label9);
            Settingsgroupebox.Controls.Add(autosaveintervalTXT);
            Settingsgroupebox.Controls.Add(autopauseCheckBOX);
            Settingsgroupebox.Controls.Add(label7);
            Settingsgroupebox.Controls.Add(autosaveslotsTXT);
            Settingsgroupebox.Location = new Point(263, 377);
            Settingsgroupebox.Name = "Settingsgroupebox";
            Settingsgroupebox.Size = new Size(243, 240);
            Settingsgroupebox.TabIndex = 6;
            Settingsgroupebox.TabStop = false;
            Settingsgroupebox.Text = "Settings";
            // 
            // enableAchievementsCheckBox
            // 
            enableAchievementsCheckBox.AutoSize = true;
            enableAchievementsCheckBox.Location = new Point(57, 215);
            enableAchievementsCheckBox.Name = "enableAchievementsCheckBox";
            enableAchievementsCheckBox.Size = new Size(180, 19);
            enableAchievementsCheckBox.TabIndex = 26;
            enableAchievementsCheckBox.Text = "Enable Achievements (Patch)";
            toolTip1.SetToolTip(enableAchievementsCheckBox, "This will remove the cheat flag and turn achievements back on, even if you've run commands.");
            enableAchievementsCheckBox.UseVisualStyleBackColor = true;
            enableAchievementsCheckBox.CheckedChanged += enableAchievementsCheckBox_CheckedChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(87, 170);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 25;
            label15.Text = "Max Upload (kbps)";
            // 
            // maxuploadspeedTXT
            // 
            maxuploadspeedTXT.Location = new Point(195, 167);
            maxuploadspeedTXT.Name = "maxuploadspeedTXT";
            maxuploadspeedTXT.RightToLeft = RightToLeft.Yes;
            maxuploadspeedTXT.Size = new Size(40, 23);
            maxuploadspeedTXT.TabIndex = 24;
            maxuploadspeedTXT.Text = "0";
            toolTip1.SetToolTip(maxuploadspeedTXT, "optional, default value is 0. 0 means unlimited.");
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(87, 146);
            label14.Name = "label14";
            label14.Size = new Size(102, 15);
            label14.TabIndex = 23;
            label14.Text = "Max Upload Slots:";
            // 
            // limitCheckBOX
            // 
            limitCheckBOX.AutoSize = true;
            limitCheckBOX.Location = new Point(57, 193);
            limitCheckBOX.Name = "limitCheckBOX";
            limitCheckBOX.Size = new Size(90, 19);
            limitCheckBOX.TabIndex = 21;
            limitCheckBOX.Text = "Ignore Limit";
            toolTip1.SetToolTip(limitCheckBOX, "Players that played on this map already can join even when the max player limit was reached.");
            limitCheckBOX.UseVisualStyleBackColor = true;
            // 
            // maxuploadslotsTXT
            // 
            maxuploadslotsTXT.Location = new Point(195, 143);
            maxuploadslotsTXT.Name = "maxuploadslotsTXT";
            maxuploadslotsTXT.RightToLeft = RightToLeft.Yes;
            maxuploadslotsTXT.Size = new Size(40, 23);
            maxuploadslotsTXT.TabIndex = 22;
            maxuploadslotsTXT.Text = "5";
            toolTip1.SetToolTip(maxuploadslotsTXT, "optional, default value is 5. 0 means unlimited.");
            // 
            // commandsDDB
            // 
            commandsDDB.FormattingEnabled = true;
            commandsDDB.Items.AddRange(new object[] { "admins-only", "true", "false" });
            commandsDDB.Location = new Point(116, 15);
            commandsDDB.Name = "commandsDDB";
            commandsDDB.Size = new Size(121, 23);
            commandsDDB.TabIndex = 20;
            commandsDDB.Text = "admins-only";
            toolTip1.SetToolTip(commandsDDB, "Allow players to use commands in console.");
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 19);
            label12.Name = "label12";
            label12.Size = new Size(108, 15);
            label12.TabIndex = 19;
            label12.Text = "Allow Commands: ";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(74, 71);
            label10.Name = "label10";
            label10.Size = new Size(117, 15);
            label10.TabIndex = 18;
            label10.Text = "AFK Auto Kick (Min):";
            // 
            // afktimeTXT
            // 
            afktimeTXT.Location = new Point(195, 69);
            afktimeTXT.Name = "afktimeTXT";
            afktimeTXT.RightToLeft = RightToLeft.Yes;
            afktimeTXT.Size = new Size(40, 23);
            afktimeTXT.TabIndex = 17;
            afktimeTXT.Text = "0";
            toolTip1.SetToolTip(afktimeTXT, "How many minutes until someone is kicked when doing nothing, 0 for never.");
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(118, 46);
            label11.Name = "label11";
            label11.Size = new Size(73, 15);
            label11.TabIndex = 16;
            label11.Text = "Max Players:";
            // 
            // maxplayersTXT
            // 
            maxplayersTXT.Location = new Point(195, 44);
            maxplayersTXT.Name = "maxplayersTXT";
            maxplayersTXT.RightToLeft = RightToLeft.Yes;
            maxplayersTXT.Size = new Size(40, 23);
            maxplayersTXT.TabIndex = 15;
            maxplayersTXT.Text = "0";
            toolTip1.SetToolTip(maxplayersTXT, "Maximum number of players allowed, admins can join even a full server. 0 means unlimited.");
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(54, 121);
            label9.Name = "label9";
            label9.Size = new Size(137, 15);
            label9.TabIndex = 14;
            label9.Text = "Auto Save Interval (Min):";
            // 
            // autosaveintervalTXT
            // 
            autosaveintervalTXT.Location = new Point(195, 119);
            autosaveintervalTXT.Name = "autosaveintervalTXT";
            autosaveintervalTXT.RightToLeft = RightToLeft.Yes;
            autosaveintervalTXT.Size = new Size(40, 23);
            autosaveintervalTXT.TabIndex = 13;
            autosaveintervalTXT.Text = "10";
            toolTip1.SetToolTip(autosaveintervalTXT, "Autosave interval in minutes");
            // 
            // autopauseCheckBOX
            // 
            autopauseCheckBOX.AutoSize = true;
            autopauseCheckBOX.Checked = true;
            autopauseCheckBOX.CheckState = CheckState.Checked;
            autopauseCheckBOX.Location = new Point(151, 193);
            autopauseCheckBOX.Name = "autopauseCheckBOX";
            autopauseCheckBOX.Size = new Size(86, 19);
            autopauseCheckBOX.TabIndex = 12;
            autopauseCheckBOX.Text = "Auto Pause";
            toolTip1.SetToolTip(autopauseCheckBOX, "Whether should the server be paused when no players are present.");
            autopauseCheckBOX.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(98, 96);
            label7.Name = "label7";
            label7.Size = new Size(91, 15);
            label7.TabIndex = 11;
            label7.Text = "Auto Save Slots:";
            // 
            // autosaveslotsTXT
            // 
            autosaveslotsTXT.Location = new Point(195, 94);
            autosaveslotsTXT.Name = "autosaveslotsTXT";
            autosaveslotsTXT.RightToLeft = RightToLeft.Yes;
            autosaveslotsTXT.Size = new Size(40, 23);
            autosaveslotsTXT.TabIndex = 10;
            autosaveslotsTXT.Text = "5";
            toolTip1.SetToolTip(autosaveslotsTXT, "server autosave slots, it is cycled through when the server autosaves.");
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(serverpassTXT);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(verificationCheckBOX);
            groupBox3.Controls.Add(lanCheckBOX);
            groupBox3.Controls.Add(publicCheckBOX);
            groupBox3.Location = new Point(10, 487);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(247, 77);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Visibility";
            // 
            // serverpassTXT
            // 
            serverpassTXT.Location = new Point(153, 45);
            serverpassTXT.Name = "serverpassTXT";
            serverpassTXT.RightToLeft = RightToLeft.Yes;
            serverpassTXT.Size = new Size(88, 23);
            serverpassTXT.TabIndex = 12;
            toolTip1.SetToolTip(serverpassTXT, "Maximum number of players allowed, admins can join even a full server. 0 means unlimited.");
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(59, 48);
            label8.Name = "label8";
            label8.Size = new Size(95, 15);
            label8.TabIndex = 13;
            label8.Text = "Server Password:";
            // 
            // verificationCheckBOX
            // 
            verificationCheckBOX.AutoSize = true;
            verificationCheckBOX.Location = new Point(87, 22);
            verificationCheckBOX.Name = "verificationCheckBOX";
            verificationCheckBOX.RightToLeft = RightToLeft.Yes;
            verificationCheckBOX.Size = new Size(154, 19);
            verificationCheckBOX.TabIndex = 8;
            verificationCheckBOX.Text = "Require User Verification";
            toolTip1.SetToolTip(verificationCheckBOX, "When checked, the server will only allow clients that have a valid Factorio.com account");
            verificationCheckBOX.UseVisualStyleBackColor = true;
            // 
            // lanCheckBOX
            // 
            lanCheckBOX.AutoSize = true;
            lanCheckBOX.Location = new Point(6, 47);
            lanCheckBOX.Name = "lanCheckBOX";
            lanCheckBOX.Size = new Size(45, 19);
            lanCheckBOX.TabIndex = 1;
            lanCheckBOX.Text = "Lan";
            toolTip1.SetToolTip(lanCheckBOX, "Game will be broadcast on LAN");
            lanCheckBOX.UseVisualStyleBackColor = true;
            // 
            // publicCheckBOX
            // 
            publicCheckBOX.AutoSize = true;
            publicCheckBOX.Location = new Point(6, 22);
            publicCheckBOX.Name = "publicCheckBOX";
            publicCheckBOX.Size = new Size(59, 19);
            publicCheckBOX.TabIndex = 0;
            publicCheckBOX.Text = "Public";
            toolTip1.SetToolTip(publicCheckBOX, "Game will be published on the official Factorio matching server");
            publicCheckBOX.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 47);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 4;
            label4.Text = "Description:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(0, 3);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 3;
            label3.Text = "Title:";
            // 
            // serverTitleTXT
            // 
            serverTitleTXT.Location = new Point(4, 21);
            serverTitleTXT.MaxLength = 80;
            serverTitleTXT.Name = "serverTitleTXT";
            serverTitleTXT.Size = new Size(502, 23);
            serverTitleTXT.TabIndex = 1;
            toolTip1.SetToolTip(serverTitleTXT, "Name of the game as it will appear in the game listing");
            // 
            // previewBox
            // 
            previewBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            previewBox.BackgroundImageLayout = ImageLayout.Zoom;
            previewBox.Location = new Point(3, 737);
            previewBox.Name = "previewBox";
            previewBox.Size = new Size(117, 107);
            previewBox.SizeMode = PictureBoxSizeMode.Zoom;
            previewBox.TabIndex = 0;
            previewBox.TabStop = false;
            // 
            // stopBTN
            // 
            stopBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            stopBTN.Enabled = false;
            stopBTN.Location = new Point(146, 72);
            stopBTN.Name = "stopBTN";
            stopBTN.Size = new Size(75, 25);
            stopBTN.TabIndex = 6;
            stopBTN.Text = "■ Stop";
            stopBTN.UseVisualStyleBackColor = true;
            stopBTN.Click += stopBTN_Click;
            // 
            // loadBTN
            // 
            loadBTN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            loadBTN.Location = new Point(612, 17);
            loadBTN.Name = "loadBTN";
            loadBTN.Size = new Size(75, 31);
            loadBTN.TabIndex = 7;
            loadBTN.Text = "Load";
            loadBTN.UseVisualStyleBackColor = true;
            loadBTN.Click += loadBTN_Click;
            // 
            // downloadServerFileBTN
            // 
            downloadServerFileBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            downloadServerFileBTN.AutoSize = true;
            downloadServerFileBTN.Cursor = Cursors.Hand;
            downloadServerFileBTN.ForeColor = Color.CornflowerBlue;
            downloadServerFileBTN.Location = new Point(571, 82);
            downloadServerFileBTN.Margin = new Padding(4, 0, 4, 0);
            downloadServerFileBTN.Name = "downloadServerFileBTN";
            downloadServerFileBTN.Size = new Size(115, 15);
            downloadServerFileBTN.TabIndex = 8;
            downloadServerFileBTN.Text = "Dowload Server Files";
            downloadServerFileBTN.Click += downloadServerFileBTN_Click;
            // 
            // exploreFolderBTN
            // 
            exploreFolderBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exploreFolderBTN.Location = new Point(818, 995);
            exploreFolderBTN.Name = "exploreFolderBTN";
            exploreFolderBTN.Size = new Size(124, 23);
            exploreFolderBTN.TabIndex = 9;
            exploreFolderBTN.Text = "Open Server Folder";
            exploreFolderBTN.UseVisualStyleBackColor = true;
            exploreFolderBTN.Click += button4_Click;
            // 
            // adminListBTN
            // 
            adminListBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            adminListBTN.Location = new Point(12, 995);
            adminListBTN.Name = "adminListBTN";
            adminListBTN.Size = new Size(75, 23);
            adminListBTN.TabIndex = 10;
            adminListBTN.Text = "Admin List";
            adminListBTN.UseVisualStyleBackColor = true;
            adminListBTN.Click += adminListBTN_Click;
            // 
            // banListBTN
            // 
            banListBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            banListBTN.Location = new Point(88, 995);
            banListBTN.Name = "banListBTN";
            banListBTN.Size = new Size(56, 23);
            banListBTN.TabIndex = 11;
            banListBTN.Text = "Ban List";
            banListBTN.UseVisualStyleBackColor = true;
            banListBTN.Click += banListBTN_Click;
            // 
            // openSavesBTN
            // 
            openSavesBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            openSavesBTN.Location = new Point(695, 995);
            openSavesBTN.Name = "openSavesBTN";
            openSavesBTN.Size = new Size(118, 23);
            openSavesBTN.TabIndex = 12;
            openSavesBTN.Text = "Open Saves Folder";
            openSavesBTN.UseVisualStyleBackColor = true;
            openSavesBTN.Click += openSavesBTN_Click;
            // 
            // portTXT
            // 
            portTXT.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            portTXT.Location = new Point(219, 995);
            portTXT.Name = "portTXT";
            portTXT.Size = new Size(52, 23);
            portTXT.TabIndex = 13;
            portTXT.Text = "34197";
            toolTip1.SetToolTip(portTXT, "Remember to Port Forward. \\n \\n Defauly Port: 34197");
            portTXT.TextChanged += portTXT_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(186, 999);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 14;
            label2.Text = "Port:";
            // 
            // loadedFolderTXT
            // 
            loadedFolderTXT.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            loadedFolderTXT.Enabled = false;
            loadedFolderTXT.Location = new Point(6, 22);
            loadedFolderTXT.Name = "loadedFolderTXT";
            loadedFolderTXT.Size = new Size(560, 23);
            loadedFolderTXT.TabIndex = 15;
            loadedFolderTXT.TextChanged += loadedFolderTXT_TextChanged;
            // 
            // ipLBL
            // 
            ipLBL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ipLBL.AutoSize = true;
            ipLBL.Location = new Point(277, 999);
            ipLBL.Name = "ipLBL";
            ipLBL.Size = new Size(62, 15);
            ipLBL.TabIndex = 16;
            ipLBL.Text = "IP Address";
            ipLBL.MouseEnter += ipLBL_MouseEnter;
            ipLBL.MouseLeave += ipLBL_MouseLeave;
            // 
            // RefreshBTN
            // 
            RefreshBTN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RefreshBTN.BackgroundImageLayout = ImageLayout.Stretch;
            RefreshBTN.Image = (Image)resources.GetObject("RefreshBTN.Image");
            RefreshBTN.Location = new Point(572, 16);
            RefreshBTN.Name = "RefreshBTN";
            RefreshBTN.Size = new Size(34, 32);
            RefreshBTN.TabIndex = 17;
            RefreshBTN.UseVisualStyleBackColor = true;
            RefreshBTN.Click += RefreshBTN_Click;
            // 
            // logBTN
            // 
            logBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            logBTN.Location = new Point(147, 995);
            logBTN.Name = "logBTN";
            logBTN.Size = new Size(38, 23);
            logBTN.TabIndex = 18;
            logBTN.Text = "Log";
            logBTN.UseVisualStyleBackColor = true;
            logBTN.Click += logBTN_Click;
            // 
            // startBTN
            // 
            startBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            startBTN.Enabled = false;
            startBTN.Location = new Point(7, 72);
            startBTN.Name = "startBTN";
            startBTN.Size = new Size(75, 25);
            startBTN.TabIndex = 20;
            startBTN.Text = "▶ Start";
            startBTN.UseVisualStyleBackColor = true;
            startBTN.Click += startBTN_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(ViewStatsLBL);
            groupBox5.Controls.Add(ServerStatusTXT);
            groupBox5.Controls.Add(startBTN);
            groupBox5.Controls.Add(stopBTN);
            groupBox5.Location = new Point(12, 7);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(227, 104);
            groupBox5.TabIndex = 21;
            groupBox5.TabStop = false;
            groupBox5.Text = "Server Status";
            // 
            // ViewStatsLBL
            // 
            ViewStatsLBL.AutoSize = true;
            ViewStatsLBL.ForeColor = SystemColors.MenuHighlight;
            ViewStatsLBL.Location = new Point(85, 77);
            ViewStatsLBL.Name = "ViewStatsLBL";
            ViewStatsLBL.Size = new Size(60, 15);
            ViewStatsLBL.TabIndex = 22;
            ViewStatsLBL.Text = "View Stats";
            ViewStatsLBL.Click += ViewStatsLBL_Click;
            // 
            // ServerStatusTXT
            // 
            ServerStatusTXT.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ServerStatusTXT.BackColor = SystemColors.Control;
            ServerStatusTXT.Location = new Point(7, 19);
            ServerStatusTXT.Multiline = true;
            ServerStatusTXT.Name = "ServerStatusTXT";
            ServerStatusTXT.ReadOnly = true;
            ServerStatusTXT.Size = new Size(214, 47);
            ServerStatusTXT.TabIndex = 21;
            // 
            // groupBox6
            // 
            groupBox6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox6.Controls.Add(AutoUpdateCheckBOX);
            groupBox6.Controls.Add(ServerVersionLBL);
            groupBox6.Controls.Add(loadedFolderTXT);
            groupBox6.Controls.Add(RefreshBTN);
            groupBox6.Controls.Add(loadBTN);
            groupBox6.Controls.Add(downloadServerFileBTN);
            groupBox6.Location = new Point(249, 7);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(693, 104);
            groupBox6.TabIndex = 22;
            groupBox6.TabStop = false;
            groupBox6.Text = "Server Files";
            // 
            // AutoUpdateCheckBOX
            // 
            AutoUpdateCheckBOX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AutoUpdateCheckBOX.AutoSize = true;
            AutoUpdateCheckBOX.CheckAlign = ContentAlignment.MiddleRight;
            AutoUpdateCheckBOX.Location = new Point(586, 54);
            AutoUpdateCheckBOX.Name = "AutoUpdateCheckBOX";
            AutoUpdateCheckBOX.Size = new Size(98, 19);
            AutoUpdateCheckBOX.TabIndex = 19;
            AutoUpdateCheckBOX.Text = "Auto Update?";
            AutoUpdateCheckBOX.UseVisualStyleBackColor = true;
            AutoUpdateCheckBOX.CheckedChanged += AutoUpdateCheckBOX_CheckedChanged;
            // 
            // ServerVersionLBL
            // 
            ServerVersionLBL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ServerVersionLBL.AutoSize = true;
            ServerVersionLBL.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            ServerVersionLBL.Location = new Point(6, 77);
            ServerVersionLBL.Name = "ServerVersionLBL";
            ServerVersionLBL.Size = new Size(201, 15);
            ServerVersionLBL.TabIndex = 18;
            ServerVersionLBL.Text = "Server Version: No Folder Selected";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 1028);
            Controls.Add(groupBox6);
            Controls.Add(groupBox5);
            Controls.Add(logBTN);
            Controls.Add(ipLBL);
            Controls.Add(label2);
            Controls.Add(portTXT);
            Controls.Add(openSavesBTN);
            Controls.Add(banListBTN);
            Controls.Add(adminListBTN);
            Controls.Add(exploreFolderBTN);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(CreatorLBL);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(970, 1067);
            MinimumSize = new Size(829, 969);
            Name = "Main";
            Text = "Factorio Headless Server Tool - Coolshrimp Modz v1.3.1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            serverDetailsPanel.ResumeLayout(false);
            serverDetailsPanel.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            Settingsgroupebox.ResumeLayout(false);
            Settingsgroupebox.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)previewBox).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox saveListBox;
        private Label CreatorLBL;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button stopBTN;
        private Button loadBTN;
        private Label downloadServerFileBTN;
        private Button exploreFolderBTN;
        private PictureBox previewBox;
        private Label titleLabel;
        private Button adminListBTN;
        private Button banListBTN;
        private Button openSavesBTN;
        private TextBox portTXT;
        private Label label2;
        private TextBox loadedFolderTXT;
        private Panel panel1;
        private Label label4;
        private Label label3;
        private TextBox serverDescriptionTXT;
        private TextBox serverTitleTXT;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private CheckBox lanCheckBOX;
        private CheckBox publicCheckBOX;
        private ToolTip toolTip1;
        private CheckBox tokenCheckBOX;
        private Label label6;
        private TextBox passTXT;
        private Label label5;
        private TextBox userTXT;
        private GroupBox Settingsgroupebox;
        private Button showPassBTN;
        private RichTextBox richTextBox1;
        private RichTextBox serverDescriptionRTXT;
        private Panel serverDetailsPanel;
        private Label label7;
        private TextBox autosaveslotsTXT;
        private Label label8;
        private TextBox serverpassTXT;
        private CheckBox verificationCheckBOX;
        private CheckBox autopauseCheckBOX;
        private Label label9;
        private TextBox autosaveintervalTXT;
        private Label label10;
        private TextBox afktimeTXT;
        private Label label11;
        private TextBox maxplayersTXT;
        private Label label12;
        private ComboBox commandsDDB;
        private CheckBox limitCheckBOX;
        private Label label13;
        private TextBox tagsTXT;
        private Label label14;
        private TextBox maxuploadslotsTXT;
        private Label label15;
        private TextBox maxuploadspeedTXT;
        private Button button2;
        private Button resetallBTN;
        private Button saveBTN;
        private LinkLabel previewdescLabel;
        private Label ipLBL;
        private CheckBox enableAchievementsCheckBox;
        private Button RefreshBTN;
        private ListView saveListView;
        private ColumnHeader columnSaveNaMe;
        private ColumnHeader columnDate;
        private ContextMenuStrip contextMenuSaves;
        private Button logBTN;
        private Button startBTN;
        private GroupBox groupBox5;
        private TextBox ServerStatusTXT;
        private GroupBox groupBox6;
        private Label ServerVersionLBL;
        private CheckBox AutoUpdateCheckBOX;
        private Label ViewStatsLBL;
    }
}