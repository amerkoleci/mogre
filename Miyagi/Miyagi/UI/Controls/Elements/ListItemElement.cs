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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Resources;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a ListBox item.
    /// </summary>
    public sealed class ListItemElement : Element<ITextElementOwner, ListItemStyle>, INamable, ITextElementOwner
    {
        #region Fields

        private TextElement textElement;
        private TextureElement textureElement;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ListBoxItemElement class.
        /// </summary>
        /// <param name="owner">The owner of the ListBoxItemElement.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public ListItemElement(ITextElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the displayed text.
        /// </summary>
        public string DisplayedText
        {
            get
            {
                return this.Owner.DisplayedText;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
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

                if (this.textureElement != null)
                {
                    yield return this.textureElement;
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
                return this.Owner.Text;
            }
        }

        /// <summary>
        /// Gets the text rectangle.
        /// </summary>
        public Rectangle TextBounds
        {
            get
            {
                return this.Owner.TextBounds;
            }
        }

        /// <summary>
        /// Gets the TextElement.
        /// </summary>
        public TextElement TextElement
        {
            get
            {
                return this.textElement ?? (this.textElement = new TextElement(this, () => this.GetZOrder() + 2));
            }
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public TextureElement TextureElement
        {
            get
            {
                return this.textureElement ?? (this.textureElement = new TextureElement(this, () => this.GetZOrder() + 1));
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
            return base.AreAllSpritesNull() && this.textElement.AreAllSpritesNull() && this.textureElement.AreAllSpritesNull();
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
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected override Texture GetDefaultTexture()
        {
            return this.Style.Texture;
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            switch (name)
            {
                case "Texture":
                    this.TextureElement.Texture = this.Style.Texture;
                    break;
                case "TextStyle":
                    this.SetSubElementStyles();
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
            this.ForEachSubElement(ele => ele.Update());
        }

        #endregion Protected Methods

        #endregion Methods
    }
}