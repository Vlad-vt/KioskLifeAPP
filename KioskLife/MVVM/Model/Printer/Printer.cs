using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public abstract class Printer : Device
    {
        public string PrinterProcess { get; set; }
        public bool PrinterOnline { get; set; }

        public Printer(string name, List<string> errors, string printerProcess, bool printerOnline) : base(name, errors)
        {
            PrinterProcess = printerProcess;
            PrinterOnline = printerOnline;
        }
    }
}
