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
    using Miyagi.Common;
    using Miyagi.Common.Events;
    using Miyagi.Common.Resources;

    /// <summary>
    /// A Button control.
    /// </summary>
    public class Button : SkinnedControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Button class.
        /// </summary>
        public Button()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Button class.
        /// </summary>
        /// <param name="name">The name of the Button.</param>
        public Button(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Handles skin events.
        /// </summary>
        /// <param name="action">The event.</param>
        protected virtual void ChangeTexture(SkinChangingEvent action)
        {
            Skin skin = this.Skin;
            if (skin != null)
            {
                string subSkin = this.CombinedSkinName + "." + action;
                if (!skin.IsSubSkinDefined(subSkin))
                {
                    subSkin = this.GetAlternativeTexture(action);
                }

                this.ChangeTexture(subSkin, action.ToString());
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.ChangeTexture(SkinChangingEvent.MouseDown);
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.ChangeTexture(SkinChangingEvent.MouseEnter);
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.ChangeTexture(SkinChangingEvent.MouseLeave);
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.ChangeTexture(SkinChangingEvent.MouseUp);
            base.OnMouseUp(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private string GetAlternativeTexture(SkinChangingEvent oldAction)
        {
            Skin skin = this.Skin;
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