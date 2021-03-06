﻿#region Using Directives

using System;
using SharpDX;

#endregion
#region Other Licenses
// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
#endregion

namespace Odyssey.Core
{
    /// <summary>
    ///     A service registry is a <see cref="IServiceProvider" /> that provides methods to register and unregister services.
    /// </summary>
    public interface IServiceRegistry : IServiceProvider
    {
        /// <summary>
        ///     Occurs when a new service is added.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceAdded;

        /// <summary>
        ///     Occurs when when a service is removed.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceRemoved;

        /// <summary>
        ///     Gets the service object of specified type. The service must be registered with the <typeparamref name="T" /> type
        ///     key.
        /// </summary>
        /// <remarks>
        ///     This method will thrown an exception if the service is not registered, it null value can be accepted - use the
        ///     <see cref="IServiceProvider.GetService" /> method.
        /// </remarks>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        /// <returns>The service instance.</returns>
        /// <exception cref="ArgumentException">Is thrown when the corresponding service is not registered.</exception>
        T GetService<T>();

        /// <summary>
        ///     Adds a service to this service provider.
        /// </summary>
        /// <param name="type">The type of service to add.</param>
        /// <param name="provider">The instance of the service provider to add.</param>
        /// <exception cref="System.ArgumentNullException">Service type cannot be null</exception>
        /// <exception cref="System.ArgumentException">Service is already registered</exception>
        void AddService(Type type, object provider);

        /// <summary>
        ///     Adds a service to this service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="provider">The instance of the service provider to add.</param>
        /// <exception cref="System.ArgumentNullException">Service type cannot be null</exception>
        /// <exception cref="System.ArgumentException">Service is already registered</exception>
        void AddService<T>(T provider);

        /// <summary>
        ///     Removes the object providing a specified service.
        /// </summary>
        /// <param name="type">The type of service.</param>
        void RemoveService(Type type);
    }
}