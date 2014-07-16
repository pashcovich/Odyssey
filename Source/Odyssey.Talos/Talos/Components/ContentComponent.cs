using System;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Utilities.Logging;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public abstract class ContentComponent : Component, IInitializable, IContentComponent, IDisposable
    {
        string assetName;
        [YamlMember(1)]
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

        public abstract void Unload();
        public abstract void Initialize();
        public abstract bool IsInited { get; }

        public override bool Validate()
        {
            bool test = Content.Contains(AssetName);
            if (!test)
                LogEvent.Engine.Error("[{0}] not found.", AssetName);
            return test;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
