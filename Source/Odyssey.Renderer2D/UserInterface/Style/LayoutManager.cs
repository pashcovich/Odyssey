#region Using Directives

using System.Linq;
using Odyssey.UserInterface.Controls;
using SharpDX.Mathematics;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public static class LayoutManager
    {
        private const float Unit = 32;
        public static float Scale = 1.0f;

        public static float Units(float units)
        {
            return Scale*Unit*units;
        }

        public static Vector3 Point(float unitsX, float unitsY, float depthZ = 0)
        {
            return Scale*Unit*new Vector3(unitsX, unitsY, depthZ);
        }

        public static Vector2 CenterControl(UIElement control, UIElement container)
        {
            return new Vector2(CenterControlHorizontal(control, container),
                CenterControlVertical(control, container));
        }

        public static void DistributeHorizontally(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            var previousPosition = Vector3.Zero;
            foreach (UIElement element in children)
            {
                element.Position = previousPosition;
                element.Arrange(new Vector3(element.DesiredSizeWithMargins.X, availableSize.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector3(element.DesiredSizeWithMargins.X, 0, 0);
            }
        }

        public static void AlignBottom(UIElement parent)
        {
            float clientAreaHeight = parent.RenderSize.Y;
            foreach (UIElement element in parent.Controls)
            {
                element.Position = new Vector3(element.Position.X, clientAreaHeight - element.Height, element.Position.Z);
            }
        }

        public static void DistributeVertically(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            var previousPosition = Vector3.Zero; 
            foreach (UIElement element in children)
            {
                element.Position = previousPosition;
                element.Arrange(new Vector3(availableSize.X, element.DesiredSizeWithMargins.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector3(0, element.DesiredSizeWithMargins.Y, element.Position.Z);
            }
        }

        private static float CenterControlHorizontal(UIElement control, UIElement container)
        {
            return (container.Width - control.Width) / 2f;
        }

        private static float CenterControlVertical(UIElement control, UIElement container)
        {
            return (container.Height - control.Height) / 2f;
        }
    }
}