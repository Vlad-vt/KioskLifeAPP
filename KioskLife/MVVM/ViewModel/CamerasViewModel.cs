using KioskLife.Core;
using KioskLife.MVVM.Model.Camera;
using KioskLife.MVVM.Model.Dispenser;
using System.Collections.ObjectModel;

namespace KioskLife.MVVM.ViewModel
{
    class CamerasViewModel : ObservableObject
    {
        private ObservableCollection<Camera> _camerasList;
        public ObservableCollection<Camera> CamerasList
        {
            get
            {
                return _camerasList;
            }
            set
            {
                _camerasList = value;
                OnPropertyChanged();
            }
        }
        public CamerasViewModel()
        {
            CamerasList = new ObservableCollection<Camera>
            {
                new Camera(true)
            };
        }
    }
}
