using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public class Grid : GridBase
    {
        private readonly StripDefinitionCollection[] stripDefinitions = new[]
        {
            new StripDefinitionCollection(),
            new StripDefinitionCollection(),
        };

        private readonly List<float>[] cachedStripPositions =
        {
            new List<float>(),
            new List<float>(), 
        };


        private readonly List<float>[] cachedStripSizes =
        {
            new List<float>(),
            new List<float>(), 
        };

        public Grid() : base(typeof(Grid).Name)
        {
        }

        void CalculateStripPositions()
        {
            for (int dim = 0; dim < stripDefinitions.Length; dim++)
            {
                cachedStripPositions[dim].Clear();
                float startPosition = 0;
                for (int i = 0; i < stripDefinitions[dim].Count; i++)
                {
                    var strip = stripDefinitions[dim][i];
                    cachedStripPositions[dim].Add(startPosition);
                    startPosition += strip.ActualSize;
                }
            }
        }

        void CalculateStripSizes()
        {
            for (int dim = 0; dim < stripDefinitions.Length; dim++)
            {
                cachedStripSizes[dim].Clear();
                for (int i = 0; i < stripDefinitions[dim].Count; i++)
                {
                    var strip = stripDefinitions[dim][i];
                    cachedStripSizes[dim].Add(strip.ActualSize);
                }
            }
        }

        protected override void Measure()
        {
            base.Measure();
            var size = new Vector2(Width, Height);
            for (int dim = 0; dim < stripDefinitions.Length; dim++)
            {
                float stripSum = stripDefinitions[dim].Sum(s => s.SizeValue);
                for (int i = 0; i < stripDefinitions[dim].Count; i++)
                {
                    var strip = stripDefinitions[dim][i];
                    strip.ActualSize = strip.SizeValue/stripSum*size[dim];
                }
            }

            CalculateStripPositions();
            CalculateStripSizes();
        }

        protected override void Arrange()
        {
            base.Arrange();
            foreach (var control in Controls.Public)
            {
                var gridPosition = GetElementGridPositions(control);
                control.Position = new Vector2(cachedStripPositions[0][gridPosition.X], cachedStripPositions[1][gridPosition.Y]);
                control.Width = cachedStripSizes[0][gridPosition.X];
                control.Height = cachedStripSizes[1][gridPosition.Y];
            }
        }

        public void AddRowDefinition(StripDefinition row)
        {
            stripDefinitions[1].Add(row);
        }

        public void AddColumnDefinition(StripDefinition column)
        {
            stripDefinitions[0].Add(column);
        }
    }
}
