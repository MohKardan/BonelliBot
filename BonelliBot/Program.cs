using BonelliBot.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BonelliBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)] //unhandled exception
        static void Main()
        {
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(frmLogin.frmLogin_UIThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(frmProxy.frmProxy_UIThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(frmPanelSetting.frmPanelSetting_UIThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(frmChallenge.frmChallenge_UIThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(frmPanel.frmPanel_UIThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmLogin());
            Application.Run(new frmPanel());
        }

        #region handledLogs

        public static bool Log(Exception ex, string message)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log.txt", true))
                {
                    writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now + ") ----------------------------------------------\r\n");
                    writer.WriteLine(ex.ToString() + "\r\n" + "Message: " + message);

                }
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        #endregion

        #region unHandledLogs

        /// <summary>
        /// It defines an event handler, MyHandler, that is invoked whenever an unhandled exception is thrown in the default application domain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter("log_unhandled_exception.txt", true))
            {
                writer.WriteLine("\r\n------------------------------------------ (" + DateTime.Now + ") ----------------------------------------------\r\n");
                writer.WriteLine(e.ToString() + "\r\n" + "Message: " + "Global Error Handler");
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
        // Handle the UI exceptions by showing a dialog box, and asking the user whether
        // or not they wish to abort execution.
        // NOTE: This exception cannot be kept from terminating the application - it can only 
        // log the event, and inform the user about it. 
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                    "with the following information:\n\n";

                // Since we can't prevent the app from terminating, log this to the event log.
                if (!EventLog.SourceExists("ThreadException"))
                {
                    EventLog.CreateEventSource("ThreadException", "Application");
                }

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "ThreadException";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error",
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        #endregion

    }
}
