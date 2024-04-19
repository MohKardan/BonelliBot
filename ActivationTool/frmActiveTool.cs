using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Models;
using QLicense;

namespace ActivationTool
{
    public partial class frmActiveTool : Form
    {
        private byte[] _certPubicKeyData;
        private SecureString _certPwd = new SecureString();

        public frmActiveTool()
        {
            InitializeComponent();

            _certPwd.AppendChar('b');
            _certPwd.AppendChar('o');
            _certPwd.AppendChar('t');
        }

        private void frmActiveTool_Load(object sender, EventArgs e)
        {
            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("ActivationTool.LicenseSign.pfx").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Initialize the path for the certificate to sign the XML license file
            licSettings.CertificatePrivateKeyData = _certPubicKeyData;
            licSettings.CertificatePassword = _certPwd;

            //Initialize a new license object
            licSettings.License = new BotLicense();
        }

        private void licSettings_OnLicenseGenerated(object sender, QLicense.Windows.Controls.LicenseGeneratedEventArgs e)
        {
            licString.LicenseString = e.LicenseBASE64String;
        }

        private void btnGenSvrMgmLic_Click(object sender, EventArgs e)
        {
            //Event raised when "Generate License" button is clicked. 
            //Call the core library to generate the license
            licString.LicenseString = LicenseHandler.GenerateLicenseBASE64String(
                new BotLicense(), 
                _certPubicKeyData,
                _certPwd);
        }
    }
}
