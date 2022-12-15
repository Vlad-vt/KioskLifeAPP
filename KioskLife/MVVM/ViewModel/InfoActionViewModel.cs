using KioskLife.Core;
using System;

namespace KioskLife.MVVM.ViewModel
{
    class InfoActionViewModel : ObservableObject
    {
        private string description;
        public string Description
        {
            get { return description; }
            set 
            {
                description = value;
                if (description.Length > 27)
                {
                    description = description.Substring(0, 27);
                    description += "...";
                }
                OnPropertyChanged();
            }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(); }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(); }
        }

        public InfoActionViewModel()
        {
            Icon = "Printer";
            Date = "[" + DateTime.Now.ToString() + "]:  ";
            //Description = "Printer started working";
            Description = "Zebra scanner started working";
        }
    }
}
