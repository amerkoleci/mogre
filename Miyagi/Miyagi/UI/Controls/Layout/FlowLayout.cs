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
// Created: 3/26/2009 11:48:10 AM
 */
namespace Miyagi.UI.Controls.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common.Data;

    /// <summary>
    /// Layout engine that dynamically lays out its contents horizontally or vertically, creating new rows or columns when necessary.
    /// </summary>
    public class FlowLayout : LayoutEngine
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Perform the layout for the given container, that has to be a FlowLayoutPanel.
        /// </summary>
        /// <param name="parent">The control whose layout has to be done.</param>
        /// <param name="layoutEventArgs">The LayoutEventArgs.</param>
        /// <returns>Always false.</returns>
        public override bool Layout(Control parent, LayoutEventArgs layoutEventArgs)
        {
            FlowLayoutPanel flowPanel = parent as FlowLayoutPanel;

            if (flowPanel == null)
            {
                throw new ArgumentException("The given container is not a FlowLayoutPanel", "parent");
            }

            if (flowPanel.Controls.Count == 0)
            {
                return false;
            }

            FlowLayoutSettings settings = flowPanel.LayoutSettings;
            var space = new Rectangle(Point.Empty, flowPanel.DisplayRectangle.Size);
            int x, y;

            // Set starting point based on flow direction
            switch (settings.FlowDirection)
            {
                case FlowDirection.BottomUp:
                    x = space.Left;
                    y = space.Bottom;
                    break;
                case FlowDirection.RightToLeft:
                    x = space.Right;
                    y = space.Top;
                    break;
                default:
                    x = space.Left;
                    y = space.Top;
                    break;
            }

            bool forceFlowBreak = false;
            List<Control> rowControls = new List<Control>();

            // Iterate through all the controls and position them inside the parent control
            foreach (Control child in flowPanel.Controls)
            {
                if (!child.Visible)
                {
                    continue;
                }

                if (child.AutoSize)
                {
                    Size size = child.GetPreferredSize(child.Size);
                    child.SetBounds(child.Left, child.Top, size.Width, size.Height, BoundsSpecified.None);
                }

                switch (settings.FlowDirection)
                {
                    case FlowDirection.BottomUp:

                        // Decide if we have to start a new column
                        if (settings.WrapContents && (forceFlowBreak || IsTopOverFlow(y, child)))
                        {
                            x = FinishColumn(rowControls);
                            y = space.Bottom;
                            forceFlowBreak = false;
                            rowControls.Clear();
                        }

                        // Move child
                        y -= child.Margin.Bottom;
                        child.SetBounds(x + child.Margin.Left, y - child.Height, child.Width, child.Height, BoundsSpecified.None);

                        // Update position
                        y -= child.Height + child.Margin.Top;

                        break;

                    default:

                        // Decide if we have to start a new row
                        if (settings.WrapContents && (forceFlowBreak || IsRightOverFlow(space, x, child)))
                        {
                            x = space.Left;
                            y = FinishRow(rowControls);
                            forceFlowBreak = false;
                            rowControls.Clear();
                        }

                        // Move child
                        x += child.Margin.Left;
                        child.SetBounds(x, y + child.Margin.Top, child.Width, child.Height, BoundsSpecified.None);

                        // Update position
                        x += child.Width + child.Margin.Right;

                        break;

                    case FlowDirection.RightToLeft:

                        // Decide if we have to start a new row
                        if (settings.WrapContents && (forceFlowBreak || IsLeftOverFlow(x, child)))
                        {
                            x = space.Right;
                            y = FinishRow(rowControls);
                            forceFlowBreak = false;
                            rowControls.Clear();
                        }

                        // Move child
                        x -= child.Margin.Right;
                        child.SetBounds(x - child.Width, y + child.Margin.Top, child.Width, child.Height, BoundsSpecified.None);

                        // Update position
                        x -= child.Width + child.Margin.Left;

                        break;

                    case FlowDirection.TopDown:

                        // Decide if we have to start a new column
                        if (settings.WrapContents && (forceFlowBreak || IsBottomOverFlow(space, y, child)))
                        {
                            x = FinishColumn(rowControls);
                            y = space.Top;
                            forceFlowBreak = false;
                            rowControls.Clear();
                        }

                        // Move child
                        y += child.Margin.Top;
                        child.SetBounds(x + child.Margin.Left, y, child.Width, child.Height, BoundsSpecified.None);

                        // Update position
                        y += child.Height + child.Margin.Bottom;

                        break;
                }

                rowControls.Add(child);
                if (settings.GetFlowBreak(child))
                {
                    forceFlowBreak = true;
                }
            }

            // Last row
            if (settings.FlowDirection == FlowDirection.LeftToRight
                || settings.FlowDirection == FlowDirection.RightToLeft)
            {
                FinishRow(rowControls);
            }
            else
            {
                FinishColumn(rowControls);
            }

            return false;
        }

        #endregion Public Methods

        #region Private Static Methods

        /// <summary>
        /// Adjusts a column of controls.
        /// </summary>
        /// <param name="controls">The controls that form the column.</param>
        /// <returns>The rightmost x coordinate of the column.</returns>
        private static int FinishColumn(ICollection<Control> controls)
        {
            if (controls.Count == 0)
            {
                return 0;
            }

            bool allDockFill = true;
            bool noAuto = true;

            // check if every control is either Dock.Fill or Anchor.Left+Right
            foreach (Control c in controls)
            {
                if (c.Dock != DockStyle.Fill && (c.Anchor & AnchorStyles.Horizontal) != AnchorStyles.Horizontal)
                {
                    allDockFill = false;
                }

                if (c.AutoSize)
                {
                    noAuto = false;
                }
            }

            int rowLeft = int.MaxValue;
            int rowRight = 0;

            // Find the widest control
            foreach (Control c in controls)
            {
                int right = c.Right + c.Margin.Right;
                int left = c.Left - c.Margin.Left;

                if (right > rowRight && c.Dock != DockStyle.Fill && (c.Anchor & AnchorStyles.Horizontal) != AnchorStyles.Horizontal)
                {
                    rowRight = right;
                }

                if (left < rowLeft)
                {
                    rowLeft = left;
                }
            }

            // Find the widest control with AutoSize
            if (rowRight == 0)
            {
                int right1 = rowRight;
                foreach (int right in from c in controls
                                      let right = c.Right + c.Margin.Right
                                      where right > right1 && c.Dock != DockStyle.Fill && c.AutoSize
                                      select right)
                {
                    rowRight = right;
                }
            }

            // Find the widest control with Dock.Fill
            if (rowRight == 0)
            {
                int right1 = rowRight;
                foreach (int right in
                    from c in controls
                    let right = c.Right + c.Margin.Right
                    where right > right1 && c.Dock == DockStyle.Fill
                    select right)
                {
                    rowRight = right;
                }
            }

            // Set widths. Copied verbatim, hope I never have to check this part
            foreach (Control c in controls)
            {
                if (allDockFill && noAuto)
                {
                    c.SetBounds(c.Left, c.Top, 0, c.Height, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Fill || (c.Anchor & AnchorStyles.Horizontal) == AnchorStyles.Horizontal)
                {
                    c.SetBounds(c.Left, c.Top, rowRight - c.Left - c.Margin.Right, c.Height, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Right || (c.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                {
                    c.SetBounds(rowRight - c.Margin.Right - c.Width, c.Top, c.Width, c.Height, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Left || (c.Anchor & AnchorStyles.Left) == AnchorStyles.Left)
                {
                    continue;
                }
                else
                {
                    int left = ((rowRight - rowLeft) / 2) - (c.Width / 2) + (int)Math.Floor((c.Margin.Left - c.Margin.Right) / 2.0) + rowLeft;
                    c.SetBounds(left, c.Top, c.Width, c.Height, BoundsSpecified.None);
                }
            }

            // return rightmost x of this row used
            return (rowRight != 0) ? rowRight : rowLeft;
        }

        /// <summary>
        /// Adjusts a row of controls.
        /// </summary>
        /// <param name="controls">The controls that form the row.</param>
        /// <returns>The highest y coordinate (bottom) of the row.</returns>
        private static int FinishRow(ICollection<Control> controls)
        {
            if (controls.Count == 0)
            {
                return 0;
            }

            bool allDockFill = true;
            bool noAuto = true;

            // check if every control is either Dock.Fill or Anchor.Top+Bottom
            foreach (Control c in controls)
            {
                if (c.Dock != DockStyle.Fill && (c.Anchor & AnchorStyles.Vertical) != AnchorStyles.Vertical)
                {
                    allDockFill = false;
                }

                if (c.AutoSize)
                {
                    noAuto = false;
                }
            }

            int rowTop = int.MaxValue;
            int rowBottom = 0;

            // Find the tallest control
            foreach (Control c in controls)
            {
                int bottom = c.Bottom + c.Margin.Bottom;
                int top = c.Top - c.Margin.Top;

                if (bottom > rowBottom && c.Dock != DockStyle.Fill && (c.Anchor & AnchorStyles.Vertical) != AnchorStyles.Vertical)
                {
                    rowBottom = bottom;
                }

                if (top < rowTop)
                {
                    rowTop = top;
                }
            }

            // Find the tallest control with AutoSize
            if (rowBottom == 0)
            {
                foreach (Control c in controls)
                {
                    int bottom = c.Bottom + c.Margin.Bottom;
                    if (bottom > rowBottom && c.Dock != DockStyle.Fill && c.AutoSize)
                    {
                        rowBottom = bottom;
                    }
                }
            }

            // Find the tallest control with Dock.Fill
            if (rowBottom == 0)
            {
                foreach (Control c in controls)
                {
                    int bottom = c.Bottom + c.Margin.Bottom;
                    if (bottom > rowBottom && c.Dock == DockStyle.Fill)
                    {
                        rowBottom = bottom;
                    }
                }
            }

            // Set heights. Copied verbatim, hope I never have to check this part
            foreach (Control c in controls)
            {
                if (allDockFill && noAuto)
                {
                    c.SetBounds(c.Left, c.Top, c.Width, 0, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Fill || (c.Anchor & AnchorStyles.Vertical) == AnchorStyles.Vertical)
                {
                    c.SetBounds(c.Left, c.Top, c.Width, rowBottom - c.Top - c.Margin.Bottom, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Bottom || (c.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                {
                    c.SetBounds(c.Left, rowBottom - c.Margin.Bottom - c.Height, c.Width, c.Height, BoundsSpecified.None);
                }
                else if (c.Dock == DockStyle.Top || (c.Anchor & AnchorStyles.Top) == AnchorStyles.Top)
                {
                    continue;
                }
                else
                {
                    int right = ((rowBottom - rowTop) / 2) - (c.Height / 2) + (int)Math.Floor((c.Margin.Top - c.Margin.Bottom) / 2.0) + rowTop;
                    c.SetBounds(c.Left, right, c.Width, c.Height, BoundsSpecified.None);
                }
            }

            // return bottom y of this row used
            return (rowBottom != 0) ? rowBottom : rowTop;
        }

        /// <summary>
        /// Checks whether the child control will overflow the bottom corner of the parent control.
        /// </summary>
        /// <param name="space">The dimensions of the parent control.</param>
        /// <param name="y">The value of the y-axis assigned to this control.</param>
        /// <param name="child">The control to position.</param>
        /// <returns><c>true</c> if there is an overflow, false otherwise.</returns>
        private static bool IsBottomOverFlow(Rectangle space, int y, Control child)
        {
            return space.Height + space.Top - y < child.Height + child.Margin.Vertical;
        }

        /// <summary>
        /// Checks whether the child control will overflow the left corner of the parent control.
        /// </summary>
        /// <param name="x">The value of the x-axis assigned to this control.</param>
        /// <param name="child">The control to position.</param>
        /// <returns><c>true</c> if there is an overflow, false otherwise.</returns>
        private static bool IsLeftOverFlow(int x, Control child)
        {
            return x < child.Width + child.Margin.Horizontal;
        }

        /// <summary>
        /// Checks whether the child control will overflow the right corner of the parent control.
        /// </summary>
        /// <param name="space">The dimensions of the parent control.</param>
        /// <param name="x">The value of the x-axis assigned to this control.</param>
        /// <param name="child">The control to position.</param>
        /// <returns><c>true</c> if there is an overflow, false otherwise.</returns>
        private static bool IsRightOverFlow(Rectangle space, int x, Control child)
        {
            return space.Width + space.Left - x < child.Width + child.Margin.Horizontal;
        }

        /// <summary>
        /// Checks whether the child control will overflow the top corner of the parent control.
        /// </summary>
        /// <param name="y">The value of the y-axis assigned to this control.</param>
        /// <param name="child">The control to position.</param>
        /// <returns><c>true</c> if there is an overflow, false otherwise.</returns>
        private static bool IsTopOverFlow(int y, Control child)
        {
            return y < child.Height + child.Margin.Vertical;
        }

        #endregion Private Static Methods

        #endregion Methods
    }
}