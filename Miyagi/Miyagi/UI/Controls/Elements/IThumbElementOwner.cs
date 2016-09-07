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
    using Miyagi.Common.Data;

    /// <summary>
    /// Enables a class the be the parent of a ThumbElement.
    /// </summary>
    public interface IThumbElementOwner<T> : IInteractiveElementOwner
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the IThumbElementOwner is inverted.
        /// </summary>
        /// <value>If true Min is the upmost / rightmost point and Max is the downmost / Leftmost point of the IThumbElementOwner.</value>
        bool Inverted
        {
            get;
        }

        /// <summary>
        /// Gets the value which it added or subtracted if PageUp or PageDown is pressed.
        /// </summary>
        T LargeChange
        {
            get;
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        T Max
        {
            get;
        }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        T Min
        {
            get;
        }

        /// <summary>
        /// Gets the thumb bounds.
        /// </summary>
        Rectangle ThumbBounds
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value of the IValueElementOwner.
        /// </summary>
        T Value
        {
            get;
            set;
        }

        #endregion Properties
    }
}