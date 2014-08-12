using System;
using System.Text.RegularExpressions;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.UserInterface.Style;

namespace Odyssey.Animations
{
    public class LinearGradientBrushCurve : AnimationCurve<LinearGradientBrushKeyFrame>
    {
        public LinearGradientBrushCurve()
        {
            Function = Discrete;
        }

        public static object Discrete(LinearGradientBrushKeyFrame start, LinearGradientBrushKeyFrame end, float time)
        {
            return end.Value;
        }

        internal static LinearGradientBrushCurve FromColor4Curve(Direct2DDevice device, Color4Curve originalCurve, LinearGradientBrush originalBrush)
        {
            var styleService = device.Services.GetService<IStyleService>();
            
            var match = Regex.Match(originalCurve.TargetProperty, @"\w*\[(?<index>\d+)\]");
            int index = int.Parse(match.Groups["index"].Value);
            var newCurve = new LinearGradientBrushCurve() {Name = originalCurve.Name};
            int keyFrameIndex = 0;
            foreach (var keyframe in originalCurve)
            {
                var gStopCollection = originalBrush.GradientStops.Copy();
                gStopCollection[index].Color = keyframe.Value;
                var newGradient = new LinearGradient(string.Format("{0}.Keyframe{1:D2}", newCurve.Name, keyFrameIndex++),
                    originalBrush.StartPoint, originalBrush.EndPoint, gStopCollection);
                var newBrush = Brush.FromColorResource(device, newGradient);
                newBrush.Initialize();
                styleService.AddResource(newBrush);

                var newKeyFrame = new LinearGradientBrushKeyFrame() { Time = keyframe.Time, Value = (LinearGradientBrush)newBrush };
                newCurve.AddKeyFrame(newKeyFrame);
            }
            newCurve.TargetName = originalCurve.TargetName;
            newCurve.TargetProperty = originalCurve.TargetProperty.Split('.')[0];
            return newCurve;
        }
    }
}
