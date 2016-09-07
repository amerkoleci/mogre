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
    using System.ComponentModel;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Resources;
    using Miyagi.Common.Serialization;

    /// <summary>
    /// The style of a TextElement.
    /// </summary>
    public sealed class TextStyle : Style
    {
        #region Fields

        private Alignment alignment;
        private Font font;
        private ColourDefinition foregroundColour;
        private bool multiline;
        private Point offset;
        private ColourDefinition selectionBackgroundColour;
        private bool wordWrap;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextStyle class.
        /// </summary>
        public TextStyle()
        {
            this.font = Font.Default;
            this.alignment = Alignment.MiddleLeft;
            this.selectionBackgroundColour = Colours.Grey;
            this.foregroundColour = Colours.Black;
            this.multiline = true;
            this.wordWrap = true;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>The alignment of the text.</value>
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
        /// Gets or sets the font.
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

                    if (this.font != null)
                    {
                        this.font.PropertyChanged -= this.FontPropertyChanged;
                    }

                    this.font = value;

                    if (this.font != null)
                    {
                        this.font.PropertyChanged += this.FontPropertyChanged;
                    }

                    this.OnPropertyChanged("Font");
                }
            }
        }

        /// <summary>
        /// Gets or sets the foreground colour of the text.
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
        /// Gets or sets a value indicating whether the TextElement can extend more than one line.
        /// </summary>
        /// <value><c>true</c> if the the TextElement can extend more than one line; otherwise, <c>false</c>.</value>
        public bool Multiline
        {
            get
            {
                return this.multiline;
            }

            set
            {
                if (this.multiline != value)
                {
                    this.OnPropertyChanging("Multiline");
                    this.multiline = value;
                    this.OnPropertyChanged("Multiline");
                }
            }
        }

        /// <summary>
        /// Gets or sets the offset of the text.
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
        /// Gets or sets the background colour of selected text.
        /// </summary>
        public ColourDefinition SelectionBackgroundColour
        {
            get
            {
                return this.selectionBackgroundColour;
            }

            set
            {
                if (this.selectionBackgroundColour != value)
                {
                    this.OnPropertyChanging("SelectionBackgroundColour");
                    this.selectionBackgroundColour = value;
                    this.OnPropertyChanged("SelectionBackgroundColour");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether words should be wrapped to the next line when necessary.
        /// </summary>
        public bool WordWrap
        {
            get
            {
                return this.wordWrap;
            }

            set
            {
                if (this.wordWrap != value)
                {
                    this.OnPropertyChanging("WordWrap");
                    this.wordWrap = value;
                    this.OnPropertyChanged("WordWrap");
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public TextStyle Clone()
        {
            TextStyle result = new TextStyle();
            result.alignment = alignment;
            result.font = font;
            result.foregroundColour = foregroundColour;
            result.multiline = multiline;
            result.offset = offset;
            result.selectionBackgroundColour = selectionBackgroundColour;
            result.wordWrap = wordWrap;
            return result;
        }

        #endregion Public Methods
        
        #region Protected Methods

        /// <summary>
        /// Resizes the style.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            this.ResizeHelper.Scale(ref this.offset);
        }

        #endregion Protected Methods

        #region Private Methods

        private void FontPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("FontProperty");
        }

        #endregion Private Methods

        #endregion Methods
    }
}