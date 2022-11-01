using Newtonsoft.Json;

namespace Kiosk_Life.Dispenser
{
    /// <summary>
    /// Class which contain information about health of Dispenser
    /// </summary>
    public class Dispenser
    {
        /// <summary>
        /// Dispenser COM name (COM1, COM2 , COM3 ...)
        /// </summary>
        [JsonProperty("DispenserCOM")]
        public string DispenserCOM { get; set; }

        /// <summary>
        /// Dispenser Errors (Not connected, Box empty ...)
        /// </summary>
        [JsonProperty("DispenserErrors")]
        public List<string> DispenserErrors { get; set; }

        [JsonProperty("DispenserWarnings")]
        public List<string> DispenserWarnings { get; set; }

        public Dispenser()
        {

        }
    }
}
