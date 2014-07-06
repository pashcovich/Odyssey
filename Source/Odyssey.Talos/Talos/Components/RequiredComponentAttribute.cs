using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Utilities.Reflection;

namespace Odyssey.Talos.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredComponentAttribute : Attribute
    {
        public Type ComponentType { get; private set; }

        public RequiredComponentAttribute(Type componentType)
        {
            if (!ReflectionHelper.IsTypeDerived(componentType, typeof(Component)))
                throw new ArgumentException(string.Format("Type [{0}] does not derive from [{1}]", componentType.Name, typeof(Component).Name));
            ComponentType = componentType;
        }
    }
}
