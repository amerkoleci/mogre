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
    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;

    /// <summary>
    /// Base Interface for element owners.
    /// </summary>
    public interface IElementOwner
    {
        #region Properties

        /// <summary>
        /// Gets the display rectangle.
        /// </summary>
        Rectangle DisplayRectangle
        {
            get;
        }

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        MiyagiSystem MiyagiSystem
        {
            get;
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        /// <value>The opacity, ranging between 0 and 1.</value>
        float Opacity
        {
            get;
        }

        /// <summary>
        /// Gets the width and height of the parent.
        /// </summary>
        /// <value>A Size representing the height and width of the parent in pixels.</value>
        Size Size
        {
            get;
        }

        /// <summary>
        /// Gets the SpriteRenderer of the parent.
        /// </summary>
        ISpriteRenderer SpriteRenderer
        {
            get;
        }

        /// <summary>
        /// Gets the texture filtering.
        /// </summary>
        /// <value>A TextureFiltering enum representing the texture filtering.</value>
        TextureFiltering TextureFiltering
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the owner is visible.
        /// </summary>
        bool Visible
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the location of the control relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        Point GetLocationInViewport();

        #endregion Methods
    }
}