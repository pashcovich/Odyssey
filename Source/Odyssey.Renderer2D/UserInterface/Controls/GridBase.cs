using Odyssey.Core;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public abstract class GridBase : PanelBase
    {
        /// <summary>
        /// The key to the Column attached dependency property. This defines the column an item is inserted into.
        /// </summary>
        /// <remarks>First column has 0 as index</remarks>
        public readonly static PropertyKey<int> ColumnPropertyKey = new PropertyKey<int>("ColumnKey", typeof(GridBase), DefaultValueMetadata.Static(0));

        /// <summary>
        /// The key to the Row attached dependency property. This defines the row an item is inserted into.
        /// </summary>
        /// <remarks>First row has 0 as index</remarks>
        public readonly static PropertyKey<int> RowPropertyKey = new PropertyKey<int>("RowKey", typeof(GridBase), DefaultValueMetadata.Static(0));

        protected GridBase(string controlClass) : base(controlClass)
        {
        }

        protected Int3 GetElementGridPositions(UIElement element)
        {
            return new Int3(
                element.DependencyProperties.Get(ColumnPropertyKey),
                element.DependencyProperties.Get(RowPropertyKey),
                0); //element.DependencyProperties.Get(LayerPropertyKey));
        }
    }
}
