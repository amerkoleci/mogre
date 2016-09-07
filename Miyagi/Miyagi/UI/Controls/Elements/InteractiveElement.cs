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

    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;
    using Miyagi.UI.Controls.Styles;

    /// <summary>
    /// An element that changes its texture on mouse events.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <typeparam name="TStyle">The type of the style.</typeparam>
    public abstract class InteractiveElement<TOwner, TStyle> : Element<TOwner, TStyle>
        where TOwner : class, IInteractiveElementOwner
        where TStyle : Style, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveElement{TOwner,TStyle}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        protected InteractiveElement(TOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the combined name of the skin.
        /// </summary>
        public virtual string CombinedSkinName
        {
            get
            {
                return this.Owner.CombinedSkinName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is mouse over.
        /// </summary>
        public bool IsMouseOver
        {
            get;
            internal set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Changes the texture.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ChangeTexture(SkinChangingEvent action)
        {
            Skin skin = this.Owner.Skin;
            if (skin != null)
            {
                string subSkin = this.CombinedSkinName + "." + action;
                if (!skin.IsSubSkinDefined(subSkin))
                {
                    subSkin = this.GetAlternativeTexture(action);
                }

                this.Texture = this.Owner.Skin.SubSkins[subSkin];
            }
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <returns>The default texture.</returns>
        protected override Texture GetDefaultTexture()
        {
            return this.Owner.Skin != null ? this.Owner.Skin.SubSkins[this.CombinedSkinName] : null;
        }

        /// <summary>
        /// Handles MouseDown injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.IsMouseOver)
            {
                this.ChangeTexture(SkinChangingEvent.MouseDown);
            }
        }

        /// <summary>
        /// Handles MouseHover injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseHover(MouseEventArgs e)
        {
            if (this.Owner.MiyagiSystem.InputManager.IsAnyMouseButtonDown())
            {
                return;
            }

            bool hit = this.HitTest(e.MouseLocation);
            if (hit && !this.IsMouseOver)
            {
                this.ChangeTexture(SkinChangingEvent.MouseEnter);
            }
            else if (!hit && this.IsMouseOver)
            {
                this.ChangeTexture(SkinChangingEvent.MouseLeave);
            }

            this.IsMouseOver = hit;
        }

        /// <summary>
        /// Handles MouseLeave injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.Owner.MiyagiSystem.InputManager.IsAnyMouseButtonDown())
            {
                return;
            }

            if (this.IsMouseOver)
            {
                this.IsMouseOver = false;
                this.ChangeTexture(SkinChangingEvent.MouseLeave);
            }
        }

        /// <summary>
        /// Handles MouseUp injections.
        /// </summary>
        /// <param name="e">The <see cref="Miyagi.Common.Events.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseOver)
            {
                this.ChangeTexture(SkinChangingEvent.MouseUp);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private string GetAlternativeTexture(SkinChangingEvent oldAction)
        {
            Skin skin = this.Owner.Skin;
            string name = this.CombinedSkinName + ".";
            string retValue = null;

            switch (oldAction)
            {
                case SkinChangingEvent.MouseDown:
                case SkinChangingEvent.MouseEnter:
                    if (skin.IsSubSkinDefined(name + "MouseLeave"))
                    {
                        retValue = name + "MouseLeave";
                    }

                    break;

                case SkinChangingEvent.MouseUp:
                    if (skin.IsSubSkinDefined(name + "MouseLeave"))
                    {
                        retValue = name + "MouseLeave";
                    }
                    else if (skin.IsSubSkinDefined(name + "MouseEnter"))
                    {
                        retValue = name + "MouseEnter";
                    }

                    break;
            }

            return retValue ?? this.CombinedSkinName;
        }

        #endregion Private Methods

        #endregion Methods
    }
}