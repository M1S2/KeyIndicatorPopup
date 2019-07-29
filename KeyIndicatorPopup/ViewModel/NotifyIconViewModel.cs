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
using Hardcodet.Wpf.TaskbarNotification;

namespace KeyIndicatorPopup.ViewModel
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    /// see: https://github.com/hardcodet/wpf-notifyicon/blob/master/Hardcodet.NotifyIcon.Wpf/Source/Windowless%20Sample/NotifyIconViewModel.cs
    /// see: https://github.com/hardcodet/wpf-notifyicon/tree/master/Hardcodet.NotifyIcon.Wpf/Source/Sample%20Project/Tutorials/09%20-%20MVVM
    public class NotifyIconViewModel
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

        #region Commands

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new RelayCommand(
                    (c) => { Application.Current.MainWindow = new MainWindow(); Application.Current.MainWindow.Show(); },
                    (c) => Application.Current.MainWindow == null);
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new RelayCommand(
                    (c) => Application.Current.MainWindow.Close(),
                    (c) => Application.Current.MainWindow != null);
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new RelayCommand(
                    (c) => Application.Current.Shutdown());
            }
        }

        #endregion

        private GlobalKeyboardHook _globalKeyboardHook;
        //private ResourceDictionary _notifyResourceDict = new ResourceDictionary();
        private TaskbarIcon _notifyIcon;
        private InfoBalloonControl _infoBalloon;

        public NotifyIconViewModel()
        {
            //_notifyResourceDict.Source = new Uri("/KeyIndicatorPopup;component/NotifyIconResources.xaml", UriKind.RelativeOrAbsolute);

            _infoBalloon = new InfoBalloonControl();

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
        }

        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                _notifyIcon = ((App)Application.Current).NotifyIcon;
                _infoBalloon.TitleText = e.KeyType.ToString();

                //infoBalloon = _notifyResourceDict["InfoBalloon"] as InfoBalloonControl;

                if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))        //CAPS Lock enabled
                {
                    _infoBalloon.InfoText = "ABC";
                }
                else if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))  //CAPS Lock disabled
                {
                    _infoBalloon.InfoText = "abc";
                }
                else if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_NUMLOCK && System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock))  //NUM Lock enabled
                {
                    _infoBalloon.InfoText = "123";
                }
                else if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_NUMLOCK && !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock))  //NUM Lock disabled
                {
                    _infoBalloon.InfoText = "NUM off";
                }
                else
                {
                    _infoBalloon.InfoText = char.ToString(Convert.ToChar(e.KeyboardData.VirtualCode));
                }

                _notifyIcon.ShowCustomBalloon(_infoBalloon, System.Windows.Controls.Primitives.PopupAnimation.Fade, 2000);
            }
        }
    }
}
