using Kiosk_Life.Network;
using Kiosk_Life.Information;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace Kiosk_Life.Terminal
{
    public class Terminal : IConsoleInfo, IErrorsCheck<string, TerminalErrors>, INetworkCheck
    {
        public string Name { get; private set; }
        [JsonIgnore]
        public TcpState ZVTConnection { get; set; }
        public bool ProtocolConnection { get; set; }
        public NetworkDevicedata NetworkData { get; set; }
        [JsonIgnore]
        public List<TerminalErrors> DeviceErrors { get; set; }
        public List<string> Errors { get; set; }

        public Terminal()
        {
            Name = "NULL";
            NetworkData = new NetworkDevicedata { IP = "NULL", MacAddress = "NULL", ManufactoryName = "NULL"};
            DeviceErrors = new List<TerminalErrors>();
            Errors = new List<string>();
        }

        public Terminal(bool initialization)
        {
            Name = "Payment Terminal FEIG";
            DeviceErrors = new List<TerminalErrors>();
            Errors = new List<string>();
            NetworkData = Network.Network.GetInstance().PingAll("FEIG");
            Network.Network.GetInstance().networkConnection += NetworkConnection;
        }

        public void ShowAllInfo()
        {
            Console.WriteLine("Terminal Name:     " + Name
                   + "\n" + "Terminal IP: " + NetworkData.IP
                   + "\n" + "Terminal MacAddress: " + NetworkData.MacAddress
                   + "\n" + "Terminal ManufactoryName: " + NetworkData.ManufactoryName);
        }

        public void GetTerminalError()
        {
            switch(DeviceErrors[0])
            {
                case TerminalErrors.TerminalOffline:
                    Console.WriteLine("Terminal offline now");
                    return;
                case TerminalErrors.TerminalNotConnectedToZVT:
                    Console.WriteLine("Terminal online but not connected to ZVT protocol");
                    return;
                default:
                    Console.WriteLine("Terminal online and connected to ZVT protocol");
                    return;
            }
        }

        public void CheckDeviceConnection()
        {
            Network.Network.GetInstance().PingDevice(NetworkData.IP);
        }

        public void NetworkConnection(string ip, bool status)
        {
            if (ip == NetworkData.IP)
            {
                if (NetworkData.ConnectedToNetwork != status)
                {
                    NetworkData = new NetworkDevicedata(NetworkData, status);
                }
            }
        }

        public void CheckForErrors(string device)
        {
            DeviceErrors.Clear();
            Errors.Clear();
            if (NetworkData.IP == "null")
            {
                DeviceErrors.Add(TerminalErrors.TerminalOffline);
                Errors.Add(DeviceErrors[0].ToString());
                return;
            }
            TcpState state = TcpState.Unknown;
            Network.Network.GetInstance().GetTcpConnection(NetworkData.IP, ref state);
            ZVTConnection = state;
            ProtocolConnection = true;
            if (ZVTConnection != TcpState.Established)
            {
                //DeviceErrors.Add(TerminalErrors.TerminalNotConnectedToZVT);
                //Errors.Add(DeviceErrors[0].ToString());
                return;
            }
            DeviceErrors.Clear();
            Errors.Clear();
        }


    }
}
