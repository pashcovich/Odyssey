using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Odyssey.Core
{
    public abstract class PropertyKey : IComparable
    {
        private DefaultValueMetadata defaultValueMetadata;

        public string Name { get; protected set; }
        public Type Type { get; protected set; }
        public Type OwnerType { get; private set; }

        protected PropertyKey(string name, Type type, Type ownerType, params PropertyKeyMetadata[] metadata)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name), "name");
            Name = name;
            Type = type;
            OwnerType = ownerType;
            foreach (PropertyKeyMetadata item in metadata)
            {
                if (item is DefaultValueMetadata)
                {
                    defaultValueMetadata = (DefaultValueMetadata)item;
                }
            }
        }

        public int CompareTo(object obj)
        {
            var key = obj as PropertyKey;
            if (key == null)
            {
                return 0;
            }

            return string.Compare(Name, key.Name, StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool IsValueType { get; }
        public DefaultValueMetadata DefaultValueMetadata { get { return defaultValueMetadata; } }
        internal abstract PropertyContainer.ValueHolder CreateValueHolder(object value);
    }

    public sealed class PropertyKey<T> : PropertyKey
    {
        private readonly static bool isValueType = typeof(T).GetTypeInfo().IsValueType;

        public PropertyKey(string name, Type ownerType, params PropertyKeyMetadata[] metadata) : base(name,typeof(T), ownerType, metadata)
        { }

        public override bool IsValueType
        {
            get { return isValueType; }
        }

        /// <summary>
        /// Gets the default value metadata.
        /// </summary>
        public DefaultValueMetadata<T> DefaultValueMetadataT { get { return (DefaultValueMetadata<T>)DefaultValueMetadata; } }

        internal override PropertyContainer.ValueHolder CreateValueHolder(object value)
        {
            return new PropertyContainer.ValueHolder<T>((T)value);
        }
    }
}
