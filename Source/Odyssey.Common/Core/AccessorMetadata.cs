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

namespace Odyssey.Core
{
    public class AccessorMetadata : PropertyKeyMetadata
    {
        public delegate object GetterDelegate(ref PropertyContainer propertyContainer);

        public delegate void SetterDelegate(ref PropertyContainer propertyContainer, object value);


        private readonly GetterDelegate getter;
        private readonly SetterDelegate setter;

        public AccessorMetadata(GetterDelegate getter, SetterDelegate setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public object GetValue(ref PropertyContainer obj)
        {
            return getter(ref obj);
        }

        public void SetValue(ref PropertyContainer obj, object value)
        {
            setter(ref obj, value);
        }
    }
}