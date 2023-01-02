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
        /// <summary>
        /// Dispenser COM name (COM1, COM2 , COM3 ...)
        /// </summary>
        [JsonProperty("DispenserCOM")]
        public string DispenserCOM { get; set; }

        /// <summary>
        /// Dispenser Errors (Not connected, Box empty ...)
        /// </summary>
        [JsonProperty("DispenserErrors")]
        public List<string> DispenserErrors { get; set; }

        [JsonProperty("DispenserWarnings")]
        public List<string> DispenserWarnings { get; set; }

        public string Errors { get; set; }

        public Dispenser(string name, List<string> deviceErrors, string isOnline, DeviceType deviceType) : base(name, deviceErrors, isOnline, deviceType)
        {

        }

        public Dispenser(string name, List<string> deviceErrors, string isOnline, DeviceType deviceType, string dispenserCOM, List<string> dispenserErrors, List<string> dispenserWarnings) : 
            base(name, deviceErrors, isOnline, deviceType)
        {
            DispenserCOM = dispenserCOM;
            DispenserErrors = dispenserErrors;
            DispenserWarnings = dispenserWarnings;
            for (int i = 0; i < DispenserErrors.Count; i++)
            {
                Errors += DispenserErrors[i] + ",";
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
                    formData["COM"] = DispenserCOM;
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
