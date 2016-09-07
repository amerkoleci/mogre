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
namespace Miyagi.UI.Controls.Elements
{
    /// <summary>
    /// Enables a class the be the parent of a ListBoxElement.
    /// </summary>
    public interface IListElementOwner : IInteractiveElementOwner
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the default key commands are enabled.
        /// </summary>
        bool DefaultKeysEnabled
        {
            get;
        }

        /// <summary>
        /// Gets the collection of items.
        /// </summary>
        ListItemCollection Items
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the items should be cropped.
        /// </summary>
        bool ListBoxCroppingDisabled
        {
            get;
        }

        /// <summary>
        /// Gets the total height available for items.
        /// </summary>
        int ListBoxHeight
        {
            get;
        }

        /// <summary>
        /// Gets the fixed vertical offset.
        /// </summary>
        int ListBoxVerticalOffset
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the scrollbar should be displayed.
        /// </summary>
        bool ShouldShowScrollBar
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Deselects all specified items.
        /// </summary>
        /// <param name="items">The items to deselect.</param>
        void DeselectItems(params ListItem[] items);

        /// <summary>
        /// Selects all specified items.
        /// </summary>
        /// <param name="items">The items to select.</param>
        void SelectItems(params ListItem[] items);

        #endregion Methods
    }
}