using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Reflection;

namespace Odyssey.Epos.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OptionalComponentAttribute : Attribute
    {
        private readonly Type[] componentTypes;
        public IEnumerable<Type> ComponentTypes { get { return componentTypes; } }

        public OptionalComponentAttribute(params Type[] componentTypes)
        {
            foreach (Type componentType in componentTypes
                .Where(componentType => !ReflectionHelper.IsTypeDerived(componentType, typeof(Component))))
                throw new ArgumentException(string.Format("Type [{0}] does not derive from [{1}]", componentType.Name, typeof(Component).Name));
            this.componentTypes = componentTypes;
        }
    }
}
