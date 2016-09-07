/*
 Miyagi v1.2.1
 Copyright (c) 2008 - 2012 Tobias Bohnen

 Permission is hereby granted, free of charge, to any person obtaining a copy of this
 software and associated documentation files (the "Software"), to deal in the Software
 without restriction, including without limitation the rights to use, copy, modify, merge,
 publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 to whom the Software is furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or
 substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 DEALINGS IN THE SOFTWARE.
 */
namespace Miyagi.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Miyagi.Common.Events;

    /// <summary>
    /// A custom implementation of IList.
    /// </summary>
    /// <typeparam name = "T">The type of items in the collection.</typeparam>
    public class MiyagiCollection<T> : IList<T>, IList
        where T : INamable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MiyagiCollection class.
        /// </summary>
        public MiyagiCollection()
        {
            this.Items = new List<T>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a new item is added.
        /// </summary>
        public event EventHandler<CollectionEventArgs<T>> ItemAdded;

        /// <summary>
        /// Occurs after an item changes.
        /// </summary>
        public event EventHandler<CollectionEventArgs<T>> ItemChanged;

        /// <summary>
        /// Occurs when a new item is inserted.
        /// </summary>
        public event EventHandler<CollectionEventArgs<T>> ItemInserted;

        /// <summary>
        /// Occurs when a item is removed.
        /// </summary>
        public event EventHandler<CollectionEventArgs<T>> ItemRemoved;

        #endregion Events

        #region Properties

        #region Explicit Interface Properties

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (T)value;
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        /// <value>The number of items in the collection.</value>
        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is readonly.
        /// </summary>
        /// <value><c>true</c> if readonly; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get
            {
                return this.Items.IsReadOnly;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the list of items.
        /// </summary>
        /// <value>The list of items.</value>
        protected IList<T> Items
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets or sets an item with the specified name.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public virtual T this[string name]
        {
            get
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.Items[i].Name == name)
                    {
                        return this.Items[i];
                    }
                }

                return default(T);
            }

            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.Items[i].Name == name)
                    {
                        this[i] = value;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets an item by index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public virtual T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                this.Items[index] = value;
                if (this.ItemChanged != null)
                {
                    this.ItemChanged(this, new CollectionEventArgs<T>(value));
                }
            }
        }

        #endregion Indexers

        #region Methods

        #region Explicit Interface Methods

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((T[])array, index);
        }

        /// <summary>
        /// Returns the enumerator of the collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        int IList.Add(object value)
        {
            this.Add((T)value);
            return this.Count;
        }

        bool IList.Contains(object value)
        {
            return this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Adds a item to the collection.
        /// </summary>
        /// <param name="item">The new item.</param>
        public virtual void Add(T item)
        {
            this.Items.Add(item);

            if (this.ItemAdded != null)
            {
                this.ItemAdded(this, new CollectionEventArgs<T>(item));
            }
        }

        /// <summary>
        /// Adds a array of items to the collection.
        /// </summary>
        /// <param name="items">The array of items.</param>
        public void AddRange(params T[] items)
        {
            foreach (T t in items)
            {
                this.Add(t);
            }
        }

        /// <summary>
        /// This method changes the position of an existing item in the list.
        /// </summary>
        /// <param name="newIndex">The zero-based index to where the item should be moved.</param>
        /// <param name="item">The object to move.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>newIndex</c> is out of range.</exception>
        /// <exception cref="ArgumentException">item is not in the list</exception>
        public void ChangeIndex(int newIndex, T item)
        {
            int currentIndex = this.Items.IndexOf(item);

            if (newIndex < 0 || newIndex > this.Items.Count)
            {
                throw new ArgumentOutOfRangeException("newIndex", "index must be between 0 and the number of elements in the list");
            }

            if (currentIndex == -1)
            {
                throw new ArgumentException("item is not in the list", "item");
            }

            if (currentIndex == newIndex)
            {
                return;
            }

            this.RemoveAt(currentIndex);
            this.Insert(newIndex, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            while (this.Items.Count > 0)
            {
                this.Remove(this.Items[0]);
            }
        }

        /// <summary>
        /// Determines whether the collection contains an item.
        /// </summary>
        /// <param name="item">The item to find.</param>
        /// <returns><c>true</c> if found; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        /// <summary>
        /// Copies the collection to an array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The starting index.</param>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>arrayIndex</c> is out of range.</exception>
        /// <exception cref="ArgumentException">Array is multi-dimensional</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (ReferenceEquals(array, null))
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "Index is out of range");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("Array is multi-dimensional", "array");
            }

            foreach (object o in this)
            {
                array.SetValue(o, arrayIndex);
                arrayIndex++;
            }
        }

        /// <summary>
        /// Executes an action for each item in the collection.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public void ForEach(Action<T> action)
        {
            int count = this.Items.Count;
            for (int i = 0; i < count; i++)
            {
                action(this.Items[i]);
            }
        }

        /// <summary>
        /// Returns the enumerator of the collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that goes from the last to the first element.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerable<T> GetReverseEnumerator()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                yield return this.Items[i];
            }
        }

        /// <summary>
        /// Returns the index of the specified item.
        /// </summary>
        /// <param name="item">The object to locate in the MiyagiCollection.</param>
        /// <returns>The zero-based index of the item.</returns>
        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index at which item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public virtual void Insert(int index, T item)
        {
            this.Items.Insert(index, item);

            if (this.ItemInserted != null)
            {
                this.ItemInserted(this, new CollectionEventArgs<T>(item));
            }
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item has been removed sucessfully; otherwise, <c>false</c>.</returns>
        public virtual bool Remove(T item)
        {
            bool removed = this.Items.Remove(item);

            if (removed)
            {
                if (this.ItemRemoved != null)
                {
                    this.ItemRemoved(this, new CollectionEventArgs<T>(item));
                }
            }

            return removed;
        }

        /// <summary>
        /// Removes an item at the specific index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public virtual void RemoveAt(int index)
        {
            T item = this[index];
            this.Remove(item);
        }

        #endregion Public Methods

        #endregion Methods
    }
}