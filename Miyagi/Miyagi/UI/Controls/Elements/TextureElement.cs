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

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a texture.
    /// </summary>
    public class TextureElement : Element<IElementOwner, Style>
    {
        #region Fields

        private Point offset;
        private Size size;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextureElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public TextureElement(IElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.size = owner.Size;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Point Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                if (this.offset != value)
                {
                    this.offset = value;
                    this.RemoveSprite();
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
                return this.size;
            }

            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.RemoveSprite();
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Resizes the sprites of the element.
        /// </summary>
        /// <param name="diff">The distance to resize.</param>
        public override void Resize(Point diff)
        {
            if (this.Sprite != null)
            {
                this.size += new Size(diff.X, diff.Y);
                this.Sprite.Resize(diff);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        protected void CreateSprite()
        {
            var rec = this.GetBounds().ToScreenCoordinates(this.ViewportSize);
            this.PrepareSprite(new Quad(rec, this.CurrentUV));

            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            return new Rectangle(
                Point.Add(this.Owner.GetLocationInViewport(), this.Offset),
                this.Size);
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
                    this.UpdateSpriteCrop();
                }
            }

            if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
            {
                this.SetSpriteTexture();
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}