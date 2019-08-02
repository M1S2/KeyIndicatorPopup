using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyIndicatorPopup.Keyboard
{
    //see: https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application

    //Based on https://gist.github.com/Stasonix
    class GlobalKeyboardHook : IDisposable
    {
        #region DLL Imports

        //http://pinvoke.net/default.aspx/user32/MapVirtualKey.html?diff=y
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyEx(uint uCode, MapVirtualKeyMapTypes uMapType, IntPtr dwhkl);
        /// <summary>
        /// The MapVirtualKey function translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code
        /// </summary>
        /// <param name="uCode">[in] Specifies the virtual-key code or scan code for a key. How this value is interpreted depends on the value of the uMapType parameter.</param>
        /// <param name="uMapType">[in] Specifies the translation to perform. The value of this parameter depends on the value of the uCode parameter.</param>
        /// <returns>Either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType. If there is no translation, the return value is zero.</returns>
        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //https://www.pinvoke.net/default.aspx/user32.getkeynametext
        /// <summary>
        /// Retrieves a string that represents the name of a key.
        /// </summary>
        /// <param name="lParam">The second parameter of the keyboard message (such as WM_KEYDOWN) to be processed. </param>
        /// <param name="lpString">The buffer that will receive the key name.</param>
        /// <param name="nSize">The maximum length, in characters, of the key name, including the terminating null character. (This parameter should be equal to the size of the buffer pointed to by the lpString parameter.)</param>
        /// <returns>If the function succeeds, a null-terminated string is copied into the specified buffer, and the return value is the length of the string, in characters, not counting the terminating null character. If the function fails, the return value is zero.</returns>
        [DllImport("user32.dll")]
        public static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString, int nSize);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lpfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">handle to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        #endregion

        //####################################################################################################################################################################################################

        /// <summary>
        /// Event that is raised whenever a key on the keyboard is pressed
        /// </summary>
        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

        //####################################################################################################################################################################################################

        private IntPtr _windowsHookHandle;
        private IntPtr _user32LibraryHandle;
        private HookProc _hookProc;

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public const int WH_KEYBOARD_LL = 13;

        //####################################################################################################################################################################################################

        /// <summary>
        /// Construct a new GlobalKeyboardHook class and install a keyboard hook
        /// </summary>
        public GlobalKeyboardHook()
        {
            _windowsHookHandle = IntPtr.Zero;
            _user32LibraryHandle = IntPtr.Zero;
            _hookProc = LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            _user32LibraryHandle = LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            _windowsHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
            if (_windowsHookHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Dispose and remove the keyboard hook
        /// </summary>
        /// <param name="disposing">If true unhook</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                if (_windowsHookHandle != IntPtr.Zero)
                {
                    if (!UnhookWindowsHookEx(_windowsHookHandle))
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode, $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }
                    _windowsHookHandle = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    _hookProc -= LowLevelKeyboardProc;
                }
            }

            if (_user32LibraryHandle != IntPtr.Zero)
            {
                if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }
                _user32LibraryHandle = IntPtr.Zero;
            }
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Dispose
        /// </summary>
        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Dispose and remove the keyboard hook
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Callback function that is called whenever a key is pressed
        /// </summary>
        /// <param name="nCode">Hook code</param>
        /// <param name="wParam">Keyboard state</param>
        /// <param name="lParam">LowLevelKeyboardInputEvent data</param>
        /// <returns>IntPtr</returns>
        private IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool fEatKeyStroke = false;

            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
            {
                object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                LowLevelKeyboardInputEvent p = (LowLevelKeyboardInputEvent)o;

                var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState)wparamTyped, GetKeyName(p.VirtualCode));

                EventHandler<GlobalKeyboardHookEventArgs> handler = KeyboardPressed;
                handler?.Invoke(this, eventArguments);

                fEatKeyStroke = eventArguments.Handled;
            }

            return fEatKeyStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        //****************************************************************************************************************************************************************************************************

        /// <summary>
        /// Get the key name from the virtual key code
        /// </summary>
        /// <param name="virtualKeyCode">virtual key code</param>
        /// <returns>Key name</returns>
        /// see: http://www.setnode.com/blog/mapvirtualkey-getkeynametext-and-a-story-of-how-to/
        private string GetKeyName(int virtualKeyCode)
        {
            int scanCode = MapVirtualKey((uint)virtualKeyCode, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC);
            
            // because MapVirtualKey strips the extended bit for some keys
            switch (virtualKeyCode)
            {
                case VirtualKeyCodes.VK_LEFT:
                case VirtualKeyCodes.VK_UP:
                case VirtualKeyCodes.VK_RIGHT:
                case VirtualKeyCodes.VK_DOWN: // arrow keys
                case VirtualKeyCodes.VK_PRIOR:
                case VirtualKeyCodes.VK_NEXT: // page up and page down
                case VirtualKeyCodes.VK_END:
                case VirtualKeyCodes.VK_HOME:
                case VirtualKeyCodes.VK_INSERT:
                case VirtualKeyCodes.VK_DELETE:
                case VirtualKeyCodes.VK_DIVIDE: // numpad slash
                case VirtualKeyCodes.VK_NUMLOCK:
                {
                    scanCode |= 0x100; // set extended bit
                    break;
                }
            }
            
            StringBuilder keyName = new StringBuilder(50);
            int result = 0;
            if ((result = GetKeyNameText(scanCode << 16, keyName, keyName.Capacity)) != 0)
            {
                return keyName.ToString();
            }
            else
            {
                return "[Error]";
            }
        }
    }
}
