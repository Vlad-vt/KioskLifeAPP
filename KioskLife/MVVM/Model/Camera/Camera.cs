using KioskLife.Enums;
using System.Collections.Generic;

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
            WriteJSON();
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


    }

}
