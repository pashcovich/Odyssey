using System;
using System.Collections.Generic;
using Odyssey.Reflection;
using Odyssey.Text;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Events;
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

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            Redraw();
            return availableSizeWithoutMargins;
        }

        protected virtual void Redraw()
        {
            if (Data == null)
                throw new InvalidOperationException("'Data' cannot be null");
            RemoveAndDispose(ref pathGeometry);
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

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);

            if (e.OldSize.X == 0 || e.OldSize.Y == 0) 
                return;
            ScaleX *= e.NewSize.X/e.OldSize.X;
            ScaleY *= e.NewSize.Y/e.OldSize.Y;

            if (e.NewSize.X > 0 && e.NewSize.Y > 0)
                Redraw();
        }

        protected internal override UIElement Copy()
        {
            var copy = (Path)base.Copy();
            copy.Data = Data;
            return copy;
        }

        public static Path FromFigure(Overlay overlay, string figureKey)
        {
            var theme = overlay.Theme;
            var styleService = overlay.Services.GetService<IStyleService>();
            var figure = theme.GetResource<Figure>(figureKey);

            var path = new Path {Data = figure.Data, StrokeThickness = figure.StrokeThickness, ScaleX = figure.ScaleTransformX, ScaleY = figure.ScaleTransformY};

            if (!string.IsNullOrEmpty(figure.Stroke))
                path.Stroke = styleService.GetBrushResource(figure.Stroke, theme);
            if (!string.IsNullOrEmpty(figure.Fill))
                path.Fill = styleService.GetBrushResource(figure.Fill, theme);

            return path;
        }
    }
}