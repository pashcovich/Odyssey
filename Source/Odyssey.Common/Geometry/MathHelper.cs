#region Using Directives

using System;
using System.Collections.Generic;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Geometry
{
    public static class MathHelper
    {
        /// <summary>
        ///     A value specifying the approximation of π which is 180 degrees.
        /// </summary>
        public const float Pi = 3.141592653589793239f;

        /// <summary>
        ///     A value specifying the approximation of 2π which is 360 degrees.
        /// </summary>
        public const float TwoPi = 6.283185307179586477f;

        /// <summary>
        ///     A value specifying the approximation of π/2 which is 90 degrees.
        /// </summary>
        public const float PiOverTwo = 1.570796326794896619f;

        /// <summary>
        ///     A value specifying the approximation of π/4 which is 45 degrees.
        /// </summary>
        public const float PiOverFour = 0.785398163397448310f;

        /// <summary>
        ///     The value for which all absolute numbers smaller than are considered equal to zero.
        /// </summary>
        public const float Epsilon = 1e-6f;

        public const double EpsilonD = 1e-6d;

        public static float Scale(float value, float min, float max)
        {
            return min + (max - min)*value;
        }

        public static double Scale(double value, double min, double max)
        {
            return min + (max - min)*value;
        }

        public static double ConvertRange(
            double originalStart, double originalEnd, // original range
            double newStart, double newEnd, // desired range
            double value) // value to convert
        {
            double scale = (newEnd - newStart)/(originalEnd - originalStart);
            return (newStart + ((value - originalStart)*scale));
        }

        public static int ConvertRange(int originalStart, int originalEnd, // original range
            int newStart, int newEnd, // desired range
            int value) // value to convert
        {
            int scale = (newEnd - newStart)/(originalEnd - originalStart);
            return (newStart + ((value - originalStart)*scale));
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees*(Pi/180);
        }


        /// <summary>
        ///     Checks if a floating point Value is within a specified
        ///     range of values (inclusive).
        /// </summary>
        /// <param name="value">The Value to check.</param>
        /// <param name="min">The minimum Value.</param>
        /// <param name="max">The maximum Value.</param>
        /// <returns>
        ///     True if the Value is within the range specified,
        ///     false otherwise.
        /// </returns>
        public static bool DoubleInRange(double value, double min, double max)
        {
            return (value >= min && value <= max);
        }


        public static bool IsValid(double x)
        {
            if (double.IsNaN(x))
            {
                // NaN.
                return false;
            }

            return !double.IsInfinity(x);
        }

        public static bool IsCloseToZero(float value)
        {
            if (Math.Abs(value) < EpsilonD)
                return true;
            return false;
        }

        public static int Mod(int value, int modulo)
        {
            return (value%modulo + modulo)%modulo;
        }

        public static bool ScalarNearEqual(float s1, float s2, float tolerance = Epsilon)
        {
            return Math.Abs(s1 - s2) < tolerance;
        }

        public static Vector3 MinCoordinates(IEnumerable<Vector3> points)
        {
            float minX, minY, minZ;
            minX = minY = minZ = float.MaxValue;

            foreach (var p in points)
            {
                if (p.X < minX)
                    minX = p.X;
                if (p.Y < minY)
                    minY = p.Y;
                if (p.Z < minZ)
                    minZ = p.Z;
            }

            return new Vector3(minX, minY, minZ);
        }

        public static Vector2 MinCoordinates(IEnumerable<Vector2> points)
        {
            float minX, minY;
            minX = minY = float.MaxValue;

            foreach (var p in points)
            {
                if (p.X < minX)
                    minX = p.X;
                if (p.Y < minY)
                    minY = p.Y;
            }

            return new Vector2(minX, minY);
        }

        public static Vector3 MaxCoordinates(IEnumerable<Vector3> points)
        {
            float maxX, maxY, maxZ;
            maxX = maxY = maxZ = float.MinValue;

            foreach (var p in points)
            {
                if (p.X > maxX)
                    maxX = p.X;
                if (p.Y > maxY)
                    maxY = p.Y;
                if (p.Z > maxZ)
                    maxZ = p.Z;
            }

            return new Vector3(maxX, maxY, maxZ);
        }

        public static Vector2 MaxCoordinates(IEnumerable<Vector2> points)
        {
            float maxX, maxY;
            maxX = maxY = float.MinValue;

            foreach (var p in points)
            {
                if (p.X > maxX)
                    maxX = p.X;
                if (p.Y > maxY)
                    maxY = p.Y;
            }

            return new Vector2(maxX, maxY);
        }
    }
}