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
    using System;

    using Miyagi.UI.Controls;

    /// <summary>
    /// EventArgs for drag-and-drop operations.
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DragEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="dragSource">The drag source.</param>
        public DragEventArgs(object data, DragDropEffect effect, Control dragSource)
        {
            this.Data = data;
            this.Effect = effect;
            this.DragSource = dragSource;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the data.
        /// </summary>
        public object Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the drag source.
        /// </summary>
        public Control DragSource
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the effect.
        /// </summary>
        public DragDropEffect Effect
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}