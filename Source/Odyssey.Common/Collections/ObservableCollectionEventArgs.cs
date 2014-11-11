using System;

namespace Odyssey.Collections
{
    /// <summary>
    /// An event providing the item changed in a collection (inserted or removed).
    /// </summary>
    /// <typeparam name="T">Type of a collection item</typeparam>
    public class ObservableCollectionEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionEventArgs{T}" /> class.
        /// </summary>
        /// <param name="item">The item from the collection.</param>
        public ObservableCollectionEventArgs(T item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the item from the collection that was inserted or removed.
        /// </summary>
        /// <value>The collection item.</value>
        public T Item { get; private set; }
    }
}