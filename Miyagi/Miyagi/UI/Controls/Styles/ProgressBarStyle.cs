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
    using Miyagi.Common.Data;

    /// <summary>
    /// The style of a ProgressBarElement.
    /// </summary>
    public sealed class ProgressBarStyle : Style
    {
        #region Fields

        private int blockExtent;
        private TimeSpan marqueeAnimationTime;
        private ProgressBarMode mode;
        private Point offset;
        private Orientation orient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BarStyle class.
        /// </summary>
        public ProgressBarStyle()
        {
            this.Mode = ProgressBarMode.Continuous;
            this.blockExtent = 1;
            this.marqueeAnimationTime = TimeSpan.FromMilliseconds(2000);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the extent of the individual blocks.
        /// </summary>
        /// <remarks>This is only used when the <see cref="Mode"/> is <see cref="ProgressBarMode.Blocks"/>.</remarks>
        /// <exception cref="ArgumentException">BlockWidth cannot be smaller than one.</exception>
        public int BlockExtent
        {
            get
            {
                return this.blockExtent;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("BlockExtent cannot be smaller than one.", "value");
                }

                if (this.blockExtent != value)
                {
                    this.OnPropertyChanging("BlockExtent");
                    this.blockExtent = value;
                    this.OnPropertyChanged("BlockExtent");
                }
            }
        }

        /// <summary>
        /// Gets or sets the time it takes for the bar to scroll.
        /// </summary>
        /// <remarks>This is only used when the <see cref="Mode"/> is BarMode.Marquee.</remarks>
        public TimeSpan MarqueeAnimationTime
        {
            get
            {
                return this.marqueeAnimationTime;
            }

            set
            {
                if (this.marqueeAnimationTime != value)
                {
                    this.OnPropertyChanging("MarqueeAnimationTime");
                    this.marqueeAnimationTime = value;
                    this.OnPropertyChanged("MarqueeAnimationTime");
                }
            }
        }

        /// <summary>
        /// Gets or sets the ProgressBarMode of the ProgressBar.
        /// </summary>
        public ProgressBarMode Mode
        {
            get
            {
                return this.mode;
            }

            set
            {
                if (this.mode != value)
                {
                    this.OnPropertyChanging("Mode");
                    this.mode = value;
                    this.OnPropertyChanged("Mode");
                }
            }
        }

        /// <summary>
        /// Gets or sets the offset of the bar.
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
                    this.OnPropertyChanging("Offset");
                    this.offset = value;
                    this.OnPropertyChanged("Offset");
                }
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this.orient;
            }

            set
            {
                if (this.orient != value)
                {
                    this.OnPropertyChanging("Orientation");
                    this.orient = value;
                    this.OnPropertyChanged("Orientation");
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
            if (this.Orientation == Orientation.Horizontal)
            {
                this.blockExtent = (int)Math.Round(this.blockExtent * widthFactor);
            }
            else
            {
                this.blockExtent = (int)Math.Round(this.blockExtent * heightFactor);
            }

            this.ResizeHelper.Scale(ref this.offset);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}