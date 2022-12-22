using AForge.Video.DirectShow;
using System.Collections;
using System;
using System.Drawing;
using KioskLife.Interfaces;
using System.Diagnostics;
using System.Windows;
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
        }

        public void ShowChanges()
        {
            AddAction($"{Name} camera started working!");
        }

    }

}
