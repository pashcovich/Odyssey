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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;

#endregion Using Directives

namespace Odyssey.Graphics.Shapes
{
    public class ShapeMap
    {
        private readonly ControlDescription description;
        private readonly Dictionary<ControlStatus, List<IShape>> map;

        public ShapeMap(ControlDescription description)
        {
            this.description = description;
            map = new Dictionary<ControlStatus, List<IShape>>();
        }

        private IEnumerable<UIElement> AllShapes
        {
            get { return map.Values.SelectMany(shapeList => shapeList).Cast<UIElement>(); }
        }

        public void Add(ControlStatus status, IShape shape)
        {
            ProcessInsertion(status, shape);
        }

        public void Add(ControlStatus status, IEnumerable<IShape> shapes)
        {
            foreach (IShape shape in shapes)
                ProcessInsertion(status, shape);
        }

        public IEnumerable<IShape> GetShapes(ControlStatus status)
        {
            return !HasShapes(status) ? Enumerable.Empty<IShape>() : map[status];
        }

        public bool HasShapes(ControlStatus status)
        {
            return map[status] != null;
        }

        public void Initialize()
        {
            foreach (UIElement shape in AllShapes)
            {
                shape.DesignMode = false;
                shape.Initialize();
            }
        }

        public void Update()
        {
            foreach (UIElement shape in AllShapes)
                shape.Layout();
        }

        private void ProcessInsertion(ControlStatus status, IShape shape)
        {
            Contract.Requires<NullReferenceException>(shape != null);
            shape.FillShader = description.GetFillGradient(status);
            shape.StrokeShader = description.GetStrokeGradient(status);

            if (!map.ContainsKey(status))
                map.Add(status, new List<IShape>());
            map[status].Add(shape);
        }
    }
}