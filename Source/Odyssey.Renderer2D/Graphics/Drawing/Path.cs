using System;
using System.Collections.Generic;
using Odyssey.Reflection;
using Odyssey.Text;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Serialization;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class Path : Shape
    {
        private PathGeometry pathGeometry;
        public IEnumerable<VectorCommand> Data { get; set; }

        public override bool Contains(Vector2 cursorLocation)
        {
            return false;
        }

        public override void Render()
        {
            Device.Transform = Matrix3x2.Scaling(ScaleX, ScaleY) * Transform;
            if (Fill != null)
            {
                Device.FillGeometry(pathGeometry, Fill);
            }
            if (Stroke != null)
                Device.DrawGeometry(pathGeometry, Stroke, StrokeThickness);
            Device.Transform = Matrix3x2.Identity;
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            if (Data == null)
                throw new InvalidOperationException("'Data' cannot be null");
            pathGeometry = ToDispose(PathGeometry.New(string.Format("{0}.Figure", Name), Device));
            pathGeometry.Initialize();
            pathGeometry.RunCommands(Data);
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            string figureData = e.XmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Path p) => p.Data));
            if (string.IsNullOrEmpty(figureData))
                return;
            string figureId = TextHelper.ParseResource(figureData);
            if (e.Theme.ContainsResource(figureId))
            {
                var figure = e.Theme.GetResource<Figure>(figureId);
                Data = figure.Data;
                ScaleX = figure.ScaleTransformX;
                ScaleY = figure.ScaleTransformY;

            }
            else Data = VectorArtParser.ParsePathData(figureData);
        }

        protected internal override UIElement Copy()
        {
            var copy = (Path)base.Copy();
            copy.Data = Data;
            return copy;
        }
    }
}