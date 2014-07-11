using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;
using SharpDX;
using System;

namespace Odyssey.Graphics
{
    public class CutCornerRectangleOutline : CutCornerRectangleBase
    {
        private GeometryGroup shape;
        private Matrix3x2 transform;

        public float OutlineThickness { get; set; }

        public override void Render()
        {
            Device.SetTransform(transform);

            Device.FillGeometry(shape, Fill);
            Device.DrawGeometry(shape, Stroke);
        }

        protected override void Measure()
        {
            transform = Matrix3x2.Translation(AbsolutePosition);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);

            Vector2[] innerPoints =
            {
                new Vector2(CutCornerLength, 0),
                    new Vector2(Width - CutCornerLength, 0),
                    new Vector2(Width, CutCornerLength),
                    new Vector2(Width, Height - CutCornerLength),
                    new Vector2(Width-CutCornerLength, Height),
                    new Vector2(CutCornerLength, Height),
                    new Vector2(0, Height - CutCornerLength),
                    new Vector2(0, CutCornerLength)
            };
            float innerOffset = OutlineThickness / (float)Math.Sqrt(2);
            Vector2[] outerPoints =
            {
                new Vector2(2 * innerOffset, innerOffset),
                    new Vector2(Width - 2 * innerOffset, innerOffset),
                    new Vector2(Width - innerOffset, 2 * innerOffset),
                    new Vector2(Width - innerOffset, Height - 2 * innerOffset),
                    new Vector2(Width - 2 * innerOffset, Height - innerOffset),
                    new Vector2(2 * innerOffset, Height - innerOffset),
                    new Vector2(innerOffset, Height - 2 * innerOffset),
                    new Vector2(innerOffset, 2 * innerOffset)
            };

            PolyLine outerShape = ToDispose(PolyLine.New(Device, innerPoints, FigureBegin.Filled, FigureEnd.Closed));
            PolyLine innerShape = ToDispose(PolyLine.New(Device, outerPoints, FigureBegin.Filled, FigureEnd.Closed));

            shape = ToDispose(GeometryGroup.New(Device, FillMode.Alternate, new[] { innerShape, outerShape, }));

            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);

            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}