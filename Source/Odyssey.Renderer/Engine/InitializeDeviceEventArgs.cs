using System;

namespace Odyssey.Engine
{
    /// <summary>
    /// Describes settings to apply before preparing a device for creation, used by <see cref="DeviceManager.OnInitializeDeviceSettings"/>.
    /// </summary>
    public class InitializeDeviceSettingsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeDeviceSettingsEventArgs" /> class.
        /// </summary>
        /// <param name="deviceInformation">The graphics device information.</param>
        public InitializeDeviceSettingsEventArgs(DeviceInformation deviceInformation)
        {
            DeviceInformation = deviceInformation;
        }

        /// <summary>
        /// Gets the graphics device information.
        /// </summary>
        /// <value>The graphics device information.</value>
        public DeviceInformation DeviceInformation { get; private set; }
    }

}
