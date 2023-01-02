using KioskLife.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace KioskLife.MVVM.Model.Camera
{
    public class Camera : Device
    {
        /// <summary>
        /// Camera resolution (1920x1080 ...)
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Camera errors
        /// </summary>
        public string Errors { get; set; }

        public Camera(string name, List<string> errors, string isOnline, string resolution, DeviceType deviceType) : base(name, errors, isOnline, deviceType)
        {
            Resolution = resolution;
            for (int i = 0; i < errors.Count; i++)
            {
                if (i == 0)
                    Errors = errors[i];
                else
                    Errors = $",{errors[i]}";
            }
            SendJSON();
        }

        public void ShowChanges()
        {
            AddAction($"{Name} camera started working!");
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
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection formData = new NameValueCollection();
                    formData["Type"] = DeviceType.ToString();
                    formData["Name"] = Name;
                    formData["Resolution"] = Resolution;
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
