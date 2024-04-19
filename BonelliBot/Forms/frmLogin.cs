using BonelliBot.Forms;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using BonelliBot;
using InstagramApiSharp.Logger;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Utility.UI;
using InstagramApiSharp.Classes;
using System.Net;
using System.Net.Http;
using BonelliBot.Properties;
using AutoMapper;
using InstagramApiSharp.Classes.Android.DeviceInfo;

namespace BonelliBot.Forms
{
    public partial class frmLogin : Form
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        /// 
        //private static IInstaApi _instaApi;
        public IInstaApi _instaApi;
        public static string user = "mldsalehi2018";
        public static string pass = "Alimilad113722";
        public static Label lblMsg = new Label();
        int userNumber=0;
        public Settings settings;

        //متغیر مربوط به پروکسی
        public static HttpClientHandler httpClientHandler = new HttpClientHandler();

        #region loadFormEvents

        public frmLogin()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            InitForm();
        }
        public frmLogin(int userNumber)
        {
            InitializeComponent();
            this.userNumber = userNumber;
            this.FormBorderStyle = FormBorderStyle.None;
            InitForm();
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }


        private async void frmLogin_Load(object sender, EventArgs e)
        {
            
            if (settings.useProxy) mnuEdit.BackColor = Color.Green;
            else mnuEdit.BackColor = Color.FromArgb(48, 79, 107);
            
            string stateFile = $"state{userNumber}.bin";
            try
            {
                if (File.Exists(stateFile))
                {
                    var userSession = new UserSessionData();
                    userSession.UserName = settings.username;
                    userSession.Password = settings.password;
                    //switch (userNumber)
                    //{
                    //    case 0:
                    //        userSession.UserName = Properties.Settings0.Default.username;//"username",
                    //        userSession.Password = Properties.Settings0.Default.password;
                    //        break;
                    //    case 1:
                    //        userSession.UserName = Properties.Settings1.Default.username;//"username",
                    //        userSession.Password = Properties.Settings1.Default.password;
                    //        break;
                    //    case 2:
                    //        userSession.UserName = Properties.Settings2.Default.username;//"username",
                    //        userSession.Password = Properties.Settings2.Default.password;
                    //        break;
                    //    case 3:
                    //        userSession.UserName = Properties.Settings3.Default.username;//"username",
                    //        userSession.Password = Properties.Settings3.Default.password;
                    //        break;
                    //    case 4:
                    //        userSession.UserName = Properties.Settings4.Default.username;//"username",
                    //        userSession.Password = Properties.Settings4.Default.password;
                    //        break;
                    //}
                    

                    var delay = RequestDelay.FromSeconds(2, 2);
                    // create new InstaApi instance using Builder
                    _instaApi = InstaApiBuilder.CreateBuilder()
                        .SetUser(userSession)
                        .UseLogger(new DebugLogger(LogLevel.Exceptions)) // use logger for requests and debug messages
                        .SetRequestDelay(delay)
                        .UseHttpClientHandler(httpClientHandler)
                        .Build();
                    using (var fs = File.OpenRead(stateFile))
                    {
                        _instaApi.LoadStateDataFromStream(fs);
                    }
                    //  چک کنیم شاید کاربر دیگه لاگین نباشه
                    try
                    {
                        if (!_instaApi.IsUserAuthenticated)
                        {
                            // login
                            lblMsg.BeginInvoke((Action)(() => lblMsg.Text = $"Logging in as {userSession.UserName}"));

                            delay.Disable();
                            var logInResult = await _instaApi.LoginAsync();
                            delay.Enable();
                            if (!logInResult.Succeeded)
                            {
                                lblMsg.BeginInvoke((Action)(() => lblMsg.Text = $"Unable to login: {logInResult.Info.Message}"));

                                CheckResponse(logInResult.Value);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Program.Log(exception, "تو فرم لود بعد از خوندن فابل وقتی میخاد لاگین کنه ");
                    }
                    //this.WindowState = FormWindowState.Minimized;
                    //this.ShowInTaskbar = false;
                    //frmPanel panel = new frmPanel(_instaApi);
                    //panel.Show();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Program.Log(ex, "احتمالا مشکلی در خوندن فایل دات بین وجود داره وقتی داره فرم رو لود میکنه");
            }
        }

        private void frmLogin_MouseEnter(object sender, EventArgs e)
        {
            settings = InitSetting(userNumber);
            if (settings.useProxy) mnuEdit.BackColor = Color.Green;
            else mnuEdit.BackColor = Color.FromArgb(48, 79, 107);
        }

        #endregion

        #region MainProcess

        private void btnLogin_Click(object sender, EventArgs e)
        {
            settings = InitSetting(userNumber);

            btnLogin.Enabled = false;
            user = txtUser.Text;
            pass = txtPass.Text;
            lblMsg = lblMessage;

            if (settings.useProxy) SetProxy();

            // Send Login Request
            var result = Task.Run(Init).GetAwaiter().GetResult();
            // Handle Response
            CheckResponse(result);

        }
        public async Task<InstaLoginResult> Init()
        {
            try
            {
                lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال اتصال ... "));
                // create user session data and provide login details
                var userSession = new UserSessionData();
                userSession.UserName = user;
                userSession.Password = pass;

                var delay = RequestDelay.FromSeconds(10, 60);
                // create new InstaApi instance using Builder
                _instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.Exceptions)) // use logger for requests and debug messages
                    .SetRequestDelay(delay)
                    .UseHttpClientHandler(httpClientHandler)
                    .Build();

                string stateFile = $"state{userNumber}.bin";
                
                if (!_instaApi.IsUserAuthenticated)
                {
                    // login
                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = $"Logging in as {userSession.UserName}"));

                    delay.Disable();
                    var logInResult = await _instaApi.LoginAsync();
                    delay.Enable();
                    if (!logInResult.Succeeded)
                    {
                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = $"Unable to login: {logInResult.Info.Message}"));

                        return logInResult.Value; // return false
                    }
                }
                var state = _instaApi.GetStateDataAsStream();
                using (var fileStream = File.Create(stateFile))
                {
                    state.Seek(0, SeekOrigin.Begin);
                    state.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                lblMsg.BeginInvoke((Action)(() => lblMsg.Text = ex.ToString()));
                Program.Log(ex, InstaLoginResult.Exception.ToString());
                return InstaLoginResult.Exception;
            }
            finally
            {
                // perform that if user needs to logged out
                // var logoutResult = Task.Run(() => _instaApi.LogoutAsync()).GetAwaiter().GetResult();
                // if (logoutResult.Succeeded) Console.WriteLine("Logout succeed");
            }
            settings.username = user;
            settings.password = pass;
            SaveSetting(userNumber, settings);
            
            return InstaLoginResult.Success;//Login Successfully
        }

        #endregion

        #region clickEvents

        private void mnuExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = RtlMessageBox.Show("آیا میخواهید از برنامه خارج شوید؟", "خروج", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No )return;
            try
            {
                this.Close();
            }
            catch (Exception exception)
            {
                Program.Log(exception, "خروج از برنامه در صفحه لاگین");
            }
        }
        private void mnuProxy_Click(object sender, EventArgs e)
        {
            frmProxy frm = new frmProxy();
            frm.settings = InitSetting(userNumber);
            frm.ShowDialog();
            SaveSetting(userNumber, frm.settings);
        }

        #endregion

        #region textBoxesEvents

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            CheckTextBox();
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            CheckTextBox();
        }

        #endregion

        #region customMethods


        public void CheckResponse(InstaLoginResult result)
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
                frmChallenge frmChallenge = new frmChallenge(_instaApi,userNumber);
                frmChallenge.ShowDialog();
                if(frmChallenge.DialogResult == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                
            }
            else if (result == InstaLoginResult.BadPassword)
            {
                lblMessage.Text = "نام کاربری یا رمز عبور شما اشتباه است";
                btnLogin.Enabled = true;
            }
        }
        #endregion

        #region Utility

        //[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        //private static extern IntPtr CreateRoundRectRgn
        //(
        //    int nLeftRect, // x-coordinate of upper-left corner
        //    int nTopRect, // y-coordinate of upper-left corner
        //    int nRightRect, // x-coordinate of lower-right corner
        //    int nBottomRect, // y-coordinate of lower-right corner
        //    int nWidthEllipse, // height of ellipse
        //    int nHeightEllipse // width of ellipse
        //);

        private void SetProxy()
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
            httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };
        }

        internal static void frmLogin_UIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                //result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
                Exception ex = e.Exception;
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log_unhandled_exception.txt", true))
                {
                    writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now + ") ----------------------------------------------\r\n");
                    writer.WriteLine(ex.ToString() + "\r\n" + "Message: " + "Global Error Handler");
                    writer.WriteLine("\r\n------------------------------------------ (" + "StackTrace" + ") ----------------------------------------------\r\n");
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


        #endregion

        Settings InitSetting(int user)
        {
            switch (user)
            {
                case 0:
                    return Mapper.Map<Settings>(Settings0.Default);
                case 1:
                    return Mapper.Map<Settings>(Settings1.Default);
                case 2:
                    return Mapper.Map<Settings>(Settings2.Default);
                case 3:
                    return Mapper.Map<Settings>(Settings3.Default);
                case 4:
                    return Mapper.Map<Settings>(Settings4.Default);
                default:
                    return new Settings();  
            }
        }
        void SaveSetting(int user,Settings setting)
        {
            switch (user)
            {
                case 0:
                    Settings0.Default = Mapper.Map<Settings0>(setting);
                    Settings0.Default.Save();
                    break;
                case 1:
                    Settings1.Default = Mapper.Map<Settings1>(setting);
                    Settings1.Default.Save();
                    break;
                case 2:
                    Settings2.Default = Mapper.Map<Settings2>(setting);
                    Settings2.Default.Save();
                    break;
                case 3:
                    Settings3.Default = Mapper.Map<Settings3>(setting);
                    Settings3.Default.Save();
                    break;
                case 4:
                    Settings4.Default = Mapper.Map<Settings4>(setting);
                    Settings4.Default.Save();
                    break;
            }

        }
    }
}

// TODO: نمونه کد ستاپ پروکسی

//var proxy = new System.Net.WebProxy()
//{
//    Address = new Uri($"proxyHost:proxyPort"), //i.e: http://1.2.3.4.5:8080
//    BypassProxyOnLocal = false,
//    UseDefaultCredentials = false,

//    // *** These creds are given to the proxy server, not the web server ***
//    Credentials = new NetworkCredential(
//    userName: "proxyUserName",
//    password: "proxyPassword")
//};

//// Now create a client handler which uses that proxy
//var httpClientHandler = new HttpClientHandler()
//{
//    Proxy = proxy,
//};
//bool needServerAuthentication = false;
//// Omit this part if you don't need to authenticate with the web server:
//if (needServerAuthentication)
//{
//httpClientHandler.PreAuthenticate = true;
//httpClientHandler.UseDefaultCredentials = false;

//// *** These creds are given to the web server, not the proxy server ***
//httpClientHandler.Credentials = new NetworkCredential(
//    userName: "serverUserName",
//    password: "serverPassword");
//}
//var InstaApi = InstaApiBuilder.CreateBuilder()
////// Setting up Instagram credentials
////.SetUser(userSession)
//.UseHttpClientHandler(httpClientHandler)
//.Build();