using KioskLife.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace KioskLife.MVVM.Model.Dispenser
{
    public class Dispenser : Device
    {
        public string Errors { get; set; }

        public Dispenser(string name, List<string> deviceErrors, string isOnline, DeviceType deviceType) : 
            base(name, deviceErrors, isOnline, deviceType)
        {
            Name = name;
            IsOnline = isOnline;
            DeviceType = deviceType;
            for (int i = 0; i < deviceErrors.Count; i++)
            {
                Errors += deviceErrors[i] + ",";
            }
        }

        protected override void SendJSON()
        {
            if (IsChanges)
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        NameValueCollection formData = new NameValueCollection();
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
