/* Permission is hereby granted, free of charge, to any person obtaining
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
// Copyright (c) 2004 Novell, Inc.
//
// Authors:
// Peter Bartok pbartok@novell.com
//
 */
namespace Miyagi.UI.Controls.Layout
{
    using System;

    /// <summary>
    /// EventArgs for changes in the layout.
    /// </summary>
    public sealed class LayoutEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LayoutEventArgs class.
        /// </summary>
        /// <param name="affectedControl">The affected control.</param>
        /// <param name="affectedProperty">The affected property.</param>
        public LayoutEventArgs(Control affectedControl, string affectedProperty)
        {
            this.AffectedControl = affectedControl;
            this.AffectedProperty = affectedProperty;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the affected control.
        /// </summary>
        /// <value>A Control representing the affected control.</value>
        public Control AffectedControl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the affected property as a string.
        /// </summary>
        /// <value>A string representing the affected property.</value>
        public string AffectedProperty
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}