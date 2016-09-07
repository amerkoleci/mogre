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
namespace Miyagi.Common.Scripting
{
    using System;

    /// <summary>
    /// EventArgs for the BindEventsRequested event.
    /// </summary>
    public class BindEventsEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BindEventsEventArgs class.
        /// </summary>
        /// <param name="scriptingScheme">The affected ScriptingScheme.</param>
        public BindEventsEventArgs(ScriptingScheme scriptingScheme)
        {
            this.ScriptingScheme = scriptingScheme;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the event has been handled.
        /// </summary>
        /// <value><c>true</c> if the event has been handled; otherwise, <c>false</c>.</value>
        public bool Handled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the affected ScriptingScheme.
        /// </summary>
        /// <value>The affected ScriptingScheme.</value>
        public ScriptingScheme ScriptingScheme
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}