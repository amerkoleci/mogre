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
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a border.
    /// </summary>
    public sealed class BorderElement : Element<IBorderElementOwner, BorderStyle>
    {
        #region Fields

        private PointF borderSpritesBottomRightDelta;
        private PointF borderSpritesTopLeftDelta;
        private Thickness oldThickness;
        private bool updateBorderUV;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BorderElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public BorderElement(IBorderElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the size of the BorderElement.
        /// </summary>
        /// <remarks>Always returns Size.Empty.</remarks>
        public override Size Size
        {
            get
            {
                return Size.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a Action which is called when Style.Thickness property changes.
        /// </summary>
        public Action<Thickness, Thickness> ThicknessChangedCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the UpdateType.
        /// </summary>
        /// <value>An UpdateType representing what should be changed when the element is updated.</value>
        public override UpdateTypes UpdateType
        {
            get
            {
                return base.UpdateType;
            }

            set
            {
                base.UpdateType = value;

                if (value == UpdateTypes.None)
                {
                    this.borderSpritesBottomRightDelta = PointF.Empty;
                    this.borderSpritesTopLeftDelta = PointF.Empty;
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Creates the Quads for a BorderElement.
        /// </summary>
        /// <param name="rect">A Rect representing the position.</param>
        /// <param name="thickness">The size of the border.</param>
        /// <returns>The newly created Quads.</returns>
        public Quad[] CreateBorderQuads(Rectangle rect, Thickness thickness)
        {
            Quad[] borderSprites = new Quad[8];
            Size screenRes = this.ViewportSize;

            int x1 = rect.Left;
            int x2 = rect.Right;
            int y1 = rect.Top;
            int y2 = rect.Bottom;

            // left
            Rectangle rec = Rectangle.FromLTRB(x1, y1 + thickness.Top, x1 + thickness.Left, y2 - thickness.Bottom);
            borderSprites[0] = new Quad(rec.ToScreenCoordinates(screenRes));

            // right
            rec = Rectangle.FromLTRB(x2 - thickness.Right, y1 + thickness.Top, x2, y2 - thickness.Bottom);
            borderSprites[1] = new Quad(rec.ToScreenCoordinates(screenRes));

            // top
            rec = Rectangle.FromLTRB(x1 + thickness.Left, y1, x2 - thickness.Right, y1 + thickness.Top);
            borderSprites[2] = new Quad(rec.ToScreenCoordinates(screenRes));

            // bottom
            rec = Rectangle.FromLTRB(x1 + thickness.Left, y2 - thickness.Bottom, x2 - thickness.Right, y2);
            borderSprites[3] = new Quad(rec.ToScreenCoordinates(screenRes));

            // top left
            rec = Rectangle.FromLTRB(x1, y1, x1 + thickness.Left, y1 + thickness.Top);
            borderSprites[4] = new Quad(rec.ToScreenCoordinates(screenRes));

            // top right
            rec = Rectangle.FromLTRB(x2 - thickness.Right, y1, x2, y1 + thickness.Top);
            borderSprites[5] = new Quad(rec.ToScreenCoordinates(screenRes));

            // bottom left
            rec = Rectangle.FromLTRB(x1, y2 - thickness.Bottom, x1 + thickness.Left, y2);
            borderSprites[6] = new Quad(rec.ToScreenCoordinates(screenRes));

            // bottom right
            rec = Rectangle.FromLTRB(x2 - thickness.Right, y2 - thickness.Bottom, x2, y2);
            borderSprites[7] = new Quad(rec.ToScreenCoordinates(screenRes));

            return borderSprites;
        }

        /// <summary>
        /// Resizes the sprites of the element.
        /// </summary>
        /// <param name="diff">The distance to resize.</param>
        public override void Resize(Point diff)
        {
            if (this.Sprite != null)
            {
                var offsetf = new PointF(
                    ((float)diff.X / this.ViewportSize.Width) * 2,
                    -(((float)diff.Y / this.ViewportSize.Height) * 2));

                this.Sprite.OffsetBorderQuads(offsetf);
            }
        }

        /// <summary>
        /// Sets the size of the border.
        /// </summary>
        /// <param name="left">The size of the Left border.</param>
        /// <param name="top">The size of the top border.</param>
        /// <param name="right">The size of the right border.</param>
        /// <param name="bottom">The size of the bottom border.</param>
        public void SetSize(int left, int top, int right, int bottom)
        {
            this.Style.Thickness = new Thickness(left, top, right, bottom);
        }

        #endregion Public Methods

        #region Internal Methods

        internal bool HasBorder()
        {
            return this.Style.Thickness != Thickness.Empty
                   && this.Owner.Skin != null
                   && this.Owner.Skin.IsSubSkinDefined(this.Owner.CombinedSkinName + ".Border");
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            return new Rectangle(this.Owner.GetLocationInViewport(), this.Owner.Size);
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected override Texture GetDefaultTexture()
        {
            return this.Owner.HasBorder
                       ? this.Owner.Skin.SubSkins[this.Owner.CombinedSkinName + ".Border"]
                       : null;
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Thickness":
                    this.SetSize();
                    break;

                case "UV":
                    if (this.Sprite != null)
                    {
                        this.updateBorderUV = true;
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
                case "Thickness":
                    this.oldThickness = this.Style.Thickness;
                    break;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            if (this.Sprite == null && this.Texture != null)
            {
                // create new sprites
                if (this.Style.Thickness != Thickness.Empty && !string.IsNullOrEmpty(this.CurrentFrame.FileName))
                {
                    this.CreateSprite();
                    this.UpdateSpriteCrop();

                    if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
                    {
                        this.UpdateType ^= UpdateTypes.Texture;
                    }
                }
            }

            if (this.UpdateType != UpdateTypes.None)
            {
                if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
                {
                    this.SetSpriteTexture();
                    if (this.Sprite != null)
                    {
                        this.updateBorderUV = true;
                    }
                }

                if (this.Sprite != null)
                {
                    // change border size
                    if (this.UpdateType.IsFlagSet(UpdateTypes.Size))
                    {
                        if (this.borderSpritesTopLeftDelta.X != 0)
                        {
                            var left = new PointF(this.borderSpritesTopLeftDelta.X, 0);
                            this.Sprite.ResizeBorderQuadLeft(left);

                            // move owner texture
                            this.Owner.Sprite.MoveLeftEdge(left);
                        }

                        if (this.borderSpritesTopLeftDelta.Y != 0)
                        {
                            var top = new PointF(0, this.borderSpritesTopLeftDelta.Y);
                            this.Sprite.ResizeBorderQuadTop(top);

                            // move owner texture
                            this.Owner.Sprite.MoveTopEdge(top);
                        }

                        if (this.borderSpritesBottomRightDelta.X != 0)
                        {
                            var right = new PointF(-this.borderSpritesBottomRightDelta.X, 0);
                            this.Sprite.ResizeBorderQuadRight(right);

                            // move owner texture
                            this.Owner.Sprite.MoveRightEdge(right);
                        }

                        if (this.borderSpritesBottomRightDelta.Y != 0)
                        {
                            var bottom = new PointF(0, -this.borderSpritesBottomRightDelta.Y);
                            this.Sprite.ResizeBorderQuadBottom(bottom);

                            // move owner texture
                            this.Owner.Sprite.MoveBottomEdge(bottom);
                        }

                        this.UpdateSpriteCrop();
                    }

                    // change borderuv
                    if (this.updateBorderUV)
                    {
                        this.SetBorderQuadUV();
                        this.UpdateSpriteCrop();
                        this.updateBorderUV = false;
                    }
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
            this.Sprite = new UISprite(this, this.CreateBorderQuads(this.GetBounds(), this.Style.Thickness))
                          {
                              Visible = this.Visible,
                              Opacity = this.Opacity,
                              ZOrder = this.GetZOrder(),
                              TexFilter = this.TextureFiltering
                          };

            this.SetBorderQuadUV();
            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        private void SetBorderQuadUV()
        {
            RectangleF borderUV = this.Style.UV;
            RectangleF textureUV = this.CurrentUV;
            this.Sprite.SetBorderQuadUV(textureUV, borderUV);
        }

        private void SetSize()
        {
            if (this.Owner == null)
            {
                return;
            }

            Thickness oldSize = this.oldThickness;
            Thickness newSize = this.Style.Thickness;

            if (this.Sprite != null)
            {
                Size screenRes = this.ViewportSize;
                Point pos = this.Owner.Location;

                PointF oldPos = new Point(pos.X + oldSize.Left, pos.Y + oldSize.Top).ToScreenCoordinates(screenRes);
                PointF newPos = new Point(pos.X + newSize.Left, pos.Y + newSize.Top).ToScreenCoordinates(screenRes);
                this.borderSpritesTopLeftDelta = newPos - oldPos;

                oldPos = new Point(pos.X + oldSize.Right, pos.Y + oldSize.Bottom).ToScreenCoordinates(screenRes);
                newPos = new Point(pos.X + newSize.Right, pos.Y + newSize.Bottom).ToScreenCoordinates(screenRes);
                this.borderSpritesBottomRightDelta = newPos - oldPos;

                this.UpdateType |= UpdateTypes.Size;
            }

            if (this.ThicknessChangedCallback != null)
            {
                this.ThicknessChangedCallback(oldSize, newSize);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}