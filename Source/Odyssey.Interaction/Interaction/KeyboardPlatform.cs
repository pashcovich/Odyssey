using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Interaction
{
    /// <summary>
    /// Provides platform-specific bindings to keyboard input events
    /// </summary>
    internal abstract class KeyboardPlatform
    {
        private readonly KeyboardManager manager;

        /// <summary>
        /// Creates a new instance of <see cref="KeyboardPlatform"/> class.
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <param name="manager">The <see cref="KeyboardManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        protected KeyboardPlatform(KeyboardManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Raised when a key down.
        /// </summary>
        internal event EventHandler<KeyEventArgs> KeyDown;

        /// <summary>
        /// Raised when a key is up.
        /// </summary>
        internal event EventHandler<KeyEventArgs> KeyUp;

        public void Initialize(object nativeWindow)
        {
            Contract.Requires<ArgumentNullException>(nativeWindow != null, "nativeWindow");
            BindWindow(nativeWindow);
            manager.Associate(this);
        }

        /// <summary>
        /// Derived classes should implement platform-specific event bindings in this method
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        protected abstract void BindWindow(object nativeWindow);

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="key">The key that was pressed</param>
        protected void RaiseKeyPressed(Keys key)
        {
            if (key == Keys.None) return;
            Raise(KeyDown, new KeyEventArgs(key));
        }

        /// <summary>
        /// Raises the <see cref="KeyUp"/> event.
        /// </summary>
        /// <param name="key">The key that was released</param>
        protected void RaiseKeyReleased(Keys key)
        {
            if (key == Keys.None) return;
            Raise(KeyUp, new KeyEventArgs(key));
        }

        /// <summary>
        /// Generic helper method to call a single-parameter event handler
        /// </summary>
        /// <remarks>This ensures that during the call - the handler reference will not be lost (due to stack-copy of delegate reference)</remarks>
        /// <typeparam name="TArg">The type of event argument</typeparam>
        /// <param name="handler">The reference to event delegate</param>
        /// <param name="argument">The event argument</param>
        private static void Raise<TArg>(EventHandler<TArg> handler, TArg argument)
        {
            if (handler != null)
                handler(null, argument);
        }
    }
}