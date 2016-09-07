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
    using System;

    using Miyagi.Common.Data;

    /// <summary>
    /// The style of a CaretElement.
    /// </summary>
    public sealed class CaretStyle : Style
    {
        #region Fields

        private TimeSpan blinkInterval;
        private ColourDefinition colour;
        private Size size;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CaretStyle class.
        /// </summary>
        public CaretStyle()
        {
            this.Colour = Colours.Black;
            this.blinkInterval = TimeSpan.FromMilliseconds(800);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the blink interval.
        /// </summary>
        public TimeSpan BlinkInterval
        {
            get
            {
                return this.blinkInterval;
            }

            set
            {
                if (this.blinkInterval != value)
                {
                    this.OnPropertyChanging("BlinkInterval");
                    this.blinkInterval = value;
                    this.OnPropertyChanged("BlinkInterval");
                }
            }
        }

        /// <summary>
        /// Gets or sets the colour of the CaretElement.
        /// </summary>
        public ColourDefinition Colour
        {
            get
            {
                return this.colour;
            }

            set
            {
                if (this.colour != value)
                {
                    this.OnPropertyChanging("Colour");
                    this.colour = value;
                    this.OnPropertyChanged("Colour");
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the CaretElement.
        /// </summary>
        /// <value>A Size representing the size of the CaretElement.</value>
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
        }

        #endregion Protected Methods

        #endregion Methods
    }
}