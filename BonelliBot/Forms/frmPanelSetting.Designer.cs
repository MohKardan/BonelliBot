namespace BonelliBot.Forms
{
    partial class frmPanelSetting
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveTargetToFile = new System.Windows.Forms.Button();
            this.trbUnfollowPerHour = new System.Windows.Forms.TrackBar();
            this.lblUnfollowPerHour = new System.Windows.Forms.Label();
            this.lblFollowPerHour = new System.Windows.Forms.Label();
            this.trbFollowPerHour = new System.Windows.Forms.TrackBar();
            this.lblProxyInfo = new System.Windows.Forms.Label();
            this.chkProxy = new System.Windows.Forms.CheckBox();
            this.btnSaveWhiteListToFile = new System.Windows.Forms.Button();
            this.btnCreateWhiteList444 = new System.Windows.Forms.Button();
            this.btnCreateWhiteList = new System.Windows.Forms.Button();
            this.chkTargetLikers = new System.Windows.Forms.CheckBox();
            this.chkTargetFollowers = new System.Windows.Forms.CheckBox();
            this.chkUnfollowNonefollowers = new System.Windows.Forms.CheckBox();
            this.chkFollowPrivateAccounts = new System.Windows.Forms.CheckBox();
            this.btnLoadWhiteFile = new System.Windows.Forms.Button();
            this.btnAddToWhileList = new System.Windows.Forms.Button();
            this.btnClearWhiteList = new System.Windows.Forms.Button();
            this.btnRemoveWhiteItem = new System.Windows.Forms.Button();
            this.txtWhiteList = new System.Windows.Forms.TextBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lblWhitelistCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbtWhiteList = new System.Windows.Forms.ListBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMinFollower = new System.Windows.Forms.TextBox();
            this.txtMaxFollower = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTargetPage = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lbtTargetPage = new System.Windows.Forms.ListBox();
            this.chkPrivateAccount = new System.Windows.Forms.CheckBox();
            this.chkProfilePic = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtfComment = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbUnfollowPerHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbFollowPerHour)).BeginInit();
            this.panel10.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(184)))), ((int)(((byte)(225)))));
            this.tabPage1.Controls.Add(this.btnSaveTargetToFile);
            this.tabPage1.Controls.Add(this.trbUnfollowPerHour);
            this.tabPage1.Controls.Add(this.lblUnfollowPerHour);
            this.tabPage1.Controls.Add(this.lblFollowPerHour);
            this.tabPage1.Controls.Add(this.trbFollowPerHour);
            this.tabPage1.Controls.Add(this.lblProxyInfo);
            this.tabPage1.Controls.Add(this.chkProxy);
            this.tabPage1.Controls.Add(this.btnSaveWhiteListToFile);
            this.tabPage1.Controls.Add(this.btnCreateWhiteList444);
            this.tabPage1.Controls.Add(this.btnCreateWhiteList);
            this.tabPage1.Controls.Add(this.chkTargetLikers);
            this.tabPage1.Controls.Add(this.chkTargetFollowers);
            this.tabPage1.Controls.Add(this.chkUnfollowNonefollowers);
            this.tabPage1.Controls.Add(this.chkFollowPrivateAccounts);
            this.tabPage1.Controls.Add(this.btnLoadWhiteFile);
            this.tabPage1.Controls.Add(this.btnAddToWhileList);
            this.tabPage1.Controls.Add(this.btnClearWhiteList);
            this.tabPage1.Controls.Add(this.btnRemoveWhiteItem);
            this.tabPage1.Controls.Add(this.txtWhiteList);
            this.tabPage1.Controls.Add(this.panel10);
            this.tabPage1.Controls.Add(this.lbtWhiteList);
            this.tabPage1.Controls.Add(this.btnLoadFile);
            this.tabPage1.Controls.Add(this.btnAddToList);
            this.tabPage1.Controls.Add(this.btnClearList);
            this.tabPage1.Controls.Add(this.btnRemoveItem);
            this.tabPage1.Controls.Add(this.panel7);
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Controls.Add(this.txtMinFollower);
            this.tabPage1.Controls.Add(this.txtMaxFollower);
            this.tabPage1.Controls.Add(this.panel5);
            this.tabPage1.Controls.Add(this.txtTargetPage);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.lbtTargetPage);
            this.tabPage1.Controls.Add(this.chkPrivateAccount);
            this.tabPage1.Controls.Add(this.chkProfilePic);
            this.tabPage1.ForeColor = System.Drawing.Color.White;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "تنظیمات فالو/ آنفالو";
            // 
            // btnSaveTargetToFile
            // 
            this.btnSaveTargetToFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnSaveTargetToFile.FlatAppearance.BorderSize = 0;
            this.btnSaveTargetToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveTargetToFile.Location = new System.Drawing.Point(724, 241);
            this.btnSaveTargetToFile.Name = "btnSaveTargetToFile";
            this.btnSaveTargetToFile.Size = new System.Drawing.Size(60, 36);
            this.btnSaveTargetToFile.TabIndex = 40;
            this.btnSaveTargetToFile.Text = "ذخیره در فایل";
            this.btnSaveTargetToFile.UseVisualStyleBackColor = false;
            this.btnSaveTargetToFile.Click += new System.EventHandler(this.btnSaveTargetToFile_Click);
            // 
            // trbUnfollowPerHour
            // 
            this.trbUnfollowPerHour.LargeChange = 1;
            this.trbUnfollowPerHour.Location = new System.Drawing.Point(270, 373);
            this.trbUnfollowPerHour.Maximum = 100;
            this.trbUnfollowPerHour.Minimum = 1;
            this.trbUnfollowPerHour.Name = "trbUnfollowPerHour";
            this.trbUnfollowPerHour.Size = new System.Drawing.Size(188, 45);
            this.trbUnfollowPerHour.TabIndex = 39;
            this.trbUnfollowPerHour.Value = 1;
            this.trbUnfollowPerHour.ValueChanged += new System.EventHandler(this.trbUnfollowPerHour_ValueChanged);
            // 
            // lblUnfollowPerHour
            // 
            this.lblUnfollowPerHour.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.lblUnfollowPerHour.Location = new System.Drawing.Point(270, 329);
            this.lblUnfollowPerHour.Name = "lblUnfollowPerHour";
            this.lblUnfollowPerHour.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUnfollowPerHour.Size = new System.Drawing.Size(190, 41);
            this.lblUnfollowPerHour.TabIndex = 38;
            this.lblUnfollowPerHour.Text = "تعداد آنفالو در ساعت";
            this.lblUnfollowPerHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFollowPerHour
            // 
            this.lblFollowPerHour.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.lblFollowPerHour.Location = new System.Drawing.Point(530, 329);
            this.lblFollowPerHour.Name = "lblFollowPerHour";
            this.lblFollowPerHour.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFollowPerHour.Size = new System.Drawing.Size(190, 41);
            this.lblFollowPerHour.TabIndex = 0;
            this.lblFollowPerHour.Text = "تعداد فالو در ساعت";
            this.lblFollowPerHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trbFollowPerHour
            // 
            this.trbFollowPerHour.LargeChange = 1;
            this.trbFollowPerHour.Location = new System.Drawing.Point(529, 373);
            this.trbFollowPerHour.Maximum = 100;
            this.trbFollowPerHour.Minimum = 1;
            this.trbFollowPerHour.Name = "trbFollowPerHour";
            this.trbFollowPerHour.Size = new System.Drawing.Size(188, 45);
            this.trbFollowPerHour.TabIndex = 37;
            this.trbFollowPerHour.Value = 1;
            this.trbFollowPerHour.ValueChanged += new System.EventHandler(this.trbFollowPerHour_ValueChanged);
            // 
            // lblProxyInfo
            // 
            this.lblProxyInfo.Location = new System.Drawing.Point(20, 219);
            this.lblProxyInfo.Name = "lblProxyInfo";
            this.lblProxyInfo.Size = new System.Drawing.Size(185, 52);
            this.lblProxyInfo.TabIndex = 36;
            // 
            // chkProxy
            // 
            this.chkProxy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkProxy.Location = new System.Drawing.Point(20, 192);
            this.chkProxy.Name = "chkProxy";
            this.chkProxy.Padding = new System.Windows.Forms.Padding(3);
            this.chkProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkProxy.Size = new System.Drawing.Size(185, 24);
            this.chkProxy.TabIndex = 35;
            this.chkProxy.Text = "پراکسی";
            this.chkProxy.UseVisualStyleBackColor = false;
            this.chkProxy.CheckedChanged += new System.EventHandler(this.chkProxy_CheckedChanged);
            // 
            // btnSaveWhiteListToFile
            // 
            this.btnSaveWhiteListToFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnSaveWhiteListToFile.FlatAppearance.BorderSize = 0;
            this.btnSaveWhiteListToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveWhiteListToFile.Location = new System.Drawing.Point(464, 275);
            this.btnSaveWhiteListToFile.Name = "btnSaveWhiteListToFile";
            this.btnSaveWhiteListToFile.Size = new System.Drawing.Size(60, 36);
            this.btnSaveWhiteListToFile.TabIndex = 33;
            this.btnSaveWhiteListToFile.Text = "ذخیره در فایل";
            this.btnSaveWhiteListToFile.UseVisualStyleBackColor = false;
            this.btnSaveWhiteListToFile.Click += new System.EventHandler(this.btnSaveWhiteListToFile_Click);
            // 
            // btnCreateWhiteList444
            // 
            this.btnCreateWhiteList444.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnCreateWhiteList444.FlatAppearance.BorderSize = 0;
            this.btnCreateWhiteList444.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateWhiteList444.Location = new System.Drawing.Point(530, 160);
            this.btnCreateWhiteList444.Name = "btnCreateWhiteList444";
            this.btnCreateWhiteList444.Size = new System.Drawing.Size(188, 38);
            this.btnCreateWhiteList444.TabIndex = 11;
            this.btnCreateWhiteList444.Text = "ایجاد لیست سفید با لیست فالویینگ ها ";
            this.btnCreateWhiteList444.UseVisualStyleBackColor = false;
            this.btnCreateWhiteList444.Visible = false;
            this.btnCreateWhiteList444.Click += new System.EventHandler(this.btnCreateWhiteList_Click);
            // 
            // btnCreateWhiteList
            // 
            this.btnCreateWhiteList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnCreateWhiteList.FlatAppearance.BorderSize = 0;
            this.btnCreateWhiteList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateWhiteList.Location = new System.Drawing.Point(464, 198);
            this.btnCreateWhiteList.Name = "btnCreateWhiteList";
            this.btnCreateWhiteList.Size = new System.Drawing.Size(60, 73);
            this.btnCreateWhiteList.TabIndex = 32;
            this.btnCreateWhiteList.Text = "ایجاد لیست سفید با فالویینگ ها ";
            this.btnCreateWhiteList.UseVisualStyleBackColor = false;
            this.btnCreateWhiteList.Click += new System.EventHandler(this.btnCreateWhiteList_Click_1);
            // 
            // chkTargetLikers
            // 
            this.chkTargetLikers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkTargetLikers.Location = new System.Drawing.Point(20, 102);
            this.chkTargetLikers.Name = "chkTargetLikers";
            this.chkTargetLikers.Padding = new System.Windows.Forms.Padding(3);
            this.chkTargetLikers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkTargetLikers.Size = new System.Drawing.Size(185, 24);
            this.chkTargetLikers.TabIndex = 31;
            this.chkTargetLikers.Text = "فالو از لایک های پیج هدف";
            this.chkTargetLikers.UseVisualStyleBackColor = false;
            this.chkTargetLikers.CheckedChanged += new System.EventHandler(this.chkTargetLikers_CheckedChanged);
            // 
            // chkTargetFollowers
            // 
            this.chkTargetFollowers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkTargetFollowers.Location = new System.Drawing.Point(20, 72);
            this.chkTargetFollowers.Name = "chkTargetFollowers";
            this.chkTargetFollowers.Padding = new System.Windows.Forms.Padding(3);
            this.chkTargetFollowers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkTargetFollowers.Size = new System.Drawing.Size(185, 24);
            this.chkTargetFollowers.TabIndex = 29;
            this.chkTargetFollowers.Text = "فالو از فالوور های پیج هدف";
            this.chkTargetFollowers.UseVisualStyleBackColor = false;
            this.chkTargetFollowers.CheckedChanged += new System.EventHandler(this.chkTargetFollowers_CheckedChanged);
            // 
            // chkUnfollowNonefollowers
            // 
            this.chkUnfollowNonefollowers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkUnfollowNonefollowers.Enabled = false;
            this.chkUnfollowNonefollowers.Location = new System.Drawing.Point(20, 132);
            this.chkUnfollowNonefollowers.Name = "chkUnfollowNonefollowers";
            this.chkUnfollowNonefollowers.Padding = new System.Windows.Forms.Padding(3);
            this.chkUnfollowNonefollowers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkUnfollowNonefollowers.Size = new System.Drawing.Size(185, 24);
            this.chkUnfollowNonefollowers.TabIndex = 27;
            this.chkUnfollowNonefollowers.Text = "آنفالو غیر فالو بک ها";
            this.chkUnfollowNonefollowers.UseVisualStyleBackColor = false;
            this.chkUnfollowNonefollowers.CheckedChanged += new System.EventHandler(this.chkUnfollowNonefollowers_CheckedChanged);
            // 
            // chkFollowPrivateAccounts
            // 
            this.chkFollowPrivateAccounts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkFollowPrivateAccounts.Location = new System.Drawing.Point(20, 162);
            this.chkFollowPrivateAccounts.Name = "chkFollowPrivateAccounts";
            this.chkFollowPrivateAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.chkFollowPrivateAccounts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkFollowPrivateAccounts.Size = new System.Drawing.Size(185, 24);
            this.chkFollowPrivateAccounts.TabIndex = 25;
            this.chkFollowPrivateAccounts.Text = "فقط اکانت های پرایوت فالو شود";
            this.chkFollowPrivateAccounts.UseVisualStyleBackColor = false;
            this.chkFollowPrivateAccounts.CheckedChanged += new System.EventHandler(this.chkFollowPrivateAccounts_CheckedChanged);
            // 
            // btnLoadWhiteFile
            // 
            this.btnLoadWhiteFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnLoadWhiteFile.FlatAppearance.BorderSize = 0;
            this.btnLoadWhiteFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadWhiteFile.Location = new System.Drawing.Point(464, 79);
            this.btnLoadWhiteFile.Name = "btnLoadWhiteFile";
            this.btnLoadWhiteFile.Size = new System.Drawing.Size(60, 36);
            this.btnLoadWhiteFile.TabIndex = 23;
            this.btnLoadWhiteFile.Text = "لود از فایل";
            this.btnLoadWhiteFile.UseVisualStyleBackColor = false;
            this.btnLoadWhiteFile.Click += new System.EventHandler(this.btnLoadWhiteFile_Click);
            // 
            // btnAddToWhileList
            // 
            this.btnAddToWhileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnAddToWhileList.FlatAppearance.BorderSize = 0;
            this.btnAddToWhileList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToWhileList.Location = new System.Drawing.Point(270, 48);
            this.btnAddToWhileList.Name = "btnAddToWhileList";
            this.btnAddToWhileList.Size = new System.Drawing.Size(64, 23);
            this.btnAddToWhileList.TabIndex = 21;
            this.btnAddToWhileList.Text = "اضافه کردن";
            this.btnAddToWhileList.UseVisualStyleBackColor = false;
            this.btnAddToWhileList.Click += new System.EventHandler(this.btnAddToWhileList_Click);
            // 
            // btnClearWhiteList
            // 
            this.btnClearWhiteList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnClearWhiteList.FlatAppearance.BorderSize = 0;
            this.btnClearWhiteList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearWhiteList.Location = new System.Drawing.Point(464, 159);
            this.btnClearWhiteList.Name = "btnClearWhiteList";
            this.btnClearWhiteList.Size = new System.Drawing.Size(60, 36);
            this.btnClearWhiteList.TabIndex = 20;
            this.btnClearWhiteList.Text = "پاک کردن کل لیست";
            this.btnClearWhiteList.UseVisualStyleBackColor = false;
            this.btnClearWhiteList.Click += new System.EventHandler(this.btnClearWhiteList_Click);
            // 
            // btnRemoveWhiteItem
            // 
            this.btnRemoveWhiteItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnRemoveWhiteItem.FlatAppearance.BorderSize = 0;
            this.btnRemoveWhiteItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveWhiteItem.Location = new System.Drawing.Point(464, 119);
            this.btnRemoveWhiteItem.Name = "btnRemoveWhiteItem";
            this.btnRemoveWhiteItem.Size = new System.Drawing.Size(60, 36);
            this.btnRemoveWhiteItem.TabIndex = 19;
            this.btnRemoveWhiteItem.Text = "پاک کردن آیتم";
            this.btnRemoveWhiteItem.UseVisualStyleBackColor = false;
            this.btnRemoveWhiteItem.Click += new System.EventHandler(this.btnRemoveWhiteItem_Click);
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.BackColor = System.Drawing.Color.White;
            this.txtWhiteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWhiteList.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtWhiteList.Location = new System.Drawing.Point(338, 48);
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWhiteList.Size = new System.Drawing.Size(120, 23);
            this.txtWhiteList.TabIndex = 18;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel10.Controls.Add(this.lblWhitelistCount);
            this.panel10.Controls.Add(this.label9);
            this.panel10.Location = new System.Drawing.Point(270, 4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(188, 38);
            this.panel10.TabIndex = 16;
            // 
            // lblWhitelistCount
            // 
            this.lblWhitelistCount.Location = new System.Drawing.Point(3, 6);
            this.lblWhitelistCount.Name = "lblWhitelistCount";
            this.lblWhitelistCount.Size = new System.Drawing.Size(94, 25);
            this.lblWhitelistCount.TabIndex = 1;
            this.lblWhitelistCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(103, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 25);
            this.label9.TabIndex = 0;
            this.label9.Text = "لیست سفید";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbtWhiteList
            // 
            this.lbtWhiteList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.lbtWhiteList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbtWhiteList.ForeColor = System.Drawing.Color.White;
            this.lbtWhiteList.FormattingEnabled = true;
            this.lbtWhiteList.Items.AddRange(new object[] {
            "هیچ آیتمی در لیست وجود ندارد"});
            this.lbtWhiteList.Location = new System.Drawing.Point(270, 77);
            this.lbtWhiteList.Name = "lbtWhiteList";
            this.lbtWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbtWhiteList.Size = new System.Drawing.Size(188, 247);
            this.lbtWhiteList.TabIndex = 17;
            this.lbtWhiteList.SelectedIndexChanged += new System.EventHandler(this.lbtWhiteList_SelectedIndexChanged);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnLoadFile.FlatAppearance.BorderSize = 0;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.Location = new System.Drawing.Point(724, 79);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(60, 48);
            this.btnLoadFile.TabIndex = 15;
            this.btnLoadFile.Text = "لود از فایل";
            this.btnLoadFile.UseVisualStyleBackColor = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnAddToList
            // 
            this.btnAddToList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnAddToList.FlatAppearance.BorderSize = 0;
            this.btnAddToList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToList.Location = new System.Drawing.Point(530, 50);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(64, 23);
            this.btnAddToList.TabIndex = 13;
            this.btnAddToList.Text = "اضافه کردن";
            this.btnAddToList.UseVisualStyleBackColor = false;
            this.btnAddToList.Click += new System.EventHandler(this.btnAddToList_Click);
            // 
            // btnClearList
            // 
            this.btnClearList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnClearList.FlatAppearance.BorderSize = 0;
            this.btnClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearList.Location = new System.Drawing.Point(724, 186);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(60, 48);
            this.btnClearList.TabIndex = 12;
            this.btnClearList.Text = "پاک کردن کل لیست";
            this.btnClearList.UseVisualStyleBackColor = false;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.btnRemoveItem.FlatAppearance.BorderSize = 0;
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveItem.Location = new System.Drawing.Point(724, 132);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(60, 48);
            this.btnRemoveItem.TabIndex = 11;
            this.btnRemoveItem.Text = "پاک کردن آیتم";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel7.Controls.Add(this.label7);
            this.panel7.Location = new System.Drawing.Point(113, 383);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(92, 22);
            this.panel7.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = " کمینه فالوِرها";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel6.Controls.Add(this.label6);
            this.panel6.Location = new System.Drawing.Point(113, 346);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(92, 22);
            this.panel6.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(0, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = " بیشینه فالوِرها";
            // 
            // txtMinFollower
            // 
            this.txtMinFollower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMinFollower.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtMinFollower.Location = new System.Drawing.Point(20, 383);
            this.txtMinFollower.Name = "txtMinFollower";
            this.txtMinFollower.Size = new System.Drawing.Size(75, 22);
            this.txtMinFollower.TabIndex = 6;
            this.txtMinFollower.Text = "30";
            this.txtMinFollower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinFollower.TextChanged += new System.EventHandler(this.txtMinFollower_TextChanged);
            this.txtMinFollower.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinFollower_KeyPress);
            // 
            // txtMaxFollower
            // 
            this.txtMaxFollower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaxFollower.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtMaxFollower.Location = new System.Drawing.Point(20, 346);
            this.txtMaxFollower.Name = "txtMaxFollower";
            this.txtMaxFollower.Size = new System.Drawing.Size(75, 22);
            this.txtMaxFollower.TabIndex = 5;
            this.txtMaxFollower.Text = "30";
            this.txtMaxFollower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaxFollower.TextChanged += new System.EventHandler(this.txtMaxFollower_TextChanged);
            this.txtMaxFollower.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxFollower_KeyPress);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel5.Controls.Add(this.label5);
            this.panel5.Location = new System.Drawing.Point(20, 302);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(185, 38);
            this.panel5.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(33, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "کمینه و بیشینه فالوِرها";
            // 
            // txtTargetPage
            // 
            this.txtTargetPage.BackColor = System.Drawing.Color.White;
            this.txtTargetPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetPage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtTargetPage.Location = new System.Drawing.Point(600, 50);
            this.txtTargetPage.Name = "txtTargetPage";
            this.txtTargetPage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTargetPage.Size = new System.Drawing.Size(120, 23);
            this.txtTargetPage.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Location = new System.Drawing.Point(530, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(188, 38);
            this.panel3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(42, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "لیست پیج های هدف";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.panel9.Location = new System.Drawing.Point(95, 27);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(188, 38);
            this.panel9.TabIndex = 10;
            this.panel9.Visible = false;
            // 
            // lbtTargetPage
            // 
            this.lbtTargetPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.lbtTargetPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbtTargetPage.ForeColor = System.Drawing.Color.White;
            this.lbtTargetPage.FormattingEnabled = true;
            this.lbtTargetPage.Items.AddRange(new object[] {
            "هیچ آیتمی در لیست وجود ندارد"});
            this.lbtTargetPage.Location = new System.Drawing.Point(530, 77);
            this.lbtTargetPage.Name = "lbtTargetPage";
            this.lbtTargetPage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbtTargetPage.Size = new System.Drawing.Size(188, 247);
            this.lbtTargetPage.TabIndex = 3;
            // 
            // chkPrivateAccount
            // 
            this.chkPrivateAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkPrivateAccount.Location = new System.Drawing.Point(20, 42);
            this.chkPrivateAccount.Name = "chkPrivateAccount";
            this.chkPrivateAccount.Padding = new System.Windows.Forms.Padding(3);
            this.chkPrivateAccount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkPrivateAccount.Size = new System.Drawing.Size(185, 24);
            this.chkPrivateAccount.TabIndex = 2;
            this.chkPrivateAccount.Text = "اکانت های پرایوت فالو نشود";
            this.chkPrivateAccount.UseVisualStyleBackColor = false;
            this.chkPrivateAccount.CheckedChanged += new System.EventHandler(this.chkPrivateAccount_CheckedChanged);
            // 
            // chkProfilePic
            // 
            this.chkProfilePic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(106)))), ((int)(((byte)(180)))));
            this.chkProfilePic.Location = new System.Drawing.Point(20, 12);
            this.chkProfilePic.Name = "chkProfilePic";
            this.chkProfilePic.Padding = new System.Windows.Forms.Padding(3);
            this.chkProfilePic.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkProfilePic.Size = new System.Drawing.Size(185, 24);
            this.chkProfilePic.TabIndex = 1;
            this.chkProfilePic.Text = "بدون عکس پروفایل فالو نشود";
            this.chkProfilePic.UseVisualStyleBackColor = false;
            this.chkProfilePic.CheckedChanged += new System.EventHandler(this.chkProfilePic_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtfComment);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(792, 424);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "یادداشت ها";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtfComment
            // 
            this.rtfComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfComment.Location = new System.Drawing.Point(0, 0);
            this.rtfComment.Name = "rtfComment";
            this.rtfComment.Size = new System.Drawing.Size(792, 424);
            this.rtfComment.TabIndex = 0;
            this.rtfComment.Text = "";
            // 
            // frmPanelSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(184)))), ((int)(((byte)(225)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "frmPanelSetting";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تنظیمات";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPanelSetting_FormClosing);
            this.Load += new System.EventHandler(this.frmPanelSetting_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbUnfollowPerHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbFollowPerHour)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox lbtTargetPage;
        private System.Windows.Forms.CheckBox chkPrivateAccount;
        private System.Windows.Forms.CheckBox chkProfilePic;
        private System.Windows.Forms.TextBox txtTargetPage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinFollower;
        private System.Windows.Forms.TextBox txtMaxFollower;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFollowPerHour;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnCreateWhiteList444;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnLoadWhiteFile;
        private System.Windows.Forms.Button btnAddToWhileList;
        private System.Windows.Forms.Button btnClearWhiteList;
        private System.Windows.Forms.Button btnRemoveWhiteItem;
        private System.Windows.Forms.TextBox txtWhiteList;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lbtWhiteList;
        private System.Windows.Forms.RichTextBox rtfComment;
        private System.Windows.Forms.CheckBox chkUnfollowNonefollowers;
        private System.Windows.Forms.CheckBox chkFollowPrivateAccounts;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label lblWhitelistCount;
        private System.Windows.Forms.Button btnCreateWhiteList;
        private System.Windows.Forms.CheckBox chkTargetLikers;
        private System.Windows.Forms.CheckBox chkTargetFollowers;
        private System.Windows.Forms.Button btnSaveWhiteListToFile;
        private System.Windows.Forms.CheckBox chkProxy;
        private System.Windows.Forms.Label lblProxyInfo;
        private System.Windows.Forms.TrackBar trbFollowPerHour;
        private System.Windows.Forms.TrackBar trbUnfollowPerHour;
        private System.Windows.Forms.Label lblUnfollowPerHour;
        private System.Windows.Forms.Button btnSaveTargetToFile;
    }
}