using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public class RenderEventArgs : EventArgs
    {
        public IDirectXTarget Target { get; private set; }

        public RenderEventArgs(IDirectXTarget target)
        {
            Target = target;
        }
    }
}
