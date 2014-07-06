using Odyssey.Engine;
using Odyssey.UserInterface;
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