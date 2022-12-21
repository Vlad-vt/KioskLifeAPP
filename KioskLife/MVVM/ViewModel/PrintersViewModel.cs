using KioskLife.Core;
using KioskLife.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace KioskLife.MVVM.ViewModel
{
    class PrintersViewModel : ObservableObject
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

        private ObservableCollection<TestClass> myVar;

        public ObservableCollection<TestClass> PrintersList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public PrintersViewModel()
        {
            PrintersList = new ObservableCollection<TestClass>
            {
                new TestClass("DADSA",  "dasdas"),
                new TestClass("312dsa", "dasdas"),
                new TestClass("das90d", "dasdas"),
                new TestClass("das123", "dasdas"),
                new TestClass("dsad23", "dasdas"),
                new TestClass("dsa233", "dasdas"),
                new TestClass("dsa231", "dasdas"),
            };
            ActionList = new ObservableCollection<DeviceAction>
            {
                new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                 new DeviceAction("Device started working", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working2", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
                new DeviceAction("Device started working3", "[" + DateTime.Now.ToString() + "]:  ", "Printer"),
            };
            Thread testThread = new Thread(() =>
            {
                while (true)
                {
                    //Application.Current.Dispatcher.Invoke(() => { PrintersList.Add(new TestClass("AA13", 3000)); });
                    Thread.Sleep(3000);
                }
            });
            testThread.IsBackground = true;
            testThread.Start();
        }
    }
}
