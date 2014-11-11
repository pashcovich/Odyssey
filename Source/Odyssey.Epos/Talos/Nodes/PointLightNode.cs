using Odyssey.Epos.Components;
using Odyssey.Geometry;
using System;
using SharpDX;

namespace Odyssey.Epos.Nodes
{
    public class PointLightNode : LightNode
    {
        public PointLightNode(Entity entity) : base(entity)
        {
            LightComponent = entity.GetComponent<PointLightComponent>();
        }

        public override void Update()
        {
            WorldPosition = Vector3.Transform(PositionComponent.Position, TransformComponent.World).ToVector3();
            //WorldPosition = PositionComponent.Position;
        }


    }
}
