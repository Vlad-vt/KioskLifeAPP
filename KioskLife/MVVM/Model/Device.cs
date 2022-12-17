using System.Collections.Generic;

namespace KioskLife.MVVM.Model
{
    public abstract class Device 
    {
        protected string Name { get; set; }
        protected List<string> DeviceErrors { get; set; }

        public Device(string name, List<string> deviceErrors)
        {
            Name = name;
            DeviceErrors = deviceErrors;
        }
    }
}
