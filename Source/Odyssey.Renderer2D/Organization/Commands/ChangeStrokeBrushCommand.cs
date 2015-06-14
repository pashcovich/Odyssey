using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Graphics.Drawing;
using Odyssey.UserInterface.Data;
using CommandType = Odyssey.Graphics.Organization.CommandType;

namespace Odyssey.Organization.Commands
{
    public class ChangeStrokeBrushCommand : BrushCommand
    {
        public ChangeStrokeBrushCommand(IServiceRegistry services, IResourceProvider overlay, Shape owner, TriggerAction triggerAction)
            : base(services, triggerAction, overlay, owner, CommandType.ChangeBrush)
        {
        }

        public override void Execute()
        {
            Owner.Stroke = Brush;
        }
    }
}
