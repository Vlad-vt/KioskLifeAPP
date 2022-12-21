using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KioskLife.MVVM.View.Small
{
    /// <summary>
    /// Логика взаимодействия для CamerasCard.xaml
    /// </summary>
    public partial class CamerasCard : UserControl
    {
        public CamerasCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CamerasCard));

        public string Resolution
        {
            get
            {
                return (string)GetValue(ResolutionProperty);
            }
            set
            {
                SetValue(ResolutionProperty, value);
            }
        }
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(string), typeof(CamerasCard));

        public string Number
        {
            get
            {
                return (string)GetValue(NumberProperty);
            }
            set
            {
                SetValue(NumberProperty, value);
            }
        }
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(string), typeof(CamerasCard));

        public string Icon
        {
            get
            {
                return (string)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(CamerasCard));

        public Color Background1
        {
            get
            {
                return (Color)GetValue(Background1Property);
            }
            set
            {
                SetValue(Background1Property, value);
            }
        }
        public static readonly DependencyProperty Background1Property = DependencyProperty.Register("Background1", typeof(Color), typeof(CamerasCard));

        public Color Background2
        {
            get
            {
                return (Color)GetValue(Background2Property);
            }
            set
            {
                SetValue(Background2Property, value);
            }
        }
        public static readonly DependencyProperty Background2Property = DependencyProperty.Register("Background2", typeof(Color), typeof(CamerasCard));

        public Color EllipseBackground1
        {
            get
            {
                return (Color)GetValue(EllipseBackground1Property);
            }
            set
            {
                SetValue(EllipseBackground1Property, value);
            }
        }
        public static readonly DependencyProperty EllipseBackground1Property = DependencyProperty.Register("EllipseBackground1", typeof(Color), typeof(CamerasCard));

        public Color EllipseBackground2
        {
            get
            {
                return (Color)GetValue(EllipseBackground2Property);
            }
            set
            {
                SetValue(EllipseBackground2Property, value);
            }
        }
        public static readonly DependencyProperty EllipseBackground2Property = DependencyProperty.Register("EllipseBackground2", typeof(Color), typeof(CamerasCard));
    }
}
