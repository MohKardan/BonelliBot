namespace BonelliBot.Forms
{
    partial class frmLicenseInfo
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
            this.licActCtrl = new QLicense.Windows.Controls.LicenseActivateControl();
            this.licInfo = new QLicense.Windows.Controls.LicenseInfoControl();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // licActCtrl
            // 
            this.licActCtrl.AppName = null;
            this.licActCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.licActCtrl.LicenseObjectType = null;
            this.licActCtrl.Location = new System.Drawing.Point(0, 0);
            this.licActCtrl.Name = "licActCtrl";
            this.licActCtrl.ShowMessageAfterValidation = true;
            this.licActCtrl.Size = new System.Drawing.Size(623, 264);
            this.licActCtrl.TabIndex = 0;
            // 
            // licInfo
            // 
            this.licInfo.DateFormat = null;
            this.licInfo.DateTimeFormat = null;
            this.licInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.licInfo.Location = new System.Drawing.Point(0, 299);
            this.licInfo.Name = "licInfo";
            this.licInfo.Size = new System.Drawing.Size(623, 144);
            this.licInfo.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(536, 270);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&تایید";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 443);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.licInfo);
            this.Controls.Add(this.licActCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmLicenseInfo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "لایسنس";
            this.Load += new System.EventHandler(this.frmLicenseInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private QLicense.Windows.Controls.LicenseActivateControl licActCtrl;
        private QLicense.Windows.Controls.LicenseInfoControl licInfo;
        private System.Windows.Forms.Button btnOk;
    }
}