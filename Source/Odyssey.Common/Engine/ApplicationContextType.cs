namespace Odyssey.Engine
{
    /// <summary>
    /// Type of a <see cref="ApplicationContext"/>.
    /// </summary>
    public enum ApplicationContextType
    {
        /// <summary>
        /// Application running on desktop in a form or <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        Desktop,

        /// <summary>
        /// Game running on desktop in a WPF border control through a HwndHost
        /// </summary>
        DesktopHwndWpf,

        /// <summary>
        /// Application running on WinRT in a CoreWindow.
        /// </summary>
        WinRT,

        /// <summary>
        /// Application running on WinRT in a SwapChainBackgroundPanel.
        /// </summary>
        WinRTBackgroundXaml,

        /// <summary>
        /// Application running on WinRT in a SurfaceImageSource.
        /// </summary>
        WinRTXaml,

        /// <summary>
        /// Application running on WinRT in a DrawingBackgroundSurface.
        /// </summary>
        WindowsPhoneBackgroundXaml,

        /// <summary>
        /// Application running on WinRT in a DrawingSurface.
        /// </summary>
        WindowsPhoneXaml,
    }
}
