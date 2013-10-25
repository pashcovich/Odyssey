using System;

namespace Odyssey.UserInterface.Controls
{
    public class ControlEventArgs : EventArgs
    {
        public ControlEventArgs(IUIElement control)
        {
            this.Control = control;
        }

        public IUIElement Control { get; set; }
    }
}
