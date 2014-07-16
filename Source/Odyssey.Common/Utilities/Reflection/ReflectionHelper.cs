#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

#endregion Using Directives

namespace Odyssey.Utilities.Reflection
{
    public static class ReflectionHelper
    {
        public static bool AreTypesDerived(Type[] derivedTypes, Type baseType)
        {
            TypeInfo baseTypeInfo = baseType.GetTypeInfo();
            return derivedTypes.All(t => baseTypeInfo.IsAssignableFrom(t.GetTypeInfo()));
        }

        public static bool ContainsProperty(Type type, string propertyName)
        {
            Contract.Requires<ArgumentNullException>(type != null, "type");

            return type.GetRuntimeProperties().Any(p => p.Name == propertyName);
        }

        public static FieldInfo FindFieldPath(Type type, string path, out PropertyInfo containingProperty)
        {
            Contract.Requires<ArgumentNullException>(type != null, "type");
            TypeInfo currentType = type.GetTypeInfo();
            containingProperty = null;

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetDeclaredProperty(propertyName);

                if (property == null)
                    return currentType.GetDeclaredField(propertyName);
                else
                {
                    currentType = property.PropertyType.GetTypeInfo();
                    containingProperty = property;
                }
            }

            return null;
        }

        public static TAttribute GetAttribute<TAttribute>(Type sourceType)
            where TAttribute : Attribute
        {
            TypeInfo typeInfo = sourceType.GetTypeInfo();
            return typeInfo.GetCustomAttribute<TAttribute>();
        }

        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(Type sourceType)
            where TAttribute : Attribute
        {
            TypeInfo typeInfo = sourceType.GetTypeInfo();
            return typeInfo.GetCustomAttributes<TAttribute>();
        }

        public static IEnumerable<Type> GetDerivedTypes(Assembly assembly, Type type)
        {
            TypeInfo baseTypeInfo = type.GetTypeInfo();
            var types = from typeInfo in assembly.DefinedTypes
                        where baseTypeInfo.IsAssignableFrom(typeInfo)
                        select typeInfo.AsType();

            return types;
        }

        [Pure]
        public static bool IsTypeDerived(Type derivedType, Type baseType)
        {
            return baseType.GetTypeInfo().IsAssignableFrom(derivedType.GetTypeInfo());
        }
    }
}