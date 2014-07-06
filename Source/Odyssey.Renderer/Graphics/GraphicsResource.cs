using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public abstract class GraphicsResource : Component
    {
        /// <summary>
        /// The attached Direct3D11 resource to this instance.
        /// </summary>
        protected DeviceChild Resource { get; set; }
        public DirectXDevice Device { get; internal set; }
        public bool IsInited { get; private set; }

        public string DebugName
        {
            get { return Resource.DebugName; }
            set { Resource.DebugName = value; }
        }

        protected GraphicsResource(DirectXDevice device)
            : this(device, null)
        {
        }

        protected GraphicsResource(DirectXDevice device, string name)
            : base(name)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");

            Device = device;
        }

        public abstract void Initialize();

        /// <summary>
        /// Initializes the specified device local.
        /// </summary>
        /// <param name="resource">The resource.</param>
        protected virtual void Initialize(DeviceChild resource)
        {
            Resource = ToDispose(resource);
            if (resource != null)
            {
                resource.Tag = this;
            }
            IsInited = true;
        }

        public void Unload()
        {
            Dispose();
            IsInited = false;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Resource(GraphicsResource from)
        {
            return from == null ? null : (Resource)from.Resource;
        }

        /// <summary>
        /// Gets the CPU access flags from the <see cref="ResourceUsage"/>.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <returns>The CPU access flags</returns>
        protected static CpuAccessFlags GetCpuAccessFlagsFromUsage(ResourceUsage usage)
        {
            switch (usage)
            {
                case ResourceUsage.Dynamic:
                    return CpuAccessFlags.Write;
                case ResourceUsage.Staging:
                    return CpuAccessFlags.Read | CpuAccessFlags.Write;
            }
            return CpuAccessFlags.None;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
                Resource = null;
        }

        /// <summary>
        /// Called when name changed for this component.
        /// </summary>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Name")
            {
                if (Device.IsDebugMode && Resource != null) Resource.DebugName = Name;
            }
        }

        protected static void UnPin(GCHandle[] handles)
        {
            if (handles != null)
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    if (handles[i].IsAllocated)
                        handles[i].Free();
                }
            }
        }


    }
}
