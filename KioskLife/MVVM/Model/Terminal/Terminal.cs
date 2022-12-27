using KioskLife.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace KioskLife.MVVM.Model.Terminal
{
    public class Terminal : Device
    {
        private string _terminalName;
        public string TerminalName 
        { 
            get
            {
                return _terminalName;
            }
            private set
            {
                _terminalName = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public TcpState ZVTConnection { get; set; }
        public bool ProtocolConnection { get; set; }

        private NetworkDeviceData _networkData;
        public NetworkDeviceData NetworkData 
        { 
            get
            {
                return _networkData;
            }
            set
            {
                _networkData = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public List<TerminalErrors> DeviceErrors { get; set; }

        private string _errors;
        public string Errors 
        { 
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        public Terminal(string name, List<string> errors, string isOnline) : base(name, errors, isOnline)
        {
            if (errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; i++)
                {
                    if (i == 0)
                        Errors += errors[i];
                    else
                        Errors += "," + errors[i];
                }
            }
            TerminalName = "Payment Terminal FEIG";
            DeviceErrors = new List<TerminalErrors>();
            Network.Network.GetInstance().networkConnection += NetworkConnection;
            ShowChanges();
        }

        public void GetNetworkData()
        {
            NetworkData = Network.Network.GetInstance().PingAll("FEIG");
            AddAction("Terminal succesfuly initiated!");
        }

        public void GetTerminalError()
        {
            switch (DeviceErrors[0])
            {
                case TerminalErrors.TerminalOffline:
                    Console.WriteLine("Terminal offline now");
                    IsOnline = "Offline";
                    return;
                case TerminalErrors.TerminalNotConnectedToZVT:
                    Console.WriteLine("Terminal online but not connected to ZVT protocol");
                    IsOnline = "Offline";
                    return;
                default:
                    Console.WriteLine("Terminal online and connected to ZVT protocol");
                    IsOnline = "Online";
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
                    NetworkData = new NetworkDeviceData(NetworkData, status);
                }
            }
        }

        public void CheckForErrors()
        {
            DeviceErrors.Clear();
            Errors = "";
            if (NetworkData.IP == "null")
            {
                DeviceErrors.Add(TerminalErrors.TerminalOffline);
                Errors += DeviceErrors[0].ToString();
                IsOnline = "Offline";
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
            Errors = "-";
        }

        public void ShowChanges()
        {
            AddAction($"{Name} terminal started working!");
        }

        public void AddEvent(string events)
        {
            CheckStatus();
            AddAction($"{events}");
        }
    }
}
