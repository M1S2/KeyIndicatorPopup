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
using KeyIndicatorPopup.Keyboard;
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

        //####################################################################################################################################################################################################

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

        //####################################################################################################################################################################################################

        private GlobalKeyboardHook _globalKeyboardHook;
        private TaskbarIcon _notifyIcon;
        private InfoBalloonControl _infoBalloon;
        private LockKeyInfoBalloonControl _lockInfoBalloon;

        //****************************************************************************************************************************************************************************************************

        public NotifyIconViewModel()
        {
            _infoBalloon = new InfoBalloonControl();
            _lockInfoBalloon = new LockKeyInfoBalloonControl();

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
        }

        //****************************************************************************************************************************************************************************************************

        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (_notifyIcon == null) { _notifyIcon = ((App)Application.Current).NotifyIcon; }

            if (e.KeyType == KeyTypes.Lock && Properties.Settings.Default.ShowLockKeys && (e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown))    // using key down because key up doesn't work for NUM and SCROLL lock keys. Negate all states is neccessary.
            {
                if (e.KeyboardData.VirtualCode == VirtualKeyCodes.VK_CAPITAL)        //CAPS Lock
                {
                    _lockInfoBalloon.IsLocked = !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
                    _lockInfoBalloon.LockKeyText = _lockInfoBalloon.IsLocked ? "A" : "a";
                }
                else if (e.KeyboardData.VirtualCode == VirtualKeyCodes.VK_NUMLOCK)  //NUM Lock
                {
                    _lockInfoBalloon.LockKeyText = "1";
                    _lockInfoBalloon.IsLocked = !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock);
                }
                else if (e.KeyboardData.VirtualCode == VirtualKeyCodes.VK_SCROLL)  //SCROLL Lock
                {
                    _lockInfoBalloon.LockKeyText = "R";
                    _lockInfoBalloon.IsLocked = !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.Scroll);
                }

                _notifyIcon.ShowCustomBalloon(_lockInfoBalloon, System.Windows.Controls.Primitives.PopupAnimation.Fade, 2000);
            }
            else if(e.KeyType != KeyTypes.Lock && (e.KeyboardState == KeyboardState.KeyUp || e.KeyboardState == KeyboardState.SysKeyUp))
            {
                if (e.KeyType == KeyTypes.Letter && Properties.Settings.Default.ShowLetterKeys || e.KeyType == KeyTypes.Numeric && Properties.Settings.Default.ShowNumericKeys || e.KeyType == KeyTypes.System && Properties.Settings.Default.ShowSystemKeys)
                {
                    _infoBalloon.TitleText = e.KeyType.ToString();
                    _infoBalloon.InfoText = e.KeyName;

                    _notifyIcon.ShowCustomBalloon(_infoBalloon, System.Windows.Controls.Primitives.PopupAnimation.Fade, 2000);
                }
            }
        }
    }
}
