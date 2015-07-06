using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Geometry;
using Odyssey.Geometry.Extensions;
using SharpDX.Mathematics;
using Rectangle = Odyssey.Geometry.Extensions.Rectangle;

namespace Odyssey.Epos.Systems
{
    public class BoundingBox2DSystem : UpdateableSystemBase
    {
        public BoundingBox2DSystem()
            : base(Selector.All(typeof(BoundingBox2DComponent), typeof(BoundingBoxComponent)))
        {
        }

        public override bool BeforeUpdate()
        {
            return Services.GetService<ICameraService>().MainCamera.Changed && base.BeforeUpdate();
        }

        public override void Process(ITimeService time)
        {
            var camera = Services.GetService<ICameraService>().MainCamera;

            foreach (var entity in Entities)
            {
                var cUI = entity.GetComponent<UIElementComponent>();
                var cBox2D = entity.GetComponent<BoundingBox2DComponent>();
                var obBox = entity.GetComponent<BoundingBoxComponent>().BoundingBox;
                var boxExtents = obBox.GetCorners().Select(p=> p.Project(camera)).ToArray();

                var pMin = MathHelper.MinCoordinates(boxExtents);
                var pMax = MathHelper.MaxCoordinates(boxExtents);

                var r = Rectangle.FromTwoPoints(pMin, pMax);
                var offset = new Vector2(r.Width, r.Height) * 0.25f;
                r.Inflate(offset.X, offset.Y);

                cBox2D.BoundingBox = r;
                cUI.Element.SetSize(0, cBox2D.BoundingBox.Width);
                cUI.Element.SetSize(1, cBox2D.BoundingBox.Height);
                cUI.Element.Position = new Vector3(cBox2D.BoundingBox.X, cBox2D.BoundingBox.Y, 0f);

            }
        }


    }
}
