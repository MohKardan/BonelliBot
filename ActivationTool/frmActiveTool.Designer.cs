namespace ActivationTool
{
    partial class frmActiveTool
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
            this.licSettings = new QLicense.Windows.Controls.LicenseSettingsControl();
            this.licString = new QLicense.Windows.Controls.LicenseStringContainer();
            this.SuspendLayout();
            // 
            // licSettings
            // 
            this.licSettings.AllowVolumeLicense = true;
            this.licSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.licSettings.Location = new System.Drawing.Point(0, 0);
            this.licSettings.Name = "licSettings";
            this.licSettings.Size = new System.Drawing.Size(420, 465);
            this.licSettings.TabIndex = 0;
            this.licSettings.OnLicenseGenerated += new QLicense.Windows.Controls.LicenseGeneratedHandler(this.licSettings_OnLicenseGenerated);
            // 
            // licString
            // 
            this.licString.Dock = System.Windows.Forms.DockStyle.Right;
            this.licString.LicenseString = "";
            this.licString.Location = new System.Drawing.Point(426, 0);
            this.licString.Name = "licString";
            this.licString.Size = new System.Drawing.Size(367, 465);
            this.licString.TabIndex = 1;
            // 
            // frmActiveTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 465);
            this.Controls.Add(this.licString);
            this.Controls.Add(this.licSettings);
            this.Name = "frmActiveTool";
            this.Text = "فرم ساخت کد";
            this.Load += new System.EventHandler(this.frmActiveTool_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private QLicense.Windows.Controls.LicenseSettingsControl licSettings;
        private QLicense.Windows.Controls.LicenseStringContainer licString;
    }
}

