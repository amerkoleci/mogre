/*
// Copyright (c) 2009 Realmforge Studios GmbH.
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
// Created: 4/16/2009 3:18:54 PM
 */
namespace Miyagi.Common.Events
{
    using System;

    /// <summary>
    /// Provides data for the Scroll event.
    /// </summary>
    public class ScrollEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScrollEventArgs class.
        /// </summary>
        /// <param name="type">One of the ScrollEventType values.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        public ScrollEventArgs(ScrollEventType type, float newValue)
            : this(type, -1, newValue, Orientation.Horizontal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ScrollEventArgs class.
        /// </summary>
        /// <param name="type">One of the ScrollEventType values.</param>
        /// <param name="oldValue">The old value for the scroll bar.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        public ScrollEventArgs(ScrollEventType type, float oldValue, float newValue)
            : this(type, oldValue, newValue, Orientation.Horizontal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ScrollEventArgs class.
        /// </summary>
        /// <param name="type">One of the ScrollEventType values.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        /// <param name="scroll">One of the ScrollOrientation values.</param>
        public ScrollEventArgs(ScrollEventType type, float newValue, Orientation scroll)
            : this(type, -1, newValue, scroll)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ScrollEventArgs class.
        /// </summary>
        /// <param name="type">One of the ScrollEventType values.</param>
        /// <param name="oldValue">The old value for the scroll bar.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        /// <param name="scroll">One of the ScrollOrientation values.</param>
        public ScrollEventArgs(ScrollEventType type, float oldValue, float newValue, Orientation scroll)
        {
            this.Type = type;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.ScrollOrientation = scroll;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the new Value of the scroll bar.
        /// </summary>
        /// <value>The new value.</value>
        public float NewValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the old Value of the scroll bar.
        /// </summary>
        /// <value>The old value.</value>
        public float OldValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scroll bar orientation that raised the Scroll event.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation ScrollOrientation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of scroll event that occurred.
        /// </summary>
        /// <value>The scoll event.</value>
        public ScrollEventType Type
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}