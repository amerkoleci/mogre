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

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.UI.Controls.Elements;

    /// <summary>
    /// Represents the control that displays a header that has a collapsible window that displays content.
    /// </summary>
    public class Expander : SkinnedControl
    {
        #region Fields

        private ButtonElement buttonElement;
        private bool expanded;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Expander"/> class.
        /// </summary>
        public Expander()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Expander"/> class.
        /// </summary>
        /// <param name="name">The name of the SkinnedControl.</param>
        public Expander(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Expanded"/> property changes.
        /// </summary>
        public event EventHandler ExpandedChanged;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a Rectangle representing the text area.
        /// </summary>
        public override Rectangle TextBounds
        {
            get
            {
                return new Rectangle(Point.Empty, this.MinSize);
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the button element.
        /// </summary>
        protected ButtonElement ButtonElement
        {
            get
            {
                if (this.buttonElement == null)
                {
                    this.buttonElement = new ButtonElement(this, () => (this.ZOrder * 10) + 1, "Collapsed")
                                         {
                                             Size = this.MinSize
                                         };
                    this.buttonElement.MouseDown += this.ButtonElementMouseDown;
                }

                return this.buttonElement;
            }
        }

        /// <summary>
        /// Gets the client location.
        /// </summary>
        protected override Point ClientLocation
        {
            get
            {
                return base.ClientLocation + new Point(0, this.MinSize.Height);
            }
        }

        /// <summary>
        /// Gets a list of elements.
        /// </summary>
        protected override IList<IElement> Elements
        {
            get
            {
                var retValue = base.Elements;
                if (this.buttonElement != null)
                {
                    retValue.Add(this.buttonElement);
                }

                return retValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Expander"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
        protected bool Expanded
        {
            get
            {
                return this.expanded;
            }

            set
            {
                if (this.expanded != value)
                {
                    this.expanded = value;
                    if (value)
                    {
                        this.OnExpanded();
                    }
                    else
                    {
                        this.OnCollapsed();
                    }

                    this.OnExpandedChanged(EventArgs.Empty);
                }
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Calculates a client area based on the size of a control.
        /// </summary>
        /// <param name="size">The size of the control.</param>
        /// <returns>A size representing said client area.</returns>
        protected override Size ClientSizeFromSize(Size size)
        {
            return !this.expanded ? base.ClientSizeFromSize(size) - new Size(0, this.MinSize.Height) : base.ClientSizeFromSize(size);
        }

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            if (this.buttonElement == null)
            {
                this.buttonElement.Dispose();
                this.buttonElement = null;
            }
        }

        /// <summary>
        /// Called when <see cref="Expanded"/> is set to <c>false</c>.
        /// </summary>
        protected virtual void OnCollapsed()
        {
            this.ButtonElement.SkinName = "Collapsed";
            this.UpdateBounds();
        }

        /// <summary>
        /// Called when <see cref="Expanded"/> is set to <c>true</c>.
        /// </summary>
        protected virtual void OnExpanded()
        {
            this.ButtonElement.SkinName = "Expanded";
            this.UpdateBounds();
        }

        /// <summary>
        /// Raises the <see cref="Expander.ExpandedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnExpandedChanged(EventArgs e)
        {
            if (this.ExpandedChanged != null)
            {
                this.ExpandedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MaxSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMaxSizeChanged(EventArgs e)
        {
            base.OnMaxSizeChanged(e);
            if (this.expanded)
            {
                this.UpdateBounds();
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MinSizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMinSizeChanged(EventArgs e)
        {
            base.OnMinSizeChanged(e);
            this.UpdateClientSize();
            if (!this.expanded)
            {
                this.UpdateBounds();
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.ButtonElement.InjectMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.ButtonElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ButtonElement.InjectMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.ButtonElement.InjectMouseUp(e);
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="Control.Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Control.Top"/> property value of the control.</param>
        /// <param name="newWidth">The new <see cref="Control.Width"/> property value of the control.</param>
        /// <param name="newHeight">The new <see cref="Control.Height"/> property value of the control.</param>
        /// <param name="specified">A bitwise combination of <see cref="BoundsSpecified"/> values.</param>
        protected override void SetBoundsCore(int x, int y, int newWidth, int newHeight, BoundsSpecified specified)
        {
            if (this.expanded)
            {
                newWidth = this.MaxSize.Width;
                newHeight = this.MaxSize.Height;
            }
            else
            {
                newWidth = this.MinSize.Width;
                newHeight = this.MinSize.Height;
            }

            base.SetBoundsCore(x, y, newWidth, newHeight, specified);
        }

        /// <summary>
        /// Calculates the size of the control based on the size of the client area.
        /// </summary>
        /// <param name="size">The size of the client area.</param>
        /// <returns>A size for the whole control.</returns>
        protected override Size SizeFromClientSize(Size size)
        {
            return !this.expanded ? base.ClientSizeFromSize(size) + new Size(0, this.MinSize.Height) : base.ClientSizeFromSize(size);
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            this.ButtonElement.Update(this.DeltaLocation, this.DeltaSize);
            base.UpdateCore();
        }

        #endregion Protected Methods

        #region Private Methods

        private void ButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Expanded = !this.Expanded;
        }

        private void UpdateBounds()
        {
            this.SetBoundsCore(this.Left, this.Top, 0, 0, BoundsSpecified.Size);
        }

        #endregion Private Methods

        #endregion Methods
    }
}