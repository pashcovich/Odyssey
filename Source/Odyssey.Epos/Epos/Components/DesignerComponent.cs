using Odyssey.Content;
using Odyssey.Epos.Messages;
using Odyssey.Graphics.Drawing;
using Odyssey.Graphics.Models;
using Odyssey.Text.Logging;

namespace Odyssey.Epos.Components
{
    public class DesignerComponent : ContentComponent
    {
        private bool isInited;
        private InstructionCollection instructions;

        public override bool IsInited { get { return isInited; } }

        public DesignerComponent() : base(ComponentTypeManager.GetType<DesignerComponent>()) { }

        [PropertyUpdate(UpdateAction.Initialize)]
        public InstructionCollection Instructions
        {
            get { return instructions; }
            set
            {
                var oldInstructions = instructions;

                if (instructions != value)
                {
                    if (oldInstructions == null || instructions.Count != oldInstructions.Count)
                    {
                        instructions = value;
                        RaisePropertyChanged();
                    }
                }
            }
        }

        public override bool Validate()
        {
            return true;
        }

        public override void Initialize()
        {
            if (instructions == null || instructions.Count == 0)
                return;
            var designer = new Designer(Services);
            designer.Begin();
            foreach (var instruction in instructions)
                instruction.Execute(designer);
            designer.End();
            isInited = true;

            var model = designer.Result;
            model.Name = Name + ".Mesh";
            AssetName = model.Name;
            var assetProvider = Services.GetService<IAssetProvider>();
            if (!assetProvider.Contains(model.Name))
            {
                assetProvider.Store(model.Name, model);
                Messenger.SendToEntity<ModelComponent>(new ContentMessage<Model>(Owner, AssetName, model), Owner);
            }
        }
    }
}
