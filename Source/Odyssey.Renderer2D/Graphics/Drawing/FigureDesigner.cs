using System;
using System.Collections.Generic;
using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public class FigureDesigner
    {
        private readonly List<VectorCommand> commands;

        public FigureDesigner()
        {
            commands = new List<VectorCommand>();
        }

        public void DrawRingSegment(float angleFrom, float angleTo, Vector2 center, float outerRadius, float innerRadius, bool isClosed = true)
        {
            float t0 = angleFrom;
            float t1 = angleTo;
            float r1 = outerRadius;
            float r2 = innerRadius;

            commands.Add(new VectorCommand('M', new[] {center.X + (float) Math.Sin(t0)*r2, center.Y - (float) Math.Cos(t0)*r2}));
            commands.Add(new VectorCommand('A', new[] {r2, r2, 0, 0, 1, center.X + (float) Math.Sin(t1)*r2, center.Y - (float) Math.Cos(t1)*r2}));
            commands.Add(new VectorCommand('L', new[] {center.X + (float) Math.Sin(t1)*r1, center.Y - (float) Math.Cos(t1)*r1}));
            commands.Add(new VectorCommand('A', new[] {r1, r1, 0, 0, 0, center.X + (float) Math.Sin(t0)*r1, center.Y - (float) Math.Cos(t0)*r1}));
            
            if (isClosed)
                commands.Add(new VectorCommand('Z', null));

        }

        public IEnumerable<VectorCommand> Result { get { return commands; } }
    }
}
