using Odyssey.Engine;
using Odyssey.Talos.Components;

namespace Odyssey.Talos.Systems
{
    public class DesignerSystem : RunOnceSystem
    {
        public DesignerSystem() : base(Selector.All(typeof(DesignerComponent), typeof(ModelComponent))) {}

        public override void Process(ITimeService time)
        {
            foreach (var entity in Entities)
            {
                var cModel = entity.GetComponent<ModelComponent>();
                var cDesigner = entity.GetComponent<DesignerComponent>();
                cModel.AssetName = cDesigner.AssetName;
            }
        }


    }
}
