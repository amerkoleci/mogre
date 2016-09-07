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

    using Miyagi.Common.Events;

    /// <summary>
    /// A CheckBox control.
    /// </summary>
    public class CheckBox : StateButton
    {
        #region Fields

        private CheckState checkState;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CheckBox class.
        /// </summary>
        public CheckBox()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CheckBox class.
        /// </summary>
        /// <param name="name">The name of the CheckBox.</param>
        public CheckBox(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Checked"/> property is changed.
        /// </summary>
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Occurs when the <see cref="CheckState"/> property is changed.
        /// </summary>
        public event EventHandler CheckStateChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The check state of the control.</value>
        public bool Checked
        {
            get
            {
                return this.checkState != CheckState.Unchecked;
            }

            set
            {
                this.ThrowIfDisposed();

                if (this.Checked != value)
                {
                    this.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                    this.OnPropertyChanged("Checked");
                }
            }
        }

        /// <summary>
        /// Gets or sets a CheckState enum representing the current state of the CheckBox.
        /// </summary>
        public CheckState CheckState
        {
            get
            {
                return this.checkState;
            }

            set
            {
                if (this.checkState != value)
                {
                    if (!this.ThreeState && value == CheckState.Indeterminate)
                    {
                        value = CheckState.Checked;
                    }

                    switch (value)
                    {
                        case CheckState.Checked:
                            this.ActiveState = "Checked";
                            break;
                        case CheckState.Indeterminate:
                            this.ActiveState = "Indeterminate";
                            break;
                        case CheckState.Unchecked:
                            this.ActiveState = string.Empty;
                            break;
                    }

                    bool raiseCheckedChange = this.checkState == CheckState.Unchecked || value == CheckState.Unchecked;

                    this.checkState = value;

                    if (raiseCheckedChange)
                    {
                        this.OnCheckedChanged(EventArgs.Empty);
                    }

                    this.OnCheckStateChanged(EventArgs.Empty);
                    this.OnPropertyChanged("CheckState");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the CheckBox can be set to the Indeterminate state.
        /// </summary>
        public bool ThreeState
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="CheckBox.CheckedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (this.CheckedChanged != null)
            {
                this.CheckedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBox.CheckStateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCheckStateChanged(EventArgs e)
        {
            if (this.CheckStateChanged != null)
            {
                this.CheckStateChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseClick event.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs.</param>
        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (!this.ThreeState)
            {
                this.Checked = !this.Checked;
            }
            else
            {
                switch (this.checkState)
                {
                    case CheckState.Checked:
                        this.CheckState = CheckState.Indeterminate;
                        break;
                    case CheckState.Indeterminate:
                        this.CheckState = CheckState.Unchecked;
                        break;
                    case CheckState.Unchecked:
                        this.CheckState = CheckState.Checked;
                        break;
                }
            }

            base.OnMouseClick(e);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}