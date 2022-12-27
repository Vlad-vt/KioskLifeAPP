using System.Collections.Generic;
using System.Printing;

namespace KioskLife.MVVM.Model.Printer
{
    public class USBPrinter : Printer
    {
        private string _errors;
        public string Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        public USBPrinter(string name, List<string> errors, string printerProcess, string printerOnline) : 
            base(name, errors, printerProcess, printerOnline)
        {

        }

        public void CheckForErrors(PrintQueue device)
        {
            Errors = "-";
            if (device.IsOffline)
            {
                Errors = "Printer Offline";
                IsOnline = "Offline";
            }
            else
            {
                IsOnline = "Online";
            }
            if (device.IsInError)
            {
                if (Errors.Length > 1)
                    Errors += ", Is In Error";
                else
                    Errors = "Is In Error";
            }
            if (device.IsPaperJammed)
            {
                if (Errors.Length > 1)
                    Errors += ", Paper Jammed";
                else
                    Errors = "Paper Jammed";
            }
            if (device.IsOutOfPaper)
            {
                if (Errors.Length > 1)
                    Errors += ", Is Out Of Paper";
                else
                    Errors = "Is Out Of Paper";
            }
        }
    }
}
