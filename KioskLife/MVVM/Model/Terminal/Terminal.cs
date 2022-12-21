using KioskLife.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace KioskLife.MVVM.Model.Terminal
{
    public class Terminal : Device
    {
        public string Name { get; private set; }
        [JsonIgnore]
        public TcpState ZVTConnection { get; set; }
        public bool ProtocolConnection { get; set; }
        public NetworkDeviceData NetworkData { get; set; }
        [JsonIgnore]
        //public List<TerminalErrors> DeviceErrors { get; set; }
        public List<string> Errors { get; set; }

        public Terminal(string name, List<string> errors) : base(name, errors)
        {

        }
    }
}
