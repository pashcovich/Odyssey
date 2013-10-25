
namespace Odyssey.Devices
{
    public delegate void KeyOperation();

    public class KeyBinding
    {
        public KeyAction Action { get; private set; }
        public Key Key { get; private set; }
        public bool State { get; private set; }
        public KeyOperation Operation { get; private set; }

        public void Apply(bool state)
        {
            State = state;
        }

        public KeyBinding(KeyAction action, Key key, KeyOperation operation)
        {
            Action = action;
            Key = key;
            Operation = operation;
        }

        void Execute()
        {
            Operation();
        }

    }
}
