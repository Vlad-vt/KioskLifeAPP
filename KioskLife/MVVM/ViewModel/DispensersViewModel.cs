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
            Thread dispensersThread = new Thread(() =>
            {
                ActionList = new ObservableCollection<DeviceAction>();
                DispensersList = new ObservableCollection<Dispenser>();
                DispensersCount = DispensersList.Count.ToString();
                while (true)
                {
                    try
                    {
                        HttpListenerContext context = httpListener.GetContext();
                        context.Response.ContentType = "text/plain, charset=UTF-8";
                        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                        context.Response.ContentEncoding = Encoding.UTF8;
                        switch (context.Request.RawUrl)
                        {
                            case "/dispensersHealth/":
                                using (var reader = new StreamReader(context.Request.InputStream))
                                {
                                    if (DispensersList.Count < 1)
                                    {
                                        var jobject = JObject.Parse(reader.ReadToEnd())["Dispensers"];
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
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserCOM");
                                                            break;
                                                        case 1:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserErrors");
                                                            break;
                                                        case 2:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserWarnings");
                                                            break;
                                                        case 3:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserCardLocation");
                                                            break;
                                                    }
                                                }
                                                var myElement = itemProperties.FirstOrDefault(x => x.Name == "Serial number");
                                                DispensersList.Add(new Dispenser($"{field[0].Value}", new List<string>(field[1].Value.ToString().Split(',')), new List<string>(field[2].Value.ToString().Split(',')), "Online", Enums.DeviceType.Dispenser));
                                                DispensersCount = DispensersList.Count.ToString();
                                            }
                                            catch (Exception e)
                                            {

                                            }
                                        }
                                        foreach (Dispenser dispenser in DispensersList)
                                        {
                                            System.Diagnostics.Trace.WriteLine(dispenser.Errors);
                                        }
                                    }
                                    else
                                    {
                                        var jobject = JObject.Parse(reader.ReadToEnd())["Dispensers"];
                                        List<Dispenser> tempList = new List<Dispenser>();
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
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserCOM");
                                                            break;
                                                        case 1:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserErrors");
                                                            break;
                                                        case 2:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserWarnings");
                                                            break;
                                                        case 3:
                                                            field[i] = itemProperties.FirstOrDefault(x => x.Name == "DispenserCardLocation");
                                                            break;
                                                    }
                                                }
                                                var myElement = itemProperties.FirstOrDefault(x => x.Name == "Serial number");
                                                tempList.Add(new Dispenser($"{field[0].Value}", new List<string>(field[1].Value.ToString().Split(',')), new List<string>(field[2].Value.ToString().Split(',')), "Online", Enums.DeviceType.Dispenser));
                                            }
                                            catch { };
                                        }
                                        if (DispensersList.Count == tempList.Count)
                                        {
                                            for (int i = 0; i < DispensersList.Count; i++)
                                            {
                                                DispensersList[i].CheckChanges(tempList[i]);
                                            }
                                        }
                                        DispensersCount = DispensersList.Count.ToString();
                                    }
                                }
                                break;
                        }
                        context.Response.KeepAlive = false;
                        context.Response.Close();
                    }
                    catch (Exception ex) { }

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
