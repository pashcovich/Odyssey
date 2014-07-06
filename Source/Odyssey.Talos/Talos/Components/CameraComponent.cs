using System.ComponentModel;
using Odyssey.Engine;
using Odyssey.Graphics;
using SharpDX;
using System;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Components
{
    public enum CameraType
    {
        Perspective,
        Orthographic,
        Stereo
    }

    [YamlTag("Camera")]
    public class CameraComponent : Component, IInitializable
    {
        public int CameraId { get; set; }
        public CameraType Type { get; set; }
        public float NearClip { get; set; }
        public float FarClip { get; set; }
        public float FieldOfView { get; set; }
        [YamlIgnore] public bool IsInited { get; private set; }

        [YamlStyle(YamlStyle.Flow)]
        public Vector3 Up { get; set; }

        [YamlStyle(YamlStyle.Flow)]
        public Vector3 Target { get; set; }

        [YamlIgnore]
        public Matrix View { get; set; }

        [YamlIgnore]
        public Matrix Projection { get; set; }

        public ViewportF Viewport { get; set; }

        public CameraComponent() : base(ComponentTypeManager.GetType<CameraComponent>())
        {
            CameraId = 0;
            NearClip = 1.0f;
            FarClip = 1000.0f;
            FieldOfView = (float) (Math.PI/4);
            Up = Vector3.UnitY;
            Target = Vector3.Zero;
        }

        public void Initialize()
        {
            if (Viewport != default(ViewportF))
                return;

            var deviceSettings = Services.GetService<IDirectXDeviceSettings>();
            Viewport = new ViewportF(0, 0, deviceSettings.PreferredBackBufferWidth, deviceSettings.PreferredBackBufferHeight);
            IsInited = true;
        }

        public void Unload()
        {
            IsInited = false;
        }
    }
}
