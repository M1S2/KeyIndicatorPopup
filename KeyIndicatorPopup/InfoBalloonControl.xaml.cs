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
using KeyIndicatorPopup.Keyboard;

namespace KeyIndicatorPopup
{
    /// <summary>
    /// Interaction logic for InfoBalloonControl.xaml
    /// </summary>
    public partial class InfoBalloonControl : UserControl
    {
        public static readonly DependencyProperty InfoTextProperty = DependencyProperty.Register("InfoText", typeof(string), typeof(InfoBalloonControl), new PropertyMetadata(""));
        public string InfoText
        {
            get { return (string)this.GetValue(InfoTextProperty); }
            set { this.SetValue(InfoTextProperty, value); }
        }

        public static readonly DependencyProperty KeyTypeProperty = DependencyProperty.Register("KeyType", typeof(KeyTypes), typeof(InfoBalloonControl), new PropertyMetadata(KeyTypes.Lock));
        public KeyTypes KeyType
        {
            get { return (KeyTypes)this.GetValue(KeyTypeProperty); }
            set { this.SetValue(KeyTypeProperty, value); }
        }

        public InfoBalloonControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
