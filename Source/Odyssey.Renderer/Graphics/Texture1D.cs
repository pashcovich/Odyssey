﻿using System;
using System.IO;
using Odyssey.Engine;
using SharpDX.Mathematics;
using SharpDX.Direct3D11;
using SharpDX.IO;

namespace Odyssey.Graphics
{
    /// <summary>
    /// A Texture 1D front end to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    public class Texture1D : Texture1DBase
    {

        internal Texture1D(DirectXDevice directX, Texture1DDescription description1D, params DataBox[] dataBox)
            : base(directX, description1D, dataBox)
        {
        }

        internal Texture1D(DirectXDevice directX, SharpDX.Direct3D11.Texture1D texture)
            : base(directX, texture)
        {
        }


        internal override TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public override Texture Clone()
        {
            return new Texture1D(Device, Description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture1DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>		
        public static Texture1D New(DirectXDevice device, Texture1DDescription description)
        {
            return new Texture1D(device, description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="SharpDX.Direct3D11.Texture1D"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="SharpDX.Direct3D11.Texture1D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(DirectXDevice device, SharpDX.Direct3D11.Texture1D texture)
        {
            return new Texture1D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(DirectXDevice device, int width, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, width, false, format, flags, arraySize, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/> class.
        /// </returns>
        /// <msdn-id>ff476520</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>	
        public static Texture1D New(DirectXDevice device, int width, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture1D(device, NewDescription(width, format, flags | TextureFlags.ShaderResource, mipCount, arraySize, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D" /> directly from an <see cref="Image"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="image">An image in CPU memory.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture1D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture1D([In] const D3D11_TEXTURE1D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture1D** ppTexture1D)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateTexture1D</unmanaged-short>
        public static Texture1D New(DirectXDevice device, Image image, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (image.Description.Dimension != TextureDimension.Texture1D)
                throw new ArgumentException("Invalid image. Must be 1D", "image");

            return new Texture1D(device, CreateTextureDescriptionFromImage(image, flags | TextureFlags.ShaderResource, usage), image.ToDataBox());
        }

        /// <summary>
        /// Loads a 1D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 1D</exception>
        /// <returns>A texture</returns>
        public static new Texture1D Load(DirectXDevice device, Stream stream, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var texture = Texture.Load(device, stream, flags | TextureFlags.ShaderResource, usage);
            if (!(texture is Texture1D))
                throw new ArgumentException(string.Format("Texture is not type of [Texture1D] but [{0}]", texture.GetType().Name));
            return (Texture1D)texture;
        }

        /// <summary>
        /// Loads a 1D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 1D</exception>
        /// <returns>A texture</returns>
        public static new Texture1D Load(DirectXDevice device, string filePath, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, flags, usage);
        }
    }
}
