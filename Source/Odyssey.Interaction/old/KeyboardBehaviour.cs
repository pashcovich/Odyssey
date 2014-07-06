using Odyssey.Devices;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.Interaction
{
    public class KeyboardBehaviour : IKeyboardController, IObjectController
    {
        private const float Speed = 5.0f;
        private SortedDictionary<Key, KeyBinding> keyBindings;
        private bool[] actions;

        public KeyboardBehaviour()
        {
            keyBindings = new SortedDictionary<Key, KeyBinding>();
            KeyBinding[] keyArray = new KeyBinding[] {
                 new KeyBinding(KeyAction.MoveForward,Key.W, MoveForward),
                new KeyBinding(KeyAction.MoveBackward, Key.S, MoveBackard),
                new KeyBinding(KeyAction.StrafeLeft, Key.A, MoveLeft),
                new KeyBinding(KeyAction.StrafeRight, Key.D, MoveRight)
            };

            foreach (KeyBinding kb in keyArray)
                this.keyBindings.Add(kb.Key, kb);
            actions = new bool[keyBindings.Count()];
        }

        public KeyboardBehaviour(IEnumerable<KeyBinding> keyBindings)
        {
            this.keyBindings = new SortedDictionary<Key, KeyBinding>();
            foreach (KeyBinding kb in keyBindings)
                this.keyBindings.Add(kb.Key, kb);
            actions = new bool[keyBindings.Count()];
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public IInteractive3DModel Mesh
        {
            get;
            set;
        }

        void IKeyboardController.OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(true);
        }

        void IKeyboardController.OnKeyUp(object sender, KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(false);
        }

        public void Update()
        {
            foreach (KeyValuePair<Key, KeyBinding> kvp in keyBindings)
            {
                KeyBinding kb = kvp.Value;

                if (kb.State) kb.Operation();
            }
        }

        void IInputController.Add()
        {
            Mesh.SetController(this);
        }

        void IInputController.Remove()
        {
            this.Mesh.RemoveController(this);
        }

        public void SetState(KeyAction action, bool state)
        {
            actions[(int)action] = state;
        }

        private void MoveLeft()
        {
            //Mesh.Translate(Vector3.UnitX, -Speed);
        }

        private void MoveRight()
        {
            //Mesh)Mesh).Translate(Vector3.UnitX, Speed);
        }

        private void MoveForward()
        {
            //Mesh.Translate(Vector3.UnitZ, Speed);
        }

        private void MoveBackard()
        {
            //Mesh.Translate(Vector3.UnitZ, -Speed);
        }
    }
}