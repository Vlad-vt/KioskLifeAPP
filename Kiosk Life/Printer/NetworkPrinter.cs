﻿using Kiosk_Life.Information;
using Kiosk_Life.Network;
using System.Net.Sockets;

namespace Kiosk_Life.Printer
{
    internal class NetworkPrinter : Printer, INetworkCheck
    {
        public NetworkDevicedata NetworkData { get; set; }
        public List<string> Errors { get; set; }
        private TcpClient _printer { get; set; }
        private Stream _printerStream { get; set; }
        private StreamReader _streamReader { get; set; }
        private string _incomingData { get; set; }
        private char[] _incomingCharData { get; set; }
        private bool _connected { get; set; }   
        public void CheckDeviceConnection()
        {
            Network.Network.GetInstance().PingDevice(NetworkData.IP);
        }

        public void NetworkConnection(string ip, bool status)
        {
            if (ip == NetworkData.IP)
            {
                if (status)
                {
                    PrinterOnline = true;
                    PrinterProcess = "Working";
                }
                else
                {
                    PrinterOnline = false;
                    PrinterProcess = "Not working";
                }
                if (NetworkData.ConnectedToNetwork != status)
                {
                    NetworkData = new NetworkDevicedata(NetworkData, status);
                }
            }
        }

        public void ConnectToDevice()
        {
            try
            {
                if (!_connected && NetworkData.IP != null)
                {
                    _incomingCharData = new char[100];
                    _connected = true;
                    _printer = new TcpClient(NetworkData.IP, 9100);
                    _printerStream = _printer.GetStream();
                    _streamReader = new StreamReader(_printerStream);
                    _printerStream.ReadTimeout = 500;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string receiveData()
        {
            string data = "0";
            try
            {
                char[] buffer = new char[_printer.Available];
                while (_printer.Available > 0)
                {
                    try
                    {
                        int num = _streamReader.Read(buffer, 0, _printer.Available);
                        for (int index = 0; index < num; ++index)
                            data += buffer[index].ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch
            {

            }
            return data;
        }

        public void readData()
        {
            if (NetworkData.IP != null && _connected)
            {
                _incomingData = receiveData();
                if (_incomingData.Length > 0)
                {
                    Establish_Status();
                }
            }
        }

        private void Establish_Status()
        {
            _incomingCharData = _incomingData.ToCharArray();
            int length = _incomingData.Length;
            _incomingData = "";
            string text2 = "";
            Errors.Clear();
            for (int index = 0; index < length; ++index)
            {
                int num = (int)_incomingCharData[index];
                switch (num)
                {
                    case 0:
                        text2 = "";
                        break;
                    case 1:
                        text2 = "Start of Heading";
                        break;
                    case 2:
                        text2 = "Start of Text";
                        break;
                    case 3:
                        text2 = "";
                        break;
                    case 4:
                        text2 = "";
                        break;
                    case 5:
                        text2 = "Test Button Ticket ACK";
                        break;
                    case 6:
                        text2 = "Ticket ACK";
                        break;
                    case 7:
                        text2 = "Wrong File Identifier During Update";
                        break;
                    case 8:
                        text2 = "Invalid Checksum";
                        break;
                    case 9:
                        text2 = "Valid Checksum";
                        break;
                    case 10:
                        text2 = "";
                        break;
                    case 11:
                        text2 = "";
                        break;
                    case 12:
                        text2 = "";
                        break;
                    case 13:
                        text2 = "";
                        break;
                    case 14:
                        text2 = "";
                        break;
                    case 15:
                        text2 = "Low Paper";
                        Errors.Clear();
                        break;
                    case 16:
                        text2 = "Out of Tickets";
                        Errors.Add(text2);
                        break;
                    case 17:
                        text2 = "Paper full";
                        Errors.Clear();
                        break;
                    case 18:
                        text2 = "Power On";
                        Errors.Clear();
                        break;
                    case 19:
                        text2 = "Out of paper";
                        Errors.Add(text2);
                        break;
                    case 20:
                        text2 = "Bad Flash Memory";
                        Errors.Add(text2);
                        break;
                    case 21:
                        text2 = "Ticket NAK";
                        Errors.Add(text2);
                        break;
                    case 22:
                        text2 = "Ribbon Low";
                        Errors.Add(text2);
                        break;
                    case 23:
                        text2 = "Ribbon Out";
                        Errors.Add(text2);
                        break;
                    case 24:
                        text2 = "Ticket Jam";
                        Errors.Add(text2);
                        break;
                    case 25:
                        text2 = "Illegal Data";
                        Errors.Add(text2);
                        break;
                    case 26:
                        text2 = "Power Up Problem";
                        Errors.Add(text2);
                        break;
                    case 27:
                        text2 = "Ticket NAK";
                        Errors.Add(text2);
                        break;
                    case 28:
                        text2 = "Downloading Error";
                        Errors.Add(text2);
                        break;
                    case 29:
                        text2 = "Cutter Jam";
                        Errors.Add(text2);
                        break;
                    default:
                        text2 = "";
                        break;
                }
            }

        }

        public NetworkPrinter(string Name, string FullName) : base(Name, FullName)
        {
            Errors = new List<string>();
            _connected = false;
            NetworkData = Network.Network.GetInstance().PingAll(Name);
            Network.Network.GetInstance().networkConnection += NetworkConnection;
        }
    }
}
