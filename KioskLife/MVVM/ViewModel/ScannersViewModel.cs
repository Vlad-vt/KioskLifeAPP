using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner;
using KioskLife.MVVM.Model.Scanner.Zebra;
using System;
using System.Collections.ObjectModel;

namespace KioskLife.MVVM.ViewModel
{
    class ScannersViewModel : ObservableObject
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

        private ObservableCollection<Scanner> _scannersList;
        public ObservableCollection<Scanner> ScannersList
        {
            get
            {
                return _scannersList;
            }
            set
            {
                _scannersList = value;
                OnPropertyChanged();
            }
        }
        public ScannersViewModel()
        {
            ScannersList = new ObservableCollection<Scanner>
            {
                new ZebraScanner("Zebra Scanner", new System.Collections.Generic.List<string>{"Paper Jam"})
            };
            ActionList = new ObservableCollection<DeviceAction>
            {
                new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Scanner"),
            };
        }
    }
}
