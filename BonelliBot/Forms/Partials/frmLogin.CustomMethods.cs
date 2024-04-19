using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BonelliBot.Forms
{
    partial class frmLogin
    {
        public void InitForm()
        {
            CheckTextBox();
            settings = InitSetting(userNumber);
        }

        public void CheckTextBox()
        {
            if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass.Text))
            {
                btnLogin.Enabled = true;
                btnLogin.BackColor = Color.FromArgb(48, 79, 107);
            }
            else
            {
                btnLogin.Enabled = false;
                btnLogin.BackColor = Color.Gray;
            }


        }

    }
}
