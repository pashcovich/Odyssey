using System.Collections.Generic;
using System.Linq;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;

namespace Odyssey.UserInterface.Style
{
    public class VisualState
    {
        private readonly Shape[] shapes;
        private readonly Dictionary<ControlStatus, Shape[]> shapeCache;

        public IEnumerable<Shape> Shapes { get { return shapes; } }

        public VisualState(IEnumerable<Shape> shapes)
        {
            this.shapes = shapes.ToArray();
            shapeCache = new Dictionary<ControlStatus, Shape[]>();
        }

        public void Initialize(Controls.Control control)
        {
            var enabledShapes = new Shape[shapes.Length];

            for (int i = 0; i < shapes.Length; i++)
            {
                var shape = shapes[i];
                var shapeCopy = (Shape) shape.Copy();
                shapeCopy.Width = control.Width*shape.Width;
                shapeCopy.Height = control.Height*shape.Height;
                shapeCopy.Parent = control;
                shapeCopy.DesignMode = false;
                shapeCopy.Initialize();
                enabledShapes[i] = shapeCopy;
            }

            shapeCache.Add(ControlStatus.Enabled, enabledShapes);
        }

        public void Update()
        {
            foreach (Shape shape in AllShapes)
                shape.Layout();
        }

        public IEnumerable<Shape> this[ControlStatus status]
        {
            get { return shapeCache[status]; }
        }

        private IEnumerable<Shape> AllShapes
        {
            get { return shapeCache.Values.SelectMany(shapeList => shapeList); }
        }
    }
}
