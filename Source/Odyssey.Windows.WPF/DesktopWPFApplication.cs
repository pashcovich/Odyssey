using Odyssey.Engine;
using System;
using System.Collections.Generic;

namespace Odyssey
{
    public class DesktopWpfApplication : Application
    {
        public DesktopWpfApplication()
            : base(typeof(DesktopWpfApplicationPlatform))
        {
        }
    }
}