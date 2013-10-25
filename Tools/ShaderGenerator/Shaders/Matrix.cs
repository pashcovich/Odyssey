using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public class Matrix : Variable
    {

        #region Matrices

        public static Matrix World
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.World, ShaderReference = new ShaderReference(EngineReference.MatrixWorld) }; }
        }
        public static Matrix WorldInverse
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.WorldInverse, ShaderReference = new ShaderReference(EngineReference.MatrixWorldInverse) }; }
        }

        public static Matrix View
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.View, ShaderReference = new ShaderReference(EngineReference.MatrixView) }; }
        }

        public static Matrix Projection
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.Projection, ShaderReference = new ShaderReference(EngineReference.MatrixProjection) }; }
        }
        #endregion


        public static Matrix LightView
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.LightView, ShaderReference = new ShaderReference(EngineReference.MatrixLightView) }; }
        }

        public static Matrix LightProjection
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.LightProjection, ShaderReference = new ShaderReference(EngineReference.MatrixLightProjection) }; }
        }

    }
}
