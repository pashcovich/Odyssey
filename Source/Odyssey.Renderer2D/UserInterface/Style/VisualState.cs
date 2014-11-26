using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Graphics.Drawing;
using Odyssey.Logging;
using Odyssey.UserInterface.Controls;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Style
{
    public sealed class VisualState : Component, IResourceProvider, IEnumerable<Shape>
    {
        private readonly Control parent;
        private readonly Shape[] shapes;

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

        public void Layout()
        {
            foreach (var shape in shapes)
            {
                if (shape.IsInternal)
                {
                    var newSize = new Vector3(shape.ScaleX*parent.RenderSize.X, shape.ScaleY*parent.RenderSize.Y, parent.RenderSize.Z);
                    shape.Width = newSize.X;
                    shape.Height = newSize.Y;
                }
                else
                {
                    LogEvent.UserInterface.Info("!");
                }
            }
            
        }

        internal static VisualState GenerateVisualStateForControl(Control control, VisualStateDefinition visualStateDefinition)
        {
            var shapeList = new List<Shape>();
            foreach (var shape in visualStateDefinition.Shapes)
            {
                var newShape = (Shape)shape.Copy();
                newShape.ScaleX *= shape.Width;
                newShape.ScaleY *= shape.Height;
                newShape.Parent = control;
                newShape.DesignMode = true;
                newShape.IsInternal = true;
                shapeList.Add(newShape);
            }

            control.Animator.AddAnimations(visualStateDefinition.Animations);
            return new VisualState(control, shapeList);
        }

        public void Update()
        {
            foreach (Shape shape in shapes)
            {
                //var newSize = parent.RenderSize / shape.RenderSize;
                //shape.Width *= newSize.X;
                //shape.Height *= newSize.Y;
                ////shape.Width *= parent.RenderSize.X;
                ////shape.Height *= parent.RenderSize.Y;
                //shape.Layout(parent.RenderSize);
                //if (shape.Width > 0 && shape.Height > 0)
                //{
                //    var newSize = parent.RenderSize / shape.RenderSize;
                //    shape.Width *= newSize.X;
                //    shape.Height *= newSize.Y;
                //}
                //else shape.IsVisible = false;
            }
        }

        public void SynchronizeSize()
        {
            foreach (var shape in shapes)
            {
                if (shape.Width > 0 && shape.Height > 0)
                {
                    var newSize = parent.RenderSize/shape.RenderSize;
                    shape.Width *= newSize.X;
                    shape.Height *= newSize.Y;
                }
                else shape.IsVisible = false;
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
