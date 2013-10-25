using Odyssey.Engine;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public class Rectangle : RectangleBase, IShapeMesh
    {
        public ShapeMeshDescription Shape { get; private set;} 

        public Rectangle() : base()
        {
        }

        public override void Initialize(IDirectXProvider directX)
        {
            Designer d = new Designer();
            d.Begin();
            d.FillRectangle(BoundingRectangle, Fill);
            d.End();

            Shape = d.Result[0];
        }

        public override void Render(IDirectXTarget target)
        {}
            
        
    }
}
