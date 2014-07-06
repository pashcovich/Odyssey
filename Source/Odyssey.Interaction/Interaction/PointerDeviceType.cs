namespace Odyssey.Interaction
{
    /// <summary>
    /// Represent the specific pointer device type
    /// </summary>
    public enum PointerDeviceType : byte
    {
        /// <summary>
        /// Touch pointer device. A touchscreen, for example.
        /// </summary>
        Touch,

        /// <summary>
        /// The pen pointer device.
        /// </summary>
        Pen,

        /// <summary>
        /// The mouse.
        /// </summary>
        Mouse
    }
}