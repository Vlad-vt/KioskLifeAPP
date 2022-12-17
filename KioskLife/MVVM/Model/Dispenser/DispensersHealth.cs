using Newtonsoft.Json;
using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Dispenser
{
    public class DispensersHealth
    {
        [JsonProperty("Dispensers")]
        public List<Dispenser> Dispensers { get; set; }

        public DispensersHealth()
        {

        }
    }
}
