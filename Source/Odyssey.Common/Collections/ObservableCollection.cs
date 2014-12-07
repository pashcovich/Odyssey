#region Using Directives

using System;
using System.Collections.ObjectModel;

#endregion

#region Other Licenses
// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
#endregion

namespace Odyssey.Collections
{
    /// <summary>
    ///     An observable collection.
    /// </summary>
    /// <typeparam name="T">Type of a collection item</typeparam>
    public class ObservableCollection<T> : Collection<T>
    {
        /// <summary>
        ///     Raised when an item is added to this instance.
        /// </summary>
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemAdded;

        /// <summary>
        ///     Raised when a item is removed from this instance.
        /// </summary>
        public event EventHandler<ObservableCollectionEventArgs<T>> ItemRemoved;

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(base[i]));

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            if (base.Contains(item))
                throw new ArgumentException("This item is already added");

            base.InsertItem(index, item);

            if (item != null)
                OnComponentAdded(new ObservableCollectionEventArgs<T>(item));
        }

        protected override void RemoveItem(int index)
        {
            T item = base[index];
            base.RemoveItem(index);
            if (item != null)
                OnComponentRemoved(new ObservableCollectionEventArgs<T>(item));
        }

        protected override void SetItem(int index, T item)
        {
            throw new NotSupportedException("Cannot set item into this instance");
        }

        private void OnComponentAdded(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemAdded;
            if (handler != null) handler(this, e);
        }

        private void OnComponentRemoved(ObservableCollectionEventArgs<T> e)
        {
            EventHandler<ObservableCollectionEventArgs<T>> handler = ItemRemoved;
            if (handler != null) handler(this, e);
        }
    }
}