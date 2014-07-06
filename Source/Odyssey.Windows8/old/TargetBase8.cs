// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Odyssey.Engine
{
    /// <summary>
    /// This class is an abstract class responsible to maintain a list of
    /// render task, render target view / depth stencil view and Direct2D
    /// render target.
    /// </summary>
    /// <remarks>
    /// SharpDX CommonDX is inspired from the DirectXBase C++ class from Win8
    /// Metro samples, but the design is slightly improved in order to reuse
    /// components more easily.
    /// <see cref="DeviceManager"/> is responsible for device creation.
    /// <see cref="TargetBase"/> is responsible for rendering, render target
    /// creation.
    /// Initialization and Rendering is event driven based, allowing a better
    /// reuse of different components.
    /// </remarks>
    public abstract class TargetBaseWindows8 : TargetBase, IDirectXTarget
    {
           /// <summary>
        /// Gets the bounds of the control linked to this render target
        /// </summary>
        public Windows.Foundation.Rect RenderTargetBounds { get; protected set; }

        /// <summary>
        /// Gets the size in pixels of the Direct3D RenderTarget
        /// </summary>
        public Windows.Foundation.Size RenderTargetSize { get { return new Windows.Foundation.Size(RenderTargetBounds.Width, RenderTargetBounds.Height); } }

        /// <summary>
        /// Gets the bounds of the control linked to this render target
        /// </summary>
        public Windows.Foundation.Rect ControlBounds { get; protected set; }

        /// <summary>
        /// Gets the current bounds of the control linked to this render target
        /// </summary>
        protected abstract Windows.Foundation.Rect CurrentControlBounds
        {
            get;
        }



        /// <summary>
        /// Initializes a new <see cref="TargetBase"/> instance
        /// </summary>
        public TargetBaseWindows8()
        {
        }

        
        /// <summary>
        /// Notifies for size changed
        /// </summary>
        public virtual void UpdateForSizeChange()
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



        private void devices_OnDpiChanged(DeviceManager obj)
        {
            OnSizeChanged(new RenderEventArgs(this));
        }


    }
}