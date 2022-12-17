using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Printer;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KioskLife.MVVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        public InfoActionViewModel infoActionViewModel { get; set; }

        public InfoCardViewModel infoCardViewModel { get; set; }

        public ObservableCollection<Device> Devices { get; set; }

        public MainWindowViewModel()
        {
            infoActionViewModel= new InfoActionViewModel();
            infoCardViewModel= new InfoCardViewModel();
            Devices = new ObservableCollection<Device>();
            Devices.Add(new NetworkPrinter());
            Devices.Add(new USBPrinter());
            for (int i = 0; i < Devices.Count; i++)
            {
                Trace.WriteLine(Devices[i].GetType());
            }
        }
    }
}
