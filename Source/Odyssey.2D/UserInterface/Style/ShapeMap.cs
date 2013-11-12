using Odyssey.Engine;
using Odyssey.Graphics.Rendering2D;
using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Style
{
    public class ShapeMap
    {
        private ControlDescription description;
        private Dictionary<ControlStatus, List<IShape>> map;

        public ShapeMap(ControlDescription description)
        {
            this.description = description;
            map = new Dictionary<ControlStatus, List<IShape>>();
        }

        public bool IsDesignMode { get; private set; }

        public void Add(ControlStatus status, IShape shape)
        {
            ProcessInsertion(status, shape);
        }

        public void Add(ControlStatus status, IEnumerable<IShape> shapes)
        {
            foreach (IShape shape in shapes)
                ProcessInsertion(status, shape);
        }

        public void BeginDesign()
        {
            IsDesignMode = true;
        }

        public void EndDesign(IDirectXProvider directX)
        {
            foreach (var kvp in map)
                foreach (IShape shape in kvp.Value)
                {
                    shape.Initialize(directX);
                }
        }

        public IEnumerable<IShape> GetShapes(ControlStatus status)
        {
            Contract.Requires<InvalidOperationException>(HasShapes(status));
            return map[status];
        }

        public bool HasShapes(ControlStatus status)
        {
            return map[status] != null;
        }

        private void ProcessInsertion(ControlStatus status, IShape shape)
        {
            Contract.Requires<NullReferenceException>(shape != null);
            shape.FillShader = description.GetFillGradient(status);
            shape.StrokeShader = description.GetStrokeGradient(status);

            if (!map.ContainsKey(status))
                map.Add(status, new List<IShape>());
            map[status].Add(shape);
        }
    }
}