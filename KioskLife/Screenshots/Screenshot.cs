using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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

        public bool DoScreenshot()
        {
            foreach (var screen in Screen.AllScreens)
            {
                try
                {
                    Bitmap screenshot = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
                    Graphics graphics = Graphics.FromImage(screenshot);
                    graphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    if (File.Exists(@$"\Screenshots\Capture.jpg"))
                        File.Delete(@$"\Screenshots\Capture.jpg");
                    screenshot.Save(AppDomain.CurrentDomain.BaseDirectory + @$"\Screenshots\Capture.jpg", ImageFormat.Jpeg);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        screenshot.Save(ms, ImageFormat.Jpeg);
                        CapturedScreen = Convert.ToBase64String(ms.ToArray());
                        SendScreenshot();
                    }
                    return true;
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
                    return false;
                }
                finally
                {
                    //numbers++;
                }
            }
            return false;
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
                    string responseFromServer = "";
                    while (responseFromServer != "OK")
                    {
                        byte[] responseBytes = webClient.UploadValues("https://vr-kiosk.app/tntools/health_terminal.php", "POST", formData);
                        responseFromServer = Encoding.UTF8.GetString(responseBytes);
                        Thread.Sleep(500);
                    }
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
