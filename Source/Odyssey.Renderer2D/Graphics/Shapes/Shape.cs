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
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;

#endregion Using Directives

namespace Odyssey.Graphics.Shapes
{
    public abstract class Shape : UIElement, IShape
    {
        private string cFillGradient;
        private string cStrokeGradient;

        protected Shape()
        {
            StrokeThickness = 1.0f;
        }

        public static Color4 DefaultFillColor = Color.MidnightBlue;
        public static Color4 DefaultStrokeColor = Color.DimGray;

        internal Brush Fill { get; set; }
        internal Brush Stroke { get; set; }

        public string FillGradientClass
        {
            get { return cFillGradient; }
            set
            {
                if (string.Equals(cFillGradient, value))
                    return;

                if (DesignMode)
                    return;

                cFillGradient = value;
                FillGradient = Overlay.StyleService.GetGradient(Overlay.ControlTheme, cFillGradient);
            }
        }

        public string StrokeGradientClass
        {
            get { return cStrokeGradient; }
            set
            {
                if (string.Equals(cStrokeGradient, value))
                    return;

                if (DesignMode)
                    return;

                cStrokeGradient = value;
                StrokeGradient = Overlay.StyleService.GetGradient(Overlay.ControlTheme, cStrokeGradient);
            }
        }

        public Gradient FillGradient { get; set; }
        public Gradient StrokeGradient { get; set; }

        public float StrokeThickness { get; set; }

        internal override UIElement Copy()
        {
            Shape copy = (Shape)base.Copy();
            copy.FillGradientClass = FillGradientClass;
            copy.StrokeGradientClass = StrokeGradientClass;
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

        protected override void OnLayoutUpdated(System.EventArgs e)
        {
            base.OnLayoutUpdated(e);
            if (Fill != null)
                Fill.Transform = Matrix3x2.Scaling(Width, Height) * Transform;
        }

        protected override void OnReadXml(System.Xml.XmlReader reader)
        {
            
        }

    }
}