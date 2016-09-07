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

    /// <summary>
    /// An element for displaying a texture that is surrounded by a border.
    /// </summary>
    public class SkinElement : TextureElement
    {
        #region Fields

        private readonly ISkinElementOwner owner;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SkinElement class.
        /// </summary>
        /// <param name="owner">The owner of the element.</param>
        /// <param name="zorderGetter">The zorder getter.</param>
        public SkinElement(ISkinElementOwner owner, Func<int> zorderGetter)
            : base(owner, zorderGetter)
        {
            this.owner = owner;
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns>A Rectangle describing the bounds of the element.</returns>
        protected override Rectangle GetBounds()
        {
            var retValue = base.GetBounds();
            var border = this.owner.BorderElement;
            if (border != null)
            {
                if (border.Texture != null)
                {
                    var thickness = border.Style.Thickness;
                    retValue = new Rectangle(
                        retValue.X + thickness.Left,
                        retValue.Y + thickness.Top,
                        retValue.Width - thickness.Horizontal,
                        retValue.Height - thickness.Vertical);
                }
            }

            return retValue;
        }

        /// <summary>
        /// Handles changed style properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected override void OnStylePropertyChanged(string name)
        {
            if (name == "Texture")
            {
                var owner = this.Owner as ISkinElementOwner;
                if (owner != null && owner.BorderElement != null)
                {
                    owner.BorderElement.UpdateType |= UpdateTypes.Texture;
                }
            }

            base.OnStylePropertyChanged(name);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}