using Odyssey.Graphics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using Device = SharpDX.Direct3D11.Device;

namespace Odyssey.Engine
{
    /// <summary>
    /// Features supported by a <see cref="DirectXDevice"/>.
    /// </summary>
    /// <remarks>
    /// This class gives also features for a particular format, using the operator this[dxgiFormat] on this structure.
    /// </remarks>
    public struct DeviceFeatures
    {
        private readonly FeaturesPerFormat[] mapFeaturesPerFormat;
        private readonly FeatureLevel level;
        private readonly bool hasComputeShaders;
        private readonly bool hasDoublePrecision;
        private readonly bool hasMultiThreadingConcurrentResources;
        private readonly bool hasDriverCommandLists;

        /// <summary>
        /// <see cref="Format"/> to exclude from the features test.
        /// </summary>
        private readonly static List<Format> ObsoleteFormatToExcludes = new List<Format>
        { 
            Format.R1_UNorm, Format.B5G6R5_UNorm, Format.B5G5R5A1_UNorm };

        internal DeviceFeatures(Device device)
        {
            mapFeaturesPerFormat = new FeaturesPerFormat[256];

            // Check global features
            level = device.FeatureLevel;
            hasComputeShaders = device.CheckFeatureSupport(Feature.ComputeShaders);
            hasDoublePrecision = device.CheckFeatureSupport(Feature.ShaderDoubles);
            device.CheckThreadingSupport(out hasMultiThreadingConcurrentResources, out hasDriverCommandLists);

            // Check features for each DXGI.Format
            foreach (var format in Enum.GetValues(typeof(Format)))
            {
                var dxgiFormat = (Format)format;
                var maximumMSAA = MSAALevel.None;
                var computeShaderFormatSupport = ComputeShaderFormatSupport.None;
                var formatSupport = FormatSupport.None;

                if (!ObsoleteFormatToExcludes.Contains(dxgiFormat))
                {
                    maximumMSAA = GetMaximumMSAASampleCount(device, dxgiFormat);
                    if (hasComputeShaders)
                        computeShaderFormatSupport = device.CheckComputeShaderFormatSupport(dxgiFormat);

                    formatSupport = device.CheckFormatSupport(dxgiFormat);
                }

                mapFeaturesPerFormat[(int)dxgiFormat] = new FeaturesPerFormat(dxgiFormat, maximumMSAA, computeShaderFormatSupport, formatSupport);
            }
        }

        /// <summary>
        /// Features level of the current device.
        /// </summary>
        /// <msdn-id>ff476528</msdn-id>        
        /// <unmanaged>GetFeatureLevel</unmanaged>        
        /// <unmanaged-short>GetFeatureLevel</unmanaged-short>        
        /// <unmanaged>D3D_FEATURE_LEVEL ID3D11Device::GetFeatureLevel()</unmanaged>
        public FeatureLevel Level { get { return level; } } 

        /// <summary>
        /// Boolean indicating if this device supports compute shaders, unordered access on structured buffers and raw structured buffers.
        /// </summary>
        public bool HasComputeShaders { get { return hasComputeShaders; } }

        /// <summary>
        /// Boolean indicating if this device supports shaders double precision calculations.
        /// </summary>
        public bool HasDoublePrecision { get { return hasDoublePrecision; } }

        /// <summary>
        /// Boolean indicating if this device supports concurrent resources in multithreading scenarios.
        /// </summary>
        public bool HasMultiThreadingConcurrentResources{ get { return hasMultiThreadingConcurrentResources; } }

        /// <summary>
        /// Boolean indicating if this device supports command lists in multithreading scenarios.
        /// </summary>
        public bool HasDriverCommandLists { get { return hasDriverCommandLists; } }

        /// <summary>
        /// Gets the <see cref="FeaturesPerFormat" /> for the specified <see cref="SharpDX.DXGI.Format" />.
        /// </summary>
        /// <param name="dxgiFormat">The DXGI format.</param>
        /// <returns>Features for the specific format.</returns>
        public FeaturesPerFormat this[Format dxgiFormat]
        {
            get { return mapFeaturesPerFormat[(int)dxgiFormat]; }
        }

        /// <summary>
        /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pixelFormat">The pixelFormat.</param>
        /// <returns>The maximum multisample count for this pixel pixelFormat</returns>
        /// <msdn-id>ff476499</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CheckMultisampleQualityLevels([In] DXGI_FORMAT Format,[In] unsigned int SampleCount,[Out] unsigned int* pNumQualityLevels)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CheckMultisampleQualityLevels</unmanaged-short>
        private static MSAALevel GetMaximumMSAASampleCount(Device device, PixelFormat pixelFormat)
        {
            int maxCount = 1;
            for (int i = 1; i <= 8; i *= 2)
            {
                if (device.CheckMultisampleQualityLevels(pixelFormat, i) != 0)
                    maxCount = i;
            }
            return (MSAALevel)maxCount;
        }

        /// <summary>
        /// The features exposed for a particular format.
        /// </summary>
        public struct FeaturesPerFormat
        {
            internal FeaturesPerFormat(Format format, MSAALevel maximumMSAALevel, ComputeShaderFormatSupport computeShaderFormatSupport, FormatSupport formatSupport)
            {
                Format = format;
                MSAALevelMax = maximumMSAALevel;
                ComputeShaderFormatSupport = computeShaderFormatSupport;
                FormatSupport = formatSupport;
            }

            /// <summary>
            /// The <see cref="SharpDX.DXGI.Format"/>.
            /// </summary>
            public readonly Format Format;

            /// <summary>
            /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat"/>.
            /// </summary>
            public readonly MSAALevel MSAALevelMax;

            /// <summary>        
            /// Gets the unordered resource support options for a compute shader resource.        
            /// </summary>        
            /// <msdn-id>ff476135</msdn-id>        
            /// <unmanaged>D3D11_FORMAT_SUPPORT2</unmanaged>        
            /// <unmanaged-short>D3D11_FORMAT_SUPPORT2</unmanaged-short>        
            public readonly ComputeShaderFormatSupport ComputeShaderFormatSupport;

            /// <summary>
            /// Support of a given format on the installed video device.
            /// </summary>
            public readonly FormatSupport FormatSupport;

            public override string ToString()
            {
                return string.Format("Format: {0}, MSAALevelMax: {1}, ComputeShaderFormatSupport: {2}, FormatSupport: {3}", Format, MSAALevelMax, ComputeShaderFormatSupport, FormatSupport);
            }
        }

        public override string ToString()
        {
            return string.Format("Level: {0}, HasComputeShaders: {1}, HasDoublePrecision: {2}, HasMultiThreadingConcurrentResources: {3}, HasDriverCommandLists: {4}", Level, hasComputeShaders, hasDoublePrecision, hasMultiThreadingConcurrentResources, hasDriverCommandLists);
        }
    }
}
