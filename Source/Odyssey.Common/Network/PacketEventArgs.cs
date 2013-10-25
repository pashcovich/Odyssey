using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Network
{
    public class PacketEventArgs<T> : EventArgs
    {
        public T Data { get; private set; }

        public PacketEventArgs(T data)
        {
            Data = data;
        }

    }
}
