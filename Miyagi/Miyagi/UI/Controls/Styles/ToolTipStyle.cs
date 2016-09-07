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

    public delegate System.Drawing.Bitmap BitmapCreator(string text, int maximumWidth);    

    /// <summary>
    /// The style of a ToolTipElement.
    /// </summary>
    public sealed class ToolTipStyle : Style
    {
        #region Fields

        private TimeSpan displayDuration;
        private TimeSpan fadeDuration;
        private TimeSpan hoverDuration;
        private Thickness padding;
        private TextStyle textStyle;
        private bool isBitmapTooltip;
        private bool isStandAlone;
        private BitmapCreator bitmapCreatorFunction;
        private int maximumWidth = 250;
        private Point mouseOffset = Point.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ToolTipStyle class.
        /// </summary>
        public ToolTipStyle()
        {
            this.textStyle = new TextStyle();
            this.hoverDuration = TimeSpan.FromMilliseconds(500);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets how long the tooltip is displayed.
        /// </summary>
        public TimeSpan DisplayDuration
        {
            get
            {
                return this.displayDuration;
            }

            set
            {
                if (this.displayDuration != value)
                {
                    this.OnPropertyChanging("DisplayDuration");
                    this.displayDuration = value;
                    this.OnPropertyChanged("DisplayDuration");
                }
            }
        }

        public BitmapCreator BitmapCreatorFunction
        {
            get { return bitmapCreatorFunction; }
            set { bitmapCreatorFunction = value; }
        }

        public int MaximumWidth
        {
            get { return maximumWidth; }
            set { maximumWidth = value; }
        }

        public Point MouseOffset
        {
            get { return mouseOffset; }
            set { mouseOffset = value; }
        }

        public bool IsBitmapTooltip
        {
            get 
            {
                return this.isBitmapTooltip;
            }
            set
            {
                if (this.isBitmapTooltip != value)
                {
                    this.isBitmapTooltip = value;
                }
            }
        }

        public bool IsStandAlone
        {
            get { return isStandAlone; }
            set { isStandAlone = value; }
        }

        /// <summary>
        /// Gets or sets how long the tooltip is fades.
        /// </summary>
        public TimeSpan FadeInDuration
        {
            get
            {
                return this.fadeDuration;
            }

            set
            {
                if (this.fadeDuration != value)
                {
                    this.OnPropertyChanging("FadeDuration");
                    this.fadeDuration = value;
                    this.OnPropertyChanged("FadeDuration");
                }
            }
        }

        /// <summary>
        /// Gets or sets how long the mouse cursor has to hover over the parent before the tooltip is displayed.
        /// </summary>
        public TimeSpan HoverDuration
        {
            get
            {
                return this.hoverDuration;
            }

            set
            {
                if (this.hoverDuration != value)
                {
                    this.OnPropertyChanging("HoverDuration");
                    this.hoverDuration = value;
                    this.OnPropertyChanged("HoverDuration");
                }
            }
        }

        /// <summary>
        /// Gets or sets the inner padding of the tooltip.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return this.padding;
            }

            set
            {
                if (this.padding != value)
                {
                    this.OnPropertyChanging("Padding");
                    this.padding = value;
                    this.OnPropertyChanged("Padding");
                }
            }
        }

        /// <summary>
        /// Gets or sets the style of the text.
        /// </summary>
        public TextStyle TextStyle
        {
            get
            {
                return this.textStyle;
            }

            set
            {
                if (this.textStyle != value)
                {
                    this.OnPropertyChanging("TextStyle");
                    this.textStyle = value;
                    this.OnPropertyChanged("TextStyle");
                }
            }
        }

        #endregion Public Properties

        #endregion Properties
    }
}