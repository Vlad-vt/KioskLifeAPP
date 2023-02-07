using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace KioskLife.Screenshots
{
    internal sealed class Screenshot
    {
        public string CapturedScreen { get; }
        private int numbers { get; set; }

        public Screenshot()
        {
            if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\");
            numbers = 1;
        }

        public void DoScreenshot()
        {
            try
            {
                Bitmap captureBitmap = new Bitmap(1080, 1920, PixelFormat.Format32bppArgb);
                Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                captureBitmap.Save(AppDomain.CurrentDomain.BaseDirectory + @$"\Screenshots\Capture{numbers}.jpg", ImageFormat.Jpeg);
            }
            catch (Exception e)
            { 

            }
            finally
            {
                numbers++;
            }
        }
    }
}
