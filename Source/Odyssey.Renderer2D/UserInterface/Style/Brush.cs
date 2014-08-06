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
using SharpDX;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public abstract class Brush : Direct2DResource
    {
        protected new readonly SharpDX.Direct2D1.Brush Resource;

        protected Brush(string name, Direct2DDevice device, SharpDX.Direct2D1.Brush brush)
            : base(name, device)
        {
            Resource = brush;
        }

        public override void Initialize()
        {
            Initialize(ToDispose(Resource));
        }

        public Matrix3x2 Transform
        {
            get { return Resource.Transform; }
            set { Resource.Transform = value; }
        }

        /// <summary>
        /// <see cref="SharpDX.Direct2D1.Brush"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct2D1.Brush(Brush from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource ?? null;
        }
    }
}
