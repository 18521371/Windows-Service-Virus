using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LoginPopUp
{
    class Notifyy
    {
        // Show pop-up with student ID
        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WTSGetActiveConsoleSessionID();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        // public static int WTS_CURRENT_SESSION = 3;
        // If using WTGetActiveConsoleSessionID() didn't work, try WTS_CURRENT_SESSION.
        // Using tasklist to check your current session id

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

        public void Start()
        {
            bool result = false;
            string title = "Hello";
            int tlen = title.Length;
            string msg = "StudentID: 18520084!";
            int mlen = msg.Length;
            int resp = 0;

            result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, WTSGetActiveConsoleSessionID(), title, tlen, msg, mlen, 0, 0, out resp, true);
            //int err = Marshal.GetLastWin32Error();
            //System.Console.WriteLine("result:{0}, errorCode:{1}, response:{2}", result, err, resp);
        }
    }


}
