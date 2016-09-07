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
namespace Miyagi.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// The abstract base class of controls which display a list.
    /// </summary>
    public abstract class ListControl : SkinnedControl, IListElementOwner
    {
        #region Fields

        private ListItem hoveredItem;
        private ListItemCollection items;
        private ListElement listElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListControl class.
        /// </summary>
        /// <param name="name">The name of the ListControl.</param>
        protected ListControl(string name)
            : base(name)
        {
            this.Items = new ListItemCollection();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="HoveredIndex"/> property is changed.
        /// </summary>
        public event EventHandler HoveredIndexChanged;

        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        /// <remarks>Submits the added item.</remarks>
        public event EventHandler<ValueEventArgs<ListItem>> ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        /// <remarks>Submits the removed item.</remarks>
        public event EventHandler<ValueEventArgs<ListItem>> ItemRemoved;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property is changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndicies"/> property is changed.
        /// </summary>
        public event EventHandler SelectedIndiciesChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the index over which the mouse cursor currently hovers.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public int HoveredIndex
        {
            get
            {
                return this.ListElement.HoveredIndex;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ListElement.HoveredIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of items.
        /// </summary>
        public ListItemCollection Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.ThrowIfDisposed();

                if (this.items != value)
                {
                    this.SetItems(value);
                    this.ListElement.ItemsDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the items should be cropped.
        /// </summary>
        public abstract bool ListBoxCroppingDisabled
        {
            get;
        }

        /// <summary>
        /// Gets the total height available for items.
        /// </summary>
        public abstract int ListBoxHeight
        {
            get;
        }

        /// <summary>
        /// Gets the fixed vertical offset.
        /// </summary>
        public abstract int ListBoxVerticalOffset
        {
            get;
        }

        /// <summary>
        /// Gets or sets the style of the ListBoxElement.
        /// </summary>
        public ListStyle ListStyle
        {
            get
            {
                return this.ListElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ListElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        /// <value>The index of the selected item. Returns -1 if no item is selected.</value>
        [SerializerOptions(Ignore = true)]
        public int SelectedIndex
        {
            get
            {
                return this.ListElement.SelectedIndex;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ListElement.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Gets the selected indicies.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public IEnumerable<int> SelectedIndicies
        {
            get
            {
                return this.ListElement.SelectedIndicies;
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public ListItem SelectedItem
        {
            get
            {
                return this.ListElement.SelectedItem;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ListElement.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        [SerializerOptions(Ignore = true)]
        public IEnumerable<ListItem> SelectedItems
        {
            get
            {
                return this.ListElement.SelectedItems;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar should be displayed.
        /// </summary>
        public abstract bool ShouldShowScrollBar
        {
            get;
        }

        /// <summary>
        /// Gets or sets the index of the first item in the list.
        /// </summary>
        /// <value>The index of the first item.</value>
        public int TopItemIndex
        {
            get
            {
                return this.ListElement.TopItemIndex;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ListElement.TopItemIndex = value;
            }
        }

        /// <summary>
        /// Gets the number of visible items.
        /// </summary>
        public int VisibleItemCount
        {
            get
            {
                return this.ListElement.VisibleItemCount;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets a value indicating whether arrow key movement is blocked.
        /// </summary>
        /// <value>
        /// <c>true</c> if arrow key movement is blocked; otherwise, <c>false</c>.
        /// </value>
        protected internal override bool IsArrowKeyMovementBlocked
        {
            get
            {
                return this.DefaultKeysEnabled;
            }
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        /// <value>A list of elements.</value>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;

                if (this.listElement != null)
                {
                    retValue.Add(this.listElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the ListBoxElement.
        /// </summary>
        protected ListElement ListElement
        {
            get
            {
                if (this.listElement == null)
                {
                    this.listElement = new ListElement(this, () => this.ZOrder * 10);
                    this.listElement.HoveredIndexChanged += this.ListHoveredIndexChanged;
                    this.listElement.SelectedIndexChanged += this.ListSelectedIndexChanged;
                    this.listElement.SelectedIndiciesChanged += this.ListSelectedIndiciesChanged;
                }

                return this.listElement;
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Deselects all selected items.
        /// </summary>
        public void ClearSelected()
        {
            this.ListElement.ClearSelected(true);
        }

        /// <summary>
        /// Deselects all specified items.
        /// </summary>
        /// <param name="items">The items to deselect.</param>
        public void DeselectItems(params ListItem[] items)
        {
            this.ListElement.DeselectItems(items);
        }

        /// <summary>
        /// Selects all specified items.
        /// </summary>
        /// <param name="items">The items to select.</param>
        public void SelectItems(params ListItem[] items)
        {
            this.ListElement.SelectItems(items);
        }

        /// <summary>
        /// Selects all specified items.
        /// </summary>
        /// <param name="selectedItems">The items to select.</param>
        public void SelectItems(params string[] selectedItems)
        {
            var selectedListBoxItems =
                (from item in this.items
                 where selectedItems.Contains(item.Text)
                 select item).ToArray();

            this.SelectItems(selectedListBoxItems);
        }

        public void SelectOnlyTheseItems(params string[] selectedItems)
        {
            var selectedListBoxItems =
                (from item in this.items
                 where selectedItems.Contains(item.Text)
                 select item).ToArray();

            if (this.SelectedItems != null)
            {
                var deselectedListBoxItems =
                    (from item in this.SelectedItems
                     where !selectedItems.Contains(item.Text)
                     select item).ToArray();

                this.DeselectItems(deselectedListBoxItems);
            }

            this.SelectItems(selectedListBoxItems);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.SetItems(null);

                if (this.listElement != null)
                {
                    this.listElement.HoveredIndexChanged -= this.ListHoveredIndexChanged;
                    this.listElement.SelectedIndexChanged -= this.ListSelectedIndexChanged;
                    this.listElement.SelectedIndiciesChanged -= this.ListSelectedIndiciesChanged;
                    this.listElement = null;
                }
            }
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            this.TopItemIndex = 0;
            this.ListElement.ItemsDirty = true;
            this.ListStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Raises the <see cref="ListControl.HoveredIndexChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnHoveredIndexChanged(EventArgs e)
        {
            if (this.HoveredIndexChanged != null)
            {
                this.HoveredIndexChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyDown(KeyboardEventArgs e)
        {
            this.ListElement.InjectKeyDown(e);
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyUp(KeyboardEventArgs e)
        {
            this.ListElement.InjectKeyUp(e);
            base.OnKeyUp(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (this.ListElement.ScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            base.OnMouseClick(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (this.ListElement.ScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.ListElement.ScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            this.ListElement.InjectMouseDown(e);
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            this.ListElement.InjectMouseDrag(e);
            base.OnMouseDrag(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            this.ListElement.InjectMouseHeld(e);
            base.OnMouseHeld(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            this.ListElement.InjectMouseHover(e);
            base.OnMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.ListElement.InjectMouseLeave(e);
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.ListElement.ScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            this.ListElement.InjectMouseUp(e);
            base.OnMouseUp(e);
            this.ListElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseWheelMoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected override bool OnMouseWheelMoved(ValueEventArgs<int> e)
        {
            this.ListElement.InjectMouseWheelMoved(e);
            base.OnMouseWheelMoved(e);
            return false;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.visible)
            {
                HoveredIndex = -1;
            }
        }

        public override bool HasMouseWheelHandlers
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Raises the <see cref="ListControl.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ListControl.SelectedIndiciesChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedIndiciesChanged(EventArgs e)
        {
            if (this.SelectedIndiciesChanged != null)
            {
                this.SelectedIndiciesChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.Items != null)
            {
                this.ListElement.ItemsDirty = true;
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            foreach (var item in this.items)
            {
                item.Update();
            }

            this.ListElement.Update(this.DeltaLocation, this.DeltaSize);
        }

        #endregion Protected Methods

        #region Private Methods

        private void ListHoveredIndexChanged(object sender, EventArgs e)
        {
            this.SetHoveredItem();
            this.OnHoveredIndexChanged(e);
            this.OnPropertyChanged("HoveredIndex");
        }

        private void ListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndexChanged(e);
            this.OnPropertyChanged("SelectedIndex");
        }

        private void ListSelectedIndiciesChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndiciesChanged(e);
            this.OnPropertyChanged("SelectedIndicies");
        }

        private void OnItemAdded(object sender, CollectionEventArgs<ListItem> e)
        {
            e.Item.Owner = this;

            if (this.ItemAdded != null)
            {
                this.ItemAdded(this, new ValueEventArgs<ListItem>(e.Item));
            }

            this.ListElement.ItemsDirty = true;
        }

        private void OnItemChanged(object sender, CollectionEventArgs<ListItem> e)
        {
            ListItem item = e.Item;
            item.Owner = this;
            int index = this.Items.IndexOf(item);
            if (index - this.TopItemIndex >= 0 && index - this.TopItemIndex < this.VisibleItemCount)
            {
                this.ListElement.ItemsDirty = true;
            }
        }

        private void OnItemRemoved(object sender, CollectionEventArgs<ListItem> e)
        {
            ListItem item = e.Item;

            if (this.ItemRemoved != null)
            {
                this.ItemRemoved(this, new ValueEventArgs<ListItem>(e.Item));
            }

            item.Dispose();

            if (this.VisibleItemCount > 0)
            {
                if (this.TopItemIndex + this.VisibleItemCount > this.Items.Count)
                {
                    this.TopItemIndex = this.Items.Count - this.VisibleItemCount;
                }
            }

            this.ListElement.ItemsDirty = true;
        }

        private void SetHoveredItem()
        {
            ListItem newHoveredItem = null;
            if (this.HoveredIndex != -1)
            {
                newHoveredItem = this.items[this.HoveredIndex];
                newHoveredItem.IsMouseOver = true;
            }

            if (this.hoveredItem != null && newHoveredItem != this.hoveredItem)
            {
                this.hoveredItem.IsMouseOver = false;
            }

            this.hoveredItem = newHoveredItem;
        }

        private void SetItems(ListItemCollection newItems)
        {
            if (this.items != null)
            {
                this.items.ItemAdded -= this.OnItemAdded;
                this.items.ItemRemoved -= this.OnItemRemoved;
                this.items.ItemChanged -= this.OnItemChanged;

                foreach (ListItem lbi in this.items)
                {
                    lbi.Dispose();
                }

                this.items.Clear();
            }

            this.items = newItems;

            if (this.items != null)
            {
                this.items.ItemAdded += this.OnItemAdded;
                this.items.ItemRemoved += this.OnItemRemoved;
                this.items.ItemChanged += this.OnItemChanged;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}