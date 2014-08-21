using System.Runtime.InteropServices;
using Odyssey.Graphics;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;
using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Talos.Maps
{
    public class EntityMap
    {
        readonly Scene scene;
        readonly Dictionary<long, Dictionary<long, Component>> componentsByEntity;
        readonly Dictionary<long, Entity> entities;
        private readonly Dictionary<long, Component> components; 

        public event EventHandler<EntityEventArgs> EntityAdded;
        public event EventHandler<EntityEventArgs> EntityRemoved;
        public event EventHandler<EntityComponentChangedEventArgs> EntityComponentAdded;
        public event EventHandler<EntityComponentChangedEventArgs> EntityComponentRemoved;

        protected Messenger Messenger { get { return scene.Messenger; } }
        public IEnumerable<Entity> Entities { get { return entities.Values; } }
        public IEnumerable<Component> Components { get { return components.Values; } } 

        public EntityMap(Scene scene)
        {
            this.scene = scene;
            entities = new Dictionary<long, Entity>();
            components = new Dictionary<long, Component>();
            componentsByEntity = new Dictionary<long, Dictionary<long, Component>>();
        }

        protected virtual void OnEntityAdded(EntityEventArgs args)
        {
            var handler = EntityAdded;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityRemoved(EntityEventArgs args)
        {
            var handler = EntityRemoved;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityComponentRemoved(EntityComponentChangedEventArgs args)
        {
            var handler = EntityComponentRemoved;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnEntityComponentAdded(EntityComponentChangedEventArgs args)
        {
            var handler = EntityComponentAdded;
            if (handler != null)
                handler(this, args);
        }

        public void AddEntity(Entity entity)
        {
            if (entity.Scene != null && entity.Scene != scene)
            {
                RemoveEntity(entity);
                LogEvent.Engine.Warning("Entity was already registered in another scene.");
            }
            componentsByEntity.Add(entity.Id, new Dictionary<long, Component>());
            entities.Add(entity.Id, entity);
            OnEntityAdded(new EntityEventArgs(entity));
        }

        public void RemoveEntity(Entity entity)
        {
            if (entity.Scene != null && entity.Scene != scene)
            {
                RemoveEntity(entity);
                LogEvent.Engine.Warning("Entity was already registered in another scene.");
            }
            componentsByEntity.Remove(entity.Id);
            entities.Remove(entity.Id);
            OnEntityRemoved(new EntityEventArgs(entity));
        }

        public Entity SelectEntity(long id)
        {
            return entities[id];
        }

        public int Count
        {
            get { return entities.Count; }
        }

        public bool ContainsEntity(Entity entity)
        {
            return entities.ContainsKey(entity.Id);
        }

        public Entity GetEntity(long entityId)
        {
            return entities[entityId];
        }

        public void Unload()
        {
            foreach (IDisposable disposableComponent in components.Values.OfType<IDisposable>())
                disposableComponent.Dispose();
        }

        public bool Validate()
        {
            bool test = true;
            foreach (Entity entity in entities.Values)
            {
                bool result = entity.Validate();
                if (!result)
                    test = false;
            }
            return test;
        }

        public IEnumerable<Entity> SelectChildren(Entity parent)
        {
            Contract.Requires<ArgumentNullException>(parent!=null, "parent");
            var childEntities = from e in Entities
                where e.ContainsComponent<ParentComponent>()
                let cParent = e.GetComponent<ParentComponent>()
                where cParent.Parent == parent
                select e;
            return childEntities;
        }

        public IEnumerable<Entity> PreOrderVisit(Entity root)
        {
            foreach (var entity in SelectChildren(root))
            {
                yield return entity;
                foreach (var childEntity in SelectChildren(entity))
                    yield return childEntity;
            }
        }

        public Entity FindChild(Entity parent, string name)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name), "name");
            return PreOrderVisit(parent).FirstOrDefault(e => string.Equals(e.Name, name));
        }

        public IEnumerable<TComponent> SelectComponents<TComponent>()
            where TComponent : Component
        {
            return components.Values.OfType<TComponent>();
        }

        public IEnumerable<Component> SelectEntityComponents(Entity entity)
        {
            return componentsByEntity[entity.Id].Values;
        }

        public TComponent GetEntityComponent<TComponent>(Entity entity, long keyPart)
            where TComponent : Component
        {
            return (TComponent)componentsByEntity[entity.Id][keyPart];
        }

        public bool TryGetEntityComponent<TComponent>(Entity entity, long keyPart, out TComponent component)
            where TComponent : Component
        {
            Component componentTemp;
            bool test = componentsByEntity[entity.Id].TryGetValue(keyPart, out componentTemp);
            component = (TComponent) componentTemp;
            return test;
        }

        public int CountEntities(Func<Entity, bool> function)
        {
            return entities.Count(kvp => function(kvp.Value));
        }

        public bool EntityHasComponent(Entity entity, long keyPart)
        {
            return componentsByEntity[entity.Id].ContainsKey(keyPart);
        }

        public void AddComponentToEntity(Component component, Entity entity)
        {
            componentsByEntity[entity.Id].Add(component.KeyPart, component);
            if (!components.ContainsKey(component.Id))
                components.Add(component.Id, component);
            OnEntityComponentAdded(new EntityComponentChangedEventArgs(entity, component));
        }

        public void RemoveComponentFromEntity(Component component, Entity entity)
        {
            componentsByEntity[entity.Id].Remove(component.KeyPart);
            components.Remove(component.Id);
            OnEntityComponentRemoved(new EntityComponentChangedEventArgs(entity, component));
        }

    }
}
