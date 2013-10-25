using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public interface IDeviceDependentComponent
    {
        void Initialize(InitializeDirectXEventArgs e);
    }
}
