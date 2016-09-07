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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a caret.
    /// </summary>
    public sealed class CaretElement : Element<ICaretElementOwner, CaretStyle>
    {
        #region Fields

        private Point caretDeltaSize;
        private DateTime lastBlinkTime;
        private Size oldSize;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CaretElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public CaretElement(ICaretElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the size of the CaretElement.
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

        #endregion Public Properties

        #region Internal Properties

        internal bool IsCaretVisible
        {
            get;
            set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the opacity.
        /// </summary>
        public override void ApplyOpacity()
        {
            if (this.IsCaretVisible)
            {
                base.ApplyOpacity();
            }
        }

        /// <summary>
        /// Applies the TextureFiltering.
        /// </summary>
        public override void ApplyTextureFiltering()
        {
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Colour":
                    this.UpdateType |= UpdateTypes.Colour;
                    break;

                case "Size":
                    if (this.Sprite != null)
                    {
                        var diffSize = this.Style.Size - this.oldSize;
                        this.caretDeltaSize = new Point(diffSize.Width, diffSize.Height);
                        this.UpdateType |= UpdateTypes.Size;
                    }

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
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            var currentTime = this.MiyagiSystem.LastUpdate;

            if (this.Sprite == null)
            {
                this.CreateSprite();
                this.UpdateSpriteCrop();
                this.lastBlinkTime = currentTime;
            }
            else
            {
                if (this.Owner.IsKeyDown)
                {
                    // always show caret if key is down
                    this.Sprite.Opacity = this.Owner.Opacity;
                }
                else if (currentTime - this.lastBlinkTime > this.Style.BlinkInterval)
                {
                    // blink
                    this.lastBlinkTime = currentTime;
                    this.Sprite.Opacity = this.IsCaretVisible ? 0 : this.Owner.Opacity;
                    this.IsCaretVisible = !this.IsCaretVisible;
                }
            }

            // change colour
            if (this.UpdateType.IsFlagSet(UpdateTypes.Colour))
            {
                this.Sprite.SetColour(this.Style.Colour);
            }

            // change size
            if (this.UpdateType.IsFlagSet(UpdateTypes.Size))
            {
                this.Sprite.Resize(this.caretDeltaSize);
                this.caretDeltaSize = Point.Empty;
                this.UpdateSpriteCrop();
            }

            if (this.UpdateType.IsFlagSet(UpdateTypes.Location))
            {
                this.SetLocation();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            this.PrepareSprite(new Quad(this.Style.Colour, Rectangle.Empty, new RectangleF(0, 0, 1, 1)));
            this.Sprite.SetTexture(RenderManager.OpaqueTexture);
            this.SetLocation();
        }

        private void SetLocation()
        {
            var textElement = this.Owner.TextElement;
            var owner = this.Owner;
            var style = this.Style;

            if (textElement.Sprite != null
                && textElement.Sprite.PrimitiveCount > 0
                && owner.CaretLocation - owner.TextScrollOffset > 0)
            {
                int caretQuad = owner.CaretLocation - owner.TextScrollOffset;
                caretQuad = caretQuad - owner.GetNumberOfNewLinesBefore(caretQuad) - 1;
                if (caretQuad >= textElement.Sprite.PrimitiveCount)
                {
                    caretQuad = textElement.Sprite.PrimitiveCount - 1;
                }

                var q = textElement.Sprite.GetQuad(caretQuad);
                float left;
                if (!owner.IsCaretOnNewLine)
                {
                    left = ((q.Right + 1) / 2) * this.ViewportSize.Width;
                }
                else
                {
                    left = ((q.Left + 1) / 2) * this.ViewportSize.Width;
                }

                float top = ((-q.Top + 1) / 2) * this.ViewportSize.Height;
                float height = (((-q.Bottom + 1) - (-q.Top + 1)) / 2) * this.ViewportSize.Height;
                top += (height - style.Size.Height) / 2;
                var rec = new Rectangle(new Point((int)left, (int)top), style.Size);
                this.Sprite.SetQuadBounds(0, rec);
            }
            else
            {
                var alignment = textElement.Style.Alignment;
                var pos = owner.DisplayRectangle.Location + this.OwningControl.GetLocationInViewport();

                int x = pos.X, y = pos.Y;

                switch (alignment)
                {
                    case Alignment.MiddleLeft:
                    case Alignment.MiddleCenter:
                    case Alignment.MiddleRight:
                        y = pos.Y + ((owner.Size.Height - style.Size.Height) / 2);
                        break;
                    case Alignment.BottomLeft:
                    case Alignment.BottomCenter:
                    case Alignment.BottomRight:
                        y = pos.Y + owner.Size.Height - style.Size.Height;
                        break;
                }

                switch (alignment)
                {
                    case Alignment.MiddleCenter:
                    case Alignment.BottomCenter:
                    case Alignment.TopCenter:
                        x = pos.X + (owner.Size.Width / 2);
                        break;

                    case Alignment.MiddleRight:
                    case Alignment.BottomRight:
                    case Alignment.TopRight:
                        x = pos.X + owner.Size.Width;
                        break;
                }

                x += textElement.Style.Offset.X;

                var rec = new Rectangle(new Point(x, y), style.Size);
                this.Sprite.SetQuadBounds(0, rec);
            }

            this.UpdateSpriteCrop();

            if (this.UpdateType.IsFlagSet(UpdateTypes.Location))
            {
                this.UpdateType ^= UpdateTypes.Location;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}