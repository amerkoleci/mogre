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
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.UI.Controls.Elements;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// Manages a related set of tab pages.
    /// </summary>
    public class TabControl : SkinnedControl, ITabBarElementOwner
    {
        #region Fields

        private int selectedIndex;
        private TabBarElement tabBarElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        /// <param name="name">The name of the TabControl.</param>
        public TabControl(string name)
            : base(name)
        {
            this.selectedIndex = -1;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property changes.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        #endregion Events

        #region Properties

        #region Explicit Interface Properties

        ITabElementOwner ITabBarElementOwner.SelectedTab
        {
            get
            {
                return this.SelectedTab;
            }

            set
            {
                this.SelectedTab = (TabPage)value;
            }
        }

        #endregion Explicit Interface Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the index of the selected tab.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;
                    this.OnSelectedIndexChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        /// <value>The selected tab.</value>
        public TabPage SelectedTab
        {
            get
            {
                if (this.selectedIndex > -1 && this.selectedIndex < this.TabCount)
                {
                    return (TabPage)this.Controls[this.SelectedIndex];
                }

                return null;
            }

            set
            {
                this.SelectedIndex = this.Controls.IndexOf(value);
            }
        }

        /// <summary>
        /// Gets or sets the tab bar style.
        /// </summary>
        /// <value>The tab bar style.</value>
        public TabBarStyle TabBarStyle
        {
            get
            {
                return this.TabBarElement.Style;
            }

            set
            {
                this.TabBarElement.Style = value;
            }
        }

        /// <summary>
        /// Gets the tab count.
        /// </summary>
        /// <value>The tab count.</value>
        public int TabCount
        {
            get
            {
                return this.Controls.Count();
            }
        }

        /// <summary>
        /// Gets the tab pages.
        /// </summary>
        /// <value>The tab pages.</value>
        [SerializerOptions(Ignore = true)]
        public IEnumerable<TabPage> TabPages
        {
            get
            {
                return this.Controls.Cast<TabPage>();
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the tab bar element.
        /// </summary>
        /// <value>The tab bar element.</value>
        protected internal TabBarElement TabBarElement
        {
            get
            {
                return this.tabBarElement ?? (this.tabBarElement = new TabBarElement(this, () => (this.ZOrder * 10) + 3)
                                                                   {
                                                                       ExtentChangedCallback = this.OnTabBarExtentChanged
                                                                   });
            }
        }

        #endregion Protected Internal Properties

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
                if (this.tabBarElement != null)
                {
                    retValue.Add(this.tabBarElement);
                }

                return retValue;
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Internal Methods

        internal Point GetTabPageLocation()
        {
            return new Point(0, this.TabBarStyle.Extent);
        }

        internal Size GetTabPageSize()
        {
            return new Size(this.Width, this.Height - this.TabBarStyle.Extent);
        }

        #endregion Internal Methods

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
                this.tabBarElement = null;
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
            this.TabBarStyle.Resize(widthFactor, heightFactor);
            this.TabBarElement.IsTabBarDirty = true;
            this.TabBarElement.RemoveButtons();
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        /// <exception cref="InvalidOperationException">Added control is not a TabPage.</exception>
        protected override void OnControlAdded(ValueEventArgs<Control> e)
        {
            var tabPage = e.Data as TabPage;
            if (tabPage == null)
            {
                throw new InvalidOperationException("Added control is not a TabPage.");
            }

            tabPage.Selected = this.selectedIndex == -1;

            this.TabBarElement.IsTabBarDirty = true;
            base.OnControlAdded(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.ControlRemoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> instance containing the event data.</param>
        protected override void OnControlRemoved(ValueEventArgs<Control> e)
        {
            this.SelectedIndex = this.TabCount > 0 ? 0 : -1;

            this.TabBarElement.IsTabBarDirty = true;
            base.OnControlRemoved(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.TabBarElement.InjectMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.TabBarElement.InjectMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHeld"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            base.OnMouseHeld(e);
            this.TabBarElement.InjectMouseHeld(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            base.OnMouseHover(e);
            this.TabBarElement.InjectMouseHover(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.TabBarElement.InjectMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.TabBarElement.InjectMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="TabControl.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            foreach (var tabPage in this.TabPages)
            {
                tabPage.Selected = this.Controls.IndexOf(tabPage) == this.selectedIndex;
            }

            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
        }

        /// <summary>
        /// Called when the tab bar extent changes.
        /// </summary>
        /// <param name="oldExtent">The old extent.</param>
        /// <param name="newExtent">The new extent.</param>
        protected virtual void OnTabBarExtentChanged(int oldExtent, int newExtent)
        {
            foreach (var tabPage in this.TabPages)
            {
                tabPage.UpdateBounds();
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        protected override void UpdateCore()
        {
            base.UpdateCore();
            this.TabBarElement.Update(this.DeltaLocation, this.DeltaSize);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}