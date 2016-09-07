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
// Created: 4/29/2009 4:31:00 PM
 */
namespace Miyagi.UI.Controls
{
    using Miyagi.Common;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls.Layout;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// Represents a parent that dynamically lays out its contents in a grid composed of rows and columns.
    /// </summary>
    public class TableLayoutPanel : Panel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TableLayoutPanel class.
        /// </summary>
        public TableLayoutPanel()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TableLayoutPanel class.
        /// </summary>
        /// <param name="name">The name of the TableLayoutPanel.</param>
        public TableLayoutPanel(string name)
            : base(name)
        {
            this.LayoutEngine = new TableLayout();
            this.LayoutSettings = new TableLayoutSettings(this);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the number of columns in the table.
        /// </summary>
        /// <value>The number of columns.</value>
        public int ColumnCount
        {
            get
            {
                return this.LayoutSettings.ColumnCount;
            }

            set
            {
                this.LayoutSettings.ColumnCount = value;
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
                return this.LayoutSettings.ColumnStyles;
            }
        }

        /// <summary>
        /// Gets the settings for the parent.
        /// </summary>
        /// <value>The LayoutSettings.</value>
        public TableLayoutSettings LayoutSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the number of rows in the table.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount
        {
            get
            {
                return this.LayoutSettings.RowCount;
            }

            set
            {
                this.LayoutSettings.RowCount = value;
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
                return this.LayoutSettings.RowStyles;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds a control to the panel, specifying the row and column where it should go.
        /// </summary>
        /// <param name="control">The control to add.</param>
        /// <param name="row">The row inside the panel where the control should go.</param>
        /// <param name="column">The column inside the panel where the control should go.</param>
        public virtual void AddControl(Control control, int row, int column)
        {
            this.LayoutSettings.SetCell(control, row, column);
            this.Controls.Add(control);
        }

        /// <summary>
        /// Converts items from a two-dimensional array to Labels and adds them to the TableLayoutPanel.
        /// </summary>
        /// <param name="array">The array containing the elements.</param>
        /// <param name="textStyle">The TextStyle which will be applied to the Labels.</param>
        /// <param name="clear">Indicates whether the current control collection should be cleared.</param>
        public void FillWith2DArray(object[,] array, TextStyle textStyle, bool clear)
        {
            if (clear)
            {
                this.Controls.Clear();
                this.ColumnCount = array.GetLength(0);
                this.RowCount = array.GetLength(1);
            }

            foreach (object obj in array)
            {
                this.Controls.Add(new Label
                                  {
                                      AutoSize = true,
                                      Dock = DockStyle.Fill,
                                      Text = obj.ToString(),
                                      TextStyle = textStyle
                                  });
            }
        }

        /// <summary>
        /// Removes a control from the panel.
        /// </summary>
        /// <param name="control">The control to remove.</param>
        public virtual void RemoveControl(Control control)
        {
            this.LayoutSettings.RemoveControl(control);
            this.Controls.Remove(control);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the ControlAdded event.
        /// </summary>
        /// <param name="e">An ControlEventArgs that contains the event data.</param>
        protected override void OnControlAdded(ValueEventArgs<Control> e)
        {
            base.OnControlAdded(e);
            int row, column;
            this.LayoutSettings.GetCell(e.Data, out row, out column);

            if (row == -1 && column == -1)
            {
                this.LayoutSettings.SetFirstFreeCell(e.Data);
            }
        }

        /// <summary>
        /// Raises the ControlRemoved event.
        /// </summary>
        /// <param name="e">An ControlEventArgs that contains the event data.</param>
        protected override void OnControlRemoved(ValueEventArgs<Control> e)
        {
            base.OnControlRemoved(e);
            this.LayoutSettings.RemoveControl(e.Data);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}