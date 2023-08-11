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
                if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new NetworkPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.NetworkPrinter, false, true));
                    PrintersList[count].Action += NewAction;
                    printersData.Add("NetworkPrinter", count);
                    count++;

                }
                else if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                {
                    PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                    PrintersList.Add(new USBPrinter(printQueue.Name, new List<string>(), "Working", "Online", DeviceType.USBPrinter, true));
                    PrintersList[count].Action += NewAction;
                    printersData.Add("USBPrinter", count);
                    count++;

                }
            }
            if (PrintersList.Count < 2)
            {
                if (PrintersList[0].DeviceType == DeviceType.NetworkPrinter)
                {
                    PrintersList.Add(new USBPrinter("NIPPON usb printer not found", new List<string>(), "Not working", "Offline", DeviceType.USBPrinter, false));
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
                        lock (PrintersList)
                        {
                            //System.Windows.MessageBox.Show($"DEVICE IS FOUND? {PrintersList[num].DeviceIsRunning}");
                            if (PrintersList[num].DeviceIsRunning)
                            {
                                PrintQueue printQueue = server.GetPrintQueue(PrintersList[num].Name);
                                (PrintersList[num] as USBPrinter).CheckForErrors(printQueue);
                            }
                            else if (defPrinterName.Contains("NPI"))
                            {

                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    (PrintersList[num] as USBPrinter).UpdatePrinterData(defPrinterName, "Working", "Online", true);
                                    //PrintersList[num].Name = defPrinterName;
                                    //PrintersList[num].IsOnline = "Online";
                                    //PrintersList.RemoveAt(num);
                                    //PrintersList.Add(new USBPrinter(defPrinterName, new List<string>(), "Working", "Online", DeviceType.USBPrinter, true));
                                    //PrintersList[PrintersList.Count - 1].Action += NewAction;
                                    //PrintersCount = PrintersList.Count.ToString();
                                });

                            }
                            else
                            {
                                (PrintersList[num] as USBPrinter).SendDeviceNotConnected();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (PrintersList)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                (PrintersList[num] as USBPrinter).UpdatePrinterData("NIPPON usb printer not found", "Not working", "Offline", true);
                                //PrintersList.RemoveAt(num);
                                //printersData.Remove("USBPrinter");
                                //PrintersList.Add(new USBPrinter("NIPPON usb printer not found", new List<string>(), "Not working", "Offline", DeviceType.USBPrinter, false));
                                //printersData.Add("USBPrinter", 1);
                                //PrintersCount = PrintersList.Count.ToString();
                            });
                        }
                        //MessageBox.Show(ex.Message);
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
                        lock (PrintersList)
                        {
                            (PrintersList[num] as NetworkPrinter).CheckPrinter(false);
                        }
                        Thread.Sleep(2000);
                        lock (PrintersList)
                        {
                            (PrintersList[num] as NetworkPrinter).CheckDeviceConnection();
                        }
                        Thread.Sleep(2000);
                        lock (PrintersList)
                        {
                            (PrintersList[num] as NetworkPrinter).ConnectToDevice();
                        }
                        Thread.Sleep(2000);
                        lock (PrintersList)
                        {
                            (PrintersList[num] as NetworkPrinter).readData();
                        }
                        Thread.Sleep(2000);

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
