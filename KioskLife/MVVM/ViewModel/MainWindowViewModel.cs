﻿using KioskLife.Core;
using KioskLife.Enums;
using KioskLife.MVVM.Model;
using KioskLife.MVVM.Model.Printer;
using KioskLife.MVVM.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public MainWindowViewModel()
        {
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
            SVM = new ScannersViewModel();
            DVM = new DispensersViewModel();
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
            //Devices.Add(new NetworkPrinter());    
           // Devices.Add(new USBPrinter());
        }
    }
}