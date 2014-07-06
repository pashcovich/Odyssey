namespace Odyssey.Engine
{
    /// <summary>
    /// Contains context used to render the game (Control for WinForm, a DrawingSurface for WP8...etc.).
    /// </summary>
    public abstract class ApplicationContext
    {
        /// <summary>
        /// The context type of this instance.
        /// </summary>
        public ApplicationContextType ContextType { get { return contextType; } }

        private readonly ApplicationContextType contextType;

        /// <summary>
        /// The native control used.
        /// </summary>
        public readonly object Control;

        /// <summary>
        /// The requested width.
        /// </summary>
        public int RequestedWidth { get; set; }

        public float DpiX { get; set; }

        public float DpiY { get; set; }

        /// <summary>
        /// The requested height.
        /// </summary>
        public int RequestedHeight { get; set; }

        protected ApplicationContext(ApplicationContextType type, object control)
        {
            Control = control;
            contextType = type;
        }
    }
}