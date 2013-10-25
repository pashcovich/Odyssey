using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
