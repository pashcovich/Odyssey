using SharpDX.DXGI;
using System.Globalization;

namespace Odyssey.Engine
{
    /// <summary>
    /// Describes the display mode. This is equivalent to <see cref="ModeDescription"/>
    /// </summary>
    /// <msdn-id>bb173064</msdn-id>
    /// <unmanaged>DXGI_MODE_DESC</unmanaged>
    /// <unmanaged-short>DXGI_MODE_DESC</unmanaged-short>
    public class DisplayMode
    {
        private readonly int height;
        private readonly Format pixelFormat;

        private readonly Rational refreshRate;
        private readonly int width;

        public DisplayMode(Format pixelFormat, int width, int height, Rational refreshRate)
        {
            this.pixelFormat = pixelFormat;
            this.width = width;
            this.height = height;
            this.refreshRate = refreshRate;
        }

        /// <summary>
        /// Gets the aspect ratio used by the graphics device.
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if ((Height != 0) && (Width != 0))
                {
                    return ((float)Width) / Height;
                }
                return 0f;
            }
        }

        /// <summary>
        /// Gets a value indicating the surface format of the display mode.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>
        /// <unmanaged>DXGI_FORMAT Format</unmanaged>
        /// <unmanaged-short>DXGI_FORMAT Format</unmanaged-short>
        public Format Format
        {
            get
            {
                return pixelFormat;
            }
        }

        /// <summary>
        /// Gets a value indicating the screen height, in pixels.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>
        /// <unmanaged>unsigned int Height</unmanaged>
        /// <unmanaged-short>unsigned int Height</unmanaged-short>
        public int Height
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Gets a value indicating the refresh rate
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>
        /// <unmanaged>DXGI_RATIONAL RefreshRate</unmanaged>
        /// <unmanaged-short>DXGI_RATIONAL RefreshRate</unmanaged-short>
        public Rational RefreshRate
        {
            get
            {
                return refreshRate;
            }
        }

        /// <summary>
        /// Gets a value indicating the screen width, in pixels.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>
        /// <unmanaged>unsigned int Width</unmanaged>
        /// <unmanaged-short>unsigned int Width</unmanaged-short>
        public int Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Retrieves a string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "Width:{0} Height:{1} Format:{2} AspectRatio:{3} RefreshRate:{4}",
                new object[] { Width, Height, Format, AspectRatio, (float)RefreshRate.Numerator / RefreshRate.Denominator });
        }
    }
}