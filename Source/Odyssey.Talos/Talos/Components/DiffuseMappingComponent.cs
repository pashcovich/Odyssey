using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Utilities.Logging;

namespace Odyssey.Talos.Components
{
    [RequiredComponent(typeof(ShaderComponent))]
    public class DiffuseMappingComponent : ContentComponent, ITextureResource
    {
        protected Dictionary<string, Texture> TextureMap { get; private set; }

        public DiffuseMappingComponent()
            : base(ComponentTypeManager.GetType(typeof(ContentComponent)))
        {
            TextureMap = new Dictionary<string, Texture>();
        }

        public override bool IsInited { get { return DiffuseMap != null && DiffuseMap.IsInited; } }
        public Texture DiffuseMap { get; protected set; }
        public string DiffuseMapKey { get; set; }

        public override void Initialize()
        {
            Contract.Requires<InvalidOperationException>(DiffuseMapKey != null);
            DiffuseMap = Content.Load<Texture>(DiffuseMapKey);
            if (!DiffuseMap.IsInited)
                DiffuseMap.Initialize();
            TextureMap.Add(TextureReference.Diffuse.ToString(), DiffuseMap);
        }

        public override bool Validate()
        {
            bool test = Content.Contains(DiffuseMapKey);
            if (!test)
                LogEvent.Engine.Error("[{0}] not found.", DiffuseMapKey);
            return test;
        }

        public Texture this[string type]
        {
            get
            {
                return TextureMap[type];
            }
        }
    }
}
