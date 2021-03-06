using System.Runtime.InteropServices;

namespace changeBackground
{
    class Program
    {
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 1;
        public const int SPIF_SENDCHANGE = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        
        static void Main(string[] args)
        {
            //@"C:\Users\Public\Pictures\Sample Pictures\blackdot.bmp"
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, args[0], SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }
}
