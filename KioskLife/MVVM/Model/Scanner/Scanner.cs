using System.Collections.Generic;

namespace KioskLife.MVVM.Model.Scanner
{
    public class Scanner : Device
    {
        public string Errors { get; set; }
        public Scanner(string name, List<string> deviceErrors) : base(name, deviceErrors)
        {
            for (int i = 0; i < deviceErrors.Count; i++)
            {
                Errors += deviceErrors[i] + ",";
            }
        }
    }
}
