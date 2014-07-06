using System;

namespace Odyssey.UserInterface.Controls
{
    public class ControlEventArgs : EventArgs
    {
        public ControlEventArgs(UIElement control)
        {
            this.Control = control;
        }

        public UIElement Control { get; set; }
    }
}
