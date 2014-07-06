using SharpDX;
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
    internal abstract class PointerPlatform : Component
    {
        protected readonly PointerManager manager;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerPlatform"/> class
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        protected PointerPlatform(PointerManager manager)
        {
            Contract.Requires<ArgumentNullException>(manager != null, "manager");

            this.manager = manager;
        }

        public void Initialize(object nativeWindow)
        {
            Contract.Requires<ArgumentNullException>(nativeWindow != null, "nativeWindow");
            BindWindow(nativeWindow);
            manager.Associate(this);
        }

        /// <summary>
        /// Derived classes should perform the binding to platform-specific events on <paramref name="nativeWindow"/> and raise the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        protected abstract void BindWindow(object nativeWindow);
    }
}