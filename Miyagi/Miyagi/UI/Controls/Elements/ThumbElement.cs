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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a thumb.
    /// </summary>
    public sealed class ThumbElement<T> : InteractiveElement<IThumbElementOwner<T>, ThumbStyle>, IBorderElementOwner
        where T : IEquatable<T>, IComparable<T>
    {
        #region Fields

        /// <summary>
        /// The minimum size of a thumb.
        /// </summary>
        public static readonly int MinThumbSize = 5;

        private BorderElement borderElement;
        private Point deltaLocation;
        private Point deltaSize;
        private Point location;
        private Size oldSize;
        private Orientation? preferredOrientation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ThumbElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public ThumbElement(IThumbElementOwner<T> owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

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
        /// Gets the texture of the border.
        /// </summary>
        public Texture BorderTexture
        {
            get
            {
                Skin skin = this.Owner.Skin;
                return skin != null && skin.IsSubSkinDefined(this.CombinedSkinName + ".Border")
                           ? skin.SubSkins[this.CombinedSkinName + ".Border"]
                           : null;
            }
        }

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public override string CombinedSkinName
        {
            get
            {
                return base.CombinedSkinName + ".Thumb";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the thumb has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return this.BorderElement.HasBorder();
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }

            private set
            {
                if (this.location != value)
                {
                    if (this.Sprite != null)
                    {
                        this.deltaLocation = new Point(value.X - this.location.X, value.Y - this.location.Y);
                    }

                    this.location = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred orientation.
        /// </summary>
        public Orientation? PreferredOrientation
        {
            get
            {
                return this.preferredOrientation;
            }

            set
            {
                this.preferredOrientation = value;
                if (value.HasValue)
                {
                    this.Style.Orientation = value.Value;
                }
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public override Size Size
        {
            get
            {
                return this.Style.Size;
            }

            set
            {
                this.Style.Size = value;
            }
        }

        /// <summary>
        /// Gets the SkinStyle.
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
                if (this.borderElement != null)
                {
                    yield return this.borderElement;
                }
            }
        }

        #endregion Public Properties

        #region Private Properties

        private double LargeChange
        {
            get
            {
                return Convert.ToDouble(this.Owner.LargeChange);
            }
        }

        private double Max
        {
            get
            {
                return Convert.ToDouble(this.Owner.Max);
            }
        }

        private double Min
        {
            get
            {
                return Convert.ToDouble(this.Owner.Min);
            }
        }

        private Orientation Orientation
        {
            get
            {
                return this.PreferredOrientation ?? this.Style.Orientation;
            }
        }

        private double Value
        {
            get
            {
                return Convert.ToDouble(this.Owner.Value);
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull() && this.borderElement.AreAllSpritesNull();
        }

        /// <summary>
        /// Gets the derived location.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.location + this.Owner.ThumbBounds.Location;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void CalculateAutoThumbSize()
        {
            if (!this.Style.AutoSize || this.Owner == null || this.Max == 0)
            {
                return;
            }

            double reason = this.LargeChange / this.Max;

            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    double width = Math.Max(MinThumbSize, this.Owner.ThumbBounds.Width * reason);
                    this.Style.Size = new Size((int)width, this.Owner.ThumbBounds.Height);
                    break;
                case Orientation.Vertical:
                    double height = Math.Max(MinThumbSize, this.Owner.ThumbBounds.Height * reason);
                    this.Style.Size = new Size(this.Owner.ThumbBounds.Width, (int)height);
                    break;
            }
        }

        internal double ValueFromThumbPos(double thumbpos)
        {
            if (this.Owner == null)
            {
                return 0;
            }

            double f = 0;

            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    float width = this.Owner.ThumbBounds.Width - this.Style.Size.Width;
                    f = thumbpos / width;
                    break;

                case Orientation.Vertical:
                    float height = this.Owner.ThumbBounds.Height - this.Style.Size.Height;
                    f = 1 - (thumbpos / height);
                    break;
            }

            if (this.Owner.Inverted)
            {
                f = 1 - f;
            }

            return (f * (this.Max - this.Min)) + this.Min;
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            this.Location = this.ThumbLocationFromValue();
            return new Rectangle(this.GetLocationInViewport(), this.Style.Size);
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "AutoSize":
                    if (this.Style.AutoSize)
                    {
                        this.CalculateAutoThumbSize();
                    }

                    break;

                case "Orientation":
                    this.UpdateType |= UpdateTypes.Size;
                    break;

                case "Size":
                    if (this.Sprite != null)
                    {
                        this.deltaSize = Point.Add(
                            this.deltaSize,
                            this.Style.Size.Width - this.oldSize.Width,
                            this.Style.Size.Height - this.oldSize.Height);
                    }

                    this.UpdateType |= UpdateTypes.Size;
                    break;

                case "BorderStyle":
                    this.SetSubElementStyles();
                    break;
            }
        }

        /// <summary>
        /// Handles changing style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanging(string name)
        {
            switch (name)
            {
                case "Size":
                    if (this.Sprite != null)
                    {
                        this.oldSize = this.Style.Size;
                    }

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
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            if (this.Texture != null)
            {
                if (this.Sprite == null)
                {
                    this.CreateSprite();
                    this.UpdateType |= UpdateTypes.Size;
                }

                // resize and position thumb and border
                if (this.UpdateType.IsFlagSet(UpdateTypes.Size))
                {
                    this.Location = this.ThumbLocationFromValue();
                    var rect = new Rectangle(
                        this.GetLocationInViewport(),
                        this.Style.Size);
                    this.Sprite.SetQuadBounds(0, rect);

                    this.borderElement.Resize(this.deltaSize);
                    this.borderElement.Move(this.deltaLocation);
                    this.deltaSize = Point.Empty;
                    this.deltaLocation = Point.Empty;
                    this.UpdateSpriteCrop();
                }
            }

            if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
            {
                this.SetSpriteTexture();
                this.BorderElement.UpdateType |= UpdateTypes.Texture;
            }

            this.ForEachSubElement(ele => ele.Update());
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            var rec = this.GetBounds().ToScreenCoordinates(this.ViewportSize);

            this.PrepareSprite(new Quad(rec, this.CurrentUV));
            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        private Point ThumbLocationFromValue()
        {
            if (this.Owner == null)
            {
                return Point.Empty;
            }

            double d = this.Max != this.Min
                           ? (this.Value - this.Min) / (this.Max - this.Min)
                           : 0;

            int left = 0, top = 0;

            var thumbRec = this.Owner.ThumbBounds;
            var size = this.Style.Size;
            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    left = (int)Math.Round((thumbRec.Width - size.Width) * d);
                    if (this.Owner.Inverted)
                    {
                        left = thumbRec.Width - size.Width - left;
                    }

                    top = (thumbRec.Height - size.Height) / 2;

                    break;

                case Orientation.Vertical:
                    left = (thumbRec.Width - size.Width) / 2;

                    top = (int)Math.Round((thumbRec.Height - size.Height) * d);
                    if (!this.Owner.Inverted)
                    {
                        top = thumbRec.Height - size.Height - top;
                    }

                    break;
            }

            return new Point(left, top);
        }

        #endregion Private Methods

        #endregion Methods
    }
}