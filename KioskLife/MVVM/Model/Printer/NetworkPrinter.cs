using HtmlAgilityPack;
using KioskLife.Enums;
using KioskLife.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KioskLife.MVVM.Model.Printer
{
    public class NetworkPrinter : Printer
    {
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
    }
}
