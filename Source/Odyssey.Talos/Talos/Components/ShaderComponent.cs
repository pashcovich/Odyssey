using Odyssey.Graphics.Shaders;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    [DebuggerDisplay("Key = {Key}", Name = "{AssetName}")]
    [YamlTag("Shader")]
    public sealed class ShaderComponent : ContentComponent, ITechniqueComponent
    {
        private string key;

        public ShaderComponent()
            : base(ComponentTypeManager.GetType<ShaderComponent>())
        {
            Key = "Default";
        }

        public override bool IsInited { get { return Technique != null && Technique.IsInited; } }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (Technique!= null)
                Technique.Dispose();
        }

        IEnumerable<Technique> ITechniqueComponent.Techniques
        {
            get { return new[] { Technique }; }
        }

        public string Key
        {
            get { return key; }
            set
            {
                if (string.Equals(key, value))
                    return;
                key = value;

                if (IsInited)
                    Technique.ActivateTechnique(key);
                RaisePropertyChange("Key");
            }
        }

        [YamlIgnore]
        public Technique Technique { get; private set; }

        public override void Initialize()
        {
            Contract.Requires<InvalidOperationException>(AssetName != null);
            ShaderCollection shaderCollection = Content.Get<ShaderCollection>(AssetName);

            Technique = new Technique(DeviceService.DirectXDevice, shaderCollection, Content);

            Technique.ActivateTechnique(Key);
            Technique.Initialize();
        }

        public override void Unload()
        {
            if (Technique != null)
                Technique.Dispose();
        }
    }
}