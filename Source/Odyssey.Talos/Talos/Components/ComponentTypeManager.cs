using Odyssey.Utilities.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Odyssey.Talos.Components
{
    public static class ComponentTypeManager
    {
        static readonly Dictionary<Type, ComponentType> ComponentTypes = new Dictionary<Type, ComponentType>();

        static ComponentTypeManager()
        {
            InitalizeTypes(typeof(ComponentTypeManager).GetTypeInfo().Assembly);
        }

        public static long GetKeyPart<T>() where T : IComponent
        {
            return GetType<T>().KeyPart;
        }

        public static long GetKeyPart(Type componentType)
        {
            return GetType(componentType).KeyPart;
        }

        public static ComponentType GetType<T>() where T : IComponent
        {
            return GetType(typeof(T));
        }

        public static ComponentType GetType(Type component)
        {
            Contract.Requires<ArgumentNullException>(component != null);

            ComponentType result;
            if (!ComponentTypes.TryGetValue(component, out result))
            {
                result = new ComponentType();
                ComponentTypes.Add(component, result);
            }

            return result;
        }

        public static bool IsComponentRegistered<T>()
            where T:IComponent
        {
            return IsComponentRegistered(typeof(T));
        }

        public static bool IsComponentRegistered(Type componentType)
        {
            return ComponentTypes.ContainsKey(componentType);
        }

        public static void InitalizeTypes(params Assembly[] assemblies)
        {
            Type componentType = typeof(IComponent);
            TypeInfo componentTypeInfo = componentType.GetTypeInfo();

            foreach (Assembly assembly in assemblies)
            {
                var derivedTypes = ReflectionHelper.GetDerivedTypes(assembly, componentType);
                foreach (Type type in derivedTypes)
                {
                    TypeInfo typeInfo = type.GetTypeInfo();
                    if (!componentTypeInfo.IsAssignableFrom(typeInfo))
                        continue;

                    if (typeInfo.IsInterface || typeInfo.IsAbstract)
                        continue;

                    GetType(type);
                }
            }
        }
    }
}
