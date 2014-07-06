using System;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Interaction;
using Odyssey.Talos.Components;
using Odyssey.Utilities.Logging;
using SharpDX;

namespace Odyssey.Talos.Nodes
{
    public class PerspectiveCameraNode : CameraNode
    {
        private readonly bool hasController;
        private readonly IKeyboardService keyboardService;
        private readonly CameraStateComponent cState;

        public PerspectiveCameraNode(IEntity entity)
            : base(entity)
        {
            entity.TryGetComponent(ComponentTypeManager.GetKeyPart<CameraStateComponent>(), out cState);
            if (cState != null)
            {
                hasController = true;
                keyboardService = entity.Scene.Services.GetService<IKeyboardService>();
            }
        }

        public override void Reset()
        {
            var deviceSettings = SourceEntity.Scene.Services.GetService<IDirectXDeviceSettings>();
            float aspectRatio = deviceSettings.PreferredBackBufferWidth / (float)deviceSettings.PreferredBackBufferHeight;

            CameraComponent.Projection = Matrix.PerspectiveFovRH(CameraComponent.FieldOfView,
                aspectRatio,
                CameraComponent.NearClip, CameraComponent.FarClip);

            CameraComponent.View = Matrix.LookAtRH(PositionComponent.Position, CameraComponent.Target, CameraComponent.Up);

            if (UpdateComponent.UpdateFrequency == UpdateFrequency.Static)
                UpdateComponent.RequiresUpdate = false;
        }

        public override void Update(ITimeService time)
        {
            if (!UpdateComponent.RequiresUpdate)
                return;

            if (hasController)
                CheckBindings(time);

            CameraComponent.View = Matrix.LookAtRH(PositionComponent.Position, CameraComponent.Target, CameraComponent.Up);
        }

        void CheckBindings(ITimeService time)
        {
            KeyboardState keyboard = keyboardService.GetState();

            foreach (var keyBinding in cState.CameraBindings)
            {
                ButtonState state = keyboard[keyBinding.Key];

                if ((state.Flags & keyBinding.Trigger) == keyBinding.Trigger)
                    ExecuteAction(keyBinding, time);
            }
        }

        void ExecuteAction(KeyBinding binding, ITimeService time)
        {
            float elapsedTime = (float) time.ElapsedApplicationTime.TotalSeconds;
            switch (binding.ActionType)
            {
                case CameraAction.MoveForward:
                    Move(cState.MovementSpeed * elapsedTime);
                    break;
                case CameraAction.MoveBackward:
                    Move(-cState.MovementSpeed*elapsedTime);
                    break;
                case CameraAction.StrafeLeft:
                    Strafe(-cState.StrafeSpeed* elapsedTime);
                    break;
                case CameraAction.StrafeRight:
                    Strafe(cState.StrafeSpeed* elapsedTime);
                    break;
                case CameraAction.HoverUp:
                    Hover(cState.MovementSpeed * elapsedTime);
                    break;
                case CameraAction.HoverDown:
                    Hover(-cState.MovementSpeed * elapsedTime);
                    break;
                case CameraAction.YawLeft:
                    Rotate(cState.YawSpeed * elapsedTime);
                    break;
                case CameraAction.YawRight:
                    Rotate(-cState.YawSpeed * elapsedTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("action");
            }
        }

        public void Move(float distance)
        {
            Vector3 offset = Direction* cState.MovementSpeed * distance;
            PositionComponent.Position += offset;
            CameraComponent.Target += offset;
        }

        public void Strafe(float distance)
        {
            Vector3 offset = CrossDirection * cState.MovementSpeed * distance;
            PositionComponent.Position += offset;
            CameraComponent.Target += offset;
        }

        public void Hover(float distance)
        {
            Vector3 offset = CameraComponent.Up * cState.MovementSpeed * distance;
            PositionComponent.Position += offset;
            CameraComponent.Target += offset;
        }

        public void Rotate(float angle)
        {
            Vector4 vector = Vector3.Transform(Direction, Matrix.RotationY(angle));
            CameraComponent.Target = vector.ToVector3() + PositionComponent.Position;
        }
    }
}