using System;

namespace Odyssey.Epos.Maps
{
    public class EntityEventArgs : EventArgs
    {
        public Entity Source { get; private set; }

        public EntityEventArgs(Entity entity)
        {
            Source = entity;
        }
    }
}
