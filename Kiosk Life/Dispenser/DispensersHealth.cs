using Newtonsoft.Json;

namespace Kiosk_Life.Dispenser
{
    public class DispensersHealth
    {
        [JsonProperty("Dispensers")]
        public List<Dispenser> Dispensers { get; set; }

        public DispensersHealth()
        {
            Dispensers = new List<Dispenser>();
        }
    }
}
