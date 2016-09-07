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
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// The style of a TabBarElement.
    /// </summary>
    public class TabBarStyle : Style
    {
        #region Fields

        private Alignment alignment;
        private int extent;
        private Font font;
        private ColourDefinition foregroundColour;
        private ColourDefinition hoverColour;
        private TabMode mode;
        private ColourDefinition selectColour;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBarStyle"/> class.
        /// </summary>
        public TabBarStyle()
        {
            this.font = Font.Default;
            this.alignment = Alignment.None;
            this.mode = TabMode.AutoSize;
            this.hoverColour = Colours.Blue;
            this.selectColour = Colours.Red;
            this.foregroundColour = Colours.Black;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the alignment of the tab text.
        /// </summary>
        public Alignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                if (this.alignment != value)
                {
                    this.OnPropertyChanging("Alignment");
                    this.alignment = value;
                    this.OnPropertyChanged("Alignment");
                }
            }
        }

        /// <summary>
        /// Gets or sets the extent.
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
        /// Gets or sets the font of the tabs.
        /// </summary>
        [SerializerOptions(Redirect = "Fonts")]
        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                if (this.font != value)
                {
                    this.OnPropertyChanging("Font");
                    this.font = value;
                    this.OnPropertyChanged("Font");
                }
            }
        }

        /// <summary>
        /// Gets or sets the colour of the tabs.
        /// </summary>
        public ColourDefinition ForegroundColour
        {
            get
            {
                return this.foregroundColour;
            }

            set
            {
                if (this.foregroundColour != value)
                {
                    this.OnPropertyChanging("ForegroundColour");
                    this.foregroundColour = value;
                    this.OnPropertyChanged("ForegroundColour");
                }
            }
        }

        /// <summary>
        /// Gets or sets the colour of the hovered tab.
        /// </summary>
        public ColourDefinition HoverColour
        {
            get
            {
                return this.hoverColour;
            }

            set
            {
                if (this.hoverColour != value)
                {
                    this.OnPropertyChanging("HoverColour");
                    this.hoverColour = value;
                    this.OnPropertyChanged("HoverColour");
                }
            }
        }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        public TabMode Mode
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
        /// Gets or sets the colour of the selected tab.
        /// </summary>
        public ColourDefinition SelectColour
        {
            get
            {
                return this.selectColour;
            }

            set
            {
                if (this.selectColour != value)
                {
                    this.OnPropertyChanging("SelectColour");
                    this.selectColour = value;
                    this.OnPropertyChanged("SelectColour");
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
            ResizeHelper.Scale(ref this.extent, heightFactor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}