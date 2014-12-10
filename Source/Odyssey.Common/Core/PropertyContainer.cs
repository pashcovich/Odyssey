#region License

// Copyright © 2013-2014 Iter Astris - Adalberto L. Simeone
// Web: http://www.iterastris.uk E-mail: adal@iterastris.uk
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Other Licenses
// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
#endregion

#region Using Directives

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

#endregion

namespace Odyssey.Core
{
    public struct PropertyContainer : IEnumerable<KeyValuePair<PropertyKey, object>>
    {
        /// <summary>
        ///     Property changed delegate.
        /// </summary>
        /// <param name="propertyContainer">The property container.</param>
        /// <param name="propertyKey">The property key.</param>
        /// <param name="newValue">The property new value.</param>
        /// <param name="oldValue">The property old value.</param>
        public delegate void PropertyUpdatedDelegate(ref PropertyContainer propertyContainer, PropertyKey propertyKey, object newValue, object oldValue);

        private static readonly ReadOnlyCollection<PropertyKey> emptyKeys = new ReadOnlyCollection<PropertyKey>(new List<PropertyKey>());
        private static readonly ReadOnlyCollection<object> emptyValues = new ReadOnlyCollection<object>(new List<object>());
        private object owner;
        private Dictionary<PropertyKey, object> properties;

        public PropertyContainer(object owner)
        {
            properties = null;
            PropertyUpdated = null;
            this.owner = owner;
        }

        public object Owner
        {
            get { return owner; }
            private set { owner = value; }
        }

        public int Count
        {
            get { return properties != null ? properties.Count : 0; }
        }

        public object this[PropertyKey key]
        {
            get { return Get(key); }
            set { SetObject(key, value, true); }
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Occurs when a property is modified.
        /// </summary>
        public event PropertyUpdatedDelegate PropertyUpdated;

        private void SetObject(PropertyKey propertyKey, object tagValue, bool tryToAdd = false)
        {
            var oldValue = Get(propertyKey, true);

            // Allow to validate the metadata before storing it.
            if (propertyKey.ValidateValueMetadata != null)
            {
                propertyKey.ValidateValueMetadata.Validate(ref tagValue);
            }

            // First, check if there is an accessor
            if (propertyKey.AccessorMetadata != null)
            {
                propertyKey.AccessorMetadata.SetValue(ref this, tagValue);
                return;
            }

            if (properties == null)
                properties = new Dictionary<PropertyKey, object>();

            var valueToSet = propertyKey.IsValueType ? propertyKey.CreateValueHolder(tagValue) : tagValue;

            if (PropertyUpdated != null || propertyKey.PropertyUpdateCallback != null)
            {
                object previousValue = GetNonRecursive(propertyKey);

                if (tryToAdd)
                    properties.Add(propertyKey, valueToSet);
                else
                    properties[propertyKey] = valueToSet;

                if (!ArePropertyValuesEqual(propertyKey, tagValue, previousValue))
                {
                    if (PropertyUpdated != null)
                        PropertyUpdated(ref this, propertyKey, tagValue, previousValue);
                    if (propertyKey.PropertyUpdateCallback != null)
                        propertyKey.PropertyUpdateCallback(ref this, propertyKey, tagValue, previousValue);
                }
            }
            else
            {
                if (tryToAdd)
                    properties.Add(propertyKey, valueToSet);
                else
                    properties[propertyKey] = valueToSet;
            }

            //if (propertyKey.ObjectInvalidationMetadata != null)
            //{
            //    propertyKey.ObjectInvalidationMetadata.Invalidate(Owner, propertyKey, oldValue);
            //}
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
                    T defaultValue = ((DefaultValueMetadata<T>) propertyKey.DefaultValueMetadata).GetDefaultValueT(ref this);

                    if (propertyKey.DefaultValueMetadata.KeepValue)
                    {
                        Set(propertyKey, defaultValue);
                    }

                    return defaultValue;
                }

                return default (T);
            }

            var result = Get(propertyKey, true);
            return result != null ? (T) result : default(T);
        }

        public void Set<T>(PropertyKey<T> propertyKey, T tagValue)
        {
            if (propertyKey.IsValueType)
            {
                ValueHolder<T> valueHolder = null;
                T oldValue;

                // Fast path for value types
                // Fast path for value type
                // First, check if there is an accessor
                if (propertyKey.AccessorMetadata != null)
                {
                    // TODO: Not optimal, but not used so far
                    oldValue = (T)propertyKey.AccessorMetadata.GetValue(ref this);
                }
                else
                {
                    object value;

                    // Get bound value, if any.
                    if (properties != null && properties.TryGetValue(propertyKey, out value))
                    {
                        valueHolder = (ValueHolder<T>)value;
                        oldValue = valueHolder.Value;
                    }
                    else if (propertyKey.DefaultValueMetadata != null)
                    {
                        // Get default value.
                        oldValue = propertyKey.DefaultValueMetadataT.GetDefaultValueT(ref this);
                    }
                    else
                    {
                        oldValue = default(T);
                    }
                }

                // Allow to validate the metadata before storing it.
                if (propertyKey.ValidateValueMetadata != null)
                {
                    // TODO: Use typed validate?
                    propertyKey.ValidateValueMetadataT.ValidateValueCallback(ref tagValue);
                }

                // First, check if there is an accessor
                if (propertyKey.AccessorMetadata != null)
                {
                    // TODO: Not optimal, but not used so far
                    propertyKey.AccessorMetadata.SetValue(ref this, tagValue);
                    return;
                }

                if (properties == null)
                    properties = new Dictionary<PropertyKey, object>();

                if (valueHolder != null)
                    valueHolder.Value = tagValue;
                else
                    valueHolder = new ValueHolder<T>(tagValue);

                if (PropertyUpdated != null || propertyKey.PropertyUpdateCallback != null)
                {
                    object previousValue = GetNonRecursive(propertyKey);

                    properties[propertyKey] = valueHolder;

                    if (!ArePropertyValuesEqual(propertyKey, tagValue, previousValue))
                    {
                        if (PropertyUpdated != null)
                            PropertyUpdated(ref this, propertyKey, tagValue, previousValue);
                        if (propertyKey.PropertyUpdateCallback != null)
                            propertyKey.PropertyUpdateCallback(ref this, propertyKey, tagValue, previousValue);
                    }
                }
                else
                {
                    properties[propertyKey] = valueHolder;
                }

                //if (propertyKey.ObjectInvalidationMetadata != null)
                //{
                //    propertyKey.ObjectInvalidationMetadataT.Invalidate(Owner, propertyKey, ref oldValue);
                //}

                return;
            }

            SetObject(propertyKey, tagValue, false);
        }

        private object Get(PropertyKey propertyKey, bool keep)
        {
            object value;

            if (properties != null && properties.TryGetValue(propertyKey, out value))
            {
                if (propertyKey.IsValueType)
                    value = ((ValueHolder) value).ObjectValue;
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

            if (PropertyUpdated != null || propertyKey.PropertyUpdateCallback != null)
            {
                object previousValue = Get(propertyKey);

                if (properties != null)
                    removed = properties.Remove(propertyKey);
                var tagValue = Get(propertyKey);

                if (!ArePropertyValuesEqual(propertyKey, tagValue, previousValue))
                {
                    if (propertyKey.PropertyUpdateCallback != null)
                        propertyKey.PropertyUpdateCallback(ref this, propertyKey, tagValue, previousValue);
                    if (PropertyUpdated != null)
                        PropertyUpdated(ref this, propertyKey, tagValue, previousValue);
                }
            }
            else if (properties != null)
                removed = properties.Remove(propertyKey);

            return removed;
        }

        public object Get(PropertyKey propertyKey)
        {
            return Get(propertyKey, true);
        }

        private object GetNonRecursive(PropertyKey propertyKey)
        {
            object value;

            // Get bound value, if any.
            if (properties != null && properties.TryGetValue(propertyKey, out value))
            {
                if (propertyKey.IsValueType)
                    return ((ValueHolder)value).ObjectValue;
                return value;
            }

            if (propertyKey.DefaultValueMetadata != null)
            {
                // Get default value.
                return propertyKey.DefaultValueMetadata.GetDefaultValue(ref this);
            }

            return null;
        }

        private static bool ArePropertyValuesEqual(PropertyKey propertyKey, object propertyValue1, object propertyValue2)
        {
            var propertyType = propertyKey.Type;

            if (!propertyType.GetTypeInfo().IsValueType && propertyType != typeof (string))
            {
                return ReferenceEquals(propertyValue1, propertyValue2);
            }

            return Equals(propertyValue1, propertyValue2);
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