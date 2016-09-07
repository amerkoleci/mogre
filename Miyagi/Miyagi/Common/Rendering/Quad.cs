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
namespace Miyagi.Common.Rendering
{
    using System;

    using Miyagi.Common.Data;

    /// <summary>
    /// A primitive representing a quad.
    /// </summary>
    public sealed class Quad : Primitive
    {
        #region Fields

        private readonly PointF[] croppedVertexPos;
        private readonly PointF[] croppedVertexUV;

        private bool isCropped;
        private bool skipRender;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> class.
        /// </summary>
        public Quad()
        {
            this.TransformedVertexPositions = new PointF[4];
            this.croppedVertexPos = new PointF[4];
            this.croppedVertexUV = new PointF[4];
            this.SetVertexColours(Colours.White);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public Quad(RectangleF bounds)
            : this()
        {
            this.SetBounds(bounds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="uv">The uv.</param>
        public Quad(RectangleF bounds, RectangleF uv)
            : this(bounds)
        {
            this.SetVertexUVs(uv);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> class.
        /// </summary>
        /// <param name="colourDefinition">The colour definition.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="uv">The uv.</param>
        public Quad(ColourDefinition colourDefinition, RectangleF bounds, RectangleF uv)
            : this(bounds, uv)
        {
            this.SetVertexColours(colourDefinition);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the bottom of the Quad.
        /// </summary>
        public float Bottom
        {
            get
            {
                return this.GeometryVertexPos[2].Y;
            }
        }

        /// <summary>
        /// Gets the center of the Quad.
        /// </summary>
        public PointF Center
        {
            get
            {
                float x = (this.GeometryVertexPos[0].X + this.GeometryVertexPos[2].X) / 2;
                float y = (this.GeometryVertexPos[0].Y + this.GeometryVertexPos[2].Y) / 2;
                return new PointF(x, y);
            }
        }

        /// <summary>
        /// Gets a geometric copy of the quad.
        /// </summary>
        public override Primitive GeometricCopy
        {
            get
            {
                var retValue = new Quad();
                retValue.SetVertexLocations(this.VertexPositions);
                return retValue;
            }
        }

        /// <summary>
        /// Gets the height of the Quad.
        /// </summary>
        /// <value>A float representing the height of the Quad.</value>
        public float Height
        {
            get
            {
                return this.Top - this.Bottom;
            }
        }

        /// <summary>
        /// Gets the x coordinate of the left edge of the Quad.
        /// </summary>
        public float Left
        {
            get
            {
                return this.GeometryVertexPos[0].X;
            }
        }

        /// <summary>
        /// Gets the x coordinate of the right edge of the Quad.
        /// </summary>
        public float Right
        {
            get
            {
                return this.GeometryVertexPos[2].X;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether rendering of this Quad should be skipped.
        /// </summary>
        public override bool SkipRender
        {
            get
            {
                return this.skipRender && this.Width != 0 && this.Height != 0;
            }

            set
            {
                this.skipRender = value;
            }
        }

        /// <summary>
        /// Gets the top of the Quad.
        /// </summary>
        public float Top
        {
            get
            {
                return this.GeometryVertexPos[0].Y;
            }
        }

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        /// <value>The triangle count.</value>
        public override int TriangleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Quad;
            }
        }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        public override int VertexCount
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// Gets the width of the Quad.
        /// </summary>
        public float Width
        {
            get
            {
                return this.Right - this.Left;
            }
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the positions of the verticies.
        /// </summary>
        protected internal override PointF[] RenderVertexPositions
        {
            get
            {
                return this.isCropped ? this.croppedVertexPos : base.RenderVertexPositions;
            }
        }

        /// <summary>
        /// Gets the uv-coordinates of the verticies.
        /// </summary>
        protected internal override PointF[] RenderVertexUV
        {
            get
            {
                return this.isCropped ? this.croppedVertexUV : base.RenderVertexUV;
            }
        }

        #endregion Protected Internal Properties

        #region Private Properties

        /// <summary>
        /// Gets the untransformed verticies positions.
        /// </summary>
        private PointF[] GeometryVertexPos
        {
            get
            {
                return this.isCropped ? this.croppedVertexPos : this.VertexPositions;
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Creates a dummy quad.
        /// </summary>
        /// <param name="posRect">A RectangleF representing the location.</param>
        /// <returns>The newly created quad.</returns>
        public static Quad CreateDummy(RectangleF posRect)
        {
            return new Quad(posRect, new RectangleF(0, 0, 1, 1))
                   {
                       SkipRender = true
                   };
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Performs a hit test.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns><c>true</c> if the primitive is hit; otherwise, <c>false</c>.</returns>
        public override bool HitTest(float x, float y)
        {
            if (this.Width == 0 || this.Height == 0)
            {
                return false;
            }

            return base.HitTest(x, y);
        }

        /// <summary>
        /// Moves the Quad.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public override void Move(PointF delta)
        {
            this.Move(delta, 0, 1, 2, 3);
        }

        /// <summary>
        /// Moves the bottom edge of the Quad.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public void MoveBottomEdge(PointF delta)
        {
            this.Move(delta, 1, 2);
        }

        /// <summary>
        /// Moves the left edge of the Quad.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public void MoveLeftEdge(PointF delta)
        {
            this.Move(delta, 0, 1);
        }

        /// <summary>
        /// Moves the right edge of the Quad.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public void MoveRightEdge(PointF delta)
        {
            this.Move(delta, 2, 3);
        }

        /// <summary>
        /// Moves the top edge of the Quad.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public void MoveTopEdge(PointF delta)
        {
            this.Move(delta, 0, 3);
        }

        /// <summary>
        /// Removes the crop.
        /// </summary>
        public void RemoveCrop()
        {
            this.isCropped = false;
        }

        /// <summary>
        /// Resizes the Quad.
        /// </summary>
        /// <param name="delta">The distance to resize.</param>
        public override void Resize(PointF delta)
        {
            this.MoveBottomEdge(new PointF(0, delta.Y));
            this.MoveRightEdge(new PointF(delta.X, 0));
        }

        /// <summary>
        /// Sets the position of the Quad.
        /// </summary>
        /// <param name="rect">A rect representing the position of the Quad in pixel.</param>
        public void SetBounds(RectangleF rect)
        {
            this.SetVertexLocations(rect.GetPoints());
        }

        /// <summary>
        /// Crops the Quad.
        /// </summary>
        /// <param name="cropArea">A RectangleF representing the area the Quad should be cropped to.</param>
        public void SetCroppedValues(RectangleF cropArea)
        {
            RectangleF oldRect = RectangleF.FromLTRB(this.VertexPositions[0].X, this.VertexPositions[0].Y, this.VertexPositions[2].X, this.VertexPositions[2].Y);
            RectangleF oldUVRect = RectangleF.FromLTRB(this.VertexUV[0].X, this.VertexUV[0].Y, this.VertexUV[2].X, this.VertexUV[2].Y);

            float oldLeft = oldRect.Left, oldTop = oldRect.Top, oldRight = oldRect.Right, oldBottom = oldRect.Bottom;
            float posLeft = 0, posTop = 0, posRight = 0, posBottom = 0;

            if (oldLeft < cropArea.Right
                && oldRight > cropArea.Left
                && oldTop > cropArea.Bottom
                && oldBottom < cropArea.Top)
            {
                posLeft = Math.Max(oldLeft, cropArea.Left);
                posTop = -Math.Max(-oldTop, -cropArea.Top);
                posRight = Math.Min(oldRight, cropArea.Right);
                posBottom = -Math.Min(-oldBottom, -cropArea.Bottom);
            }

            if (oldLeft == posLeft && oldRight == posRight && oldTop == posTop && oldBottom == posBottom)
            {
                this.isCropped = false;
            }
            else
            {
                this.croppedVertexPos[0] = new PointF(posLeft, posTop);
                this.croppedVertexPos[1] = new PointF(posLeft, posBottom);
                this.croppedVertexPos[2] = new PointF(posRight, posBottom);
                this.croppedVertexPos[3] = new PointF(posRight, posTop);

                // UV
                float uvLeft = ((posLeft - oldLeft) * oldUVRect.Width / oldRect.Width) + oldUVRect.Left;
                float uvRight = oldUVRect.Right - ((oldRect.Right - posRight) * oldUVRect.Width / oldRect.Width);
                float uvTop = oldUVRect.Top - ((oldTop - posTop) * oldUVRect.Height / oldRect.Height);
                float uvBottom = ((posBottom - oldBottom) * oldUVRect.Height / oldRect.Height) + oldUVRect.Bottom;
                this.croppedVertexUV[0] = new PointF(uvLeft, uvTop);
                this.croppedVertexUV[1] = new PointF(uvLeft, uvBottom);
                this.croppedVertexUV[2] = new PointF(uvRight, uvBottom);
                this.croppedVertexUV[3] = new PointF(uvRight, uvTop);

                this.isCropped = true;
            }
        }

        /// <summary>
        /// Sets the uv-coordinates of the Quad.
        /// </summary>
        /// <param name="rect">A FloatRect representing the uv-coordinates.</param>
        public void SetVertexUVs(RectangleF rect)
        {
            this.SetVertexUVs(rect.GetPoints());
        }

        #endregion Public Methods

        #region Private Methods

        private void Move(PointF delta, params int[] vertex)
        {
            foreach (int index in vertex)
            {
                this.VertexPositions[index] = PointF.Add(this.VertexPositions[index], delta.X, delta.Y);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}