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


        public Camera(string name, List<string> errors, string isOnline) : base(name, errors, isOnline)
        {

        }

        public Camera(string name, List<string> errors, string isOnline, string resolution) : base(name, errors, isOnline)
        {
            Resolution = resolution;
            for (int i = 0; i < errors.Count; i++)
            {
                if (i == 0)
                    Errors = errors[i];
                else
                    Errors = $",{errors[i]}";
            }
        }

        public void ShowChanges()
        {
            AddAction($"{Name} camera started working!");
        }

    }

}
