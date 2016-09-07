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
    /// Enables a class the be the parent of a CaretElement.
    /// </summary>
    public interface ICaretElementOwner : IElementOwner
    {
        #region Properties

        /// <summary>
        /// Gets the position of the caret.
        /// </summary>
        int CaretLocation
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the caret is on the beginning of a new line.
        /// </summary>
        bool IsCaretOnNewLine
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether a key is currently down.
        /// </summary>
        /// <value><c>true</c> if a key is currently down; otherwise, <c>false</c>.</value>
        bool IsKeyDown
        {
            get;
        }

        /// <summary>
        /// Gets the TextElement of the ICaretElementOwner.
        /// </summary>
        /// <value>A TextElement representing the text of the ICaretElementOwner.</value>
        TextElement TextElement
        {
            get;
        }

        /// <summary>
        /// Gets the scroll offset.
        /// </summary>
        int TextScrollOffset
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the number of new lines before the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The number of new lines before the specified index.</returns>
        int GetNumberOfNewLinesBefore(int index);

        #endregion Methods
    }
}