using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Terminal;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace KioskLife.MVVM.ViewModel
{
    class TerminalsViewModel : ObservableObject
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

        private ObservableCollection<Terminal> myVar;

        public ObservableCollection<Terminal> TerminalsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private void TerminalAction(string action)
        {
            Application.Current.Dispatcher.Invoke(() => ActionList.Add(new DeviceAction(action, "[" + DateTime.Now.ToString() + "]", "Payment")));
        }

        public TerminalsViewModel()
        {
            Thread terminalThread = new Thread(() =>
            {
                try
                {
                    ActionList = new ObservableCollection<DeviceAction>();
                    TerminalsList = new ObservableCollection<Terminal>
                    { 
                        new Terminal("", new System.Collections.Generic.List<string>(), "Online", Enums.DeviceType.Terminal),
                    };
                    for (int i = 0; i < TerminalsList.Count; i++)
                    {
                        TerminalsList[i].Action += TerminalAction;
                        TerminalsList[i].GetNetworkData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                while(true)
                {
                    for (int i = 0; i < TerminalsList.Count; i++)
                    {
                        TerminalsList[i].CheckForErrors();
                        TerminalsList[i].CheckDeviceConnection();
                    }
                    Thread.Sleep(5000);
                }
            });
            terminalThread.IsBackground = true;
            terminalThread.Start();
        }
    }
}
