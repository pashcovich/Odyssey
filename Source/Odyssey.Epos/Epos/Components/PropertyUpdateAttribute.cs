using System;

namespace Odyssey.Epos.Components
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyUpdateAttribute : Attribute
    {
        public PropertyUpdateAttribute(UpdateAction updateAction)
        {
            UpdateAction = updateAction;
        }

        public UpdateAction UpdateAction { get; private set; }
        
    }
}
