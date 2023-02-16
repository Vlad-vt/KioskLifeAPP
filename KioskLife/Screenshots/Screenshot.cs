using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace KioskLife.Screenshots
{
    internal sealed class Screenshot
    {
        public string CapturedScreen { get; private set; }
       // private int numbers { get; set; }

        public Screenshot()
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\");
                else
                {
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\", true);
                    System.Threading.Thread.Sleep(2000);
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Screenshots\");
                }
            }
            catch (Exception e)
            {

            }
            //numbers = 1;
        }

        public void DoScreenshot()
        {
            try
            {
                Bitmap captureBitmap = new Bitmap(1080, 1920, PixelFormat.Format32bppArgb);
                Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                if (File.Exists(@$"\Screenshots\Capture.jpg"))
                    File.Delete(@$"\Screenshots\Capture.jpg");
                captureBitmap.Save(AppDomain.CurrentDomain.BaseDirectory + @$"\Screenshots\Capture.jpg", ImageFormat.Jpeg);
                using (MemoryStream ms = new MemoryStream())
                {
                    captureBitmap.Save(ms, ImageFormat.Jpeg);
                    CapturedScreen = Convert.ToBase64String(ms.ToArray());
                    SendScreenshot();
                }
            }
            catch (Exception e)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\");
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt"))
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt",
                        $"[{DateTime.Now}]: {e.Message}\n");
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt",
                        $"[{DateTime.Now}]: {e.Message}\n");
            }
            finally
            {
                //numbers++;
            }
        }

        private void SendScreenshot()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    System.Diagnostics.Trace.WriteLine("DATA SEND\n" + Environment.MachineName + "\n" + "screenshot");
                    NameValueCollection formData = new NameValueCollection();
                    formData["MachineName"] = Environment.MachineName;
                    formData["Screenshot"] = CapturedScreen;
                    byte[] responseBytes = webClient.UploadValues("https://vr-kiosk.app/kiosklife/api.php", "POST", formData);
                    string responsefromserver = Encoding.UTF8.GetString(responseBytes);
                    webClient.Dispose();
                }
            }
            catch (Exception e)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\");
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt"))
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt",
                        $"[{DateTime.Now}]: {e.Message}");
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Screenshots\log.txt",
                        $"[{DateTime.Now}]: {e.Message}");
            }
        }
    }
}
