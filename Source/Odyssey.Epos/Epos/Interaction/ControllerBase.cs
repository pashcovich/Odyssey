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

#endregion

#region Using Directives

using System;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Epos.Interaction
{
    public abstract class ControllerBase : IEntityController
    {
        private readonly IServiceRegistry services;
        private PositionComponent cPosition;
        private OrientationComponent cOrientation;
        public bool IsEnabled { get; set; }

        public ControllerBase(IServiceRegistry services)
        {
            this.services = services;
        }

        protected OrientationComponent COrientation
        {
            get { return cOrientation; }
        }

        protected PositionComponent CPosition
        {
            get { return cPosition; }
        }

        protected Entity Source { get; private set; }

        protected IServiceRegistry Services
        {
            get { return services; }
        }

        public virtual void BindToEntity(Entity source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            Source = source;
            const string errorFormat = "'{0}' does not contain a {1}";
            if (!Source.TryGetComponent(out cPosition))
                throw new InvalidOperationException(string.Format(errorFormat, source.Name, typeof(PositionComponent)));
            if (!source.TryGetComponent(out cOrientation))
                throw new InvalidOperationException(string.Format(errorFormat, source.Name, typeof(OrientationComponent)));
        }

        public abstract void Update(ITimeService time);
    }
}