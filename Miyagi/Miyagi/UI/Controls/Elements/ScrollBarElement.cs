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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a scrollbar.
    /// </summary>
    public sealed class ScrollBarElement : InteractiveElement<IInteractiveElementOwner, ScrollBarStyle>, IThumbElementOwner<int>, IBorderElementOwner
    {
        #region Fields

        private readonly IScrollBarElementController controller;

        private BorderElement borderElement;
        private ButtonElement decArrowButtonElement;
        private ButtonElement incArrowButtonElement;
        private bool isVisible;
        private Orientation orientation;
        private int scrollBarValue;
        private ThumbElement<int> thumbElement;
        private int thumbLocation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScrollBarElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        /// <param name="controller">The controller of the element.</param>
        public ScrollBarElement(IInteractiveElementOwner owner, Func<int> zorderGetter, IScrollBarElementController controller)
            : base(owner, zorderGetter)
        {
            this.controller = controller;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the Value property is changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Occurs when the Visible property is changed.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>A BorderElement representing the border.</value>
        public BorderElement BorderElement
        {
            get
            {
                return this.borderElement ?? (this.borderElement = new BorderElement(this, () => this.GetZOrder() + 1));
            }
        }

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public override string CombinedSkinName
        {
            get
            {
                string prefix = string.Empty;
                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        prefix = "H";
                        break;
                    case Orientation.Vertical:
                        prefix = "V";
                        break;
                }

                return base.CombinedSkinName + "." + prefix + "ScrollBar";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return this.BorderElement.HasBorder();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar is invented.
        /// </summary>
        public bool Inverted
        {
            get
            {
                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        return false;
                    case Orientation.Vertical:
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the value which is added or subtracted if PageUp or PageDown is pressed.
        /// </summary>
        public int LargeChange
        {
            get
            {
                return this.controller.GetVisibleUnitsMax();
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.controller.GetLocation();
            }
        }

        /// <summary>
        /// Gets the maximum value of the ScrollBarElement.
        /// </summary>
        public int Max
        {
            get
            {
                return this.controller.GetTotalUnitsCount() - this.controller.GetVisibleUnitsCount();
            }
        }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public int Min
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                this.orientation = value;
                this.ThumbElement.PreferredOrientation = value;
            }
        }

        /// <summary>
        /// Gets the size in pixels.
        /// </summary>
        public override Size Size
        {
            get
            {
                int userExtent = this.Style.Extent;
                int autoExtent = this.controller.GetAutoExtent();

                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        return new Size(autoExtent, userExtent);
                    case Orientation.Vertical:
                        return new Size(userExtent, autoExtent);
                }

                return Size.Empty;
            }
        }

        /// <summary>
        /// Gets the TextureStyle.
        /// </summary>
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
                if (this.incArrowButtonElement != null)
                {
                    yield return this.incArrowButtonElement;
                }

                if (this.decArrowButtonElement != null)
                {
                    yield return this.decArrowButtonElement;
                }

                if (this.thumbElement != null)
                {
                    yield return this.thumbElement;
                }

                if (this.borderElement != null)
                {
                    yield return this.borderElement;
                }
            }
        }

        /// <summary>
        /// Gets the thumb bounds.
        /// </summary>
        public Rectangle ThumbBounds
        {
            get
            {
                if (!this.Style.ShowButtons)
                {
                    return new Rectangle(this.GetLocationInViewport(), this.Size);
                }

                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        return new Rectangle(
                            this.GetLocationInViewport() + new Point(this.Style.Extent, 0),
                            this.Size - new Size(this.Style.Extent * 2, 0));
                    case Orientation.Vertical:
                        return new Rectangle(
                            this.GetLocationInViewport() + new Point(0, this.Style.Extent),
                            this.Size - new Size(0, this.Style.Extent * 2));
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
        }

        /// <summary>
        /// Gets the thumb.
        /// </summary>
        public ThumbElement<int> ThumbElement
        {
            get
            {
                return this.thumbElement ?? (this.thumbElement = new ThumbElement<int>(this, () => this.GetZOrder() + 2));
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get
            {
                return this.scrollBarValue;
            }

            set
            {
                value = value.Clamp(this.Min, this.Max);

                if (this.scrollBarValue != value)
                {
                    this.scrollBarValue = value;
                    this.ThumbSizeDirty = true;
                    if (this.ValueChanged != null)
                    {
                        this.ValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ScrollBarElement is visible.
        /// </summary>
        public override bool Visible
        {
            get
            {
                if (!this.Owner.Visible)
                {
                    return false;
                }

                if (this.Style != null && this.Style.AlwaysVisible)
                {
                    return true;
                }

                return this.isVisible;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal bool IsThumbHit
        {
            get;
            private set;
        }

        internal bool ThumbSizeDirty
        {
            get;
            set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull() && this.borderElement.AreAllSpritesNull() && this.thumbElement.AreAllSpritesNull();
        }

        /// <summary>
        /// Gets the location relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.controller.GetLocation() + this.Owner.GetLocationInViewport();
        }

        /// <summary>
        /// Hides the ScrollBarElement.
        /// </summary>
        public void Hide()
        {
            this.SetVisible(false);
            this.RemoveSprite();
        }

        /// <summary>
        /// Shows the ScollBarElement.
        /// </summary>
        public void Show()
        {
            this.SetVisible(true);
            this.ThumbSizeDirty = true;
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
            this.ValueChanged = null;
            this.VisibleChanged = null;
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            return new Rectangle(this.GetLocationInViewport(), this.Size);
        }

        /// <summary>
        /// Handles MouseDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var mouseLoc = e.MouseLocation;

            if (this.ThumbElement.HitTest(mouseLoc))
            {
                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        this.thumbLocation = mouseLoc.X - (this.GetLocationInViewport().X + this.ThumbElement.Location.X);
                        break;

                    case Orientation.Vertical:
                        this.thumbLocation = mouseLoc.Y - (this.GetLocationInViewport().Y + this.ThumbElement.Location.Y);
                        break;
                }

                this.IsThumbHit = true;
                this.MiyagiSystem.GUIManager.GrabbedControl = null;
                this.ThumbElement.InjectMouseDown(e);
            }
            else
            {
                this.IsThumbHit = false;
            }

            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseDown(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseDown(e);
            }
        }

        /// <summary>
        /// Handles MouseDrag injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnMouseDrag(ChangedValueEventArgs<Point> e)
        {
            if (this.IsThumbHit)
            {
                this.CalculateValue(e.NewValue);
            }
        }

        /// <summary>
        /// Handles MouseEnter injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseEnter(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseEnter(e);
            }
        }

        /// <summary>
        /// Handles MouseHeld injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseDown(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseDown(e);
            }
        }

        /// <summary>
        /// Handles MouseHover injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.ThumbElement.InjectMouseHover(e);
            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseHover(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseHover(e);
            }
        }

        /// <summary>
        /// Handles MouseLeave injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ThumbElement.InjectMouseLeave(e);
            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseLeave(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseLeave(e);
            }
        }

        /// <summary>
        /// Handles MouseUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            this.ThumbElement.InjectMouseUp(e);
            if (this.incArrowButtonElement != null)
            {
                this.incArrowButtonElement.InjectMouseUp(e);
            }

            if (this.decArrowButtonElement != null)
            {
                this.decArrowButtonElement.InjectMouseUp(e);
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
                case "AlwaysVisible":
                    break;

                case "Extent":
                    this.ThumbSizeDirty = true;
                    this.RemoveSprite();
                    break;

                case "BorderStyle":
                case "ThumbStyle":
                    this.SetSubElementStyles();
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
                this.BorderElement.Style = this.Style.BorderStyle;
                this.ThumbElement.Style = this.Style.ThumbStyle;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            if (this.Style.Extent == 0)
            {
                return;
            }

            if (!this.Style.AlwaysVisible)
            {
                if (this.isVisible)
                {
                    if (this.controller.GetVisibleUnitsMax() >= this.controller.GetTotalUnitsCount())
                    {
                        this.Hide();
                    }
                }
                else if (this.controller.GetShouldShow())
                {
                    this.Show();
                }
            }
            else if (!this.isVisible)
            {
                this.Show();
            }

            if (this.isVisible)
            {
                if (this.ThumbSizeDirty)
                {
                    this.CalculateThumbSize();
                    this.RemoveSprite();
                    this.ThumbSizeDirty = false;
                }

                if (this.Sprite == null && this.Texture != null)
                {
                    this.CreateSprite();
                    this.CreateButtons();
                    this.UpdateSpriteCrop();
                }
                else
                {
                    if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
                    {
                        this.SetSpriteTexture();

                        this.borderElement.UpdateType |= UpdateTypes.Texture;
                        this.thumbElement.UpdateType |= UpdateTypes.Texture;
                    }
                }

                this.ForEachSubElement(ele => ele.Update());
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void CalculateThumbSize()
        {
            int width = this.ThumbBounds.Width;
            int height = this.ThumbBounds.Height;

            int visibleUnits = this.controller.GetVisibleUnitsCount();
            if (visibleUnits > 0)
            {
                float f = (float)visibleUnits / this.controller.GetTotalUnitsCount();
                f = f > 1 ? 1 : f;

                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        width = Math.Max((int)(width * f), 10);
                        break;
                    case Orientation.Vertical:
                        height = Math.Max((int)(height * f), 10);
                        break;
                }
            }

            this.Style.ThumbStyle.Size = new Size(width, height);
        }

        private void CalculateValue(Point loc)
        {
            float pos = 0, extent = 0;

            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    if (this.ThumbBounds.Width == this.Style.ThumbStyle.Size.Width)
                    {
                        return;
                    }

                    pos = loc.X - this.GetLocationInViewport().X - this.thumbLocation;
                    extent = this.ThumbBounds.Width - this.Style.ThumbStyle.Size.Width;
                    break;

                case Orientation.Vertical:
                    if (this.ThumbBounds.Height == this.Style.ThumbStyle.Size.Height)
                    {
                        return;
                    }

                    pos = loc.Y - this.GetLocationInViewport().Y - this.thumbLocation;
                    extent = this.ThumbBounds.Height - this.Style.ThumbStyle.Size.Height;
                    break;
            }

            pos = pos.Clamp(0, extent);
            float f = pos / extent;

            this.Value = (int)Math.Round(f * this.Max);
        }

        private void CreateButtons()
        {
            if (this.Style.ShowButtons)
            {
                Point incButtonLocation = Point.Empty, decButtonLocation = Point.Empty;
                switch (this.Orientation)
                {
                    case Orientation.Horizontal:
                        incButtonLocation = new Point(this.Size.Width - this.Style.Extent, 0);
                        break;
                    case Orientation.Vertical:
                        decButtonLocation = new Point(0, this.Size.Height - this.Style.Extent);
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }

                bool oldMouseOver = false;
                if (this.incArrowButtonElement != null)
                {
                    oldMouseOver = this.incArrowButtonElement.IsMouseOver;
                    this.incArrowButtonElement.Dispose();
                    this.incArrowButtonElement.MouseDown -= this.IncArrowButtonElementMouseDown;
                    this.incArrowButtonElement = null;
                }

                this.incArrowButtonElement = new ButtonElement(this, () => this.GetZOrder() + 3, "IncreaseButton")
                                             {
                                                 Location = incButtonLocation,
                                                 Size = new Size(this.Style.Extent, this.Style.Extent),
                                                 IsMouseOver = oldMouseOver
                                             };

                this.incArrowButtonElement.MouseDown += this.IncArrowButtonElementMouseDown;

                oldMouseOver = false;
                if (this.decArrowButtonElement != null)
                {
                    oldMouseOver = this.decArrowButtonElement.IsMouseOver;
                    this.decArrowButtonElement.Dispose();
                    this.decArrowButtonElement.MouseDown -= this.DecArrowButtonElementMouseDown;
                    this.decArrowButtonElement = null;
                }

                this.decArrowButtonElement = new ButtonElement(this, () => this.GetZOrder() + 3, "DecreaseButton")
                                             {
                                                 Location = decButtonLocation,
                                                 Size = new Size(this.Style.Extent, this.Style.Extent),
                                                 IsMouseOver = oldMouseOver
                                             };

                this.decArrowButtonElement.MouseDown += this.DecArrowButtonElementMouseDown;
            }
        }

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            var rec = this.GetBounds().ToScreenCoordinates(this.ViewportSize);
            this.PrepareSprite(new Quad(rec, this.CurrentUV));
            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        private void DecArrowButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Inverted)
            {
                this.Value++;
            }
            else
            {
                this.Value--;
            }
        }

        private void IncArrowButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Inverted)
            {
                this.Value--;
            }
            else
            {
                this.Value++;
            }
        }

        private void SetVisible(bool value)
        {
            if (this.isVisible != value)
            {
                this.isVisible = value;

                if (!this.Style.AlwaysVisible)
                {
                    if (this.VisibleChanged != null)
                    {
                        this.VisibleChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}