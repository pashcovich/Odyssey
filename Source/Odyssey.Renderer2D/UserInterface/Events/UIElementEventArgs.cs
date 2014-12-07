using System;

namespace Odyssey.UserInterface.Events
{
    public class UIElementEventArgs : EventArgs
    {
        public UIElement Element { get; private set; }
        public int Index { get; private set; }

        public UIElementEventArgs(UIElement element, int index)
        {
            Element = element;
            Index = index;
        }
    }
}
