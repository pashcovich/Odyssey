using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using PixelFormat = Odyssey.Graphics.PixelFormat;

namespace Odyssey.Engine
{
    public class BitmapTarget : Bitmap
    {
        protected BitmapTarget(Direct2DDevice device, Surface surface, BitmapProperties1 description)
            : base(device, surface, description)
        {
            Initialize(Resource);
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator Bitmap1(BitmapTarget from)
        {
            return from == null ? null : from.Resource != null ? (Bitmap1)from.Resource : null;
        }

        /// <summary>
        /// Creates a new texture description for a <see cref="BitmapTarget" />.
        /// </summary>
        /// <param name="dpiX">The width.</param>
        /// <param name="dpiY">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="alphaMode">The <see cref="AlphaMode"/>.</param>
        /// <param name="options">Sets the bitmap options.</param>
        /// <param name="colorContext">The <see cref="ColorContext"/>.</param>
        /// <returns>A new instance of <see cref="BitmapTarget" /> class.</returns>
        public static BitmapProperties1 CreateDescription(float dpiX, float dpiY, PixelFormat format,
            AlphaMode alphaMode = AlphaMode.Premultiplied,
            BitmapOptions options = BitmapOptions.Target | BitmapOptions.CannotDraw, ColorContext colorContext = null)
        {
            // Make sure that the texture to create is a render target
            options |= BitmapOptions.Target;
            var description = NewDescription(dpiX, dpiY, format, alphaMode, options, colorContext);
            return description;
        }

        public static BitmapTarget New(Direct2DDevice device, Surface surface)
        {
            return new BitmapTarget(device, surface, CreateDescription(device.HorizontalDpi, device.VerticalDpi, surface.Description.Format));
        }

        public static BitmapTarget New(Direct2DDevice device, int width, int height, PixelFormat format)
        {
            Texture2DDescription renderTargetDescription = new Texture2DDescription
                {
                    Width = width,
                    Height = height,
                    ArraySize = 1,
                    SampleDescription = new SampleDescription(1, 0),
                    BindFlags = BindFlags.RenderTarget,
                    Format = format,
                    MipLevels = CalculateMipMapCount(width, height),
                    Usage = ResourceUsage.Default,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

            Texture2D texture = new Texture2D(device, renderTargetDescription);

            return new BitmapTarget(device, texture.QueryInterface<Surface>(), CreateDescription(device.HorizontalDpi, device.VerticalDpi, format));
        }

        private static int CalculateMipMapCount(int width, int height)
        {
            int size = Math.Max(Math.Max(width, height), 0);
            int maxMipMap = 1 + (int)Math.Log(size, 2);
            return maxMipMap;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator SharpDX.Direct2D1.Resource(BitmapTarget from)
        {
            return from == null ? null : @from.Resource;
        }
    }
}