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
    using System.ComponentModel;

    using Miyagi.Common.Data;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// Represents a single tab page in a <see cref="TabControl"/>.
    /// </summary>
    public class TabPage : ScrollableControl, ITabElementOwner
    {
        #region Fields

        private bool selected;
        private TabElement tabElement;
        private string title;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage"/> class.
        /// </summary>
        public TabPage()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage"/> class.
        /// </summary>
        /// <param name="name">The name of the TabPage.</param>
        public TabPage(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the edges of the parent to which the control is bound.
        /// </summary>
        /// <value>
        /// A bitwise combination of <see cref="AnchorStyles"/>. The default is <b>Top</b> and <b>Left</b>.
        /// </value>
        public override AnchorStyles Anchor
        {
            get
            {
                return AnchorStyles.None;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will resize according to the setting of AutoSizeMode.
        /// </summary>
        public override bool AutoSize
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.
        /// </summary>
        /// <value>One of the DockStyle values. The default is None.</value>
        public override DockStyle Dock
        {
            get
            {
                return DockStyle.None;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TabPage"/> is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.Parent != null && this.selected;
            }

            internal set
            {
                if (this.Parent == null)
                {
                    return;
                }

                if (this.selected != value)
                {
                    this.selected = value;

                    if (value)
                    {
                        this.TabControl.SelectedTab = this;
                    }

                    this.OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the tab style.
        /// </summary>
        public TabStyle TabStyle
        {
            get
            {
                return this.TabElement.Style;
            }

            set
            {
                this.TabElement.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Localizable(true)]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.TabElement.TextElement.UpdateType |= UpdateTypes.Text;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and its children are visible.
        /// </summary>
        /// <value>If the control is visible true; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                if (this.Parent == null)
                {
                    return false;
                }

                return this.Parent.Visible && this.Selected;
            }

            set
            {
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the tab element.
        /// </summary>
        protected internal TabElement TabElement
        {
            get
            {
                return this.tabElement ?? (this.tabElement = new TabElement(this, () => (this.Parent.ZOrder * 10) + 4));
            }
        }

        #endregion Protected Internal Properties

        #region Private Properties

        private TabControl TabControl
        {
            get
            {
                return (TabControl)this.Parent;
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Internal Methods

        internal void UpdateBounds()
        {
            this.SetBounds(this.TabControl.GetTabPageLocation(), this.TabControl.GetTabPageSize());
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Resizes the control.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
            this.TabStyle.Resize(widthFactor, heightFactor);
        }

        /// <summary>
        /// Raises the <see cref="Control.ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.Parent != null)
            {
                this.UpdateBounds();
            }
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
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetBounds(Point location, Size size)
        {
            base.SetBoundsCore(location.X, location.Y, size.Width, size.Height, BoundsSpecified.All);
        }

        #endregion Private Methods

        #endregion Methods
    }
}