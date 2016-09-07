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
    using Miyagi.Common.Data;

    /// <summary>
    /// The abstract base class for 2D overlays.
    /// </summary>
    public abstract class Overlay : LayerElement
    {
        #region Fields

        private Point location;
        private float opacity;
        private int rotation;
        private PointF scale;
        private PointF skew;
        private bool transformationNeedsUpdate;
        private Point translation;
        private bool visible;
        private int zorder;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Overlay class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected Overlay(string name)
            : base(name)
        {
            this.visible = true;
            this.opacity = 1;
            this.scale = new PointF(1, 1);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }

            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.OnBoundsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        public float Opacity
        {
            get
            {
                return this.opacity;
            }

            set
            {
                if (this.opacity != value)
                {
                    this.opacity = value;
                    this.OnOpacityChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation in degrees.
        /// </summary>
        public int Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                if (this.rotation != value)
                {
                    if (value >= 360)
                    {
                        value = 0;
                    }

                    this.rotation = value;
                    this.transformationNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public PointF Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                if (this.scale != value)
                {
                    if (value.X < 0)
                    {
                        value = new PointF(0, value.Y);
                    }

                    if (value.Y < 0)
                    {
                        value = new PointF(value.X, 0);
                    }

                    this.scale = value;
                    this.transformationNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal scale.
        /// </summary>
        public float ScaleX
        {
            get
            {
                return this.scale.X;
            }

            set
            {
                this.Scale = new PointF(value, this.scale.Y);
            }
        }

        /// <summary>
        /// Gets or sets the vertical scale.
        /// </summary>
        public float ScaleY
        {
            get
            {
                return this.scale.Y;
            }

            set
            {
                this.Scale = new PointF(this.scale.X, value);
            }
        }

        /// <summary>
        /// Gets or sets the skew.
        /// </summary>
        public PointF Skew
        {
            get
            {
                return this.skew;
            }

            set
            {
                if (this.skew != value)
                {
                    if (value.X < 0)
                    {
                        value = new PointF(0, value.Y);
                    }

                    if (value.Y < 0)
                    {
                        value = new PointF(value.X, 0);
                    }

                    this.skew = value;
                    this.transformationNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal skew.
        /// </summary>
        public float SkewX
        {
            get
            {
                return this.skew.X;
            }

            set
            {
                this.Skew = new PointF(value, this.skew.Y);
            }
        }

        /// <summary>
        /// Gets or sets the vertical skew.
        /// </summary>
        public float SkewY
        {
            get
            {
                return this.skew.Y;
            }

            set
            {
                this.Skew = new PointF(this.skew.X, value);
            }
        }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        public Point Translation
        {
            get
            {
                return this.translation;
            }

            set
            {
                if (this.translation != value)
                {
                    this.translation = value;
                    this.transformationNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal translation.
        /// </summary>
        public int TranslationX
        {
            get
            {
                return this.translation.X;
            }

            set
            {
                this.Translation = new Point(value, this.translation.Y);
            }
        }

        /// <summary>
        /// Gets or sets the vertical translation.
        /// </summary>
        public int TranslationY
        {
            get
            {
                return this.translation.Y;
            }

            set
            {
                this.Translation = new Point(this.translation.X, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Overlay is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.visible;
            }

            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Z-order.
        /// </summary>
        public int ZOrder
        {
            get
            {
                return this.zorder;
            }

            set
            {
                if (this.zorder != value)
                {
                    this.zorder = value;
                    if (this.Sprite != null)
                    {
                        this.Sprite.ZOrder = value;
                    }
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the pivot point.
        /// </summary>
        protected abstract Point Pivot
        {
            get;
        }

        /// <summary>
        /// Gets or sets the sprite.
        /// </summary>
        protected TwoDSprite Sprite
        {
            get;
            set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Updates the Overlay.
        /// </summary>
        public override void Update()
        {
            if (this.Sprite != null)
            {
                if (this.transformationNeedsUpdate)
                {
                    this.Sprite.Rotate(this.rotation, this.Pivot);
                    this.Sprite.Scale(this.scale, this.Pivot);
                    this.Sprite.Translate(this.translation);
                    this.Sprite.Skew(this.skew);
                    this.Sprite.ApplyTransformation();
                    this.transformationNeedsUpdate = false;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Destroys the Overlay.
        /// </summary>
        protected override void DestroyElement()
        {
            if (this.Sprite != null)
            {
                this.Sprite.RemoveFromRenderer();
                this.Sprite = null;
            }
        }

        /// <summary>
        /// Handles bounds changes.
        /// </summary>
        protected virtual void OnBoundsChanged()
        {
        }

        /// <summary>
        /// Handles opacity changes.
        /// </summary>
        protected virtual void OnOpacityChanged()
        {
            if (this.Sprite != null)
            {
                this.Sprite.Opacity = this.Opacity;
            }
        }

        /// <summary>
        /// Handles visibility changes.
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            this.Sprite.Visible = this.Visible;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}