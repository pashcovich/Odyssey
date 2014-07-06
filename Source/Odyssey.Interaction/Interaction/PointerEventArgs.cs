using System;

namespace Odyssey.Interaction
{
    public class PointerEventArgs : EventArgs
    {
        public int EventId { get; set; }
        public bool Handled { get; set; }
        public PointerPoint CurrentPoint { get; private set; }

        public PointerEventArgs(int eventId, PointerPoint point)
        {
            EventId = eventId;
            CurrentPoint = point;
        }

    }
}
