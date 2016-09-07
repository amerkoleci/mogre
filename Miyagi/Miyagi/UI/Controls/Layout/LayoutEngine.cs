/*
// Copyright (c) 2009 Realmforge Studios GmbH.
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Author: Mario Fernandez
// Created: 3/26/2009 11:48:10 AM
 */
namespace Miyagi.UI.Controls.Layout
{
    /// <summary>
    /// Provides the base class for implementing layout engines.
    /// </summary>
    public abstract class LayoutEngine
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Initializes the layout engine.
        /// </summary>
        /// <param name="child">The container on which the layout engine will operate.</param>
        /// <param name="specified">The bounds defining the container's size and position.</param>
        public virtual void InitLayout(Control child, BoundsSpecified specified)
        {
        }

        /// <summary>
        /// Requests that the layout engine perform a layout operation.
        /// </summary>
        /// <param name="parent">The container on which the layout engine will operate.</param>
        /// <param name="layoutEventArgs">An event argument from a Control.Layout event.</param>
        /// <returns><c>true</c> if layout should be performed again by the parent of container; otherwise, <c>false</c>.</returns>
        public virtual bool Layout(Control parent, LayoutEventArgs layoutEventArgs)
        {
            return false;
        }

        #endregion Public Methods

        #endregion Methods
    }
}