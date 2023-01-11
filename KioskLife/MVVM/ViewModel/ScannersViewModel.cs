using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner;
using KioskLife.MVVM.Model.Scanner.Zebra;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

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

        private string _scannersCount;
        public string ScannersCount
        {
            get
            {
                return _scannersCount;
            }
            set
            {
                _scannersCount = value;
                OnPropertyChanged();
            }
        }
        public ScannersViewModel(HttpListener httpListener)
        {
            Thread scannersThread = new Thread(() =>
            {
                ActionList = new ObservableCollection<DeviceAction>();
                ScannersList = new ObservableCollection<Scanner>();
                ScannersCount = ScannersList.Count.ToString();
                while (true)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    context.Response.ContentType = "text/plain, charset=UTF-8";
                    context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                    context.Response.ContentEncoding = Encoding.UTF8;
                    System.Diagnostics.Trace.WriteLine("GETTING INFO");
                    switch (context.Request.RawUrl)
                    {
                        case "/zebrascannersHealth/":
                            using (var reader = new StreamReader(context.Request.InputStream))
                            {
                                if (ScannersList.Count < 1)
                                {
                                    var jobject = JObject.Parse(reader.ReadToEnd())["Zebra Scanners"];
                                    foreach (JToken item in jobject.Children())
                                    {
                                        try
                                        {
                                            var itemProperties = item.Children<JProperty>();
                                            JProperty[] field = new JProperty[4];
                                            for (int i = 0; i < 4; i++)
                                            {
                                                switch (i)
                                                {
                                                    case 0:
                                                        field[i] = itemProperties.FirstOrDefault(x => x.Name == "Error");
                                                        break;
                                                    case 1:
                                                        field[i] = itemProperties.FirstOrDefault(x => x.Name == "Scanner ID");
                                                        break;
                                                    case 2:
                                                        field[i] = itemProperties.FirstOrDefault(x => x.Name == "Scanner Type");
                                                        break;
                                                    case 3:
                                                        field[i] = itemProperties.FirstOrDefault(x => x.Name == "Serial number");
                                                        break;
                                                }
                                            }
                                            var myElement = itemProperties.FirstOrDefault(x => x.Name == "Serial number");
                                            ScannersList.Add(new ZebraScanner($"Zebra ID {field[3].Value}", new List<string>(), "Online", Enums.DeviceType.Scanner, field[2].Value.ToString()));
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                        finally
                                        {
                                            ScannersCount = ScannersList.Count.ToString();
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    context.Response.KeepAlive = false;
                    context.Response.Close();
                }
            });
            scannersThread.IsBackground = true;
            scannersThread.Start();
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
