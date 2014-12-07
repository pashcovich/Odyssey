using System;
using Odyssey.Engine;

namespace Odyssey.UserInterface.Events
{
    public class TimeEventArgs : EventArgs
    {
        public ITimeService Time { get; private set; }

        public TimeEventArgs(ITimeService time)
        {
            Time = time;
        }
    }
}
