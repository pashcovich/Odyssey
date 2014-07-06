using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.UserInterface.Controls;
using Odyssey.Utilities.Logging;
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
        private readonly ControlDescription description;
        private readonly Dictionary<ControlStatus, List<IShape>> map;

        public ShapeMap(ControlDescription description)
        {
            this.description = description;
            map = new Dictionary<ControlStatus, List<IShape>>();
        }

        private IEnumerable<UIElement> AllShapes
        {
            get { return map.Values.SelectMany(shapeList => shapeList).Cast<UIElement>(); }
        }

        public void Add(ControlStatus status, IShape shape)
        {
            ProcessInsertion(status, shape);
        }

        public void Add(ControlStatus status, IEnumerable<IShape> shapes)
        {
            foreach (IShape shape in shapes)
                ProcessInsertion(status, shape);
        }

        public IEnumerable<IShape> GetShapes(ControlStatus status)
        {
            return !HasShapes(status) ? Enumerable.Empty<IShape>() : map[status];
        }

        public bool HasShapes(ControlStatus status)
        {
            return map[status] != null;
        }

        public void Initialize()
        {
            foreach (UIElement shape in AllShapes)
            {
                shape.DesignMode = false;
                shape.Layout();
                shape.Initialize();
            }
        }

        public void Update()
        {
            foreach (UIElement shape in AllShapes)
                shape.Layout();
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