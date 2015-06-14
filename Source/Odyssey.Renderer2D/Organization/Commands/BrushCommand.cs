using System;
using System.Xml;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Graphics;
using Odyssey.Graphics.Drawing;
using Odyssey.Interaction;
using Odyssey.Reflection;
using Odyssey.Text;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using CommandType = Odyssey.Graphics.Organization.CommandType;

namespace Odyssey.Organization.Commands
{
    public abstract class BrushCommand : Command
    {
        private readonly Brush brush;
        private readonly Shape owner;
        private readonly TriggerAction triggerAction;

        protected Shape Owner
        {
            get { return owner; }
        }

        protected BrushCommand(IServiceRegistry services, TriggerAction action, IResourceProvider rProvider, Shape owner, CommandType type) : base(services, type)
        {
            this.owner = owner;
            string resource = TextHelper.ParseResource(action[ReflectionHelper.GetPropertyName((ChangeStrokeBrushCommand c) => c.Brush)]);
            
            if (!rProvider.ContainsResource(resource))
                throw new InvalidOperationException(String.Format("No resource '{0}' found", resource));

            var styleService = Services.GetService<IStyleService>();
            brush = styleService.GetBrushResource(resource, rProvider);
        }

        public Brush Brush
        {
            get { return brush; }
        }

        public override void Initialize()
        {
            IsInited = true;
        }

        protected void EncapsulatePointerEvent(object sender, PointerEventArgs eventArgs)
        {
            Execute();
        }
    }
}