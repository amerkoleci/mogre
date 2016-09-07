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
    using Miyagi.Common.Data;

    /// <summary>
    /// The style of a BorderElement.
    /// </summary>
    public sealed class BorderStyle : Style
    {
        #region Fields

        private Thickness thickness;
        private RectangleF uv;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BorderStyle class.
        /// </summary>
        public BorderStyle()
        {
            this.uv = RectangleF.FromLTRB(0.25f, 0.25f, 0.75f, 0.75f);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        public Thickness Thickness
        {
            get
            {
                return this.thickness;
            }

            set
            {
                if (this.thickness != value)
                {
                    this.OnPropertyChanging("Thickness");
                    this.thickness = value;
                    this.OnPropertyChanged("Thickness");
                }
            }
        }

        /// <summary>
        /// Gets or sets the UV-Coordinates used by the BorderElement.
        /// </summary>
        /// <value>A RectangleF representing the UV-coordinates used by the BorderElement. Default is (0.25, 0.25, 0.75, 0.75).</value>
        /// <remarks>These UV-coordinates represent the inner rectangle of a border texture, which is not used for the actual border.</remarks>
        public RectangleF UV
        {
            get
            {
                return this.uv;
            }

            set
            {
                if (this.uv != value)
                {
                    this.OnPropertyChanging("UV");
                    this.uv = value;
                    this.OnPropertyChanged("UV");
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
            this.ResizeHelper.Scale(ref this.thickness);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}