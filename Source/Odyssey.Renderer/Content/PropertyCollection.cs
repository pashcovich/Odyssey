﻿using System;
using System.Collections.Generic;
using Odyssey.Serialization;

namespace Odyssey.Content
{
    public class PropertyCollection : Dictionary<PropertyKey, object>, IDataSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection"/> class.
        /// </summary>
        public PropertyCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="PropertyCollection" /> can contain.</param>
        public PropertyCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollection"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public PropertyCollection(IDictionary<PropertyKey, object> dictionary)
            : base(dictionary)
        {
        }

        public void SetProperty<T>(PropertyKey<T> key, T value)
        {
            if (SharpDX.Utilities.IsEnum(typeof (T)))
            {
                var intValue = Convert.ToInt32(value);
                Add(key, intValue);
            }
            else
            {
                Add(key, value);
            }
        }

        public bool ContainsKey<T>(PropertyKey<T> key)
        {
            return base.ContainsKey(key);
        }

        public T GetProperty<T>(PropertyKey<T> key)
        {
            object value;
            return TryGetValue(key, out value)
                ? SharpDX.Utilities.IsEnum(typeof (T)) ? (T) Enum.ToObject(typeof (T), (int) value) : (T) value
                : default(T);
        }

        public virtual PropertyCollection Clone()
        {
            return (PropertyCollection) MemberwiseClone();
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(Count);
                foreach (var item in this)
                {
                    var key = item.Key.ToString();
                    var value = item.Value;
                    serializer.Serialize(ref key);
                    serializer.SerializeDynamic(ref value, SerializeFlags.Nullable);
                }
            }
            else
            {
                var count = serializer.Reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string name = null;
                    object value = null;
                    serializer.Serialize(ref name);
                    serializer.SerializeDynamic(ref value, SerializeFlags.Nullable);
                    Add(new PropertyKey(name), value);
                }
            }
        }
    }
}
