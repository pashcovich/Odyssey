using System;
using Odyssey.Talos.Systems;

namespace Odyssey.Talos.Maps
{
    public class SystemEventArgs : EventArgs
    {
        public ISystem Source { get; private set; }

        public SystemEventArgs(ISystem system)
        {
            Source = system;
        }
        
    }
}