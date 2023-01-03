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

        protected List<string> LastErrors;

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
