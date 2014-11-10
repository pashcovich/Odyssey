using System.Collections.Generic;
using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public class FigureParser
    {
        private readonly GeometrySink sink;
        private Vector2 startPoint;
        private Vector2 previousPoint;

        public bool IsFigureOpen { get; private set; }

        public FigureParser(GeometrySink sink)
        {
            this.sink = sink;
        }

        public void Execute(IEnumerable<VectorCommand> commands)
        {
            foreach (var instruction in commands)
            {
                switch (instruction.Command)
                {
                    case 'M':
                        Move(instruction, true);
                        break;

                    case 'L':
                        Line(instruction, true);
                        break;

                    case 'Z':
                        Close(FigureEnd.Closed);
                        break;

                    case 'A':
                        Arc(instruction, true);
                        break;

                }
            }
        }

        void Move(VectorCommand instruction, bool isAbsolute)
        {
            if (IsFigureOpen)
                Close(FigureEnd.Open);

            var point = new Vector2(instruction.Arguments[0], instruction.Arguments[1]);
            startPoint = isAbsolute ? point : point + startPoint;
            previousPoint = startPoint;
            sink.BeginFigure(startPoint, FigureBegin.Filled);
            IsFigureOpen = true;
        }

        void Line(VectorCommand instruction, bool isAbsolute)
        {
            var points = new List<Vector2>();
            for (var i = 0; i < instruction.Arguments.Length; i = i + 2)
            {
                var point = new Vector2(instruction.Arguments[i], instruction.Arguments[i + 1]);
                if (!isAbsolute)
                    point += previousPoint;
                points.Add(point);
                previousPoint = points[i];
            }
            sink.AddLines(points);
        }

        void Arc(VectorCommand instruction, bool isAbsolute)
        {
            float w = instruction.Arguments[0];
            float h = instruction.Arguments[1];
            float a = instruction.Arguments[2];
            bool isLargeArc = (int)instruction.Arguments[3] == 1;
            bool sweepDirection = (int)instruction.Arguments[4] == 1;
            
            var p = new Vector2(instruction.Arguments[5], instruction.Arguments[6]);
            if (!isAbsolute)
                p += previousPoint;
            sink.AddArc(w,h, a, isLargeArc, sweepDirection, p);
        }

        void Close(FigureEnd endType)
        {
            IsFigureOpen = false;
            sink.EndFigure(endType);
        }

    }
}
