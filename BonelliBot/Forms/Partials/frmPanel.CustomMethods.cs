using a.Utilities;
using AutoMapper;
using BonelliBot.Models;
using BonelliBot.Properties;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using QLicense;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BonelliBot.Forms
{
    partial class frmPanel
    {

        #region FormLoadsEvents

        void InitSettings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Settings0, Settings>();
                cfg.CreateMap<Settings, Settings0>();

                cfg.CreateMap<Settings1, Settings>();
                cfg.CreateMap<Settings, Settings1>();

                cfg.CreateMap<Settings2, Settings>();
                cfg.CreateMap<Settings, Settings2>();

                cfg.CreateMap<Settings3, Settings>();
                cfg.CreateMap<Settings, Settings3>();

                cfg.CreateMap<Settings4, Settings>();
                cfg.CreateMap<Settings, Settings4>();
            });
        }

        void CheckStateFile()
        {
            int count = Directory.GetFiles(path).Where(g => g.Contains("state")).Count();
            for (var i = 0; i <= count; i++)
            {
                //CheckLogin(i);
            }

        }

        void Init()
        {

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer
            {
                Interval = 30 * 60000 // هر 30 دقیقه
            };
            t.Tick += new System.EventHandler(OnTimerEvent);
            t.Start();

            if (_status == LicenseStatus.VALID)
            {
                //یوزر اول
                if (Settings0.Default.followStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnFollow.BackColor = AppSettings.Default.Green;
                }
                if (Settings0.Default.unFollowStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnUnfollow.BackColor = AppSettings.Default.Green;
                }
                //یوزر دوم
                if (Settings1.Default.followStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnFollow1.BackColor = AppSettings.Default.Green;
                }
                if (Settings1.Default.unFollowStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnUnfollow1.BackColor = AppSettings.Default.Green;
                }
                //یوزر سوم
                if (Settings2.Default.followStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnFollow2.BackColor = AppSettings.Default.Green;
                }
                if (Settings2.Default.unFollowStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnUnfollow2.BackColor = AppSettings.Default.Green;
                }
                //یوزر چهارم
                if (Settings3.Default.followStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnFollow3.BackColor = AppSettings.Default.Green;
                }
                if (Settings3.Default.unFollowStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnUnfollow3.BackColor = AppSettings.Default.Green;
                }
                //یوزر پنجم
                if (Settings4.Default.followStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnFollow4.BackColor = AppSettings.Default.Green;
                }
                if (Settings4.Default.unFollowStart /*&& _status == LicenseStatus.VALID*/)
                {
                    btnUnfollow4.BackColor = AppSettings.Default.Green;
                }
            }

        }

        private HttpClientHandler SetProxy(Settings settings)
        {

            var proxy = new WebProxy()
            {
                Address = new Uri($"{Properties.Settings.Default.proxyProtocol}{Properties.Settings.Default.ipAddress}:{Properties.Settings.Default.port}"), //i.e:
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                // *** These creds are given to the proxy server, not the web server ***
                Credentials = new NetworkCredential(
                userName: settings.proxyUser,
                password: settings.proxyPassword)
            };

            // Now create a client handler which uses that proxy
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = true
            };
            return httpClientHandler;
        }
        
        public void CheckResponse(InstaLoginResult result, int userNumber = 0)
        {
            if (result == InstaLoginResult.Success)
            {
                //frmPanel frmPanel = new frmPanel(_instaApi);
                //frmPanel.Show();
                //this.Hide();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (result == InstaLoginResult.ChallengeRequired)
            {
                frmChallenge frmChallenge = new frmChallenge(_instaApi, userNumber);
                frmChallenge.Show();
                //this.Hide();
            }
            else if (result == InstaLoginResult.BadPassword)
            {
                this.Text = "نام کاربری یا رمز عبور شما اشتباه است";
                btnLogin.Enabled = true;
            }
        }

        #endregion

        #region License
        private bool CheckLisence()
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

            frmLicenseInfo.CertificatePublicKeyData = _certPubicKeyData;
            switch (_status)
            {
                case LicenseStatus.VALID:

                    //TODO: If license is valid, you can do extra checking here
                    //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
                    //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                    // چک کردن تاریخ لایسنس
                    if (!CheckLicenseDate())
                    {
                        ExpiredLicense();
                        return false;
                        //break;
                    }
                    CheckLicenseFeatures();
                    //licInfo.ShowLicenseInfo(_lic);
                    return true;
                //break;

                default:
                    //for the other status of license file, show the warning message
                    //and also popup the activation form for user to activate your application
                    //MessageBox.Show(_msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    InvalidLicense();
                    return false;
            }
        }

        void InvalidLicense()
        {
            lblLicenceStatus.Text = "اکانت شما فعال نشده است";
            RedButtons();
            ResetSettings();
        }

        void CheckLicenseFeatures()
        {
            if (_lic.FirstAccount)
            {
                btnLogin.Enabled = true;
                var file = Directory.GetFiles(path).FirstOrDefault(g => g.Contains("state"));
                if (file != null)
                {
                    InitBonelliCore(0);
                }
            }
            if (_lic.SecondAccount)
            {
                btnLogin1.Enabled = true;
                var file = Directory.GetFiles(path).FirstOrDefault(g => g.Contains("state1"));
                if (file != null)
                {
                    InitBonelliCore(1);
                }
            }
            if (_lic.ThirdAccount)
            {
                btnLogin2.Enabled = true;
                var file = Directory.GetFiles(path).FirstOrDefault(g => g.Contains("state2"));
                if (file != null)
                {
                    InitBonelliCore(2);
                }
            }
            if (_lic.ForthAccount)
            {
                btnLogin3.Enabled = true;
                var file = Directory.GetFiles(path).FirstOrDefault(g => g.Contains("state3"));
                if (file != null)
                {
                    InitBonelliCore(3);
                }
            }
            if (_lic.FifthAccount)
            {
                btnLogin4.Enabled = true;
                var file = Directory.GetFiles(path).FirstOrDefault(g => g.Contains("state4"));
                if (file != null)
                {
                    InitBonelliCore(4);
                }
            }
        }

        void ExpiredLicense()
        {
            lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = "انقضای اکانت شما به پایان رسیده است"));
            _status = LicenseStatus.INVALID;

            RedButtons();
            StopBonelliCoreRobot();
            ResetSettings();

            void StopBonelliCoreRobot()
            {
                if (bonelliCore != null)
                    bonelliCore.runRobot = false;
                if (bonelliCore1 != null)
                    bonelliCore1.runRobot = false;
                if (bonelliCore2 != null)
                    bonelliCore2.runRobot = false;
                if (bonelliCore3 != null)
                    bonelliCore3.runRobot = false;
                if (bonelliCore4 != null)
                    bonelliCore4.runRobot = false;
            }
        }

        private bool CheckLicenseDate()
        {
            // چک میکنیم تاریخ نال نباشد
            if (_lic.ExpiriedDate != DateTime.MinValue)
            {
                if (_lic.ExpiriedDate < GetDateFromInternet())
                {
                    return false;
                }
                lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = $"این برنامه تا تاریخ {_lic.ExpiriedDate.ToShamsi()} فعال است"));
            }
            else
            {
                lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = $"این برنامه بدون محدودیت زمانی فعال است"));
            }

            lblLicenceStatus.BackColor = AppSettings.Default.PrimaryColor;
            return true;
        }
        #endregion

        #region BonelliCore

        private void InitBonelliCore(int userNumber)
        {
            switch (userNumber)
            {
                case 0:
                    bonelliCore = new BonelliCore();//BonelliCore(instaApi);
                    bonelliCore.btnFollow = btnFollow;
                    bonelliCore.btnSettings = btnSettings;
                    bonelliCore.btnUnfollow = btnUnfollow;
                    bonelliCore.lblCurrentUser = lblCurrentUser;
                    bonelliCore.lblFolloweBack = lblFolloweBack;
                    bonelliCore.lblFollowed = lblFollowed;
                    bonelliCore.lblFollowers = lblFollowers;
                    bonelliCore.lblFollowings = lblFollowings;
                    bonelliCore.lblLicenceStatus = lblLicenceStatus;
                    bonelliCore.lblSkipped = lblSkipped;
                    bonelliCore.lblUnFollowed = lblUnFollowed;
                    bonelliCore.lblRequested = lblRequested;
                    bonelliCore.lblPercentage = lblPercentage;
                    bonelliCore.lblMsg = lblMsg;
                    bonelliCore.settings = Mapper.Map<Settings>(Settings0.Default);
                    bonelliCore.btnLogin = btnLogin;
                    if (bonelliCore.CheckLogin(0).Result == true)
                    {
                        bonelliCore.Start();
                        btnLogin.Text = "خروج از اکانت";
                        btnSettings.Enabled = true;
                    }
                    break;
                case 1:
                    bonelliCore1 = new BonelliCore();//(instaApi);
                    bonelliCore1.btnFollow = btnFollow1;
                    bonelliCore1.btnSettings = btnSettings1;
                    bonelliCore1.btnUnfollow = btnUnfollow1;
                    bonelliCore1.lblCurrentUser = lblCurrentUser1;
                    bonelliCore1.lblFolloweBack = lblFolloweBack1;
                    bonelliCore1.lblFollowed = lblFollowed1;
                    bonelliCore1.lblFollowers = lblFollowers1;
                    bonelliCore1.lblFollowings = lblFollowings1;
                    bonelliCore1.lblLicenceStatus = lblLicenceStatus;
                    bonelliCore1.lblSkipped = lblSkipped1;
                    bonelliCore1.lblUnFollowed = lblUnFollowed1;
                    bonelliCore1.lblRequested = lblRequested1;
                    bonelliCore1.lblPercentage = lblPercentage1;
                    bonelliCore1.lblMsg = lblMsg1;
                    bonelliCore1.settings = Mapper.Map<Settings>(Settings1.Default);
                    bonelliCore1.btnLogin = btnLogin1;
                    if (bonelliCore1.CheckLogin(1).Result == true)
                    {
                        bonelliCore1.Start();
                        btnLogin1.Text = "خروج از اکانت";
                        btnSettings1.Enabled = true;
                    }
                    break;
                case 2:
                    bonelliCore2 = new BonelliCore();// (instaApi);
                    bonelliCore2.btnFollow = btnFollow2;
                    bonelliCore2.btnSettings = btnSettings2;
                    bonelliCore2.btnUnfollow = btnUnfollow2;
                    bonelliCore2.lblCurrentUser = lblCurrentUser2;
                    bonelliCore2.lblFolloweBack = lblFolloweBack2;
                    bonelliCore2.lblFollowed = lblFollowed2;
                    bonelliCore2.lblFollowers = lblFollowers2;
                    bonelliCore2.lblFollowings = lblFollowings2;
                    bonelliCore2.lblLicenceStatus = lblLicenceStatus;
                    bonelliCore2.lblSkipped = lblSkipped2;
                    bonelliCore2.lblUnFollowed = lblUnFollowed2;
                    bonelliCore2.lblRequested = lblRequested2;
                    bonelliCore2.lblPercentage = lblPercentage2;
                    bonelliCore2.lblMsg = lblMsg2;
                    bonelliCore2.settings = Mapper.Map<Settings>(Settings2.Default);
                    bonelliCore2.btnLogin = btnLogin2;
                    if (bonelliCore2.CheckLogin(2).Result == true)
                    {
                        bonelliCore2.Start();
                        btnLogin2.Text = "خروج از اکانت";
                        btnSettings2.Enabled = true;
                    }
                    break;
                case 3:
                    bonelliCore3 = new BonelliCore();// (instaApi);
                    bonelliCore3.btnFollow = btnFollow3;
                    bonelliCore3.btnSettings = btnSettings3;
                    bonelliCore3.btnUnfollow = btnUnfollow3;
                    bonelliCore3.lblCurrentUser = lblCurrentUser3;
                    bonelliCore3.lblFolloweBack = lblFolloweBack3;
                    bonelliCore3.lblFollowed = lblFollowed3;
                    bonelliCore3.lblFollowers = lblFollowers3;
                    bonelliCore3.lblFollowings = lblFollowings3;
                    bonelliCore3.lblLicenceStatus = lblLicenceStatus;
                    bonelliCore3.lblSkipped = lblSkipped3;
                    bonelliCore3.lblUnFollowed = lblUnFollowed3;
                    bonelliCore3.lblRequested = lblRequested3;
                    bonelliCore3.lblPercentage = lblPercentage3;
                    bonelliCore3.lblMsg = lblMsg3;
                    bonelliCore3.settings = Mapper.Map<Settings>(Settings3.Default);
                    bonelliCore3.btnLogin = btnLogin3;
                    if (bonelliCore3.CheckLogin(3).Result == true)
                    {
                        bonelliCore3.Start();
                        btnLogin3.Text = "خروج از اکانت";
                        btnSettings3.Enabled = true;
                    }
                    break;
                case 4:
                    bonelliCore4 = new BonelliCore();// (instaApi);
                    bonelliCore4.btnFollow = btnFollow4;
                    bonelliCore4.btnSettings = btnSettings4;
                    bonelliCore4.btnUnfollow = btnUnfollow4;
                    bonelliCore4.lblCurrentUser = lblCurrentUser4;
                    bonelliCore4.lblFolloweBack = lblFolloweBack4;
                    bonelliCore4.lblFollowed = lblFollowed4;
                    bonelliCore4.lblFollowers = lblFollowers4;
                    bonelliCore4.lblFollowings = lblFollowings4;
                    bonelliCore4.lblLicenceStatus = lblLicenceStatus;
                    bonelliCore4.lblSkipped = lblSkipped4;
                    bonelliCore4.lblUnFollowed = lblUnFollowed4;
                    bonelliCore4.lblRequested = lblRequested4;
                    bonelliCore4.lblPercentage = lblPercentage4;
                    bonelliCore4.lblMsg = lblMsg4;
                    bonelliCore4.settings = Mapper.Map<Settings>(Settings4.Default);
                    bonelliCore4.btnLogin = btnLogin4;
                    if (bonelliCore4.CheckLogin(4).Result == true)
                    {
                        bonelliCore4.Start();
                        btnLogin4.Text = "خروج از اکانت";
                        btnSettings4.Enabled = true;
                    }
                    break;
            }
        }
        private void ClearAccountData(int userNumber)
        {
            switch (userNumber)
            {
                case 0:
                    bonelliCore.runRobot = false;
                    bonelliCore?.doJobTask.Wait();
                    bonelliCore?.doJobTask.Dispose();
                    bonelliCore = null;

                    btnSettings.Enabled = false;
                    lblFolloweBack.Text = "0";
                    lblFollowed.Text = "0";
                    lblFollowers.Text = "0";
                    lblFollowings.Text = "0";
                    lblPercentage.Text = "0";
                    lblRequested.Text = "0";
                    lblSkipped.Text = "0";
                    lblUnFollowed.Text = "0";
                    lblMsg.Text = "";
                    btnFollow.BackColor = AppSettings.Default.Red;
                    btnUnfollow.BackColor = AppSettings.Default.Red;
                    btnSettings.BackColor = AppSettings.Default.PrimaryColor;

                    settings0.maxFollow = 10000;
                    settings0.minFollow = 5;
                    settings0.useProxy = false;
                    settings0.Save();

                    break;
                case 1:
                    bonelliCore1.runRobot = false;
                    bonelliCore1?.doJobTask.Wait();
                    bonelliCore1?.doJobTask.Dispose();
                    bonelliCore1 = null;

                    btnSettings1.Enabled = false;
                    lblFolloweBack1.Text = "0";
                    lblFollowed1.Text = "0";
                    lblFollowers1.Text = "0";
                    lblFollowings1.Text = "0";
                    lblPercentage1.Text = "0";
                    lblRequested1.Text = "0";
                    lblSkipped1.Text = "0";
                    lblUnFollowed1.Text = "0";
                    lblMsg1.Text = "";
                    btnFollow1.BackColor = AppSettings.Default.Red;
                    btnUnfollow1.BackColor = AppSettings.Default.Red;
                    btnSettings1.BackColor = AppSettings.Default.PrimaryColor;

                    settings1.maxFollow = 10000;
                    settings1.minFollow = 5;
                    settings1.useProxy = false;
                    settings1.Save();

                    break;
                case 2:
                    bonelliCore2.runRobot = false;
                    bonelliCore2?.doJobTask.Wait();
                    bonelliCore2?.doJobTask.Dispose();
                    bonelliCore2 = null;

                    btnSettings2.Enabled = false;
                    lblFolloweBack2.Text = "0";
                    lblFollowed2.Text = "0";
                    lblFollowers2.Text = "0";
                    lblFollowings2.Text = "0";
                    lblPercentage2.Text = "0";
                    lblRequested2.Text = "0";
                    lblSkipped2.Text = "0";
                    lblUnFollowed2.Text = "0";
                    lblMsg2.Text = "";
                    btnFollow2.BackColor = AppSettings.Default.Red;
                    btnUnfollow2.BackColor = AppSettings.Default.Red;
                    btnSettings2.BackColor = AppSettings.Default.PrimaryColor;

                    settings2.maxFollow = 10000;
                    settings2.minFollow = 5;
                    settings2.useProxy = false;
                    settings2.Save();

                    break;
                case 3:
                    bonelliCore3.runRobot = false;
                    bonelliCore3?.doJobTask.Wait();
                    bonelliCore3?.doJobTask.Dispose();
                    bonelliCore3 = null;

                    btnSettings3.Enabled = false;
                    lblFolloweBack3.Text = "0";
                    lblFollowed3.Text = "0";
                    lblFollowers3.Text = "0";
                    lblFollowings3.Text = "0";
                    lblPercentage3.Text = "0";
                    lblRequested3.Text = "0";
                    lblSkipped3.Text = "0";
                    lblUnFollowed3.Text = "0";
                    lblMsg3.Text = "";
                    btnFollow3.BackColor = AppSettings.Default.Red;
                    btnUnfollow3.BackColor = AppSettings.Default.Red;
                    btnSettings3.BackColor = AppSettings.Default.PrimaryColor;

                    settings3.maxFollow = 10000;
                    settings3.minFollow = 5;
                    settings3.useProxy = false;
                    settings3.Save();

                    break;
                case 4:
                    bonelliCore4.runRobot = false;
                    bonelliCore4?.doJobTask.Wait();
                    bonelliCore4?.doJobTask.Dispose();
                    bonelliCore4 = null;

                    btnSettings4.Enabled = false;
                    lblFolloweBack4.Text = "0";
                    lblFollowed4.Text = "0";
                    lblFollowers4.Text = "0";
                    lblFollowings4.Text = "0";
                    lblPercentage4.Text = "0";
                    lblRequested4.Text = "0";
                    lblSkipped4.Text = "0";
                    lblUnFollowed4.Text = "0";
                    lblMsg4.Text = "";
                    btnFollow4.BackColor = AppSettings.Default.Red;
                    btnUnfollow4.BackColor = AppSettings.Default.Red;
                    btnSettings4.BackColor = AppSettings.Default.PrimaryColor;

                    settings4.maxFollow = 10000;
                    settings4.minFollow = 5;
                    settings4.useProxy = false;
                    settings4.Save();

                    break;
            }
        }
        #endregion

        #region utility

        internal static void frmPanel_UIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                //result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
                Exception ex = e.Exception;
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log_unhandled_exception.txt", true))
                {
                    writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now +
                                     ") ----------------------------------------------\r\n");
                    writer.WriteLine(ex.ToString() + "\r\n" + "Message: " + "Global Error Handler");
                    writer.WriteLine("\r\n------------------------------------------ (" + "StackTrace" +
                                     ") ----------------------------------------------\r\n");
                    StackTrace st = new StackTrace(true);
                    string stackIndent = "";
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        // Note that at this level, there are four
                        // stack frames, one for each method invocation.
                        StackFrame sf = st.GetFrame(i);
                        writer.WriteLine("\n\r");
                        writer.WriteLine(stackIndent + " Method: " + sf.GetMethod());
                        writer.WriteLine(stackIndent + " File: " + sf.GetFileName());
                        writer.WriteLine(stackIndent + " Line Number: " + sf.GetFileLineNumber());
                        stackIndent += "  ";
                    }
                }
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();
        }
        public DateTime GetDateFromInternet()
        {
            try
            {
                using (var response =
                        WebRequest.Create("http://www.google.com").GetResponse())
                    //string todaysDates =  response.Headers["date"];
                    return DateTime.ParseExact(response.Headers["date"],
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal);
            }
            catch (WebException)
            {
                return DateTime.Now; //In case something goes wrong. 
            }
        }

        void RedButtons()
        {
            lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.BackColor = AppSettings.Default.Red));

            btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = AppSettings.Default.Red));
            btnFollow1.BeginInvoke((Action)(() => btnFollow1.BackColor = AppSettings.Default.Red));
            btnFollow2.BeginInvoke((Action)(() => btnFollow2.BackColor = AppSettings.Default.Red));
            btnFollow3.BeginInvoke((Action)(() => btnFollow3.BackColor = AppSettings.Default.Red));
            btnFollow4.BeginInvoke((Action)(() => btnFollow4.BackColor = AppSettings.Default.Red));
            btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = AppSettings.Default.Red));
            btnUnfollow1.BeginInvoke((Action)(() => btnUnfollow1.BackColor = AppSettings.Default.Red));
            btnUnfollow2.BeginInvoke((Action)(() => btnUnfollow2.BackColor = AppSettings.Default.Red));
            btnUnfollow3.BeginInvoke((Action)(() => btnUnfollow3.BackColor = AppSettings.Default.Red));
            btnUnfollow4.BeginInvoke((Action)(() => btnUnfollow4.BackColor = AppSettings.Default.Red));
        }
        void ResetSettings()
        {
            Settings0.Default.followStart = false;
            Settings0.Default.unFollowStart = false;
            Settings0.Default.Save();
            Settings1.Default.followStart = false;
            Settings1.Default.unFollowStart = false;
            Settings1.Default.Save();
            Settings2.Default.followStart = false;
            Settings2.Default.unFollowStart = false;
            Settings2.Default.Save();
            Settings3.Default.followStart = false;
            Settings3.Default.unFollowStart = false;
            Settings3.Default.Save();
            Settings4.Default.followStart = false;
            Settings4.Default.unFollowStart = false;
            Settings4.Default.Save();
        }
        // هر 30 دقیقه چک می شود
        public void OnTimerEvent(object source, EventArgs e)
        {
            if (!CheckLicenseDate())
            {
                ExpiredLicense();
            }
        }

        #endregion Utility

    }
}
