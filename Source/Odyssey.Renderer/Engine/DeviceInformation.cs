using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Odyssey.Engine
{
    public class DeviceInformation
    {
        private GraphicsAdapter adapter;
        private FeatureLevel graphicsProfile;
        private PresentationParameters presentationParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInformation" /> class.
        /// </summary>
        public DeviceInformation()
        {
            Adapter = GraphicsAdapter.Default;
            PresentationParameters = new PresentationParameters();
        }

        /// <summary>
        /// Gets or sets the adapter.
        /// </summary>
        /// <value>The adapter.</value>
        /// <exception cref="System.ArgumentNullException">if value is null</exception>
        public GraphicsAdapter Adapter
        {
            get
            {
                return adapter;
            }

            set
            {
                adapter = value;
            }
        }

        /// <summary>
        /// Gets or sets the creation flags.
        /// </summary>
        /// <value>The creation flags.</value>
        public DeviceCreationFlags DeviceCreationFlags { get; set; }

        /// <summary>
        /// Gets or sets the graphics profile.
        /// </summary>
        /// <value>The graphics profile.</value>
        /// <exception cref="System.ArgumentNullException">if value is null</exception>
        public FeatureLevel GraphicsProfile
        {
            get
            {
                return graphicsProfile;
            }

            set
            {
                graphicsProfile = value;
            }
        }

        /// <summary>
        /// Gets or sets the presentation parameters.
        /// </summary>
        /// <value>The presentation parameters.</value>
        /// <exception cref="System.ArgumentNullException">if value is null</exception>
        public PresentationParameters PresentationParameters
        {
            get
            {
                return presentationParameters;
            }

            set
            {
                presentationParameters = value;
            }
        }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A new copy-instance of this GraphicsDeviceInformation.</returns>
        public DeviceInformation Clone()
        {
            return new DeviceInformation { Adapter = Adapter, GraphicsProfile = GraphicsProfile, PresentationParameters = PresentationParameters.Clone() };
        }

        /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
        /// <param name="obj">The Object to compare with the current GraphicsDeviceInformation.</param>
        public override bool Equals(object obj)
        {
            var information = obj as DeviceInformation;
            if (information == null)
            {
                return false;
            }

            if (!Equals(information.adapter, adapter))
            {
                return false;
            }

            if (information.graphicsProfile != graphicsProfile)
            {
                return false;
            }

            return information.PresentationParameters.BackBufferWidth == PresentationParameters.BackBufferWidth
                && information.PresentationParameters.BackBufferHeight == PresentationParameters.BackBufferHeight
                && information.PresentationParameters.BackBufferFormat == PresentationParameters.BackBufferFormat
                && information.PresentationParameters.DepthStencilFormat == PresentationParameters.DepthStencilFormat
                && information.PresentationParameters.MultiSampleCount == PresentationParameters.MultiSampleCount
                && information.PresentationParameters.RefreshRate == PresentationParameters.RefreshRate
                && information.PresentationParameters.PresentationInterval == PresentationParameters.PresentationInterval
                && information.PresentationParameters.RenderTargetUsage == PresentationParameters.RenderTargetUsage
                && information.PresentationParameters.DeviceWindowHandle == PresentationParameters.DeviceWindowHandle
                && information.PresentationParameters.IsFullScreen == PresentationParameters.IsFullScreen
                && information.PresentationParameters.IsStereo == PresentationParameters.IsStereo;
        }

        /// <summary>Gets the hash code for this object.</summary>
        public override int GetHashCode()
        {
            return graphicsProfile.GetHashCode()
                   ^ (adapter == null ? 0 : adapter.GetHashCode())
                   ^ presentationParameters.BackBufferWidth.GetHashCode()
                   ^ presentationParameters.BackBufferHeight.GetHashCode()
                   ^ presentationParameters.BackBufferFormat.GetHashCode()
                   ^ presentationParameters.DepthStencilFormat.GetHashCode()
                   ^ presentationParameters.MultiSampleCount.GetHashCode()
                   ^ presentationParameters.PresentationInterval.GetHashCode()
                   ^ presentationParameters.RenderTargetUsage.GetHashCode()
                   ^ presentationParameters.RefreshRate.GetHashCode()
                   ^ presentationParameters.DeviceWindowHandle.GetHashCode()
                   ^ presentationParameters.IsFullScreen.GetHashCode()
                   ^ presentationParameters.IsStereo.GetHashCode();
        }
    }
}