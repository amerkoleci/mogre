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
    /// Enables a class the be the parent of a EditBoxElement.
    /// </summary>
    public interface IEditBoxElementOwner : ITextElementOwner
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the owner has the focus.
        /// </summary>
        bool Focused
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether a key is currently down.
        /// </summary>
        bool IsKeyDown
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is over the control.
        /// </summary>
        bool IsMouseOver
        {
            get;
        }

        /// <summary>
        /// Gets the TextElement.
        /// </summary>
        TextElement TextElement
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetText(string text);

        #endregion Methods
    }
}