using Odyssey.Engine;
using SharpDX.Mathematics;
using System;

namespace Odyssey.Epos.Components
{
    public enum CameraType
    {
        PerspectiveMono,
        Orthographic,
        PerspectiveStereo
    }

    [RequiredComponent(typeof(PositionComponent))]
    public class CameraComponent : Component, ICamera
    {
        public int Index { get; set; }
        public float NearClip { get; set; }
        public float FarClip { get; set; }
        public float FieldOfView { get; set; }
        public CameraType Type { get; protected set; }
        public Vector3 Up { get; set; }
        public Matrix View { get; internal set; }
        public Matrix Projection { get; internal set; }
        public ViewportF Viewport { get; set; }
        public bool Changed { get; internal set; }

        public Vector3 Direction => new Vector3(-View.M13, -View.M23, -View.M33);

        public Vector3 AxisX => new Vector3(View.M11, View.M21, View.M31);

        public Vector3 AxisY => new Vector3(View.M12, View.M22, View.M32);

        public Vector3 AxisZ => new Vector3(View.M13, View.M23, View.M33);

        protected CameraComponent(ComponentType type) : base(type)
        {
            Index = 0;
            NearClip = 1.0f;
            FarClip = 1000.0f;
            FieldOfView = (float)(Math.PI / 4);
            Up = Vector3.UnitY;
        }

        public CameraComponent() :this(ComponentTypeManager.GetType<CameraComponent>())
        {
            Type = CameraType.PerspectiveMono;
        }

        Vector3 ICamera.Position => Owner.GetComponent<PositionComponent>().Position;
    }
}
