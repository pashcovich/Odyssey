using System;

namespace Odyssey.Graphics.Models
{
    [Flags]
    public enum ModelOperation
    {
        None = 0,
        CalculateTangents = 1 << 0,
        CalculateBarycentricCoordinates = 1 << 1,
        CalculateBarycentricCoordinatesAndExcludeEdges = 1 << 2,
        ReverseIndices = 1 << 3,
        ReshuffleIndices = 1 << 4,
        
    }
}