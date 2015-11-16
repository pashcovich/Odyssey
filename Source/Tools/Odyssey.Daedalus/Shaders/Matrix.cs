using Odyssey.Daedalus.Data;
using Odyssey.Engine;

namespace Odyssey.Daedalus.Shaders
{
    public partial class Matrix : Variable
    {
        #region Matrices

        public static Matrix EntityWorld
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.World, EngineReference = ReferenceFactory.Entity.MatrixWorld }; }
        }

        public static Matrix EntityInstanceWorld
        {
            get
            {
                var matrix = new Matrix
                {
                    Type = Type.Matrix,
                    Name = Param.SemanticVariables.InstanceWorld,
                    EngineReference = ReferenceFactory.Entity.MatrixWorld,
                    Semantic = Semantics.InstanceWorld,
                };
                matrix.SetMarkup(Param.Properties.InstanceSlot, 1);
                return matrix;
            }
        }

        public static Matrix EntityWorldInverse
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.WorldInverse, EngineReference = ReferenceFactory.Entity.MatrixWorldInverse }; }
        }

        public static Matrix EntityWorldInverseTranspose
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.WorldInverseTranspose, EngineReference = ReferenceFactory.Entity.MatrixWorldInverseTranspose }; }
        }

        public static Matrix CameraView
        {
            get
            {
                var mView = new Matrix { Type = Type.Matrix, Name = Param.Matrices.View, EngineReference = ReferenceFactory.Camera.MatrixView };
                mView.SetMarkup(Param.Properties.CameraId, 0);
                return mView;
            }
        }

        public static Matrix CameraProjection
        {
            get
            {
                var mProjection = new Matrix { Type = Type.Matrix, Name = Param.Matrices.Projection, EngineReference = ReferenceFactory.Camera.MatrixProjection };
                mProjection.SetMarkup(Param.Properties.CameraId, 0);
                return mProjection;
            }
        }

        #endregion Matrices

        public static Matrix LightView
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.LightView, EngineReference = ReferenceFactory.Light.MatrixView }; }
        }

        public static Matrix LightProjection
        {
            get { return new Matrix { Type = Type.Matrix, Name = Param.Matrices.LightProjection, EngineReference = ReferenceFactory.Light.MatrixProjection }; }
        }
    }
}