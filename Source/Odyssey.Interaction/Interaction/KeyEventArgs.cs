using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    public class KeyEventArgs : EventArgs
    {
        public Keys KeyData { get; private set; }

        public KeyEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }
    }
}
