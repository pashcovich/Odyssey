using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Components
{
    public class ControllerComponent : Component
    {
        public IEntityController Controller { get; set; }

        public ControllerComponent() : base(ComponentTypeManager.GetType<ControllerComponent>())
        {
        }
    }
}
