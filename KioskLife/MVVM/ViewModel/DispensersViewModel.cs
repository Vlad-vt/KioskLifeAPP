using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner.Zebra;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KioskLife.MVVM.ViewModel
{
    class DispensersViewModel : ObservableObject
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

        private ObservableCollection<Dispenser> _dispensersList;
        public ObservableCollection<Dispenser> DispensersList
        {
            get
            {
                return _dispensersList;
            }
            set
            {
                _dispensersList = value;
                OnPropertyChanged();
            }
        }

        private string _dispensersCount;

        public string DispensersCount
        {
            get { return _dispensersCount; }
            set { _dispensersCount = value; OnPropertyChanged(); }
        }

        public DispensersViewModel(HttpListener httpListener)
        {
            System.Diagnostics.Trace.WriteLine("START WORK");
            Thread dispensersThread = new Thread(() =>
            {
                ActionList = new ObservableCollection<DeviceAction>();
                DispensersList = new ObservableCollection<Dispenser>();
                DispensersCount = DispensersList.Count.ToString();
                while (true)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    context.Response.ContentType = "text/plain, charset=UTF-8";
                    context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                    context.Response.ContentEncoding = Encoding.UTF8;
                    System.Diagnostics.Trace.WriteLine("GETTING INFO");
                    switch (context.Request.RawUrl)
                    {
                        case "/dispensersHealth/":
                            using (var reader = new StreamReader(context.Request.InputStream))
                            {/*
                                   // if (ScannersList.Count < 1)
                                    //{
                                        var jobject = JObject.Parse(reader.ReadToEnd())["Zebra Scanners"];
                                        int number = 0;
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
                                              //  if (ScannersList.Count == 0)
                                                //{
                                                //    ScannersList.Add(new ZebraScanner($"Zebra ID {field[3].Value}", new List<string>(), "Online", Enums.DeviceType.Scanner, field[2].Value.ToString()));
                                                //}
                                                //else
                                                //{
                                                 //   if (ScannersList[number].Err)
                                          //  }
                                            //    number++;
                                              //  ScannersCount = ScannersList.Count.ToString();
                                            }
                                            catch (Exception e)
                                            {

                                            }
                                        }*/
                                    
                                }
                            break;
                    }
                    context.Response.KeepAlive = false;
                    context.Response.Close();
                }
            });
            dispensersThread.IsBackground = true;
            dispensersThread.Start();
        }

        private void NewAction(string action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ActionList.Insert(0, new DeviceAction(action, "[" + DateTime.Now.ToString() + "]:  ", "SIM"));
            });
        }
    }
}
