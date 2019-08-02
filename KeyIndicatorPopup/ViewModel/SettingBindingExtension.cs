using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KeyIndicatorPopup.ViewModel
{
    /// <summary>
    /// Binding that can be used to bind to application settings in Properties.Settings.Default
    /// </summary>
    /// see: https://www.broculos.net/2014/01/wpf-how-to-bind-to-applications-settings.html
    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }

        public SettingBindingExtension(string path) : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
