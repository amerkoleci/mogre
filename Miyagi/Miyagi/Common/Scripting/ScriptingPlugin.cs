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
    using Miyagi.Common.Plugins;

    /// <summary>
    /// The base class for scripting plugins.
    /// </summary>
    public abstract class ScriptingPlugin : Plugin
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScriptingPlugin class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected ScriptingPlugin(MiyagiSystem system)
            : base(system)
        {
        }

        #endregion Constructors

        #region Properties

        #region Protected Properties

        /// <summary>
        /// Gets the name of the scripting language.
        /// </summary>
        protected abstract string Language
        {
            get;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Is called when the plugin is loaded.
        /// </summary>
        /// <param name="args">Optional array of load arguments.</param>
        public override void NotifyLoaded(params object[] args)
        {
            this.MiyagiSystem.ScriptingManager.BindEventsRequested += this.OnBindEventsRequested;
        }

        /// <summary>
        /// Is called when the plugin is unloaded.
        /// </summary>
        public override void NotifyUnloaded()
        {
            this.MiyagiSystem.ScriptingManager.BindEventsRequested -= this.OnBindEventsRequested;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Binds the events.
        /// </summary>
        /// <param name="scheme">The ScriptingScheme.</param>
        protected abstract void BindEvents(ScriptingScheme scheme);

        #endregion Protected Methods

        #region Private Methods

        private void OnBindEventsRequested(object sender, BindEventsEventArgs e)
        {
            if (!e.Handled && string.Compare(this.Language, e.ScriptingScheme.Language, true) == 0)
            {
                this.BindEvents(e.ScriptingScheme);
                e.Handled = true;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}