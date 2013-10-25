using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public class WP8DeviceSettings : ISettings
    {
        public int ScreenWidth { get; private set;}
        public int ScreenHeight { get; private set;}

        public float Dpi { get { throw new NotImplementedException(); } }
    }
}
