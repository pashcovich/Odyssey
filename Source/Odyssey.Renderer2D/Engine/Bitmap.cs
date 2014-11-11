#region Using Directives

using SharpDX;
using SharpDX.Mathematics;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using PixelFormat = Odyssey.Graphics.PixelFormat;
using Resource = SharpDX.Direct2D1.Resource;

#endregion Using Directives

namespace Odyssey.Engine
{
    public abstract class Bitmap : Direct2DResource, IComparable<Bitmap>
    {
        public readonly Size2 Size;

        private long bitmapId;

        protected Bitmap(string name, Direct2DDevice device, Size2 size, BitmapProperties1 bitmapProperties)
            : base(name, device)
        {
            Properties = bitmapProperties;
            Resource = ToDispose(new Bitmap1(device, size, bitmapProperties));
            Size = size;
        }

        protected Bitmap(string name, Direct2DDevice device, Surface surface, BitmapProperties1 bitmapProperties)
            : base(name, device)
        {
            Properties = bitmapProperties;
            Resource = ToDispose(new Bitmap1(device, ToDispose(surface), bitmapProperties));
        }

        /// <summary>
        /// Gets the texture format.
        /// </summary>
        /// <value>The texture format.</value>
        public PixelFormat Format
        {
            get { return Properties.PixelFormat; }
        }

        /// <summary>
        /// Common description for this texture.
        /// </summary>
        public BitmapProperties1 Properties { get; private set; }

        public int CompareTo(Bitmap obj)
        {
            return bitmapId.CompareTo(obj.bitmapId);
        }

        protected static BitmapProperties1 NewDescription(float dpiX, float dpiY, PixelFormat format, AlphaMode alphaMode,
            BitmapOptions bitmapOptions, ColorContext colorContext)
        {
            var pixelFormat = new SharpDX.Direct2D1.PixelFormat(format, alphaMode);
            return new BitmapProperties1(pixelFormat, dpiX, dpiY, bitmapOptions, colorContext);
        }

        protected override void Initialize(Resource resource)
        {
            // Gets a Texture ID
            bitmapId = resource.NativePointer.ToInt64();
        }
    }
}