using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyIndicatorPopup.Keyboard
{
    /// <summary>
    /// Class that holds the virtual key codes of several keys
    /// </summary>
    /// see: https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    public static class VirtualKeyCodes
    {
        public const int VK_0 = 0x30;           // 0 key
        public const int VK_9 = 0x39;           // 9 key
        public const int VK_NUM0 = 0x60;        // Numpad 0 key
        public const int VK_NUM9 = 0x69;        // Numpad 9 key
        public const int VK_A = 0x41;           // A key
        public const int VK_Z = 0x5A;           // Z key

        public const int VK_LEFT = 0x25;        // LEFT ARROW key
        public const int VK_UP = 0x26;          // UP ARROW key
        public const int VK_RIGHT = 0x27;       // RIGHT ARROW key
        public const int VK_DOWN = 0x28;        // DOWN ARROW key

        public const int VK_PRIOR = 0x21;       // PAGE UP key
        public const int VK_NEXT = 0x22;        // PAGE DOWN key

        public const int VK_END = 0x23;         // END key
        public const int VK_HOME = 0x24;        // HOME key
        public const int VK_INSERT = 0x2D;      // INS key
        public const int VK_DELETE = 0x2E;      // DEL key
        public const int VK_DIVIDE = 0x6F;      // Divide key

        public const int VK_CAPITAL = 0x14;     // CAPS LOCK key
        public const int VK_NUMLOCK = 0x90;     // NUM LOCK key
        public const int VK_SCROLL = 0x91;      // SCROLL LOCK key
    }
}
