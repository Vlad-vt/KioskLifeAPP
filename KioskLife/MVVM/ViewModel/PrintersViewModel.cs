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

        public PrintersViewModel()
        {
            Thread printersThread = new Thread(() =>
            {
                LocalPrintServer server = new LocalPrintServer();
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new NetworkPrinter(printQueue.Name, new List<string>(),"Working", "Online", DeviceType.NetworkPrinter));
                    }
                    if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.NetworkPrinter));
                    }
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

                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            PrintersList = new ObservableCollection<Printer>
            {
                new USBPrinter("NPI Nippon", new System.Collections.Generic.List<string>(), "", "Online", Enums.DeviceType.USBPrinter),
                new USBPrinter("Boca BDI", new System.Collections.Generic.List<string>(), "", "Online", Enums.DeviceType.NetworkPrinter),
                //new NetworkPrinter("312dsa", "dasdas"),
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

        private void GetAllPrinters()
        {
            LocalPrintServer server = new LocalPrintServer();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new NetworkPrinter(printQueue.Name, new System.Collections.Generic.List<string>(), "Working", "Online", Enums.DeviceType.USBPrinter));
                }
                if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new USBPrinter(printQueue.Name, new System.Collections.Generic.List<string>(), "Working", "Online", Enums.DeviceType.NetworkPrinter));
                }
            }
            do
            {
                foreach (Printer printer in PrintersList)
                {
                    if (printer.GetType() == typeof(USBPrinter))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(printer.Name);
                        if (printQueue.IsPrinting)
                            printer.PrinterProcess = "Printing";
                        else if (printQueue.IsPaused)
                            printer.PrinterProcess = "Paused";
                        else if (printQueue.IsBusy)
                            printer.PrinterProcess = "Busy";
                        else
                            printer.PrinterProcess = "Working";
                        (printer as USBPrinter).CheckForErrors(printQueue);
                    }
                    else if (printer.GetType() == typeof(NetworkPrinter))
                    {
                        //(printer as NetworkPrinter).CheckPrinter();
                        //(printer as NetworkPrinter).CheckDeviceConnection();
                        //(printer as NetworkPrinter).ConnectToDevice();
                        //(printer as NetworkPrinter).readData();
                    }
                }
            } while (true);
        }
    }
}
