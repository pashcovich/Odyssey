using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    /// <summary>
    /// Base class for platform-specific event bindings
    /// </summary>
    internal abstract class MousePlatform
    {
        private readonly object nativeWindow; // used to retrieve mouse location

        /// <summary>
        /// Initializes a new instance of <see cref="MousePlatform"/> class
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null</exception>
        protected MousePlatform(object nativeWindow)
        {
            Contract.Requires<ArgumentNullException>(nativeWindow!= null, "nativeWindow");

            this.nativeWindow = nativeWindow;
            BindWindow(nativeWindow);
        }

        /// <summary>
        /// Raised when a button is pressed
        /// </summary>
        internal EventHandler<MouseEventArgs> ;
        internal event Action<MouseButton> MouseDown;

        /// <summary>
        /// Raised when a button is released
        /// </summary>
        internal event Action<MouseButton> MouseUp;

        /// <summary>
        /// Raised when mouse wheel delta is changed
        /// </summary>
        internal event Action<int> MouseWheelDelta;

        /// <summary>
        /// Returns the location of mouse cursor relative to program window
        /// </summary>
        /// <returns></returns>
        internal Vector2 GetLocation()
        {
            return GetLocationInternal();
        }

        /// <summary>
        /// Sets the mouse cursor location.
        /// </summary>
        /// <param name="point">The position in space [0,1].</param>
        /// <remarks>Supported only on Desktop platform. On other platforms the call of this method has no effect.</remarks>
        internal virtual void SetLocation(Vector2 point) { }

        /// <summary>
        /// Derived classes should implement platform-specific event bindings in this method
        /// </summary>
        /// <param name="nativeWindow">The native window object reference</param>
        protected abstract void BindWindow(object nativeWindow);

        /// <summary>
        /// Derived classes should implement platform-specific code to retrieve the mouse cursor location
        /// </summary>
        protected abstract Vector2 GetLocationInternal();

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event
        /// </summary>
        /// <param name="button">Mouse button which has been pressed</param>
        protected void OnMouseDown(MouseButton button)
        {
            Raise(MouseDown, button);
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event
        /// </summary>
        /// <param name="button">Mouse button which has been released</param>
        protected void OnMouseUp(MouseButton button)
        {
            Raise(MouseUp, button);
        }

        /// <summary>
        /// Raises the <see cref="MouseWheelDelta"/> event
        /// </summary>
        /// <param name="wheelDelta">Current value of mouse wheel delta</param>
        protected void OnMouseWheel(int wheelDelta)
        {
            Raise(MouseWheelDelta, wheelDelta);
        }

        /// <summary>
        /// Generic helper method to call a single-parameter event handler
        /// </summary>
        /// <remarks>This ensures that during the call - the handler reference will not be lost (due to stack-copy of delegate reference)</remarks>
        /// <typeparam name="TArg">The type of event argument</typeparam>
        /// <param name="handler">The reference to event delegate</param>
        /// <param name="argument">The event argument</param>
        private static void Raise<TArg>(Action<TArg> handler, TArg argument)
        {
            if (handler != null)
                handler(argument);
        }
    }
}
