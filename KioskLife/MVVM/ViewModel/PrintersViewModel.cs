using KioskLife.Core;
using KioskLife.Enums;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Printer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Printing;
using System.Reflection;
using System.Threading;
using System.Windows;

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

        private ObservableCollection<Printer> myVar;

        public ObservableCollection<Printer> PrintersList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private string _printersCount;

        public string PrintersCount
        {
            get { return _printersCount; }
            set { _printersCount = value; OnPropertyChanged(); }
        }


        public PrintersViewModel()
        {
            Thread printersThread = new Thread(() =>
            {
                ActionList = new ObservableCollection<DeviceAction>();
                LocalPrintServer server = new LocalPrintServer();
                PrintersList = new ObservableCollection<Printer>();
                int count = 0;
                bool defaultPrinterFound = false;
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new NetworkPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.NetworkPrinter, false));
                        PrintersList[count].Action += NewAction;
                        count++;

                    }
                    else if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name) && !defaultPrinterFound)
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.USBPrinter));
                        PrintersList[count].Action += NewAction;
                        count++;
                        defaultPrinterFound = true;
                    }
                    PrintersCount = PrintersList.Count.ToString();
                }
                while (true)
                {
                    foreach(var printer in PrintersList)
                    {
                        if(printer.GetType() == typeof(USBPrinter))
                        {
                            PrintQueue printQueue = server.GetPrintQueue(printer.Name);
                            (printer as USBPrinter).CheckForErrors(printQueue);
                        }
                        else if(printer.GetType() == typeof(NetworkPrinter))
                        {
                            (printer as NetworkPrinter).CheckPrinter(false);
                            (printer as NetworkPrinter).CheckDeviceConnection();
                            (printer as NetworkPrinter).ConnectToDevice();
                            (printer as NetworkPrinter).readData();
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            printersThread.IsBackground = true;
            printersThread.Start();
        }

        private void NewAction(string action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ActionList.Insert(0, new DeviceAction(action, "[" + DateTime.Now.ToString() + "]:  ", "Printer"));
            });
        }
    }
}
