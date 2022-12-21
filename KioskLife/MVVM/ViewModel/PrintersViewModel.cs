using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Printer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KioskLife.MVVM.ViewModel
{
    class PrintersViewModel : ObservableObject
    {
        private ObservableCollection<TestClass> _printersList;
        public ObservableCollection<TestClass> PrintersList
        {
            get
            {
                return _printersList;
            }
            set
            {
                _printersList = value;
                OnPropertyChanged();
            }
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
