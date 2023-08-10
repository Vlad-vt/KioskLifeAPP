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

        public USBPrinter(string name, List<string> errors, string printerProcess, string printerOnline, DeviceType deviceType, bool deviceISFound) : 
            base(name, errors, printerProcess, printerOnline, deviceType, deviceISFound)
        {
            WriteJSON();
            LastErrors = new List<string>();
        }

        public void CheckForErrors(PrintQueue device)
        {
            Errors = "-";
            if (device.IsOffline)
            {
                if (CheckLastChanges("Printer Offline"))
                {
                    LastErrors.Add("Printer Offline");
                    IsOnline = "Offline";
                    PrinterProcess = "Not Working";
                    IsChanges = true;
                    AddAction($"{Name} is Offline now!");
                }
            }
            else
            {
                if (LastErrors.Remove("Printer Offline"))
                {
                    IsChanges = true;
                    AddAction($"SOLVED --Online now--");
                }
                PrinterProcess = "Working";
                IsOnline = "Online";
            }
            if (device.IsInError)
            {
                if (CheckLastChanges("Is In Error"))
                {
                    LastErrors.Add("Is In Error");
                    AddAction($"{Name} has error!");
                    IsChanges = true;
                    IsOnline = "Errors";
                }
            }
            else
            {
                if (LastErrors.Remove("Is In Error"))
                {
                    IsChanges = true;
                    AddAction($"SOLVED --No more errors--");
                }
            }
            if (device.IsPaperJammed)
            {
                if (CheckLastChanges("Paper Jammed"))
                {
                    LastErrors.Add("Paper Jammed");
                    AddAction($"{Name} has paper jam!");
                    IsChanges = true;
                    IsOnline = "Errors";
                }
            }
            else
            {
                if (LastErrors.Remove("Paper Jammed"))
                {
                    IsChanges = true;
                    AddAction($"SOLVED --Paper Jammed--");
                }
            }
            if (device.IsOutOfPaper)
            {
                if (CheckLastChanges("Is Out Of Paper"))
                {
                    LastErrors.Add("Is Out Of Paper");
                    AddAction($"{Name} Is Out Of Paper now");
                    IsChanges = true;
                    IsOnline = "Errors";
                }
            }
            else
            {
                if (LastErrors.Remove("Is Out Of Paper"))
                {
                    IsChanges = true;
                    AddAction($"SOLVED --Is Out Of Paper--");
                }
            }
            for (int i = 0; i < LastErrors.Count; i++)
            {
                if (i == 0)
                    Errors = LastErrors[i];
                else
                    Errors += "," + LastErrors[i];
            }
            CheckStatus();
            if (IsChanges)
                SendJSON();
        }

        private bool CheckLastChanges(string newChange)
        {
            bool changes = true;
            for (int i = 0; i < LastErrors.Count; i++)
            {
                if (LastErrors[i] == newChange)
                    changes = false;
            }
            return changes;
        }

        public void ShowChanges()
        {
            AddAction($"{Name} terminal started working!");
            SendJSON();
            WriteJSON();
        }

        public void AddEvent(string events)
        {
            CheckStatus();
            WriteJSON();
            AddAction($"{events}");
        }

        protected override void SendJSON()
        {
            try
            {
                IsChanges = false;
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection formData = new NameValueCollection();
                    formData["MachineName"] = MachineName;
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
                try
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\USBPrinters\");
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\USBPrinters\log.txt"))
                    {
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\USBPrinters\log.txt",
                            $"[{DateTime.Now}]: {e.Message}");
                    }
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\USBPrinters\log.txt",
                            $"[{DateTime.Now}]: {e.Message}");
                }
                catch (IOException)
                {

                }
            }
        }
    }
}
