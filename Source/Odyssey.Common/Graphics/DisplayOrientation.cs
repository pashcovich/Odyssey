using System;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Describes the orientation of the display.
    /// </summary>
    [Flags]
    public enum DisplayOrientation
    {
        /// <summary>
        /// The default value for the orientation.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Displays in landscape mode to the left.
        /// </summary>
        LandscapeLeft = 1,

        /// <summary>
        /// Displays in landscape mode to the right.
        /// </summary>
        LandscapeRight = 2,

        /// <summary>
        /// Displays in portrait mode.
        /// </summary>
        Portrait = 4
    }
}
