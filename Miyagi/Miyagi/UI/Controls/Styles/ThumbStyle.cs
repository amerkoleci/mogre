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
namespace Miyagi.UI.Controls.Styles
{
    using Miyagi.Common;
    using Miyagi.Common.Data;

    /// <summary>
    /// The style of a ThumbElement.
    /// </summary>
    public sealed class ThumbStyle : Style
    {
        #region Fields

        private bool autoSize;
        private BorderStyle borderStyle;
        private Orientation orientation;
        private Size size;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ThumbStyle class.
        /// </summary>
        public ThumbStyle()
        {
            this.borderStyle = new BorderStyle();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the thumb should be resized according to the Min and Max values.
        /// </summary>
        /// <value>If set to true, the thumb resizes if Max or Min has changed. Default is false.</value>
        public bool AutoSize
        {
            get
            {
                return this.autoSize;
            }

            set
            {
                if (this.autoSize != value)
                {
                    this.OnPropertyChanging("AutoSize");
                    this.autoSize = value;
                    this.OnPropertyChanged("AutoSize");
                }
            }
        }

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>A BorderElement representing the border of the control.</value>
        public BorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }

            set
            {
                if (this.borderStyle != value)
                {
                    this.OnPropertyChanging("BorderStyle");
                    this.borderStyle = value;
                    this.OnPropertyChanged("BorderStyle");
                }
            }
        }

        /// <summary>
        /// Gets or sets the orientation of the ThumbElement.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                if (this.orientation != value)
                {
                    this.OnPropertyChanging("Orientation");
                    this.orientation = value;
                    this.OnPropertyChanged("Orientation");
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the thumb.
        /// </summary>
        /// <value>The height in pixels.</value>
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
                    this.OnPropertyChanging("Size");
                    this.size = value;
                    this.OnPropertyChanged("Size");
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Resizes the style.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            this.ResizeHelper.Scale(ref this.size);
            this.borderStyle.Resize(widthFactor, heightFactor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}