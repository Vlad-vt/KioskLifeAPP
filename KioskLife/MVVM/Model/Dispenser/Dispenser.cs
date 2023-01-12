using KioskLife.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace KioskLife.MVVM.Model.Dispenser
{
    public class Dispenser : Device
    {
        public string Errors { get; set; }

        public string Warnings { get; set; }

        public Dispenser(string name, List<string> deviceErrors, List<string> deviceWarnings, string isOnline, DeviceType deviceType) : 
            base(name, deviceErrors, isOnline, deviceType)
        {
            Name = name;
            IsOnline = isOnline;
            DeviceType = deviceType;
            Errors = "";
            System.Diagnostics.Trace.WriteLine("DISPENSER INFO:::::::::");
            for (int i = 0; i < deviceErrors.Count; i++)
            {
                deviceErrors[i] = deviceErrors[i].Remove(deviceErrors[i].Length - 1, 1).Remove(0,1);
                if (deviceErrors[i] != "")
                {
                    if (i != 0)
                        Errors += deviceErrors[i] + ",";
                    else
                        Errors += deviceErrors[i];
                }
                else
                {
                    Errors += "-";
                }
            }
            for(int i = 0; i < deviceWarnings.Count; i++)
            {
                if (i != 0)
                    Warnings += deviceWarnings[i] + ",";
                else
                    Warnings += deviceWarnings[i];
            }
            if (Errors == "Dispenser not connected")
                IsOnline = "Offline";
            else if (Errors != "-")
                IsOnline = "Errors";
        }

        public void CheckChanges(Dispenser dispenser)
        {
            if (dispenser.Errors != Errors)
                IsChanges = true;
            else
            {
                IsChanges = false;
                return;
            }
            Errors = dispenser.Errors;
        }

        public void CheckChanges()
        {
            SendJSON();
        }

        protected override void SendJSON()
        {
            if (IsChanges)
            {
                CheckStatus();
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        NameValueCollection formData = new NameValueCollection();
                        formData["MachineName"] = MachineName;
                        formData["Type"] = DeviceType.ToString();
                        formData["COM"] = Name;
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
}
