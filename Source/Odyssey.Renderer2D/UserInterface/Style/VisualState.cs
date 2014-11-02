using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Content;
using Odyssey.Graphics.Drawing;
using Odyssey.UserInterface.Controls;
using SharpDX;

namespace Odyssey.UserInterface.Style
{
    public sealed class VisualState : Component, IResourceProvider, IEnumerable<Shape>
    {
        private readonly Control parent ;
        private readonly  Shape[] shapes;

        private VisualState(Control parent, IEnumerable<Shape> shapes)
        {
            this.parent = parent;
            this.shapes = shapes.ToArray();
        }

        public void Initialize()
        {
            foreach (var shape in shapes)
            {
                ToDispose(shape);
                shape.Initialize();
            }
        }

        internal static VisualState GenerateVisualStateForControl(Control control, VisualStateDefinition visualStateDefinition)
        {
            var shapeList = new List<Shape>();
            foreach (var shape in visualStateDefinition.Shapes)
            {
                var newShape = (Shape)shape.Copy();
                newShape.Width = control.Width * shape.Width;
                newShape.Height = control.Height * shape.Height;
                newShape.Parent = control;
                newShape.DesignMode = true;
                newShape.IsInternal = true;
                newShape.Position = new Vector2(control.Width, control.Height) * shape.Position;
                shapeList.Add(newShape);
            }

            control.Animator.AddAnimations(visualStateDefinition.Animations);
            return new VisualState(control, shapeList);
        }

        public void Update()
        {
            foreach (Shape shape in shapes)
                shape.Layout();
        }

        public void SynchronizeSize()
        {
            foreach (var shape in shapes)
            {
                shape.Width *= parent.Width / shape.Width;
                shape.Height *= parent.Height/shape.Height;
            }

        }

        #region IResourceProvider
        TResource IResourceProvider.GetResource<TResource>(string resourceName)
        {
            return shapes.FirstOrDefault(s => s.Name == resourceName) as TResource;
        }

        IEnumerable<IResource> IResourceProvider.Resources
        {
            get { return shapes; }
        }

        public bool ContainsResource(string resourceName)
        {
            return shapes.Any(s=> s.Name == resourceName);
        }
        #endregion

        public Shape this[string shapeName]
        {
            get { return shapes.FirstOrDefault(s => s.Name == shapeName); }
        }

        public Shape this[int index]
        {
            get { return shapes[index]; }
        }

        #region IEnumerable<Shape>

        public IEnumerator<Shape> GetEnumerator()
        {
            foreach (Shape s in shapes)
                yield return s;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return shapes.GetEnumerator();
        }

        #endregion
    }
}
