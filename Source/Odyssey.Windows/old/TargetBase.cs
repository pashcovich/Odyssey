using Odyssey.Engine;
using System;

namespace Odyssey.Platforms.Windows
{
    public abstract class TargetBaseWindows : TargetBase, IDirectXTarget
    {

        /// <summary>
        /// Gets the current bounds of the control linked to this render target
        /// </summary>
        protected abstract System.Drawing.Rectangle CurrentControlBounds
        {
            get;
        }

        /// <summary>
        /// Notifies for size changed
        /// </summary>
        public void UpdateForSizeChange()
        {
            var newBounds = CurrentControlBounds;

            if (newBounds.Width != ControlBounds.Width ||
                newBounds.Height != ControlBounds.Height)
            {
                // Store the window bounds so the next time we get a SizeChanged event we can
                // avoid rebuilding everything if the size is identical.
                ControlBounds = newBounds;

                OnSizeChanged(new RenderEventArgs(this));
            }
        }

        /// <summary>
        /// Gets the bounds of the control linked to this render target
        /// </summary>
        public System.Drawing.Rectangle RenderTargetBounds { get; protected set; }

        /// <summary>
        /// Gets the size in pixels of the Direct3D RenderTarget
        /// </summary>
        public System.Drawing.Size RenderTargetSize { get { return new System.Drawing.Size(RenderTargetBounds.Width, RenderTargetBounds.Height); } }

        /// <summary>
        /// Gets the bounds of the control linked to this render target
        /// </summary>
        public System.Drawing.Rectangle ControlBounds { get; protected set; }



    }
}
