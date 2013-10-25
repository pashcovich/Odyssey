using Odyssey.Devices;
using Odyssey.Engine;
using Odyssey.UserInterface.Xml;
using SharpDX;
using System;

namespace Odyssey.UserInterface
{
    public interface IUIElement
    {
        Vector2 AbsolutePosition { get; }

        bool CanRaiseEvents { get; set; }

        string Id { get; set; }

        bool IsEnabled { get; set; }

        bool IsFocused { get; }

        bool IsHighlighted { get; set; }

        bool IsVisible { get; set; }

        UIElement Parent { get; }

        Vector2 Position { get; set; }

        event EventHandler GotFocus;

        event EventHandler HighlightedChanged;

        event EventHandler<KeyEventArgs> KeyDown;

        //event KeyPressEventHandler KeyPress;
        event EventHandler<KeyEventArgs> KeyUp;

        event EventHandler LostFocus;

        event EventHandler<PointerEventArgs> PointerCaptureChanged;

        event EventHandler<PointerEventArgs> Tap;

        event EventHandler<PointerEventArgs> PointerPressed;

        event EventHandler<PointerEventArgs> PointerEntered;

        event EventHandler<PointerEventArgs> PointerExited;

        event EventHandler<PointerEventArgs> PointerMoved;

        event EventHandler<PointerEventArgs> PointerReleased;

        event EventHandler<PointerEventArgs> PointerWheelChanged;

        event EventHandler Move;

        event EventHandler PositionChanged;

        event EventHandler SizeChanged;

        event EventHandler VisibleChanged;

        void Focus();

        bool Contains(Vector2 cursorLocation);
    }

    public interface IXmlControl
    {
        XmlBaseControl ToXmlControl();
    }
}