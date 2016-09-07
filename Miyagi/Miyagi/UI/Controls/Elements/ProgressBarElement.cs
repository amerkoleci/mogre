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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a progressbar.
    /// </summary>
    public sealed class ProgressBarElement : Element<IProgressBarElementOwner, ProgressBarStyle>
    {
        #region Fields

        private TimeSpan marqueeTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BarElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public ProgressBarElement(IProgressBarElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            Point pos = this.Owner.DisplayRectangle.Location + this.Owner.GetLocationInViewport() + this.Style.Offset;
            return new Rectangle(pos, Size.Empty);
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected override Texture GetDefaultTexture()
        {
            return this.Owner.Skin.SubSkins[this.Owner.CombinedSkinName + ".Bar"];
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Offset":
                case "Orientation":
                case "MarqueeAnimationTime":
                case "Mode":
                case "BlockExtent":
                    this.RemoveSprite();
                    break;
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
                    this.UpdateSpriteCrop();
                    this.UpdateType |= UpdateTypes.Size;
                }

                if (this.UpdateType != UpdateTypes.None)
                {
                    if (this.Style.Mode != ProgressBarMode.Marquee && this.UpdateType.IsFlagSet(UpdateTypes.Size))
                    {
                        this.SetSpriteBoundsAndUV();
                    }

                    if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
                    {
                        this.SetSpriteTexture();
                    }
                }

                if (this.Style.Mode == ProgressBarMode.Marquee)
                {
                    this.AnimateMarqueeBar();
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void AnimateMarqueeBar()
        {
            if (this.marqueeTime > this.Style.MarqueeAnimationTime)
            {
                this.marqueeTime = TimeSpan.Zero;
            }

            if (this.Style.MarqueeAnimationTime.TotalMilliseconds != 0)
            {
                Size size = Size.Empty;
                Point loc = Point.Empty;
                Point derLoc = this.Owner.GetLocationInViewport();

                double ff = this.marqueeTime.TotalMilliseconds / this.Style.MarqueeAnimationTime.TotalMilliseconds;

                switch (this.Style.Orientation)
                {
                    case Orientation.Horizontal:
                        {
                            size = new Size(this.Owner.Size.Width / 4, this.Owner.Size.Height - (this.Style.Offset.Y * 2));
                            int length = this.Owner.Size.Width - (this.Style.Offset.X * 2) - size.Width;
                            loc = new Point(
                                derLoc.X + this.Style.Offset.X + (int)(length * ff),
                                derLoc.Y + this.Style.Offset.Y);
                            break;
                        }

                    case Orientation.Vertical:
                        {
                            size = new Size(this.Owner.Size.Width - (this.Style.Offset.X * 2), this.Owner.Size.Height / 4);
                            int length = this.Owner.Size.Height - (this.Style.Offset.Y * 2) - size.Height;
                            loc = new Point(
                                derLoc.X + this.Style.Offset.X,
                                derLoc.Y - this.Style.Offset.Y + this.Owner.Size.Height - size.Height - (int)(length * ff));
                            break;
                        }
                }

                this.Sprite.SetQuadBounds(0, new Rectangle(loc, size));
                this.UpdateSpriteCrop();
                this.marqueeTime += this.Owner.MiyagiSystem.TimeSinceLastUpdate;
            }
        }

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            var bounds = this.GetBounds();
            var rec = bounds.ToScreenCoordinates(this.ViewportSize);

            if (this.Style.Mode == ProgressBarMode.Blocks && this.Style.BlockExtent != 0)
            {
                int count = 0;
                switch (this.Style.Orientation)
                {
                    case Orientation.Horizontal:
                        count = (this.Owner.Size.Width - (this.Style.Offset.X * 2)) / this.Style.BlockExtent;
                        break;

                    case Orientation.Vertical:
                        count = (this.Owner.Size.Height - (this.Style.Offset.Y * 2)) / this.Style.BlockExtent;
                        break;
                }

                if (count > 0)
                {
                    var quads = new Quad[count];
                    for (int i = 0; i < count; i++)
                    {
                        quads[i] = new Quad(rec, this.CurrentUV);
                    }

                    this.PrepareSprite(quads);
                }
            }
            else
            {
                this.PrepareSprite(new Quad(rec, this.CurrentUV));
            }

            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        private void SetSpriteBoundsAndUV()
        {
            float ff = this.Owner.Max != this.Owner.Min ? (float)(this.Owner.Value - this.Owner.Min) / (this.Owner.Max - this.Owner.Min) : 0;

            // set location and size
            var derLoc = this.Owner.GetLocationInViewport();
            var ownerSize = this.Owner.Size;
            var offset = this.Style.Offset;

            switch (this.Style.Mode)
            {
                case ProgressBarMode.Continuous:
                    this.SetSpriteBoundsAndUVContinuous(ownerSize, derLoc, offset, ff);
                    break;
                case ProgressBarMode.Blocks:
                    this.SetSpriteBoundsAndUVBlocks(ownerSize, derLoc, offset, ff);
                    break;
            }

            this.UpdateSpriteCrop();
        }

        private void SetSpriteBoundsAndUVBlocks(Size ownerSize, Point ownerLocation, Point offset, float factor)
        {
            if (this.Sprite == null || this.Sprite.VertexCount < 1)
            {
                return;
            }

            int blockCount = this.Sprite.PrimitiveCount;

            int activeBlock = (int)(factor * blockCount);
            activeBlock = activeBlock.Clamp(0, blockCount - 1);
            float f = ((factor * blockCount) - activeBlock);

            var size = Size.Empty;
            var location = Point.Empty;
            for (int i = 0; i < blockCount; i++)
            {
                this.Sprite.SetUV(i, this.CurrentUV);
                this.Sprite.SetQuadBounds(i, new Rectangle(Point.Empty, Size.Empty));
            }

            switch (this.Style.Orientation)
            {
                case Orientation.Horizontal:
                    int height = ownerSize.Height - (offset.Y * 2);
                    for (int i = 0; i < activeBlock; i++)
                    {
                        var loc = new Point(ownerLocation.X + offset.X + (this.Style.BlockExtent * i), ownerLocation.Y + offset.Y);
                        this.Sprite.SetQuadBounds(i, new Rectangle(loc, new Size(this.Style.BlockExtent, height)));
                    }

                    size = new Size((int)(this.Style.BlockExtent * f), height);
                    location = new Point(ownerLocation.X + offset.X + (this.Style.BlockExtent * activeBlock), ownerLocation.Y + offset.Y);
                    break;

                case Orientation.Vertical:
                    int width = ownerSize.Width - (offset.X * 2);
                    for (int i = 0; i < activeBlock; i++)
                    {
                        var loc = new Point(ownerLocation.X + offset.X, ownerLocation.Y - offset.Y + ownerSize.Height - (this.Style.BlockExtent * (i + 1)));
                        this.Sprite.SetQuadBounds(i, new Rectangle(loc, new Size(width, this.Style.BlockExtent)));
                    }

                    size = new Size(width, (int)(this.Style.BlockExtent * f));
                    location = new Point(ownerLocation.X + offset.X, ownerLocation.Y - offset.Y + ownerSize.Height - size.Height - (this.Style.BlockExtent * activeBlock));
                    break;
            }

            this.Sprite.SetQuadBounds(activeBlock, new Rectangle(location, size));

            // set uv
            RectangleF uvRect;
            switch (this.Style.Orientation)
            {
                case Orientation.Horizontal:
                    uvRect = RectangleF.FromLTRB(0, 0, f, 1);
                    break;

                case Orientation.Vertical:
                    uvRect = RectangleF.FromLTRB(0, 1 - f, 1, 1);
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            this.Sprite.SetUV(activeBlock, this.CurrentUV.GetUVOffset(uvRect));
        }

        private void SetSpriteBoundsAndUVContinuous(Size ownerSize, Point ownerLocation, Point offset, float factor)
        {
            if (this.Sprite == null)
            {
                return;
            }

            var size = Size.Empty;
            var location = Point.Empty;
            switch (this.Style.Orientation)
            {
                case Orientation.Horizontal:
                    size = new Size(
                        (int)((ownerSize.Width - (offset.X * 2)) * factor),
                        ownerSize.Height - (offset.Y * 2));
                    location = new Point(ownerLocation.X + offset.X, ownerLocation.Y + offset.Y);
                    break;

                case Orientation.Vertical:
                    size = new Size(
                        ownerSize.Width - (offset.X * 2),
                        (int)((ownerSize.Height - (offset.Y * 2)) * factor));
                    location = new Point(
                        ownerLocation.X + offset.X,
                        ownerLocation.Y - offset.Y + ownerSize.Height - size.Height);
                    break;
            }

            this.Sprite.SetQuadBounds(0, new Rectangle(location, size));

            // set uv
            this.Sprite.SetUV(this.CurrentUV.GetUVOffset(RectangleF.FromLTRB(0, 0, 1, 1)));
        }

        #endregion Private Methods

        #endregion Methods
    }
}