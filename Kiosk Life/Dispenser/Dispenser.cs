using Kiosk_Life.Information;
using Newtonsoft.Json;

namespace Kiosk_Life.Dispenser
{
    /// <summary>
    /// Class which contain information about health of Dispenser
    /// </summary>
    public class Dispenser : IConsoleInfo
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

        [JsonProperty("DispenserCardLocation")]
        public string CardLocation { get; set; }

        public Dispenser()
        {

        }
        public void ShowAllInfo()
        {
            Console.WriteLine("Dispenser COM: {0}\n CardLocation: {1}\nErrors: {2}\nWarnings: {3}\n", DispenserCOM, CardLocation, DispenserErrors, DispenserWarnings);
        }
    }
}
