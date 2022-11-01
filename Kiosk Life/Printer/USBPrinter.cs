using Kiosk_Life.Information;
using Newtonsoft.Json;
using System.Printing;
namespace Kiosk_Life.Printer
{
    internal class USBPrinter : Printer, IErrorsCheck<PrintQueue, PrinterErrors>
    {
        [JsonIgnore]
        public List<PrinterErrors> DeviceErrors { get; set; }
        public List<string> Errors { get; set; }
        public USBPrinter(string Name, string FullName) : base(Name, FullName)
        {
            DeviceErrors = new List<PrinterErrors>();
            Errors = new List<string>();
        }

        public override void ShowAllInfo()
        {
            base.ShowAllInfo();
            for(int i = 0; i < Errors.Count; i++)
            {
                Console.WriteLine(Errors[i]);
            }
        }

        public void CheckForErrors(PrintQueue device)
        {
            if (device.IsOffline)
            {
                DeviceErrors.Add(PrinterErrors.PrinterOffline);
                Errors.Add("Printer Offline");
                PrinterOnline = false;
            }
            else
            {
                PrinterOnline = true;
            }
            if (device.IsInError)
            {
                DeviceErrors.Add(PrinterErrors.PrinterIsInError);
                Errors.Add("Is In Error");
            }
            else
            {
                Errors.Clear();
            }
            if (device.IsPaperJammed)
            {
                DeviceErrors.Add(PrinterErrors.PaperJam);
                Errors.Add("Paper Jammed");
            }
            else
            {
                Errors.Clear();
            }
            if (device.IsOutOfPaper)
            {
                DeviceErrors.Add(PrinterErrors.PrinterBinOut);
                Errors.Add("Is Out Of Paper");
            }
            else
            {
                Errors.Clear();
            }
        }
    }
}
