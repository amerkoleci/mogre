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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Resources;

    /// <summary>
    /// A simple 2D overlay for displaying text.
    /// </summary>
    public class TextOverlay : Overlay
    {
        #region Fields

        private ColourDefinition colourDefinition;
        private Font font;
        private Size size;
        private string text;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextOverlay class.
        /// </summary>
        public TextOverlay()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TextOverlay class.
        /// </summary>
        /// <param name="name">The name of the TextOverlay.</param>
        public TextOverlay(string name)
            : base(name)
        {
            this.Font = Font.Default;
            this.colourDefinition = Colours.White;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(this.Location, this.size == Size.Empty ? this.ViewportSize : this.size);
            }

            set
            {
                this.Location = value.Location;

                if (this.size != value.Size)
                {
                    this.size = value.Size;
                    this.DestroyElement();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        public ColourDefinition ColourDefinition
        {
            get
            {
                return this.colourDefinition;
            }

            set
            {
                if (this.colourDefinition != value)
                {
                    this.colourDefinition = value;
                    this.OnColourChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.font != value)
                {
                    if (this.font != null)
                    {
                        this.font.PropertyChanged -= this.TextOverlayPropertyChanged;
                    }

                    this.font = value;
                    this.font.PropertyChanged += this.TextOverlayPropertyChanged;

                    this.DestroyElement();
                }
            }
        }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.DestroyElement();
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the pivot point.
        /// </summary>
        protected override Point Pivot
        {
            get
            {
                Size s = this.font.MeasureString(this.text);
                return new Point(this.Location.X + (s.Width / 2), this.Location.Y + (s.Height / 2));
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Updates the TextOverlay.
        /// </summary>
        public override void Update()
        {
            if (this.Sprite == null && !string.IsNullOrEmpty(this.text) && this.font != null)
            {
                var quads = new TextFormatter(
                    new TextFormatterSettings(
                        Alignment.TopLeft,
                        this.ColourDefinition,
                        this.Font,
                        true,
                        true,
                        this.ViewportSize)).CreateTextQuads(this.Bounds, this.text);

                this.Sprite = new TwoDSprite(this.Layer.SpriteRenderer, quads)
                              {
                                  Visible = this.Visible,
                                  Opacity = this.Opacity,
                                  ZOrder = this.ZOrder
                              };

                this.Sprite.SetColour(this.colourDefinition);
                this.Sprite.SetTexture(this.Font.TextureName);
            }

            base.Update();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the TextOverlay.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.Font != null)
            {
                this.Font.PropertyChanged -= this.TextOverlayPropertyChanged;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnColourChanged()
        {
            if (this.Sprite != null)
            {
                this.Sprite.SetColour(this.colourDefinition);
            }
        }

        private void TextOverlayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.DestroyElement();
        }

        #endregion Private Methods

        #endregion Methods
    }
}