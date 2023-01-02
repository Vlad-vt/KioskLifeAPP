using KioskLife.Enums;
using System.Collections.Generic;
using System.Printing;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

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

        public USBPrinter(string name, List<string> errors, string printerProcess, string printerOnline, DeviceType deviceType) : 
            base(name, errors, printerProcess, printerOnline, deviceType)
        {
            WriteJSON();
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

        protected override void SendJSON()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection formData = new NameValueCollection();
                    formData["Type"] = DeviceType.ToString();
                    formData["Name"] = Name;
                    formData["Process"] = PrinterProcess;
                    formData["Errors"] = Errors;
                    formData["Status"] = IsOnline;
                    byte[] responseBytes = webClient.UploadValues("https://vr-kiosk.app/tntools/health_terminal.php", "POST", formData);
                    string responsefromserver = Encoding.UTF8.GetString(responseBytes);
                    webClient.Dispose();
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("log.txt", e.Message + "\n");
            }
        }
    }
}
