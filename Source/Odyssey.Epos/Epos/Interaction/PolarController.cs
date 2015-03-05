using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Interaction;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Interaction
{
    public class PolarController : PointerControllerBase
    {

        private float arcBallRadius;
        private bool isDragging;
        private Vector2 lastPointerPosition;
        private RotationMode mode;
        private float totalYaw;
        private float totalPitch;
        private float rotationSpeed = 1.0f;
        private Quaternion qStart;
        private Quaternion qCurrent;

        public PolarController(IServiceRegistry services)
            : base(services)
        {
        }

        /// <summary>
        /// ArcBall radius multiplier.
        /// </summary>
        public float ArcBallRadius
        {
            get { return arcBallRadius; }
            set { arcBallRadius = value; }
        }

        protected override void PointerPressed(PointerPoint point, ITimeService time)
        {
            lastPointerPosition = point.Position;
            if (point.IsLeftButtonPressed)
            {
                isDragging = true;
                qStart = COrientation.Orientation;

            }
        }

        protected override void PointerMoved(PointerPoint point, ITimeService time)
        {
            if (isDragging)
            {
                Vector2 currentPoint = point.Position;
                UpdateEntity(currentPoint);
            }

            lastPointerPosition = point.Position;
        }

        protected override void PointerReleased(PointerPoint point, ITimeService time)
        {
            if (point.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                isDragging = false;
                qStart = COrientation.Orientation;
                qCurrent = Quaternion.Identity;
            }
        }

        private static Vector3 MapToArcBall(Vector2 point, Vector2 screenSize, float arcBallRadius)
        {
            //// Adjust point coords and scale down to range of [-1 ... 1]
            float x = -(point.X - screenSize.X/2)/(arcBallRadius*screenSize.X/2);
            float y = (point.Y - screenSize.Y/2)/(arcBallRadius*screenSize.Y/2);

            float z = 0.0f;
            float mag = x*x + y*y;

            if (mag > 1.0f)
            {
                float scale = 1.0f/(float) Math.Sqrt(mag);
                x *= scale;
                y *= scale;
            }
            else
                z = (float)Math.Sqrt(1.0f - mag);

            // Return vector
            return new Vector3(x, y, z);
        }

        protected virtual void UpdateEntity(Vector2 current)
        {
            if (current.X < 0 || current.X > ScreenSize.X)
                return;

            if (current.Y < 0 || current.Y > ScreenSize.Y)
                return;

            Vector2 delta = current - lastPointerPosition;
            totalYaw += delta.X;
            totalPitch += delta.Y;
            //switch (mode)
            //{
            //    case RotationMode.Yaw:
            //        currentPoint = new Vector3(currentPoint.X, prevSpherePoint.Y, prevSpherePoint.Z);
            //        break;
            //}

            // Computes the quaternion necessary to rotate from the previous 
            // ArcBall point to the current one
            //var r = Quaternion.RotationAxis(Vector3.UnitY, totalYaw/100f);
            //var s = Quaternion.RotationAxis(Vector3.UnitX, totalPitch / 100f);
            var r = Quaternion.RotationYawPitchRoll(totalYaw/100, totalPitch/100, 0);
            // Rotation relative to local Frame
            //Quaternion rot = Quaternion.Normalize(qCurrent*qStart);
            COrientation.Orientation = Quaternion.Normalize(r*Quaternion.Invert(qStart));
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
