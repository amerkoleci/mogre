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
namespace Miyagi.UI
{
    using Miyagi.Common.Rendering;
    using Miyagi.UI.Controls;

    /// <summary>
    /// Enables a class to be a modal dialog.
    /// </summary>
    public interface IModalDialog
    {
        #region Properties

        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        ISpriteRenderer SpriteRenderer
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns the topmost control at the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position where you want to look for a control.</param>
        /// <param name="y">The y-coordinate of the position where you want to look for a control.</param>
        /// <returns>If there is a control at the position the topmost; otherwise, null.</returns>
        Control GetTopControlAt(int x, int y);

        /// <summary>
        /// Signals that the mouse has left.
        /// </summary>
        void InjectMouseLeave();

        /// <summary>
        /// Signals that the mouse has been moved.
        /// </summary>
        void InjectMouseMoved();

        /// <summary>
        /// Updated the modal dialog.
        /// </summary>
        void Update();

        bool CanReactToInput { get; }

        #endregion Methods
    }
}