namespace Odyssey.Interaction
{
    public enum PointerEventType : byte
    {
        /// <summary>
        /// The pointer capture was lost
        /// </summary>
        CaptureLost,

        /// <summary>
        /// The pointer entered the current control bounds
        /// </summary>
        Entered,

        /// <summary>
        /// The pointer exited the current control bounds
        /// </summary>
        Exited,

        /// <summary>
        /// The pointer moved over the current control bounds
        /// </summary>
        Moved,

        /// <summary>
        /// The pointer was pressed
        /// </summary>
        Pressed,

        /// <summary>
        /// The pointer was released
        /// </summary>
        Released,

        /// <summary>
        /// The pointer wheel changed its state
        /// </summary>
        WheelChanged,
    }
    
}