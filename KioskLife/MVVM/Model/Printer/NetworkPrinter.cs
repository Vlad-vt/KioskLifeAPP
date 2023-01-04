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

        [JsonProperty("Network")]
        public NetworkDeviceData NetworkData { get; set; }

        private HtmlWeb _webPage;

        private HtmlDocument _htmlDocument;

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
