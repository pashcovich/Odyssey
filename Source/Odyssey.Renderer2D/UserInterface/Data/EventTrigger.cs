using System;
using System.Linq;
using System.Xml;
using Odyssey.Content;
using Odyssey.Graphics.Drawing;
using Odyssey.Organization.Commands;
using Odyssey.Reflection;
using Odyssey.Text;
using Odyssey.Text.Logging;

namespace Odyssey.UserInterface.Data
{
    public class EventTrigger : TriggerBase
    {
        private const string type = "Type";
        private readonly TriggerActionCollection triggerActions;
        public string EventName { get; private set; }
        public string SourceName { get; private set; }
        public TriggerActionCollection TriggerActions { get { return triggerActions; }}

        public EventTrigger()
        {
            triggerActions = new TriggerActionCollection();
        }

        public override void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            string eventName = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((EventTrigger et) => et.EventName));
            string sourceName = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((EventTrigger et) => et.SourceName));

            if (!string.IsNullOrEmpty(sourceName))
            {
                var parsedResource = TextHelper.ParseResource(sourceName);
                if (!resourceProvider.ContainsResource(parsedResource))
                    throw new InvalidOperationException(String.Format("No resource '{0}' found", sourceName));
                else
                    SourceName = parsedResource;
            }
            EventName = eventName;

            xmlReader.ReadStartElement();
            while (xmlReader.IsStartElement())
            {
                var triggerAction = new TriggerAction();
                triggerAction.DeserializeXml(resourceProvider, xmlReader);
                triggerActions.Add(triggerAction);
            }

        }

        public override void Initialize(UIElement target)
        {
            var targetType = target.GetType();
            var eventInfo = ReflectionHelper.GetEvent(target.GetType(), EventName);
            if (eventInfo == null)
            {
                LogEvent.UserInterface.Error("No event '{0}' in {1}.", EventName, targetType);
                return;
            }

            var overlay = target.Overlay;
            var source = SourceName == null ? target.Parent : overlay.FindDescendant(SourceName);

            foreach (var action in TriggerActions)
            {
                Command cmd = null;
                string targetName = TextHelper.ParseResource(action["TargetName"]);
                Shape realTarget;

                if (string.IsNullOrEmpty(targetName))
                    realTarget = (Shape) target;
                else
                {
                    var tempTarget = target.FindDescendant(targetName);
                    realTarget = tempTarget as Shape ?? tempTarget.FindDescendants<Shape>().First();
                    if (action.HasKey(type))
                    {
                        var expectedType = Type.GetType(string.Format("Odyssey.Graphics.Drawing.{0}", action[type]));
                        if (realTarget.GetType() != expectedType)
                            throw new InvalidOperationException(string.Format("Element '{0}' is of type: {1}. Expected type:{2}.", realTarget.Name,
                                realTarget.GetType(), expectedType));
                    }
                }
                switch (action.CommandName)
                {
                    case "ChangeStrokeBrush":
                        cmd = new ChangeStrokeBrushCommand(overlay.Services, overlay.Theme, realTarget, action);
                        break;

                    case "ChangeFillBrush":
                        cmd = new ChangeFillBrushCommand(overlay.Services, overlay.Theme, realTarget, action);
                        break;
                }

                var methodInfo = ReflectionHelper.GetMethod(cmd.GetType(), "EncapsulatePointerEvent");
                var handler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, cmd);
                eventInfo.AddEventHandler(source, handler);
            }
        }
    }
}
