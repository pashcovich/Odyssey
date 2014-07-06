using Odyssey.Talos.Messages;
using Odyssey.Talos.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Talos.Maps
{
    public class SystemMap
    {
        readonly Scene scene;
        readonly LinkedList<ISystem> systems;
        readonly Dictionary<ISystem, long> systemIds = new Dictionary<ISystem, long>();
        readonly Dictionary<long, List<IEntity>> entitiesBySystem;

        public IEnumerable<ISystem> Systems { get { return systems; } }

        public event EventHandler<SystemEventArgs> SystemAdded;
        public event EventHandler<SystemEventArgs> SystemRemoved;

        public SystemMap(Scene scene)
        {
            this.scene = scene;
            systems = new LinkedList<ISystem>();
            systemIds = new Dictionary<ISystem, long>();
            entitiesBySystem = new Dictionary<long, List<IEntity>>();
        }

        public void AddSystem(ISystem system)
        {
            systems.AddLast(system);
            systemIds.Add(system, system.Id);
            entitiesBySystem.Add(system.Id, new List<IEntity>());
            system.AssignToScene(scene);
            OnSystemAdded(new SystemEventArgs(system));
        }

        public void RemoveSystem(ISystem system)
        {
            system.Stop();
            system.Unload();
            systems.Remove(system);
            systemIds.Remove(system);
            entitiesBySystem.Remove(system.Id);
            OnSystemRemoved(new SystemEventArgs(system));
            
        }

        protected virtual void OnSystemAdded(SystemEventArgs args)
        {
            var handler = SystemAdded;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnSystemRemoved(SystemEventArgs args)
        {
            var handler = SystemRemoved;
            if (handler != null)
                handler(this, args);
        }

        public bool SystemHasEntities(ISystem system)
        {
            return CountSystemEntities(system) > 0;
        }

        public int CountSystemEntities(ISystem system)
        {
            Contract.Requires<ArgumentNullException>(system != null);

            return entitiesBySystem[system.Id].Count;
        }

        public ISystem GetSystem(IEntity entity)
        {
            return systems.First(s => s.Aspect.Interests(entity.Key));
        }

        void AddEntityToSystems(IEntity entity)
        {
            foreach (ISystem system in systems)
            {
                if (system.Supports(entity.Key))
                    RegisterEntityToSystem(entity, system);
            }
        }

        void RemoveEntityFromSystems(IEntity entity)
        {
            foreach (ISystem system in systems)
            {
                if (system.Supports(entity.Key))
                    UnregisterEntityFromSystem(entity, system);
            }
        }

        internal void OnEntityAdded(object sender, EntityEventArgs args)
        {
            AddEntityToSystems(args.Source);
        }

        internal void OnEntityRemoved(object sender, EntityEventArgs args)
        {
            RemoveEntityFromSystems(args.Source);
        }

        internal void OnEntityComponentAdded(object sender, EntityChangedEventArgs args)
        {
            AddEntityToSystems(args.Source);
        }

        internal void OnEntityComponentRemoved(object sender, EntityChangedEventArgs args)
        {
            RemoveEntityFromSystems(args.Source);
        }

        internal void RegisterEntityToSystem(IEntity entity, ISystem system)
        {
            if (!entitiesBySystem.ContainsKey(system.Id))
                entitiesBySystem.Add(system.Id, new List<IEntity>());

            if (!entitiesBySystem[system.Id].Contains(entity))
            {
                entitiesBySystem[system.Id].Add(entity);
                scene.Messenger.SendTo(new EntityChangeMessage(entity, ChangeType.Added), system);
            }
        }

        internal void ClearAllEntitiesFromSystem(ISystem system)
        {
            var entitiesCopy = new List<IEntity>(entitiesBySystem[system.Id]);
            foreach (IEntity entity in entitiesCopy)
                UnregisterEntityFromSystem(entity, system);
        }

        internal void UnregisterEntityFromSystem(IEntity entity, ISystem system)
        {
            entitiesBySystem[system.Id].Remove(entity);

            scene.Messenger.SendTo(new EntityChangeMessage(entity, ChangeType.Removed), system);
        }

        internal bool IsEntityRegisteredToSystem(IEntity entity, ISystem system)
        {
            return entitiesBySystem[system.Id].Any(e => e.Id == entity.Id);
        }

        /// <summary>
        /// Selects all entities that have been assigned to this system./>.
        /// </summary>
        /// <param name="system">The <see cref="System"/>.</param>
        /// <returns>A list of matching entities.</returns>
        public IEnumerable<IEntity> SelectAllEntities(ISystem system)
        {
            return entitiesBySystem[system.Id];
        }


        #region Selection Methods
        /// <summary>
        /// Selects the systems that support the <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The list of systems.</returns>
        public IEnumerable<ISystem> SelectSupportedSystems(IEntity entity)
        {
            return systems.Where(s => s.Supports(entity.Key));
        }

        #endregion

        public void Unload()
        {
            foreach (ISystem system in systems)
            {
                system.Stop();
                system.Unload();
            }
        }
    }
}
