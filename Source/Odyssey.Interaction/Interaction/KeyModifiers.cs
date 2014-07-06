using System;

namespace Odyssey.Interaction
{
    /// <summary>
    /// These flags represent the corresponding modifier keys that were pressed at some specific event.
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// No modifier key are pressed.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The CTRL modifier key.
        /// </summary>
        Control = 0x01,

        /// <summary>
        /// The SHIFT modifier key.
        /// </summary>
        Shift = 0x02,

        /// <summary>
        /// The ALT modifier key.
        /// </summary>
        Menu = 0x04,

        /// <summary>
        /// The WIN modifier key.
        /// </summary>
        Windows = 0x08
    }
}