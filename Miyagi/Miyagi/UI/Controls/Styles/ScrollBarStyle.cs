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

    using Miyagi.Common;

    /// <summary>
    /// The style of a ScrollBarElement.
    /// </summary>
    public sealed class ScrollBarStyle : Style
    {
        #region Fields

        private bool alwaysVisible;
        private BorderStyle borderStyle;
        private int extent;
        private bool showButtons;
        private ThumbStyle thumbStyle;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScrollBarStyle class.
        /// </summary>
        public ScrollBarStyle()
        {
            this.thumbStyle = new ThumbStyle();
            this.borderStyle = new BorderStyle();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the scrollbar is always visible.
        /// </summary>
        public bool AlwaysVisible
        {
            get
            {
                return this.alwaysVisible;
            }

            set
            {
                if (this.alwaysVisible != value)
                {
                    this.OnPropertyChanging("AlwaysVisible");
                    this.alwaysVisible = value;
                    this.OnPropertyChanged("AlwaysVisible");
                }
            }
        }

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
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
        /// Gets or sets the extent of the scrollbar.
        /// </summary>
        public int Extent
        {
            get
            {
                return this.extent;
            }

            set
            {
                if (this.extent != value)
                {
                    this.OnPropertyChanging("Extent");
                    this.extent = value;
                    this.OnPropertyChanged("Extent");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the scrollbar buttons.
        /// </summary>
        public bool ShowButtons
        {
            get
            {
                return this.showButtons;
            }

            set
            {
                if (this.showButtons != value)
                {
                    this.OnPropertyChanging("ShowButtons");
                    this.showButtons = value;
                    this.OnPropertyChanged("ShowButtons");
                }
            }
        }

        /// <summary>
        /// Gets or sets the thumb of the scrollbar.
        /// </summary>
        public ThumbStyle ThumbStyle
        {
            get
            {
                return this.thumbStyle;
            }

            set
            {
                if (this.thumbStyle != value)
                {
                    this.OnPropertyChanging("ThumbStyle");
                    this.thumbStyle = value;
                    this.OnPropertyChanged("ThumbStyle");
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
            if (this.thumbStyle.Orientation == Orientation.Vertical)
            {
                this.extent = (int)Math.Round(this.extent * widthFactor);
            }
            else
            {
                this.extent = (int)Math.Round(this.extent * heightFactor);
            }

            this.borderStyle.Resize(widthFactor, heightFactor);
            this.thumbStyle.Resize(widthFactor, heightFactor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}