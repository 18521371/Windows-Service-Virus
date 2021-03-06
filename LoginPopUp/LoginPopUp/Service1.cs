using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace LoginPopUp
{
    public partial class Service1 : ServiceBase
    {
        // Khai báo các thông tin biến và import các thư viện cho 
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WTSGetActiveConsoleSessionID();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
                IntPtr hServer,
                [MarshalAs(UnmanagedType.I4)] int SessionId,
                String pTitle,
                [MarshalAs(UnmanagedType.U4)] int TitleLength,
                String pMessage,
                [MarshalAs(UnmanagedType.U4)] int MessageLength,
                [MarshalAs(UnmanagedType.U4)] int Style,
                [MarshalAs(UnmanagedType.U4)] int Timeout,
                [MarshalAs(UnmanagedType.U4)] out int pResponse,
                bool bWait);
        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public static int WTS_CURRENT_SESSION = 1;
        // Hàm thực hiện show popup, tham số truyền vào là chuỗi cần hiện ra
        public void Start(string s)
        {
            try
            {
                bool result = false;
                String title = "MSSV";
                int tlen = title.Length;
                string msg = s;
                int mlen = msg.Length;
                int resp = 0;
                //result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, user_session, title, tlen, msg, mlen, 4,
                //    0, out resp, true);
                result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, 1, title, tlen, msg, mlen, 0, 0, out resp, true);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("no such thread exists", ex);
            }
        }

        //
        System.Timers.Timer timer = new System.Timers.Timer(); // name space(using System.Timers;) 
        public Service1()
        {
            // Thiết lập các quyền để dịch vụ sử dụng
            InitializeComponent();
            this.CanHandleSessionChangeEvent = true; //Điều kiện có thể handle SessionChange (nên có)
            this.CanHandlePowerEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.AutoLog = true;
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            Thread.Sleep(3000);
 
            if (changeDescription.Reason == SessionChangeReason.SessionLock ||
                changeDescription.Reason == SessionChangeReason.SessionLogoff ||
                changeDescription.Reason == SessionChangeReason.ConsoleDisconnect)
            {
                //Start("18521371");
            }
            else if (changeDescription.Reason == SessionChangeReason.SessionUnlock) {

                Start("18521371");
            }
            base.OnSessionChange(changeDescription);
        }
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds 
            timer.Enabled = true;

            SessionChangeDescription sschange = new SessionChangeDescription();
            OnSessionChange(sschange);
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory +
           "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') +
           ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}