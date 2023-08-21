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

        private string _isOnline;
        public string IsOnline 
        { 
            get
            {
                return _isOnline;   
            }
            set
            {
                _isOnline = value;
                OnPropertyChanged();
            }
        }

        private string _workingColor;

        [JsonIgnore]
        public string WorkingColor 
        { 
            get
            {
                return _workingColor;
            }
            set
            {
                _workingColor = value;
                OnPropertyChanged();
            }
        }

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
            IsChanges = true;
            LastErrors = new List<string>();

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
            try
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\");
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt"))
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt",
                        $"[{DateTime.Now}]: {json}");
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Terminals\log.txt",
                        $"[{DateTime.Now}]: {json}");
            }
            catch (IOException)
            {

            }
        }
    }
}
