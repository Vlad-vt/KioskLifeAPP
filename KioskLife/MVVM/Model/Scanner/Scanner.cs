using KioskLife.Enums;
using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Scanner
{
    public class Scanner : Device
    {
        public string ScannerName { get; set; }
        public string Errors { get; set; }
        public Scanner(string name, List<string> deviceErrors, string isOnline, DeviceType deviceType) : base(name, deviceErrors, isOnline, deviceType)
        {
            ScannerName = Name;
            for (int i = 0; i < deviceErrors.Count; i++)
            {
                if (i == 0)
                    Errors += deviceErrors[i];
                else
                    Errors += "," + deviceErrors[i];
            }
        }
    }
}
