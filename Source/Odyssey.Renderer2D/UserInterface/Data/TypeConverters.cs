using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Data
{
    internal static class TypeConverters
    {
        internal static bool CanConvertTo<T>(Type destinationType)
        {
            Contract.Requires<ArgumentNullException>(destinationType!=null, "destinationType");
            return (destinationType == typeof(string)) || destinationType.GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo());
        }

        internal static bool CanConvertFrom<T>(Type sourceType)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null, "sourceType");
            return (sourceType == typeof(string)) || typeof(T).GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo());
        }
    }
}
