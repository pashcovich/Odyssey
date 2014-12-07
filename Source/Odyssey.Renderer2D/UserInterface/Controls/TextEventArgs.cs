using System;

namespace Odyssey.UserInterface.Controls
{
    public class TextEventArgs : EventArgs
    {
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        public TextEventArgs(string newValue, string oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
