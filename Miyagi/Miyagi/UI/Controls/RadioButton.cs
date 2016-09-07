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
    /// A RadioButton control.
    /// </summary>
    public class RadioButton : StateButton
    {
        #region Fields

        private bool isChecked;
        private string group = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RadioButton class.
        /// </summary>
        public RadioButton()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadioButton class.
        /// </summary>
        /// <param name="name">The name of the RadioButton.</param>
        public RadioButton(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the Checked property is changed.
        /// </summary>
        public event EventHandler CheckedChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        public string Group
        {
            get 
            {
                return group;
            }
            set { group = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The check state of the control.</value>
        public bool Checked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.ThrowIfDisposed();

                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.ActiveState = value ? "Checked" : string.Empty;
                    this.OnCheckedChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Checked");
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="RadioButton.CheckedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (this.isChecked)
            {
                // get the siblings
                ControlCollection cc = this.GetParentCollection();

                if (cc != null)
                {
                    // uncheck all RadioButtons
                    foreach (Control c in cc)
                    {
                        RadioButton rb = c as RadioButton;
                        if (rb != null 
                            && rb != this
                            && rb.group == group)
                        {
                            rb.Checked = false;
                        }
                    }
                }
            }

            if (this.CheckedChanged != null)
            {
                this.CheckedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (!this.isChecked)
            {
                this.Checked = true;
            }

            base.OnMouseClick(e);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}