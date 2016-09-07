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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A ScriptMap defines which methods of a script are bound to a event of the specified control.
    /// </summary>
    public class ScriptMap
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScriptMap class.
        /// </summary>
        /// <param name="target">The object which events will be defined.</param>
        public ScriptMap(object target)
        {
            this.Target = target;
            this.EventMethods = new Collection<KeyValuePair<string, string>>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a collection representing the events and methods.
        /// </summary>
        /// <value>A collection representing the events and methods.</value>
        public Collection<KeyValuePair<string, string>> EventMethods
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target object.
        /// </summary>
        public object Target
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Sets a event and its method.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="methodName">The name of the method.</param>
        public void SetEventMethod(string eventName, string methodName)
        {
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(eventName, methodName);

            if (!this.EventMethods.Contains(kvp))
            {
                this.EventMethods.Add(kvp);
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}