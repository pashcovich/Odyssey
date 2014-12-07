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

using System;

#endregion

namespace Odyssey.Core
{
    /// <summary>
    ///     Delegate ValidateValueCallback used by <see cref="ValidateValueMetadata" />.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>The same value or a coerced value.</returns>
    public delegate void ValidateValueCallback<T>(ref T value);

    public abstract class ValidateValueMetadata : PropertyKeyMetadata
    {
        public static ValidateValueMetadata<T> New<T>(ValidateValueCallback<T> invalidationCallback)
        {
            return new ValidateValueMetadata<T>(invalidationCallback);
        }

        public abstract void Validate(ref object obj);
    }

    /// <summary>
    ///     A metadata to allow validation/coercision of a value before storing the value into the
    ///     <see cref="PropertyContainer" />.
    /// </summary>
    public class ValidateValueMetadata<T> : ValidateValueMetadata
    {
        private readonly ValidateValueCallback<T> validateValueCallback;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateValueMetadata" /> class.
        /// </summary>
        /// <param name="validateValueCallback">The validate value callback.</param>
        /// <exception cref="System.ArgumentNullException">validateValueCallback</exception>
        public ValidateValueMetadata(ValidateValueCallback<T> validateValueCallback)
        {
            if (validateValueCallback == null) throw new ArgumentNullException("validateValueCallback");
            this.validateValueCallback = validateValueCallback;
        }

        /// <summary>
        ///     Gets the validate value callback.
        /// </summary>
        /// <value>The validate value callback.</value>
        public ValidateValueCallback<T> ValidateValueCallback
        {
            get { return validateValueCallback; }
        }

        public override void Validate(ref object obj)
        {
            var objCopy = (T) obj;
            validateValueCallback(ref objCopy);
            obj = objCopy;
        }
    }
}