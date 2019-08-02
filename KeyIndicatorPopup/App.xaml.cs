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
        //see: https://www.codeproject.com/Articles/36468/WPF-NotifyIcon-2
        //see: https://github.com/hardcodet/wpf-notifyicon/blob/master/Hardcodet.NotifyIcon.Wpf/Source/Windowless%20Sample/App.xaml.cs

        /// <summary>
        /// TaskbarIcon that is used to display a popup whenever a key is pressed
        /// </summary>
        public TaskbarIcon NotifyIcon { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
#warning Add app to startup folder
            // Create a temporary theme control to load the saved theme. This is normally done while loading the main window. On startup only the notify icon is loaded and the window loads only if the user want's to.
            WindowTheme.WindowThemeUserControl tmpThemeControl = new WindowTheme.WindowThemeUserControl();
            tmpThemeControl.LoadSavedTheme();

            base.OnStartup(e);

            // Create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            NotifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotifyIcon?.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
