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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// Base class for scrollable controls.
    /// </summary>
    public abstract class ScrollableControl : SkinnedControl
    {
        #region Fields

        private readonly HorizontalScrollBarController horizontalScrollController;
        private readonly VerticalScrollBarController verticalScrollController;

        private ScrollBarElement horiScrollBarElement;
        private int lastHScrollBarElementValue;
        private int lastVScrollBarElementValue;
        private TextureElement scrollBarCorner;
        private bool scrollBarCornerVisible;
        private int scrollOffsetBottom;
        private int scrollOffsetLeft;
        private int scrollOffsetRight;
        private int scrollOffsetTop;
        private ScrollBarElement vertScrollBarElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScrollableControl class.
        /// </summary>
        /// <param name="name">The name of the ScrollableControl.</param>
        protected ScrollableControl(string name)
            : base(name)
        {
            this.verticalScrollController = new VerticalScrollBarController(this);
            this.horizontalScrollController = new HorizontalScrollBarController(this);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the style of the horizontal scrollbar.
        /// </summary>
        public ScrollBarStyle HScrollBarStyle
        {
            get
            {
                return this.HScrollBarElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.HScrollBarElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the current scroll offset.
        /// </summary>
        public Point Scroll
        {
            get
            {
                return new Point(this.HScrollBarElement.Value, this.VScrollBarElement.Value);
            }

            set
            {
                this.HScrollBarElement.Value = value.X;
                this.VScrollBarElement.Value = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the style of the vertical scrollbar.
        /// </summary>
        public ScrollBarStyle VScrollBarStyle
        {
            get
            {
                return this.VScrollBarElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.VScrollBarElement.Style = value;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a <see cref="Point"/> representing the offset that is applied to added controls.
        /// </summary>
        protected override Point ChildOffset
        {
            get
            {
                return base.ChildOffset - new Point(this.lastHScrollBarElementValue, this.lastVScrollBarElementValue);
            }
        }

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        /// <value>A list of elements.</value>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.vertScrollBarElement != null)
                {
                    retValue.Add(this.vertScrollBarElement);
                }

                if (this.horiScrollBarElement != null)
                {
                    retValue.Add(this.horiScrollBarElement);
                }

                if (this.scrollBarCorner != null)
                {
                    retValue.Add(this.scrollBarCorner);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the horizontal scrollbar.
        /// </summary>
        /// <value>A ScrollBarElement representing the scrollbar.</value>
        protected ScrollBarElement HScrollBarElement
        {
            get
            {
                if (this.horiScrollBarElement == null)
                {
                    this.horiScrollBarElement = new ScrollBarElement(
                        this,
                        () => (this.ZOrder * 10) + 5,
                        this.horizontalScrollController)
                                                {
                                                    Orientation = Orientation.Horizontal,
                                                };

                    this.horiScrollBarElement.ValueChanged += this.OnScrollBarValueChanged;
                    this.horiScrollBarElement.VisibleChanged += this.OnScrollBarVisibleChanged;
                }

                return this.horiScrollBarElement;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scrolbars needs to be updated.
        /// </summary>
        protected bool ScrollBarsNeedUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the vertival scrollbar.
        /// </summary>
        /// <value>A ScrollBarElement representing the scrollbar.</value>
        protected ScrollBarElement VScrollBarElement
        {
            get
            {
                if (this.vertScrollBarElement == null)
                {
                    this.vertScrollBarElement = new ScrollBarElement(
                        this,
                        () => (this.ZOrder * 10) + 5,
                        this.verticalScrollController)
                                                {
                                                    Orientation = Orientation.Vertical,
                                                };

                    this.vertScrollBarElement.ValueChanged += this.OnScrollBarValueChanged;
                    this.vertScrollBarElement.VisibleChanged += this.OnScrollBarVisibleChanged;
                }

                return this.vertScrollBarElement;
            }
        }

        #endregion Protected Properties

        #region Private Properties

        private TextureElement ScrollBarCorner
        {
            get
            {
                return this.scrollBarCorner ?? (this.scrollBarCorner = new TextureElement(this, () => this.VScrollBarElement.GetZOrder())
                                                                       {
                                                                           ResizeDisabled = true,
                                                                           Texture = this.Skin.SubSkins[this.Skin.Name + ".ScrollBarCorner"],
                                                                       });
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Scrolls vertically to the end.
        /// </summary>
        public void ScrollToBottom()
        {
            this.UpdateScrollBars();
            this.Scroll = new Point(this.HScrollBarElement.Value, this.VScrollBarElement.Max);
        }

        /// <summary>
        /// Scrolls horizontally to the beginning.
        /// </summary>
        public void ScrollToLeft()
        {
            this.UpdateScrollBars();
            this.Scroll = new Point(0, this.VScrollBarElement.Value);
        }

        /// <summary>
        /// Scrolls horizontally to the end.
        /// </summary>
        public void ScrollToRight()
        {
            this.UpdateScrollBars();
            this.Scroll = new Point(this.HScrollBarElement.Max, this.VScrollBarElement.Value);
        }

        /// <summary>
        /// Scrolls vertically to the beginning.
        /// </summary>
        public void ScrollToTop()
        {
            this.UpdateScrollBars();
            this.Scroll = new Point(this.HScrollBarElement.Value, 0);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Calculates a client area based on the size of a control.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        /// <returns>A size representing said client area.</returns>
        protected override Size ClientSizeFromSize(Size size)
        {
            var retValue = base.ClientSizeFromSize(size);
            if (this.VScrollBarElement.Visible)
            {
                retValue = new Size(retValue.Width - this.VScrollBarStyle.Extent, retValue.Height);
            }

            if (this.HScrollBarElement.Visible)
            {
                retValue = new Size(retValue.Width, retValue.Height - this.HScrollBarStyle.Extent);
            }

            return retValue;
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.VScrollBarElement.ValueChanged -= this.OnScrollBarValueChanged;
                this.VScrollBarElement.VisibleChanged -= this.OnScrollBarVisibleChanged;
                this.vertScrollBarElement = null;

                this.HScrollBarElement.ValueChanged -= this.OnScrollBarValueChanged;
                this.HScrollBarElement.VisibleChanged -= this.OnScrollBarVisibleChanged;
                this.horiScrollBarElement = null;

                if (this.scrollBarCorner != null)
                {
                    this.scrollBarCorner.Dispose();
                    this.scrollBarCorner = null;
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

            this.HScrollBarStyle.Resize(widthFactor, heightFactor);
            this.VScrollBarStyle.Resize(widthFactor, heightFactor);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Handles child location changes.
        /// </summary>
        /// <param name="child">The child control which location has been changed.</param>
        protected override void OnChildLocationChanged(Control child)
        {
            base.OnChildLocationChanged(child);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Handles child size changes.
        /// </summary>
        /// <param name="child">The child control which size has been changed.</param>
        protected override void OnChildSizeChanged(Control child)
        {
            base.OnChildSizeChanged(child);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Handles child visibility changes.
        /// </summary>
        /// <param name="child">The child control which visibility has been changed.</param>
        protected override void OnChildVisibleChanged(Control child)
        {
            base.OnChildVisibleChanged(child);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Raises the <see cref="Control.ClientSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnControlAdded(ValueEventArgs<Control> e)
        {
            base.OnControlAdded(e);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlRemoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnControlRemoved(ValueEventArgs<Control> e)
        {
            base.OnControlRemoved(e);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (this.HScrollBarElement.IsMouseOver || this.VScrollBarElement.IsMouseOver)
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
            if (this.HScrollBarElement.IsMouseOver || this.VScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.HScrollBarElement.IsMouseOver || this.VScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            base.OnMouseDown(e);
            this.VScrollBarElement.InjectMouseDown(e);
            this.HScrollBarElement.InjectMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            base.OnMouseDrag(e);
            this.VScrollBarElement.InjectMouseDrag(e);
            this.HScrollBarElement.InjectMouseDrag(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            base.OnMouseHeld(e);
            this.VScrollBarElement.InjectMouseHeld(e);
            this.HScrollBarElement.InjectMouseHeld(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.VScrollBarElement.InjectMouseHover(e);
            this.HScrollBarElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.HScrollBarElement.IsMouseOver || this.VScrollBarElement.IsMouseOver)
            {
                e.CancelEvent = true;
            }

            base.OnMouseUp(e);
            this.VScrollBarElement.InjectMouseUp(e);
            this.HScrollBarElement.InjectMouseUp(e);
            this.VScrollBarElement.InjectMouseHover(e);
            this.HScrollBarElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);
            this.ScrollBarsNeedUpdate = true;
        }

        /// <summary>
        /// Calculates the size of the control based on the size of the client area.
        /// </summary>
        /// <param name="size">The size of the client area.</param>
        /// <returns>A size for the whole control.</returns>
        protected override Size SizeFromClientSize(Size size)
        {
            Size retValue = base.SizeFromClientSize(size);
            if (this.VScrollBarElement.Visible)
            {
                retValue = new Size(retValue.Width + this.VScrollBarStyle.Extent, retValue.Height);
            }

            if (this.HScrollBarElement.Visible)
            {
                retValue = new Size(retValue.Width, retValue.Height + this.HScrollBarStyle.Extent);
            }

            return retValue;
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();

            if (this.ScrollBarsNeedUpdate)
            {
                this.UpdateScrollBars();
                this.SetupScrollBarCorner();
                this.ScrollBarsNeedUpdate = false;
            }

            this.VScrollBarElement.Update(this.DeltaLocation, this.DeltaSize);
            this.HScrollBarElement.Update(this.DeltaLocation, this.DeltaSize);

            if (this.scrollBarCornerVisible)
            {
                this.ScrollBarCorner.Update(this.DeltaLocation, this.DeltaSize);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void MoveChildren(Point diff)
        {
            this.SuspendLayout();
            foreach (var child in this.Controls)
            {
                child.Top += diff.Y;
                child.Left += diff.X;
            }

            this.ResumeLayout();
        }

        private void OnScrollBarValueChanged(object sender, EventArgs e)
        {
            if (sender == this.HScrollBarElement)
            {
                int diff = this.lastHScrollBarElementValue - this.HScrollBarElement.Value;
                if (diff != 0)
                {
                    if (this.HScrollBarElement.Max > 0)
                    {
                        this.lastHScrollBarElementValue = this.HScrollBarElement.Value;
                        this.MoveChildren(new Point(diff, 0));
                    }
                }
            }
            else if (sender == this.VScrollBarElement)
            {
                int diff = this.lastVScrollBarElementValue - this.VScrollBarElement.Value;
                if (diff != 0)
                {
                    if (this.VScrollBarElement.Max > 0)
                    {
                        this.lastVScrollBarElementValue = this.VScrollBarElement.Value;
                        this.MoveChildren(new Point(0, diff));
                    }
                }
            }
        }

        private void OnScrollBarVisibleChanged(object sender, EventArgs e)
        {
            if (this.VScrollBarElement.Visible
                && this.HScrollBarElement.Visible
                && !this.scrollBarCornerVisible)
            {
                this.scrollBarCornerVisible = true;
                this.SetupScrollBarCorner();
                this.ScrollBarsNeedUpdate = true;
            }
            else
            {
                this.scrollBarCornerVisible = false;
                if (this.scrollBarCorner != null)
                {
                    this.scrollBarCorner.Dispose();
                    this.scrollBarCorner = null;
                }
            }

            this.UpdateClientSize();
            this.PerformLayout();
        }

        private void SetupScrollBarCorner()
        {
            if (this.scrollBarCornerVisible)
            {
                this.ScrollBarCorner.Size = new Size(this.VScrollBarStyle.Extent, this.HScrollBarStyle.Extent);
                this.ScrollBarCorner.Offset = new Point(
                    this.verticalScrollController.GetLocation().X,
                    this.horizontalScrollController.GetLocation().Y);
            }
        }

        private void UpdateScrollBars()
        {
            // get the scrollbar max values
            var rect = new Rectangle(Point.Empty, this.DisplayRectangle.Size);
            int count = this.Controls.Count;
            int excLeft = 0, excTop = 0, excRight = 0, excBottom = 0;

            for (int i = 0; i < count; i++)
            {
                Control control = this.Controls[i];
                if (control.Visible)
                {
                    // vertical scrollbar
                    if (control.Left < rect.Left)
                    {
                        int newLeft = -control.Left;
                        excLeft = excLeft < newLeft ? newLeft : excLeft;
                    }

                    if (control.Right > rect.Right)
                    {
                        int newRight = control.Right - rect.Width;
                        excRight = excRight < newRight ? newRight : excRight;
                    }

                    // horizontal scrollbar
                    if (control.Top < rect.Top)
                    {
                        int newTop = -control.Top;
                        excTop = excTop < newTop ? newTop : excTop;
                    }

                    if (control.Bottom > rect.Bottom)
                    {
                        int newBottom = control.Bottom - rect.Height;
                        excBottom = excBottom < newBottom ? newBottom : excBottom;
                    }
                }
            }

            if (excLeft != this.scrollOffsetLeft || excRight != this.scrollOffsetRight)
            {
                this.scrollOffsetLeft = excLeft;
                this.scrollOffsetRight = excRight;
            }

            if (excTop != this.scrollOffsetTop || excBottom != this.scrollOffsetBottom)
            {
                this.scrollOffsetTop = excTop;
                this.scrollOffsetBottom = excBottom;
            }

            this.VScrollBarElement.Value = this.lastVScrollBarElementValue = this.scrollOffsetTop;
            this.VScrollBarElement.ThumbSizeDirty = true;

            this.HScrollBarElement.Value = this.lastHScrollBarElementValue = this.scrollOffsetLeft;
            this.HScrollBarElement.ThumbSizeDirty = true;
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private sealed class HorizontalScrollBarController : IScrollBarElementController
        {
            #region Fields

            private readonly ScrollableControl parent;

            #endregion Fields

            #region Constructors

            public HorizontalScrollBarController(ScrollableControl parent)
            {
                this.parent = parent;
            }

            #endregion Constructors

            #region Methods

            #region Public Methods

            public int GetAutoExtent()
            {
                int retValue = this.parent.Width;

                if (this.parent.HasBorder)
                {
                    retValue -= this.parent.BorderStyle.Thickness.Horizontal;
                }

                if (this.parent.VScrollBarElement.Visible)
                {
                    retValue -= this.parent.VScrollBarStyle.Extent;
                }

                return retValue;
            }

            public Point GetLocation()
            {
                var retValue = new Point(0, this.parent.Height - this.parent.HScrollBarStyle.Extent);

                if (this.parent.HasBorder)
                {
                    retValue = Point.Add(
                        retValue,
                        this.parent.BorderStyle.Thickness.Left,
                        -this.parent.BorderStyle.Thickness.Bottom);
                }

                return retValue;
            }

            public bool GetShouldShow()
            {
                return this.parent.scrollOffsetLeft + this.parent.scrollOffsetRight > 0;
            }

            public int GetTotalUnitsCount()
            {
                int size = this.GetAutoExtent();
                return this.parent.scrollOffsetLeft + this.parent.scrollOffsetRight + size;
            }

            public int GetVisibleUnitsCount()
            {
                return this.GetAutoExtent();
            }

            public int GetVisibleUnitsMax()
            {
                return this.GetAutoExtent();
            }

            #endregion Public Methods

            #endregion Methods
        }

        private sealed class VerticalScrollBarController : IScrollBarElementController
        {
            #region Fields

            private readonly ScrollableControl parent;

            #endregion Fields

            #region Constructors

            public VerticalScrollBarController(ScrollableControl parent)
            {
                this.parent = parent;
            }

            #endregion Constructors

            #region Methods

            #region Public Methods

            public int GetAutoExtent()
            {
                int retValue = this.parent.Height;

                if (this.parent.HasBorder)
                {
                    retValue -= this.parent.BorderStyle.Thickness.Vertical;
                }

                if (this.parent.HScrollBarElement.Visible)
                {
                    retValue -= this.parent.HScrollBarStyle.Extent;
                }

                return retValue;
            }

            public Point GetLocation()
            {
                Point retValue = new Point(this.parent.Width - this.parent.VScrollBarStyle.Extent, 0);

                if (this.parent.HasBorder)
                {
                    retValue = Point.Add(
                        retValue,
                        -this.parent.BorderStyle.Thickness.Right,
                        this.parent.BorderStyle.Thickness.Top);
                }

                return retValue;
            }

            public bool GetShouldShow()
            {
                return this.parent.scrollOffsetTop + this.parent.scrollOffsetBottom > 0;
            }

            public int GetTotalUnitsCount()
            {
                int size = this.GetAutoExtent();
                return this.parent.scrollOffsetBottom + this.parent.scrollOffsetTop + size;
            }

            public int GetVisibleUnitsCount()
            {
                return this.GetAutoExtent();
            }

            public int GetVisibleUnitsMax()
            {
                return this.GetAutoExtent();
            }

            #endregion Public Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}