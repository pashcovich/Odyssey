using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Graphics.Drawing;
using Odyssey.UserInterface.Controls;

namespace Odyssey.UserInterface.Style
{
    public sealed class VisualState : Component, IResourceProvider, IEnumerable<Shape>
    {
        private readonly VisualElement parent;
        private readonly Shape[] shapes;

        private VisualState(VisualElement parent, IEnumerable<Shape> shapes)
        {
            this.parent = parent;
            this.shapes = shapes.ToArray();
        }

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

        public void Initialize()
        {
            foreach (Shape shape in shapes)
            {
                ToDispose(shape);
                shape.Initialize();
            }

        }
        
        internal static VisualState GenerateVisualStateForControl(VisualElement control,
            VisualStateDefinition visualStateDefinition)
        {
            var shapeList = new List<Shape>();
            foreach (Shape shape in visualStateDefinition.Shapes)
            {
                var newShape = (Shape) shape.Copy();
                newShape.ScaleX = shape.Width;
                newShape.ScaleY = shape.Height;
                newShape.Parent = control;
                newShape.DesignMode = true;
                newShape.IsInternal = true;
                newShape.HorizontalAlignment = control.HorizontalAlignment;
                newShape.VerticalAlignment = control.VerticalAlignment;
                newShape.PositionOffsets = control.PositionOffsets;
                shapeList.Add(newShape);
            }

            control.Triggers.AddRange(visualStateDefinition.Triggers);
            control.Animator.AddAnimations(visualStateDefinition.Animations);
            return new VisualState(control, shapeList);
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
            return shapes.Any(s => s.Name == resourceName);
        }

        #endregion
    }
}