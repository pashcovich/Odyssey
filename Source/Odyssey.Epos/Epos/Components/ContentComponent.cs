using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Text.Logging;

namespace Odyssey.Epos.Components
{
    public abstract class ContentComponent : Component, IContentComponent, IInitializable
    {
        string assetName;
        
        [PropertyUpdate(UpdateAction.Register)]
        public string AssetName
        {
            get { return assetName; }
            set
            {
                if (assetName != value)
                {
                    assetName = value;
                    RaisePropertyChanged();
                }
            }
        }

        
        protected IAssetProvider Content { get { return Services.GetService<IAssetProvider>(); } }

        protected IGraphicsDeviceService DeviceService { get { return Services.GetService<IGraphicsDeviceService>(); } }

        protected ContentComponent(ComponentType componentType) : base(componentType)
        {
        }

        /// <summary>
        /// Initializes the content of this component.
        /// </summary>
        public abstract void Initialize();
        public abstract bool IsInited { get; }
        public bool IsProcedurallyGenerated { get; set; }

        public override bool Validate()
        {
            if (IsProcedurallyGenerated)
                return true;
            bool test = Content.Contains(AssetName);
            if (!test)
                LogEvent.Engine.Error("[{0}] not found.", AssetName);
            return test;
        }

    }
}
