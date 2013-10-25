using Odyssey.Graphics.Materials;
using SharpDX;

namespace Odyssey.Graphics
{
    public interface IInstance
    {
        Matrix World { get; set; }
        Matrix Translation { get; }
        Matrix Rotation { get; }
        Matrix Scaling { get; }
        Vector3 AbsolutePosition { get; }
        Vector3 PositionV3 { get; }
        Vector4 PositionV4 { get; }
        Vector3 RotationDelta { get; }
        Vector3 ScalingValues { get; }
        Quaternion CurrentRotation { get; set; }

        IMaterial Material { get; set; }
    }

    

}
