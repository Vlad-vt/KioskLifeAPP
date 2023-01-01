using KioskLife.Core;
using KioskLife.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace KioskLife.MVVM.Model
{
    public abstract class Device : ObservableObject
    {
        public string MachineName { get; set; }
        public string Name { get; set; }
        protected List<string> DeviceErrors { get; set; }
        public string IsOnline { get; set; }
        [JsonIgnore]
        public string WorkingColor { get; set; }
        public DeviceType DeviceType { get; set; }

        public delegate void DeviceAction(string action);

        [JsonIgnore]
        public bool IsChanges { get; set; }

        public event DeviceAction Action;

        public Device(string name, List<string> deviceErrors, string isOnline, DeviceType deviceType)
        {
            MachineName = System.Environment.MachineName;
            Name = name;
            DeviceErrors = deviceErrors;
            IsOnline = isOnline;
            DeviceType = deviceType;
            if (IsOnline == "Online")
                WorkingColor = "#FF47FF3E";
            else
                WorkingColor = "#FFFF623E";

        }

        protected void AddAction(string action)
        {
            Action?.Invoke(action);
        }

        protected void CheckStatus()
        {
            if (IsOnline == "Online")
                WorkingColor = "#FF47FF3E";
            else
                WorkingColor = "#FFFF623E";
        }

        protected virtual void SendJSON()
        {
           // try
          //  {
                    WebClient webClient = new WebClient();
                   // MessageBox.Show("HELLO");
                    string json = "";
                    json = JsonConvert.SerializeObject(this, Formatting.Indented);
                    //json = json.Replace("{", "{\"").Replace(":", "\":\"").Replace(",", "\",\"").Replace("}", "\"}").Replace(":\"[", ":[").Replace(":\"{", ":{").Replace("https\":\"", "https:").Replace("http\":\"", "http:").Replace("\":\"9", ":9").Replace("}\",", "},").Replace("]\",", "],").Replace("}\"}", "}}");
                    MessageBox.Show(json);
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    NameValueCollection formData = new NameValueCollection();
                    foreach (var k in dict)
                    {
                        MessageBox.Show(k.Key.ToString());
                        MessageBox.Show(k.Value.ToString());
                        formData.Add(k.Key, k.Value);
                    }
                    //formData["username"] = "testuser";
                    //formData["password"] = "mypassword";

                    byte[] responseBytes = webClient.UploadValues("https://vr-kiosk.app/tntools/health_terminal.php", "POST", formData);
                    string responsefromserver = Encoding.UTF8.GetString(responseBytes);
                    MessageBox.Show(responsefromserver);
                    webClient.Dispose();
                
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.Message);

       //     }
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create("https://vr-kiosk.app/tntools/health_terminal.php");
                //var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                MessageBox.Show(json);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/text";
                using (var requestStream = httpRequest.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(json);
                }
                using (var httpResponse = httpRequest.GetResponse())
                using (var responseStream = httpResponse.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    string response = reader.ReadToEnd();
                    MessageBox.Show(response);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("SMTH wrong with endpoint");
            }
        }

        protected virtual void ShowJSON()
        {

        }

        protected virtual void WriteJSON()
        {
            string json = "";
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            json = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSerializerSettings);
            File.WriteAllText($"{DeviceType}.json", json);
        }
    }
}
