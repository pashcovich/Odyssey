using Odyssey.Interaction;

namespace Odyssey.Talos.Interaction
{
    public class KeyBinding : Binding<Keys, CameraAction>
    {
        public KeyBinding()
            : base(Keys.None, CameraAction.None, ButtonStateFlags.None)
        { }

        public KeyBinding(Keys key, CameraAction action, ButtonStateFlags trigger)
            : base(key, action, trigger)
        {
        }

    }
}