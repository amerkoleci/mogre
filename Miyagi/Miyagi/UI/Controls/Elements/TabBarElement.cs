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
namespace Miyagi.UI.Controls.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a tabbar.
    /// </summary>
    public class TabBarElement : Element<ITabBarElementOwner, TabBarStyle>, IInteractiveElementOwner
    {
        #region Fields

        private readonly List<TabElement> tabElements;

        private ButtonElement leftArrowButtonElement;
        private int oldExtent;
        private ButtonElement rightArrowButtonElement;
        private int tabOffset;
        private int tabOffsetStep;
        private int totalTabWidth;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBarElement"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public TabBarElement(ITabBarElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.tabElements = new List<TabElement>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public string CombinedSkinName
        {
            get
            {
                return this.Owner.CombinedSkinName + ".TabBar";
            }
        }

        /// <summary>
        /// Gets or sets the extent changed callback.
        /// </summary>
        public Action<int, int> ExtentChangedCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tab bar is dirty.
        /// </summary>
        public bool IsTabBarDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the skin.
        /// </summary>
        /// <value>The skin.</value>
        public Skin Skin
        {
            get
            {
                return this.Owner.Skin;
            }
        }

        /// <summary>
        /// Gets the sub elements.
        /// </summary>
        public override IEnumerable<IElement> SubElements
        {
            get
            {
                if (this.tabElements != null)
                {
                    int count = this.tabElements.Count;
                    for (int i = 0; i < count; i++)
                    {
                        yield return this.tabElements[i];
                    }
                }

                if (this.leftArrowButtonElement != null)
                {
                    yield return this.leftArrowButtonElement;
                }

                if (this.rightArrowButtonElement != null)
                {
                    yield return this.rightArrowButtonElement;
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the location of the control relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.Owner.GetLocationInViewport();
        }

        /// <summary>
        /// Removes the buttons.
        /// </summary>
        public void RemoveButtons()
        {
            if (this.leftArrowButtonElement != null)
            {
            this.leftArrowButtonElement.Dispose();
            this.leftArrowButtonElement = null;
            }

             if (this.rightArrowButtonElement != null)
             {
             this.rightArrowButtonElement.Dispose();
             this.rightArrowButtonElement = null;
             }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            return new Rectangle(this.Owner.GetLocationInViewport(), new Size(this.Owner.Size.Width, this.Style.Extent));
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected override Texture GetDefaultTexture()
        {
            return this.Owner.Skin.SubSkins[this.CombinedSkinName];
        }

        /// <summary>
        /// Handles MouseDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            TabElement selectedTab = null;

            var topElement = this.GetTopElement(e.MouseLocation);

            if (topElement != null)
            {
                topElement.InjectMouseDown(e);
                selectedTab = topElement as TabElement;
            }

            if (selectedTab != null)
            {
                foreach (var tabElement in this.tabElements)
                {
                    tabElement.Style.TextStyle.ForegroundColour = this.Style.ForegroundColour;
                }

                this.Owner.SelectedTab = selectedTab.Owner;
                selectedTab.Style.TextStyle.ForegroundColour = this.Style.SelectColour;
            }
        }

        /// <summary>
        /// Handles MouseEnter injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var topElement = this.GetTopElement(e.MouseLocation);

            if (topElement != null)
            {
                topElement.InjectMouseEnter(e);
            }
        }

        /// <summary>
        /// Handles MouseHeld injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHeld(MouseButtonEventArgs e)
        {
            if (this.leftArrowButtonElement != null)
            {
                this.leftArrowButtonElement.InjectMouseDown(e);
            }

            if (this.rightArrowButtonElement != null)
            {
                this.rightArrowButtonElement.InjectMouseDown(e);
            }
        }

        /// <summary>
        /// Handles MouseHover injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            var topElement = this.GetTopElement(e.MouseLocation);

            if (topElement is ButtonElement)
            {
                topElement.InjectMouseHover(e);
            }
            else
            {
                if (this.leftArrowButtonElement != null)
                {
                    this.leftArrowButtonElement.InjectMouseLeave(e);
                }

                if (this.rightArrowButtonElement != null)
                {
                    this.rightArrowButtonElement.InjectMouseLeave(e);
                }
            }

            foreach (var tabElement in this.tabElements)
            {
                tabElement.InjectMouseHover(e);
                tabElement.Style.TextStyle.ForegroundColour =
                    tabElement.Owner.Selected
                        ? this.Style.SelectColour
                        : tabElement.IsMouseOver
                              ? this.Style.HoverColour
                              : this.Style.ForegroundColour;
            }
        }

        /// <summary>
        /// Handles MouseLeave injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            foreach (var tabElement in this.tabElements)
            {
                tabElement.InjectMouseLeave(e);
                tabElement.Style.TextStyle.ForegroundColour = tabElement.Owner.Selected ? this.Style.SelectColour : this.Style.ForegroundColour;
            }
        }

        /// <summary>
        /// Handles MouseUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            var topElement = this.GetTopElement(e.MouseLocation);

            if (topElement != null)
            {
                topElement.InjectMouseUp(e);
            }
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Extent":
                    if (this.ExtentChangedCallback != null)
                    {
                        this.ExtentChangedCallback(this.oldExtent, this.Style.Extent);
                    }

                    this.RemoveSprite();
                    this.RemoveButtons();
                    this.IsTabBarDirty = true;
                    break;

                case "Alignment":
                case "Font":
                case "Mode":
                    this.IsTabBarDirty = true;
                    break;
            }
        }

        /// <summary>
        /// Handles changing style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanging(string name)
        {
            switch (name)
            {
                case "Extent":
                    this.oldExtent = this.Style.Extent;
                    break;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        protected override void UpdateCore()
        {
            if (this.Texture != null)
            {
                if (this.Sprite == null)
                {
                    this.CreateSprite();
                    this.UpdateSpriteCrop();
                }

                if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
                {
                    this.SetSpriteTexture();
                }
            }

            if (this.IsTabBarDirty)
            {
                this.FillTabBar();
            }

            this.ForEachSubElement(ele => ele.Update());
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Creates the sprite of the element.
        /// </summary>
        private void CreateSprite()
        {
            var rec = this.GetBounds().ToScreenCoordinates(this.ViewportSize);
            this.PrepareSprite(new Quad(rec, this.CurrentUV));
            this.Sprite.SetTexture(this.CurrentFrame.FileName);
        }

        private void FillTabBar()
        {
            if (this.tabElements.Count > 0)
            {
                foreach (var tabElement in this.tabElements)
                {
                    tabElement.RemoveSprite();
                }

                this.tabElements.Clear();
            }

            foreach (var tabPage in this.Owner.TabPages)
            {
                var tabElement = tabPage.TabElement;
                tabElement.Style.TextStyle.Font = this.Style.Font;
                tabElement.Style.TextStyle.Alignment = this.Style.Alignment;
                tabElement.Style.TextStyle.ForegroundColour = tabElement.Owner.Selected ? this.Style.SelectColour : this.Style.ForegroundColour;
                this.tabElements.Add(tabElement);
            }

            this.FormatTabs();

            this.IsTabBarDirty = false;
        }

        private void FormatTabs()
        {
            int tabX = this.tabOffset;
            foreach (var element in this.tabElements)
            {
                int width;
                var tabElement = element;

                switch (this.Style.Mode)
                {
                    case TabMode.Fill:
                        width = this.Owner.Size.Width / this.tabElements.Count;
                        break;
                    case TabMode.AutoSize:
                        width = tabElement.TextElement.Style.Font.MeasureString(tabElement.Text).Width;
                        break;
                    case TabMode.FixedSize:
                        width = tabElement.Style.FixedSize;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                tabElement.Size = new Size(width, this.Style.Extent);
                tabElement.Location = new Point(tabX, -this.Style.Extent);
                tabX += width;
            }

            this.totalTabWidth = tabX - this.tabOffset;
            this.tabOffsetStep = (this.totalTabWidth - this.Owner.Size.Width) / 10;

            if (this.tabOffset < 0)
            {
                if (this.leftArrowButtonElement == null)
                {
                    this.leftArrowButtonElement = new ButtonElement(this, () => this.GetZOrder() + 3, "LeftButton")
                                                  {
                                                      Size = new Size(this.Style.Extent, this.Style.Extent)
                                                  };
                    this.leftArrowButtonElement.MouseDown += this.LeftArrowButtonElementMouseDown;
                }
            }
            else if (this.leftArrowButtonElement != null)
            {
                this.leftArrowButtonElement.MouseDown -= this.LeftArrowButtonElementMouseDown;
                this.leftArrowButtonElement.Dispose();
                this.leftArrowButtonElement = null;
            }

            if (tabX > this.Owner.Size.Width)
            {
                if (this.rightArrowButtonElement == null)
                {
                    var x = this.GetBounds().Width - this.Style.Extent;
                    this.rightArrowButtonElement = new ButtonElement(this, () => this.GetZOrder() + 3, "RightButton")
                                                   {
                                                       Location = new Point(x, 0),
                                                       Size = new Size(this.Style.Extent, this.Style.Extent)
                                                   };
                    this.rightArrowButtonElement.MouseDown += this.RightArrowButtonElementMouseDown;
                }
            }
            else if (this.rightArrowButtonElement != null)
            {
                this.rightArrowButtonElement.MouseDown -= this.LeftArrowButtonElementMouseDown;
                this.rightArrowButtonElement.Dispose();
                this.rightArrowButtonElement = null;
            }
        }

        private IElement GetTopElement(Point loc)
        {
            if (this.leftArrowButtonElement != null && this.leftArrowButtonElement.HitTest(loc))
            {
                return this.leftArrowButtonElement;
            }

            if (this.rightArrowButtonElement != null && this.rightArrowButtonElement.HitTest(loc))
            {
                return this.rightArrowButtonElement;
            }

            return this.tabElements.FirstOrDefault(element => element.HitTest(loc));
        }

        private void LeftArrowButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.tabOffset += this.tabOffsetStep;
            this.IsTabBarDirty = true;
        }

        private void RightArrowButtonElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.tabOffset -= this.tabOffsetStep;
            this.IsTabBarDirty = true;
        }

        #endregion Private Methods

        #endregion Methods
    }
}