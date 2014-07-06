using Odyssey.Geometry;
using System;

namespace Odyssey.Engine
{
    public struct StereoParameters : IEquatable<StereoParameters>
    {
        const float defaultInterocularDistance = 1.25f;

        public float ViewerDistanceInches;
        public float DisplaySizeInches;
        public float PixelResolutionWidth;
        public float PixelResolutionHeight;
        public float StereoSeparationFactor;
        public float StereoExaggerationFactor;
        public float InterocularDistanceInches;

        public StereoParameters(float screenWidth, float screenHeight,
            float viewerDistanceInches = 24.0f, float displaySizeInches = 22.0f, float interocularDistanceInches = defaultInterocularDistance,
            float stereoSeparationFactor = 1.0f, float stereoExaggerationFactor = 1.0f)
            : this()
        {
            PixelResolutionWidth = screenWidth;
            PixelResolutionHeight = screenHeight;
            ViewerDistanceInches = viewerDistanceInches;
            DisplaySizeInches = displaySizeInches;
            InterocularDistanceInches = interocularDistanceInches;
            StereoSeparationFactor = stereoSeparationFactor;
            StereoExaggerationFactor = stereoExaggerationFactor;
        }

        public bool Equals(StereoParameters other)
        {
            return MathHelper.ScalarNearEqual(PixelResolutionWidth, other.PixelResolutionWidth)
                && MathHelper.ScalarNearEqual(PixelResolutionHeight, other.PixelResolutionHeight)
                && MathHelper.ScalarNearEqual(ViewerDistanceInches, other.ViewerDistanceInches)
                && MathHelper.ScalarNearEqual(DisplaySizeInches, other.DisplaySizeInches)
                && MathHelper.ScalarNearEqual(StereoSeparationFactor, other.StereoSeparationFactor)
                && MathHelper.ScalarNearEqual(StereoExaggerationFactor, other.StereoExaggerationFactor);
        }

        public static bool operator ==(StereoParameters left, StereoParameters right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(StereoParameters left, StereoParameters right)
        {
            return !left.Equals(right);
        }

    }
}
