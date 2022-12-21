using KioskLife.Core;
using KioskLife.MVVM.Model.Dispenser;
using KioskLife.MVVM.Model.Scanner;
using KioskLife.MVVM.Model.Scanner.Zebra;
using System.Collections.ObjectModel;

namespace KioskLife.MVVM.ViewModel
{
    class ScannersViewModel : ObservableObject
    {
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
        public ScannersViewModel()
        {
            ScannersList = new ObservableCollection<Scanner>
            {
                new ZebraScanner("Zebra Scanner", new System.Collections.Generic.List<string>{"Paper Jam"})
            };
        }
    }
}
