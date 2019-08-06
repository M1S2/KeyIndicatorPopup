using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KeyIndicatorPopup.WindowTheme;

namespace KeyIndicatorPopup.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is called by the Set accessor of each property. The CallerMemberName attribute that is applied to the optional propertyName parameter causes the property name of the caller to be substituted as an argument.
        /// </summary>
        /// <param name="propertyName">Name of the property that is changed</param>
        /// see: https://docs.microsoft.com/de-de/dotnet/framework/winforms/how-to-implement-the-inotifypropertychanged-interface
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        //####################################################################################################################################################################################################

        #region Commands

        /// <summary>
        /// Command that is used to add or remove the app to/from autostart. If command parameter is true add to autostart. Otherwise remove.
        /// </summary>
        public ICommand AutostartToggleCommand
        {
            get
            {
                return new RelayCommand(
                    (c) =>
                    {
                        if ((bool)c == true) { Startup.StartupHelper.AddAppToStartup(); }
                        else { Startup.StartupHelper.RemoveAppFromStartup(); }
                    });
            }
        }

        #endregion

        //####################################################################################################################################################################################################

        public MainViewModel()
        {

        }

    }
}
