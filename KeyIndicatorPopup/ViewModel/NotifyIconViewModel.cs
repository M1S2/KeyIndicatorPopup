using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                return new GalaSoft.MvvmLight.Command.RelayCommand(
                    () => { Application.Current.MainWindow = new MainWindow(); Application.Current.MainWindow.Show(); },
                    () => Application.Current.MainWindow == null);
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new GalaSoft.MvvmLight.Command.RelayCommand(
                    () => Application.Current.MainWindow.Close(),
                    () => Application.Current.MainWindow != null);
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new GalaSoft.MvvmLight.Command.RelayCommand(
                    () => Application.Current.Shutdown());
            }
        }

        #endregion

        private string _pressedKeyName;
        public string PressedKeyName
        {
            get { return _pressedKeyName; }
            set { _pressedKeyName = value;  OnPropertyChanged(); }
        }

        private GlobalKeyboardHook _globalKeyboardHook;
        private ResourceDictionary _notifyResourceDict = new ResourceDictionary();
        private TaskbarIcon _notifyIcon;

        public NotifyIconViewModel()
        {
            _notifyResourceDict.Source = new Uri("/KeyIndicatorPopup;component/NotifyIconResources.xaml", UriKind.RelativeOrAbsolute);
            //_notifyIcon = _notifyResourceDict["NotifyIcon"] as TaskbarIcon;

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
        }

        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                if (_notifyIcon == null) { _notifyIcon = _notifyResourceDict["NotifyIcon"] as TaskbarIcon; }
                UIElement infoBalloon = _notifyResourceDict["InfoBalloon"] as System.Windows.Controls.UserControl;

                PressedKeyName = e.KeyboardData.VirtualCode.ToString();

                if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
                {
                    //_notifyIcon.ShowBalloonTip("CAPS LOCK (" + e.KeyType.ToString() + ")", "The CAPS LOCK key was pressed", BalloonIcon.Info);
                    //infoBalloon.Text = "CAPS LOCK (" + e.KeyType.ToString() + ")  " + "The CAPS LOCK key was pressed";
                }
                else if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
                {
                    //_notifyIcon.ShowBalloonTip("CAPS LOCK (" + e.KeyType.ToString() + ")", "The CAPS LOCK key was released", BalloonIcon.Info);
                    //infoBalloon.Text = "CAPS LOCK (" + e.KeyType.ToString() + ")  " + "The CAPS LOCK key was released";
                }
                else
                {
                    //_notifyIcon.ShowBalloonTip(e.KeyType.ToString(), "Keycode = " + e.KeyboardData.VirtualCode.ToString(), BalloonIcon.Info);
                    //infoBalloon.Text = e.KeyType.ToString() + " Keycode = " + e.KeyboardData.VirtualCode.ToString();
                }

                _notifyIcon.ShowCustomBalloon(infoBalloon, System.Windows.Controls.Primitives.PopupAnimation.Fade, 2000);
            }
        }
    }
}
