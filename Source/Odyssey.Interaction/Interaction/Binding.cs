
using System;

namespace Odyssey.Interaction
{
    public class Binding<TIndex, TAction>
    {
        public TAction ActionType { get; set; }
        public TIndex Key { get; set; }
        public ButtonStateFlags Trigger { get; set; }

        public Binding(TIndex key, TAction action, ButtonStateFlags trigger)
        {
            Key = key;
            ActionType = action;
            Trigger = trigger;

        }
    }

}
