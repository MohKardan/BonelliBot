using AutoMapper;
using InstagramApiSharp;
using InstagramApiSharp.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Properties;
using InstagramApiSharp.Classes;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using BonelliBot.Models;
using BonelliBot.Utilities;
using QLicense;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using InstagramApiSharp.Classes.Android.DeviceInfo;

namespace BonelliBot.Forms
{
    public partial class frmPanel : Form
    {
        #region Variables

        BonelliCore bonelliCore, bonelliCore1, bonelliCore2, bonelliCore3, bonelliCore4;
        private static IInstaApi _instaApi;
        private IResult<InstaCurrentUser> _currentUser;
        private const int completed = 1;
        string path;


        // متغیر های مربوز به لایسنس
        byte[] _certPubicKeyData;
        BotLicense _lic = null;
        string _msg = string.Empty;
        LicenseStatus _status = LicenseStatus.UNDEFINED;

        Settings settings0 = new Settings();
        Settings settings1 = new Settings();
        Settings settings2 = new Settings();
        Settings settings3 = new Settings();
        Settings settings4 = new Settings();

        #endregion

        #region formEvents
        public frmPanel()
        {
            InitializeComponent();
            path = Path.GetDirectoryName(Application.ExecutablePath);
        }

        public frmPanel(IInstaApi instaApi)
        {
            InitializeComponent();
            _instaApi = instaApi;
        }

        private void frmPanel_Load(object sender, EventArgs e)
        {
            try
            {
                if (Process.GetProcessesByName("sqlcese21").Count() == 0)
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = Path.Combine("x86\\sqlcese21.exe", "");

                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.CreateNoWindow = true;

                    proc.StartInfo.RedirectStandardOutput = true;

                    proc.StartInfo.UseShellExecute = false;

                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                Program.Log(ex, "Fuck Fuck Fuck");
            }
            lblDate.Text = $"{DateTime.Today.PersionDayOfWeek()}  {DateTime.Now.ToShamsi()}";

            // مپ کردن تنظیمات
            InitSettings();

            // چک کردن لایسنس
            //فعال شدن امکانات  برنامه 
            if (CheckLisence())
            {
                // لاگین از روی فایل
                //CheckStateFile();

                // تنظیمات اولیه
                Init();
          
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += " " + version;
            //Init();
        }

        private void frmPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO باید تسک ها رو کنسل کنیم
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult dr = RtlMessageBox.Show("آیا میخواهید از برنامه خارج شوید؟", "خروج", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No) e.Cancel = true;
                else
                {
                    if (bonelliCore != null)
                    {
                        bonelliCore.runRobot = false;
                        bonelliCore?.doJobTask?.Wait();
                    }
                    if (bonelliCore1 != null)
                    {
                        bonelliCore1.runRobot = false;
                        bonelliCore1?.doJobTask?.Wait();
                    }
                    if (bonelliCore2 != null)
                    {
                        bonelliCore2.runRobot = false;
                        bonelliCore2?.doJobTask?.Wait();
                    }
                    if (bonelliCore3 != null)
                    {
                        bonelliCore3.runRobot = false;
                        bonelliCore3?.doJobTask?.Wait();
                    }
                    if (bonelliCore4 != null)
                    {
                        bonelliCore4.runRobot = false;
                        bonelliCore4?.doJobTask?.Wait();
                    }
                    
                    Environment.Exit(0);
                }
            }

            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                if (bonelliCore != null)
                {
                    bonelliCore.runRobot = false;
                    bonelliCore?.doJobTask?.Wait();
                }
                if (bonelliCore1 != null)
                {
                    bonelliCore1.runRobot = false;
                    bonelliCore1?.doJobTask?.Wait();
                }
                Environment.Exit(0);
            }

        }

        #endregion

        #region clickEvents

        private void mnuLicense_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo();
            frm.Show();
        }

        private void MnuBlocskReset_Click(object sender, EventArgs e)
        {
            using(Models.BonelliContext db = new Models.BonelliContext())
            {
                List<CurrentUser> lstCU = db.CurrentUsers.ToList();
                foreach (var item in lstCU)
                {
                    item.FollowBlock = null;
                    item.UnFollowBlock = null;
                }
                db.SaveChanges();
            }
            AndroidDevice androidDevice = AndroidDeviceGenerator.AndroidAndroidDeviceSets.ElementAt(0).Value;//AndroidDeviceGenerator.GetRandomAndroidDevice();
            bonelliCore._instaApi.SetDevice(androidDevice);
            //bonelliCore1._instaApi.SetDevice(androidDevice);
            //bonelliCore2._instaApi.SetDevice(androidDevice);
            //bonelliCore3._instaApi.SetDevice(androidDevice);
            //bonelliCore4._instaApi.SetDevice(androidDevice);
            btnFollow.BackColor = Color.Green;
            btnFollow1.BackColor = Color.Green;
            btnFollow2.BackColor = Color.Green;
            btnFollow3.BackColor = Color.Green;
            btnFollow4.BackColor = Color.Green;
        }


        #endregion

        #region clickEventsForFirstUser

        //private void btnLogout_Click(object sender, EventArgs e)
        //{
        //    // perform that if user needs to logged out
        //    var logoutResult = Task.Run(() => _instaApi.LogoutAsync()).GetAwaiter().GetResult();
        //    if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
        //    File.Delete("state.bin");

        //    // TODO: باید تسک ها رو کنسل کنیم 
        //    //Environment.Exit(0);

        //    Application.Exit();
        //}

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (bonelliCore._currentUser != null)
            {

                if (!CheckMinimized($"تنظیمات {bonelliCore._currentUser.Value.UserName}"))
                {
                    if (bonelliCore._currentUser != null)//(_currentUser != null)
                    {
                        settings0 = Mapper.Map<Settings>(Settings0.Default);
                        frmPanelSetting frm = new frmPanelSetting(bonelliCore._instaApi, bonelliCore._currentUser);
                        frm.settings = settings0;
                        frm.ShowDialog();
                        bonelliCore.settings = frm.settings;
                        Settings0.Default = Mapper.Map<Settings0>(frm.settings);
                        Settings0.Default.Save();
                    }
                    else
                        RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
                }
                else
                    RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
            }

            bool CheckMinimized(string name)
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm.Text == name && frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        return true;
                    }
                }
                return false;
            }
        }


        private void btnUnfollow_Click(object sender, EventArgs e)
        {
            if (bonelliCore != null)
            {
                if (_status == LicenseStatus.VALID && !Settings0.Default.unFollowStart)
                {
                    if (bonelliCore._currentUser != null)
                    {
                        using (BonelliContext db = new BonelliContext())
                        {
                            // اینجا باید تو انی شرط بزاریم که وایت لیست مرتبط به یوزر فعلی باشه
                            if (db.WhiteLists.Any(w => w.CurrentUserPk == bonelliCore._currentUser.Value.Pk))
                            {
                                Settings0.Default.unFollowStart = true;
                                bonelliCore.settings.unFollowStart = true;
                                btnUnfollow.BackColor = AppSettings.Default.Green;
                            }
                            else
                            {
                                DialogResult dr = RtlMessageBox.Show("وایت لیست شما خالی است ،درصورت تایید تمام لیست شما آنفالو خواهند شد", "اخطار", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    Settings0.Default.unFollowStart = true;
                                    bonelliCore.settings.unFollowStart = true;
                                    btnUnfollow.BackColor = AppSettings.Default.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا صبر کنید تا اطلاعات کاربر دریافت گردد");
                    }
                }
                else
                {
                    bonelliCore.settings.unFollowStart = false;
                    Settings0.Default.unFollowStart = false;
                    btnUnfollow.BackColor = AppSettings.Default.Red;
                }
                bonelliCore.settings.Save();
                Settings0.Default.Save();
            }
        }

        private void btnFollow_Click(object sender, EventArgs e)
        {
            if (bonelliCore != null)
            {
                if (_status == LicenseStatus.VALID && !Settings0.Default.followStart)
                {
                    //using (BonelliContext db = new BonelliContext())
                    {
                        // جدید اضافه کردم
                        //if (await db.TargetFollowers.AnyAsync(u => u.CurrentUserPk == bonelliCore._currentUser.Value.Pk))
                        {
                            Settings0.Default.followStart = true;
                            bonelliCore.settings.followStart = true;
                            btnFollow.BackColor = AppSettings.Default.Green;
                        }
                    }


                }
                else
                {
                    Settings0.Default.followStart = false;
                    bonelliCore.settings.followStart = false;
                    btnFollow.BackColor = AppSettings.Default.Red;
                }
                bonelliCore.settings.Save();
                Settings0.Default.Save();
            }
        }

        private void btnSupport_Click(object sender, EventArgs e)
        {
            RtlMessageBox.Show("محمد مرادی");
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text == "خروج از اکانت")
            {
                var logoutResult = Task.Run(() => bonelliCore._instaApi.LogoutAsync()).GetAwaiter().GetResult();
                if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
                File.Delete($"state{0}.bin");
                btnLogin.Text = "ورود";
                lblCurrentUser.Text = "";
                ClearAccountData(0);
                Settings0.Default.Reset();

            }
            else
            {
                frmLogin login = new frmLogin(0);
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    InitBonelliCore(0);
                }
                login.Dispose();
                login = null;
            }

            if (_status == LicenseStatus.VALID)
            {
                //Start();
            }
        }

        #endregion

        #region clickEventsForSecondUser

        private void btnSettings1_Click(object sender, EventArgs e)
        {
            if (bonelliCore1._currentUser != null)
            {
                if (!CheckMinimized($"تنظیمات {bonelliCore1._currentUser.Value.UserName}"))
                {
                    if (bonelliCore1._currentUser != null)//(_currentUser != null)
                    {
                        settings1 = Mapper.Map<Settings>(Settings1.Default);
                        frmPanelSetting frm = new frmPanelSetting(bonelliCore1._instaApi, bonelliCore1._currentUser);
                        frm.settings = settings1;
                        frm.ShowDialog();
                        bonelliCore1.settings = frm.settings;
                        Settings1.Default = Mapper.Map<Settings1>(frm.settings);
                        Settings1.Default.Save();
                    }
                    else
                        RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
                }

            }

            bool CheckMinimized(string name)
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm.Text == name && frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        return true;
                    }
                }
                return false;
            }
        }

        private void btnLogin1_Click(object sender, EventArgs e)
        {
            if (btnLogin1.Text == "خروج از اکانت")
            {
                var logoutResult = Task.Run(() => bonelliCore1._instaApi.LogoutAsync()).GetAwaiter().GetResult();
                if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
                File.Delete($"state{1}.bin");
                btnLogin1.Text = "ورود";
                lblCurrentUser1.Text = "";
                ClearAccountData(1);
                Settings1.Default.Reset();

            }
            else
            {
                frmLogin login = new frmLogin(1);
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    InitBonelliCore(1);
                }
                login.Dispose();
                login = null;
            }

            if (_status == LicenseStatus.VALID)
            {
                //Start();
            }
        }

        private void btnFollow1_Click(object sender, EventArgs e)
        {
            if (bonelliCore1 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings1.Default.followStart)
                {
                    //using (BonelliContext db = new BonelliContext())
                    {
                        // جدید اضافه کردم
                        //if (await db.TargetFollowers.AnyAsync(u => u.CurrentUserPk == bonelliCore1._currentUser.Value.Pk))
                        {
                            Settings1.Default.followStart = true;
                            bonelliCore1.settings.followStart = true;
                            btnFollow1.BackColor = AppSettings.Default.Green;
                        }
                    }
       
                }
                else
                {
                    Settings1.Default.followStart = false;
                    bonelliCore1.settings.followStart = false;
                    btnFollow1.BackColor = AppSettings.Default.Red;
                }
                bonelliCore1.settings.Save();
                Settings1.Default.Save();
            }
        }


        private void btnUnfollow1_Click(object sender, EventArgs e)
        {
            if (bonelliCore1 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings1.Default.unFollowStart)
                {
                    if (bonelliCore1._currentUser != null)
                    {
                        using (BonelliContext db = new BonelliContext())
                        {
                            // اینجا باید تو انی شرط بزاریم که وایت لیست مرتبط به یوزر فعلی باشه
                            if (db.WhiteLists.Any(w => w.CurrentUserPk == bonelliCore1._currentUser.Value.Pk))
                            {
                                Settings1.Default.unFollowStart = true;
                                bonelliCore1.settings.unFollowStart = true;
                                btnUnfollow1.BackColor = AppSettings.Default.Green;
                            }
                            else
                            {
                                DialogResult dr = RtlMessageBox.Show("وایت لیست شما خالی است ،درصورت تایید تمام لیست شما آنفالو خواهند شد", "اخطار", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    Settings1.Default.unFollowStart = true;
                                    bonelliCore1.settings.unFollowStart = true;
                                    btnUnfollow1.BackColor = AppSettings.Default.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا صبر کنید تا اطلاعات کاربر دریافت گردد");
                    }
                }
                else
                {
                    bonelliCore1.settings.unFollowStart = false;
                    Settings1.Default.unFollowStart = false;
                    btnUnfollow1.BackColor = AppSettings.Default.Red;
                }
                bonelliCore1.settings.Save();
                Settings1.Default.Save();
            }
        }
        
        #endregion

        #region clickEventsForThirdUser

        private void btnLogin2_Click(object sender, EventArgs e)
        {
            if (btnLogin2.Text == "خروج از اکانت")
            {
                var logoutResult = Task.Run(() => bonelliCore2._instaApi.LogoutAsync()).GetAwaiter().GetResult();
                if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
                File.Delete($"state{2}.bin");
                btnLogin2.Text = "ورود";
                lblCurrentUser2.Text = "";
                ClearAccountData(2);
                Settings2.Default.Reset();

            }
            else
            {
                frmLogin login = new frmLogin(2);
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    InitBonelliCore(2);
                }
                login.Dispose();
                login = null;
            }

        }

        private void lblCurrentUser3_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin4_Click(object sender, EventArgs e)
        {
            if (btnLogin4.Text == "خروج از اکانت")
            {
                var logoutResult = Task.Run(() => bonelliCore4._instaApi.LogoutAsync()).GetAwaiter().GetResult();
                if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
                File.Delete($"state{4}.bin");
                btnLogin4.Text = "ورود";
                lblCurrentUser4.Text = "";
                ClearAccountData(4);
                Settings4.Default.Reset();
            }
            else
            {
                frmLogin login = new frmLogin(4);
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    InitBonelliCore(4);
                }
                login.Dispose();
                login = null;
            }
        }

        private void btnSettings3_Click(object sender, EventArgs e)
        {
            if (bonelliCore3._currentUser != null)
            {
                if (!CheckMinimized($"تنظیمات {bonelliCore3._currentUser.Value.UserName}"))
                {
                    if (bonelliCore3._currentUser != null)//(_currentUser != null)
                    {
                        settings3 = Mapper.Map<Settings>(Settings3.Default);
                        frmPanelSetting frm = new frmPanelSetting(bonelliCore3._instaApi, bonelliCore3._currentUser);
                        frm.settings = settings3;
                        frm.ShowDialog();
                        bonelliCore3.settings = frm.settings;
                        Settings3.Default = Mapper.Map<Settings3>(frm.settings);
                        Settings3.Default.Save();
                    }
                    else
                        RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
                }

            }

            bool CheckMinimized(string name)
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm.Text == name && frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        return true;
                    }
                }
                return false;
            }
        }

        private void btnSettings4_Click(object sender, EventArgs e)
        {
            if (bonelliCore4._currentUser != null)
            {
                if (!CheckMinimized($"تنظیمات {bonelliCore4._currentUser.Value.UserName}"))
                {
                    if (bonelliCore4._currentUser != null)//(_currentUser != null)
                    {
                        settings4 = Mapper.Map<Settings>(Settings4.Default);
                        frmPanelSetting frm = new frmPanelSetting(bonelliCore4._instaApi, bonelliCore4._currentUser);
                        frm.settings = settings4;
                        frm.ShowDialog();
                        bonelliCore4.settings = frm.settings;
                        Settings4.Default = Mapper.Map<Settings4>(frm.settings);
                        Settings4.Default.Save();
                    }
                    else
                        RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
                }

            }

            bool CheckMinimized(string name)
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm.Text == name && frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        return true;
                    }
                }
                return false;
            }
        }

        private void btnFollow3_Click(object sender, EventArgs e)
        {
            if (bonelliCore3 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings3.Default.followStart)
                {
                    //using (BonelliContext db = new BonelliContext())
                    {
                        // جدید اضافه کردم
                        //if (await db.TargetFollowers.AnyAsync(u => u.CurrentUserPk == bonelliCore3._currentUser.Value.Pk))
                        {
                            Settings3.Default.followStart = true;
                            bonelliCore3.settings.followStart = true;
                            btnFollow3.BackColor = AppSettings.Default.Green;
                        }
                    }
                }
                else
                {
                    Settings3.Default.followStart = false;
                    bonelliCore3.settings.followStart = false;
                    btnFollow3.BackColor = AppSettings.Default.Red;
                }
                bonelliCore3.settings.Save();
                Settings3.Default.Save();
            }
        }

        private void btnFollow4_Click(object sender, EventArgs e)
        {
            if (bonelliCore4 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings4.Default.followStart)
                {
                    //using (BonelliContext db = new BonelliContext())
                    {
                        // جدید اضافه کردم
                        //if (await db.TargetFollowers.AnyAsync(u => u.CurrentUserPk == bonelliCore4._currentUser.Value.Pk))
                        {
                            Settings4.Default.followStart = true;
                            bonelliCore4.settings.followStart = true;
                            btnFollow4.BackColor = AppSettings.Default.Green;
                        }
                    }
                }
                else
                {
                    Settings4.Default.followStart = false;
                    bonelliCore4.settings.followStart = false;
                    btnFollow4.BackColor = AppSettings.Default.Red;
                }
                bonelliCore4.settings.Save();
                Settings4.Default.Save();
            }
        }

        private void btnUnfollow3_Click(object sender, EventArgs e)
        {
            if (bonelliCore3 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings3.Default.unFollowStart)
                {
                    if (bonelliCore3._currentUser != null)
                    {
                        using (BonelliContext db = new BonelliContext())
                        {
                            // اینجا باید تو انی شرط بزاریم که وایت لیست مرتبط به یوزر فعلی باشه
                            if (db.WhiteLists.Any(w => w.CurrentUserPk == bonelliCore3._currentUser.Value.Pk))
                            {
                                Settings3.Default.unFollowStart = true;
                                bonelliCore3.settings.unFollowStart = true;
                                btnUnfollow3.BackColor = AppSettings.Default.Green;
                            }
                            else
                            {
                                DialogResult dr = RtlMessageBox.Show("وایت لیست شما خالی است ،درصورت تایید تمام لیست شما آنفالو خواهند شد", "اخطار", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    Settings3.Default.unFollowStart = true;
                                    bonelliCore3.settings.unFollowStart = true;
                                    btnUnfollow3.BackColor = AppSettings.Default.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا صبر کنید تا اطلاعات کاربر دریافت گردد");
                    }
                }
                else
                {
                    bonelliCore3.settings.unFollowStart = false;
                    Settings3.Default.unFollowStart = false;
                    btnUnfollow3.BackColor = AppSettings.Default.Red;
                }
                bonelliCore3.settings.Save();
                Settings3.Default.Save();
            }
        }

        private void btnUnfollow4_Click(object sender, EventArgs e)
        {
            if (bonelliCore4 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings4.Default.unFollowStart)
                {
                    if (bonelliCore4._currentUser != null)
                    {
                        using (BonelliContext db = new BonelliContext())
                        {
                            // اینجا باید تو انی شرط بزاریم که وایت لیست مرتبط به یوزر فعلی باشه
                            if (db.WhiteLists.Any(w => w.CurrentUserPk == bonelliCore4._currentUser.Value.Pk))
                            {
                                Settings4.Default.unFollowStart = true;
                                bonelliCore4.settings.unFollowStart = true;
                                btnUnfollow4.BackColor = AppSettings.Default.Green;
                            }
                            else
                            {
                                DialogResult dr = RtlMessageBox.Show("وایت لیست شما خالی است ،درصورت تایید تمام لیست شما آنفالو خواهند شد", "اخطار", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    Settings4.Default.unFollowStart = true;
                                    bonelliCore4.settings.unFollowStart = true;
                                    btnUnfollow4.BackColor = AppSettings.Default.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا صبر کنید تا اطلاعات کاربر دریافت گردد");
                    }
                }
                else
                {
                    bonelliCore4.settings.unFollowStart = false;
                    Settings4.Default.unFollowStart = false;
                    btnUnfollow4.BackColor = AppSettings.Default.Red;
                }
                bonelliCore4.settings.Save();
                Settings4.Default.Save();
            }
        }

        private void btnSettings2_Click(object sender, EventArgs e)
        {
            if (bonelliCore2._currentUser != null)
            {
                if (!CheckMinimized($"تنظیمات {bonelliCore2._currentUser.Value.UserName}"))
                {
                    if (bonelliCore2._currentUser != null)//(_currentUser != null)
                    {
                        settings2 = Mapper.Map<Settings>(Settings2.Default);
                        frmPanelSetting frm = new frmPanelSetting(bonelliCore2._instaApi, bonelliCore2._currentUser);
                        frm.settings = settings2;
                        frm.ShowDialog();
                        bonelliCore2.settings = frm.settings;
                        Settings2.Default = Mapper.Map<Settings2>(frm.settings);
                        Settings2.Default.Save();
                    }
                    else
                        RtlMessageBox.Show("لطفا صبر کنید تااطلاعات کاربر دریافت گردد");
                }

            }

            bool CheckMinimized(string name)
            {
                FormCollection fc = Application.OpenForms;

                foreach (Form frm in fc)
                {
                    if (frm.Text == name && frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        return true;
                    }
                }
                return false;
            }
        }

        private void btnLogin3_Click(object sender, EventArgs e)
        {
            if (btnLogin3.Text == "خروج از اکانت")
            {
                var logoutResult = Task.Run(() => bonelliCore3._instaApi.LogoutAsync()).GetAwaiter().GetResult();
                if (!logoutResult.Succeeded) MessageBox.Show("نمیتواند خارج شود");
                File.Delete($"state{3}.bin");
                btnLogin3.Text = "ورود";
                lblCurrentUser3.Text = "";
                ClearAccountData(3);
                Settings3.Default.Reset();

            }
            else
            {
                frmLogin login = new frmLogin(3);
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    InitBonelliCore(3);
                }
                login.Dispose();
                login = null;
            }
        }

        private void btnFollow2_Click(object sender, EventArgs e)
        {
            if (bonelliCore2 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings2.Default.followStart)
                {
                    //using(BonelliContext db = new BonelliContext())
                    {
                        // جدید اضافه کردم
                        //if(await db.TargetFollowers.AnyAsync(u => u.CurrentUserPk == bonelliCore2._currentUser.Value.Pk))
                        {
                            Settings2.Default.followStart = true;
                            bonelliCore2.settings.followStart = true;
                            btnFollow2.BackColor = AppSettings.Default.Green;
                        }
                    }
                }
                else
                {
                    Settings2.Default.followStart = false;
                    bonelliCore2.settings.followStart = false;
                    btnFollow2.BackColor = AppSettings.Default.Red;
                }
                bonelliCore2.settings.Save();
                Settings2.Default.Save();
            }

        }

        private void btnUnfollow2_Click(object sender, EventArgs e)
        {
            if (bonelliCore2 != null)
            {
                if (_status == LicenseStatus.VALID && !Settings2.Default.unFollowStart)
                {
                    if (bonelliCore2._currentUser != null)
                    {
                        using (BonelliContext db = new BonelliContext())
                        {
                            // اینجا باید تو انی شرط بزاریم که وایت لیست مرتبط به یوزر فعلی باشه
                            if (db.WhiteLists.Any(w => w.CurrentUserPk == bonelliCore2._currentUser.Value.Pk))
                            {
                                Settings2.Default.unFollowStart = true;
                                bonelliCore2.settings.unFollowStart = true;
                                btnUnfollow2.BackColor = AppSettings.Default.Green;
                            }
                            else
                            {
                                DialogResult dr = RtlMessageBox.Show("وایت لیست شما خالی است ،درصورت تایید تمام لیست شما آنفالو خواهند شد", "اخطار", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    Settings2.Default.unFollowStart = true;
                                    bonelliCore2.settings.unFollowStart = true;
                                    btnUnfollow2.BackColor = AppSettings.Default.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا صبر کنید تا اطلاعات کاربر دریافت گردد");
                    }
                }
                else
                {
                    bonelliCore2.settings.unFollowStart = false;
                    Settings2.Default.unFollowStart = false;
                    btnUnfollow2.BackColor = AppSettings.Default.Red;
                }
                bonelliCore2.settings.Save();
                Settings2.Default.Save();
            }

        }

        #endregion

    }

   

}

#region Old

#region customMethods

//private void UpdatePercentage(long follow, long followBack)
//{
//    if (follow != 0)
//    {
//        lblPercentage.BeginInvoke((Action)(() =>
//            lblPercentage.Text =
//                (((double)followBack * 100.0) /
//                 (double)follow).ToString("00.00") + " %"));
//    }
//}

//private void WaitForConnectingToInternet()
//{
//    this.BeginInvoke((Action)(() =>
//        this.Text =
//            "در ارتباط با اینترنت مشکلی رخ داده است. لطفا چند دقیقه صبر نمایید"));
//    this.BeginInvoke((Action)(() => this.BackColor = Color.Orange));
//    Thread.Sleep(2 * 60 * 1000);
//}


//private Counter CreateCounterTable(long pk)
//{
//    Models.Counter newCounter = new Models.Counter();
//    newCounter.Reqeusted = 0;
//    newCounter.Follow = 0;
//    newCounter.UnFollow = 0;
//    newCounter.FollowBack = 0;
//    newCounter.Skipped = 0;
//    newCounter.CurrentUserPk = pk;
//    return newCounter;
//}

//private void FillAccountDataLabels(Counter counter, long followBack)
//{
//    lblFollowed.BeginInvoke((Action)(() => lblFollowed.Text = counter.Follow.ToString()));
//    lblUnFollowed.BeginInvoke((Action)(() => lblUnFollowed.Text = counter.UnFollow.ToString()));
//    //lblFolloweBack.BeginInvoke((Action)(() =>
//    //lblFolloweBack.Text = counter.FollowBack.ToString()));
//    lblFolloweBack.BeginInvoke((Action)(() =>
//        lblFolloweBack.Text = followBack.ToString()));
//    //UpdatePercentage(counter.Follow, counter.FollowBack);
//    UpdatePercentage(counter.Follow, followBack);
//    lblSkipped.BeginInvoke((Action)(() => lblSkipped.Text = counter.Skipped.ToString()));
//    lblSkipped.BeginInvoke((Action)(() => lblRequested.Text = counter.Reqeusted.ToString()));
//}

#endregion

//#region DoJob

//Task doJobTask;
//bool runRobot = true;

//public bool Start()
//{
//    if (doJobTask == null)
//    {
//        runRobot = true;
//        try
//        {
//            doJobTask = new Task(MainJob);
//            doJobTask.Start();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log.txt", true))
//            {
//                writer.WriteLine(ex.ToString());
//                writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now +
//                                 ") ----------------------------------------------\r\n");
//            }

//            return false;
//        }
//    }
//    else
//    {
//        //Console.WriteLine(" > Robot is Started!");
//        return false;
//    }
//}

//private async void MainJob()
//{
//    string nextIdMyFollowing = null;
//    long baseFollower = -1, currentFollower = -1;

//    Stopwatch stopwatchFollow = new Stopwatch();
//    stopwatchFollow.Start();

//    Stopwatch stopwatchUnFollow = new Stopwatch();
//    stopwatchUnFollow.Start();

//    Stopwatch stopwatchUnFollowMyFollower = new Stopwatch();
//    stopwatchUnFollowMyFollower.Start();
//    TimeSpan tsUnFollowMyFollower = new TimeSpan(0, 0, 0);

//    Models.BonelliContext db = new Models.BonelliContext();

//    #region UpdatePanle

//    Stopwatch stopwatchUpdateCurrentUser = new Stopwatch();
//    stopwatchUpdateCurrentUser.Start();

//    this.BeginInvoke((Action)(() => this.Text = "در حال دریافت اطلاعات کاربر"));

//    IResult<InstaCurrentUser> currentUser = await _instaApi.GetCurrentUserAsync();
//    for (int tr = 0; tr < 5; tr++)
//    {
//        try
//        {
//            currentUser = await _instaApi.GetCurrentUserAsync();
//            if (currentUser.Succeeded)
//            {
//                break;
//            }
//            else
//            {
//                continue;
//            }
//        }
//        catch (Exception ex)
//        {
//            Program.Log(ex, "UpdatePanle");
//            continue;
//        }
//    }

//    if (currentUser.Succeeded)
//    {
//        _currentUser = currentUser;

//        this.BeginInvoke((Action)(() => this.Text = "پنل مدیریت"));
//        this.BeginInvoke((Action)(() => this.BackColor = Color.FromArgb(139, 184, 225)));

//        /*if (!db.CurrentUsers.Any(cu => cu.Pk == currentUser.Value.Pk))
//        {
//            db.InstaCurrentUsers.Add(Mapper.Map<InstaCurrentUser>(currentUser.Value));
//            CurrentUser currentUserTmp = new CurrentUser();
//            currentUserTmp.Pk = currentUser.Value.;

//            await db.SaveChangesAsync();
//        }*/

//        lblCurrentUser.BeginInvoke((Action)(() =>
//           lblCurrentUser.Text = $"{currentUser.Value.UserName}({currentUser.Value.FullName})"));
//        //toolTip.SetToolTip(lblCurrentUser, $"{currentUser.Value.UserName}({currentUser.Value.FullName})");
//        var fullInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(currentUser.Value.Pk);
//        if (fullInfo.Succeeded)
//        {
//            lblFollowers.BeginInvoke(
//                (Action)(() => lblFollowers.Text = fullInfo.Value.FollowerCount.ToString()));
//            lblFollowings.BeginInvoke((Action)(() =>
//               lblFollowings.Text = fullInfo.Value.FollowingCount.ToString()));
//            if (!db.CurrentUsers.Any(cu => cu.Pk == fullInfo.Value.Pk))
//            {
//                //db.InstaUserInfos.Add(Mapper.Map<InstaUserInfo>(fullInfo.Value));
//                CurrentUser currentUserTmp = new CurrentUser();
//                currentUserTmp.Pk = currentUser.Value.Pk;
//                currentUserTmp.FollowerCount = fullInfo.Value.FollowerCount;
//                db.CurrentUsers.Add(currentUserTmp);
//                await db.SaveChangesAsync();
//                baseFollower = fullInfo.Value.FollowerCount;
//            }
//            else
//            {
//                baseFollower = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(s => s.FollowerCount).FirstOrDefaultAsync();

//            }
//            if (baseFollower == null || baseFollower < 0)
//            {
//                //TODO
//                RtlMessageBox.Show("خطایی رخ داده است برنامه را دباره اجرا کنید");
//                Application.Exit();
//            }
//            currentFollower = fullInfo.Value.FollowerCount;
//            var counter = await db.Counters.Where(c => c.CurrentUserPk == fullInfo.Value.Pk).FirstOrDefaultAsync();
//            if (counter != null)
//            {
//                FillAccountDataLabels(counter, currentFollower - baseFollower);
//                //UpdatePercentage(counter.Follow, currentFollower - baseFollower);
//            }
//        }
//    }
//    else if (!IsConnectedToInternet())
//    {
//        WaitForConnectingToInternet();

//    }
//    else if (currentUser.Info.ResponseType == ResponseType.LoginRequired)
//    {
//        // اینجا رو باید تغییر بدم
//        RtlMessageBox.Show("شما از اکانت کاربری خود خارج شدید. لطفا  دوباره لاگین کنید");
//        File.Delete("state.bin");
//        Application.Exit();
//    }
//    else
//    {
//        Program.Log(new Exception(currentUser.Info.Message), "اپدیت پنل اول");
//    }

//    #endregion UpdatePanle

//    // پروسه اصلی
//    while (runRobot)
//    {
//        if (!CheckLicenseDate()) break;

//        try
//        {
//            // اپدیت پنل

//            #region UpdatePanle

//            if (stopwatchUpdateCurrentUser.ElapsedMilliseconds > 2 * 60 * 1000)
//            {

//                this.BeginInvoke((Action)(() => this.Text = "در حال دریافت اطلاعات کاربر"));
//                stopwatchUpdateCurrentUser.Restart();
//                for (int tr = 0; tr < 3; tr++)
//                {
//                    try
//                    {
//                        currentUser = await _instaApi.GetCurrentUserAsync();
//                        if (currentUser.Succeeded)
//                        {
//                            break;
//                        }
//                        else
//                        {
//                            continue;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Program.Log(ex, "UpdatePanle");
//                        continue;
//                    }
//                }

//                if (currentUser.Succeeded)
//                {
//                    this.BeginInvoke((Action)(() => this.Text = "پنل مدیریت"));
//                    this.BeginInvoke((Action)(() => this.BackColor = Color.FromArgb(139, 184, 225)));
//                    /*if (!db.InstaCurrentUsers.Any(cu => cu.Pk == currentUser.Value.Pk))
//                    {
//                        db.InstaCurrentUsers.Add(Mapper.Map<InstaCurrentUser>(currentUser.Value));
//                        await db.SaveChangesAsync();
//                    }*/

//                    lblCurrentUser.BeginInvoke((Action)(() =>
//                       lblCurrentUser.Text = $"{currentUser.Value.UserName}({currentUser.Value.FullName})"));
//                    var fullInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(currentUser.Value.Pk);

//                    if (fullInfo.Succeeded)
//                    {
//                        lblFollowers.BeginInvoke((Action)(() =>
//                           lblFollowers.Text = fullInfo.Value.FollowerCount.ToString()));
//                        lblFollowings.BeginInvoke((Action)(() =>
//                           lblFollowings.Text = fullInfo.Value.FollowingCount.ToString()));
//                        /*if (!db.InstaUserInfos.Any(cu => cu.Pk == fullInfo.Value.Pk))
//                        {
//                            db.InstaUserInfos.Add(Mapper.Map<BonelliBot.Models.InstaUserInfo>(fullInfo.Value));
//                            await db.SaveChangesAsync();
//                        }*/
//                        currentFollower = fullInfo.Value.FollowerCount;
//                        var counter = await db.Counters.Where(c => c.CurrentUserPk == fullInfo.Value.Pk).FirstOrDefaultAsync();
//                        if (counter != null)
//                        {
//                            FillAccountDataLabels(counter, currentFollower - baseFollower);
//                            //UpdatePercentage(counter.Follow, currentFollower - baseFollower);
//                        }
//                    }
//                }
//                else
//                {
//                    WaitForConnectingToInternet();
//                    continue;
//                    //Application.Exit();
//                }
//            }
//            else
//            {
//                this.BeginInvoke((Action)(() => this.Text = "پنل مدیریت"));
//            }

//            #endregion UpdatePanle

//            // دریافت لیست فالور ها

//            #region GetFollowerTarget
//            if (db.TargetFollowers.Count(w => w.CurrentUserPk == currentUser.Value.Pk) < 10000)
//                if (db.Targets.Any(t => t.CurrentUserPk == currentUser.Value.Pk))
//                {
//                    btnSettings.BeginInvoke((Action)(() => btnSettings.BackColor = Color.Green));
//                    DateTime dateTime = DateTime.Now - TimeSpan.FromMinutes(2);
//                    var targetUser = await db.Targets.Where(target => target.CurrentUserPk == currentUser.Value.Pk
//                                                                        && target.LastUpdate < dateTime)
//                        .FirstOrDefaultAsync();
//                    if (targetUser != null)
//                    {
//                        this.BeginInvoke((Action)(() => this.Text = "در حال دریافت فالورهای کاربران هدف"));
//                        if (targetUser.Status == completed)
//                        {
//                            db.Targets.Remove(targetUser);
//                            await db.SaveChangesAsync();
//                            continue;
//                        }
//                        IResult<InstagramApiSharp.Classes.Models.InstaUserShortList> targetFollower;
//                        if (targetUser.NextMaxId == null)
//                        {
//                            // get target follower 
//                            targetFollower =
//                                await _instaApi.UserProcessor.GetUserFollowersAsync(targetUser.UserName,
//                                    PaginationParameters.MaxPagesToLoad(1));
//                        }
//                        else
//                        {
//                            // get target follower by next id
//                            targetFollower = await _instaApi.UserProcessor.GetUserFollowersAsync(
//                                targetUser.UserName,
//                                PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(targetUser.NextMaxId));
//                        }
//                        if (targetFollower.Succeeded)
//                        {
//                            for (int i = 0; i < targetFollower.Value.Count; i++)
//                            {
//                                TargetFollower targetFollowerTmp = new TargetFollower();
//                                targetFollowerTmp.LastUpdate = DateTime.Now;
//                                targetFollowerTmp.Pk = targetFollower.Value[i].Pk;
//                                targetFollowerTmp.CurrentUserPk = currentUser.Value.Pk;

//                                db.TargetFollowers.Add(targetFollowerTmp);
//                            }
//                            if (targetFollower.Value.NextMaxId == null)
//                            {
//                                targetUser.Status = completed;
//                            }

//                            targetUser.NextMaxId = targetFollower.Value.NextMaxId;
//                            targetUser.LastUpdate = DateTime.Now;
//                            await db.SaveChangesAsync();
//                        }
//                        else
//                        {
//                            // اگه نتونیم لیست رو بگیری یعنی یا اینترنت قطع شده یا طرف مقابل بلاکمون کرده یا پیجش پرایوت
//                            if (!IsConnectedToInternet())
//                            {
//                                WaitForConnectingToInternet();
//                                continue;
//                            }

//                            db.Targets.Remove(targetUser);
//                            await db.SaveChangesAsync();
//                        }
//                    }
//                }
//                else
//                {
//                    btnSettings.BeginInvoke((Action)(() => btnSettings.BackColor = Color.Red));
//                }

//            #endregion GetFollowerTarget

//            // پروسه فالو

//            #region SendRequestFollow
//            //if (stopwatchFollow.ElapsedMilliseconds > tsFollowBlock.TotalMilliseconds)
//            DateTime? followBlock = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(dt => dt.FollowBlock).FirstOrDefaultAsync();
//            if (DateTime.Now >= followBlock || followBlock == null)
//            {
//                if (stopwatchFollow.ElapsedMilliseconds > (60 / Settings.Default.followPerHour) * 60000)
//                {

//                    if (Settings.Default.followStart)
//                    {

//                        var instaUser = await db.TargetFollowers.Where(d => d.CurrentUserPk == currentUser.Value.Pk)
//                            .FirstOrDefaultAsync();
//                        if (instaUser != null)
//                        {
//                            this.BeginInvoke((Action)(() => this.Text = "در حال فالو کردن"));
//                            var instaUserInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(instaUser.Pk);
//                            if (instaUserInfo.Succeeded)
//                            {
//                                var friendship =
//                                    await _instaApi.UserProcessor.GetFriendshipStatusAsync(instaUserInfo.Value.Pk);
//                                if (friendship.Succeeded)
//                                {
//                                    // شرط دوم رو اینجا من اضافه کردم که فالو بک ها فالو نشن
//                                    if (friendship.Value.Following == false &&
//                                        friendship.Value.FollowedBy == false &&
//                                        friendship.Value.IncomingRequest == false &&
//                                        friendship.Value.OutgoingRequest == false)
//                                    {

//                                        if ((Settings.Default.profilePic && instaUserInfo.Value.HasAnonymousProfilePicture) ||
//                                            (Settings.Default.privateAccount && instaUserInfo.Value.IsPrivate))
//                                        {
//                                            // اینجا باید یک جدول داشته باشیم که داخلش فالوهای انجام شده را بشماریم
//                                            if (!db.Counters.Any(c => c.CurrentUserPk == currentUser.Value.Pk))
//                                            {
//                                                db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
//                                                await db.SaveChangesAsync();
//                                            }

//                                            var counter = await db.Counters.Where(c => c.CurrentUserPk == currentUser.Value.Pk)
//                                                .FirstOrDefaultAsync();
//                                            if (counter != null)
//                                            {
//                                                counter.Skipped++;
//                                                lblSkipped.BeginInvoke((Action)(() =>
//                                                   lblSkipped.Text = counter.Skipped.ToString()));
//                                            }

//                                            db.TargetFollowers.Remove(instaUser);
//                                            await db.SaveChangesAsync();
//                                            //continue;
//                                        }
//                                        else
//                                        {
//                                            friendship =
//                                                await _instaApi.UserProcessor.FollowUserAsync(instaUserInfo.Value.Pk);

//                                            if (friendship.Succeeded)
//                                            {
//                                                if (Settings.Default.followStart)
//                                                    btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Green));
//                                                stopwatchFollow.Restart();

//                                                db.TargetFollowers.Remove(instaUser);

//                                                TargetUnFollow targetUnFollowTmp = new TargetUnFollow();
//                                                targetUnFollowTmp.CurrentUserPk = currentUser.Value.Pk;
//                                                targetUnFollowTmp.LastUpdate = DateTime.Now; targetUnFollowTmp.CurrentUserPk = currentUser.Value.Pk;
//                                                targetUnFollowTmp.Pk = instaUserInfo.Value.Pk;
//                                                db.TargetUnFollows.Add(targetUnFollowTmp);
//                                                // اینجا باید یک جدول داشته باشیم که داخلش فالوهای انجام شده را بشماریم
//                                                if (!db.Counters.Any(c => c.CurrentUserPk == currentUser.Value.Pk))
//                                                {
//                                                    db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
//                                                    await db.SaveChangesAsync();
//                                                }

//                                                var counter = await db.Counters.Where(c => c.CurrentUserPk == currentUser.Value.Pk)
//                                                    .FirstOrDefaultAsync();
//                                                if (counter != null)
//                                                {
//                                                    if (friendship.Value.OutgoingRequest)
//                                                    {
//                                                        // TODO اینجا اگر پیج هدف پرایوت بوده و درخواست فالو فرستاده بودیم به کانتر درخواست شده ها اضافه کنیم
//                                                        counter.Reqeusted++;
//                                                    }

//                                                    // اینجا اگر پیج هدف پابلیک بود به تعداد فالو شده ها اضافه کنیم
//                                                    counter.Follow++;

//                                                    lblFollowed.BeginInvoke((Action)(() =>
//                                                       lblFollowed.Text = counter.Follow.ToString()));
//                                                    lblFollowed.BeginInvoke((Action)(() =>
//                                                       lblRequested.Text = counter.Reqeusted.ToString()));

//                                                    //UpdatePercentage(counter.Follow, counter.FollowBack);
//                                                    UpdatePercentage(counter.Follow, currentFollower - baseFollower);
//                                                }

//                                                await db.SaveChangesAsync();
//                                            }
//                                            else if (friendship.Info.ResponseType == ResponseType.Spam)
//                                            {

//                                                // اینجا اومد یعنی بلاک شده

//                                                CurrentUser cuTmp = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).FirstOrDefaultAsync();
//                                                cuTmp.FollowBlock = DateTime.Now.AddHours(12);
//                                                db.CurrentUsers.Add(cuTmp);
//                                                await db.SaveChangesAsync();
//                                                stopwatchFollow.Restart();
//                                                btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Yellow));

//                                            }
//                                            else
//                                            {
//                                                Program.Log(new Exception(friendship.Info.Message), "فالو");
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        db.TargetFollowers.Remove(instaUser);
//                                        await db.SaveChangesAsync();
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                if (!IsConnectedToInternet())
//                                {
//                                    WaitForConnectingToInternet();
//                                    continue;
//                                }
//                                Program.Log(new Exception(instaUserInfo.Info.ResponseType.ToString()), "مشکلی در دریافت اطلاعات کاربر برای فالو کردن وجود دارد");
//                                if (instaUserInfo.Info.ResponseType == ResponseType.UnExpectedResponse)
//                                {
//                                    db.TargetFollowers.Remove(instaUser);
//                                    await db.SaveChangesAsync();
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Yellow));
//                //btnFollow.BeginInvoke((Action)(() => btnFollow.Text = "فالو\n\rBlock"));
//            }

//            #endregion SendRequestFollow

//            // پروسه انفالو

//            #region UnFollow
//            //if (stopwatchUnFollow.ElapsedMilliseconds > tsUnFollowBlock.TotalMilliseconds)
//            DateTime? unFollowBlock = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(dt => dt.UnFollowBlock).FirstOrDefaultAsync();
//            if (DateTime.Now >= unFollowBlock || unFollowBlock == null)
//            {
//                if (stopwatchUnFollow.ElapsedMilliseconds > (60 / Settings.Default.unFollowPerHour) * 60000)
//                {

//                    if (Settings.Default.unFollowStart)
//                    {
//                        DateTime dtTmp = DateTime.Now - TimeSpan.FromDays(2);
//                        long cui = db.TargetUnFollows.Count(c => c.CurrentUserPk == currentUser.Value.Pk && c.LastUpdate < dtTmp);
//                        if (cui < 10) tsUnFollowMyFollower = TimeSpan.FromMinutes(0);
//                        else if (cui >= 1000 && cui < 2000) tsUnFollowMyFollower = TimeSpan.FromMinutes(60);
//                        else if (cui >= 2000 && cui <= 5000) tsUnFollowMyFollower = TimeSpan.FromMinutes(120);
//                        else
//                        {
//                            tsUnFollowMyFollower = TimeSpan.FromMinutes(180);
//                            stopwatchUnFollowMyFollower.Restart();
//                        }
//                        if (stopwatchUnFollowMyFollower.ElapsedMilliseconds > tsUnFollowMyFollower.TotalMilliseconds)
//                        {
//                            tsUnFollowMyFollower = TimeSpan.FromMinutes(15);
//                            stopwatchUnFollowMyFollower.Restart();
//                            IResult<InstagramApiSharp.Classes.Models.InstaUserShortList> myFollowing;
//                            if (nextIdMyFollowing == null)
//                            {
//                                this.BeginInvoke((Action)(() => this.Text = "در حال جمع آوری فالوینگ های یوزر فعلی"));
//                                myFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(currentUser.Value.UserName,
//                                    PaginationParameters.MaxPagesToLoad(1));
//                            }
//                            else
//                            {
//                                this.BeginInvoke((Action)(() => this.Text = "در حال جمع آوری فالوینگ های یوزر فعلی"));
//                                myFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(
//                                        currentUser.Value.UserName,
//                                        PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(nextIdMyFollowing));
//                            }


//                            if (myFollowing.Succeeded)
//                            {

//                                for (int i = 0; i < myFollowing.Value.Count /*&& i < 250*/; i++)
//                                {
//                                    long pk = myFollowing.Value[i].Pk;
//                                    // این شرط رو اافه کردم که تو لوپ نیفته 

//                                    if (!db.WhiteLists.Any(w => w.CurrentUserPk == currentUser.Value.Pk && w.Pk == pk))
//                                    {
//                                        if (!db.TargetUnFollows.Any(d => d.CurrentUserPk == currentUser.Value.Pk && d.Pk == pk))
//                                        {
//                                            TargetUnFollow targetUnFollowTmp = new TargetUnFollow();
//                                            targetUnFollowTmp.LastUpdate = DateTime.Now - TimeSpan.FromDays(2);
//                                            targetUnFollowTmp.Pk = myFollowing.Value[i].Pk;
//                                            targetUnFollowTmp.CurrentUserPk = currentUser.Value.Pk;
//                                            db.TargetUnFollows.Add(targetUnFollowTmp);
//                                        }
//                                    }
//                                }
//                                nextIdMyFollowing = myFollowing.Value.NextMaxId;
//                                await db.SaveChangesAsync();
//                            }
//                        }
//                        DateTime dtunFollow = DateTime.Now - TimeSpan.FromDays(2);
//                        var targetUnFollow = await db.TargetUnFollows.Where(d => d.CurrentUserPk == currentUser.Value.Pk && d.LastUpdate <= dtunFollow)
//                            .FirstOrDefaultAsync();

//                        if (targetUnFollow != null &&
//                            !db.WhiteLists.Any(w => w.CurrentUserPk == currentUser.Value.Pk && w.Pk == targetUnFollow.Pk))
//                        {
//                            this.BeginInvoke((Action)(() => this.Text = "در حال آنفالو کردن"));
//                            var friendship =
//                                await _instaApi.UserProcessor.GetFriendshipStatusAsync(targetUnFollow.Pk);
//                            if (friendship.Succeeded)
//                            {
//                                if (!db.Counters.Any(c => c.CurrentUserPk == currentUser.Value.Pk))
//                                {
//                                    db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
//                                    await db.SaveChangesAsync();
//                                }

//                                var counter = await db.Counters.FirstOrDefaultAsync(c => c.CurrentUserPk == currentUser.Value.Pk);

//                                if (counter != null && friendship.Value.FollowedBy)
//                                {
//                                    counter.FollowBack++;
//                                    lblFolloweBack.BeginInvoke((Action)(() =>
//                                       lblFolloweBack.Text = (currentFollower - baseFollower).ToString()));
//                                    UpdatePercentage(counter.Follow, currentFollower - baseFollower);
//                                }

//                                friendship =
//                                            await _instaApi.UserProcessor.UnFollowUserAsync(targetUnFollow.Pk);
//                                if (friendship.Succeeded)
//                                {
//                                    if (Settings.Default.unFollowStart)
//                                        btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Green));

//                                    stopwatchUnFollow.Restart();
//                                    counter.UnFollow++;
//                                    lblUnFollowed.BeginInvoke((Action)(() =>
//                                       lblUnFollowed.Text = counter.UnFollow.ToString()));
//                                }
//                                else if (friendship.Info.ResponseType == ResponseType.Spam)
//                                {
//                                    // اینجا اومد یعنی بلاک شده

//                                    CurrentUser cuTmp = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).FirstOrDefaultAsync();
//                                    cuTmp.UnFollowBlock = DateTime.Now.AddHours(12);
//                                    db.CurrentUsers.Add(cuTmp);
//                                    await db.SaveChangesAsync();
//                                    stopwatchUnFollow.Restart();
//                                    btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Yellow));
//                                }
//                                else
//                                {
//                                    Program.Log(new Exception(friendship.Info.Message), "انفالو");
//                                }
//                                db.TargetUnFollows.Remove(targetUnFollow);
//                                await db.SaveChangesAsync();
//                            }
//                            else
//                            {
//                                if (!IsConnectedToInternet())
//                                {
//                                    WaitForConnectingToInternet();
//                                    continue;
//                                }

//                                Program.Log(new Exception(friendship.Info.ResponseType.ToString()), "مشکلی در دریافت اطلاعات کاربر برای انفالو کردن وجود دارد");
//                                if (friendship.Info.ResponseType == ResponseType.UnExpectedResponse)
//                                {
//                                    db.TargetUnFollows.Remove(targetUnFollow);
//                                    await db.SaveChangesAsync();
//                                }
//                            }
//                        }
//                        else
//                        {
//                            // اینجا اومد یعنی تو وایت لیست بوده
//                            db.TargetUnFollows.Remove(targetUnFollow);
//                            await db.SaveChangesAsync();
//                        }
//                    }

//                }
//            }
//            else
//            {
//                btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Yellow));
//                //btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.Text = "آنفالو\n\rBlock"));
//            }

//            #endregion UnFollow

//            Thread.Sleep(250);
//        }
//        catch (Exception e)
//        {
//            Program.Log(e, "حلقه اصلی برنامه");
//            continue;
//        }
//    }

//    db.Dispose();
//}

//#endregion DoJob

#endregion