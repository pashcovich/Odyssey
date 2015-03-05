#region Using Directives

using System.Linq;
using Odyssey.Core;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public enum Dock
    {
        Left,
        Top,
        Right,
        Bottom
    }

    public class DockPanel : Panel
    {
        /// <summary>
        ///     The key to the Dock attached dependency property. This defines the position of a child item within the panel.
        /// </summary>
        public static readonly PropertyKey<Dock> DockPropertyKey = new PropertyKey<Dock>("DockKey", typeof (DockPanel), DefaultValueMetadata.Static(Dock.Left));

        public DockPanel()
        {
            LastChildFill = true;
        }

        public bool LastChildFill { get; set; }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            float availableWidth = availableSizeWithoutMargins.X;
            float availableHeight = availableSizeWithoutMargins.Y;
            float availableDepth = availableSizeWithoutMargins.Z;

            var visualChildren = Children.Visual.ToArray();

            for (int i = 0; i < visualChildren.Length; i++)
            {
                var control = visualChildren[i];
                float controlWidth = float.IsNaN(control.Width) ? control.MinimumWidth : control.Width;
                float controlHeight = float.IsNaN(control.Height) ? control.MinimumHeight : control.Height;
                float controlDepth = float.IsNaN(control.Depth) ? control.MinimumDepth : control.Depth;

                if (LastChildFill && i == visualChildren.Length - 1)
                {
                    control.Measure(new Vector3(availableWidth, availableHeight, availableDepth));
                }
                else
                {
                    Dock dock = control.DependencyProperties.Get(DockPropertyKey);
                    control.Measure(availableSizeWithoutMargins);

                    switch (dock)
                    {
                        case Dock.Bottom:
                            availableHeight -= control.DesiredSizeWithMargins.Y;
                            break;

                        case Dock.Right:
                            availableWidth -= control.DesiredSizeWithMargins.X;
                            break;

                        case Dock.Left:
                            availableWidth -= control.DesiredSizeWithMargins.X;
                            break;

                        case Dock.Top:
                            availableHeight -= control.DesiredSizeWithMargins.Y;
                            break;
                    }
                }
            }

            foreach (var shape in Children.Internal)
                shape.Measure(availableSizeWithoutMargins);

            return availableSizeWithoutMargins;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            float availableWidth = availableSizeWithoutMargins.X;
            float availableHeight = availableSizeWithoutMargins.Y;
            float availableDepth = availableSizeWithoutMargins.Z;
            float topOffset = 0;
            float rightOffset = 0;
            float leftOffset = 0;
            float bottomOffset = 0;
            float depthOffset = 0;

            var visualChildren = Children.Visual.ToArray();

            for (int i = 0; i < visualChildren.Length; i++)
            {
                var control = visualChildren[i];

                if (LastChildFill && i == visualChildren.Length - 1)
                {
                    control.SetPosition(new Vector3(leftOffset, topOffset, depthOffset));
                    control.Arrange(new Vector3(availableWidth, availableHeight, availableDepth));
                }
                else
                {
                    Dock dock = control.DependencyProperties.Get(DockPropertyKey);
                    switch (dock)
                    {
                        case Dock.Bottom:
                            control.SetPosition(new Vector3(leftOffset, availableHeight - control.DesiredSizeWithMargins.Y - bottomOffset, depthOffset));
                            control.Arrange(new Vector3(availableWidth, control.DesiredSizeWithMargins.Y, availableDepth));
                            bottomOffset += control.DesiredSizeWithMargins.Y;
                            availableHeight -= control.DesiredSizeWithMargins.Y;
                            break;

                        case Dock.Right:
                            control.SetPosition(new Vector3(availableWidth - control.DesiredSizeWithMargins.X - rightOffset, topOffset, depthOffset));
                            control.Arrange(new Vector3(control.DesiredSizeWithMargins.X, availableHeight, availableDepth));
                            rightOffset += control.DesiredSizeWithMargins.X;
                            availableWidth -= control.DesiredSizeWithMargins.X;
                            break;

                        case Dock.Left:
                            control.SetPosition(new Vector3(leftOffset, topOffset, depthOffset));
                            control.Arrange(new Vector3(control.DesiredSizeWithMargins.X, availableHeight, availableDepth));
                            leftOffset += control.DesiredSizeWithMargins.X;
                            availableWidth -= control.DesiredSizeWithMargins.X;
                            break;

                        case Dock.Top:
                            control.SetPosition(new Vector3(leftOffset, topOffset, depthOffset));
                            control.Arrange(new Vector3(availableWidth, control.DesiredSizeWithMargins.Y, availableDepth));
                            topOffset += control.DesiredSizeWithMargins.Y;
                            availableHeight -= control.DesiredSizeWithMargins.Y;
                            break;
                    }
                }
            }

            foreach (var shape in Children.Internal)
                shape.Arrange(availableSizeWithoutMargins);

            return availableSizeWithoutMargins;
        }
    }
}