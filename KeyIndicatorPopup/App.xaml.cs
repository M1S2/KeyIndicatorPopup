using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace KeyIndicatorPopup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// see: https://www.thomasclaudiushuber.com/2015/08/22/creating-a-background-application-with-wpf/
    public partial class App : Application
    {
        /*private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;
        private GlobalKeyboardHook _globalKeyboardHook;*/

        //see: https://www.codeproject.com/Articles/36468/WPF-NotifyIcon-2
        //see: https://github.com/hardcodet/wpf-notifyicon/blob/master/Hardcodet.NotifyIcon.Wpf/Source/Windowless%20Sample/App.xaml.cs
        private TaskbarIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            //MainWindow = new MainWindow();
            //MainWindow.Closing += MainWindow_Closing;

            //_globalKeyboardHook = new GlobalKeyboardHook();
            //_globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;

            //_notifyIcon = new System.Windows.Forms.NotifyIcon();
            //_notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            //_notifyIcon.Icon = KeyIndicatorPopup.Properties.Resources.AppIcon;
            //_notifyIcon.Visible = true;

            //CreateContextMenu();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        /*
        // see: https://stackoverflow.com/questions/17683620/c-sharp-actively-detect-lock-keys
        private void _globalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if(e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                _notifyIcon.BalloonTipTitle = "CAPS LOCK (" + e.KeyType.ToString() + ")";
                _notifyIcon.BalloonTipText = "The CAPS LOCK key was pressed";
                _notifyIcon.ShowBalloonTip(2000);
            }
            else if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VK_CAPITAL && !System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                _notifyIcon.BalloonTipTitle = "CAPS LOCK (" + e.KeyType.ToString() + ")";
                _notifyIcon.BalloonTipText = "The CAPS LOCK key was released";
                _notifyIcon.ShowBalloonTip(2000);
            }
            else
            {
                _notifyIcon.BalloonTipTitle = e.KeyType.ToString();
                _notifyIcon.BalloonTipText = "Keycode = " + e.KeyboardData.VirtualCode.ToString();
                _notifyIcon.ShowBalloonTip(2000);
            }
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _globalKeyboardHook?.Dispose();
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }*/
    }
}
