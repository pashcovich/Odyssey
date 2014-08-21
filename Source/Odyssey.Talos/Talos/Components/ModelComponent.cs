using Odyssey.Graphics.Models;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Talos.Components
{
    [OptionalComponent(typeof(ShaderComponent), typeof(PostProcessComponent))]
    [RequiredComponent(typeof(PositionComponent))]
    [RequiredComponent(typeof(TransformComponent))]
    public sealed class ModelComponent : ContentComponent
    {
        public Model Model { get; private set; }
        public override bool IsInited { get { return Model != null; } }

        
        public ModelComponent()
            : base(ComponentTypeManager.GetType<ModelComponent>())
        {
        }

        public override void Initialize()
        {
            if (IsProcedurallyGenerated)
                return;
            Model = Content.Load<Model>(AssetName);
        }
    }
}
