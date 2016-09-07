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
// Created: 4/29/2009 4:27:00 PM
 */
namespace Miyagi.UI.Controls.Layout
{
    using System;
    using System.Collections.Generic;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;

    /// <summary>
    /// Collects the characteristics associated with table layouts.
    /// </summary>
    public class TableLayoutSettings
    {
        #region Fields

        private readonly Dictionary<Control, Range> cells;
        private readonly MiyagiCollection<TableLayoutStyle> columnStyles;
        private readonly TableLayoutPanel parent;
        private readonly MiyagiCollection<TableLayoutStyle> rowStyles;

        private int columnCount;
        private int rowCount;

        #endregion Fields

        #region Constructors

        internal TableLayoutSettings(TableLayoutPanel parent)
        {
            this.parent = parent;

            this.cells = new Dictionary<Control, Range>();

            this.columnStyles = new MiyagiCollection<TableLayoutStyle>();
            this.columnStyles.ItemAdded += this.TableLayoutStyleChanged;
            this.columnStyles.ItemRemoved += this.TableLayoutStyleChanged;
            this.rowStyles = new MiyagiCollection<TableLayoutStyle>();
            this.rowStyles.ItemAdded += this.TableLayoutStyleChanged;
            this.rowStyles.ItemRemoved += this.TableLayoutStyleChanged;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum number of columns allowed in the table layout.
        /// </summary>
        /// <value>The maximum number of columns.</value>
        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Number of columns can't be negative");
                }

                if (this.columnCount != value)
                {
                    this.columnCount = value;
                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this.parent, "ColumnCount");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the collection of styles used to determine the look and feel of the table layout columns.
        /// </summary>
        /// <value>The collection of styles.</value>
        public MiyagiCollection<TableLayoutStyle> ColumnStyles
        {
            get
            {
                return this.columnStyles;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of rows allowed in the table layout.
        /// </summary>
        /// <value>The maximum number of rows.</value>
        public int RowCount
        {
            get
            {
                return this.rowCount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Number of rows can't be negative");
                }

                if (this.rowCount != value)
                {
                    this.rowCount = value;
                    if (this.parent != null)
                    {
                        this.parent.PerformLayout(this.parent, "RowCount");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the collection of styles used to determine the look and feel of the table layout rows.
        /// </summary>
        /// <value>The collection of styles.</value>
        public MiyagiCollection<TableLayoutStyle> RowStyles
        {
            get
            {
                return this.rowStyles;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the row and column position of the specified child control.
        /// </summary>
        /// <param name="control">The child control.</param>
        /// <param name="row">The row position of the specified child control, or -1 if the position of control is determined by <see cref="Control.LayoutEngine"/>.</param>
        /// <param name="column">The column position of the specified child control, or -1 if the position of control is determined by <see cref="Control.LayoutEngine"/>.</param>
        public void GetCell(Control control, out int row, out int column)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Range cell;

            if (this.cells.TryGetValue(control, out cell))
            {
                row = cell.First;
                column = cell.Last;
            }
            else
            {
                row = -1;
                column = -1;
            }
        }

        /// <summary>
        /// Returns the column position of the specified child control.
        /// </summary>
        /// <param name="control">The child control.</param>
        /// <returns>The column position of the specified child control, or -1 if the position of control is determined by <see cref="Control.LayoutEngine"/>.</returns>
        public int GetColumn(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Range cell;

            if (this.cells.TryGetValue(control, out cell))
            {
                return cell.Last;
            }

            return -1;
        }

        /// <summary>
        /// Returns the row position of the specified child control.
        /// </summary>
        /// <param name="control">The child control.</param>
        /// <returns>The row position of the specified child control, or -1 if the position of control is determined by <see cref="Control.LayoutEngine"/>.</returns>
        public int GetRow(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Range cell;

            if (this.cells.TryGetValue(control, out cell))
            {
                return cell.First;
            }

            return -1;
        }

        /// <summary>
        /// Removes a control from the settings.
        /// </summary>
        /// <param name="control">The control to remove.</param>
        public void RemoveControl(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            this.cells.Remove(control);
        }

        /// <summary>
        /// Sets the row and column position of the specified child control.
        /// </summary>
        /// <param name="control">The control to move.</param>
        /// <param name="row">The row to which control will be moved.</param>
        /// <param name="column">The column to which the control will be moved.</param>
        public void SetCell(Control control, int row, int column)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (row < -1)
            {
                throw new ArgumentException("The given row is invalid", "row");
            }

            if (column < -1)
            {
                throw new ArgumentException("The given column is invalid", "column");
            }

            this.cells[control] = new Range(row, column);

            if (this.parent != null)
            {
                this.parent.PerformLayout(this.parent, "SetCell");
            }
        }

        /// <summary>
        /// Sets the column position of the specified child control.
        /// </summary>
        /// <param name="control">The control to move to another column.</param>
        /// <param name="column">The column to which control will be moved.</param>
        public void SetColumn(Control control, int column)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (column < -1)
            {
                throw new ArgumentException("The given column is invalid", "column");
            }

            this.cells[control] = new Range(this.cells[control].First, column);

            if (this.parent != null)
            {
                this.parent.PerformLayout(this.parent, "SetColumn");
            }
        }

        /// <summary>
        /// Sets the row and column position of the specified child control to the first free cell.
        /// </summary>
        /// <param name="control">The child control.</param>
        public void SetFirstFreeCell(Control control)
        {
            for (int row = 0; row < this.rowCount; row++)
            {
                for (int column = 0; column < this.columnCount; column++)
                {
                    Range cell = new Range(row, column);
                    if (!this.cells.ContainsValue(cell))
                    {
                        this.SetCell(control, row, column);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the row position of the specified child control.
        /// </summary>
        /// <param name="control">The control to move to another row.</param>
        /// <param name="row">The row to which control will be moved.</param>
        public void SetRow(Control control, int row)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (row < -1)
            {
                throw new ArgumentException("The given row is invalid", "row");
            }

            this.cells[control] = new Range(row, this.cells[control].Last);

            if (this.parent != null)
            {
                this.parent.PerformLayout(this.parent, "SetRow");
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void TableLayoutStyleChanged(object sender, CollectionEventArgs<TableLayoutStyle> style)
        {
            if (this.parent != null)
            {
                this.parent.PerformLayout(this.parent, "TableLayoutStyle_Changed");
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}