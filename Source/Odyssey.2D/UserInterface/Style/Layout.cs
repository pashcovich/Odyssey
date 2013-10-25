using Odyssey.UserInterface.Controls;
using SharpDX;
using System;

namespace Odyssey.UserInterface.Style
{
    public static class Layout
    {
        public static float CenterControlHorizontal(UIElement control, UIElement container)
        {
            return (container.Width - control.Width)/2f;
        }

        public static float CenterControlVertical(UIElement control, UIElement container)
        {
            return (container.Height - control.Height)/2f;
        }

        public static Vector2 CenterControl(UIElement control, UIElement container)
        {
            return new Vector2(CenterControlHorizontal(control,container),
                CenterControlVertical(control,container));
        }

        public static Vector3 OrthographicTransform(Vector2 screenPosition, float zOrder, Size screenSize)
        {
            return new Vector3
                       {
                           X =(float)Math.Floor(((screenSize.Width/ 2f) * -1f) + screenPosition.X),
                           Y =(float)Math.Floor((screenSize.Height / 2f) - screenPosition.Y),
                           Z = zOrder,
                       };
        }
    }
}
