using System;
using SharpDX.Mathematics;
using System.Linq;

namespace Odyssey.Geometry.Extensions
{
    public static class Extensions
    {

        #region Matrix

        public static Vector4[] ToVector4Array(this Matrix matrix)
        {
            return new[] {
                new Vector4(matrix.M11, matrix.M12, matrix.M13, matrix.M14),
                new Vector4(matrix.M21, matrix.M22, matrix.M23, matrix.M24),
                new Vector4(matrix.M31, matrix.M32, matrix.M33, matrix.M34),
                new Vector4(matrix.M41, matrix.M42, matrix.M43, matrix.M44)
            };
        }

        #endregion

        #region Plane
        #endregion

        #region RectangleF


        #endregion


    }
}
