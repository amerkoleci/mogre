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
// Created: 3/26/2009 11:51:49 AM
 */
namespace Miyagi.UI.Controls
{
    using System;

    using Miyagi.Common.Data;
    using Miyagi.UI.Controls.Layout;

    /// <summary>
    /// Represents a parent that dynamically lays out its contents horizontally or vertically.
    /// </summary>
    public class FlowLayoutPanel : Panel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FlowLayoutPanel class.
        /// </summary>
        public FlowLayoutPanel()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FlowLayoutPanel class.
        /// </summary>
        /// <param name="name">The name of the FlowLayoutPanel.</param>
        public FlowLayoutPanel(string name)
            : base(name)
        {
            this.LayoutEngine = new FlowLayout();
            this.LayoutSettings = new FlowLayoutSettings(this);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating the flow direction of the FlowLayoutPanel control.
        /// </summary>
        /// <value>The flow direction of the FlowLayoutPanel control.</value>
        public FlowDirection FlowDirection
        {
            get
            {
                return this.LayoutSettings.FlowDirection;
            }

            set
            {
                this.LayoutSettings.FlowDirection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the FlowLayoutPanel control should wrap its contents or let the contents be clipped.
        /// </summary>
        /// <value><c>true</c> if the contents are wrapped; otherwise, <c>false</c>.</value>
        public bool WrapContents
        {
            get
            {
                return this.LayoutSettings.WrapContents;
            }

            set
            {
                this.LayoutSettings.WrapContents = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// Gets the settings for the parent.
        /// </summary>
        /// <value>The settings for the parent.</value>
        internal FlowLayoutSettings LayoutSettings
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns a value that represents the flow-break setting of the FlowLayoutPanel control.
        /// </summary>
        /// <param name="control">The child control.</param>
        /// <returns><c>true</c> if the flow break is set; otherwise, <c>false</c>.</returns>
        public bool GetFlowBreak(Control control)
        {
            this.ThrowIfDisposed();
            return this.LayoutSettings.GetFlowBreak(control);
        }

        /// <summary>
        /// Sets the value that represents the flow-break setting of the FlowLayoutPanel control.
        /// </summary>
        /// <param name="control">The child control.</param>
        /// <param name="value">The flow-break value to set.</param>
        public void SetFlowBreak(Control control, bool value)
        {
            this.ThrowIfDisposed();
            this.LayoutSettings.SetFlowBreak(control, value);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Retrieves the size of a rectangular area into which the flow parent and its childs can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>A Size representing the width and height of a rectangle.</returns>
        protected override Size GetPreferredSizeCore(Size proposedSize)
        {
            Size preferredSize;

            bool isHorizontal = (this.FlowDirection == FlowDirection.LeftToRight)
                                || (this.FlowDirection == FlowDirection.RightToLeft);

            if (!this.WrapContents || (isHorizontal && proposedSize.Width == 0) || (!isHorizontal && proposedSize.Height == 0))
            {
                preferredSize = this.SizeWithoutBreaks(isHorizontal);
            }
            else
            {
                preferredSize = this.SizeWithBreaks(proposedSize, isHorizontal);
            }

            return this.SizeFromClientSize(preferredSize);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Calculates a size for the parent that might need breaks between some controls.
        /// </summary>
        /// <param name="proposedSize">The proposed size for the control.</param>
        /// <param name="isHorizontal">Whether the flow is horizontal or vertical.</param>
        /// <returns>A size for the parent.</returns>
        private Size SizeWithBreaks(Size proposedSize, bool isHorizontal)
        {
            int width = 0;
            int height = 0;
            int flowSize = 0;
            int otherSize = 0;

            foreach (Control control in Controls)
            {
                Size size = control.Size;
                if (control.AutoSize)
                {
                    size = control.PreferredSize;
                }

                Thickness margin = control.Margin;

                int increase;

                if (isHorizontal)
                {
                    increase = size.Width + margin.Horizontal;

                    if (flowSize != 0 && flowSize + increase >= proposedSize.Width)
                    {
                        width = Math.Max(width, flowSize);
                        flowSize = 0;
                        height += otherSize;
                        otherSize = 0;
                    }

                    flowSize += increase;
                    otherSize = Math.Max(otherSize, size.Height + margin.Vertical);
                }
                else
                {
                    increase = size.Height + margin.Vertical;

                    if (flowSize != 0 && flowSize + increase >= proposedSize.Height)
                    {
                        height = Math.Max(height, flowSize);
                        flowSize = 0;
                        width += otherSize;
                        otherSize = 0;
                    }

                    flowSize += increase;
                    otherSize = Math.Max(otherSize, size.Width + margin.Horizontal);
                }
            }

            if (isHorizontal)
            {
                width = Math.Max(width, flowSize);
                height += otherSize;
            }
            else
            {
                height = Math.Max(height, flowSize);
                width += otherSize;
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Calculates a size for the parent without breaks between child controls.
        /// </summary>
        /// <param name="isHorizontal">Whether the flow is horizontal or vertical.</param>
        /// <returns>A size for the parent.</returns>
        private Size SizeWithoutBreaks(bool isHorizontal)
        {
            int width = 0;
            int height = 0;

            foreach (Control control in Controls)
            {
                Size size = control.Size;

                if (control.AutoSize)
                {
                    size = control.PreferredSize;
                }

                Thickness margin = control.Margin;

                if (isHorizontal)
                {
                    width += size.Width + margin.Horizontal;
                    height = Math.Max(height, size.Height + margin.Vertical);
                }
                else
                {
                    height += size.Height + margin.Vertical;
                    width = Math.Max(width, size.Width + margin.Horizontal);
                }
            }

            return new Size(width, height);
        }

        #endregion Private Methods

        #endregion Methods
    }
}