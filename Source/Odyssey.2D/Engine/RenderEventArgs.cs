#if !ODYSSEY_ENGINE && DIRECTX11_1
using System;

namespace Odyssey.Engine
{
    public class RenderEventArgs : EventArgs
    {
        public RenderEventArgs(IDirectXTarget target)
        {
            Target = target;
        }

        public IDirectXTarget Target { get; private set; }
    }
}
#endif