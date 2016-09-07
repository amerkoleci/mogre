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
namespace Miyagi.UI.Controls
{
    using System;

    /// <summary>
    /// The abstract base class for state changing buttons.
    /// </summary>
    public class StateButton : Button
    {
        #region Fields

        private string activeState;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the StateButton class.
        /// </summary>
        public StateButton()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the StateButton class.
        /// </summary>
        /// <param name="name">The name of the StateButton.</param>
        public StateButton(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public override string CombinedSkinName
        {
            get
            {
                return !string.IsNullOrEmpty(this.activeState)
                           ? string.Concat(base.CombinedSkinName, ".", this.activeState)
                           : base.CombinedSkinName;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets or sets the active state.
        /// </summary>
        protected string ActiveState
        {
            get
            {
                return this.activeState;
            }

            set
            {
                this.ThrowIfDisposed();

                if (this.activeState != value)
                {
                    this.activeState = value;
                    this.OnActiveStateChanged(EventArgs.Empty);
                }
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Handles active state changes.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnActiveStateChanged(EventArgs e)
        {
            this.ChangeTexture(this.CombinedSkinName, "ActiveStateChanged");
        }

        #endregion Protected Methods

        #endregion Methods
    }
}