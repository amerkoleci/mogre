/*
// Copyright (c) 2009 Realmforge Studios GmbH.
//
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
// Created: 4/29/2009 4:09:44 PM
 */
namespace Miyagi.UI.Controls.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Miyagi.Common.Data;

    /// <summary>
    /// Layout engine that positions the controls in a table with user-defined rows and columns.
    /// </summary>
    public class TableLayout : LayoutEngine
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Performs the layout for the given container, that has to be a TableLayoutPanel.
        /// </summary>
        /// <param name="parent">The control whose layout has to be done.</param>
        /// <param name="layoutEventArgs">The LayoutEventArgs.</param>
        /// <returns>Always false.</returns>
        public override bool Layout(Control parent, LayoutEventArgs layoutEventArgs)
        {
            TableLayoutPanel tableLayoutPanel = parent as TableLayoutPanel;

            if (tableLayoutPanel == null)
            {
                throw new ArgumentException("The given container is not a TableLayoutPanel", "parent");
            }

            if (tableLayoutPanel.Controls.Count == 0)
            {
                return false;
            }

            TableLayoutSettings settings = tableLayoutPanel.LayoutSettings;

            // STEP 1:
            // - Figure out which row/column each control goes into
            Control[,] grid = this.CalculateControlLocations(tableLayoutPanel, settings, settings.ColumnCount, settings.RowCount);

            // STEP 2:
            // - Figure out the sizes of each row/column
            int[] columnWidths;
            int[] rowHeights;
            CalculateCellSizes(tableLayoutPanel, settings, grid, out columnWidths, out rowHeights);

            // STEP 3:
            // - Size and position each control
            LayoutControls(tableLayoutPanel, grid, columnWidths, rowHeights);

            return false;
        }

        #endregion Public Methods

        #region Private Static Methods

        /// <summary>
        /// Calculate the widths of the columns and the heights of the rows.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="settings">The layout settings.</param>
        /// <param name="grid">The matrix with the controls in their corresponding positions.</param>
        /// <param name="columnWidths">The list of widths for the columns.</param>
        /// <param name="rowHeights">The list of heights for the rows.</param>
        private static void CalculateCellSizes(Control parent, TableLayoutSettings settings, Control[,] grid, out int[] columnWidths, out int[] rowHeights)
        {
            int columns = grid.GetLength(0);
            int rows = grid.GetLength(1);

            var rect = new Rectangle(Point.Empty, parent.DisplayRectangle.Size);

            // Column widths
            Func<int, int> findWidestControl = index => FindBiggestControl(rows, i => grid[index, i], s => s.Width, margin => margin.Horizontal);
            columnWidths = GetDimensionSize(columns, settings.ColumnStyles, rect.Width, findWidestControl);

            // Row heights
            Func<int, int> findTallestControl = index => FindBiggestControl(columns, i => grid[i, index], s => s.Height, margin => margin.Vertical);
            rowHeights = GetDimensionSize(rows, settings.RowStyles, rect.Height, findTallestControl);
        }

        /// <summary>
        /// Finds the biggest control in a given list, for a dimension specified by a function.
        /// </summary>
        /// <param name="elements">The number of elements in the list.</param>
        /// <param name="getChild">A function that retrieves an element from the list, given an index.</param>
        /// <param name="getDimension">A function that retrieves the value of the dimension (height or width) measured, given a size.</param>
        /// <param name="getMargin">A function that retrieves the value of the aplicable margin (vertical or horizontal), given a margin.</param>
        /// <returns>The value of the dimension (height or width) of the biggest control in the list.</returns>
        private static int FindBiggestControl(int elements, Func<int, Control> getChild, Func<Size, int> getDimension, Func<Thickness, int> getMargin)
        {
            int maxSize = 0;

            for (int i = 0; i < elements; i++)
            {
                Control child = getChild(i);

                if (child != null && child.Visible)
                {
                    int size = child.AutoSize ? getDimension(child.PreferredSize) : getDimension(child.Size);
                    maxSize = Math.Max(maxSize, size + getMargin(child.Margin));
                }
            }

            return maxSize;
        }

        /// <summary>
        /// Calculates the value of a dimension (height or width) for a list of elements, given a list of styles, the amount of available space, and a function to calculate the biggest element for an index.
        /// </summary>
        /// <param name="elements">The number of elements.</param>
        /// <param name="styles">A collection of styles.</param>
        /// <param name="availableSpace">The amount of available space.</param>
        /// <param name="findBiggest">A function that returns the biggest value for a dimension, given an index.</param>
        /// <returns>A list of with the values of the measured dimension.</returns>
        private static int[] GetDimensionSize(int elements, IEnumerable<TableLayoutStyle> styles, int availableSpace, Func<int, int> findBiggest)
        {
            int[] sizes = new int[elements];
            int totalSize = availableSpace;
            int index = 0;

            // Absolute elements take a fixed amount of space
            foreach (TableLayoutStyle style in styles)
            {
                if (style.SizeType == SizeType.Absolute)
                {
                    sizes[index] = (int)style.Magnitude;
                    totalSize -= (int)style.Magnitude;
                }

                index++;
            }

            index = 0;

            // AutoSize elements take the biggest size in its own opposite line
            foreach (TableLayoutStyle style in styles)
            {
                if (style.SizeType == SizeType.AutoSize)
                {
                    int maxSize = findBiggest(index);

                    sizes[index] = maxSize;
                    totalSize -= maxSize;
                }

                index++;
            }

            index = 0;

            // Percent elements divide the amount left between themselves
            // Note: The percents are based upon the sum of all the elements with a percent type, e.g: if one element has 30 as percent and the sum is 120, it will take 30/120 of the remaining space.
            if (totalSize > 0)
            {
                int percentSize = totalSize;
                float totalPercent = styles.Where(style => style.SizeType == SizeType.Percent).Sum(style => style.Magnitude);

                // Find the total percent (not always 100%)

                // Div up the space..
                foreach (TableLayoutStyle style in styles)
                {
                    if (style.SizeType == SizeType.Percent)
                    {
                        sizes[index] = (int)((style.Magnitude / totalPercent) * percentSize);
                        totalSize -= sizes[index];
                    }

                    index++;
                }
            }

            return sizes;
        }

        // ignore border width, dummy controls, spans
        private static void LayoutControls(Control parent, Control[,] grid, IList<int> columnWidths, IList<int> rowHeights)
        {
            int columns = grid.GetLength(0);
            int rows = grid.GetLength(1);

            var position = Point.Empty;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Control child = grid[col, row];

                    if (child != null)
                    {
                        Size preferredSize = child.AutoSize ? child.PreferredSize : child.Size;

                        int x, y, width, height;

                        // Ignore 0 sizes or the dimensions of some children could get corrupted
                        if (columnWidths[col] == 0 || rowHeights[row] == 0)
                        {
                            continue;
                        }

                        // Figure out the width of the control
                        int columnWidth = columnWidths[col];

                        if (child.Dock == DockStyle.Fill
                            || child.Dock == DockStyle.Top
                            || child.Dock == DockStyle.Bottom
                            || (child.Anchor & AnchorStyles.Horizontal) == AnchorStyles.Horizontal)
                        {
                            width = columnWidth - child.Margin.Horizontal;
                        }
                        else
                        {
                            width = Math.Min(preferredSize.Width, columnWidth - child.Margin.Horizontal);
                        }

                        // Figure out the height of the control
                        int columnHeight = rowHeights[row];

                        if (child.Dock == DockStyle.Fill
                            || child.Dock == DockStyle.Left
                            || child.Dock == DockStyle.Right
                            || (child.Anchor & AnchorStyles.Vertical) == AnchorStyles.Vertical)
                        {
                            height = columnHeight - child.Margin.Vertical;
                        }
                        else
                        {
                            height = Math.Min(preferredSize.Height, columnHeight - child.Margin.Vertical);
                        }

                        // Figure out the left location of the control
                        int right = position.X + columnWidth;

                        if (child.Dock == DockStyle.Left
                            || child.Dock == DockStyle.Fill
                            || (child.Anchor & AnchorStyles.Left) == AnchorStyles.Left)
                        {
                            x = position.X + child.Margin.Left;
                        }
                        else if (child.Dock == DockStyle.Right
                                 || (child.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                        {
                            x = right - width - child.Margin.Right;
                        }
                        else
                        {
                            x = (position.X + ((columnWidth - child.Margin.Horizontal) / 2)) + child.Margin.Left - (width / 2);
                        }

                        // Figure out the top location of the control
                        int bottom = position.Y + columnHeight;

                        if (child.Dock == DockStyle.Top
                            || child.Dock == DockStyle.Fill
                            || (child.Anchor & AnchorStyles.Top) == AnchorStyles.Top)
                        {
                            y = position.Y + child.Margin.Top;
                        }
                        else if (child.Dock == DockStyle.Bottom
                                 || (child.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                        {
                            y = bottom - height - child.Margin.Bottom;
                        }
                        else
                        {
                            y = (position.Y + ((columnHeight - child.Margin.Vertical) / 2)) + child.Margin.Top - (height / 2);
                        }

                        child.SetBounds(x, y, width, height, BoundsSpecified.None);
                    }

                    position = Point.Add(position, columnWidths[col], 0);
                }

                position = Point.Add(position, (-1 * position.X), rowHeights[row]);
            }
        }

        #endregion Private Static Methods

        #region Private Methods

        /// <summary>
        /// Fit each control into its corresponding position in the grid.
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="settings">The layout settings.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="rows">The number of rows.</param>
        /// <returns>A matrix with the controls in their corresponding positions according to the settings.</returns>
        private Control[,] CalculateControlLocations(IControlCollectionOwner parent, TableLayoutSettings settings, int columns, int rows)
        {
            var grid = new Control[columns,rows];

            // Assert consistency
            Debug.Assert(parent.Controls.Count <= columns * rows, string.Format("{0}: Consistency Error, There are more controls than empty spaces", this));

            // First place all controls that have an explicit col/row
            foreach (Control child in parent.Controls)
            {
                int col = settings.GetColumn(child);
                int row = settings.GetRow(child);

                // Assert consistency
                Debug.Assert(col >= 0 && col < columns && row >= 0 && row < rows, string.Format("{0}: Consistency Error, A control has an invalid space assigned", this));

                // Assert consistency
                Debug.Assert(grid[col, row] == null, string.Format("{0}: Consistency Error, There are two controls assigned to the same cell", this));

                grid[col, row] = child;
            }

            return grid;
        }

        #endregion Private Methods

        #endregion Methods
    }
}