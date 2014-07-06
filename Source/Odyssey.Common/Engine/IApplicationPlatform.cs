namespace Odyssey.Engine
{
    /// <summary>
    /// Interface for a game platform (OS, machine dependent).
    /// </summary>
    public interface IApplicationPlatform
    {
        /// <summary>
        /// Gets the default app directory.
        /// </summary>
        /// <value>The default app directory.</value>
        string DefaultAppDirectory { get; }

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <value>The main window.</value>
        ApplicationWindow MainWindow { get; }

        /// <summary>
        /// Creates the a new <see cref="ApplicationWindow"/>. See remarks.
        /// </summary>
        /// <param name="gameContext">The window context. See remarks.</param>
        /// <returns>A new game window.</returns>
        /// <remarks>
        /// This is currently only supported on Windows Desktop. The window context supported on windows is a subclass of System.Windows.Forms.Control (or null and a default RenderForm will be created).
        /// </remarks>
        ApplicationWindow CreateWindow(ApplicationContext gameContext = null);

    }
}
