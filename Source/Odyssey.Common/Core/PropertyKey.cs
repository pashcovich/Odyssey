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

// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
#endregion

#region Using Directives

using System;
using System.Diagnostics.Contracts;
using System.Reflection;

#endregion

namespace Odyssey.Core
{
    public abstract class PropertyKey : IComparable
    {
        private DefaultValueMetadata defaultValueMetadata;

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
                    DefaultValueMetadata = (DefaultValueMetadata) item;
                }
                if (item is AccessorMetadata)
                {
                    AccessorMetadata = (AccessorMetadata)item;
                }
                if (item is ValidateValueMetadata)
                {
                    ValidateValueMetadata = (ValidateValueMetadata)item;
                }
            }
        }

        public string Name { get; protected set; }
        public Type Type { get; protected set; }
        public Type OwnerType { get; private set; }

        public abstract bool IsValueType { get; }

        public DefaultValueMetadata DefaultValueMetadata
        {
            get { return defaultValueMetadata; }
            set
            {
                defaultValueMetadata = value;
                PropertyUpdateCallback = defaultValueMetadata.PropertyUpdateCallback;
            }
        }

        /// <summary>
        /// Gets the accessor metadata (may be null).
        /// </summary>
        /// <value>The accessor metadata.</value>
        public AccessorMetadata AccessorMetadata { get; private set; }

        /// <summary>
        /// Gets the validate value metadata (may be null).
        /// </summary>
        /// <value>The validate value metadata.</value>
        public ValidateValueMetadata ValidateValueMetadata { get; private set; }


        /// <summary>Gets or sets the property update callback.</summary>
        /// <value>The property update callback.</value>
        internal PropertyContainer.PropertyUpdatedDelegate PropertyUpdateCallback { get; private set; }

        public int CompareTo(object obj)
        {
            var key = obj as PropertyKey;
            if (key == null)
            {
                return 0;
            }

            return string.Compare(Name, key.Name, StringComparison.OrdinalIgnoreCase);
        }

        internal abstract PropertyContainer.ValueHolder CreateValueHolder(object value);
    }

    public sealed class PropertyKey<T> : PropertyKey
    {
        private static readonly bool isValueType = typeof (T).GetTypeInfo().IsValueType;

        public PropertyKey(string name, Type ownerType, params PropertyKeyMetadata[] metadata) : base(name, typeof (T), ownerType, metadata)
        {
        }

        public override bool IsValueType
        {
            get { return isValueType; }
        }

        /// <summary>
        ///     Gets the default value metadata.
        /// </summary>
        public DefaultValueMetadata<T> DefaultValueMetadataT
        {
            get { return (DefaultValueMetadata<T>) DefaultValueMetadata; }
        }

        /// <summary>
        /// Gets the validate value metadata (may be null).
        /// </summary>
        /// <value>The validate value metadata.</value>
        public ValidateValueMetadata<T> ValidateValueMetadataT { get { return (ValidateValueMetadata<T>)ValidateValueMetadata; } }

        internal override PropertyContainer.ValueHolder CreateValueHolder(object value)
        {
            return new PropertyContainer.ValueHolder<T>((T) value);
        }
    }
}