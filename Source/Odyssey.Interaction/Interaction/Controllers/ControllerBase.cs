using Odyssey.Engine;
using Odyssey.Talos;
using Odyssey.Talos.Components;
using System;
using System.Diagnostics.Contracts;
using SharpDX;

namespace Odyssey.Interaction.Controllers
{
    public abstract class ControllerBase : IEntityController
    {
        private PositionComponent cPosition;
        private RotationComponent cRotation;
        private UpdateComponent cUpdate;
        protected RotationComponent CRotation
        {
            get { return cRotation; }
        }

        protected UpdateComponent CUpdate
        {
            get { return cUpdate; }
        }

        protected PositionComponent CPosition { get { return cPosition; } }

        protected IEntity Source { get; private set; }

        public virtual void BindToEntity(IEntity source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            Source = source;
            if (!Source.TryGetComponent(out cPosition))
                throw new InvalidOperationException(string.Format("'{0}' does not contain a {1}", source.Name, cPosition.GetType()));
            if (!source.TryGetComponent(out cRotation))
                throw new InvalidOperationException(string.Format("'{0}' does not contain a {1}", source.Name, cRotation.GetType()));
            if (!source.TryGetComponent(out cUpdate))
                throw new InvalidOperationException(string.Format("'{0}' does not contain a {1}", source.Name, cUpdate.GetType()));
        }

        public abstract void Update(ITimeService time);

    }
}