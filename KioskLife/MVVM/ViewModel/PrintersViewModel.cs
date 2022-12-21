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

namespace KioskLife.MVVM.ViewModel
{
    class PrintersViewModel : ObservableObject
    {
        private List<TestClass> _printersList;
        public List<TestClass> PrintersList
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
        //public event NotifyCollectionChangedEventHandler CollectionChanged;
        //public ObservableCollection<TestClass> PrintersList;
        public PrintersViewModel()
        {
            PrintersList = new List<TestClass>
            {
                new TestClass("DADSA",  100),
                new TestClass("312dsa", 150),
                new TestClass("das90d", 150),
                new TestClass("das123", 250),
                new TestClass("dsad23", 150),
                new TestClass("dsa233", 200),
                new TestClass("dsa231", 150),
            };
            Thread testThread = new Thread(() =>
            {
                while(true)
                {
                    PrintersList.Add(new TestClass("AA13", 3000));
                    OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Add);
                    Thread.Sleep(3000);
                }
            });
            testThread.IsBackground = true;
            testThread.Start();
        }
    }
}
