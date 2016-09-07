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
    using System;

    /// <summary>
    /// The base class for input plugins.
    /// </summary>
    public abstract class InputPlugin : Plugin
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InputPlugin class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected InputPlugin(MiyagiSystem system)
            : base(system)
        {
            this.InputManager = this.CreateInputManager();
            system.RegisterManager(this.InputManager);
        }

        #endregion Constructors

        #region Properties

        #region Protected Properties

        /// <summary>
        /// Gets the InputManager.
        /// </summary>
        /// <value>The InputManager.</value>
        protected InputManager InputManager
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
        public override void NotifyLoaded(params object[] args)
        {
            this.InputManager.Disposing += this.InputManagerDisposing;
        }

        /// <summary>
        /// Is called when the plugin is unloaded.
        /// </summary>
        public override void NotifyUnloaded()
        {
            this.UnregisterEvents();
        }

        /// <summary>
        /// Sets the keyboard capture device.
        /// </summary>
        /// <param name="keyboard">The keyboard capture device.</param>
        public abstract void SetKeyboardCaptureDevice(object keyboard);

        /// <summary>
        /// Sets the mouse capture device.
        /// </summary>
        /// <param name="mouse">The mouse capture device.</param>
        public abstract void SetMouseCaptureDevice(object mouse);

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Creates the input manager.
        /// </summary>
        /// <returns>The newly created input manager.</returns>
        protected abstract InputManager CreateInputManager();

        /// <summary>
        /// Handles a disposing InputManager.
        /// </summary>
        protected virtual void HandleInputManagerDisposing()
        {
            this.UnregisterEvents();
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected virtual void UnregisterEvents()
        {
            this.InputManager.Disposing -= this.InputManagerDisposing;

            this.SetKeyboardCaptureDevice(null);
            this.SetMouseCaptureDevice(null);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InputManagerDisposing(object sender, EventArgs e)
        {
            this.HandleInputManagerDisposing();
        }

        #endregion Private Methods

        #endregion Methods
    }
}