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
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A Slider control.
    /// </summary>
    public class Slider : SkinnedControl, IThumbElementOwner<float>
    {
        #region Fields

        private bool inverted;
        private float largeChange;
        private float max;
        private float min;
        private float sliderValue;
        private float smallChange;
        private ThumbElement<float> thumbElement;
        private bool thumbHit;
        private float thumbLocation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Slider class.
        /// </summary>
        public Slider()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Slider class.
        /// </summary>
        /// <param name="name">The name of the Slider.</param>
        public Slider(string name)
            : base(name)
        {
            this.Max = 100;
            this.Min = 0;
            this.LargeChange = 5;
            this.SmallChange = 1;
            this.Value = 0;
            this.AutoOrientation = true;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the scroll box has been moved by either a mouse or keyboard action.
        /// </summary>
        public event EventHandler<ScrollEventArgs> Scroll;

        /// <summary>
        /// Occurs when the Value property is changed.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion Events

        #region Properties

        #region Explicit Interface Properties

        Rectangle IThumbElementOwner<float>.ThumbBounds
        {
            get
            {
                return new Rectangle(this.GetLocationInViewport(), this.Size);
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Styles.ThumbStyle.Orientation"/> property is determinated automatically.
        /// </summary>
        public bool AutoOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse clicks on the background set the position of the thumb.
        /// </summary>
        /// <value>If set to true, the thumb doesn't move without being grabbed. Default is false.</value>
        public bool IgnoreBackgroundClicks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Slider is inverted.
        /// </summary>
        /// <value>If true Min is the upmost / rightmost point and Max is the downmost / leftmost point of the Slider.</value>
        public bool Inverted
        {
            get
            {
                return this.inverted;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.inverted != value)
                {
                    if (this.ThumbElement != null)
                    {
                        this.ThumbElement.UpdateType |= UpdateTypes.Size;
                    }

                    this.inverted = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value which is added or subtracted if PageUp or PageDown is pressed.
        /// </summary>
        /// <value>An <see cref="Int32"/> representing the value.</value>
        public float LargeChange
        {
            get
            {
                return this.largeChange;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.largeChange != value)
                {
                    this.largeChange = value;

                    if (this.ThumbElement != null)
                    {
                        this.ThumbElement.CalculateAutoThumbSize();
                    }

                    if (this.smallChange > value)
                    {
                        this.SmallChange = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the Slider.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <remarks>Max will be set to Min if it is set to a value smaller than Min.</remarks>
        public float Max
        {
            get
            {
                return this.max;
            }

            set
            {
                this.ThrowIfDisposed();
                this.max = Math.Max(value, this.min);

                if (this.sliderValue > this.max)
                {
                    this.Value = this.max;
                }

                if (this.ThumbElement != null)
                {
                    this.ThumbElement.CalculateAutoThumbSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the Slider.
        /// </summary>
        /// <value>The minimum value.</value>
        /// <remarks>Min will be set to Max if it is set to a value larger than Max.</remarks>
        public float Min
        {
            get
            {
                return this.min;
            }

            set
            {
                this.ThrowIfDisposed();
                this.min = Math.Min(value, this.max);

                if (this.sliderValue < this.min)
                {
                    this.Value = this.min;
                }

                if (this.ThumbElement != null)
                {
                    this.ThumbElement.CalculateAutoThumbSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the value which it added or subtracted if the mouse wheel is moved or the arrow keys are pressed.
        /// </summary>
        /// <value>An <see cref="Int32"/> representing the value.</value>
        public float SmallChange
        {
            get
            {
                return this.smallChange;
            }

            set
            {
                this.smallChange = value;

                if (this.largeChange < value)
                {
                    this.LargeChange = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the style of the thumb.
        /// </summary>
        /// <value>A ThumbStyle representing the style of the thumb.</value>
        public ThumbStyle ThumbStyle
        {
            get
            {
                return this.ThumbElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ThumbElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the Slider.
        /// </summary>
        /// <value>The value of the Slider.</value>
        /// <remarks>Value will be set to Max if the value is larger than Max, and set to Min if the value is smaller than Min.</remarks>
        public float Value
        {
            get
            {
                return this.sliderValue;
            }

            set
            {
                this.ThrowIfDisposed();
                this.CheckValue(ref value);

                if (this.sliderValue != value)
                {
                    this.sliderValue = value;
                    this.OnValueChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Value");
                }
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
                if (this.thumbElement != null)
                {
                    retValue.Add(this.thumbElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the thumb.
        /// </summary>
        /// <value>A ThumbElement representing the thumb.</value>
        protected ThumbElement<float> ThumbElement
        {
            get
            {
                return this.thumbElement ?? (this.thumbElement = new ThumbElement<float>(this, () => (this.ZOrder * 10) + 3));
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Bring slider to the Max position.
        /// </summary>
        public void EndIncrement()
        {
            this.Modify(ScrollEventType.Last, this.Max);
        }

        /// <summary>
        /// Substract LargeChange to the slider.
        /// </summary>
        public void LargeDecrement()
        {
            this.Modify(ScrollEventType.LargeDecrement, -this.LargeChange);
        }

        /// <summary>
        /// Add LargeChange to the slider.
        /// </summary>
        public void LargeIncrement()
        {
            this.Modify(ScrollEventType.LargeIncrement, this.LargeChange);
        }

        /// <summary>
        /// Substract SmallChange to the slider.
        /// </summary>
        public void SmallDecrement()
        {
            this.Modify(ScrollEventType.SmallDecrement, -this.SmallChange);
        }

        /// <summary>
        /// Add SmallChange to the slider.
        /// </summary>
        public void SmallIncrement()
        {
            this.Modify(ScrollEventType.SmallIncrement, this.SmallChange);
        }

        /// <summary>
        /// Bring slider to the Min position.
        /// </summary>
        public void StartDecrement()
        {
            this.Modify(ScrollEventType.First, -this.Value);
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
                this.thumbElement = null;
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
            this.ThumbStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected override void OnKeyDown(KeyboardEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.DefaultKeysEnabled)
            {
                switch (e.KeyCode)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.DownArrow:
                        this.SmallDecrement();
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                        this.SmallIncrement();
                        break;

                    case ConsoleKey.PageUp:
                        this.LargeDecrement();
                        break;

                    case ConsoleKey.PageDown:
                        this.LargeIncrement();
                        break;

                    case ConsoleKey.Home:
                        this.StartDecrement();
                        break;

                    case ConsoleKey.End:
                        this.EndIncrement();
                        break;

                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            this.thumbHit = false;
            Point mouseLoc = e.MouseLocation;

            if (this.ThumbElement.HitTest(mouseLoc) || !this.IgnoreBackgroundClicks)
            {
                Point derLoc = this.GetLocationInViewport();

                // ungrab if we hit the thumb
                this.MiyagiSystem.GUIManager.GrabbedControl = null;
                this.thumbHit = true;

                Point thumbPos = this.ThumbElement.Location;
                Size thumbSize = this.ThumbStyle.Size;

                // get thumb grab point
                switch (this.ThumbStyle.Orientation)
                {
                    case Orientation.Horizontal:
                        this.thumbLocation = mouseLoc.X - (derLoc.X + thumbPos.X);
                        if (this.thumbLocation < 0 || this.thumbLocation > thumbSize.Width)
                        {
                            this.thumbLocation = (float)thumbSize.Width / 2;
                        }

                        break;

                    case Orientation.Vertical:
                        this.thumbLocation = mouseLoc.Y - (derLoc.Y + thumbPos.Y);
                        if (this.thumbLocation < 0 || this.thumbLocation > thumbSize.Height)
                        {
                            this.thumbLocation = (float)thumbSize.Height / 2;
                        }

                        break;
                }

                this.CalculateValue(mouseLoc);
                this.ThumbElement.InjectMouseDown(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDrag"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            base.OnMouseDrag(e);

            if (this.thumbHit)
            {
                this.CalculateValue(e.NewValue);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.ThumbElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ThumbElement.InjectMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.thumbHit = false;
            this.ThumbElement.InjectMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseWheelMoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected override bool OnMouseWheelMoved(ValueEventArgs<int> e)
        {
            bool result = base.OnMouseWheelMoved(e);

            int z = e.Data;

            // move the slider
            bool tempResult = false;
            if ((z > 0 && !this.Inverted) || (z < 0 && this.Inverted))
            {
                this.SmallDecrement();
                tempResult = true;
            }
            else if ((z > 0 && this.Inverted) || (z < 0 && !this.Inverted))
            {
                this.SmallIncrement();
                tempResult = true;
            }

            if (tempResult)
                result = tempResult;
            return result;
        }

        public override bool HasMouseWheelHandlers
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Raises the <see cref="Slider.Scroll"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ScrollEventArgs"/> instance containing the event data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            if (this.Scroll != null)
            {
                this.Scroll(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.AutoOrientation)
            {
                this.ThumbStyle.Orientation = e.NewValue.Width > e.NewValue.Height ? Orientation.Horizontal : Orientation.Vertical;
            }

            if (this.thumbElement != null)
            {
                this.ThumbElement.CalculateAutoThumbSize();
            }
        }

        /// <summary>
        /// Raises the <see cref="Slider.ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }

            if (this.ThumbElement != null)
            {
                this.ThumbElement.UpdateType |= UpdateTypes.Size;
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();
            this.ThumbElement.Update(this.DeltaLocation, this.DeltaSize);
        }

        #endregion Protected Methods

        #region Private Methods

        private void CalculateValue(Point loc)
        {
            float position = 0;

            switch (this.ThumbStyle.Orientation)
            {
                case Orientation.Horizontal:
                    if (this.Width == this.ThumbStyle.Size.Width)
                    {
                        return;
                    }

                    position = loc.X - this.GetLocationInViewport().X - this.thumbLocation;
                    position = position.Clamp(0, this.Width - this.ThumbStyle.Size.Width);
                    break;

                case Orientation.Vertical:
                    if (this.Height == this.ThumbStyle.Size.Height)
                    {
                        return;
                    }

                    position = loc.Y - this.GetLocationInViewport().Y - this.thumbLocation;
                    position = position.Clamp(0, this.Height - this.ThumbStyle.Size.Height);
                    break;
            }

            float newValue = (float)this.ThumbElement.ValueFromThumbPos(position) - this.Value;
            this.Modify(ScrollEventType.ThumbTrack, !this.Inverted ? newValue : -newValue);
        }

        /// <summary>
        /// Gets a proposed value for the slider, returns a value that satisfies the constraints of the slider.
        /// </summary>
        /// <param name="proposedValue">The proposed value for the slider.</param>
        private void CheckValue(ref float proposedValue)
        {
            proposedValue = (float)Math.Round((proposedValue / this.SmallChange)) * this.SmallChange;
            proposedValue = proposedValue.Clamp(this.min, this.max);
        }

        /// <summary>
        /// Modifies the position of the slider by the specified value, and fires a scroll event.
        /// </summary>
        /// <param name="type">The type of change to the value of the slider.</param>
        /// <param name="delta">The amount to add to the current value of the slider.</param>
        private void Modify(ScrollEventType type, float delta)
        {
            float oldValue = this.Value;
            this.Value += !this.Inverted ? delta : -delta;
            this.OnScroll(new ScrollEventArgs(type, oldValue, this.Value, this.ThumbStyle.Orientation));
        }

        #endregion Private Methods

        #endregion Methods
    }
}