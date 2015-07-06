using Odyssey.Engine;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders.Structs
{
    public partial class ConstantBuffer
    {
        public static ConstantBuffer CBMaterial
        {
            get
            {
                ConstantBuffer cbMaterial = new ConstantBuffer()
                {
                    Name = Param.ConstantBuffer.Material,
                    CbUpdateType = CBUpdateType.InstanceFrame
                };

                cbMaterial.Add(Material);
                return cbMaterial;
            }
        }

        public static ConstantBuffer CBPerFrame
        {
            get
            {
                ConstantBuffer cbFrame = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerFrame,
                    CbUpdateType = CBUpdateType.SceneFrame
                };
                cbFrame.Add(Matrix.CameraView);
                cbFrame.Add(Matrix.CameraProjection);

                return cbFrame;
            }
        }

        public static ConstantBuffer CBPerInstance
        {
            get
            {
                ConstantBuffer cbInstance = new ConstantBuffer
                {
                    Name = Param.ConstantBuffer.PerInstance,
                    CbUpdateType = CBUpdateType.InstanceFrame
                };
                cbInstance.Add(Matrix.EntityWorld);
                return cbInstance;
            }
        }
    }
}
