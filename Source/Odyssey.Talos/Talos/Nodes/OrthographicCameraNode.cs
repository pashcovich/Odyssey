using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Talos.Nodes
{
    public class OrthographicCameraNode : CameraNode
    {
        public OrthographicCameraNode(IEntity entity)
            : base(entity)
        { }

        public override void Reset()
        {
            //TODO Orthographic Camera
            CameraComponent.Projection = Matrix.OrthoRH(1280,720,
                        CameraComponent.NearClip, CameraComponent.FarClip);
        }

        public override void Update(ITimeService time)
        {
            throw new System.NotImplementedException();
        }
    }
}
