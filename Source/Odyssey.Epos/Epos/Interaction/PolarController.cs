using System;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Interaction;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Interaction
{
    public class PolarController : PointerControllerBase
    {
        private float arcBallRadius;
        private RotationMode mode;
        private float rotYaw;
        private float rotPitch;
        private  float rotationSpeed = 1.0f;
        private Quaternion qStart;

        public MouseButtons RotationButton { get; set; }

        public PolarController(IServiceRegistry services)
            : base(services)
        {
            RotationButton = MouseButtons.Right;
        }

        protected override void PointerPressed(PointerPoint point, ITimeService time)
        {
            PreviousPointerPosition = point.Position;
            switch (Translate(point.PointerUpdateKind))
            {
                case MouseButtons.Left:
                    IsLeftButtonDragging = true;
                    break;

                case MouseButtons.Right:
                    IsRightButtonDragging = true;
                    break;
            }

            if (CheckButton(point.PointerUpdateKind, RotationButton, true))
            {
                qStart = COrientation.Orientation;
            }
        }

        protected override void PointerMoved(PointerPoint point, ITimeService time)
        {
            CurrentPointerPosition = point.Position;
            if (IsRightButtonDragging || IsLeftButtonDragging)
            {
                UpdateEntity(time);
            }

            PreviousPointerPosition = point.Position;
        }

        protected override void PointerReleased(PointerPoint point, ITimeService time)
        {
            switch (Translate(point.PointerUpdateKind))
            {
                case MouseButtons.Left:
                    IsLeftButtonDragging = false;
                    break;

                case MouseButtons.Right:
                    IsRightButtonDragging = false;
                    break;
            }

            if (CheckButton(point.PointerUpdateKind, RotationButton, false))
            {
                qStart = COrientation.Orientation;
                rotPitch = rotYaw = 0;
            }
        }

        protected virtual void UpdateEntity(ITimeService time)
        {
            if (!IsRightButtonDragging)
                return;
            if (CurrentPointerPosition.X < 0 || CurrentPointerPosition.X > ScreenSize.X)
                return;

            if (CurrentPointerPosition.Y < 0 || CurrentPointerPosition.Y > ScreenSize.Y)
                return;

            Vector2 delta = CurrentPointerPosition - PreviousPointerPosition;
            rotYaw = delta.X * rotationSpeed * time.FrameTime;
            rotPitch = delta.Y * rotationSpeed*time.FrameTime;
           
            var r = Quaternion.RotationAxis(Vector3.UnitY, rotYaw);
            var s = Quaternion.RotationAxis(Vector3.UnitX, rotPitch);

            COrientation.Orientation = s*r* COrientation.Orientation;
        }

        protected override void KeyPressed(Keys key, ITimeService time)
        {
            switch (key)
            {
                case Keys.Shift:
                case Keys.LeftShift:
                    mode = RotationMode.Yaw;
                    break;
            }
        }

        protected override void KeyReleased(Keys key, ITimeService time)
        {
            switch (key)
            {
                case Keys.Shift:
                case Keys.LeftShift:
                    mode = RotationMode.YawPitchRoll;
                    break;
            }
        }
    }
}
