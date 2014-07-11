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

using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.Graphics
{
    internal class ShapeInitializer
    {
        private readonly Direct2DDevice device;
        private readonly List<Direct2DResource> resources;

        public ShapeInitializer(Direct2DDevice device)
        {
            this.device = device;
            resources = new List<Direct2DResource>();
        }

        public IEnumerable<Direct2DResource> CreatedResources
        {
            get { return resources; }
        }

        public void Initialize(Shape shape)
        {
            if (shape.FillShader == null)
                shape.FillShader = LinearGradient.CreateUniform(Shape.DefaultFillColor);
            if (shape.StrokeShader == null)
                shape.StrokeShader = LinearGradient.CreateUniform(Shape.DefaultStrokeColor);

            switch (shape.FillShader.Type)
            {
                default:
                    shape.Stroke = SolidBrush.New(device, shape.StrokeShader.GradientStops[0].Color);
                    shape.Fill = SolidBrush.New(device, shape.FillShader.GradientStops[0].Color);
                    break;
            }

            resources.Add(shape.Stroke);
            resources.Add(shape.Fill);
        }
    }
}