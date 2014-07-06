using System.Collections;
using System.Collections.Generic;
using Odyssey.Graphics.Shaders;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using SharpDX.Direct3D11;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    [DebuggerDisplay("Key = {Key}", Name="{AssetName}")]
    [YamlTag("Shader")]
    public sealed class ShaderComponent : ContentComponent, ITechniqueComponent
    {
        string key;
        [YamlIgnore]
        public Technique Technique { get; private set; }
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

        public override bool IsInited { get { return Technique != null && Technique.IsInited; } }

        public ShaderComponent() : base (ComponentTypeManager.GetType<ShaderComponent>())
        {
            Key = "Default";
        }

        public override void Unload()
        {
            if (Technique != null)
                Technique.Dispose();
        }

        public override void Initialize()
        {
            Contract.Requires<InvalidOperationException>(AssetName != null);
            ShaderCollection shaderCollection = Content.Get<ShaderCollection>(AssetName);
            
            Technique = new Technique(DeviceService.DirectXDevice, shaderCollection, Content);
            
            Technique.ActivateTechnique(Key);
            Technique.Initialize();
        }


        IEnumerable<Technique> ITechniqueComponent.Techniques
        {
            get { return new[] {Technique}; }
        }
    }
}
