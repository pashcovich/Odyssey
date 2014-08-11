#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using System;
using System.Linq;
using System.Reflection;
using Odyssey.Animations;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Serialization;
using Odyssey.UserInterface.Style;
using Odyssey.Utilities.Reflection;
using Odyssey.Utilities.Text;
using SharpDX;

#endregion Using Directives

namespace Odyssey.Graphics.Shapes
{
    public abstract class Shape : UIElement, IShape, IRequiresCaching
    {
        public static Color4 DefaultFillColor = Color.MidnightBlue;
        public static Color4 DefaultStrokeColor = Color.DimGray;

        private string fillBrushClass;
        private string strokeBrushClass;

        [CacheAnimation(typeof(GradientStop), "Color")]
        public Brush Fill { get; set; }
        [CacheAnimation(typeof(GradientStop), "Color")]
        public Brush Stroke { get; set; }
        
        public float StrokeThickness { get; set; }

        internal override UIElement Copy()
        {
            Shape copy = (Shape)base.Copy();
            copy.fillBrushClass = fillBrushClass;
            copy.strokeBrushClass = strokeBrushClass;
            copy.StrokeThickness = StrokeThickness;
            return copy;
        }

        public static TShape FromControl<TShape>(Control control, string shapeName)
            where TShape : UIElement, IShape, new()
        {
            TShape shape = new TShape()
            {
                Name = shapeName,
                Width = control.Width,
                Height = control.Height,
                AbsolutePosition = control.AbsolutePosition,
                Margin = control.Margin,
            };

            return shape;
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            var styleService = Device.Services.GetService<IStyleService>();
            if (!string.IsNullOrEmpty(fillBrushClass))
            {
                var fillColor = Overlay.Theme.GetResource<ColorResource>(fillBrushClass);
                if (styleService.ContainsResource(fillBrushClass))
                    Fill = styleService.GetResource<Brush>(fillBrushClass);
                else
                {
                    Fill = Brush.FromColorResource(Device, fillColor);
                    styleService.AddResource(Fill);
                }
                Fill.Initialize();
            }

            if (!string.IsNullOrEmpty(strokeBrushClass))
            {
                var strokeColor = Overlay.Theme.GetResource<ColorResource>(strokeBrushClass);
                if (styleService.ContainsResource(strokeBrushClass))
                    Stroke = styleService.GetResource<Brush>(strokeBrushClass);
                else
                {
                    Stroke = Brush.FromColorResource(Device, strokeColor);
                    styleService.AddResource(Stroke);
                }
                Stroke.Initialize();
            }
        }


        protected override void OnLayoutUpdated(EventArgs e)
        {
            base.OnLayoutUpdated(e);
            if (Fill != null)
                Fill.Transform = Matrix3x2.Scaling(Width, Height) * Transform;
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            var reader = e.XmlReader;
            string strokeThickness = reader.GetAttribute("StrokeThickness");
            StrokeThickness = string.IsNullOrEmpty(strokeThickness) ? 0 : float.Parse(strokeThickness);

            string sFill = reader.GetAttribute("Fill");
            string sStroke = reader.GetAttribute("Stroke");
            if (!string.IsNullOrEmpty(sFill))
            {
                fillBrushClass = Text.ParseResource(sFill);
            }
            if (!string.IsNullOrEmpty(sStroke))
            {
                strokeBrushClass = Text.ParseResource(sStroke);
            }

            if (!reader.IsEmptyElement)
                reader.ReadEndElement();
        }
        
        public IAnimationCurve CacheAnimation(Type type, string propertyName, IAnimationCurve animationCurve)
        {
            // Checks whether the curve affects a property marked with the CacheAnimationAttribute
            var property = (from p in ReflectionHelper.GetProperties(GetType())
                let attribute = p.GetCustomAttribute<CacheAnimationAttribute>()
                where attribute != null && attribute.Type == type && attribute.PropertyName == propertyName
                select p).FirstOrDefault();

            if (property != null)
            {
                object value = property.GetValue(this);

                var bGradient = value as GradientBrush;
                if (value == null)
                    throw new NotImplementedException("Solid Color animation not yet supported");
                return GradientBrushCurve.FromColor4Curve(Device, (Color4Curve) animationCurve, bGradient);
            }

            return null;
        }
    }
}