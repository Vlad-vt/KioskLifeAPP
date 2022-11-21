using Newtonsoft.Json;

namespace Kiosk_Life.Scanner.Zebra
{
    public class ZebraScannerHealth
    {
        [JsonProperty("ZebraScanners")]
        public List<ZebraScanner> ZebraScanners { get; set; }

        public ZebraScannerHealth()
        {
            ZebraScanners = new List<ZebraScanner>();
        }
    }
}
