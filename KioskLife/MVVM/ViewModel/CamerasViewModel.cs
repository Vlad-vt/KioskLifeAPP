using AForge.Video.DirectShow;
using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Camera;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace KioskLife.MVVM.ViewModel
{
    class CamerasViewModel : ObservableObject
    {
        private ObservableCollection<DeviceAction> _actionList;
        public ObservableCollection<DeviceAction> ActionList
        {
            get
            {
                return _actionList;
            }
            set
            {
                _actionList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Camera> _camerasList;
        public ObservableCollection<Camera> CamerasList
        {
            get
            {
                return _camerasList;
            }
            set
            {
                _camerasList = value;
                OnPropertyChanged();
            }
        }

        private int _cameraCount;

        public int CameraCount
        {
            get { return _cameraCount; }
            set { _cameraCount = value; OnPropertyChanged(); }
        }


        private VideoCaptureDevice _device;



        public CamerasViewModel()
        {
            Thread camerasInfoThread = new Thread(() =>
            {
                while (true)
                {
                    GetAllCameras();
                    Thread.Sleep(10000);
                }
            });
            camerasInfoThread.IsBackground = true;
            camerasInfoThread.Start();
        }

        private void GetAllCameras()
        {
            CamerasList = new ObservableCollection<Camera>();
            int i = 0;
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection((Guid)FilterCategory.VideoInputDevice);
            if (filterInfoCollection == null || ((CollectionBase)filterInfoCollection).Count <= 0)
            {
                Trace.WriteLine("No video devices found");
                CameraCount = CamerasList.Count;
                return;
            }
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                //if (filterInfo.Name.Contains("RGB"))
                //{
                _device = new VideoCaptureDevice(filterInfo.MonikerString);
                int num1 = 0;
                int num2 = 3000000;
                int index1 = -1;
                System.Drawing.Size frameSize;
                if (_device.VideoCapabilities.Length > 0)
                {
                    for (int index2 = 0; index2 < _device.VideoCapabilities.Length; ++index2)
                    {

                        VideoCapabilities videoCapability = _device.VideoCapabilities[index2];
                        frameSize = (System.Drawing.Size)videoCapability.FrameSize;
                        int width = frameSize.Width;
                        frameSize = (System.Drawing.Size)videoCapability.FrameSize;
                        int height = frameSize.Height;
                        int num3 = width * height;
                        if (num3 > num1 && num3 <= num2)
                        {
                            num1 = num3;
                            index1 = index2;
                        }
                    }
                    frameSize = _device.VideoCapabilities[index1].FrameSize;
                    CamerasList.Add(new Camera(filterInfo.Name, new List<string>(), "Online", frameSize.Height.ToString() + "*" + frameSize.Width.ToString()));
                    CamerasList[i].Action += NewAction;
                    CamerasList[i].ShowChanges();
                    i++;
                    break;
                }
                // }
            }
            CameraCount = CamerasList.Count;
        }

        private void NewAction(string action)
        {
            ActionList.Add(new DeviceAction(action, "[" + DateTime.Now.ToString() + "]:  ", "Camera"));
        }
    }
}
