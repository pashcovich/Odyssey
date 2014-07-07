using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredServiceAttribute : Attribute
    {
        public Type ServiceType { get; private set; }
        public Type ClassType { get; private set; }

        public RequiredServiceAttribute(Type serviceType, Type classType)
        {
            ClassType = classType;
            ServiceType = serviceType;
        }
    }
}
