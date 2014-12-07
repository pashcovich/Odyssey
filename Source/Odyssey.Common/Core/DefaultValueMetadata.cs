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

namespace Odyssey.Core
{
    /// <summary>
    ///     Abstract class that could be overloaded in order to define how to get default value of an
    ///     <see cref="PropertyKey" />.
    /// </summary>
    public abstract class DefaultValueMetadata : PropertyKeyMetadata
    {
        /// <summary>
        ///     Gets a value indicating whether this value is kept.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this value is kept; otherwise, <c>false</c>.
        /// </value>
        public virtual bool KeepValue
        {
            get { return false; }
        }

        /// <summary>Gets or sets the property update callback.</summary>
        /// <value>The property update callback.</value>
        public PropertyContainer.PropertyUpdatedDelegate PropertyUpdateCallback { get; set; }

        public abstract object GetDefaultValue(ref PropertyContainer obj);

        public static StaticDefaultValueMetadata<T> Static<T>(T defaultValue, bool keepDefaultValue = false)
        {
            return new StaticDefaultValueMetadata<T>(defaultValue, keepDefaultValue);
        }
    }

    public abstract class DefaultValueMetadata<T> : DefaultValueMetadata
    {
        public abstract T GetDefaultValueT(ref PropertyContainer obj);

        public override object GetDefaultValue(ref PropertyContainer obj)
        {
            return GetDefaultValueT(ref obj);
        }
    }

    public class StaticDefaultValueMetadata<T> : DefaultValueMetadata<T>
    {
        private readonly T defaultValue;
        private readonly bool keepDefaultValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaticDefaultValueMetadata" /> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="keepDefaultValue">if set to <c>true</c> [keep default value].</param>
        public StaticDefaultValueMetadata(T defaultValue, bool keepDefaultValue = false)
        {
            this.defaultValue = defaultValue;
            this.keepDefaultValue = keepDefaultValue;
        }

        /// <inheritdoc />
        public override bool KeepValue
        {
            get { return keepDefaultValue; }
        }

        /// <inheritdoc />
        public override T GetDefaultValueT(ref PropertyContainer obj)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Specifies a delegate to fetch the default value of an <see cref="PropertyKey"/>.
    /// </summary>
    public class DelegateDefaultValueMetadata<T> : DefaultValueMetadata<T>
    {
        private readonly DefaultValueCallback callback;

        /// <summary>
        /// Callback used to initialiwe the tag value.
        /// </summary>
        /// <param name="container">The tag property container.</param>
        /// <returns>Value of the tag.</returns>
        public delegate T DefaultValueCallback(ref PropertyContainer container);

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateDefaultValueMetadata"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public DelegateDefaultValueMetadata(DefaultValueCallback callback)
        {
            this.callback = callback;
        }

        public override T GetDefaultValueT(ref PropertyContainer obj)
        {
            return callback(ref obj);
        }

        public override bool KeepValue
        {
            get { return true; }
        }
    }
}