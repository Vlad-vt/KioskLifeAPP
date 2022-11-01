using System.Diagnostics;
using System.Printing;
using Kiosk_Life.Network;
using Kiosk_Life.Information;
using Newtonsoft.Json;
namespace Kiosk_Life.Printer
{
    public class Printer : IConsoleInfo
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string PrinterProcess { get; set; }
        public bool PrinterOnline { get; set; }

        public Printer()
        {

        }

        public Printer(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
            PrinterProcess = "Working";
        }

        public virtual void ShowAllInfo()
        {
                Console.WriteLine("Printer Name:     " + Name
                    + "\n" + "Printer FullName:    " + FullName
                    + "\n" + "Printer Process: " + PrinterProcess
                    + "\n" + "Printer Online: " + PrinterOnline);
        }

    }
}
