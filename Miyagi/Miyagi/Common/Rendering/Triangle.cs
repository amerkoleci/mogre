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
    /// A primitive representing a triangle.
    /// </summary>
    public sealed class Triangle : Primitive
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle"/> class.
        /// </summary>
        public Triangle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle"/> class.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        public Triangle(PointF a, PointF b, PointF c)
            : this()
        {
            this.SetVertexLocations(a, b, c);
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a geometric copy of the primitive.
        /// </summary>
        public override Primitive GeometricCopy
        {
            get
            {
                return new Triangle(this.VertexPositions[0], this.VertexPositions[1], this.VertexPositions[2]);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether rendering of this primitive should be skipped.
        /// </summary>
        public override bool SkipRender
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        public override int TriangleCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public override PrimitiveType Type
        {
            get
            {
                return PrimitiveType.Triangle;
            }
        }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        public override int VertexCount
        {
            get
            {
                return 3;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Moves the primitive.
        /// </summary>
        /// <param name="delta">The distance to move.</param>
        public override void Move(PointF delta)
        {
            for (int i = 0; i < 3; i++)
            {
                this.VertexPositions[i] += delta;
            }
        }

        /// <summary>
        /// Resizes the primitive.
        /// </summary>
        /// <param name="delta">The distance to resize.</param>
        public override void Resize(PointF delta)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #endregion Methods
    }
}