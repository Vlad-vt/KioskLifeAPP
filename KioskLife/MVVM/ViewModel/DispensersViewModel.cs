using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner.Zebra;
using Newtonsoft.Json;
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
                DispensersList = new ObservableCollection<Dispenser>();
                DispensersCount = DispensersList.Count.ToString();
                while (true)
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
                                //DispensersHealth dispensersHealth = JsonConvert.DeserializeObject<DispensersHealth>(reader.ReadToEnd());
                                Application.Current.Dispatcher.Invoke(() => DispensersCount = DispensersList.Count.ToString());
                                MessageBox.Show(reader.ReadToEnd());
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
                ActionList.Add(new DeviceAction(action, "[" + DateTime.Now.ToString() + "]:  ", "SIM"));
            });
        }
    }
}
