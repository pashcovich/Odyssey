using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class Technique : Component
    {
        public const string DefaultTechnique = "Default";

        private readonly DirectXDevice device;
        private readonly ShaderCollection shaderCollection;
        private readonly IAssetProvider content;
        private Effect effect;

        internal string ActiveTechniqueId { get { return string.Format("{0}.{1}", Name, ActiveTechnique.Name); } }
        internal DirectXDevice Device { get { return device; } }

        public TechniqueMapping ActiveTechnique { get; protected set; }

        public Effect Effect { get { return effect; } }

        public bool IsInited { get; private set; }

        protected ShaderCollection ShaderCollection { get { return shaderCollection; } }
        protected IAssetProvider Content { get { return content; } }

        public Technique(DirectXDevice device, ShaderCollection shaderCollection, IAssetProvider assetProvider)
        {
            Contract.Requires<ArgumentNullException>(shaderCollection != null, "shaderCollection");
            Contract.Requires<ArgumentNullException>(assetProvider != null, "assetProvider");
            Contract.Requires<ArgumentException>(shaderCollection.Contains(DefaultTechnique), "Default technique not found");
            this.device = device;
            this.shaderCollection = shaderCollection;
            content = assetProvider;
            RemoveAndDispose(ref effect);
            ActiveTechnique = shaderCollection.Get(DefaultTechnique);
            Name = shaderCollection.Name;
        }

        public void ActivateTechnique(TechniqueKey key)
        {
            Contract.Requires<ArgumentException>(ContainsTechnique(key));
            ActiveTechnique = ShaderCollection.Get(key);
        }

        public void ActivateTechnique(string key)
        {
            Contract.Requires<ArgumentException>(ContainsTechnique(key));
            ActiveTechnique = ShaderCollection.Get(key);
        }

        [Pure]
        public bool ContainsTechnique(TechniqueKey key)
        {
            return ShaderCollection.Contains(key);
        }

        [Pure]
        public bool ContainsTechnique(string key)
        {
            return ShaderCollection.Contains(key);
        }

        public void Initialize()
        {
            effect = ToDispose(new Effect(device, shaderCollection.Name, ActiveTechnique));
         
            IsInited = true;
        }

        #region Technique Feature Methods

        public bool GetFeatureStatus(VertexShaderFlags feature)
        {
            return ActiveTechnique.Key.Supports(feature);
        }

        public bool GetFeatureStatus(PixelShaderFlags feature)
        {
            return ActiveTechnique.Key.Supports(feature);
        }

        public bool SupportsFeature(VertexShaderFlags feature)
        {
            return ShaderCollection.Any(t => t.Key.Supports(feature));
        }

        public bool SupportsFeature(PixelShaderFlags feature)
        {
            return ShaderCollection.Any(t => t.Key.Supports(feature));
        }

        #endregion Technique Feature Methods

        public void Unload()
        {
            effect.Dispose();
            IsInited = false;
        }
    }
}