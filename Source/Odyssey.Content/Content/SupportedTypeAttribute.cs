using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Content
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedTypeAttribute : Attribute
    {
        Type supportedType;

        public Type SupportedType { get { return supportedType; } }

        public SupportedTypeAttribute(Type supportedType)
        {
            this.supportedType = supportedType;
        }
    }
}
