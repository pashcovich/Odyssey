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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Odyssey.Talos.Components;
using Odyssey.Utilities.Logging;
using Odyssey.Utilities.Reflection;
using SharpYaml.Serialization;

#endregion

namespace Odyssey.Talos
{
    [YamlTag("Entity")]
    [DebuggerDisplay("{Name}, Id={Id}")]
    public sealed class Entity : IEntity
    {
        private static int count;

        private readonly long id;
        private long key;
        private Scene scene;

        public IEnumerable<IEntity> Children { get { return scene.GetChildren(this); } }

        public Entity() : this("Untitled")
        {
        }

        public Entity(string name)
        {
            Name = name;
            id = count++;
            IsEnabled = true;
        }

        /// <summary>
        /// Returns the progressive id number that identifies this entity instance.
        /// </summary>
        public long Id
        {
            get { return id; }
        }

        [YamlMember(0)]
        public string Name { get; set; }

        [YamlMember(1)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Returns the byte value resulting from the set of components associated to this entity instance.
        /// </summary>
        public long Key
        {
            get { return key; }
        }

        [YamlIgnore]
        public IScene Scene
        {
            get { return scene; }
        }

        [YamlIgnore]
        public IEnumerable<IComponent> Components
        {
            get { return Scene.SelectEntityComponents(this); }
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

        public bool TryGetComponent<TComponent>(out TComponent component)
            where TComponent : IComponent
        {
            return Scene.TryGetEntityComponent(this, ComponentTypeManager.GetKeyPart<TComponent>(), out component);
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

        public IComponent GetComponent(Type componentType)
        {
            Contract.Requires<ArgumentNullException>(componentType!= null, "componentType");
            Contract.Requires<ArithmeticException>(ReflectionHelper.IsTypeDerived(componentType, typeof(IComponent)));
            return Scene.GetEntityComponent<IComponent>(this, ComponentTypeManager.GetKeyPart(componentType));
        }

        public IEntity FindChild(string name)
        {
            return scene.FindChild(this, name);
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

        public void RegisterComponent(Component component)
        {
            Contract.Requires<ArgumentNullException>(component != null, "component");
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

        private bool CheckRequiredComponents()
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

        private bool CheckOptionalComponents()
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
    }
}