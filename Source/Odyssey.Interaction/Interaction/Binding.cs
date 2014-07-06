
using System;
using SharpYaml.Serialization;

namespace Odyssey.Interaction
{
    [YamlTag("Binding")] 
    public class Binding<TIndex, TAction>
    {
        [YamlMember(0)] public TAction ActionType { get; set; }
        [YamlMember(1)] public TIndex Key { get; set; }
        [YamlMember(2)] public ButtonStateFlags Trigger { get; set; }

        public Binding(TIndex key, TAction action, ButtonStateFlags trigger)
        {
            Key = key;
            ActionType = action;
            Trigger = trigger;

        }
    }

}
