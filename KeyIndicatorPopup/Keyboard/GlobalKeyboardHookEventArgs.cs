using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KeyIndicatorPopup.Keyboard
{
    /// <summary>
    /// Event args of the Keyboard pressed event
    /// </summary>
    class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Keyboard state (Key down, Key up)
        /// </summary>
        public KeyboardState KeyboardState { get; private set; }

        /// <summary>
        /// Low level keyboard event data
        /// </summary>
        public LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        /// <summary>
        /// Key Type (Numeric, Letter, Lock, System)
        /// </summary>
        public KeyTypes KeyType { get; private set; }

        /// <summary>
        /// String that represents the name of the key
        /// </summary>
        public string KeyName { get; private set; }


        public GlobalKeyboardHookEventArgs(LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState, string keyName)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
            KeyName = keyName;

            if ((KeyboardData.VirtualCode >= VirtualKeyCodes.VK_0 && KeyboardData.VirtualCode <= VirtualKeyCodes.VK_9) || (KeyboardData.VirtualCode >= VirtualKeyCodes.VK_NUM0 && KeyboardData.VirtualCode <= VirtualKeyCodes.VK_NUM9))
            {
                KeyType = KeyTypes.Numeric;
            }
            else if (KeyboardData.VirtualCode >= VirtualKeyCodes.VK_A && KeyboardData.VirtualCode <= VirtualKeyCodes.VK_Z)
            {
                KeyType = KeyTypes.Letter;
            }
            else if(keyboardData.VirtualCode == VirtualKeyCodes.VK_NUMLOCK || keyboardData.VirtualCode == VirtualKeyCodes.VK_CAPITAL || keyboardData.VirtualCode == VirtualKeyCodes.VK_SCROLL)
            {
                KeyType = KeyTypes.Lock;
            }
            else
            {
                KeyType = KeyTypes.System;
            }
        }
    }
}
