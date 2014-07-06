using System;

namespace Odyssey.Interaction
{
    /// <summary>
    /// State of a button declared as an enum. See <see cref="ButtonState"/>.
    /// </summary>
    [Flags]
    public enum ButtonStateFlags : byte
    {
        /// <summary>
        /// Button is in a none state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The button is being pressed.
        /// </summary>
        Down = 1,

        /// <summary>
        /// The button was pressed since last frame.
        /// </summary>
        Pressed = 2,

        /// <summary>
        /// The button was released since last frame.
        /// </summary>
        Released = 4,
    }
}