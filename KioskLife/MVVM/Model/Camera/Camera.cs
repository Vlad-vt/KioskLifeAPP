using AForge.Video.DirectShow;
using System.Collections;
using System;
using System.Drawing;
using KioskLife.Interfaces;

namespace KioskLife.MVVM.Model.Camera
{
    public class Camera : IErrorsCheck<>
    {
        /// <summary>
        /// Camera Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Camera resolution (1920x1080 ...)
        /// </summary>
        public string Resolution { get; set; }
        /// <summary>
        /// Camera status
        /// </summary>
        public bool Online { get; set; }

        private VideoCaptureDevice _device;

        public Camera()
        {

        }

        public Camera(bool initialization)
        {
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection((Guid)FilterCategory.VideoInputDevice);
            if (filterInfoCollection == null || ((CollectionBase)filterInfoCollection).Count <= 0)
            {
                Console.WriteLine("No video devices found");
                return;
            }
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                if (filterInfo.Name.Contains("RGB"))
                {
                    _device = new VideoCaptureDevice(filterInfo.MonikerString);
                    int num1 = 0;
                    int num2 = 3000000;
                    int index1 = -1;
                    Size frameSize;
                    for (int index2 = 0; index2 < _device.VideoCapabilities.Length; ++index2)
                    {

                        VideoCapabilities videoCapability = _device.VideoCapabilities[index2];
                        frameSize = (Size)videoCapability.FrameSize;
                        int width = frameSize.Width;
                        frameSize = (Size)videoCapability.FrameSize;
                        int height = frameSize.Height;
                        int num3 = width * height;
                        if (num3 > num1 && num3 <= num2)
                        {
                            num1 = num3;
                            index1 = index2;
                        }
                    }
                    frameSize = _device.VideoCapabilities[index1].FrameSize;
                    Name = filterInfo.Name;
                    Online = true;
                    Resolution = frameSize.Height.ToString() + "*" + frameSize.Width.ToString();
                    break;
                }
            }
        }
    }

}
