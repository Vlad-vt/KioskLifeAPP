using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public class USBPrinter : Printer
    {
        public USBPrinter(string name, List<string> errors, string printerProcess, string printerOnline) : 
            base(name, errors, printerProcess, printerOnline)
        {

        }
    }
}
