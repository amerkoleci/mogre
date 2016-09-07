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
namespace Miyagi.TwoD
{
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;

    /// <summary>
    /// A representation of a Sprite for 2D elements.
    /// </summary>
    public class TwoDSprite : Sprite
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TwoDSprite class.
        /// </summary>
        /// <param name="spriteRenderer">The owning <see cref="ISpriteRenderer"/>.</param>
        /// <param name="primitives">The primitives.</param>
        public TwoDSprite(ISpriteRenderer spriteRenderer, params Primitive[] primitives)
            : base(spriteRenderer, primitives)
        {
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the transformation.
        /// </summary>
        public void ApplyTransformation()
        {
            this.ForEachPrimitive(q => q.ApplyTransformation(this.ViewportAspectRatio));
            this.SpriteRenderer.BufferDirty = true;
        }

        /// <summary>
        /// Rotates the sprite.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="rotationPivot">The pivot point.</param>
        /// <param name="doTransform">Indicates whether the rotation should be applied directly.</param>
        public void Rotate(float angle, Point rotationPivot, bool doTransform = false)
        {
            PointF pivot = rotationPivot.ToScreenCoordinates(this.ViewportSize);
            this.ForEachPrimitive(q => q.Rotate(angle, pivot));
            if (doTransform)
            {
                this.ApplyTransformation();
            }
        }

        /// <summary>
        /// Scales the sprite.
        /// </summary>
        /// <param name="scaleFactor">The scale factor..</param>
        /// <param name="scalePivot">The pivot point.</param>
        /// <param name="doTransform">Indicates whether the scaling should be applied directly.</param>
        public void Scale(PointF scaleFactor, Point scalePivot, bool doTransform = false)
        {
            PointF pivot = scalePivot.ToScreenCoordinates(this.ViewportSize);
            this.ForEachPrimitive(q => q.Scale(scaleFactor, pivot));
            if (doTransform)
            {
                this.ApplyTransformation();
            }
        }

        /// <summary>
        /// Skews the sprite.
        /// </summary>
        /// <param name="skewFactor">The skew factor.</param>
        /// <param name="doTransform">Indicates whether the skewing should be applied directly.</param>
        public void Skew(PointF skewFactor, bool doTransform = false)
        {
            this.ForEachPrimitive(q => q.Skew(skewFactor));
            if (doTransform)
            {
                this.ApplyTransformation();
            }
        }

        /// <summary>
        /// Translates the sprite.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="doTransform">Indicates whether the translation should be applied directly.</param>
        public void Translate(Point offset, bool doTransform = false)
        {
            var off = new PointF(
                ((float)offset.X / this.ViewportSize.Width) * 2,
                ((float)offset.Y / this.ViewportSize.Height) * 2);

            this.ForEachPrimitive(q => q.Translate(off));
            if (doTransform)
            {
                this.ApplyTransformation();
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}