using Odyssey.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Devices
{
    public class KeyEventArgs
    {
        public bool Handled { get; internal set; }
        public Key KeyCode { get; private set; }

        public KeyEventArgs(Key key)
        {
            KeyCode = key;
        }
    }
}