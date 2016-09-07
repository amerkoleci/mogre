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
    using System.Collections.Generic;

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// A ProgressBar control.
    /// </summary>
    public class ProgressBar : SkinnedControl, IProgressBarElementOwner
    {
        #region Fields

        private int max;
        private int min;
        private ProgressBarElement progressBarElement;
        private int progressBarValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProgressBar class.
        /// </summary>
        public ProgressBar()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProgressBar class.
        /// </summary>
        /// <param name="name">The name of the ProgressBar.</param>
        public ProgressBar(string name)
            : base(name)
        {
            this.Max = 100;
            this.AutoOrientation = true;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Styles.ProgressBarStyle.Orientation"/> property is determinated automatically.
        /// </summary>
        public bool AutoOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum value of the ProgressBar.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <remarks>Max will be set to Min if it is set to a value smaller than Min.</remarks>
        public int Max
        {
            get
            {
                return this.max;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.max != value)
                {
                    this.max = value >= this.min ? value : this.min;

                    if (this.progressBarValue > this.max)
                    {
                        this.Value = this.max;
                    }

                    this.ProgressBarElement.UpdateType |= UpdateTypes.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the ProgressBar.
        /// </summary>
        /// <remarks>Min will be set to Max if it is set to a value larger than Max.</remarks>
        public int Min
        {
            get
            {
                return this.min;
            }

            set
            {
                this.ThrowIfDisposed();
                if (this.min != value)
                {
                    this.min = value <= this.max ? value : this.max;

                    if (this.progressBarValue < this.min)
                    {
                        this.Value = this.min;
                    }

                    this.ProgressBarElement.UpdateType |= UpdateTypes.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets the style of the bar.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get
            {
                return this.ProgressBarElement.Style;
            }

            set
            {
                this.ThrowIfDisposed();
                this.ProgressBarElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the ProgressBar.
        /// </summary>
        /// <value>The value of the ProgressBar.</value>
        /// <remarks>Value will be set to Max if the value is larger than Max, and set to Min if the value is smaller than Min.</remarks>
        public int Value
        {
            get
            {
                return this.progressBarValue;
            }

            set
            {
                this.ThrowIfDisposed();
                value = value.Clamp(this.min, this.max);
                if (this.progressBarValue != value)
                {
                    this.progressBarValue = value;
                    this.OnValueChanged(EventArgs.Empty);
                    this.OnPropertyChanged("Value");
                }
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        /// <value>A list of elements.</value>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.progressBarElement != null)
                {
                    retValue.Add(this.progressBarElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets the bar.
        /// </summary>
        /// <value>A BarElement representing the bar.</value>
        protected ProgressBarElement ProgressBarElement
        {
            get
            {
                return this.progressBarElement ?? (this.progressBarElement = new ProgressBarElement(this, () => (this.ZOrder * 10) + 3));
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.progressBarElement = null;
            }
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            this.ProgressBarStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnSizeChanged(ChangedValueEventArgs<Size> e)
        {
            base.OnSizeChanged(e);

            if (this.AutoOrientation)
            {
                this.ProgressBarStyle.Orientation = e.NewValue.Width > e.NewValue.Height ? Orientation.Horizontal : Orientation.Vertical;
            }
        }

        /// <summary>
        /// Raises the <see cref="ProgressBar.ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            this.ProgressBarElement.UpdateType |= UpdateTypes.Size;

            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();
            this.ProgressBarElement.Update(this.DeltaLocation, this.DeltaSize);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}