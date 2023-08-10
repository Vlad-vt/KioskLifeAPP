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
using System.Threading.Tasks;
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
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new NetworkPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.NetworkPrinter, false, true));
                        PrintersList[count].Action += NewAction;
                        count++;

                    }
                    else if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                    {
                        PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                        PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.USBPrinter, true));
                        PrintersList[count].Action += NewAction;
                        count++;

                    }
                    PrintersCount = PrintersList.Count.ToString();
                }
                if(PrintersList.Count < 2)
                {
                    if (PrintersList[0].DeviceType == DeviceType.NetworkPrinter) 
                    {
                        PrintersList.Add(new NetworkPrinter("NIPPON usb printer", new List<string>(), "Not working", "Offline", DeviceType.NetworkPrinter, false, false));
                        count++;
                    }
                }
                while (true)
                {
                    Parallel.ForEach(PrintersList, async printer =>
                    {
                        if(printer.DeviceIsRunning)
                        {
                            if(printer is USBPrinter usbPrinter)
                            {
                                PrintQueue printQueue = server.GetPrintQueue(printer.Name);
                                usbPrinter.CheckForErrors(printQueue);
                            }
                            else if (printer is NetworkPrinter networkPrinter)
                            {
                                networkPrinter.CheckPrinter(false);
                                networkPrinter.CheckDeviceConnection();
                                networkPrinter.ConnectToDevice();
                                networkPrinter.readData();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                            {
                                if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                                {
                                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                                    PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.USBPrinter, true));
                                    PrintersList[count].Action += NewAction;
                                    count++;

                                }
                                PrintersCount = PrintersList.Count.ToString();
                            }
                        }
                       /* if (printer.GetType() == typeof(USBPrinter) && printer.DeviceIsRunning)
                        {
                            PrintQueue printQueue = server.GetPrintQueue(printer.Name);
                            (printer as USBPrinter).CheckForErrors(printQueue);
                        }
                        else if (printer.GetType() == typeof(NetworkPrinter) && printer.DeviceIsRunning)
                        {
                            (printer as NetworkPrinter).CheckPrinter(false);
                            (printer as NetworkPrinter).CheckDeviceConnection();
                            (printer as NetworkPrinter).ConnectToDevice();
                            (printer as NetworkPrinter).readData();
                        }*/
                    });
                    /*foreach (var printer in PrintersList)
                    {
                        if(printer.GetType() == typeof(USBPrinter) && printer.DeviceIsRunning)
                        {
                            PrintQueue printQueue = server.GetPrintQueue(printer.Name);
                            (printer as USBPrinter).CheckForErrors(printQueue);
                        }
                        else if(printer.GetType() == typeof(NetworkPrinter) && printer.DeviceIsRunning)
                        {
                            (printer as NetworkPrinter).CheckPrinter(false);
                            (printer as NetworkPrinter).CheckDeviceConnection();
                            (printer as NetworkPrinter).ConnectToDevice();
                            (printer as NetworkPrinter).readData();
                        }
                    }*/
                    Thread.Sleep(2000);
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
