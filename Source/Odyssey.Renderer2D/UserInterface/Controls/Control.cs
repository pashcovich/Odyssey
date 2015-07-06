#region Using Directives

using System;
using System.Linq;
using System.ServiceModel.Channels;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class Control : VisualElement
    {
        protected Control(string controlStyleClass, string textStyleClass = TextStyle.Default) : base(controlStyleClass, textStyleClass)
        {
        }

        public Thickness Padding { get; set; }

        internal Vector3 PaddingInternal
        {
            get { return new Vector3(Padding.Horizontal, Padding.Vertical, 0); }
        }

        protected internal override UIElement Copy()
        {
            var newControl = (Control)base.Copy();
            newControl.Padding = Padding;
            return newControl;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            foreach (var child in Children)
                child.Measure(child.IsInternal ? availableSizeWithoutMargins : availableSizeWithoutMargins - PaddingInternal);
            return availableSizeWithoutMargins;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            foreach (var child in Children)
            {
                Vector3 size = availableSizeWithoutMargins;
                if (!child.IsInternal && !Padding.IsEmpty)
                    size -= new Vector3(Padding.Horizontal, Padding.Vertical, 0);

                child.Arrange(size);
            }
            return availableSizeWithoutMargins;
        }


    }
}