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
namespace Miyagi.Common.Plugins
{
    /// <summary>
    /// The abstract base class for plugins.
    /// </summary>
    public abstract class Plugin
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Plugin class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected Plugin(MiyagiSystem system)
        {
            this.MiyagiSystem = system;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value>A string representing the name.</value>
        public abstract string Name
        {
            get;
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        /// <value>The MiyagiSystem.</value>
        protected MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Is called when the plugin is loaded.
        /// </summary>
        /// <param name="args">Optional array of load arguments.</param>
        public abstract void NotifyLoaded(params object[] args);

        /// <summary>
        /// Is called when the plugin is unloaded.
        /// </summary>
        public abstract void NotifyUnloaded();

        #endregion Public Methods

        #endregion Methods
    }
}