using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    internal class DesktopWpfApplicationPlatform : DesktopApplicationPlatform
    {
        public DesktopWpfApplicationPlatform(Application application) : base(application)
        {
        }

        protected override IEnumerable<ApplicationWindow> GetSupportedApplicationWindows()
        {
            return new ApplicationWindow[] { new DesktopWpfApplicationWindow(),  };
        }
    }
}
