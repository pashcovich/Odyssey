using System;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Utilities.Logging;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public abstract class ContentComponent : Component, IContentComponent, IInitializable
    {
        string assetName;
        public string AssetName
        {
            get { return assetName; }
            set
            {
                if (assetName != value)
                {
                    assetName = value;
                    RaisePropertyChange("AssetName");
                }
            }
        }

        protected IAssetProvider Content { get { return Services.GetService<IAssetProvider>(); } }
        protected IOdysseyDeviceService DeviceService { get { return Services.GetService<IOdysseyDeviceService>(); } }

        protected ContentComponent(ComponentType componentType) : base(componentType)
        {
        }

        /// <summary>
        /// Initializes the content of this component.
        /// </summary>
        public abstract void Initialize();
        public abstract bool IsInited { get; }

        public override bool Validate()
        {
            bool test = Content.Contains(AssetName);
            if (!test)
                LogEvent.Engine.Error("[{0}] not found.", AssetName);
            return test;
        }

    }
}
