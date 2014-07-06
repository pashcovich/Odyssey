using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [YamlTag("OrbitBehaviour")]
    public class OrbitBehaviourComponent : Component
    {
        public float RadiusX { get; set; }
        public float RadiusZ { get; set; }
        public float Theta { get; set; }
        public float Speed { get; set; }

        public OrbitBehaviourComponent() : base(ComponentTypeManager.GetType<OrbitBehaviourComponent>())
        {
            RadiusX = 1;
            RadiusZ = 1;
            Theta = 0;
            Speed = 5;
        }
    }
}
