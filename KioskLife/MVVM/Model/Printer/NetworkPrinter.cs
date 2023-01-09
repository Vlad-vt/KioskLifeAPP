using HtmlAgilityPack;
using KioskLife.Enums;
using KioskLife.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Sockets;

namespace KioskLife.MVVM.Model.Printer
{
    public class NetworkPrinter : Printer
    {
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

        private bool _isParsing = false;

        private NetworkDeviceData _networkData;
        [JsonProperty("Network")]
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

        private HtmlWeb _webPage;

        private HtmlDocument _htmlDocument;


        private TcpClient _printer { get; set; }
        private Stream _printerStream { get; set; }
        private StreamReader _streamReader { get; set; }
        private string _incomingData { get; set; }
        private char[] _incomingCharData { get; set; }


        public NetworkPrinter(string name, List<string> errors, string printerProcess, string printerOnline, DeviceType deviceType) :
            base(name, errors, printerProcess, printerOnline, deviceType)
        {
            NetworkData = new NetworkDeviceData();
            _webPage= new HtmlWeb();
            _htmlDocument= new HtmlDocument();
            WriteJSON();
        }

        public void CheckPrinter(bool test)
        {
            if(_isParsing) 
                return;

            #region Initiation
            //Errors = "-";
            string pageText = "", ip = "";
            if (test)
            {
                pageText = "http://192.168.178.172/realtime.htm";
                ip = "192.168.178.172";
            }
            else
            {
                pageText = "http://192.168.0.155/realtime.htm";
                ip = "192.168.0.155";
            }
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            #endregion

            #region Parsing Page
            try
            {
                document = web.Load(pageText);
                _isParsing= true;
            }
            catch (Exception e)
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "/log.txt"))
                    File.Create(Directory.GetCurrentDirectory() + "/log.txt");
                File.WriteAllText(Directory.GetCurrentDirectory() + "/log.txt", "[" + DateTime.Now.ToString() + "]: " + e);
            }
            #endregion

             #region Getting Info
            try
            {
                HtmlNode[] nodes = document.DocumentNode.SelectNodes("//td").Where(x => x.InnerHtml.Contains("READY")).ToArray();
                HtmlNode[] nodes1 = document.DocumentNode.SelectNodes("//tr").Where(x => x.InnerHtml.Contains("MAC ID")).ToArray();
                string macAddress = nodes1[2].InnerText.Substring(nodes1[2].InnerText.IndexOf("MAC ID = ") + "MAC ID = ".Length);
                NetworkData = new NetworkDeviceData { IP = ip, MacAddress = macAddress, ConnectedToNetwork = true, ManufactoryName = "Boca printer" };
                PrinterProcess = "Working";
                string result = nodes[0].InnerText.Replace("\n", "").Replace("\r", "");
                #region Getting errors
                for (int i = 0; i < 5; i++)
                {
                    string text = "";
                    switch (i)
                    {
                        case 0:
                            text = result.Substring(result.IndexOf("READY", 4));
                            text = text.Substring(text.IndexOf("READY") + "READY".Length + 1);
                            if (text[0].ToString() + text[1].ToString() == "YE")
                            {
                                text = "YES";
                                PrinterProcess = "Not Working";
                                if (LastErrors.Remove("Not ready!"))
                                {
                                    IsChanges = true;
                                    AddAction($"SOLVED --Not ready to print--");
                                }
                            }
                            else
                            {
                                text = "NO";
                                if(LastErrors.Contains("Not ready!"))
                                {
                                    LastErrors.Add("Not ready!");
                                    AddAction($"{Name} not ready to print now!");
                                    IsOnline = "Errors";
                                    IsChanges = true;
                                }
                            }
                            break;
                        case 1:
                            text = result.Substring(result.IndexOf("TICKETS LOW"));
                            text = text.Substring(text.IndexOf("TICKETS LOW") + "TICKETS LOW".Length + 1);
                            if (text[0].ToString() + text[1].ToString() == "YE")
                            {
                                text = "YES";
                                if(!LastErrors.Contains("Tickets Low"))
                                {
                                    LastErrors.Add("Tickets Low");
                                    IsOnline = "Errors";
                                    AddAction($"{Name} has low tickets!");
                                    IsChanges = true;
                                }
                            }
                            else
                            {
                                text = "NO";
                                if (LastErrors.Remove("TicketsLow"))
                                {
                                    IsChanges = true;
                                    AddAction($"SOLVED --Tickets Low--");
                                }
                            }
                            break;
                        case 2:
                            text = result.Substring(result.IndexOf("PAPER OUT"));
                            text = text.Substring(text.IndexOf("PAPER OUT") + "PAPER OUT".Length);
                            if (text[0].ToString() + text[1].ToString() == "YE")
                            {
                                if (!LastErrors.Contains("Paper Out"))
                                {
                                    LastErrors.Add("Paper Out");
                                    IsOnline = "Errors";
                                    AddAction($"{Name} is out of paper now!");
                                    IsChanges = true;
                                }
                                text = "YES";
                            }
                            else
                            {
                                text = "NO";
                                if (LastErrors.Remove("Paper Out"))
                                {
                                    IsChanges = true;
                                    AddAction($"SOLVED --Paper Out--");
                                }
                            }
                            break;
                        case 3:
                            text = result.Substring(result.IndexOf("PAPER JAM"));
                            text = text.Substring(text.IndexOf("PAPER JAM") + "PAPER JAM".Length + 1);
                            if (text[0].ToString() + text[1].ToString() == "YE" || text[0].ToString() == "E" || text[0].ToString() == "S")
                            {
                                if (!LastErrors.Contains("Paper Jam"))
                                {
                                    LastErrors.Add("Paper Jam");
                                    IsOnline = "Errors";
                                    AddAction($"{Name} has paper jam now!");
                                    IsChanges = true;
                                }
                                text = "YES";
                            }
                            else
                            {
                                text = "NO";
                                if (LastErrors.Remove("Paper Jam"))
                                {
                                    IsChanges = true;
                                    AddAction($"SOLVED --Paper Jam--");
                                }
                            }
                            break;
                        case 4:
                            text = result.Substring(result.IndexOf("CUTTER JAM"));
                            text = text.Substring(text.IndexOf("CUTTER JAM") + "CUTTER JAM".Length + 1);
                            if (text[0].ToString() + text[1].ToString() == "YE")
                            {
                                if (!LastErrors.Contains("Cutter Jam"))
                                {
                                    LastErrors.Add("Cutter Jam");
                                    IsOnline = "Errors";
                                    AddAction($"{Name} has cutter jam now!");
                                    IsChanges = true;
                                }
                                text = "YES";
                            }
                            else
                            {
                                text = "NO";
                                if (LastErrors.Remove("Cutter Jam"))
                                {
                                    IsChanges = true;
                                    AddAction($"SOLVED --Cutter Jam--");
                                }
                            }
                            break;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "/log.txt"))
                    File.Create(Directory.GetCurrentDirectory() + "/log.txt");
                File.WriteAllText(Directory.GetCurrentDirectory() + "/log.txt", "[" + DateTime.Now.ToString() + "]: " + e);
            }
            finally
            {
                for (int i = 0; i < LastErrors.Count; i++)
                {
                    if (i == 0)
                        Errors = LastErrors[i];
                    else
                        Errors += "," + LastErrors[i];
                }
                if(LastErrors.Count == 0)
                    IsOnline = "Online";
                if (IsChanges)
                    SendJSON();
                CheckStatus();
                _isParsing = false;
            }
            #endregion
        }

        public void ConnectToDevice()
        {
            try
            {
                if (NetworkData.IP != null)
                {
                    _incomingCharData = new char[100];
                    _printer = new TcpClient(NetworkData.IP, 9100);
                    _printerStream = _printer.GetStream();
                    _streamReader = new StreamReader(_printerStream);
                    _printerStream.ReadTimeout = 500;
                }
            }
            catch (Exception e)
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
            if (NetworkData.IP != null)
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

        protected override void SendJSON()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection formData = new NameValueCollection();
                    formData["Type"] = DeviceType.ToString();
                    formData["Name"] = Name;
                    formData["Process"] = PrinterProcess;
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
            catch (Exception e)
            {
                File.WriteAllText("log.txt", e.Message + "\n");
            }
        }
    }
}
