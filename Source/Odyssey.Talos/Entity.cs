using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Odyssey.Talos.Components;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Utilities.Logging;
using Odyssey.Utilities.Reflection;
using SharpYaml.Serialization;

namespace Odyssey.Talos
{
    [YamlTag("Entity")]
    [DebuggerDisplay("{Name}, Id={Id}")]
    public sealed class Entity : IEntity
    {
        static int count;

        readonly long id;
        Scene scene;
        long key;

        /// <summary>
        /// Returns the progressive id number that identifies this entity instance.
        /// </summary>
        public long Id { get { return id; } }

        [YamlMember(0)]
        public string Name { get; set; }
        [YamlMember(1)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Returns the byte value resulting from the set of components associated to this entity instance.
        /// </summary>
        public long Key { get { return key; } }

        [YamlIgnore]
        public IScene Scene { get { return scene; } }

        [YamlIgnore]
        public IEnumerable<IComponent> Components { get { return Scene.SelectEntityComponents(this); } }

        public Entity() : this("Untitled")
        {
        }

        public Entity(string name)
        {
            Name = name;
            id = count++;
            IsEnabled = true;
        }

        public void RegisterComponent(Component component)
        {
            key |= component.KeyPart;
            component.AssignToScene(scene);
            scene.AddComponentToEntity(component, this);
        }

        public void UnregisterComponent(Component component)
        {
            key &= ~component.KeyPart;
            scene.RemoveComponentFromEntity(component, this);
        }

        internal void AssignToScene(Scene scene)
        {
            this.scene = scene;
        }

        [Pure]
        public bool ContainsComponent(long keyPart)
        {
            return Scene.EntityHasComponent(this, keyPart);
        }

        [Pure]
        public bool ContainsComponent<TComponent>()
            where TComponent : IComponent

        {
            return Scene.EntityHasComponent(this, ComponentTypeManager.GetKeyPart<TComponent>());
        }

        public bool TryGetComponent<TComponent>(long keyPart, out TComponent component)
            where TComponent : IComponent
        {
            return Scene.TryGetEntityComponent(this, keyPart, out component);
        }

        public TComponent GetComponent<TComponent>()
             where TComponent : IComponent
        {
            return GetComponent<TComponent>(ComponentTypeManager.GetKeyPart<TComponent>());
        }

        public TComponent GetComponent<TComponent>(long keyPart)
            where TComponent : IComponent
        {
            return Scene.GetEntityComponent<TComponent>(this, keyPart);
        }

        public IComponent GetComponent(string componentType)
        {
            return GetComponent<IComponent>(ComponentTypeManager.GetKeyPart(componentType));
        }

        bool CheckRequiredComponents()
        {
            bool test = true;
            var attributeTypes = (from component in Components
                                  from attribute in ReflectionHelper.GetAttributes<RequiredComponentAttribute>(component.GetType())
                                  select attribute.ComponentType).Distinct();

            var componentTypes = (from otherComponent in Components select otherComponent.GetType()).ToArray();

            foreach (Type type in attributeTypes.Where(type => !componentTypes.Contains(type)))
            {
                test = false;
                LogEvent.Engine.Error("[{0}] is missing [{1}]", Name, type);
            }
            return test;
        }

        bool CheckOptionalComponents()
        {
            bool test = true;
            var attributeTypes = (from component in Components
                                  from attribute in ReflectionHelper.GetAttributes<OptionalComponentAttribute>(component.GetType())
                                  from componentType in attribute.ComponentTypes
                                      select componentType).Distinct().ToArray();

            if (!attributeTypes.Any())
                return true;

            var componentTypes = (from otherComponent in Components select otherComponent.GetType()).ToArray();

            var intersection = attributeTypes.Intersect(componentTypes);

            if (!intersection.Any())
            {
                test = false;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("[{0}] is missing at least one component between:", Name));
                foreach (Type type in attributeTypes)
                    sb.AppendLine(type.Name);
                LogEvent.Engine.Error(sb.ToString());
            }
            return test;
        }

        public bool Validate()
        {
            bool test = true;

            if (!CheckRequiredComponents() || !CheckOptionalComponents())
                return false;

            foreach (var component in Components)
            {
                bool result = component.Validate();
                if (!result)
                    test = false;
            }

            return test;
        }
    }
}
