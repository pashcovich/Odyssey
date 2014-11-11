using System;
using Odyssey.Epos.Systems;

namespace Odyssey.Epos.Maps
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