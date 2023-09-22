using KioskLife.Core;
using KioskLife.Enums;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Printer;
using KioskLife.MVVM.View;
using KioskLife.Screenshots;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace KioskLife.MVVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {

        public RelayCommand ExitProgram { get; set; }

        public RelayCommand HideProgram { get; set; }

        public RelayCommand MaximizeProgram { get; set; }

        public DashBoardViewModel DBVM { get; set; }

        public PrintersViewModel PVM { get; set; }

        public ScannersViewModel SVM { get; set; }

        public CamerasViewModel CVM { get; set; }

        public TerminalsViewModel TVM { get; set; }

        public DispensersViewModel DVM { get; set; }

        private string _activeButtonStyle;
        public string ActiveButtonStyle
        {
            get { return _activeButtonStyle; }
            set { _activeButtonStyle = value; }
        }


        private object _currentView;
        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private MenuButtonType _menuButtonType;

        #region MenuButtonsCommands
        public RelayCommand ShowDashboardsView { get; set; }

        public RelayCommand ShowPrintersView { get; set; }

        public RelayCommand ShowScannersView { get; set; }

        public RelayCommand ShowDispensersView { get; set; }

        public RelayCommand ShowTerminalsView { get; set; }

        public RelayCommand ShowCamerasView { get; set; }
        #endregion

        #region MenuButtonsStyles
        private Style _dashboardbuttonStyle;
        public Style DashboardbuttonStyle
        {
            get
            {
                return _dashboardbuttonStyle;
            }
            set
            {
                _dashboardbuttonStyle = value;
                OnPropertyChanged();
            }
        }

        private Style _printersbuttonStyle;
        public Style PrintersbuttonStyle
        {
            get
            {
                return _printersbuttonStyle;
            }
            set
            {
                _printersbuttonStyle = value;
                OnPropertyChanged();
            }
        }

        private Style _scannersbuttonStyle;
        public Style ScannersbuttonStyle
        {
            get
            {
                return _scannersbuttonStyle;
            }
            set
            {
                _scannersbuttonStyle = value;
                OnPropertyChanged();
            }
        }

        private Style _terminalsbuttonStyle;
        public Style TerminalsbuttonStyle
        {
            get
            {
                return _terminalsbuttonStyle;
            }
            set
            {
                _terminalsbuttonStyle = value;
                OnPropertyChanged();
            }
        }

        private Style _dispensersbuttonStyle;
        public Style DispensersbuttonStyle
        {
            get
            {
                return _dispensersbuttonStyle;
            }
            set
            {
                _dispensersbuttonStyle = value;
                OnPropertyChanged();
            }
        }

        private Style _camerasbuttonStyle;
        public Style CamerasbuttonStyle
        {
            get
            {
                return _camerasbuttonStyle;
            }
            set
            {
                _camerasbuttonStyle = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private static HttpListener _httpListener = new HttpListener();

        public MainWindowViewModel()
        {
            #region Start Local Server
            try
            {
                _httpListener.Prefixes.Add("http://localhost:7000/kiosklife/");
                _httpListener.Prefixes.Add("http://localhost:7000/dispensersHealth/");
                _httpListener.Prefixes.Add("http://localhost:7000/zebrascannersHealth/");
                _httpListener.Prefixes.Add("http://localhost:7000/bocaHealth/");
                _httpListener.Start();
            }
            catch(Exception e)
            {
                File.WriteAllText(@"C:\VReKiosk\Telenorma\KioskLifeAPP\serverlog.txt", e.Message + "\n");
                Application.Current.Shutdown();
            }
            #endregion

            #region DefaultButtons commands
            ExitProgram = new RelayCommand(o => { Application.Current.Shutdown(); });
            HideProgram = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            MaximizeProgram = new RelayCommand(o => 
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                else
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
            });
            #endregion

            DBVM = new DashBoardViewModel();
            TVM = new TerminalsViewModel();
            CVM = new CamerasViewModel();
            SVM = new ScannersViewModel(_httpListener);
            DVM = new DispensersViewModel(_httpListener);
            PVM = new PrintersViewModel();

            #region MenuButtons commands

            ShowDashboardsView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.DashboardButton;
                DashboardbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = DBVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });

            ShowPrintersView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.PrintersButton;
                PrintersbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = PVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });

            ShowScannersView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.ScannersButton;
                ScannersbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = SVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });

            ShowTerminalsView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.TerminalsButton;
                TerminalsbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = TVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });

            ShowCamerasView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.CamerasButton;
                CamerasbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = CVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });

            ShowDispensersView = new RelayCommand(o =>
            {
                switch (_menuButtonType)
                {
                    case MenuButtonType.DashboardButton:
                        DashboardbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.PrintersButton:
                        PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.ScannersButton:
                        ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.TerminalsButton:
                        TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.CamerasButton:
                        CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                    case MenuButtonType.DispensersButton:
                        DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
                        break;
                }
                _menuButtonType = MenuButtonType.DispensersButton;
                DispensersbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
                Thread commandThread = new Thread(() =>
                {
                    CurrentView = DVM;
                });
                commandThread.IsBackground = true;
                commandThread.Start();
            });
            #endregion

            DashboardbuttonStyle = Application.Current.FindResource("menuButtonActive") as Style;
            PrintersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
            ScannersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
            CamerasbuttonStyle = Application.Current.FindResource("menuButton") as Style;
            DispensersbuttonStyle = Application.Current.FindResource("menuButton") as Style;
            TerminalsbuttonStyle = Application.Current.FindResource("menuButton") as Style;
            ActiveButtonStyle = "menuButtonActive";
            CurrentView = DBVM;

            TimeSpan timeSpan = new TimeSpan(0, 5, 0);
            Thread screensShotThread = new Thread(() =>
            {
                Screenshot screenshot = new Screenshot();
                while (true)
                {
                    screenshot.DoScreenshot();
                    Thread.Sleep(timeSpan);
                }
            });
            screensShotThread.IsBackground = true;
            screensShotThread.Start();
            Thread reloadAPP = new Thread(ReloadAPP);
            reloadAPP.IsBackground = true;
            reloadAPP.Start();

        }

        /// <summary>
        /// This thread reload APP in special time
        /// </summary>
        private void ReloadAPP()
        {
            AppManager appManager = new AppManager();
            DateTime nextReloadTime;
            while (true)
            {
                try
                {
                    switch (DateTime.Now.Hour)
                    {
                        case 6:
                            if (appManager.FirstReload())
                            {
                                appManager.FirstReloadActivate();
                                App.Current.Shutdown();
                            }
                            else
                            {
                                nextReloadTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
                                TimeSpan timeUntilNextReload = nextReloadTime - DateTime.Now;
                                if (timeUntilNextReload.TotalMilliseconds > 0)
                                {
                                    Thread.Sleep(timeUntilNextReload);
                                }
                            }
                            break;
                        case 18:
                            if (appManager.SecondReload())
                            {
                                appManager.SecondReloadActivate();
                                App.Current.Shutdown();
                            }
                            else
                            {
                                nextReloadTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
                                TimeSpan timeUntilNextReload = nextReloadTime - DateTime.Now;
                                if (timeUntilNextReload.TotalMilliseconds > 0)
                                {
                                    Thread.Sleep(timeUntilNextReload);
                                }
                            }
                            break;
                        default:
                            if (!appManager.FirstReload())
                            {
                                nextReloadTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
                                TimeSpan timeUntilNextReload = nextReloadTime - DateTime.Now;
                                if (timeUntilNextReload.TotalMilliseconds > 0)
                                {
                                    Thread.Sleep(timeUntilNextReload);
                                }
                            }
                            else if (!appManager.SecondReload())
                            {
                                nextReloadTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
                                TimeSpan timeUntilNextReload = nextReloadTime - DateTime.Now;
                                if (timeUntilNextReload.TotalMilliseconds > 0)
                                {
                                    Thread.Sleep(timeUntilNextReload);
                                }
                            }
                            break;
                    }
                }
                catch(Exception ex) 
                {
                    Trace.WriteLine(ex.Message);
                }
            }

        }
    }
}
