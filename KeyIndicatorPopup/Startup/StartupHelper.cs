using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IWshRuntimeLibrary;

namespace KeyIndicatorPopup.Startup
{
    public static class StartupHelper
    {
        #region INotifyPropertyChanged implementation
        /// <summary>
        /// Raised when a property has a new value.
        /// </summary>
        /// see: http://10rem.net/blog/2011/11/29/wpf-45-binding-and-change-notification-for-static-properties
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        /// <summary>
        /// This method is called by the Set accessor of each property. The CallerMemberName attribute that is applied to the optional propertyName parameter causes the property name of the caller to be substituted as an argument.
        /// </summary>
        /// <param name="propertyName">Name of the property that is changed</param>
        /// see: https://docs.microsoft.com/de-de/dotnet/framework/winforms/how-to-implement-the-inotifypropertychanged-interface
        public static void OnStaticPropertyChanged([CallerMemberName] string propertyName = "")
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        //####################################################################################################################################################################################################
        
        public static string StartUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string AppName = Application.ResourceAssembly.GetName().Name;

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Check if the current app is in the startup folder
        /// </summary>
        /// <returns>true if app is in startup folder; otherwise false</returns>
        public static bool IsAppInStartup
        {
            get { return System.IO.File.Exists(StartUpFolderPath + "\\" + AppName + ".lnk"); }
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Add the current app to the startup folder
        /// </summary>
        /// see: https://stackoverflow.com/questions/3391923/placing-a-shortcut-in-users-startup-folder-to-start-with-windows
        public static void AddAppToStartup()
        {
            if(IsAppInStartup) { return; }

            WshShell wshShell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(StartUpFolderPath + "\\" + AppName + ".lnk");

            shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(shortcut.TargetPath);
            shortcut.Description = "Launch " + AppName;
            shortcut.Save();

            OnStaticPropertyChanged("IsAppInStartup");
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Remove the current app from the startup folder
        /// </summary>
        public static void RemoveAppFromStartup()
        {
            if (!IsAppInStartup) { return; }

            System.IO.File.Delete(StartUpFolderPath + "\\" + AppName + ".lnk");

            OnStaticPropertyChanged("IsAppInStartup");
        }

    }
}
