using System;

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
