using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public class USBPrinter : Printer
    {
        public USBPrinter(string name, List<string> errors, string printerProcess, bool printerOnline) : 
            base(name, errors, printerProcess, printerOnline)
        {

        }
    }
}
