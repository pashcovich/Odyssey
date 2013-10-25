using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class MeshOverlay : Overlay
    {
        List<ShapeMeshDescription> shapes;
        MeshInterface mesh;

        public MeshOverlay(OverlayDescription description)
        {
            AbsolutePosition = Vector2.Zero;
            Size = new Size(description.Width, description.Height);
            OverlayDescription = description;
            Depth = new Depth
                        {
                            WindowLayer = Depth.Background,
                            ComponentLayer = Depth.Background,
                            ZOrder = Depth.Background
                        };
        }

        public override void Initialize(IDirectXProvider directX)
        {
            base.Initialize(directX);
            shapes = new List<ShapeMeshDescription>();
            mesh = new MeshInterface();
        }

        public override void EndDesign(IDirectXProvider directX)
        {
            base.EndDesign(directX);

            foreach (IControlMesh control in TreeTraversal.PreOrderVisibleControlsVisit(this))
            {
                shapes.AddRange(control.Shapes);
            }
            
            mesh.Build(shapes);
            mesh.Initialize(directX);
        }

        public override void Render(IDirectXTarget target)
        {
            mesh.Render(target);
        }
    
    }
}
