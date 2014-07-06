using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using Device = SharpDX.Direct3D11.Device;

namespace Odyssey.Engine
{
    /// <summary>
    /// Provides methods to retrieve and manipulate graphics adapters. This is the equivalent to <see cref="Adapter1"/>.
    /// </summary>
    /// <msdn-id>ff471329</msdn-id>        
    /// <unmanaged>IDXGIAdapter1</unmanaged>        
    /// <unmanaged-short>IDXGIAdapter1</unmanaged-short>        
    public class GraphicsAdapter : Component
    {
        private static DisposeCollector staticCollector;
        private readonly Adapter1 adapter;
        private readonly int adapterOrdinal;

        private readonly GraphicsOutput[] outputs1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsAdapter" /> class.
        /// </summary>
        /// <param name="adapterOrdinal">The adapter ordinal.</param>
        private GraphicsAdapter(int adapterOrdinal)
        {
            this.adapterOrdinal = adapterOrdinal;
            adapter = ToDispose(Factory.GetAdapter1(adapterOrdinal));
            Description = adapter.Description1;
            var outputs = adapter.Outputs;

            outputs1 = new GraphicsOutput[outputs.Length];
            for (var i = 0; i < outputs.Length; i++)
                outputs1[i] = ToDispose(new GraphicsOutput(outputs[i]));
        }

        /// <summary>
        /// Initializes static members of the <see cref="GraphicsAdapter" /> class.
        /// </summary>
        static GraphicsAdapter()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the GraphicsAdapter. On Desktop and WinRT, this is done statically.
        /// </summary>
        public static void Initialize()
        {
            if (Adapters == null)
            {
#if DIRECTX11_1
            using (var factory = new Factory1()) Initialize(factory.QueryInterface<Factory2>());
#else
                Initialize(new Factory1());
#endif
            }
        }

        /// <summary>
        /// Dispose all statically cached value by this instance.
        /// </summary>
        public new static void Dispose()
        {
            SharpDX.Utilities.Dispose(ref staticCollector);
            Adapters = null;
            Default = null;
        }

        /// <summary>
        /// Initializes all adapters with the specified factory.
        /// </summary>
        /// <param name="factory1">The factory1.</param>
        internal static void Initialize(Factory1 factory1)
        {
            if (staticCollector != null)
            {
                staticCollector.Dispose();
            }

            staticCollector = new DisposeCollector();
            Factory = factory1;
            staticCollector.Collect(Factory);

            int countAdapters = Factory.GetAdapterCount1();
            var adapters = new List<GraphicsAdapter>();
            for (int i = 0; i < countAdapters; i++)
            {
                var adapter = new GraphicsAdapter(i);
                staticCollector.Collect(adapter);
                adapters.Add(adapter);
            }

            Default = adapters[0];
            Adapters = adapters.ToArray();
        }

        /// <summary>
        /// Collection of available adapters on the system.
        /// </summary>
        public static GraphicsAdapter[] Adapters { get; private set; }

        /// <summary>
        /// Gets the default adapter.
        /// </summary>
        public static GraphicsAdapter Default { get; private set; }

        /// <summary>
        /// Gets the number of <see cref="GraphicsOutput"/> attached to this <see cref="GraphicsAdapter"/>.
        /// </summary>
        public int OutputsCount { get { return outputs1.Length; } }

        /// <summary>
        /// Gets the description for this adapter.
        /// </summary>
        public readonly AdapterDescription1 Description;

        /// <summary>
        /// Default PixelFormat used.
        /// </summary>
        public readonly Format DefaultFormat = Format.R8G8B8A8_UNorm;

        /// <summary>
        /// Gets the <see cref="Factory1"/> used by all GraphicsAdapter.
        /// </summary>
        public static Factory1 Factory { get; private set; }

        /// <summary>
        /// Determines if this instance of GraphicsAdapter is the default adapter.
        /// </summary>
        public bool IsDefaultAdapter { get { return adapterOrdinal == 0; } }

        /// <summary>
        /// <see cref="Adapter1"/> casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Adapter1(GraphicsAdapter from)
        {
            return from.adapter;
        }

        /// <summary>
        /// <see cref="Adapter1"/> casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Factory1(GraphicsAdapter from)
        {
            return Factory;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsOutput"/> attached to this adapter at the specified index.
        /// </summary>
        /// <param name="index">The index of the output to get.</param>
        /// <returns>The <see cref="GraphicsOutput"/> at the specified <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when <paramref name="index"/> is less than zero or greater or equal to <see cref="OutputsCount"/>.</exception>
        public GraphicsOutput GetOutputAt(int index)
        {
            if (index < 0 || index >= OutputsCount)
                throw new ArgumentOutOfRangeException("index");

            return outputs1[index];
        }

        /// <summary>
        /// Tests to see if the adapter supports the requested profile.
        /// </summary>
        /// <param name="featureLevel">The graphics profile.</param>
        /// <returns>true if the profile is supported</returns>
        public bool IsProfileSupported(FeatureLevel featureLevel)
        {
            return Device.IsSupportedFeatureLevel(this, featureLevel);
        }

        /// <summary>
        /// Return the description of this adapter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description.Description;
        }
    }
}
