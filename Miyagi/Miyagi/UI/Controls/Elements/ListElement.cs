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
namespace Miyagi.UI.Controls.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a ListBox.
    /// </summary>
    public sealed class ListElement : Element<IListElementOwner, ListStyle>, IInteractiveElementOwner
    {
        #region Fields

        private int hoveredIndex = -1;
        private bool isCtrlDown;
        private bool isShiftDown;
        private ScrollBarElement scrollBarElement;
        private int shiftStartItemIndex = -1;
        private ListItemCollection visibleItems;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListBoxElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public ListElement(IListElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.ItemsDirty = true;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="HoveredIndex"/> property is changed.
        /// </summary>
        public event EventHandler HoveredIndexChanged;

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
        /// Gets or sets a value indicating whether the colours of the items need to be updated.
        /// </summary>
        public bool ColoursDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the combined skin name.
        /// </summary>
        public string CombinedSkinName
        {
            get
            {
                return this.Owner.CombinedSkinName;
            }
        }

        /// <summary>
        /// Gets or sets the index over which the mouse cursor currently hovers.
        /// </summary>
        public int HoveredIndex
        {
            get
            {
                return this.hoveredIndex;
            }

            set
            {
                if (this.hoveredIndex != value)
                {
                    this.hoveredIndex = value;
                    this.ColoursDirty = true;

                    if (this.HoveredIndexChanged != null)
                    {
                        this.HoveredIndexChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the items need to be updated.
        /// </summary>
        public bool ItemsDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the scrollbar.
        /// </summary>
        /// <value>A ScrollBarElement representing the scrollbar.</value>
        public ScrollBarElement ScrollBarElement
        {
            get
            {
                if (this.scrollBarElement == null)
                {
                    this.scrollBarElement = new ScrollBarElement(
                        this,
                        () => this.GetZOrder() + 5,
                        new ScrollBarController(this));

                    this.scrollBarElement.ValueChanged += this.ScrollBarValueChanged;
                }

                return this.scrollBarElement;
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                // return the index of the first selected item
                for (int i = 0; i < this.Owner.Items.Count; i++)
                {
                    if (this.Owner.Items[i].Selected)
                    {
                        return i;
                    }
                }

                return -1;
            }

            set
            {
                if (this.SelectedIndex != value)
                {
                    this.HoveredIndex = -1;

                    if (value == -1)
                    {
                        this.ClearSelected(true);
                    }

                    if (value >= this.Owner.Items.Count || value < 0)
                    {
                        return;
                    }

                    this.SelectItems(this.Owner.Items[value]);
                }
            }
        }

        /// <summary>
        /// Gets the selected indicies.
        /// </summary>
        public IEnumerable<int> SelectedIndicies
        {
            get
            {
                IEnumerable<ListItem> selectedItems = this.SelectedItems;
                return selectedItems == null
                           ? null
                           : selectedItems.Select(item => this.Owner.Items.IndexOf(item)).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public ListItem SelectedItem
        {
            get
            {
                int selIndex = this.SelectedIndex;
                return selIndex != -1 ? this.Owner.Items[selIndex] : null;
            }

            set
            {
                if (this.SelectedItem != value)
                {
                    foreach (ListItem item in this.Owner.Items)
                    {
                        if (item == value)
                        {
                            this.SelectItems(item);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public IEnumerable<ListItem> SelectedItems
        {
            get
            {
                List<ListItem> retValue = null;

                foreach (ListItem item in this.Owner.Items)
                {
                    if (item.Selected)
                    {
                        if (retValue == null)
                        {
                            retValue = new List<ListItem>();
                        }

                        retValue.Add(item);

                        if (!this.Style.MultiSelect)
                        {
                            break;
                        }
                    }
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the skin.
        /// </summary>
        /// <value>The skin.</value>
        public Skin Skin
        {
            get
            {
                return this.Owner.Skin;
            }
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        public override IEnumerable<IElement> SubElements
        {
            get
            {
                if (this.visibleItems != null)
                {
                    int count = this.visibleItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        yield return this.visibleItems[i].ListItemElement;
                    }
                }

                if (this.scrollBarElement != null)
                {
                    yield return this.scrollBarElement;
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the first item in the list.
        /// </summary>
        public int TopItemIndex
        {
            get
            {
                return this.ScrollBarElement.Value;
            }

            set
            {
                if (value > this.Owner.Items.Count - this.VisibleItemCount)
                {
                    value = this.Owner.Items.Count - this.VisibleItemCount;
                }

                if (value < 0)
                {
                    value = 0;
                }

                if (this.ScrollBarElement.Value != value)
                {
                    this.ScrollBarElement.Value = value;
                    this.HoveredIndex = -1;
                    this.ItemsDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the number of visible items.
        /// </summary>
        public int VisibleItemCount
        {
            get
            {
                return this.visibleItems != null ? this.visibleItems.Count : 0;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull()
                   && this.scrollBarElement.AreAllSpritesNull()
                   && this.VisibleItemCount == 0;
        }

        /// <summary>
        /// Deselects all selected items.
        /// </summary>
        /// <param name="raise">Indicates whether events should be raised.</param>
        public void ClearSelected(bool raise)
        {
            if (this.SelectedIndex == -1)
            {
                return;
            }

            for (int i = 0; i < this.Owner.Items.Count; i++)
            {
                this.Owner.Items[i].SetSelected(false);
            }

            this.ColoursDirty = true;

            if (raise)
            {
                this.RaiseSelectedIndexChanged();
                this.RaiseSelectedIndiciesChanged();
            }
        }

        /// <summary>
        /// Deselects all specified items.
        /// </summary>
        /// <param name="items">The items to deselect.</param>
        public void DeselectItems(params ListItem[] items)
        {
            int selIndex = this.SelectedIndex;

            foreach (ListItem item in items)
            {
                if (!this.Owner.Items.Contains(item))
                {
                    continue;
                }

                if (item.Selected)
                {
                    item.SetSelected(false);
                }
            }

            if (this.SelectedIndex != selIndex)
            {
                this.RaiseSelectedIndexChanged();
            }

            this.RaiseSelectedIndiciesChanged();
        }

        /// <summary>
        /// Gets the location relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.Owner.GetLocationInViewport();
        }

        /// <summary>
        /// Hides the ListBoxElement.
        /// </summary>
        public void Hide()
        {
            if (this.visibleItems != null)
            {
                this.visibleItems.ForEach(it => it.ListItemElement.RemoveSprite());
                this.visibleItems.Clear();
            }

            this.ScrollBarElement.Hide();
        }

        /// <summary>
        /// Selects all specified items.
        /// </summary>
        /// <param name="items">The items to select.</param>
        public void SelectItems(params ListItem[] items)
        {
            int selIndex = this.SelectedIndex;

            foreach (ListItem item in items)
            {
                if (!this.Owner.Items.Contains(item))
                {
                    continue;
                }

                if (!item.Selected)
                {
                    // deselect other item
                    if (!this.Style.MultiSelect && selIndex != -1)
                    {
                        this.SelectedItem.SetSelected(false);
                    }

                    item.SetSelected(true);
                }
            }

            if (this.SelectedIndex != selIndex)
            {
                this.RaiseSelectedIndexChanged();
            }

            this.RaiseSelectedIndiciesChanged();
        }

        /// <summary>
        /// Shows the ListBoxElement.
        /// </summary>
        public void Show()
        {
            this.ScrollBarElement.Show();
            this.ItemsDirty = true;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.HoveredIndexChanged = null;
            this.SelectedIndexChanged = null;
            this.SelectedIndiciesChanged = null;
        }

        /// <summary>
        /// Handles KeyDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyDown(KeyboardEventArgs e)
        {
            this.isCtrlDown = e.KeyEvent.Modifiers.IsFlagSet(ConsoleModifiers.Control);
            this.isShiftDown = e.KeyEvent.Modifiers.IsFlagSet(ConsoleModifiers.Shift);

            if (this.Owner.DefaultKeysEnabled)
            {
                int newIndex = -1;

                switch (e.KeyCode)
                {
                    case ConsoleKey.DownArrow:
                        newIndex = this.SelectedIndicies.Last() + 1;
                        break;

                    case ConsoleKey.UpArrow:
                        newIndex = this.SelectedIndex - 1;
                        break;

                    case ConsoleKey.Home:
                        newIndex = 0;
                        break;

                    case ConsoleKey.End:
                        newIndex = this.Owner.Items.Count - 1;
                        break;
                }

                if (newIndex > -1 && newIndex < this.Owner.Items.Count)
                {
                    if (!this.isShiftDown)
                    {
                        this.ClearSelected(false);
                    }

                    this.SelectItems(this.Owner.Items[newIndex]);

                    if (this.SelectedIndex < this.TopItemIndex)
                    {
                        this.TopItemIndex = this.SelectedIndex;
                    }
                    else if (this.SelectedIndex > this.TopItemIndex + this.VisibleItemCount - 1)
                    {
                        this.TopItemIndex = this.SelectedIndex - this.VisibleItemCount + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Handles KeyUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyUp(KeyboardEventArgs e)
        {
            this.isCtrlDown = e.KeyEvent.Modifiers.IsFlagSet(ConsoleModifiers.Control);
            this.isShiftDown = e.KeyEvent.Modifiers.IsFlagSet(ConsoleModifiers.Shift);
        }

        /// <summary>
        /// Handles MouseDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.ScrollBarElement.InjectMouseDown(e);

            if (this.hoveredIndex != -1)
            {
                bool handled = false;

                if (!this.isShiftDown)
                {
                    this.shiftStartItemIndex = this.hoveredIndex;
                }

                if (this.Style.MultiSelect && (this.isCtrlDown || this.isShiftDown))
                {
                    if (this.isShiftDown)
                    {
                        if (this.shiftStartItemIndex != -1)
                        {
                            bool doSelect = true;

                            // clear selected items if ctrl isn't pressed
                            if (!this.isCtrlDown)
                            {
                                this.ClearSelected(false);
                            }
                            else
                            {
                                doSelect = this.Owner.Items[this.shiftStartItemIndex].Selected;
                            }

                            int min = Math.Min(this.hoveredIndex, this.shiftStartItemIndex);
                            int count = Math.Max(this.hoveredIndex, this.shiftStartItemIndex) - min + 1;
                            ListItem[] items = this.Owner.Items.Skip(min).Take(count).ToArray();
                            if (doSelect)
                            {
                                this.SelectItems(items);
                            }
                            else
                            {
                                this.DeselectItems(items);
                            }

                            handled = true;
                        }
                    }
                    else if (this.isCtrlDown)
                    {
                        this.Owner.Items[this.hoveredIndex].Selected = !this.Owner.Items[this.hoveredIndex].Selected;
                        handled = true;
                    }
                }

                if (!handled)
                {
                    var selectedIndicies = this.SelectedIndicies;
                    if (selectedIndicies == null || !selectedIndicies.Contains(this.hoveredIndex))
                    {
                        this.ClearSelected(false);
                        this.Owner.Items[this.hoveredIndex].Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles MouseDrag injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            this.ScrollBarElement.InjectMouseDrag(e);
        }

        /// <summary>
        /// Handles MouseHeld injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            this.ScrollBarElement.InjectMouseHeld(e);
        }

        /// <summary>
        /// Handles MouseHover injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            this.ScrollBarElement.InjectMouseHover(e);

            // check if mouse is over an item
            if (this.visibleItems != null && !this.ScrollBarElement.HitTest(e.MouseLocation))
            {
                for (int i = 0; i < this.visibleItems.Count; i++)
                {
                    if (this.ItemHitTest(i, e.MouseLocation) && i + this.TopItemIndex < this.Owner.Items.Count)
                    {
                        this.HoveredIndex = i + this.TopItemIndex;
                        return;
                    }
                }
            }

            if (this.hoveredIndex != -1)
            {
                this.HoveredIndex = -1;
            }
        }

        /// <summary>
        /// Handles MouseLeave injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.HoveredIndex = -1;
            this.ScrollBarElement.InjectMouseLeave(e);
        }

        /// <summary>
        /// Handles MouseUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.ScrollBarElement.InjectMouseUp(e);
        }

        /// <summary>
        /// Inject a moved mouse wheel.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> to inject.</param>
        protected override void OnMouseWheelMoved(ValueEventArgs<int> e)
        {
            if (e.Data < 0)
            {
                this.TopItemIndex++;
            }
            else
            {
                this.TopItemIndex--;
            }
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "MultiSelect":
                    if (!this.Style.MultiSelect)
                    {
                        this.ClearSelected(false);
                    }

                    break;

                case "ScrollBarStyle":
                    this.SetSubElementStyles();
                    break;

                case "HoverColour":
                case "Alignment":
                case "Font":
                case "ForegroundColour":
                case "ItemOffset":
                case "MaxVisibleItems":
                case "SelectColour":
                    this.ItemsDirty = true;
                    break;
            }
        }

        /// <summary>
        /// Sets the style of subelements.
        /// </summary>
        protected override void SetSubElementStyles()
        {
            if (this.Style != null)
            {
                this.ScrollBarElement.Style = this.Style.ScrollBarStyle;
                this.ScrollBarElement.Orientation = Orientation.Vertical;
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            if (this.ItemsDirty)
            {
                this.FillListBox();
            }

            if (this.ColoursDirty)
            {
                this.SetListBoxColours();
            }

            this.ForEachSubElement(ele => ele.Update());
        }

        #endregion Protected Methods

        #region Private Methods

        private void FillListBox()
        {
            if (this.Style.MaxVisibleItems > 0)
            {
                // remove old items
                if (this.visibleItems != null)
                {
                    this.visibleItems.ForEach(it => it.ListItemElement.RemoveSprite());
                    this.visibleItems.Clear();
                }

                this.visibleItems = new ListItemCollection();

                var itemSize = new Size(
                    this.Owner.DisplayRectangle.Width - this.Style.ItemOffset.X - this.Style.ScrollBarStyle.Extent,
                    this.Owner.ListBoxHeight / this.Style.MaxVisibleItems);

                // fill list
                for (int i = this.TopItemIndex; i < this.Style.MaxVisibleItems + this.TopItemIndex && i < this.Owner.Items.Count; i++)
                {
                    ListItem lbi = this.Owner.Items[i];

                    lbi.Size = itemSize;
                    lbi.Location = this.GetItemLocation(i - this.TopItemIndex);

                    lbi.Style.TextStyle.Alignment = this.Style.Alignment;
                    lbi.ListItemElement.TextElement.UpdateType |= UpdateTypes.Text;
                    lbi.ListItemElement.TextureElement.Size = itemSize;

                    lbi.ListItemElement.CroppingDisabled = this.Owner.ListBoxCroppingDisabled;
                    this.visibleItems.Add(lbi);
                }

                this.ColoursDirty = true;
                this.ItemsDirty = false;
            }

            this.ScrollBarElement.ThumbSizeDirty = true;
        }

        /// <summary>
        /// Gets the position of an item by index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The position of the item.</returns>
        private Point GetItemLocation(int index)
        {
            return new Point(
                this.Style.ItemOffset.X,
                this.Style.ItemOffset.Y + this.Owner.ListBoxVerticalOffset + (int)((float)this.Owner.ListBoxHeight / this.Style.MaxVisibleItems * index));
        }

        private bool ItemHitTest(int index, Point loc)
        {
            // make sure visibleItems is up-to-date
            if (this.ItemsDirty)
            {
                this.UpdateCore();
            }

            var lbe = this.visibleItems[index];

            var dr = new Rectangle(lbe.GetLocationInViewport(), lbe.Size);
            return dr.Contains(loc);
        }

        private void RaiseSelectedIndexChanged()
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void RaiseSelectedIndiciesChanged()
        {
            this.ColoursDirty = true;

            if (this.SelectedIndiciesChanged != null)
            {
                this.SelectedIndiciesChanged(this, EventArgs.Empty);
            }
        }

        private void ScrollBarValueChanged(object sender, EventArgs e)
        {
            this.HoveredIndex = -1;
            this.ItemsDirty = true;
        }

        private void SetListBoxColours()
        {
            if (this.visibleItems != null)
            {
                var style = this.Style;
                var font = style.Font;
                var selectedItems = this.SelectedItems;

                for (int i = 0; i < this.visibleItems.Count; i++)
                {
                    ListItem item = this.visibleItems[i];

                    if (item != null)
                    {
                        item.Style.TextStyle.Font = font;

                        if (selectedItems != null && selectedItems.Contains(item))
                        {
                            // selected item
                            item.Style.TextStyle.ForegroundColour = style.SelectColour;
                        }
                        else if (i + this.TopItemIndex == this.hoveredIndex)
                        {
                            // hovered item
                            item.Style.TextStyle.ForegroundColour = style.HoverColour;
                        }
                        else
                        {
                            item.Style.TextStyle.ForegroundColour = style.ForegroundColour;
                        }
                    }
                }
            }

            this.ColoursDirty = false;
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private sealed class ScrollBarController : IScrollBarElementController
        {
            #region Fields

            private readonly ListElement parent;

            #endregion Fields

            #region Constructors

            public ScrollBarController(ListElement parent)
            {
                this.parent = parent;
            }

            #endregion Constructors

            #region Methods

            #region Public Methods

            public int GetAutoExtent()
            {
                return this.parent.Owner.ListBoxHeight;
            }

            public Point GetLocation()
            {
                return new Point(this.parent.Owner.Size.Width - this.parent.Style.ScrollBarStyle.Extent, this.parent.Owner.ListBoxVerticalOffset);
            }

            public bool GetShouldShow()
            {
                return this.parent.Owner.ShouldShowScrollBar;
            }

            public int GetTotalUnitsCount()
            {
                return this.parent.Owner.Items.Count;
            }

            public int GetVisibleUnitsCount()
            {
                return this.parent.VisibleItemCount;
            }

            public int GetVisibleUnitsMax()
            {
                return this.parent.Style.MaxVisibleItems;
            }

            #endregion Public Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}