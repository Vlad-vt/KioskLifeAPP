using KioskLife.Core;

namespace KioskLife.MVVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        public InfoActionViewModel infoActionViewModel { get; set; }

        public InfoCardViewModel infoCardViewModel { get; set; }

        public MainWindowViewModel()
        {
            infoActionViewModel= new InfoActionViewModel();
            infoCardViewModel= new InfoCardViewModel();

        }
    }
}
