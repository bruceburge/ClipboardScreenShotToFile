using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClipboardScreenShotToFile
{
    public static class ScreenShotToBitmap
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        public static Bitmap Screenshot()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            var screenBmp = new Bitmap(screenWidth, screenHeight);
            var g = Graphics.FromImage(screenBmp);

            var dc1 = GetDC(GetDesktopWindow());
            var dc2 = g.GetHdc();

            BitBlt(dc2, 0, 0, screenWidth, screenHeight, dc1, 0, 0, 13369376);

            ReleaseDC(GetDesktopWindow(), dc1);
            g.ReleaseHdc(dc2);
            g.Dispose();

            return screenBmp;
        }
    }
}
