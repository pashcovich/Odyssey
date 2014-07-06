using Odyssey.Talos.Components;
using Odyssey.Talos.Nodes;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [YamlTag("PerspectiveCameraSystem")]
    public class PerspectiveCameraSystem : CameraSystem<PerspectiveCameraNode>
    {
        public PerspectiveCameraSystem() 
            : base(Aspect.All(typeof(PositionComponent), typeof(CameraComponent), typeof(UpdateComponent)))
        {
        }

        public override void AfterUpdate()
        {
        }
    }
}
