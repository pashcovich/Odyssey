using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Geometry;
using Odyssey.Interaction;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public enum CameraAction
    {
        None,
        MoveForward,
        MoveBackward,
        StrafeLeft,
        StrafeRight,
        HoverUp,
        HoverDown,
        YawLeft,
        YawRight,
        
    }

    [YamlTag("KeyBinding")]
    public class KeyBinding : Binding<Keys,CameraAction>
    {
        public KeyBinding() : base(Keys.None, CameraAction.None, ButtonStateFlags.None)
        { }

        public KeyBinding(Keys key, CameraAction action, ButtonStateFlags trigger) : base(key, action, trigger)
        {
        }

    }

    [RequiredComponent(typeof(CameraComponent))]
    [YamlTag("CameraState")]
    public class CameraStateComponent : Component
    {
        [YamlMember(1)]
        public float MovementSpeed { get; set; }
        [YamlMember(2)]
        public float StrafeSpeed { get; set; }
        [YamlMember(3)]
        public float YawSpeed { get; set; }

        public KeyBinding[] CameraBindings { get; set; }

        public CameraStateComponent() : base(ComponentTypeManager.GetType<CameraStateComponent>())
        {
            MovementSpeed = 3.0f;
            StrafeSpeed = 3f;
            YawSpeed = MathHelper.Pi/4;
        }
    }
}
