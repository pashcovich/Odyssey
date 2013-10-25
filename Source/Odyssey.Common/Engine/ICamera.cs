using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Engine
{
    public interface ICamera
    {
        event EventHandler CameraMoved;
        event EventHandler CameraReset;

        Matrix OrthoProjection { get; }
        Matrix Rotation { get; }
        Matrix Projection { get; }
        Matrix View { get; }
        Vector3 ViewVector { get; }
        Vector3 Position { get; }
        Quaternion Orientation { get; }
        Viewport Viewport { get; }

        float NearClip { get; }
        float FarClip { get; }
        float MovementSpeed { get; set; }
        float RotationSpeed { get; set; }
 
        void Reset();
        void LookAt(Vector3 to, Vector3 from);
        void Update();
        void Move(float distance);
        void Strafe(float distance);
        void Rotate(float angle, Vector3 axis);

    }
}
