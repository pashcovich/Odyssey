using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public partial class Matrix : Variable
    {
        #region Matrices

        public static Matrix EntityWorld
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.World, ShaderReference = new ShaderReference(EngineReference.EntityMatrixWorld) }; }
        }

        public static Matrix EntityInstanceWorld
        {
            get
            {
                var matrix = new Matrix
                {
                    Type = Type.Matrix,
                    Name = Param.SemanticVariables.InstanceWorld,
                    ShaderReference = new ShaderReference(EngineReference.EntityMatrixWorld),
                    Semantic = Semantics.InstanceWorld,
                };
                matrix.SetMarkup(Param.Properties.InstanceSlot, 1);
                return matrix;
            }
        }

        public static Matrix EntityWorldInverse
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.WorldInverse, ShaderReference = new ShaderReference(EngineReference.EntityMatrixWorldInverse) }; }
        }

        public static Matrix EntityWorldInverseTranspose
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.WorldInverseTranspose, ShaderReference = new ShaderReference(EngineReference.EntityMatrixWorldInverseTranspose) }; }
        }

        public static Matrix CameraView
        {
            get
            {
                var mView = new Matrix { Type = Type.Matrix, Name = Param.Matrices.View, ShaderReference = new ShaderReference(EngineReference.CameraMatrixView) };
                mView.SetMarkup(Param.Properties.CameraId, 0);
                return mView;
            }
        }

        public static Matrix CameraProjection
        {
            get
            {
                var mProjection = new Matrix { Type = Type.Matrix, Name = Param.Matrices.Projection, ShaderReference = new ShaderReference(EngineReference.CameraMatrixProjection) };
                mProjection.SetMarkup(Param.Properties.CameraId, 0);
                return mProjection;
            }
        }

        #endregion Matrices

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