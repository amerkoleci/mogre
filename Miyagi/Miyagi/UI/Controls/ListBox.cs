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
namespace Miyagi.UI.Controls
{
    /// <summary>
    /// A ListBox control.
    /// </summary>
    public class ListBox : ListControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListBox class.
        /// </summary>
        public ListBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ListBox class.
        /// </summary>
        /// <param name="name">The name of the ListBox.</param>
        public ListBox(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the items should be cropped.
        /// </summary>
        public override bool ListBoxCroppingDisabled
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the total height available for items.
        /// </summary>
        public override int ListBoxHeight
        {
            get
            {
                return this.Height;
            }
        }

        /// <summary>
        /// Gets the fixed vertical offset.
        /// </summary>
        public override int ListBoxVerticalOffset
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar should be displayed.
        /// </summary>
        public override bool ShouldShowScrollBar
        {
            get
            {
                return this.Items.Count > this.ListStyle.MaxVisibleItems;
            }
        }

        #endregion Public Properties

        #endregion Properties
    }
}