using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.IO;
using System;
using System.IO;

namespace Odyssey.Graphics
{
    /// <summary>
    /// A Texture 2D front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    public class Texture2D : Texture2DBase
    {
        internal Texture2D(DirectXDevice device, Texture2DDescription description2D, params DataBox[] dataBoxes)
            : base(device, description2D, dataBoxes)
        {
        }

        internal Texture2D(DirectXDevice device, SharpDX.Direct3D11.Texture2D texture)
            : base(device, texture)
        {
        }

        /// <summary>
        /// Loads a 2D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 2D</exception>
        /// <returns>A texture</returns>
        public static new Texture2D Load(DirectXDevice device, Stream stream, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            var texture = Texture.Load(device, stream, flags | TextureFlags.ShaderResource, usage);
            if (!(texture is Texture2D))
                throw new ArgumentException(string.Format("Texture is not type of [Texture2D] but [{0}]", texture.GetType().Name));
            return (Texture2D)texture;
        }

        /// <summary>
        /// Loads a 2D texture from a stream.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="filePath">The file to load the texture from.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">Usage of the resource. Default is <see cref="ResourceUsage.Immutable"/> </param>
        /// <exception cref="ArgumentException">If the texture is not of type 2D</exception>
        /// <returns>A texture</returns>
        public static new Texture2D Load(DirectXDevice device, string filePath, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            using (var stream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read))
                return Load(device, stream, flags | TextureFlags.ShaderResource, usage);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, Texture2DDescription description)
        {
            return new Texture2D(device, description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="SharpDX.Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="SharpDX.Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, SharpDX.Direct3D11.Texture2D texture)
        {
            return new Texture2D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture2D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(device, width, height, false, format, flags, arraySize, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Texture2D" />.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.ShaderResource, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture2D(device, NewDescription(width, height, format, flags | TextureFlags.ShaderResource, mipCount, arraySize, usage));
        }

        /// <summary>
        /// Creates a new <see cref="Texture2D" />.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="textureData">Texture data through an array of <see cref="DataBox"/> </param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, DataBox[] textureData, TextureFlags flags = TextureFlags.ShaderResource, int arraySize = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            return new Texture2D(device, NewDescription(width, height, format, flags | TextureFlags.ShaderResource, mipCount, arraySize, usage), textureData);
        }

        /// <summary>
        /// Creates a new <see cref="Texture2D" /> directly from an <see cref="Image"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="image">An image in CPU memory.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of <see cref="Texture2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2D New(DirectXDevice device, Image image, TextureFlags flags = TextureFlags.ShaderResource, ResourceUsage usage = ResourceUsage.Immutable)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (image.Description.Dimension != TextureDimension.Texture2D)
                throw new ArgumentException("Invalid image. Must be 2D", "image");

            return new Texture2D(device, CreateTextureDescriptionFromImage(image, flags | TextureFlags.ShaderResource, usage), image.ToDataBox());
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
            return new Texture2D(Device, Description);
        }

        internal override TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipMapSlice)
        {
            throw new NotSupportedException();
        }
    }
}