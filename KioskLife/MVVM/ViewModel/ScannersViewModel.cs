using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner;
using KioskLife.MVVM.Model.Scanner.Zebra;
using System;
using System.Collections.ObjectModel;
using System.Net;

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
        public ScannersViewModel(HttpListener httpListener)
        {
            ScannersList = new ObservableCollection<Scanner>
            {
                new ZebraScanner("Zebra Scanner", new System.Collections.Generic.List<string>{"Paper Jam"}, "Online", Enums.DeviceType.Scanner)
            };
        }
    }
}
