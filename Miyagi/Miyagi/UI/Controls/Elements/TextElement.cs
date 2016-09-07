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
    /// An element for displaying text.
    /// </summary>
    public sealed class TextElement : Element<ITextElementOwner, TextStyle>
    {
        #region Fields

        private Point deltaLocation;
        private Point oldTextOffset;
        private int selectedQuadsOffset;
        private Range selectedQuadsRange;
        private UISprite selectedTextSprites;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public TextElement(ITextElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.selectedQuadsRange = new Range(-1, -1);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets an action which is called when the font changes.
        /// </summary>
        public Action FontChangedCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public override Size Size
        {
            get
            {
                return this.Owner.TextBounds.Size;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the opacity.
        /// </summary>
        public override void ApplyOpacity()
        {
            base.ApplyOpacity();
            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.Opacity = this.Owner.Opacity;
            }
        }

        /// <summary>
        /// Applies the TextureFiltering.
        /// </summary>
        public override void ApplyTextureFiltering()
        {
            // nothing to do. We always stay at None.
        }

        /// <summary>
        /// Applies the visibility.
        /// </summary>
        public override void ApplyVisibility()
        {
            base.ApplyVisibility();
            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.Visible = this.Owner.Visible;
            }
        }

        /// <summary>
        /// Applies the ZOrder.
        /// </summary>
        public override void ApplyZOrder()
        {
            base.ApplyZOrder();

            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.ZOrder = this.Sprite.ZOrder - 1;
            }
        }

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull() && this.selectedTextSprites == null;
        }

        /// <summary>
        /// Moves the sprites of the element.
        /// </summary>
        /// <param name="offset">The distance to move.</param>
        public override void Move(Point offset)
        {
            base.Move(offset);

            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.Move(offset);
            }
        }

        /// <summary>
        /// Removes the sprites.
        /// </summary>
        public override void RemoveSprite()
        {
            base.RemoveSprite();

            if (this.selectedTextSprites != null && this.Owner != null)
            {
                this.selectedTextSprites.RemoveFromRenderer();
                this.selectedTextSprites = null;
            }
        }

        /// <summary>
        /// Sets the quads of the selected text.
        /// </summary>
        /// <param name="range">The first and last index of the selected text.</param>
        /// <param name="offset">The offset.</param>
        public void SetSelectedQuads(Range range, int offset)
        {
            if (range.First == this.selectedQuadsRange.First
                && range.Last == this.selectedQuadsRange.Last
                && offset == this.selectedQuadsOffset)
            {
                return;
            }

            if (this.selectedTextSprites != null && range.First < 0)
            {
                this.selectedTextSprites.RemoveFromRenderer();
                this.selectedTextSprites = null;
            }

            this.selectedQuadsOffset = offset;
            this.selectedQuadsRange = range;
            this.UpdateType |= UpdateTypes.SelectedText;
        }

        /// <summary>
        /// Crops the sprites.
        /// </summary>
        public override void UpdateSpriteCrop()
        {
            base.UpdateSpriteCrop();

            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.UpdateCrop();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Style != null)
                {
                    this.Style.Font = null;
                }
            }

            this.Owner = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            var location = this.Style.Offset + this.Owner.TextBounds.Location + this.Owner.GetLocationInViewport();
            return new Rectangle(location, this.Size);
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Alignment":
                case "Multiline":
                case "WordWrap":
                    this.UpdateType |= UpdateTypes.Text;
                    break;

                case "Offset":
                    if (this.Sprite != null)
                    {
                        Point newPos = this.Style.Offset;
                        this.deltaLocation = Point.Add(
                            this.deltaLocation,
                            newPos.X - this.oldTextOffset.X,
                            newPos.Y - this.oldTextOffset.Y);

                        this.UpdateType |= UpdateTypes.Location;
                    }

                    break;

                case "Font":
                case "FontProperty":
                    this.OnFontChanged();
                    break;

                case "ForegroundColour":
                    this.UpdateType |= UpdateTypes.Colour;
                    break;

                case "SelectionBackgroundColour":
                    this.UpdateType |= UpdateTypes.SelectedText;
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
                case "Offset":
                    if (this.Sprite != null)
                    {
                        this.oldTextOffset = this.Style.Offset;
                    }

                    break;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            // Handle TextSprite changes
            if (this.UpdateType.IsFlagSet(UpdateTypes.Text) || this.Sprite == null)
            {
                if (this.Sprite != null)
                {
                    // remove old sprites
                    this.Sprite.RemoveFromRenderer();
                    this.Sprite = null;
                }

                if (!string.IsNullOrEmpty(this.Owner.DisplayedText))
                {
                    this.CreateSprite();
                    this.UpdateSpriteCrop();
                }

                this.UpdateType |= UpdateTypes.SelectedText;
                this.deltaLocation = Point.Empty;
            }

            if (this.UpdateType != UpdateTypes.None && this.Sprite != null && this.Sprite.PrimitiveCount > 0)
            {
                // move sprites to new position
                if (this.deltaLocation != PointF.Empty && this.UpdateType.IsFlagSet(UpdateTypes.Location))
                {
                    this.Sprite.Move(this.deltaLocation);
                    this.deltaLocation = Point.Empty;
                    this.UpdateSpriteCrop();
                }

                // change colour
                if (this.UpdateType.IsFlagSet(UpdateTypes.Colour))
                {
                    this.Sprite.SetColour(this.Style.ForegroundColour);
                }

                if (this.UpdateType.IsFlagSet(UpdateTypes.SelectedText))
                {
                    this.SetSelectedText();
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            string displayText = this.Owner.DisplayedText;

            var style = this.Style;
            var quads = new TextFormatter(
                new TextFormatterSettings(
                    style.Alignment,
                    style.ForegroundColour,
                    style.Font,
                    style.Multiline,
                    style.WordWrap,
                    this.ViewportSize)).CreateTextQuads(this.GetBounds(), displayText);

            if (quads.Length > 0)
            {
                this.Sprite = new UISprite(this, quads)
                              {
                                  Visible = this.Visible,
                                  Opacity = this.Opacity,
                                  ZOrder = this.GetZOrder(),
                                  TexFilter = TextureFiltering.Linear
                              };

                this.Sprite.SetTexture(this.Style.Font.TextureName);
            }
        }

        private void OnFontChanged()
        {
            this.UpdateType |= UpdateTypes.Text | UpdateTypes.SelectedText;

            if (this.FontChangedCallback != null)
            {
                this.FontChangedCallback();
            }
        }

        private void SetSelectedText()
        {
            if (this.selectedTextSprites != null)
            {
                this.selectedTextSprites.RemoveFromRenderer();
                this.selectedTextSprites = null;
            }

            if (this.selectedQuadsRange.First >= 0 && this.selectedQuadsRange.Last >= 0)
            {
                int max = this.selectedQuadsRange.Last;
                int min = this.selectedQuadsRange.First;

                int count = max - min + 1;

                if (min - this.selectedQuadsOffset + count > this.Sprite.PrimitiveCount)
                {
                    count = this.Sprite.PrimitiveCount - min + this.selectedQuadsOffset - 1;
                }

                if (count < 1)
                {
                    return;
                }

                var cd = this.Style.SelectionBackgroundColour;
                var quads = new Quad[count];
                for (int i = min - this.selectedQuadsOffset, j = 0; j < count; i++, j++)
                {
                    quads[j] = (Quad)this.Sprite.GetPrimitive(i).GeometricCopy;
                    quads[j].SetVertexColours(cd);
                }

                this.selectedTextSprites = new UISprite(this, quads)
                                           {
                                               Visible = this.Visible,
                                               Opacity = this.Opacity,
                                               ZOrder = this.GetZOrder() - 1,
                                               TexFilter = TextureFiltering.Linear
                                           };

                this.selectedTextSprites.SetTexture(RenderManager.OpaqueTexture);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}