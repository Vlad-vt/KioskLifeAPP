   M S B A M L   `   `   `  ����D  @KioskLife, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:KioskLife	KioskLife  5   	   C&clr-namespace:KioskLife.MVVM.ViewModelKioskLife.MVVM.ViewModel  5   	   9!clr-namespace:KioskLife.MVVM.ViewKioskLife.MVVM.View  5	   	       KioskLife.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:KioskLife     5   	   <	viewModel&clr-namespace:KioskLife.MVVM.ViewModel     5   	   _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   *uc!clr-namespace:KioskLife.MVVM.View  5	   	   -   5          � Title$  
MainWindow��5   	   $	��740��6   $
��1280��6)    � WindowStartupLocation$ CenterScreen=�66    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��6   
������  � WindowState$ 	Minimized=�6�    � DataContext 5      2  ,KioskLife.MVVM.ViewModel.MainWindowViewModel  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��* �6   .+�5   
   �� $��2��6    �� CornerRadius$ 	10,0,0,10��6"   ��5      �� 5      	 �� 
StartPoint$	 0.5,0&�6&   
 �� EndPoint$
 0.5,1&�69   .��5      �  � Color$ #223266��6#    � Offset$ 0Q�63   � 5      $ #27396b��6#   $ 1Q�63   5      ._�5!      �� .+�5#      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   $��0, 35, 0, 0q�6S   .+�5$      �� $ 10��6   $��40��6/   $��40��6:   ��5%      �� 5&      $	 0.5,0&�62   $
 0.5,1&�6E   .��5'   "   � $ #7f7cf1��6/   $ 0Q�6?   � 5(   "   $ #d3b7ff��6/   $ 1Q�6?   5)      ._�5+      �� $��KL��6$   ���#ffff$��22(�6A   $��SemiBold"�6O    �� TextAlignment$ Center=�5,   $   $��Center=�6;   $��3,0,0,2q�6V   5-      �� 5.      $��10,0,0,0q�6!   $��Center=�63   .+�5/      �� $��KioskLife APP��6$   $��15(�69   $��0,0,0,5q�6G   ���#c2cfff6X   �� 50      $��
Beta 2.0.0��6$   $��11(�66   ���#7788bf6D   51      52      �� 53      $	��0.8��6   $��20,30q�6)   ���#46558a68   �� 56      ��6   �� *ShowDashboardsView+��6A   �� *DashboardbuttonStyle+.��57      �� $ 
Horizontal=�6!   .+�58      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ MonitorDashboard=�6&      menuButtonIcon#��[  6>   �� 59      $��	Dashboard��6$     menuButtonText#��[ 65   5:      5;      �� 5=      ��6   �� *ShowPrintersView+��6>   �� *PrintersbuttonStyle+.��5>      �� $ 
Horizontal=�6!   .+�5?        $ Printer=�6&   #��[  65   �� 5@      $��Printers��6$   #��[ 64   5A      5B      �� 5D      ��6   �� *ShowScannersView+��6>   �� *ScannersbuttonStyle+.��5E      �� $ 
Horizontal=�6!   .+�5F        $ Scanner=�6&   #��[  65   �� 5G      $��Scanners��6$   #��[ 64   5H      5I      �� 5K      ��6   �� *ShowDispensersView+��6A   �� *DispensersbuttonStyle+.��5L      �� $ 
Horizontal=�6!   .+�5M        $	 Sim=�6&   #��[  61   �� 5N      $��
Dispensers��6$   #��[ 66   5O      5P      �� 5Q      ��6   �� *ShowCamerasView+��6>   �� *CamerasbuttonStyle+.��5R      �� $ 
Horizontal=�6!   .+�5S        $ Camera=�6&   #��[  64   �� 5T      $��Cameras��6$   #��[ 63   5U      5V      �� 5W      ��6   �� *ShowTerminalsView+��6@   �� *TerminalsbuttonStyle+.��5X      �� $ 
Horizontal=�6!   .+�5Y        $ Payment=�6&   #��[  65   �� 5Z      $��	Terminals��6$   #��[ 65   5[      5\      �� 5]        
menuButton#��[ 6   .��5^      �� $ 
Horizontal=�6!   .+�5_        $ Settings=�6&   #��[  66   �� 5`      $��Settings��6$   #��[ 64   5a      5b      5c      5d      �� 5g   
   $��1��6   $��0��6!   $��2��6.   $ 	0,10,10,0��6@   ��5h      �� 5i      $		 1,0&�6&   $	
 0,1&�67   .��5j      � $ #41518f��6#   $ 0Q�63   � 5k      $ #2c386c��6#   $ 1Q�63   5l      5n      �� 5o   
   $��1��6   $��0��6%   $ 
Horizontal=�63   $��Right=�6L   .+�5p      ��  , (MaterialDesignFloatingActionAccentButton#��[ 65   $��25��6w   $��25��6�   
������� � Cursor$
 Hand��6�   $��0,-1,0,0q�6�   ��6   �� *HideProgram+��6�   Z� ��6�   Z� .��5q        $ WindowMinimize=�6   5r      �� 5s      #��[ 69   $��25��6{   $��25��6�   
�������$
 Hand��6�   $��0,-1,0,0q�6�   ��6   �� *MaximizeProgram+��6�   Z� ��6�   Z� .��5t        $ WindowMaximize=�6   5u      �� 5v       0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 65   $��25��6{   $��25��6�   
�������$
 Hand��6�   $��0,-1,3,0q�6�   ��6   �� *ExitProgram+��6�   Z� ��6�   Z� .��5w        $ 	CloseBold=�6   5x      5y      �� 5z   
   $��1��69   $��1��6I   ��6   �� *CurrentView+6I   5{      5|      