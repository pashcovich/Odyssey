namespace Odyssey.Engine
{
    /// <summary>
    /// Service providing method to access Device life-cycle.
    /// </summary>
    public interface IGraphicsDeviceService : IDirectXDeviceService
    {
        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        new DirectXDevice DirectXDevice { get; }
    }
}