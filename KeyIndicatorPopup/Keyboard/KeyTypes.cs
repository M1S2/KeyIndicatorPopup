using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyIndicatorPopup.Keyboard
{
    /// <summary>
    /// Classification of the keys in different categories
    /// </summary>
    public enum KeyTypes
    {
        /// <summary>
        /// Numeric keys 0 - 9. Also containing 0 - 9 on the numpad.
        /// </summary>
        Numeric,

        /// <summary>
        /// Letters A - Z
        /// </summary>
        Letter,

        /// <summary>
        /// Lock keys (NUM-Lock, CAPS-Lock, ROLL-Lock)
        /// </summary>
        Lock,

        /// <summary>
        /// All other keys
        /// </summary>
        System
    }
}
