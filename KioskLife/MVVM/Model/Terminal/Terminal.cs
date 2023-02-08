using KioskLife.Enums;
using KioskLife.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace KioskLife.MVVM.Model.Terminal
{
    public class Terminal : Device
    {
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

        public Terminal(string name, List<string> errors, string isOnline, DeviceType deviceType) : base(name, errors, isOnline, deviceType)
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
            Name = "Payment Terminal FEIG";
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
                    IsChanges = true;
                    if (status)
                    {
                        IsOnline = "Online";
                        Errors = "-";
                    }
                    else
                    {
                        IsOnline = "Offline";
                        Errors = "Terminal Offline";
                    }
                    CheckStatus();
                }
            }
        }

        public void CheckForErrors()
        {
            if (!NetworkData.ConnectedToNetwork)
                return;
            DeviceErrors.Clear();
            if (NetworkData.IP == "null")
            {
                DeviceErrors.Add(TerminalErrors.TerminalOffline);
                Errors += DeviceErrors[0].ToString();
                if (IsOnline == "Offline")
                    IsChanges = false;
                else
                    IsChanges = true;
                IsOnline = "Offline";
                CheckStatus();
                if (IsChanges)
                {
                    AddAction($"{Name} is offline now!");
                    SendJSON();
                }
                return;
            }
            TcpState state = TcpState.Unknown;
            Network.Network.GetInstance().GetTcpConnection(NetworkData.IP, ref state);
            ZVTConnection = state;
            ProtocolConnection = true;
            DeviceErrors.Clear();
            Errors = "-";
            if (IsChanges)
            {
                AddAction($"{Name} is online now!");
                SendJSON();
            }
            if (IsOnline == "Online")
                IsChanges = false;
            else
                IsChanges = true;
            if (ZVTConnection != TcpState.Established)
            {
                //DeviceErrors.Add(TerminalErrors.TerminalNotConnectedToZVT);
                //Errors.Add(DeviceErrors[0].ToString());
                return;
            }
        }

        public void ShowChanges()
        {
            AddAction($"{Name} terminal started working!");
            SendJSON();
            WriteJSON();
        }

        public void AddEvent(string events)
        {
            CheckStatus();
            WriteJSON();
            AddAction($"{events}");
        }

        protected override void SendJSON()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection formData = new NameValueCollection();
                    formData["MachineName"] = MachineName;
                    formData["Type"] = DeviceType.ToString();
                    formData["Name"] = Name;
                    formData["IP"] = NetworkData.IP;
                    formData["MacAddress"] = NetworkData.MacAddress;
                    formData["ManufactoryName"] = NetworkData.ManufactoryName;
                    formData["ConnectedToNetwork"] = NetworkData.ConnectedToNetwork.ToString();
                    formData["Errors"] = Errors;
                    formData["Status"] = IsOnline;
                    byte[] responseBytes = webClient.UploadValues("https://vr-kiosk.app/tntools/health_terminal.php", "POST", formData);
                    string responsefromserver = Encoding.UTF8.GetString(responseBytes);
                    webClient.Dispose();
                }
            }
            catch(Exception e)
            {
                try
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\");
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt"))
                    {
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt",
                            $"[{DateTime.Now}]: {e.Message}");
                    }
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt",
                            $"[{DateTime.Now}]: {e.Message}");
                }
                catch (IOException)
                {

                }
            }
        }
    }
}
