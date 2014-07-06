using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics
{
    internal class ShapeInitializer<TShape>
        where TShape : Component, IShapeD2D
    {
        private readonly Direct2DDevice device;
        private readonly List<Direct2DResource> resources;

        public ShapeInitializer(Direct2DDevice device)
        {
            this.device = device;
            resources = new List<Direct2DResource>();
        }

        public IEnumerable<Direct2DResource> CreatedResources { get { return resources; } }

        public void Initialize(TShape shape)
        {
            if (shape.FillShader == null)
                shape.FillShader = LinearGradient.CreateUniform(ShapeBase.DefaultFillColor);
            if (shape.StrokeShader == null)
                shape.StrokeShader = LinearGradient.CreateUniform(ShapeBase.DefaultStrokeColor);

            switch (shape.FillShader.Type)
            {
                default:
                    shape.Stroke = SolidBrush.New(device, shape.StrokeShader.GradientStops[0].Color);
                    shape.Fill = SolidBrush.New(device, shape.FillShader.GradientStops[0].Color);
                    break;
            }

            resources.Add(shape.Stroke);
            resources.Add(shape.Fill);
        }
    }
}