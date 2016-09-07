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

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a tab.
    /// </summary>
    public sealed class TabElement : InteractiveElement<ITabElementOwner, TabStyle>, ITextElementOwner
    {
        #region Fields

        private TextElement textElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabElement"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public TabElement(ITabElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the name of the combined skin.
        /// </summary>
        /// <value>The name of the combined skin.</value>
        public override string CombinedSkinName
        {
            get
            {
                return base.CombinedSkinName + ".Tab";
            }
        }

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public string DisplayedText
        {
            get
            {
                return this.Text;
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public override Size Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        public override IEnumerable<IElement> SubElements
        {
            get
            {
                if (this.textElement != null)
                {
                    yield return this.textElement;
                }
            }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.Owner.Title;
            }
        }

        /// <summary>
        /// Gets the text rectangle.
        /// </summary>
        public Rectangle TextBounds
        {
            get
            {
                return new Rectangle(this.Location, this.Size);
            }
        }

        /// <summary>
        /// Gets the TextElement.
        /// </summary>
        public TextElement TextElement
        {
            get
            {
                return this.textElement ?? (this.textElement = new TextElement(this, () => this.GetZOrder() + 1));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TabElement"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return this.Owner.Parent != null && this.Owner.Parent.Visible;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        public override bool AreAllSpritesNull()
        {
            return base.AreAllSpritesNull() && this.textElement.AreAllSpritesNull();
        }

        /// <summary>
        /// Gets the location relative to its viewport origin.
        /// </summary>
        /// <returns>A <see cref="Point"/> representing the location of the control relative to its viewport origin.</returns>
        public Point GetLocationInViewport()
        {
            return this.Owner.GetLocationInViewport();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            return new Rectangle(this.Location + this.Owner.GetLocationInViewport(), this.Size);
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "TextStyle":
                    this.SetSubElementStyles();
                    break;
                case "FixedSize":
                    if (this.OwningControl.Parent != null)
                    {
                        ((TabControl)this.OwningControl.Parent).TabBarElement.IsTabBarDirty = true;
                    }

                    break;
            }
        }

        /// <summary>
        /// Sets the style of subelements.
        /// </summary>
        protected override void SetSubElementStyles()
        {
            if (this.TextElement.Style != null)
            {
                this.TextElement.Style.Font = null;
            }

            if (this.Style != null)
            {
                this.TextElement.Style = this.Style.TextStyle;
            }
        }

        /// <summary>
        /// Updates the ListBoxItemElement.
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
             }

            if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
            {
                this.SetSpriteTexture();
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

        #endregion Private Methods

        #endregion Methods
    }
}