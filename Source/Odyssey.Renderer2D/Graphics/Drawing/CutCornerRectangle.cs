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
using Odyssey.UserInterface.Controls;
using SharpDX;

#endregion Using Directives

namespace Odyssey.Graphics.Drawing
{
    public class CutCornerRectangle : CutCornerRectangleBase
    {
        private PolyLine shape;
        private Vector2[] points;

        protected PolyLine Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public override void Render()
        {
            Device.Transform = Transform;
            if (Fill != null)
            {
                Fill.Transform = Matrix3x2.Scaling(Width, Height)*Transform;
                Device.FillGeometry(shape, Fill);
            }
            if (Stroke!=null)
                Device.DrawGeometry(shape, Stroke);
            Device.Transform = Matrix3x2.Identity;
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            SetupShapes();
        }

        void SetupShapes()
        {
            points = new[]
            {
                new Vector2(CutCornerLength, 0),
                new Vector2(Width - CutCornerLength, 0),
                new Vector2(Width, CutCornerLength),
                new Vector2(Width, Height - CutCornerLength),
                new Vector2(Width - CutCornerLength, Height),
                new Vector2(CutCornerLength, Height),
                new Vector2(0, Height - CutCornerLength),
                new Vector2(0, CutCornerLength)
            };
            RemoveAndDispose(ref shape);
            shape = ToDispose(PolyLine.New(string.Format("PL.{0}", Name), Device, points, FigureBegin.Filled, FigureEnd.Closed));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetupShapes();
        }


    }
}