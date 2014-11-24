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
                cachedStripPositions[dim].Add(startPosition);
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

        protected override Vector2 MeasureOverride(Vector2 availableSizeWithoutMargins)
        {
            for (int dim = 0; dim < stripDefinitions.Length; dim++)
            {
                float stripSum = stripDefinitions[dim].Sum(s => s.SizeValue);
                for (int i = 0; i < stripDefinitions[dim].Count; i++)
                {
                    var strip = stripDefinitions[dim][i];
                    strip.ActualSize = strip.SizeValue/stripSum*availableSizeWithoutMargins[dim];
                }
            }

            CalculateStripPositions();
            CalculateStripSizes();

            foreach (var control in Controls.Public)
            {
                var gridPosition = GetElementGridPositions(control);
                control.Measure(new Vector2(cachedStripSizes[0][gridPosition.X], cachedStripSizes[1][gridPosition.Y]));
            }

            return availableSizeWithoutMargins;
        }

        protected override Vector2 ArrangeOverride(Vector2 availableSizeWithoutMargins)
        {
            CalculateStripPositions();
            CalculateStripSizes();

            foreach (var control in Controls.Public)
            {
                var gridPosition = GetElementGridPositions(control);
                control.Position = new Vector2(cachedStripPositions[0][gridPosition.X], cachedStripPositions[1][gridPosition.Y]);
                control.Arrange(new Vector2(cachedStripSizes[0][gridPosition.X],cachedStripSizes[1][gridPosition.Y] ));
            }

            Vector2 finalSize = Vector2.Zero;
            for (int dim = 0; dim < 2; dim++)
                finalSize[dim] = Math.Max(cachedStripPositions[dim][stripDefinitions[dim].Count], availableSizeWithoutMargins[dim]);

            return finalSize;
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
