using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("Target")]
    public class TargetComponent : Component
    {
        public Vector3 Location { get; set; }

        public TargetComponent() : base(ComponentTypeManager.GetType<TargetComponent>())
        {
            Location = Vector3.Zero;
        }
    }
}
