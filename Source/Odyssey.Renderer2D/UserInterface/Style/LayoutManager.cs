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

        public static Vector2 Units(float unitsX, float unitsY)
        {
            return Scale*Unit*new Vector2(unitsX, unitsY);
        }

        public static Vector2 CenterControl(UIElement control, UIElement container)
        {
            return new Vector2(CenterControlHorizontal(control, container),
                CenterControlVertical(control, container));
        }

        public static void DistributeHorizontally(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            Vector2 previousPosition = Vector2.Zero;
            foreach (UIElement element in children)
            {
                element.Position = previousPosition;
                element.Arrange(new Vector3(element.DesiredSizeWithMargins.X, availableSize.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector2(element.DesiredSizeWithMargins.X, 0);
            }
        }

        public static void AlignBottom(Control parent, IEnumerable<UIElement> children)
        {
            float clientAreaHeight = parent.ClientAreaHeight;
            foreach (UIElement element in children)
            {
                element.Position = new Vector2(element.Position.X, clientAreaHeight - element.Height);
            }
        }

        public static void DistributeVertically(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            Vector2 previousPosition = Vector2.Zero; 
            foreach (UIElement element in children)
            {
                element.Position = previousPosition;
                element.Arrange(new Vector3(availableSize.X, element.DesiredSizeWithMargins.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector2(0, element.DesiredSizeWithMargins.Y);
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