using KioskLife.Core;
using KioskLife.MVVM.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KioskLife.MVVM.ViewModel
{
    class DashBoardViewModel : ObservableObject
    {
        public InfoActionViewModel infoActionViewModel { get; set; }

        public InfoCardViewModel infoCardViewModel { get; set; }

        public ObservableCollection<Device> Devices { get; set; }

        public DashBoardViewModel()
        {
            infoActionViewModel = new InfoActionViewModel();
            infoCardViewModel = new InfoCardViewModel();
            /*for (int i = 0; i < Devices.Count; i++)
            {
                Trace.WriteLine(Devices[i].GetType());
            }*/
        }
    }
}
