using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BonelliBot.Models;
using BonelliBot.Properties;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;

namespace BonelliBot.Forms
{
    public partial class frmPanelSetting : Form
    {
        IInstaApi _instaApi;
        IResult<InstaCurrentUser> _currentUser;
        bool _isTaskRunning = false;
        public Settings settings;
        public frmPanelSetting(IInstaApi instaApi, IResult<InstaCurrentUser> currentUser)
        {
            InitializeComponent();
            _instaApi = instaApi;
            _currentUser = currentUser;
        }
        
        #region formEvents

        private void frmPanelSetting_Load(object sender, EventArgs e)
        {
            this.Text = $"تنظیمات {_currentUser.Value.UserName}";
            try
            {
                using (BonelliContext db = new BonelliContext())
                {
                    var pk = _currentUser.Value.Pk;
                    List<Target> targets = db.Targets.Where(t => t.CurrentUserPk == pk).OrderBy(t => t.Id).ToList();
                    if (targets.Any())
                    {
                        lbtTargetPage.Items.Clear();
                        foreach (var item in targets)
                        {
                            lbtTargetPage.Items.Add(item.UserName);
                        }
                    }

                    List<WhiteList> whiteLists = db.WhiteLists.Where(t => t.CurrentUserPk == pk).OrderBy(t => t.Id).ToList();
                    if (whiteLists.Any())
                    {
                        lbtWhiteList.Items.Clear();
                        foreach (var item in whiteLists)
                        {
                            lbtWhiteList.Items.Add(item.UserName);
                        }
                        if (!lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                            lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $"{lbtWhiteList.Items.Count}مورد"));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Log(ex, "Error in frmPanelSetting_Load");
            }

            chkPrivateAccount.Checked = settings.privateAccount;
            chkProfilePic.Checked = settings.profilePic;
            chkTargetFollowers.Checked = settings.TargetFollowers;
            chkTargetLikers.Checked = settings.TargetLikers;
            chkFollowPrivateAccounts.Checked = settings.followOnlyPrivate;
            chkUnfollowNonefollowers.Checked = settings.unfollowNoneFollowers;
            chkProxy.Checked = settings.useProxy;
            lblProxyInfo.Text = settings.ipAddress + " : " + settings.port + "\n" + settings.proxyUser + " : " + settings.proxyPassword;
            trbFollowPerHour.Value = settings.followPerHour;
            trbUnfollowPerHour.Value = settings.unFollowPerHour;
            txtMaxFollower.Text = settings.maxFollow.ToString();
            txtMinFollower.Text = settings.minFollow.ToString();
            rtfComment.Text = settings.comment;

        }

        private void frmPanelSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isTaskRunning)
            {
                try
                {
                    settings.followPerHour = trbFollowPerHour.Value;
                    settings.unFollowPerHour = trbUnfollowPerHour.Value;
                    settings.minFollow = int.Parse(txtMinFollower.Text);
                    settings.maxFollow = int.Parse(txtMaxFollower.Text);
                }
                catch(Exception ex)
                {
                    Program.Log(ex, "Error in frmPanelSetting_FormClosing");
                }
                settings.comment = rtfComment.Text;
                settings.Save();
            }
            else
            {
                //RtlMessageBox.Show("لطفا صبر کنید تا عملیات تمام شود");
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                //this.ShowInTaskbar = false;
            }

        }

        #endregion

        #region targetPages

        private async void btnAddToList_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTargetPage.Text.Trim()))
            {
                btnAddToList.Enabled = false;
                try
                {
                    using (BonelliContext db = new BonelliContext())
                    {
                        if (!db.Targets.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == txtTargetPage.Text))
                        {
                            Target target = new Target()
                            {
                                UserName = txtTargetPage.Text,
                                CurrentUserPk = _currentUser.Value.Pk
                            };
                            db.Targets.Add(target);
                            await db.SaveChangesAsync();
                            if (lbtTargetPage.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                                lbtTargetPage.Items.Clear();
                            lbtTargetPage.Items.Add(txtTargetPage.Text);
                            txtTargetPage.Text = "";
                        }
                        else
                            RtlMessageBox.Show("این مورد قبلا به لیست اضافه شده است");
                    }

                }
                catch (Exception exception)
                {

                }
                finally
                {
                    btnAddToList.Enabled = true;
                }
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (BonelliContext db = new BonelliContext())
                {
                    Target target = db.Targets.SingleOrDefault(t => t.UserName == lbtTargetPage.SelectedItem.ToString() && t.CurrentUserPk == _currentUser.Value.Pk);
                    if (target.Id != 0)
                    {
                        db.Targets.Remove(target);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {

            }
            try
            {
                lbtTargetPage.Items.RemoveAt(lbtTargetPage.SelectedIndex);

                if (lbtTargetPage.Items.Count == 0)
                {
                    lbtTargetPage.Items.Add("هیچ آیتمی در لیست وجود ندارد");
                }
            }
            catch (Exception exception)
            {
                RtlMessageBox.Show("لطفا یک مورد را انتخاب کنید");
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            if (!lbtTargetPage.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
            {
                using (BonelliContext db = new BonelliContext())
                {
                    try
                    {
                        foreach (var item in lbtTargetPage.Items)
                        {
                            Target target = db.Targets.SingleOrDefault(t => t.UserName == item.ToString() && t.CurrentUserPk == _currentUser.Value.Pk);
                            if (target.Id != 0)
                            {
                                db.Targets.Remove(target);
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception exception)
                    {

                    }
                }
                lbtTargetPage.Items.Clear();
                lbtTargetPage.Items.Add("هیچ آیتمی در لیست وجود ندارد");
            }
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            if (!lbtTargetPage.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
            {
                using (BonelliContext db = new BonelliContext())
                {
                    foreach (var item in lbtTargetPage.Items)
                    {
                        if (!db.Targets.Any(t => t.UserName == item.ToString()))
                        {
                            Target target = new Target()
                            {
                                UserName = item.ToString()
                            };
                            db.Targets.Add(target);
                        }
                    }

                    db.SaveChanges();
                }

                RtlMessageBox.Show("لیست ذخیره شد");
            }
            else
                RtlMessageBox.Show("لیست خالی است");
        }

        private async void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {

                var test = f.OpenFile();
                string[] lines = File.ReadAllLines(f.FileName);
                if (lines.Count() > 0)
                {
                    if (lbtTargetPage.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                        lbtTargetPage.Items.Clear();

                    using (BonelliContext db = new BonelliContext())
                    {
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (!db.Targets.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == item))
                                {
                                    Target target = new Target()
                                    {
                                        UserName = item,
                                        CurrentUserPk = _currentUser.Value.Pk
                                    };
                                    db.Targets.Add(target);
                                    lbtTargetPage.Items.Add(item);
                                }
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        #endregion

        #region whiteList

        private async void btnAddToWhileList_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWhiteList.Text.Trim()))
            {
                btnAddToWhileList.Enabled = false;
                try
                {
                    using (BonelliContext db = new BonelliContext())
                    {
                        var instaUserWhiteList = await _instaApi.UserProcessor.GetUserAsync(txtWhiteList.Text);
                        if (instaUserWhiteList.Succeeded)
                        {
                            //باید pk یوزر فعلی چک شود
                            if (!db.WhiteLists.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == txtWhiteList.Text))
                            {
                                WhiteList whiteList = new WhiteList()
                                {
                                    UserName = txtWhiteList.Text,
                                    CurrentUserPk = _currentUser.Value.Pk,
                                    Pk = instaUserWhiteList.Value.Pk
                                };
                                db.WhiteLists.Add(whiteList);
                                db.SaveChanges();
                                if (lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                                    lbtWhiteList.Items.Clear();
                                lbtWhiteList.Items.Add(txtWhiteList.Text);
                                txtWhiteList.Text = "";
                                lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $"{lbtWhiteList.Items.Count}مورد"));
                            }
                            else
                                RtlMessageBox.Show("این مورد قبلا به لیست اضافه شده است");
                        }
                        else
                        {
                            MessageBox.Show(instaUserWhiteList.Info.Message);
                        }
                    }
                }
                catch (Exception exception)
                {

                }
                btnAddToWhileList.Enabled = true;
            }
        }

        private async void btnLoadWhiteFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(f.FileName);
                if (lines.Count() > 0)
                {
                    if (lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                        lbtWhiteList.Items.Clear();
                    using (BonelliContext db = new BonelliContext())
                    {
                        var instaUserWhiteList = await _instaApi.UserProcessor.GetUserAsync(_currentUser.Value.UserName);
                        if (instaUserWhiteList.Succeeded)
                        {
                            foreach (var item in lines)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    if (!db.WhiteLists.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == item))
                                    {
                                        WhiteList whiteList = new WhiteList()
                                        {
                                            UserName = item,
                                            CurrentUserPk = _currentUser.Value.Pk,
                                            Pk = instaUserWhiteList.Value.Pk
                                        };
                                        db.WhiteLists.Add(whiteList);
                                        lbtWhiteList.Items.Add(item);
                                        lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $"{lbtWhiteList.Items.Count}مورد"));
                                    }
                                }
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                }
            }

        }

        private void btnCreateWhiteList_Click(object sender, EventArgs e)
        {

            if (lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                lbtWhiteList.Items.Clear();

            Task _createWhiteListTask = new Task(GetFollowing_AddToDatabaseAsync);
            _createWhiteListTask.Start();


            // TODO ساخت لیست سفید و ذخیره در دیتابیس
            async void GetFollowing_AddToDatabaseAsync()
            {
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = "لطفا صبر کنید"));
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = false));
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.BackColor = Color.Gray));

                btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = false));


                BonelliContext db = null;

                try
                {
                    _isTaskRunning = true;
                    db = new BonelliContext();
                    string nextMaxId = null;
                    int followersPerRequest = 0;

                    var current = await _instaApi.UserProcessor.GetUserFollowingAsync(_currentUser.Value.UserName,
                                          PaginationParameters.MaxPagesToLoad(1));

                    if (current.Succeeded)
                    {
                        followersPerRequest = current.Value.Count;
                        btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = $"{followersPerRequest} دریافت شد"));

                        AddToDatabase();
                        nextMaxId = current.Value.NextMaxId;

                        while (nextMaxId != null)
                        {
                            current = null;
                            current = await _instaApi.UserProcessor.GetUserFollowingAsync(_currentUser.Value.UserName,
                                            PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(nextMaxId));
                            if (current.Succeeded)
                            {
                                followersPerRequest = current.Value.Count;
                                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = $"{followersPerRequest} دریافت شد"));

                                AddToDatabase();
                                nextMaxId = current.Value.NextMaxId;
                            }
                        }
                    }


                    async void AddToDatabase()
                    {
                        for (var i = 0; i < current.Value.Count; i++)
                        {
                            string username = current.Value[i].UserName;
                            if (!db.WhiteLists.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == username))
                            {
                                WhiteList whiteList = new WhiteList()
                                {
                                    Pk = current.Value[i].Pk,
                                    UserName = username,
                                    CurrentUserPk = _currentUser.Value.Pk
                                };
                                db.WhiteLists.Add(whiteList);
                                await db.SaveChangesAsync();
                                lbtWhiteList.BeginInvoke((Action)(() => lbtWhiteList.Items.Add(username)));
                                lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $"{lbtWhiteList.Items.Count}مورد"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Log(ex, "ساخت لیست سفید با دریافت لیست فالویینگ های فعلی");
                    this.BeginInvoke((Action)(() => this.Text = "لیست به طور کامل دریافت نشد "));
                }
                finally
                {
                    db?.Dispose();
                    _isTaskRunning = false;
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = "ایجاد لیست سفید با لیست فالویینگ ها "));
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = true));
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.BackColor = Color.FromArgb(65, 106, 180)));
                    btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = true));

                }
            }

        }

        private void btnSaveWhiteList_Click(object sender, EventArgs e)
        {
            if (!lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
            {
                using (BonelliContext db = new BonelliContext())
                {
                    foreach (var item in lbtWhiteList.Items)
                    {
                        if (!db.WhiteLists.Any(t => t.UserName == item.ToString()))
                        {
                            WhiteList whiteList = new WhiteList()
                            {
                                UserName = item.ToString()
                            };
                            db.WhiteLists.Add(whiteList);
                        }
                    }

                    db.SaveChanges();
                }

                RtlMessageBox.Show("لیست ذخیره شد");
            }
            else
                RtlMessageBox.Show("لیست خالی است");
        }

        private void btnRemoveWhiteItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (BonelliContext db = new BonelliContext())
                {
                    WhiteList whiteList = db.WhiteLists.SingleOrDefault(t => t.UserName == lbtWhiteList.SelectedItem.ToString() && t.CurrentUserPk == _currentUser.Value.Pk);
                    if (whiteList.Id != 0)
                    {
                        db.WhiteLists.Remove(whiteList);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.ToString());
            }
            try
            {
                lbtWhiteList.Items.RemoveAt(lbtWhiteList.SelectedIndex);
                if (lbtWhiteList.Items.Count == 0)
                {
                    lbtWhiteList.Items.Add("هیچ آیتمی در لیست وجود ندارد");
                }
                lblWhitelistCount.Text = lbtWhiteList.Items.Count.ToString();
            }
            catch (Exception exception)
            {
                RtlMessageBox.Show("لطفا یک مورد را انتخاب کنید");
            }
        }

        private async void btnClearWhiteList_Click(object sender, EventArgs e)
        {
            btnClearWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = "لطفا صبر کنید"));
            btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = false));
            btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.BackColor = Color.Gray));

            btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = false));


            if (!lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
            {
                BonelliContext db = null;

                Task deleteWhiteListTask = new Task(DeleteWhiteList);
                deleteWhiteListTask.Start();

                async void DeleteWhiteList()
                {
                    try
                    {
                        db = new BonelliContext();
                        _isTaskRunning = true;

                        foreach (var item in lbtWhiteList.Items)
                        {
                            WhiteList whiteList = db.WhiteLists.SingleOrDefault(t => t.UserName == item.ToString() && t.CurrentUserPk == _currentUser.Value.Pk);
                            if (whiteList.Id != 0)
                            {
                                db.WhiteLists.Remove(whiteList);
                            }
                        }
                        await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Program.Log(ex, "پاک کردن لیست سفید");
                    }
                    finally
                    {
                        db?.Dispose();
                        _isTaskRunning = false;
                        btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Text = "پاک کردن کل لیست"));
                        btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = true));
                        btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.BackColor = Color.FromArgb(65, 106, 180)));
                        btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = true));

                        lbtWhiteList.BeginInvoke((Action)(() => lbtWhiteList.Items.Clear()));
                        lbtWhiteList.BeginInvoke((Action)(() => lbtWhiteList.Items.Add("هیچ آیتمی در لیست وجود ندارد")));
                        lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $""));
                    }
                }
            }

        }

        #endregion

        #region checkBoxes

        private void chkProfilePic_CheckedChanged(object sender, EventArgs e)
        {
            settings.profilePic = chkProfilePic.Checked;
            settings.Save();
        }

        private void chkPrivateAccount_CheckedChanged(object sender, EventArgs e)
        {
            settings.privateAccount = chkPrivateAccount.Checked;
            settings.Save();
        }

        private void chkFollowPrivateAccounts_CheckedChanged(object sender, EventArgs e)
        {
            settings.followOnlyPrivate = chkFollowPrivateAccounts.Checked;
            settings.Save();
        }

        private void chkUnfollowNonefollowers_CheckedChanged(object sender, EventArgs e)
        {
            settings.unfollowNoneFollowers = chkUnfollowNonefollowers.Checked;
            settings.Save();
        }



        #endregion

        #region Utility

        internal static void frmPanelSetting_UIThreadException(object sender, ThreadExceptionEventArgs e)
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

        private void txtMaxFollower_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMaxFollower.Text, "[^0-9]"))
            {
                txtMaxFollower.Text = txtMaxFollower.Text.Remove(txtMaxFollower.Text.Length - 1);
            }
            if (txtMaxFollower.Text.StartsWith("0"))
            {
                txtMaxFollower.Text = txtMaxFollower.Text.Remove(0);
            }
        }

        private void txtMinFollower_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMinFollower.Text, "[^0-9]"))
            {
                txtMinFollower.Text = txtMinFollower.Text.Remove(txtMinFollower.Text.Length - 1);
            }
            if (txtMinFollower.Text.StartsWith("0"))
            {
                txtMinFollower.Text = txtMinFollower.Text.Remove(0);
            }
        }

        private void txtMinFollower_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                
            }
          
        }


        private void txtMaxFollower_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;

            }
        }

        private void txtFollowPerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;

            }
        }

        private void txtUnfollowPerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;

            }
        }

        private void btnCreateWhiteList_Click_1(object sender, EventArgs e)
        {
            if (lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
                lbtWhiteList.Items.Clear();

            Task _createWhiteListTask = new Task(GetFollowing_AddToDatabaseAsync);
            _createWhiteListTask.Start();


            // TODO ساخت لیست سفید و ذخیره در دیتابیس
            async void GetFollowing_AddToDatabaseAsync()
            {
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = "لطفا صبر کنید"));
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = false));
                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.BackColor = Color.Gray));

                btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = false));


                BonelliContext db = null;

                try
                {
                    _isTaskRunning = true;
                    db = new BonelliContext();
                    string nextMaxId = null;
                    int followersPerRequest = 0;

                    var current = await _instaApi.UserProcessor.GetUserFollowingAsync(_currentUser.Value.UserName,
                                          PaginationParameters.MaxPagesToLoad(1));

                    if (current.Succeeded)
                    {
                        followersPerRequest = current.Value.Count;
                        btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = $"{followersPerRequest} دریافت شد"));

                        AddToDatabase();
                        nextMaxId = current.Value.NextMaxId;

                        while (nextMaxId != null)
                        {
                            current = null;
                            current = await _instaApi.UserProcessor.GetUserFollowingAsync(_currentUser.Value.UserName,
                                            PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(nextMaxId));
                            if (current.Succeeded)
                            {
                                followersPerRequest = current.Value.Count;
                                btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = $"{followersPerRequest} دریافت شد"));

                                AddToDatabase();
                                nextMaxId = current.Value.NextMaxId;
                            }
                        }
                    }


                    async void AddToDatabase()
                    {
                        for (var i = 0; i < current.Value.Count; i++)
                        {
                            string username = current.Value[i].UserName;
                            if (!db.WhiteLists.Where(t => t.CurrentUserPk == _currentUser.Value.Pk).Any(t => t.UserName == username))
                            {
                                WhiteList whiteList = new WhiteList()
                                {
                                    Pk = current.Value[i].Pk,
                                    UserName = username,
                                    CurrentUserPk = _currentUser.Value.Pk
                                };
                                db.WhiteLists.Add(whiteList);
                                await db.SaveChangesAsync();
                                lbtWhiteList.BeginInvoke((Action)(() => lbtWhiteList.Items.Add(username)));
                                lblWhitelistCount.BeginInvoke((Action)(() => lblWhitelistCount.Text = $"{lbtWhiteList.Items.Count}مورد"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Log(ex, "ساخت لیست سفید با دریافت لیست فالویینگ های فعلی");
                    this.BeginInvoke((Action)(() => this.Text = "لیست به طور کامل دریافت نشد "));
                }
                finally
                {
                    db?.Dispose();
                    _isTaskRunning = false;
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Text = "ایجاد لیست سفید با لیست فالویینگ ها "));
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.Enabled = true));
                    btnCreateWhiteList.BeginInvoke((Action)(() => btnCreateWhiteList.BackColor = Color.FromArgb(65, 106, 180)));
                    btnClearWhiteList.BeginInvoke((Action)(() => btnClearWhiteList.Enabled = true));

                }
            }

        }

        private void chkTargetFollowers_CheckedChanged(object sender, EventArgs e)
        {
            settings.TargetFollowers = chkTargetFollowers.Checked;
            settings.Save();
        }

        private void chkTargetLikers_CheckedChanged(object sender, EventArgs e)
        {
            settings.TargetLikers = chkTargetLikers.Checked;
            settings.Save();
        }

        private void lbtWhiteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clipboard.SetText(((ListBox)sender).SelectedItem.ToString());
        }

        private void btnSaveWhiteListToFile_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        foreach (var item in lbtWhiteList.Items)
                        {
                            sw.WriteLine(item);
                        }
                    }
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "پیام", MessageBoxButtons.OK, MessageBoxIcon.None);

                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void chkProxy_CheckedChanged(object sender, EventArgs e)
        {
            settings.useProxy = chkProxy.Checked;
            settings.Save();
        }

        private void trbFollowPerHour_ValueChanged(object sender, EventArgs e)
        {
            lblFollowPerHour.Text = "تعداد فالو در ساعت: " + trbFollowPerHour.Value;
        }

        private void trbUnfollowPerHour_ValueChanged(object sender, EventArgs e)
        {
            lblUnfollowPerHour.Text = "تعداد آنفالو در ساعت: " + trbUnfollowPerHour.Value;
        }

        private void btnSaveTargetToFile_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        foreach (var item in lbtTargetPage.Items)
                        {
                            sw.WriteLine(item);
                        }
                    }
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "پیام", MessageBoxButtons.OK, MessageBoxIcon.None);

                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }
    }
}

#region openFile

//OpenFileDialog f = new OpenFileDialog();
//if (f.ShowDialog() == DialogResult.OK)
//{
//if (lbtWhiteList.Items.Contains("هیچ آیتمی در لیست وجود ندارد"))
//lbtWhiteList.Items.Clear();

//List<string> lines = new List<string>();
//using (StreamReader r = new StreamReader(f.OpenFile()))
//{
//string line;
//    while ((line = r.ReadLine()) != null)
//{
//    lbtWhiteList.Items.Add(line);
//}
//}
//}

#endregion