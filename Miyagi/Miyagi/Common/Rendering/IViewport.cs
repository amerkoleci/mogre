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
namespace Miyagi.Common.Rendering
{
    using Miyagi.Common.Data;

    /// <summary>
    /// Encapsulates a backend viewport.
    /// </summary>
    public interface IViewport
    {
        #region Properties

        /// <summary>
        /// Gets the bounds of the viewport.
        /// </summary>
        Rectangle Bounds
        {
            get;
        }

        /// <summary>
        /// Gets the backend viewport.
        /// </summary>
        object Native
        {
            get;
        }

        /// <summary>
        /// Gets the offset of the viewport.
        /// </summary>
        Point Offset
        {
            get;
        }

        /// <summary>
        /// Gets the size of the viewport.
        /// </summary>
        Size Size
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Updates the bounds of the viewport.
        /// </summary>
        void UpdateBounds();

        #endregion Methods
    }
}