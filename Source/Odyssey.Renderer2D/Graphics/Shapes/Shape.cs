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
        protected Shape()
        {
            StrokeThickness = 1.0f;
        }

        public static Color4 DefaultFillColor = Color.MidnightBlue;
        public static Color4 DefaultStrokeColor = Color.DimGray;

        internal string FillBrushClass { get; set; }
        internal string StrokeBrushClass { get; set; }

        [CacheAnimation]
        public Brush Fill { get; private set; }
        [CacheAnimation]
        public Brush Stroke { get; private set; }
        
        public float StrokeThickness { get; set; }

        internal override UIElement Copy()
        {
            Shape copy = (Shape)base.Copy();
            copy.FillBrushClass = FillBrushClass;
            copy.StrokeBrushClass = StrokeBrushClass;
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
            var fillColor = Overlay.Theme.GetResource<ColorResource>(FillBrushClass);
            var strokeColor = Overlay.Theme.GetResource<ColorResource>(StrokeBrushClass);

            var styleService = Device.Services.GetService<IStyleService>();
            if (styleService.ContainsResource(FillBrushClass))
                Fill = styleService.GetResource<Brush>(FillBrushClass);
            else
            {
                Fill = Brush.FromColorResource(Device, fillColor);
                styleService.AddResource(Fill);
            }

            if (styleService.ContainsResource(StrokeBrushClass))
                Stroke = styleService.GetResource<Brush>(StrokeBrushClass);
            else
            {
                Stroke = Brush.FromColorResource(Device, strokeColor);
                styleService.AddResource(Stroke);
            }

            Fill.Initialize();
            Stroke.Initialize();

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
                FillBrushClass = Text.ParseResource(sFill);
            }
            if (!string.IsNullOrEmpty(sStroke))
            {
                StrokeBrushClass = Text.ParseResource(sStroke);
            }

            if (!reader.IsEmptyElement)
                reader.ReadEndElement();
        }
        
        public void CacheAnimation(string propertyName, IAnimationCurve animationCurve)
        {
            var property = (from p in ReflectionHelper.GetProperties(GetType())
                where p.GetCustomAttribute<CacheAnimationAttribute>() != null
                && p.Name == propertyName
                select p).FirstOrDefault();

            if (property != null)
            {
                var styleService = Overlay.Services.GetService<IStyleService>();
                var curve = (Color4Curve) animationCurve;
                GradientBrush brush = (GradientBrush) property.GetValue(this);
                var gradientStopCollection = brush.GradientStops.Copy();

                foreach (var gradientStop in brush.GradientStops)
                {
                    gradientStop.PropertyChanged += (s, e) => { Fill = styleService.GetResource<Brush>(brush.Name + 0.ToString()); };
                }

                int index = 0;
                foreach (var keyframe in curve)
                {
                    gradientStopCollection[0].Color = keyframe.Value;
                    Gradient gradient;
                    var bLinearGradient = brush as LinearGradientBrush;
                    if (bLinearGradient != null)
                    {
                        gradient = new LinearGradient(brush.Name + index++, bLinearGradient.StartPoint, bLinearGradient.EndPoint,
                            gradientStopCollection);
                        var newBrush = Brush.FromColorResource(Device, gradient);
                        newBrush.Initialize();

                        styleService.AddResource(newBrush);
                    }
                }

            }

        }
    }
}