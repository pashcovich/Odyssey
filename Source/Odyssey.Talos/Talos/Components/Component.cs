using Odyssey.Talos.Messages;
using SharpDX;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    public abstract class Component : IComponent
    {
        private static long count;
        private static readonly Dictionary<Type, long> TypeIndex = new Dictionary<Type, long>();
        private Scene scene;
        private readonly long id;
        private readonly ComponentType componentType;

        [YamlMember(0)]
        public string Name { get; set; }

        protected Scene Scene { get { return scene; } }

        /// <summary>
        /// Returns the progressive id number that identifies this component instance.
        /// </summary>
        [YamlIgnore]
        public long Id { get { return id; } }

        [YamlIgnore]
        public long KeyPart { get { return componentType.KeyPart; } }

        [YamlIgnore]
        public ComponentType ComponentType { get { return componentType; } }

        [YamlIgnore]
        public IServiceRegistry Services { get; set; }

        internal Messenger Messenger { get; set; }

        protected Component(ComponentType type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            componentType = type;

            Type thisType = GetType();
            if (!TypeIndex.ContainsKey(thisType))
                TypeIndex.Add(thisType, 1);

            id = count++;
            Name = string.Format("{0}{1:00}", thisType.Name, ++TypeIndex[thisType]);
        }

        protected void RaisePropertyChange(string property)
        {
            if (scene != null && !scene.IsDesignMode)
                Messenger.Send(new PropertyChangeMessage(property, this));
        }

        internal void AssignToScene(Scene scene)
        {
            this.scene = scene;
            Services = scene.Services;
            Messenger = scene.Messenger;
        }

        public virtual bool Validate()
        {
            return true;
        }
    }
}