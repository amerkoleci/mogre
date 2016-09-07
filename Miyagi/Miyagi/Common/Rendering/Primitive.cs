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
    using System.Diagnostics;

    using Miyagi.Common.Data;
    using Miyagi.Internals.ThirdParty;

    /// <summary>
    /// Represents a render primitive.
    /// </summary>
    public abstract class Primitive
    {
        #region Fields

        private float rotationAngle;
        private PointF rotationPivot;
        private PointF scaleFactor;
        private PointF scalePivot;
        private PointF skewFactor;
        private PointF translationOffset;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Primitive"/> class.
        /// </summary>
        protected Primitive()
        {
            this.scaleFactor = new PointF(1, 1);
            int count = this.VertexCount;
            this.VertexColours = new Colour[count];
            this.VertexUV = new PointF[count];
            this.VertexPositions = new PointF[count];
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a geometric copy of the primitive.
        /// </summary>
        public abstract Primitive GeometricCopy
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether rendering of this primitive should be skipped.
        /// </summary>
        public abstract bool SkipRender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        /// <value>The triangle count.</value>
        public abstract int TriangleCount
        {
            get;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public abstract PrimitiveType Type
        {
            get;
        }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        /// <value>The vertex count.</value>
        public abstract int VertexCount
        {
            get;
        }

        #endregion Public Properties

        #region Protected Internal Properties

        /// <summary>
        /// Gets the render vertex colours.
        /// </summary>
        protected internal virtual Colour[] RenderVertexColours
        {
            get
            {
                return this.VertexColours;
            }
        }

        /// <summary>
        /// Gets the render vertex positions.
        /// </summary>
        protected internal virtual PointF[] RenderVertexPositions
        {
            get
            {
                return this.IsTransformed ? this.TransformedVertexPositions : this.VertexPositions;
            }
        }

        /// <summary>
        /// Gets the render vertex UVs.
        /// </summary>
        /// <value>The render vertex UVs.</value>
        protected internal virtual PointF[] RenderVertexUV
        {
            get
            {
                return this.VertexUV;
            }
        }

        /// <summary>
        /// Gets the vertex colours.
        /// </summary>
        protected internal Colour[] VertexColours
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the vertex positions.
        /// </summary>
        protected internal PointF[] VertexPositions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the vertex UV.
        /// </summary>
        protected internal PointF[] VertexUV
        {
            get;
            private set;
        }

        #endregion Protected Internal Properties

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating whether this instance is transformed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is transformed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsTransformed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the transformed vertex positions.
        /// </summary>
        protected PointF[] TransformedVertexPositions
        {
            get;
            set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Applies the transformation.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio.</param>
        public void ApplyTransformation(float aspectRatio)
        {
            var m = Matrix.Identity;

            float invAspectRatio = 1.0f / aspectRatio;
            var aspectRatioCorrection = new Matrix(1, 0, 0, invAspectRatio, 0, 0);
            var undoAspectRatioCorrection = new Matrix(1, 0, 0, aspectRatio, 0, 0);

            this.IsTransformed = false;

            this.VertexPositions.CopyTo(this.TransformedVertexPositions, 0);

            m.Translate(this.translationOffset.X, this.translationOffset.Y);
            m.ScaleAt(this.scaleFactor.X, this.scaleFactor.Y, this.scalePivot.X, this.scalePivot.Y);
            m.Append(aspectRatioCorrection);
            m.RotateAt(this.rotationAngle, this.rotationPivot.X, this.rotationPivot.Y * invAspectRatio);
            m.Append(undoAspectRatioCorrection);
            m.Skew(this.skewFactor.X, this.skewFactor.Y);
            m.Transform(this.TransformedVertexPositions);

            this.IsTransformed = true;
        }

        /// <summary>
        /// Gets a vertex of the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="Vertex"/> of the specified index.</returns>
        public Vertex GetVertex(int index)
        {
            Debug.Assert(index < this.VertexCount);

            return new Vertex
                   {
                       Colour = this.RenderVertexColours[index],
                       Location = this.RenderVertexPositions[index],
                       UV = this.RenderVertexUV[index]
                   };
        }

        /// <summary>
        /// Performs a hit test.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns><c>true</c> if the primitive is hit; otherwise, <c>false</c>.</returns>
        public virtual bool HitTest(float x, float y)
        {
            return new PointF(x, y).IsInsidePolygon(this.RenderVertexPositions);
        }

        /// <summary>
        /// Moves the primitive.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public abstract void Move(PointF delta);

        /// <summary>
        /// Removes the transformation.
        /// </summary>
        public void RemoveTransformation()
        {
            this.IsTransformed = false;
        }

        /// <summary>
        /// Resizes the primitive.
        /// </summary>
        /// <param name="delta">The distance to resize.</param>
        public abstract void Resize(PointF delta);

        /// <summary>
        /// Rotates the Quad.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="pivot">The pivot point.</param>
        public void Rotate(float angle, PointF pivot)
        {
            this.rotationAngle = angle;
            this.rotationPivot = pivot;
        }

        /// <summary>
        /// Scales the Quad.
        /// </summary>
        /// <param name="scaleFactor">The scale factor..</param>
        /// <param name="pivot">The pivot point.</param>
        public void Scale(PointF scaleFactor, PointF pivot)
        {
            this.scaleFactor = scaleFactor;
            this.scalePivot = pivot;
        }

        /// <summary>
        /// Sets the vertex colours.
        /// </summary>
        /// <param name="colour">The colour.</param>
        public void SetVertexColours(ColourDefinition colour)
        {
            for (int i = 0; i < this.VertexCount; i++)
            {
                if (colour.Colours.Count == this.VertexCount)
                {
                    this.VertexColours[i] = colour.Colours[i];
                }
                else
                {
                    this.VertexColours[i] = colour.Colours[0];
                }
            }
        }

        /// <summary>
        /// Sets the vertex locations.
        /// </summary>
        /// <param name="points">The points.</param>
        public void SetVertexLocations(params PointF[] points)
        {
            Debug.Assert(points.Length == this.VertexCount);
            this.VertexPositions = points;
        }

        /// <summary>
        /// Sets the vertex UVs.
        /// </summary>
        /// <param name="points">The points.</param>
        public void SetVertexUVs(params PointF[] points)
        {
            Debug.Assert(points.Length == this.VertexCount);
            this.VertexUV = points;
        }

        /// <summary>
        /// Skews the primitive.
        /// </summary>
        /// <param name="skewFactor">The skew factor.</param>
        public void Skew(PointF skewFactor)
        {
            this.skewFactor = skewFactor;
        }

        /// <summary>
        /// Translates the primitive.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void Translate(PointF offset)
        {
            this.translationOffset = offset;
        }

        #endregion Public Methods

        #endregion Methods
    }
}