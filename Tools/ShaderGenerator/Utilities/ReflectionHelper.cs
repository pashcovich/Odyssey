using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Odyssey.Utils
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> GetDerivedTypes(Type baseClass)
        {
            return (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from lType in lAssembly.GetTypes()
                    where lType.IsSubclassOf(baseClass)
                    select lType);
        }

        public static IEnumerable<Type> SearchTypes(string typeName, string namespaceName)
        {
            return (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from lType in lAssembly.GetTypes()
                    where lType.Name == typeName && lType.Namespace == namespaceName
                    select lType);
        }

        public static bool HasAttribute<TAttribute>(Type containingType)
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(containingType) != null;
        }

        public static TAttribute GetAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attributes = GetAttributes<TAttribute>(type);
            return attributes.Any() ? attributes.First() : null;
        }

        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            return type.GetCustomAttributes<TAttribute>();
        }


        public static PropertyInfo[] GetProperties<T>(object data, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, bool searchDerived = true)
        {
            if (searchDerived)
                return data.GetType().GetProperties(flags).Where(p => typeof(T).IsAssignableFrom(p.PropertyType)).ToArray();
            else
                return data.GetType().GetProperties(flags).Where(p => p.PropertyType == typeof(T)).ToArray();
        }

        public static TEnum ParseEnum<TEnum>(string value)
        {
            Contract.Requires<ArgumentException>(typeof(System.Enum).IsAssignableFrom(typeof(TEnum)));
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
}
