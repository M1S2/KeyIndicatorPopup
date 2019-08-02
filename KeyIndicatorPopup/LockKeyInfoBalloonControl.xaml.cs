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

namespace KeyIndicatorPopup
{
    /// <summary>
    /// Interaction logic for LockKeyInfoBalloonControl.xaml
    /// </summary>
    public partial class LockKeyInfoBalloonControl : UserControl
    {
        public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register("IsLocked", typeof(bool), typeof(LockKeyInfoBalloonControl), new PropertyMetadata(false));
        public bool IsLocked
        {
            get { return (bool)this.GetValue(IsLockedProperty); }
            set { this.SetValue(IsLockedProperty, value); }
        }

        public static readonly DependencyProperty LockKeyTextProperty = DependencyProperty.Register("LockKeyText", typeof(string), typeof(LockKeyInfoBalloonControl), new PropertyMetadata(""));
        public string LockKeyText
        {
            get { return (string)this.GetValue(LockKeyTextProperty); }
            set { this.SetValue(LockKeyTextProperty, value); }
        }

        public LockKeyInfoBalloonControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
