using Newtonsoft.Json;
using Kiosk_Life.Printer;
using Kiosk_Life.Scanner;
using System.Net;

namespace Kiosk_Life.Kiosk
{
    public class KioskConfiguration
    {
        [JsonIgnore]
        private static KioskConfiguration _singleton;

        [JsonProperty("Printers")]
        public List<Printer.Printer> _printers;
        [JsonProperty("Terminal")]
        public Terminal.Terminal _terminal;
        [JsonProperty("Camera")]
        public Camera.Camera _camera;

        public static KioskConfiguration GetInstance()
        {
            if (_singleton == null)
                _singleton = new KioskConfiguration();
            return _singleton;
        }

        public KioskConfiguration()
        {
            _printers = new List<Printer.Printer>();
            _terminal = new Terminal.Terminal();
            _camera = new Camera.Camera();
        }

        public void SendFileToBrowser(ref HttpListenerContext response)
        {
            string json = "";
            json = JsonConvert.SerializeObject(this, Formatting.Indented);
            Console.WriteLine(json);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
            response.Response.ContentLength64 = buffer.Length;
            response.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        public void TestSend()
        {
            string json = "";
            json = JsonConvert.SerializeObject(this, Formatting.Indented);
            Console.WriteLine(json);
        }

    }
}
