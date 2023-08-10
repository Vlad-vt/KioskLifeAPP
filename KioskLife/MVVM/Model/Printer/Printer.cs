using KioskLife.Enums;
using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Printer
{
    public abstract class Printer : Device
    {
        public string PrinterProcess { get; set; }

        protected bool DeviceISFound { get; set; }

        public bool DeviceIsRunning 
        { 
            get 
            { 
                return DeviceISFound; 
            } 
        }

        public Printer(string name, List<string> errors, string printerProcess, string isOnline, DeviceType deviceType, bool deviceISFound) : base(name, errors, isOnline, deviceType)
        {
            PrinterProcess = printerProcess;
            AddAction($"{Name} succesfully initiated");
            DeviceISFound = deviceISFound;
        }

    }
}
