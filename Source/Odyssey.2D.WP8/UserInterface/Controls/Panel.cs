using Odyssey.Graphics.Rendering2D.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class Panel : PanelBase, IControlMesh
    {
        public IEnumerable<ShapeMeshDescription> Shapes { get { yield return shape; } }

        ShapeMeshDescription shape;

        public override bool Contains(SharpDX.Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Initialize(Engine.IDirectXProvider directX)
        {
            base.Initialize(directX);
            
            ShapeMap.BeginDesign();
            Rectangle rEnabled = ToDispose(new Rectangle()
            {
                Id = string.Format("{0}_{1}_rectangle", Id, ControlStatus.Enabled),
                BoundingRectangle = BoundingRectangle
            });
            
            ShapeMap.Add(ControlStatus.Enabled, rEnabled);
            ShapeMap.EndDesign(directX);
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();

            shape = ((IShapeMesh)ActiveStyle[0]).Shape;
        }

    }
}
