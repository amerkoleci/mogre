/*
// FlowLayoutSettings.cs
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
// Copyright (c) 2006 Jonathan Pobst
//
// Authors:
//    Jonathan Pobst (monkey@jpobst.com)
 */
namespace Miyagi.UI.Controls.Layout
{
    using System.Collections.Generic;

    /// <summary>
    /// Collects the characteristics associated with flow layouts.
    /// </summary>
    public class FlowLayoutSettings
    {
        #region Fields

        /// <summary>
        /// Stores the breaks for the child controls.
        /// </summary>
        private readonly Dictionary<Control, bool> flowBreaks;

        /// <summary>
        /// The parent control.
        /// </summary>
        private readonly FlowLayoutPanel parent;

        private FlowDirection flowDirection;
        private bool wrapContents;

        #endregion Fields

        #region Constructors

        internal FlowLayoutSettings(FlowLayoutPanel parent)
        {
            this.flowBreaks = new Dictionary<Control, bool>();
            this.wrapContents = true;
            this.flowDirection = FlowDirection.LeftToRight;
            this.parent = parent;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating the flow direction of consecutive controls.
        /// </summary>
        /// <value>The flow direction of consecutive controls.</value>
        public FlowDirection FlowDirection
        {
            get
            {
                return this.flowDirection;
            }

            set
            {
                if (this.flowDirection != value)
                {
                    this.flowDirection = value;
                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this.parent, "FlowDirection");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contents should be wrapped or clipped when they exceed the original boundaries of their container.
        /// </summary>
        /// <value><c>true</c> if the contents are wrapped; otherwise, <c>false</c>.</value>
        public bool WrapContents
        {
            get
            {
                return this.wrapContents;
            }

            set
            {
                if (this.wrapContents != value)
                {
                    this.wrapContents = value;
                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this.parent, "WrapContents");
                    }
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns a value that represents the flow break setting of the control.
        /// </summary>
        /// <param name="child">The child control.</param>
        /// <returns><c>true</c> if the flow break is set; otherwise, <c>false</c>.</returns>
        public bool GetFlowBreak(Control child)
        {
            bool retval;

            return this.flowBreaks.TryGetValue(child, out retval) && retval;
        }

        /// <summary>
        /// Sets the value that represents the flow break setting of the control.
        /// </summary>
        /// <param name="child">The child control.</param>
        /// <param name="value">The flow break value to set.</param>
        public void SetFlowBreak(Control child, bool value)
        {
            this.flowBreaks[child] = value;
            if (this.parent != null)
            {
                this.parent.PerformLayout(child, "FlowBreak");
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}