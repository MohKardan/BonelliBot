using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Models;
using QLicense;
using RtlMessageBox = System.Windows.Forms.RtlMessageBox;

namespace BonelliBot.Forms
{
    public partial class frmLicenseInfo : Form
    {
        public static byte[] CertificatePublicKeyData { private get; set; }

        byte[] _certPubicKeyData;
        BotLicense _lic = null;
        string _msg = string.Empty;
        LicenseStatus _status = LicenseStatus.UNDEFINED;

        public frmLicenseInfo()
        {
            InitializeComponent();
        }

        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("BonelliBot.LicenseVerify.cer").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (BotLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(BotLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            if (_status == LicenseStatus.VALID)
            {
                licInfo.ShowLicenseInfo(_lic);
            }
           

            //Assign the application information values to the license control
            licActCtrl.AppName = "BonelliBot";
            licActCtrl.LicenseObjectType = typeof(BotLicense);
            licActCtrl.CertificatePublicKeyData = CertificatePublicKeyData;
            //Display the device unique ID
            licActCtrl.ShowUID();

            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // LicenseActivateControl Class in ActivationControls4Win
            //پیغام ها از طریق تابع داخل ایف پایین نمایش داده میشوند
                if (licActCtrl.ValidateLicense())
                {
                    //If license if valid, save the license string into a local file
                    File.WriteAllText(Path.Combine(Application.StartupPath, "license.lic"), licActCtrl.LicenseBASE64String);
                    this.Close();
                }
            }
        }
    }

