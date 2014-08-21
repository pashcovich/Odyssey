using Odyssey.Talos.Messages;
using Odyssey.Talos.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Talos.Maps
{
    public sealed class SystemMap
    {
        readonly Scene scene;
        readonly LinkedList<SystemBase> systems;
        readonly Dictionary<SystemBase, long> systemIds = new Dictionary<SystemBase, long>();
        readonly Dictionary<long, List<Entity>> entitiesBySystem;

        public IEnumerable<SystemBase> Systems { get { return systems; } }

        public event EventHandler<SystemEventArgs> SystemAdded;
        public event EventHandler<SystemEventArgs> SystemRemoved;

        public SystemMap(Scene scene)
        {
            this.scene = scene;
            systems = new LinkedList<SystemBase>();
            systemIds = new Dictionary<SystemBase, long>();
            entitiesBySystem = new Dictionary<long, List<Entity>>();
        }

        public void AddSystem(SystemBase system)
        {
            systems.AddLast(system);
            systemIds.Add(system, system.Id);
            entitiesBySystem.Add(system.Id, new List<Entity>());
            system.AssignToScene(scene);
            OnSystemAdded(new SystemEventArgs(system));
        }

        public void RemoveSystem(SystemBase system)
        {
            system.Stop();
            system.Unload();
            systems.Remove(system);
            systemIds.Remove(system);
            entitiesBySystem.Remove(system.Id);
            OnSystemRemoved(new SystemEventArgs(system));
            
        }

        void OnSystemAdded(SystemEventArgs args)
        {
            var handler = SystemAdded;
            if (handler != null)
                handler(this, args);
        }

        void OnSystemRemoved(SystemEventArgs args)
        {
            var handler = SystemRemoved;
            if (handler != null)
                handler(this, args);
        }

        public bool SystemHasEntities(SystemBase system)
        {
            return CountSystemEntities(system) > 0;
        }

        public int CountSystemEntities(SystemBase system)
        {
            Contract.Requires<ArgumentNullException>(system != null);

            return entitiesBySystem[system.Id].Count;
        }

        public SystemBase GetSystem(Entity entity)
        {
            return systems.First(s => s.Selector.Interests(entity.Key));
        }

        void AddEntityToSystems(Entity entity)
        {
            foreach (SystemBase system in systems)
            {
                if (system.Supports(entity.Key))
                    RegisterEntityToSystem(entity, system);
            }
        }

        void RemoveEntityFromSystems(Entity entity)
        {
            foreach (SystemBase system in systems)
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

        internal void OnEntityComponentAdded(object sender, EntityComponentChangedEventArgs args)
        {
            AddEntityToSystems(args.Source);
        }

        internal void OnEntityComponentRemoved(object sender, EntityComponentChangedEventArgs args)
        {
            RemoveEntityFromSystems(args.Source);
        }

        internal void RegisterEntityToSystem(Entity entity, SystemBase system)
        {
            if (!entitiesBySystem.ContainsKey(system.Id))
                entitiesBySystem.Add(system.Id, new List<Entity>());

            if (!entitiesBySystem[system.Id].Contains(entity))
            {
                entitiesBySystem[system.Id].Add(entity);
                scene.Messenger.SendTo(new EntityChangeMessage(entity, ChangeType.Added), system);
            }
        }

        internal void ClearAllEntitiesFromSystem(SystemBase system)
        {
            var entitiesCopy = new List<Entity>(entitiesBySystem[system.Id]);
            foreach (Entity entity in entitiesCopy)
                UnregisterEntityFromSystem(entity, system);
        }

        internal void UnregisterEntityFromSystem(Entity entity, SystemBase system)
        {
            entitiesBySystem[system.Id].Remove(entity);

            scene.Messenger.SendTo(new EntityChangeMessage(entity, ChangeType.Removed), system);
        }

        internal bool IsEntityRegisteredToSystem(IEntity entity, SystemBase system)
        {
            return entitiesBySystem[system.Id].Any(e => e.Id == entity.Id);
        }

        /// <summary>
        /// Selects all entities that have been assigned to this system./>.
        /// </summary>
        /// <param name="system">The <see cref="System"/>.</param>
        /// <returns>A list of matching entities.</returns>
        public IEnumerable<Entity> SelectAllEntities(SystemBase system)
        {
            return entitiesBySystem[system.Id];
        }


        #region Selection Methods
        /// <summary>
        /// Selects the systems that support the <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The list of systems.</returns>
        public IEnumerable<SystemBase> SelectSupportedSystems(IEntity entity)
        {
            return systems.Where(s => s.Supports(entity.Key));
        }

        #endregion

        public void Unload()
        {
            foreach (SystemBase system in systems)
            {
                system.Stop();
                system.Unload();
            }
        }
    }
}
