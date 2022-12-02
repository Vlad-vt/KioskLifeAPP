using Newtonsoft.Json;
using ZebraAPP.Health;

namespace Kiosk_Life.Scanner.Zebra
{
    public class ZebraHealth
    {
        [JsonProperty("APP version")]
        public string AppVersion { get; set; }

        [JsonProperty("App logs")]
        public List<string> AppLogs { get; set; }

        [JsonProperty("Zebra Scanners")]
        public List<ZebraScanner> ZebraScanners;

        public ZebraHealth()
        {
            AppLogs= new List<string>();    
            ZebraScanners= new List<ZebraScanner>();
        }
    }
}
