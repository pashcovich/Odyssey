using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Structs
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
                    UpdateFrequency = UpdateFrequency.Static
                };

                cbMaterial.Add(Struct.Material);
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
                    UpdateFrequency = UpdateFrequency.PerFrame
                };
                cbFrame.Add(Matrix.View);
                cbFrame.Add(Matrix.Projection);

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
                    UpdateFrequency = UpdateFrequency.PerInstance
                };
                cbInstance.Add(Matrix.World);
                return cbInstance;
            }
        }
    }
}
