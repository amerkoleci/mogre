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

    /// <summary>
    /// A GUI that block all other GUIs when it's shown.
    /// </summary>
    public class ModalGUI : GUI, IModalDialog
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ModalGUI class.
        /// </summary>
        public ModalGUI()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ModalGUI class.
        /// </summary>
        /// <param name="name">The name of the ModalGUI.</param>
        public ModalGUI(string name)
            : base(name)
        {
            this.Visible = false;
        }

        public bool CanReactToInput { get; set; }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Releases the unmanaged resources used by the GUI and disposed all its controls.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            this.Pop();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the VisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                this.Push();
            }
            else
            {
                this.Pop();
            }
        }

        /// <summary>
        /// Pops this instance.
        /// </summary>
        protected void Pop()
        {
            var guiMgr = this.GUIManager;
            if (guiMgr != null)
            {
                guiMgr.PopModalDialog(this);
            }
        }

        /// <summary>
        /// Pushes this instance.
        /// </summary>
        protected void Push()
        {
            var guiMgr = this.GUIManager;
            if (guiMgr != null)
            {
                if (guiMgr.GUIs.Contains(this))
                {
                    guiMgr.PushModalDialog(this);
                }
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}