using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace Odyssey.Geometry.Extensions
{
    public static class Vector4Extensions
    {
        public static Vector3 ToVector3(this Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        }

        public static Vector3[] ToVector3Array(this Vector4[] vector4Array)
        {
            var vector3Array = new Vector3[vector4Array.Length - 1];
            for (int i = 0; i < vector4Array.Length - 1; i++)
                vector3Array[i] = vector4Array[i].ToVector3();

            return vector3Array;
        }

        public static Vector4[] Concatenate(this IEnumerable<Vector4> vector4Array, Vector4 field)
        {
            return vector4Array.Concat(new[] { field }).ToArray();
        }

    }
}
