using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace KioskLife.Network
{
    public sealed class Network
    {
        private static Network _singleton;

        public event NetworkConnection networkConnection;

        public delegate void NetworkConnection(string ip, bool status);

        private NetworkDeviceData _devicedata;

        private string _deviceName;

        private Dictionary<string, string> DeviceCreator;

        public static Network GetInstance()
        {
            if (_singleton == null)
                _singleton = new Network();
            return _singleton;
        }

        public Network()
        {
            string path = "oui.txt";
            DeviceCreator = new Dictionary<string, string>();
            _devicedata = new NetworkDeviceData { IP = "null", MacAddress = "null", ManufactoryName = "null", ConnectedToNetwork = false };
            using (StreamReader reader = new StreamReader(path))
            {
                int startLine = 0;
                int workLine = 0;
                string? line;
                string? key;
                string? value;
                while ((line = reader.ReadLine()) != null)
                {
                    if (startLine >= 4)
                    {
                        if (workLine > 0 && workLine < 2)
                        {
                            try
                            {
                                line = Regex.Replace(line, @"\s+", " ");
                                key = line.Remove(line.IndexOf(' '));
                                if (key != "")
                                {
                                    line = line.Remove(0, line.IndexOf(' ') + 1);
                                    line = line.Remove(0, line.IndexOf(' ') + 1);
                                    value = line.Remove(0, line.IndexOf(' ') + 1);
                                    DeviceCreator.Add(key, value);
                                }
                            }
                            catch
                            {

                            }

                        }
                        workLine++;
                        if (workLine == 6)
                            workLine = 0;
                    }
                    else
                        startLine++;
                }
            }
        }

        private string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process Process = new System.Diagnostics.Process();
            Process.StartInfo.FileName = "arp";
            Process.StartInfo.Arguments = "-a " + ipAddress;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.Start();
            string strOutput = Process.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "OWN Machine";
            }
        }

        public void GetTcpConnections()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation t in connections)
            {
                Console.Write("Local endpoint: {0} ", t.LocalEndPoint.Address);
                Console.Write("Local port: {0} ", t.LocalEndPoint.Port);
                Console.Write("Remote endpoint: {0} ", t.RemoteEndPoint.Address);
                Console.Write("Remote port: {0} ", t.RemoteEndPoint.Port);
                Console.WriteLine("{0}", t.State);
            }
            Console.WriteLine();
        }

        public void GetTcpConnection(string ip, ref TcpState state)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation t in connections)
            {
                if (t.RemoteEndPoint.Address.ToString() == ip)
                {
                    state = t.State;
                    return;
                }
            }
        }

        private void ClearNetworkData()
        {
            _devicedata.IP = "null";
            _devicedata.ManufactoryName = "null";
            _devicedata.MacAddress = "null";
        }

        private string NetworkGateway()
        {
            string ip = null;

            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip = d.Address.ToString();
                    }
                }
            }
            return ip;
        }

        public NetworkDeviceData PingAll(string deviceName)
        {
            ClearNetworkData();
            _deviceName = deviceName;
            string gate_ip = NetworkGateway();
            if (gate_ip == null)
                return _devicedata;
            string[] array = gate_ip.Split('.');
            int i;
            for (i = 1; i <= 255; i++)
            {
                string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + i;
                Ping(ping_var, 1, 2000);
            }
            Thread.Sleep(5000);
            return _devicedata;
        }

        public void PingDevice(string deviceIP)
        {
            if (deviceIP != "null")
            {
                string[] array = deviceIP.Split('.');
                string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + array[3];
                PingDevice(ping_var, 1, 2000);
            }
        }

        private void Ping(string host, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        Ping ping = new Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        ping.SendAsync(host, timeout, host);
                    }
                    catch
                    {

                    }
                }).Start();
            }
        }

        private void PingDevice(string host, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        Ping ping = new Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingDeviceCompleted);
                        ping.SendAsync(host, timeout, host);
                    }
                    catch
                    {

                    }
                }).Start();
            }
        }

        private void PingDeviceCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                networkConnection?.Invoke(ip, true);
            }
            else
            {
                networkConnection?.Invoke(ip, false);
            }
        }

        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(ip);
                string macaddres = GetMacAddress(ip);
                string manufactoryname = "";
                try
                {
                    manufactoryname = DeviceCreator[(macaddres[0].ToString() + macaddres[1].ToString() + macaddres[3].ToString() + macaddres[4].ToString() + macaddres[6].ToString() + macaddres[7].ToString()).ToUpper()];
                }
                catch { }
                finally
                {
                    if ((_deviceName.Contains("Boca") && manufactoryname.Contains("Boca")) || (_deviceName.Contains("FEIG") && manufactoryname.Contains("FEIG")))
                    {
                        _devicedata.IP = ip;
                        _devicedata.MacAddress = macaddres;
                        _devicedata.ManufactoryName = manufactoryname;
                        _devicedata.ConnectedToNetwork = true;
                    }
                    //Console.WriteLine("IP: " + ip + "\n" + "  MacAddress: " + macaddres + "  Name: " + manufactoryname + "  Reply Status: " + e.Reply.Status.ToString());
                }
            }
        }

        private string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException)
            {

            }

            return "No HostName";
        }
    }
}
