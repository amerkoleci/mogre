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
namespace Miyagi.TwoD.Layers
{
    using System;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;

    /// <summary>
    /// A simple 2D overlay for displaying a Texture.
    /// </summary>
    public class TextureOverlay : Overlay
    {
        #region Fields

        private TextureFrame currentFrame;
        private RectangleF currentUV;
        private Quad quad;
        private Size size;
        private Texture texture;
        private TimeSpan textureAnimationTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextureOverlay class.
        /// </summary>
        public TextureOverlay()
            : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TextureOverlay class.
        /// </summary>
        /// <param name="name">The name of the TextureOverlay.</param>
        public TextureOverlay(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size Size
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
                    this.OnBoundsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Texture.
        /// </summary>
        public Texture Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.texture != value)
                {
                    this.texture = value;
                    this.DestroyElement();
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the current frame.
        /// </summary>
        protected TextureFrame CurrentFrame
        {
            get
            {
                return this.currentFrame;
            }

            private set
            {
                if (this.currentFrame != value)
                {
                    this.currentFrame = value;
                    this.CurrentUV = value.UV;
                    this.Sprite.SetTexture(value.FileName);
                    this.Sprite.SetUV(value.UV.GetPoints());
                }
            }
        }

        /// <summary>
        /// Gets the current uv-coordinates.
        /// </summary>
        protected RectangleF CurrentUV
        {
            get
            {
                return this.currentUV;
            }

            private set
            {
                if (this.currentUV != value)
                {
                    this.currentUV = value;
                    this.Sprite.SetUV(value.GetPoints());
                }
            }
        }

        /// <summary>
        /// Gets the pivot point.
        /// </summary>
        protected override Point Pivot
        {
            get
            {
                return new Point(this.Rectangle.Left + (this.Rectangle.Width / 2), this.Rectangle.Top + (this.Rectangle.Height / 2));
            }
        }

        /// <summary>
        /// Gets a Rectangle representing the bounds of the TextureOverlay.
        /// </summary>
        protected Rectangle Rectangle
        {
            get
            {
                return new Rectangle(this.Location, this.size);
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Updates the TextureOverlay.
        /// </summary>
        public override void Update()
        {
            if (this.Sprite == null && this.texture != null)
            {
                Size viewportSize = this.ViewportSize;
                this.quad = new Quad(this.Rectangle.ToScreenCoordinates(viewportSize));
                this.Sprite = new TwoDSprite(this.Layer.SpriteRenderer, this.quad)
                              {
                                  Visible = this.Visible,
                                  Opacity = this.Opacity,
                                  ZOrder = this.ZOrder,
                                  GpuPrograms = this.texture.GpuPrograms
                              };
            }

            if (this.Sprite != null)
            {
                this.CurrentFrame = this.texture.GetFrameFromTime(this.textureAnimationTime);
                this.textureAnimationTime += this.Layer.TwoDManager.MiyagiSystem.TimeSinceLastUpdate;
                this.CurrentUV = this.texture.GetScrollVectorOffset(this.currentUV, this.Layer.TwoDManager.MiyagiSystem.TimeSinceLastUpdate.TotalMilliseconds);
            }

            base.Update();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Handles bounds changes.
        /// </summary>
        protected override void OnBoundsChanged()
        {
            base.OnBoundsChanged();

            if (this.quad != null)
            {
                this.quad.SetBounds(this.Rectangle);
                if (this.Layer != null)
                {
                    this.Layer.SpriteRenderer.BufferDirty = true;
                }
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}