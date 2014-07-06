namespace Odyssey.Engine
{
    /// <summary>
    /// Defines the interface for an object that manages a GraphicsDevice.
    /// </summary>
    public interface IDirectXDeviceManager
    {
        /// <summary>
        /// Starts the drawing of a frame.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        bool BeginDraw();

        /// <summary>
        /// Called to ensure that the device manager has created a valid device.
        /// </summary>
        void CreateDevice();

        /// <summary>
        /// Called by the game at the end of drawing; presents the final rendering.
        /// </summary>
        void EndDraw();
    }
}
