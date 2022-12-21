using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Camera;
using KioskLife.MVVM.Model.Dispenser;
using System;
using System.Collections.ObjectModel;

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
        public CamerasViewModel()
        {
            CamerasList = new ObservableCollection<Camera>
            {
                new Camera(true)
            };
            ActionList = new ObservableCollection<DeviceAction>
            {
                new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Camera"),
            };
        }
    }
}
