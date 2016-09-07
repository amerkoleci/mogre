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
namespace Miyagi.UI
{
    using System;
    using System.Linq;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.UI.Controls;
    using Miyagi.UI.Controls.Elements;

    /// <summary>
    /// A representation of a Sprite for UI elements.
    /// </summary>
    public class UISprite : Sprite
    {
        #region Fields

        private readonly Control owningControl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UISprite class.
        /// </summary>
        /// <param name="owner">The owner of the UISprite.</param>
        /// <param name="primitives">The primitives.</param>
        public UISprite(IElement owner, params Primitive[] primitives)
            : base(owner.SpriteRenderer, primitives)
        {
            if (primitives.Any(primitive => !(primitive is Quad)))
            {
                throw new ArgumentException("primitives must be quads", "primitives");
            }

            this.Owner = owner;
            this.owningControl = owner.OwningControl;
        }

        #endregion Constructors

        #region Enumerations

        /// <summary>
        /// Specifies the border quad.
        /// </summary>
        private enum BorderQuad
        {
            /// <summary>
            /// The buttom border quad.
            /// </summary>
            Bottom = 3,

            /// <summary>
            /// The buttom left border quad.
            /// </summary>
            BottomLeft = 6,

            /// <summary>
            /// The buttom right border quad.
            /// </summary>
            BottomRight = 7,

            /// <summary>
            /// The left border quad.
            /// </summary>
            Left = 0,

            /// <summary>
            /// The right border quad.
            /// </summary>
            Right = 1,

            /// <summary>
            /// The top border quad.
            /// </summary>
            Top = 2,

            /// <summary>
            /// The top left border quad.
            /// </summary>
            TopLeft = 4,

            /// <summary>
            /// The top right border quad.
            /// </summary>
            TopRight = 5
        }

        #endregion Enumerations

        #region Properties

        #region Internal Properties

        internal IElement Owner
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the quad.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The quad.</returns>
        public Quad GetQuad(int index)
        {
            return (Quad)this.GetPrimitive(index);
        }

        /// <summary>
        /// Performs a hit test.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <param name="pixelPerfect">if set to <c>true</c> performs a pixel-perfect hit test.</param>
        /// <returns>
        /// <c>true</c> if the primitive is hit; otherwise, <c>false</c>.
        /// </returns>
        public bool HitTest(Point p, bool pixelPerfect)
        {
            if (!pixelPerfect)
            {
                return this.HitTest(p);
            }

            var pf = p.ToScreenCoordinates(this.ViewportSize);
            return this.PixelPerfectHitTest(pf.X, pf.Y);
        }

        /// <summary>
        /// Removes the crop.
        /// </summary>
        public void RemoveCrop()
        {
            for (int i = 0; i < this.PrimitiveCount; i++)
            {
                this.GetQuad(i).RemoveCrop();
            }

            this.SpriteRenderer.BufferDirty = true;
        }

        /// <summary>
        /// Sets the quad bounds.
        /// </summary>
        /// <param name="quadIndex">Index of the quad.</param>
        /// <param name="bounds">The bounds.</param>
        public void SetQuadBounds(int quadIndex, Rectangle bounds)
        {
            this.GetQuad(quadIndex).SetBounds(bounds.ToScreenCoordinates(this.ViewportSize));
            this.SpriteRenderer.BufferDirty = true;
        }

        /// <summary>
        /// Sets the uv coordinates of a sprite.
        /// </summary>
        /// <param name="uvRect">A RectangleF representing the uv coordinates.</param>
        public void SetUV(RectangleF uvRect)
        {
            this.ForEachQuad(q => q.SetVertexUVs(uvRect));
            this.SpriteRenderer.BufferDirty = true;
        }

        /// <summary>
        /// Sets the uv coordinates of a sprite.
        /// </summary>
        /// <param name="quadIndex">Index of the quad.</param>
        /// <param name="uvRect">A RectangleF representing the uv coordinates.</param>
        public void SetUV(int quadIndex, RectangleF uvRect)
        {
            this.GetQuad(quadIndex).SetVertexUVs(uvRect);
            this.SpriteRenderer.BufferDirty = true;
        }

        /// <summary>
        /// Updates the sprite crop.
        /// </summary>
        public void UpdateCrop()
        {
            if (this.Owner == null || this.Owner.CroppingDisabled)
            {
                return;
            }

            var owner = this.owningControl;

            if (owner != null)
            {
                var parent = owner.Parent;
                if (parent == null)
                {
                    this.RemoveCrop();
                }
                else
                {
                    var parentRect = new Rectangle(
                        parent.DisplayRectangle.Location + parent.GetLocationInViewport(),
                        parent.DisplayRectangle.Size);

                    var grandParent = parent.Parent;
                    while (grandParent != null)
                    {
                        var grandParentRect = new Rectangle(
                            grandParent.DisplayRectangle.Location + grandParent.GetLocationInViewport(),
                            grandParent.DisplayRectangle.Size);

                        parentRect = Rectangle.Intersect(grandParentRect, parentRect);

                        grandParent = grandParent.Parent;
                    }

                    var parentRectSc = parentRect.ToScreenCoordinates(this.ViewportSize);

                    for (int i = 0; i < this.PrimitiveCount; i++)
                    {
                        Quad q = this.GetQuad(i);
                        q.SetCroppedValues(parentRectSc);
                    }
                }

                this.SpriteRenderer.BufferDirty = true;
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void MoveBottomEdge(PointF delta)
        {
            this.GetQuad(0).MoveBottomEdge(delta);
            this.SpriteRenderer.BufferDirty = true;
        }

        internal void MoveLeftEdge(PointF delta)
        {
            this.GetQuad(0).MoveLeftEdge(delta);
            this.SpriteRenderer.BufferDirty = true;
        }

        internal void MoveRightEdge(PointF delta)
        {
            this.GetQuad(0).MoveRightEdge(delta);
            this.SpriteRenderer.BufferDirty = true;
        }

        internal void MoveTopEdge(PointF delta)
        {
            this.GetQuad(0).MoveTopEdge(delta);
            this.SpriteRenderer.BufferDirty = true;
        }

        internal void OffsetBorderQuads(PointF offsetf)
        {
            if (offsetf.X != 0)
            {
                var right = new PointF(offsetf.X, 0);
                this.GetBorderQuad(BorderQuad.Top).MoveRightEdge(right);
                this.GetBorderQuad(BorderQuad.Bottom).MoveRightEdge(right);

                this.GetBorderQuad(BorderQuad.Right).Move(right);
                this.GetBorderQuad(BorderQuad.TopRight).Move(right);
                this.GetBorderQuad(BorderQuad.BottomRight).Move(right);
            }

            if (offsetf.Y != 0)
            {
                var bottom = new PointF(0, offsetf.Y);
                this.GetBorderQuad(BorderQuad.Left).MoveBottomEdge(bottom);
                this.GetBorderQuad(BorderQuad.Right).MoveBottomEdge(bottom);

                this.GetBorderQuad(BorderQuad.Bottom).Move(bottom);
                this.GetBorderQuad(BorderQuad.BottomLeft).Move(bottom);
                this.GetBorderQuad(BorderQuad.BottomRight).Move(bottom);
            }

            this.SpriteRenderer.BufferDirty = true;
        }

        internal void ResizeBorderQuadBottom(PointF bottom)
        {
            this.GetBorderQuad(BorderQuad.Bottom).MoveTopEdge(bottom); // bottom
            this.GetBorderQuad(BorderQuad.BottomLeft).MoveTopEdge(bottom); // bottom left
            this.GetBorderQuad(BorderQuad.BottomRight).MoveTopEdge(bottom); // bottom right
            this.GetBorderQuad(BorderQuad.Left).MoveBottomEdge(bottom); // left
            this.GetBorderQuad(BorderQuad.Right).MoveBottomEdge(bottom); // right

            this.SpriteRenderer.BufferDirty = true;
        }

        internal void ResizeBorderQuadLeft(PointF left)
        {
            this.GetBorderQuad(BorderQuad.Left).MoveRightEdge(left); // Left
            this.GetBorderQuad(BorderQuad.BottomLeft).MoveRightEdge(left); // bottom Left
            this.GetBorderQuad(BorderQuad.TopLeft).MoveRightEdge(left); // top Left
            this.GetBorderQuad(BorderQuad.Top).MoveLeftEdge(left); // top
            this.GetBorderQuad(BorderQuad.Bottom).MoveLeftEdge(left); // bottom

            this.SpriteRenderer.BufferDirty = true;
        }

        internal void ResizeBorderQuadRight(PointF right)
        {
            this.GetBorderQuad(BorderQuad.Right).MoveLeftEdge(right); // right
            this.GetBorderQuad(BorderQuad.TopRight).MoveLeftEdge(right); // top right
            this.GetBorderQuad(BorderQuad.BottomRight).MoveLeftEdge(right); // bottom right
            this.GetBorderQuad(BorderQuad.Top).MoveRightEdge(right); // top
            this.GetBorderQuad(BorderQuad.Bottom).MoveRightEdge(right); // bottom

            this.SpriteRenderer.BufferDirty = true;
        }

        internal void ResizeBorderQuadTop(PointF top)
        {
            this.GetBorderQuad(BorderQuad.Top).MoveBottomEdge(top); // top
            this.GetBorderQuad(BorderQuad.TopLeft).MoveBottomEdge(top); // top Left
            this.GetBorderQuad(BorderQuad.TopRight).MoveBottomEdge(top); // top right
            this.GetBorderQuad(BorderQuad.Left).MoveTopEdge(top); // Left
            this.GetBorderQuad(BorderQuad.Right).MoveTopEdge(top); // right

            this.SpriteRenderer.BufferDirty = true;
        }

        internal void SetBorderQuadUV(RectangleF textureUV, RectangleF borderUV)
        {
            this.GetBorderQuad(BorderQuad.Left).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(0, borderUV.Top, borderUV.Left, borderUV.Bottom)));
            this.GetBorderQuad(BorderQuad.Right).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(borderUV.Right, borderUV.Top, 1, borderUV.Bottom)));
            this.GetBorderQuad(BorderQuad.Top).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(borderUV.Left, 0, borderUV.Right, borderUV.Top)));
            this.GetBorderQuad(BorderQuad.Bottom).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(borderUV.Left, borderUV.Bottom, borderUV.Right, 1)));
            this.GetBorderQuad(BorderQuad.TopLeft).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(0, 0, borderUV.Left, borderUV.Top)));
            this.GetBorderQuad(BorderQuad.TopRight).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(borderUV.Right, 0, 1, borderUV.Top)));
            this.GetBorderQuad(BorderQuad.BottomLeft).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(0, borderUV.Bottom, borderUV.Left, 1)));
            this.GetBorderQuad(BorderQuad.BottomRight).SetVertexUVs(textureUV.GetUVOffset(RectangleF.FromLTRB(borderUV.Right, borderUV.Bottom, 1, 1)));

            this.SpriteRenderer.BufferDirty = true;
        }

        #endregion Internal Methods

        #region Private Methods

        private void ForEachQuad(Action<Quad> action)
        {
            int count = this.PrimitiveCount;
            for (int j = 0; j < count; j++)
            {
                action(this.GetQuad(j));
            }
        }

        private Quad GetBorderQuad(BorderQuad borderQuad)
        {
            return this.GetQuad((int)borderQuad);
        }

        private bool PixelPerfectHitTest(float x, float y)
        {
            var quad = (Quad)this.HitTest(x, y);

            if (quad != null)
            {
                x -= quad.Left;
                y -= quad.Top;
                float x1 = (x / 2) * this.ViewportSize.Width;
                float y1 = (-y / 2) * this.ViewportSize.Height;
                float width = quad.Width * this.ViewportSize.Width / 2;
                float height = quad.Height * this.ViewportSize.Height / 2;
                var size = new SizeF(width, height);
                var uvRect = RectangleF.FromLTRB(quad.RenderVertexUV[0].X, quad.RenderVertexUV[0].Y, quad.RenderVertexUV[2].X, quad.RenderVertexUV[2].Y);
                var texSize = this.Backend.GetTextureSize(this.TextureHandle);
                var left = (int)((((texSize.Width / size.Width) * uvRect.Width) * x1) + (uvRect.Left * texSize.Width));
                var top = (int)((((texSize.Height / size.Height) * uvRect.Height) * y1) + (uvRect.Top * texSize.Height));

                return this.Backend.GetTextureAlpha(this.TextureHandle, new Point(left, top)) > 0.01f;
            }

            return false;
        }

        #endregion Private Methods

        #endregion Methods
    }
}