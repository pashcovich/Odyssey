namespace Odyssey.Interaction
{
    /// <summary>
    /// Indicates the kind of the pointer state change.
    /// </summary>
    public enum PointerUpdateKind
    {
        /// <summary>
        /// Other pointer event.
        /// </summary>
        Other,

        /// <summary>
        /// The left device button was pressed.
        /// </summary>
        LeftButtonPressed,

        /// <summary>
        /// The left device button was released.
        /// </summary>
        LeftButtonReleased,

        /// <summary>
        /// The right device button was pressed.
        /// </summary>
        RightButtonPressed,

        /// <summary>
        /// The right device button was released.
        /// </summary>
        RightButtonReleased,

        /// <summary>
        /// The middle device button was pressed.
        /// </summary>
        MiddleButtonPressed,

        /// <summary>
        /// The middle device button was released.
        /// </summary>
        MiddleButtonReleased,

        /// <summary>
        /// The device X-button 1 was pressed.
        /// </summary>
        XButton1Pressed,

        /// <summary>
        /// The device X-button 1 was released.
        /// </summary>
        XButton1Released,

        /// <summary>
        /// The device X-button 2 was pressed.
        /// </summary>
        XButton2Pressed,

        /// <summary>
        /// The device X-button 2 was released.
        /// </summary>
        XButton2Released,
    }
}
