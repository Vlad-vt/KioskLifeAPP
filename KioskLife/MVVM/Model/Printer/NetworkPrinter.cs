﻿using HtmlAgilityPack;
using KioskLife.Enums;
using KioskLife.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

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

        protected override void WriteJSON()
        {
            string json = "";
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            json = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSerializerSettings);
            File.WriteAllText($"{DeviceType}.json", json);
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
