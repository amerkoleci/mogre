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
    /// <summary>
    /// The style of a EditBoxElement.
    /// </summary>
    public abstract class EditBoxStyle : Style
    {
        #region Fields

        private CaretStyle caretStyle;
        private int maxCharacterLimit;
        private bool readOnly;
        private bool useCaret;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextBoxStyle class.
        /// </summary>
        protected EditBoxStyle()
        {
            this.UseCaret = true;
            this.caretStyle = new CaretStyle();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the style of the caret.
        /// </summary>
        public CaretStyle CaretStyle
        {
            get
            {
                return this.caretStyle;
            }

            set
            {
                if (this.caretStyle != value)
                {
                    this.OnPropertyChanging("CaretStyle");
                    this.caretStyle = value;
                    this.OnPropertyChanged("CaretStyle");
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount of characters.
        /// </summary>
        public int MaxCharacterLimit
        {
            get
            {
                return this.maxCharacterLimit;
            }

            set
            {
                if (this.maxCharacterLimit != value)
                {
                    this.OnPropertyChanging("MaxCharacterLimit");
                    this.maxCharacterLimit = value;
                    this.OnPropertyChanged("MaxCharacterLimit");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can modify the text of the TextBox.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }

            set
            {
                if (this.readOnly != value)
                {
                    this.OnPropertyChanging("ReadOnly");
                    this.readOnly = value;
                    this.OnPropertyChanged("ReadOnly");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TextBox should use a caret.
        /// </summary>
        /// <value><c>true</c> if the TextBox uses a caret; otherwise, <c>false</c>.</value>
        public bool UseCaret
        {
            get
            {
                return this.useCaret;
            }

            set
            {
                if (this.useCaret != value)
                {
                    this.OnPropertyChanging("UseCaret");
                    this.useCaret = value;
                    this.OnPropertyChanged("UseCaret");
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
            this.caretStyle.Resize(widthFactor, heightFactor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}