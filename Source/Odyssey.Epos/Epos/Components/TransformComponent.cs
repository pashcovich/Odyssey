using SharpDX;

namespace Odyssey.Epos.Components
{
    public class TransformComponent : Component
    {
        public Matrix Local { get; set; }
        public Matrix World { get; set; }
        public Matrix Translation { get; internal set; }
        public Matrix Rotation { get; internal set; }
        public Matrix Scaling { get; internal set; }

        public TransformComponent()
            : base(ComponentTypeManager.GetType<TransformComponent>())
        {
            Local = Matrix.Identity;
            World = Matrix.Identity;
            Translation = Matrix.Identity;
            Rotation = Matrix.Identity;
            Scaling = Matrix.Identity;
        }
    }
}
