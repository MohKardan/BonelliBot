using BonelliBot.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Properties;

namespace BonelliBot.Forms
{
    public partial class frmProxy : Form
    {
        public Settings settings;

        public frmProxy()
        {
            InitializeComponent();
        }

        #region formEvents

        private void frmProxy_Load(object sender, EventArgs e)
        {
            txtIpAddress.Text = settings.ipAddress;
            txtPort.Text = settings.port;
            txtUsername.Text = settings.proxyUser;
            txtPassword.Text = settings.proxyPassword;
            chkUseProxy.Checked = settings.useProxy;
            cboProxyProtocol.Text = settings.proxyProtocol;

            CheckTextBox();
        }

        #endregion

        #region clickEvents
        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnSave.BackColor = Color.Gray;
            btnSave.Text = "لطفا صبر کنید";

            if (await TestProxyAsync())
            {
                settings.ipAddress = txtIpAddress.Text;
                settings.port = txtPort.Text;
                settings.proxyUser = txtUsername.Text;
                settings.proxyPassword = txtPassword.Text;
                settings.useProxy = chkUseProxy.Checked;
                settings.proxyProtocol = cboProxyProtocol.Text;
                settings.Save();
                RtlMessageBox.Show("پروکسی ذخیره شد");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                RtlMessageBox.Show("پروکسی وارد شده صحیح نیست");
                chkUseProxy.Checked = false;
                settings.useProxy = chkUseProxy.Checked;
                this.DialogResult = DialogResult.OK;
            }

            btnSave.Enabled = true;
            btnSave.BackColor = Color.FromArgb(48, 79, 107);
            btnSave.Text = "ذخیره";

        }

        #endregion

        #region Utility

        public bool TestProxy()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.instagram.com/");
                WebProxy myproxy = new WebProxy(txtIpAddress.Text, int.Parse(txtPort.Text));
                myproxy.BypassProxyOnLocal = false;
                myproxy.Credentials = new NetworkCredential(
                    userName: txtUsername.Text,
                    password: txtPassword.Text);
                request.Proxy = myproxy;
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                request.Timeout = 6000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string Content = sr.ReadToEnd();
                    if (!Content.Contains("instagram.com"))
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        async Task<bool> TestProxyAsync()
        {

            Task<bool> task = new Task<bool>(TestProxy);
            task.Start();

            return await task;
        }

        internal static void frmProxy_UIThreadException(object sender, ThreadExceptionEventArgs e)
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

        #region textBoxesEvents
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPort.Text, "[^0-9]"))
            {
                txtPort.Text = txtPort.Text.Remove(txtPort.Text.Length - 1);
            }
            CheckTextBox();
        }

        private void txtIpAddress_TextChanged(object sender, EventArgs e)
        {
            CheckTextBox();
        }

        #endregion

        #region customMethods
        public void CheckTextBox()
        {
            if (!string.IsNullOrEmpty(txtIpAddress.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {
                btnSave.Enabled = true;
                btnSave.BackColor = Color.FromArgb(48, 79, 107);
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.Gray;
            }


        }
        #endregion

    }
}
