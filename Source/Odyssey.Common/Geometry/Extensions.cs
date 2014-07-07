using SharpDX;
using System.Linq;

namespace Odyssey.Geometry
{
    public static class Extensions
    {

        #region Vector2 extensions

        public static Vector4 ToVector4(this Vector2 vector2, float y = 0f, float w = 0f)
        {
            return new Vector4(vector2, 0, 0);
        }

        #endregion

        #region Vector3 extensions
        public static Vector4 ToVector4(this Vector3 vector3, float w = 1.0f)
        {
            return new Vector4(vector3, w);
        }

       

        #endregion

        #region Vector4 extensions
        public static Vector3 ToVector3(this Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        }

        public static Vector3[] ToVector3Array(this Vector4[] vector4Array)
        {
            Vector3[] vector3Array = new Vector3[vector4Array.Length - 1];
            for (int i = 0; i < vector4Array.Length - 1; i++)
                vector3Array[i] = vector4Array[i].ToVector3();

            return vector3Array;
        }

        public static Vector4[] Concatenate(this Vector4[] vector4Array, Vector4 field)
        {
            return vector4Array.Concat(new[] { field }).ToArray();
        }
        #endregion 

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

        #region Plane extensions
        #endregion


    }
}
