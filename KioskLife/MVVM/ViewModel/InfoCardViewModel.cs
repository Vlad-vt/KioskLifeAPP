using KioskLife.Core;

namespace KioskLife.MVVM.ViewModel
{
    class InfoCardViewModel : ObservableObject 
    {
        #region TotalUses
        private int totalUses;
		public int TotalUses
		{
			get { return totalUses; }
			set { totalUses = value; OnPropertyChanged(); }
		}
        #endregion

        #region TotalWarnings
        private int totalWarnings;
        public int TotalWarnings
        {
            get { return totalWarnings; }
            set { totalWarnings = value; OnPropertyChanged(); }
        }
        #endregion

        #region TotalErrors
        private int totalErrors;
        public int TotalErrors
        {
            get { return totalErrors; }
            set { totalErrors = value; OnPropertyChanged(); }
        }
        #endregion

        


        public InfoCardViewModel()
        {
            TotalUses = 30;
            TotalWarnings = 2;
            TotalErrors = 1;
        }

    }
}
