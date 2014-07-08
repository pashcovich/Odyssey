using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;

using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math
{
    public class MatrixMultiplyNode : MultiplyNode
    {
        public override string Access()
        {
            return string.Format("mul({0},{1})", Input1.Reference, Input2.Reference);
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
            MatrixMultiplyNode m1m2 = new MatrixMultiplyNode
            {
                Input1 = new ReferenceNode { Value = m1 },
                Input2 = new ReferenceNode { Value = m2 },
            };

            MatrixMultiplyNode mulResult = new MatrixMultiplyNode
            {
                Input1 = m1m2,
                Input2 = new ReferenceNode { Value = m3 },
                IsVerbose = true
            };

            return mulResult;
        }

        public static MatrixMultiplyNode WorldViewProjection
        {
            get
            {
                return Multiply(Matrix.EntityWorld, Matrix.CameraView, Matrix.CameraProjection);
            }
        }

        public static MatrixMultiplyNode LightWorldViewProjection
        {
            get
            {
                MatrixMultiplyNode mulVP = new MatrixMultiplyNode
                {
                    Input1 = new ReferenceNode { Value = Matrix.LightView },
                    Input2 = new ReferenceNode { Value = Matrix.LightProjection },
                };

                MatrixMultiplyNode mulLightWVP = new MatrixMultiplyNode
                {
                    Input1 = new ReferenceNode { Value = Matrix.EntityWorld },
                    Input2 = mulVP,
                    IsVerbose = true
                };

                return mulLightWVP;
            }
        }
    }
}