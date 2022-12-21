using KioskLife.Core;
using KioskLife.MVVM.Model;
using System;
using System.Collections.ObjectModel;

namespace KioskLife.MVVM.ViewModel
{
    class TerminalsViewModel : ObservableObject
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
        public TerminalsViewModel()
        {
            ActionList = new ObservableCollection<DeviceAction>
            {
                new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Payment"),
            };
        }
    }
}
