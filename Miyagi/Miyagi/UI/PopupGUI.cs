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
    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Internals;

    /// <summary>
    /// A GUI that popups when the mouse cursor is between certain coordinates.
    /// </summary>
    public class PopupGUI : GUI
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PopupGUI class.
        /// </summary>
        public PopupGUI()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PopupGUI class.
        /// </summary>
        /// <param name="name">The name of the PopupGUI.</param>
        public PopupGUI(string name)
            : base(name)
        {
            this.Visible = false;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the popup orientation.
        /// </summary>
        /// <remarks>If the orientation is horizontal</remarks>
        public Orientation PopupOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the range when the GUI should pop up.
        /// </summary>
        public Range PopupRange
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating whether this instance is popuped.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is popuped; otherwise, <c>false</c>.
        /// </value>
        protected bool IsPopuped
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Resizes the GUI.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            Range range = this.PopupRange;

            switch (this.PopupOrientation)
            {
                case Orientation.Horizontal:
                    ResizeHelper.Scale(ref range, widthFactor);
                    break;
                case Orientation.Vertical:
                    ResizeHelper.Scale(ref range, heightFactor);
                    break;
            }

            this.PopupRange = range;

            base.DoResize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Handles mouse cursor outside the popup range.
        /// </summary>
        protected virtual void OnPopupClosed()
        {
            this.Visible = false;
        }

        /// <summary>
        /// Handles mouse cursor inside the popup range.
        /// </summary>
        protected virtual void OnPopupOpened()
        {
            this.Visible = true;
        }

        /// <summary>
        /// Updates the GUI.
        /// </summary>
        protected override void UpdateCore()
        {
            // popup behaviour
            if (this.GUIManager.IsCursorVisible)
            {
                InputManager iptMgr = this.GUIManager.MiyagiSystem.InputManager;

                // show when within range
                Point mouseLoc = iptMgr.MouseLocation;
                Point offset = this.SpriteRenderer.Viewport.Offset;

                int i = this.PopupOrientation == Orientation.Horizontal
                            ? mouseLoc.X - offset.X
                            : mouseLoc.Y - offset.Y;

                if (i >= this.PopupRange.First && i <= this.PopupRange.Last)
                {
                    if (!this.IsPopuped)
                    {
                        this.IsPopuped = true;
                        this.OnPopupOpened();
                    }
                }
                else if (this.IsPopuped)
                {
                    // hide gui when cursor is not over any control
                    if (this.GetTopControl() == null && this.GUIManager.GrabbedControl == null)
                    {
                        this.IsPopuped = false;
                        this.OnPopupClosed();
                    }
                }
            }

            base.UpdateCore();
        }

        #endregion Protected Methods

        #endregion Methods
    }
}