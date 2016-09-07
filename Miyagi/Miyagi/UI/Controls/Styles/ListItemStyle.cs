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
    using Miyagi.Common.Resources;

    /// <summary>
    /// The style of a ListBoxItemElement.
    /// </summary>
    public sealed class ListItemStyle : Style
    {
        #region Fields

        private TextStyle textStyle;
        private Texture texture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListBoxItemStyle class.
        /// </summary>
        public ListItemStyle()
        {
            this.textStyle = new TextStyle();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

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

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public Texture Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                if (this.texture != value)
                {
                    this.OnPropertyChanging("Texture");
                    this.texture = value;
                    this.OnPropertyChanged("Texture");
                }
            }
        }

        #endregion Public Properties

        #endregion Properties
    }
}