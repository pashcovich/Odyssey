using Odyssey.Engine;
using Odyssey.Talos.Components;
using SharpDX;

namespace Odyssey.Talos.Nodes
{
    public abstract class CameraNode : ICamera
    {
        public long EntityId { get { return entity.Id; } }
        readonly IEntity entity;

        readonly PositionComponent cPosition;
        readonly CameraComponent cCamera;
        readonly UpdateComponent cUpdate;

        protected IEntity SourceEntity { get { return entity; } }

        public PositionComponent PositionComponent { get { return cPosition; } }
        public CameraComponent CameraComponent { get { return cCamera; } }
        public UpdateComponent UpdateComponent { get { return cUpdate; } }

        protected CameraNode(IEntity entity)
        {
            this.entity = entity;
            cPosition = entity.GetComponent<PositionComponent>();
            cCamera = entity.GetComponent<CameraComponent>();
            cUpdate = entity.GetComponent<UpdateComponent>();
        }

        public abstract void Reset();

        public abstract void Update(ITimeService time);

        public int Id
        {
            get { return cCamera.CameraId; }
        }

        public Vector3 Direction
        {
            get { return Vector3.Normalize(CameraComponent.Target - cPosition.Position); }
        }

        public Matrix World
        {
            get { return Matrix.Translation(cPosition.Position); }
        }

        public Matrix Projection
        {
            get { return cCamera.Projection; }
        }

        public Matrix View
        {
            get { return cCamera.View; }
        }

        public ViewportF Viewport
        {
            get { return cCamera.Viewport; }
        }

        public Matrix WorldViewProjection
        {
            get { return World*View*Projection; }
        }

        internal Vector3 CrossDirection
        {
            get { return Vector3.Cross(Direction, cCamera.Up); }
        }


        
    }
}
