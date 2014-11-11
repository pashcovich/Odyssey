#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using Odyssey.Core;
using SharpDX;
using System;
using System.Diagnostics.Contracts;

#endregion Using Directives

namespace Odyssey.Interaction
{
    /// <summary>
    /// Base class for platform-specific event bindings
    /// </summary>
    internal abstract class PointerPlatform : Component
    {
        protected readonly PointerManager manager;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerPlatform"/> class
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        protected PointerPlatform(PointerManager manager)
        {
            Contract.Requires<ArgumentNullException>(manager != null, "manager");

            this.manager = manager;
        }

        public void Initialize(object nativeWindow)
        {
            Contract.Requires<ArgumentNullException>(nativeWindow != null, "nativeWindow");
            BindWindow(nativeWindow);
            manager.Associate(this);
        }

        /// <summary>
        /// Derived classes should perform the binding to platform-specific events on <paramref name="nativeWindow"/> and raise the corresponding events on <paramref name="manager"/>.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        protected abstract void BindWindow(object nativeWindow);
    }
}