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

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Internals;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element for displaying a button.
    /// </summary>
    public sealed class ButtonElement : InteractiveElement<IInteractiveElementOwner, Style>
    {
        #region Fields

        private string skinName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonElement"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        /// <param name="skinName">Name of the skin.</param>
        public ButtonElement(IInteractiveElementOwner owner, Func<int> zorderGetter, string skinName)
            : base(owner, zorderGetter)
        {
            this.SkinName = skinName;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public override string CombinedSkinName
        {
            get
            {
                return base.CombinedSkinName + this.SkinName;
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
        /// Gets or sets the name of the skin.
        /// </summary>
        /// <value>The name of the skin.</value>
        public string SkinName
        {
            get
            {
                return this.skinName;
            }

            set
            {
                if (this.skinName != value)
                {
                    if (!value.StartsWith("."))
                    {
                        value = "." + value;
                    }

                    this.skinName = value;
                    if (this.Owner != null && this.Owner.Skin != null)
                    {
                        this.Texture = this.Owner.Skin.SubSkins[value];
                    }
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

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
        /// Raises the <see cref="ButtonElement.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.IsMouseOver)
            {
                this.Owner.MiyagiSystem.GUIManager.GrabbedControl = null;
                if (this.MouseDown != null)
                {
                    this.MouseDown(this, e);
                }
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
            }

            if (this.UpdateType.IsFlagSet(UpdateTypes.Texture))
            {
                this.SetSpriteTexture();
            }
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