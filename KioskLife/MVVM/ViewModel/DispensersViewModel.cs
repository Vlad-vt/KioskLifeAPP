using KioskLife.Core;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Dispenser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskLife.MVVM.ViewModel
{
    class DispensersViewModel : ObservableObject
    {
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
        public DispensersViewModel()
        {
            DispensersList = new ObservableCollection<Dispenser>
            {
                new Dispenser("COM1", new List<string>{"Paper Jam", "Cutter Jam", "AAA"}, new List<string>{"0 warnings"}),
                new Dispenser("COM2", new List<string>{"Paper Jam"}, new List<string>{"0 warnings"}),
            };
        }
    }
}
