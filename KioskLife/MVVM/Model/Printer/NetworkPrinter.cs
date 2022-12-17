using HtmlAgilityPack;
using KioskLife.Network;
using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public class NetworkPrinter : Printer
    {
        public NetworkDeviceData NetworkData { get; set; }

        private HtmlWeb _webPage;

        private HtmlDocument _htmlDocument;

        public NetworkPrinter(string name, List<string> errors, string printerProcess, bool printerOnline) :
            base(name, errors, printerProcess, printerOnline)
        {
            _webPage= new HtmlWeb();
            _htmlDocument= new HtmlDocument();
        }
    }
}
