using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Sampler state collection.
    /// </summary>
    public sealed class SamplerStateCollection : StateCollectionBase<SamplerState>
    {
        /// <summary>
        /// Default state is using linear filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState Default;

        /// <summary>
        /// Point filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState PointWrap;

        /// <summary>
        /// Point filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState PointClamp;

        /// <summary>
        /// Point filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState PointMirror;

        /// <summary>
        /// Linear filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState LinearWrap;

        /// <summary>
        /// Linear filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState LinearClamp;

        /// <summary>
        /// Linear filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState LinearMirror;

        /// <summary>
        /// Anisotropic filtering with texture coordinate wrapping.
        /// </summary>
        public readonly SamplerState AnisotropicWrap;

        /// <summary>
        /// Anisotropic filtering with texture coordinate clamping.
        /// </summary>
        public readonly SamplerState AnisotropicClamp;

        /// <summary>
        /// Anisotropic filtering with texture coordinate mirroring.
        /// </summary>
        public readonly SamplerState AnisotropicMirror;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal SamplerStateCollection(DirectXDevice device)
            : base(device)
        {
            PointWrap = Add(SamplerState.New(device, "PointWrap", Filter.MinMagMipPoint, TextureAddressMode.Wrap));
            PointClamp = Add(SamplerState.New(device, "PointClamp", Filter.MinMagMipPoint, TextureAddressMode.Clamp));
            PointMirror = Add(SamplerState.New(device, "PointMirror", Filter.MinMagMipPoint, TextureAddressMode.Mirror));
            LinearWrap = Add(SamplerState.New(device, "LinearWrap", Filter.MinMagMipLinear, TextureAddressMode.Wrap));
            LinearClamp = Add(SamplerState.New(device, "LinearClamp", Filter.MinMagMipLinear, TextureAddressMode.Clamp));
            LinearMirror = Add(SamplerState.New(device, "LinearMirror", Filter.MinMagMipLinear, TextureAddressMode.Mirror));
            AnisotropicWrap = Add(SamplerState.New(device, "AnisotropicWrap", Filter.Anisotropic, TextureAddressMode.Wrap));
            AnisotropicClamp = Add(SamplerState.New(device, "AnisotropicClamp", Filter.Anisotropic, TextureAddressMode.Clamp));
            AnisotropicMirror = Add(SamplerState.New(device, "AnisotropicMirror", Filter.Anisotropic, TextureAddressMode.Mirror));
            Default = LinearClamp;
        }
    }
}
