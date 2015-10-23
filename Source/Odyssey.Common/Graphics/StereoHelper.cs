using System;
using System.Diagnostics.Contracts;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Text.Logging;
using SharpDX.Mathematics;

namespace Odyssey.Graphics
{
    public enum StereoChannel
    {
        NotSet,
        Left,
        Right
    }

    public enum StereoMode
    {
        Normal, Inverted
    }

    public static class StereoHelper
    {
        static bool CheckParameters(StereoParameters stereoParameters, float fovAngleY, float aspectHbyW, float nearZ, out float virtualProjection, out float zNearWidth, out float zNearHeight)
        {
            // note that most people have difficulty fusing images into 3D
            // if the separation equals even just the human average. by
            // reducing the separation (interocular distance) by 1/2, we
            // guarantee a larger subset of people will see full 3D

            // the conservative setting should always be used. the only problem
            // with the conservative setting is that the 3D effect will be less
            // impressive on smaller screens (which makes sense, since your eye
            // cannot be tricked as easily based on the smaller fov). to simulate
            // the effect of a larger screen, use the liberal settings (debug only)

            // Conservative Settings: * max acuity angle: 0.8f degrees * interoc distance: 1.25 inches

            // Liberal Settings: * max acuity angle: 1.6f degrees * interoc distance: 2.5f inches

            // maximum visual accuity angle allowed is 3.2 degrees for
            // a physical scene, and 1.6 degrees for a virtual one.
            // thus we cannot allow an object to appear any closer to
            // the viewer than 1.6 degrees (divided by two for most
            // half-angle calculations)

            const float maxStereoDistance = 85; // 780 inches (should be between 10 and 20m)
            const float maxVisualAcuityAngle = 1.6f * (MathHelper.Pi / 180.0f);  // radians
            //const float interocularDistance = 2.5f; // inches

            bool comfortableResult = true;

            float displayHeight = stereoParameters.DisplaySizeInches / (float)Math.Sqrt(aspectHbyW * aspectHbyW + 1.0f);
            float displayWidth = displayHeight * aspectHbyW;
            float halfInterocular = 0.5f * stereoParameters.InterocularDistanceInches * stereoParameters.StereoExaggerationFactor;
            float halfPixelWidth = displayWidth / stereoParameters.PixelResolutionWidth * 0.5f;
            float halfMaximumAcuityAngle = maxVisualAcuityAngle * 0.5f * stereoParameters.StereoExaggerationFactor;

            float maxSeparationAcuityAngle = (float)Math.Atan(halfInterocular / maxStereoDistance);
            float maxSeparationDistance = halfPixelWidth / (float)Math.Tan(maxSeparationAcuityAngle);
            float refinedMaxStereoDistance = maxStereoDistance - maxSeparationDistance;
            float fovHalfAngle = fovAngleY / 2.0f;

            if (refinedMaxStereoDistance < 0.0f || maxSeparationDistance > 0.1f * maxStereoDistance)
                comfortableResult = false;

            float refinedMaxSeparationAcuityAngle = (float)Math.Atan(halfInterocular / refinedMaxStereoDistance);
            float physicalZNearDistance = halfInterocular / (float)Math.Tan(halfMaximumAcuityAngle);

            float nearZSeparation = (float)Math.Tan(refinedMaxSeparationAcuityAngle) * (refinedMaxStereoDistance - physicalZNearDistance);

            zNearHeight = (float)(Math.Cos(fovHalfAngle) / Math.Sin(fovHalfAngle));
            zNearWidth = zNearHeight / aspectHbyW;
            virtualProjection = (nearZSeparation * nearZ * (zNearWidth * 4.0f)) / (2.0f * nearZ);

            return comfortableResult;
        }

        public static Matrix StereoProjectionFovRH(StereoParameters stereoParameters, float fovAngleY, 
            float aspectHbyW, 
            float nearZ, float farZ,
            StereoChannel channel, StereoMode stereoMode = StereoMode.Normal)
        {
            Contract.Requires<ArgumentException>(!MathHelper.ScalarNearEqual(fovAngleY, 0.0f));
            Contract.Requires<ArgumentException>(!MathHelper.ScalarNearEqual(aspectHbyW, 0.0f));
            Contract.Requires<ArgumentException>(!MathHelper.ScalarNearEqual(farZ, nearZ));
            Contract.Requires<ArgumentException>(stereoParameters.StereoSeparationFactor >= 0 && stereoParameters.StereoSeparationFactor <= 1.0f);
            Contract.Requires<ArgumentException>(stereoParameters.StereoExaggerationFactor >= 1.0 && stereoParameters.StereoSeparationFactor <= 2.0f);
            float virtualProjection = 0.0f;
            float zNearWidth = 0.0f;
            float zNearHeight = 0.0f;
            float invertedAngle;
            Matrix patchedProjection;
            Matrix projection;

            //if (stereoParameters == default(StereoParameters))
            //    stereoParameters = new StereoParameters(Application.Settings.ScreenWidth, Application.Settings.ScreenHeight);

            bool test = CheckParameters(stereoParameters, fovAngleY, aspectHbyW, nearZ, out virtualProjection, out zNearWidth, out zNearHeight);

            if (!test)
                LogEvent.Engine.Warning("Pixel resolution is too low to offer a comfortable stereo experience.");

            virtualProjection *= stereoParameters.StereoSeparationFactor; // incorporate developer defined bias

            //
            // By applying a translation, we are forcing our cameras to be parallel
            //

            invertedAngle = (float)Math.Atan(virtualProjection / (2.0f * nearZ));
            projection = Matrix.PerspectiveFovRH(fovAngleY, aspectHbyW, nearZ, farZ);
            if (channel == StereoChannel.Left)
            {
                if (stereoMode == StereoMode.Inverted)
                {
                    Matrix rotation, translation;
                    rotation = Matrix.RotationY(invertedAngle);
                    translation = Matrix.Translation(-virtualProjection, 0, 0);
                    patchedProjection = Matrix.Multiply(Matrix.Multiply(rotation, translation), projection);
                }
                else
                {
                    Matrix translation;
                    translation = Matrix.Translation(-virtualProjection, 0, 0);
                    patchedProjection = Matrix.Multiply(translation, projection);
                }
            }
            else
            {
                if (stereoMode == StereoMode.Inverted)
                {
                    Matrix rotation, translation;
                    rotation = Matrix.RotationY(-invertedAngle);
                    translation = Matrix.Translation(+virtualProjection, 0, 0);
                    patchedProjection = Matrix.Multiply(Matrix.Multiply(rotation, translation), projection);
                }
                else
                {
                    Matrix translation;
                    translation = Matrix.Translation(virtualProjection, 0, 0);
                    patchedProjection = Matrix.Multiply(translation, projection);
                }
            }

            return patchedProjection;
        }
    }
}
