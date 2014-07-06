using System;

namespace Odyssey.Talos.Maps
{
    public class EntityEventArgs : EventArgs
    {
        public IEntity Source { get; private set; }

        public EntityEventArgs(IEntity entity)
        {
            Source = entity;
        }
    }
}
