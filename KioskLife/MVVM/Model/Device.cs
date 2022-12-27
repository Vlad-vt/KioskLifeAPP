using KioskLife.Core;
using System.Collections.Generic;

namespace KioskLife.MVVM.Model
{
    public abstract class Device : ObservableObject
    {
        public string Name { get; set; }
        protected List<string> DeviceErrors { get; set; }
        public string IsOnline { get; set; }
        public string WorkingColor { get; set; }

        public delegate void DeviceAction(string action);

        public event DeviceAction Action;

        public Device(string name, List<string> deviceErrors, string isOnline)
        {
            Name = name;
            DeviceErrors = deviceErrors;
            IsOnline = isOnline;
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
    }
}
