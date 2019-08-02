using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyIndicatorPopup.Keyboard
{
    /// <summary>
    /// Keyboard states
    /// </summary>
    public enum KeyboardState
    {
        /// <summary>
        /// Key was pressed down
        /// </summary>
        KeyDown = 0x0100,

        /// <summary>
        /// Key was released up
        /// </summary>
        KeyUp = 0x0101,

        /// <summary>
        /// System key was pressed down
        /// </summary>
        SysKeyDown = 0x0104,

        /// <summary>
        /// System key was released up
        /// </summary>
        SysKeyUp = 0x0105
    }
}
