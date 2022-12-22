using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public abstract class Printer : Device
    {
        public string PrinterProcess { get; set; }

        public Printer(string name, List<string> errors, string printerProcess, string isOnline) : base(name, errors, isOnline)
        {
            PrinterProcess = printerProcess;
        }
    }
}
