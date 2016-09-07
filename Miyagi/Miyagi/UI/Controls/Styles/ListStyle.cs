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

    /// <summary>
    /// The common style of all items of a ListBox.
    /// </summary>
    public sealed class ListStyle : Style
    {
        #region Fields

        private Alignment alignment;
        private Font font;
        private ColourDefinition foregroundColour;
        private ColourDefinition hoverColour;
        private Point itemOffset;
        private int maxVisibleItems;
        private bool multiSelect;
        private ScrollBarStyle scrollBarStyle;
        private ColourDefinition selectColour;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListBoxStyle class.
        /// </summary>
        public ListStyle()
        {
            this.selectColour = Colours.Red;
            this.hoverColour = Colours.Blue;
            this.foregroundColour = Colours.Black;
            this.scrollBarStyle = new ScrollBarStyle();
            this.Font = Font.Default;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the alignment of the items.
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
        /// Gets or sets the font of the items.
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
        /// Gets or sets the foreground colour of the items.
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
        /// Gets or sets the colour of the hovered item.
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
        /// Gets or sets the offset of the items.
        /// </summary>
        public Point ItemOffset
        {
            get
            {
                return this.itemOffset;
            }

            set
            {
                if (this.itemOffset != value)
                {
                    this.OnPropertyChanging("ItemOffset");
                    this.itemOffset = value;
                    this.OnPropertyChanged("ItemOffset");
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of visible items.
        /// </summary>
        public int MaxVisibleItems
        {
            get
            {
                return this.maxVisibleItems;
            }

            set
            {
                if (this.maxVisibleItems != value)
                {
                    this.OnPropertyChanging("MaxVisibleItems");
                    this.maxVisibleItems = value;
                    this.OnPropertyChanged("MaxVisibleItems");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multiple items can be selected.
        /// </summary>
        public bool MultiSelect
        {
            get
            {
                return this.multiSelect;
            }

            set
            {
                if (this.multiSelect != value)
                {
                    this.OnPropertyChanging("MultiSelect");
                    this.multiSelect = value;
                    this.OnPropertyChanged("MultiSelect");
                }
            }
        }

        /// <summary>
        /// Gets or sets the style of the scrollbar.
        /// </summary>
        public ScrollBarStyle ScrollBarStyle
        {
            get
            {
                return this.scrollBarStyle;
            }

            set
            {
                if (this.scrollBarStyle != value)
                {
                    this.OnPropertyChanging("ScrollBarStyle");
                    this.scrollBarStyle = value;
                    this.OnPropertyChanged("ScrollBarStyle");
                }
            }
        }

        /// <summary>
        /// Gets or sets the colour of selected items.
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
            this.ResizeHelper.Scale(ref this.itemOffset);
            this.scrollBarStyle.Resize(widthFactor, heightFactor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}