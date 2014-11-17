using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Odyssey.Core
{
    public struct PropertyContainer : IEnumerable<KeyValuePair<PropertyKey, object>>
    {
        private object owner;
        private static ReadOnlyCollection<PropertyKey> emptyKeys = new ReadOnlyCollection<PropertyKey>(new List<PropertyKey>());
        private static ReadOnlyCollection<object> emptyValues = new ReadOnlyCollection<object>(new List<object>());
        private Dictionary<PropertyKey, object> properties;

        public PropertyContainer(object owner)
        {
            properties = null;
            this.owner = owner;
        }

        public object Owner
        {
            get { return owner; }
            private set { owner = value; }
        }

        public IEnumerator<KeyValuePair<PropertyKey, object>> GetEnumerator()
        {
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    yield return new KeyValuePair<PropertyKey, object>(property.Key, property.Value);
                }
            }
        }

        public int Count
        {
            get { return properties != null ? properties.Count : 0; }
        }

        private void SetObject(PropertyKey propertyKey, object tagValue, bool tryToAdd = false)
        {
            if (properties == null)
                properties = new Dictionary<PropertyKey, object>();

            var valueToSet = propertyKey.IsValueType ? propertyKey.CreateValueHolder(tagValue) : tagValue;

            if (tryToAdd)
                properties.Add(propertyKey, valueToSet);
            else
                properties[propertyKey] = valueToSet;
        }

        public void Add<T>(PropertyKey<T> key, T value)
        {
            SetObject(key, value);
        }

        public T Get<T>(PropertyKey<T> propertyKey)
        {
            if (propertyKey.IsValueType)
            {

                object value;

                if (properties != null && properties.TryGetValue(propertyKey, out value))
                    return ((ValueHolder<T>) value).Value;

                if (propertyKey.DefaultValueMetadata != null)
                {
                    T defaultValue = ((DefaultValueMetadata<T>)propertyKey.DefaultValueMetadata).GetDefaultValueT(ref this);

                    if (propertyKey.DefaultValueMetadata.KeepValue)
                    {
                        Set(propertyKey, defaultValue);
                    }

                    return defaultValue;
                }

                return default (T);
            }

            var result = Get(propertyKey, true);
            return result != null ? (T)result : default(T);
        }

        public void Set<T>(PropertyKey<T> propertyKey, T tagValue)
        {
            if (propertyKey.IsValueType)
            {
                ValueHolder<T> valueHolder = null;

                object value;

                if (properties != null && properties.TryGetValue(propertyKey, out value))
                {
                    valueHolder = (ValueHolder<T>) value;
                }

                if (properties == null)
                    properties = new Dictionary<PropertyKey, object>();

                if (valueHolder != null)
                    valueHolder.Value = tagValue;
                else
                    valueHolder = new ValueHolder<T>(tagValue);

                properties[propertyKey] = valueHolder;

                return;
            }

            SetObject(propertyKey, tagValue, true);
        }

        private object Get(PropertyKey propertyKey, bool keep)
        {
            object value;

            if (properties != null && properties.TryGetValue(propertyKey, out value))
            {
                if (propertyKey.IsValueType)
                    value = ((ValueHolder)value).ObjectValue;
                return value;
            }

            if (propertyKey.DefaultValueMetadata != null)
            {
                object defaultValue = propertyKey.DefaultValueMetadata.GetDefaultValue(ref this);

                if (propertyKey.DefaultValueMetadata.KeepValue && keep)
                {
                    SetObject(propertyKey, defaultValue);
                }
                return defaultValue;
            }

            return null;
        }

        public bool Remove(PropertyKey propertyKey)
        {
            bool removed = false;


            if (properties != null)
                removed = properties.Remove(propertyKey);

            return removed;
        }

        public object Get(PropertyKey propertyKey)
        {
            return Get(propertyKey, true);
        }


        public object this[PropertyKey key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                SetObject(key, value, true);
            }
        }

        public ICollection<PropertyKey> Keys
        {
            get
            {
                if (properties != null) return properties.Keys;
                return emptyKeys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                if (properties != null) return properties.Values;
                return emptyValues;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal abstract class
            ValueHolder
        {
            public abstract object ObjectValue { get; }
        }

        internal class ValueHolder<T> : ValueHolder
        {
            public T Value;

            public ValueHolder(T value)
            {
                Value = value;
            }

            public override object ObjectValue
            {
                get { return Value; }
            }
        }
    }
}
