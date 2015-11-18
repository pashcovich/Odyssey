using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public class MatrixMultiplyNode : MultiplyNode
    {
        public override string Access()
        {
            return $"mul({Input1.Reference},{Input2.Reference})";
        }

        /// <summary>
        /// Multiplies (m1*m2)*m3.
        /// </summary>
        /// <param name="m1">The first matrix.</param>
        /// <param name="m2">The second matrix.</param>
        /// <param name="m3">The third matrix.</param>
        /// <returns>The resulting <see cref="MatrixMultiplyNode"/>.</returns>
        public static MatrixMultiplyNode Multiply(Matrix m1, Matrix m2, Matrix m3)
        {
            var m1m2 = new MatrixMultiplyNode
            {
                Input1 = new ReferenceNode { Value = m1 },
                Input2 = new ReferenceNode { Value = m2 },
            };

            var mulResult = new MatrixMultiplyNode
            {
                Input1 = m1m2,
                Input2 = new ReferenceNode { Value = m3 },
                IsVerbose = true
            };

            return mulResult;
        }

        public static MatrixMultiplyNode WorldViewProjection => Multiply(Matrix.EntityWorld, Matrix.CameraView, Matrix.CameraProjection);

        public static MatrixMultiplyNode LightWorldViewProjection
        {
            get
            {
                var mulVP = new MatrixMultiplyNode
                {
                    Input1 = new ReferenceNode { Value = Matrix.LightView },
                    Input2 = new ReferenceNode { Value = Matrix.LightProjection },
                };

                var mulLightWVP = new MatrixMultiplyNode
                {
                    Input1 = new ReferenceNode { Value = Matrix.EntityWorld },
                    Input2 = mulVP,
                    IsVerbose = true
                };

                return mulLightWVP;
            }
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
        }
    }
}