using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using BonelliBot;
using BonelliBot.Models;
using InstagramApiSharp.Classes.SessionHandlers;
using InstagramApiSharp.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstagramApiSharp.Classes;
using System.IO;

namespace BonelliBot.Forms
{
    public partial class frmChallenge : Form
    {
        // Note: the old challenge require function is not supported anymore.
        // Note: new challenge require functions is very easy to use.
        // there are 5 functions I've added to IInstaApi for challenge require (checkpoint_endpoint)

        // here:
        // 1. Task<IResult<ChallengeRequireVerifyMethod>> GetChallengeRequireVerifyMethodAsync();
        // If your login needs challenge, first you should call this function.
        // Note: if you call this and SubmitPhoneRequired was true, you should sumbit phone number
        // with this function:
        // Task<IResult<ChallengeRequireSMSVerify>> SubmitPhoneNumberForChallengeRequireAsync();


        // 2. Task<IResult<ChallengeRequireSMSVerify>> RequestVerifyCodeToSMSForChallengeRequireAsync();
        // This function will send you verification code via SMS.


        // 3. Task<IResult<ChallengeRequireEmailVerify>> RequestVerifyCodeToEmailForChallengeRequireAsync();
        // This function will send you verification code via Email.


        // 4. Task<IResult<ChallengeRequireVerifyMethod>> ResetChallengeRequireVerifyMethodAsync();
        // Reset challenge require.
        // Example: if your account has phone number and email, and you request for email(or phone number)
        // and now you want to change it to another one, you should first call this function,
        // then you have to call GetChallengeRequireVerifyMethodAsync and after that you can change your method!!!


        // 5. Task<IResult<ChallengeRequireVerifyCode>> VerifyCodeForChallengeRequireAsync(string verifyCode);
        // Verify sms or email verification code for login.
        string StateFile;
        const string AppName = "Challenge Required";
        readonly Size NormalSize = new Size(432, 164);
        readonly Size ChallengeSize = new Size(432, 604);
        private static IInstaApi InstaApi;

        public frmChallenge(IInstaApi _instaApi,int userNumber)
        {
            InitializeComponent();
            Load += frmChallenge_Load;
            InstaApi = _instaApi;
            StateFile = $"state{userNumber}.bin";
        }
        public frmChallenge()
        {
            InitializeComponent();
        }
        private async void frmChallenge_Load(object sender, EventArgs e)
        {
            Size = NormalSize;
            var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();
            if (challenge.Succeeded)
            {
                if (challenge.Value.SubmitPhoneRequired)
                {
                    SubmitPhoneChallengeGroup.Visible = true;
                    Size = ChallengeSize;
                }
                else
                {
                    if (challenge.Value.StepData != null)
                    {
                        if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                        {
                            RadioVerifyWithPhoneNumber.Checked = false;
                            RadioVerifyWithPhoneNumber.Visible = true;
                            RadioVerifyWithPhoneNumber.Text = challenge.Value.StepData.PhoneNumber;
                        }
                        if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                        {
                            RadioVerifyWithEmail.Checked = false;
                            RadioVerifyWithEmail.Visible = true;
                            RadioVerifyWithEmail.Text = challenge.Value.StepData.Email;
                        }

                        SelectMethodGroupBox.Visible = true;
                        Size = ChallengeSize;
                    }
                }
            }
            else
                MessageBox.Show(challenge.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void SubmitPhoneChallengeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSubmitPhoneForChallenge.Text) ||
                    string.IsNullOrWhiteSpace(txtSubmitPhoneForChallenge.Text))
                {
                    MessageBox.Show("Please type a valid phone number(with country code).\r\ni.e: +989123456789", "ERR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var phoneNumber = txtSubmitPhoneForChallenge.Text;
                if (!phoneNumber.StartsWith("+"))
                    phoneNumber = $"+{phoneNumber}";

                var submitPhone = await InstaApi.SubmitPhoneNumberForChallengeRequireAsync(phoneNumber);
                if (submitPhone.Succeeded)
                {
                    VerifyCodeGroupBox.Visible = true;
                    SubmitPhoneChallengeGroup.Visible = false;
                }
                else
                    MessageBox.Show(submitPhone.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.Log(ex, "تو چلنج یه مشکی به وجود امده");
            }
        }
        private async void SendCodeButton_Click(object sender, EventArgs e)
        {
            bool isEmail = false;
            if (RadioVerifyWithEmail.Checked)
                isEmail = true;
            //if (RadioVerifyWithPhoneNumber.Checked)
            //    isEmail = false;

            try
            {
                // Note: every request to this endpoint is limited to 60 seconds                 
                if (isEmail)
                {
                    // send verification code to email
                    var email = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                    if (email.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verify code to this email:\n{email.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                        MessageBox.Show(email.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // send verification code to phone number
                    var phoneNumber = await InstaApi.RequestVerifyCodeToSMSForChallengeRequireAsync();
                    if (phoneNumber.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verify code to this phone number(it's end with this):\n{phoneNumber.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                        MessageBox.Show(phoneNumber.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.Log(ex, "چلنج ");
            }

        }
        private async void ResendButton_Click(object sender, EventArgs e)
        {
            bool isEmail = false;
            if (RadioVerifyWithEmail.Checked)
                isEmail = true;

            try
            {
                // Note: every request to this endpoint is limited to 60 seconds                 
                if (isEmail)
                {
                    // send verification code to email
                    var email = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync(replayChallenge: true);
                    if (email.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verification code one more time\r\nto this email:\n{email.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                        MessageBox.Show(email.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // send verification code to phone number
                    var phoneNumber =
                        await InstaApi.RequestVerifyCodeToSMSForChallengeRequireAsync(replayChallenge: true);
                    if (phoneNumber.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verification code one more time\r\nto this phone number(it's end with this):{phoneNumber.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                        MessageBox.Show(phoneNumber.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.Log(ex, "چلنج");
            }
        }
        // فرم پنل اینجا باز میشود
        private async void VerifyButton_Click(object sender, EventArgs e)
        {
            VerifyButton.Enabled = false;
            txtVerifyCode.Text = txtVerifyCode.Text.Trim();
            txtVerifyCode.Text = txtVerifyCode.Text.Replace(" ", "");
            var regex = new Regex(@"^-*[0-9,\.]+$");
            if (!regex.IsMatch(txtVerifyCode.Text))
            {
                RtlMessageBox.Show("کد وارد شده باید عددی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtVerifyCode.Text.Length != 6)
            {
                RtlMessageBox.Show("کد باید شش رقمی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Note: calling VerifyCodeForChallengeRequireAsync function, 
                // if user has two factor enabled, will wait 15 seconds and it will try to
                // call LoginAsync.

                var verifyLogin = await InstaApi.VerifyCodeForChallengeRequireAsync(txtVerifyCode.Text);
                if (verifyLogin.Succeeded)
                {
                    // you are logged in sucessfully.

                    //VerifyCodeGroupBox.Visible = SelectMethodGroupBox.Visible = false;
                    //Size = ChallengeSize;
                    //GetFeedButton.Visible = true;

                    // Save session
                    SaveSession();

                    Text = $"{AppName} لاگین با موفقیت انجام شد.";

                    //this.Hide();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    //frmPanel frm = new frmPanel(InstaApi);
                    //frm.Show();
                }
                else
                {
                    VerifyButton.Enabled = true;
                    VerifyCodeGroupBox.Visible = SelectMethodGroupBox.Visible = false;
                    // two factor is required
                    if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        TwoFactorGroupBox.Visible = true;
                    }
                    else
                        MessageBox.Show(verifyLogin.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.Log(ex, "چلنج");
            }

        }
        private async void TwoFactorButton_Click(object sender, EventArgs e)
        {
            if (InstaApi == null)
                return;
            if (string.IsNullOrEmpty(txtTwoFactorCode.Text))
            {
                MessageBox.Show("Please type your two factor code and then press Auth button.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // send two factor code
            var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(txtTwoFactorCode.Text);
            Debug.WriteLine(twoFactorLogin.Value);
            if (twoFactorLogin.Succeeded)
            {
                // connected
                // save session
                SaveSession();
                Size = ChallengeSize;
                TwoFactorGroupBox.Visible = false;
                GetFeedButton.Visible = true;
                Text = $"{AppName} Connected";
                Size = NormalSize;
            }
            else
            {
                MessageBox.Show(twoFactorLogin.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadSession()
        {
            InstaApi?.SessionHandler?.Load();

            //// Old load session
            //try
            //{
            //    if (File.Exists(StateFile))
            //    {
            //        Debug.WriteLine("Loading state from file");
            //        using (var fs = File.OpenRead(StateFile))
            //        {
            //            InstaApi.LoadStateDataFromStream(fs);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }
        void SaveSession()
        {
            //if (InstaApi == null)
            //    return;
            //if (!InstaApi.IsUserAuthenticated)
            //    return;
            //InstaApi.SessionHandler.Save();

            // Old save session 
            var state = InstaApi.GetStateDataAsStream();
            using (var fileStream = File.Create(StateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }

        #region Utility
            
        internal static void frmChallenge_UIThreadException(object sender, ThreadExceptionEventArgs e)
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

        #region Samples

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            Size = NormalSize;
            var userSession = new UserSessionData
            {
                UserName = txtUser.Text,
                Password = txtPass.Text
            };

            InstaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                // Session handler, set a file path to save/load your state/session data
                .SetSessionHandler(new FileSessionHandler() { FilePath = StateFile })
                .Build();
            Text = $"{AppName} Connecting";
            //Load session
            LoadSession();
            if (!InstaApi.IsUserAuthenticated)
            {
                var logInResult = await InstaApi.LoginAsync();
                Debug.WriteLine(logInResult.Value);
                if (logInResult.Succeeded)
                {
                    GetFeedButton.Visible = true;
                    Text = $"{AppName} Connected";
                    // Save session 
                    SaveSession();
                }
                else
                {
                    if (logInResult.Value == InstaLoginResult.ChallengeRequired)
                    {
                        var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();
                        if (challenge.Succeeded)
                        {
                            if (challenge.Value.SubmitPhoneRequired)
                            {
                                SubmitPhoneChallengeGroup.Visible = true;
                                Size = ChallengeSize;
                            }
                            else
                            {
                                if (challenge.Value.StepData != null)
                                {
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                    {
                                        RadioVerifyWithPhoneNumber.Checked = false;
                                        RadioVerifyWithPhoneNumber.Visible = true;
                                        RadioVerifyWithPhoneNumber.Text = challenge.Value.StepData.PhoneNumber;
                                    }
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                                    {
                                        RadioVerifyWithEmail.Checked = false;
                                        RadioVerifyWithEmail.Visible = true;
                                        RadioVerifyWithEmail.Text = challenge.Value.StepData.Email;
                                    }

                                    SelectMethodGroupBox.Visible = true;
                                    Size = ChallengeSize;
                                }
                            }
                        }
                        else
                            MessageBox.Show(challenge.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        TwoFactorGroupBox.Visible = true;
                        Size = ChallengeSize;
                    }
                }
            }
            else
            {
                Text = $"{AppName} Connected";
                GetFeedButton.Visible = true;
            }
        }
        private async void GetFeedButton_Click(object sender, EventArgs e)
        {
            if (InstaApi == null)
            {
                MessageBox.Show("Login first.");
                return;
            }
            if (!InstaApi.IsUserAuthenticated)
            {
                MessageBox.Show("Login first.");
                return;
            }
            var x = await InstaApi.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

            if (x.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                sb2.AppendLine("Like 5 Media>");
                foreach (var item in x.Value.Medias.Take(5))
                {
                    // like media...
                    var liked = await InstaApi.MediaProcessor.LikeMediaAsync(item.InstaIdentifier);
                    sb2.AppendLine($"{item.InstaIdentifier} liked? {liked.Succeeded}");
                }

                sb.AppendLine("Explore Feeds Result: " + x.Succeeded);
                foreach (var media in x.Value.Medias)
                {
                    sb.AppendLine(DebugUtils.PrintMedia("Feed media", media));
                }
                RtBox.Text = sb2.ToString() + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                RtBox.Text += sb.ToString();
                RtBox.Visible = true;
                Size = ChallengeSize;
            }
        }

        #endregion
    }
    public static class DebugUtils
    {
        public static string PrintMedia(string header, InstagramApiSharp.Classes.Models.InstaMedia media)
        {
            var content = $"{header}: {media.Caption?.Text.Truncate(30)}, {media.Code}";
            Debug.WriteLine(content);
            return content;
        }
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
