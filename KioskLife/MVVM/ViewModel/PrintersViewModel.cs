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
            ActionList = new ObservableCollection<DeviceAction>();
            LocalPrintServer server = new LocalPrintServer();
            PrintersList = new ObservableCollection<Printer>();
            int count = 0;
            Dictionary<string, int> printersData = new Dictionary<string, int>();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                if (PrinterSettings.InstalledPrinters[i].Contains("Boca") && !printersData.ContainsKey("NetworkPrinter"))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new NetworkPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.NetworkPrinter, false, true));
                    PrintersList[count].Action += NewAction;
                    printersData.Add("NetworkPrinter", count);
                    count++;

                }
                else if ((PrinterSettings.InstalledPrinters[i] == server.DefaultPrintQueue.Name) && (server.DefaultPrintQueue.Name.Contains("NPI") || server.DefaultPrintQueue.Name.Contains("EPSON")) && !printersData.ContainsKey("USBPrinter"))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.USBPrinter, true));
                    PrintersList[count].Action += NewAction;
                    printersData.Add("USBPrinter", count);
                    count++;

                }
            }
            if (PrintersList.Count < 2 && PrintersList.Count > 0)
            {
                if (PrintersList[0].DeviceType == DeviceType.NetworkPrinter)
                {
                    PrintersList.Add(new USBPrinter("USB printer not found", new List<string>(), "Not working", "Offline", DeviceType.USBPrinter, false));
                    printersData.Add("USBPrinter", count);
                    count++;
                }
            }
            PrintersCount = PrintersList.Count.ToString();
            Thread usbPrintersThread = new Thread(() =>
            {
                while (true)
                {
                    server = new LocalPrintServer();
                    string defPrinterName = server.DefaultPrintQueue.Name;
                    int num = printersData.GetValueOrDefault("USBPrinter");
                    try
                    {
                        if (PrintersList[num].DeviceIsRunning)
                        {
                            PrintQueue printQueue = server.GetPrintQueue(PrintersList[num].Name);
                            (PrintersList[num] as USBPrinter).CheckForErrors(printQueue);
                        }
                        else if (defPrinterName.Contains("NPI") || defPrinterName.Contains("EPSON"))
                        {

                            App.Current.Dispatcher.Invoke(() =>
                            {
                                (PrintersList[num] as USBPrinter).UpdatePrinterData(defPrinterName, "Working", "Online", true);
                            });

                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                (PrintersList[num] as USBPrinter).UpdatePrinterData("USB printer not found", "Not working", "Offline", false);
                                (PrintersList[num] as USBPrinter).SendDeviceNotConnected();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                (PrintersList[num] as USBPrinter).UpdatePrinterData("USB printer not found", "Not working", "Offline", false);
                            }

                            catch (Exception e)
                            {

                            }

                        });
                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
            usbPrintersThread.IsBackground = true;
            usbPrintersThread.Start();
            Thread networkPrinterThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        int num = printersData.GetValueOrDefault("NetworkPrinter");

                        (PrintersList[num] as NetworkPrinter).CheckPrinter(false);

                        (PrintersList[num] as NetworkPrinter).CheckDeviceConnection();

                        (PrintersList[num] as NetworkPrinter).ConnectToDevice();

                        (PrintersList[num] as NetworkPrinter).readData();


                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        Thread.Sleep(10000);
                    }

                }
            });
            networkPrinterThread.IsBackground = true;
            networkPrinterThread.Start();
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
