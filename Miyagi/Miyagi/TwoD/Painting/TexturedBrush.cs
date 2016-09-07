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
namespace Miyagi.TwoD.Painting
{
    using Miyagi.Common.Data;

    /// <summary>
    /// A brush representing a texture.
    /// </summary>
    public class TexturedBrush : IBrush
    {
        #region Fields

        private readonly string texture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturedBrush"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public TexturedBrush(string texture)
        {
            this.texture = texture;
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the brush to the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public void Apply(TwoDSprite sprite)
        {
            sprite.SetTexture(this.texture);
            sprite.SetColour(Colours.White);
        }

        #endregion Public Methods

        #endregion Methods
    }
}