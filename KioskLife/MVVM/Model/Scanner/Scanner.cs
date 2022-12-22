using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Scanner
{
    public class Scanner : Device
    {
        public string ScannerName { get; set; }
        public string Errors { get; set; }
        public Scanner(string name, List<string> deviceErrors, string isOnline) : base(name, deviceErrors, isOnline)
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
