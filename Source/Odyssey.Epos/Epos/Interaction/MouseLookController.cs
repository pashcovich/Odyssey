using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Interaction
{
    public class MouseLookController : PointerControllerBase
    {
        private CameraComponent camera;
        private float currentYaw;
        private float currentPitch;

        public float RotationSpeed { get; set; }

        public MouseLookController(IServiceRegistry services) : base(services)
        {
            RotationSpeed = 0.25f;
        }

        public override void BindToEntity(Entity source)
        {
            base.BindToEntity(source);
            if (!source.TryGetComponent(out camera))
                throw new InvalidOperationException(string.Format("'{0}' does not contain a {1}", source.Name, camera.GetType()));

            // Initialise values based on current orientation
            Quaternion q = COrientation.Orientation;
            currentYaw = (float)Math.Asin(2 * q.X * q.Y + 2 * q.Z * q.W);
            currentPitch = -(float)Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * q.X * q.X - 2 * q.Z * q.Z);
        }

        public override void Update(ITimeService time)
        {
            Quaternion q = COrientation.Orientation;
            base.Update(time);
            if (!IsEnabled)
            {
                currentYaw = (float)Math.Asin(2 * q.X * q.Y + 2 * q.Z * q.W);
                currentPitch = (float)Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * q.X * q.X - 2 * q.Z * q.Z);
                return;
            }
            COrientation.Orientation = Quaternion.RotationAxis(Vector3.UnitX, currentPitch) * Quaternion.RotationAxis(Vector3.UnitY, currentYaw); ;
            //COrientation.Orientation = Quaternion.LookAtRH(CPosition.Position, cTarget.Location, Vector3.Up);
        }

        protected override void PointerPressed(Odyssey.Interaction.PointerPoint point, ITimeService time)
        {
            if (point.IsLeftButtonPressed)
                IsEnabled = true;
        }

        protected override void PointerMoved(Odyssey.Interaction.PointerPoint point, ITimeService time)
        {
            CurrentPointerPosition = point.Position;
            if (!IsEnabled)
                return;

            if (PreviousPointerPosition == Vector2.Zero)
                PreviousPointerPosition = CurrentPointerPosition;

            Vector2 delta = (CurrentPointerPosition - PreviousPointerPosition);
            currentYaw += MathUtil.DegreesToRadians(RotationSpeed * delta.X);
            currentPitch += MathUtil.DegreesToRadians(RotationSpeed * delta.Y);
            PreviousPointerPosition = CurrentPointerPosition;
            camera.Changed = true;

            Clamp();
        }

        protected override void PointerReleased(Odyssey.Interaction.PointerPoint point, ITimeService time)
        {
            if (!point.IsLeftButtonPressed)
                IsEnabled = false;
        }

        void Clamp()
        {
            if (currentYaw > MathUtil.Pi)
                currentYaw = MathUtil.Pi;
            if (currentYaw < -MathUtil.Pi)
                currentYaw = -MathUtil.Pi;

            if (currentPitch > MathUtil.Pi)
                currentPitch = MathUtil.Pi;
            if (currentPitch < -MathUtil.Pi)
                currentPitch = -MathUtil.Pi;
        }
    }
}
