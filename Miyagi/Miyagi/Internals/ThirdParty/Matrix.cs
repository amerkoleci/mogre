// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Authors:
//      Chris Toshok (toshok@ximian.com)
namespace Miyagi.Internals.ThirdParty
{
    using System;

    using Miyagi.Common.Data;

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    internal struct Matrix : IFormattable
    {
        #region Fields

        private double m11;
        private double m12;
        private double m21;
        private double m22;
        private double offsetX;
        private double offsetY;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> struct.
        /// </summary>
        /// <param name="m11">The M11.</param>
        /// <param name="m12">The M12.</param>
        /// <param name="m21">The M21.</param>
        /// <param name="m22">The M22.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public Matrix(double m11,
            double m12,
            double m21,
            double m22,
            double offsetX,
            double offsetY)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        #endregion Constructors

        #region Properties

        #region Public Static Properties

        public static Matrix Identity
        {
            get
            {
                return new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
            }
        }

        #endregion Public Static Properties

        #region Public Properties

        public double Determinant
        {
            get
            {
                return m11 * m22 - m12 * m21;
            }
        }

        public bool HasInverse
        {
            get
            {
                return Determinant != 0;
            }
        }

        public bool IsIdentity
        {
            get
            {
                return Equals(Matrix.Identity);
            }
        }

        public double M11
        {
            get
            {
                return m11;
            }

            set
            {
                m11 = value;
            }
        }

        public double M12
        {
            get
            {
                return m12;
            }

            set
            {
                m12 = value;
            }
        }

        public double M21
        {
            get
            {
                return m21;
            }

            set
            {
                m21 = value;
            }
        }

        public double M22
        {
            get
            {
                return m22;
            }

            set
            {
                m22 = value;
            }
        }

        public double OffsetX
        {
            get
            {
                return offsetX;
            }

            set
            {
                offsetX = value;
            }
        }

        public double OffsetY
        {
            get
            {
                return offsetY;
            }

            set
            {
                offsetY = value;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        public static bool operator !=(Matrix matrix1,
            Matrix matrix2)
        {
            return !matrix1.Equals(matrix2);
        }

        public static Matrix operator *(Matrix trans1,
            Matrix trans2)
        {
            Matrix result = trans1;
            result.Append(trans2);
            return result;
        }

        public static bool operator ==(Matrix matrix1,
            Matrix matrix2)
        {
            return matrix1.Equals(matrix2);
        }

        public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top)
        {
            Matrix matrix = Matrix.Identity;
            matrix.M11 = 2f / (right - left);
            matrix.M12 = 0f;
            matrix.M22 = 2f / (top - bottom);
            matrix.M21 = 0f;
            return matrix;
        }

        public static bool Equals(Matrix matrix1,
            Matrix matrix2)
        {
            return matrix1.Equals(matrix2);
        }

        public static Matrix Multiply(Matrix trans1,
            Matrix trans2)
        {
            Matrix m = trans1;
            m.Append(trans2);
            return m;
        }

        #endregion Public Static Methods

        #region Explicit Interface Methods

        string IFormattable.ToString(string format,
            IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        public void Append(Matrix matrix)
        {
            double m11 = this.m11 * matrix.M11 + this.m12 * matrix.M21;
            double m12 = this.m11 * matrix.M12 + this.m12 * matrix.M22;
            double m21 = this.m21 * matrix.M11 + this.m22 * matrix.M21;
            double m22 = this.m21 * matrix.M12 + this.m22 * matrix.M22;

            double offsetX = this.offsetX * matrix.M11 + this.offsetY * matrix.M21 + matrix.OffsetX;
            double offsetY = this.offsetX * matrix.M12 + this.offsetY * matrix.M22 + matrix.OffsetY;

            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public bool Equals(Matrix value)
        {
            return (m11 == value.M11 &&
                    m12 == value.M12 &&
                    m21 == value.M21 &&
                    m22 == value.M22 &&
                    offsetX == value.OffsetX &&
                    offsetY == value.OffsetY);
        }

        public override bool Equals(object o)
        {
            if (!(o is Matrix))
            {
                return false;
            }

            return Equals((Matrix)o);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public void Invert()
        {
            if (!HasInverse)
            {
                throw new InvalidOperationException("Transform is not invertible.");
            }

            double d = Determinant;

            /* 1/(ad-bc)[d -b; -c a] */

            double m11 = this.m22;
            double m12 = -this.m12;
            double m21 = -this.m21;
            double m22 = this.m11;

            double offsetX = m21 * this.offsetY - m22 * this.offsetX;
            double offsetY = m12 * this.offsetX - m11 * this.offsetY;

            this.m11 = m11 / d;
            this.m12 = m12 / d;
            this.m21 = m21 / d;
            this.m22 = m22 / d;
            this.offsetX = offsetX / d;
            this.offsetY = offsetY / d;
        }

        public void Prepend(Matrix matrix)
        {
            double m11 = matrix.M11 * this.m11 + matrix.M12 * this.m21;
            double m12 = matrix.M11 * this.m12 + matrix.M12 * this.m22;
            double m21 = matrix.M21 * this.m11 + matrix.M22 * this.m21;
            double m22 = matrix.M21 * this.m12 + matrix.M22 * this.m22;

            double offsetX = matrix.OffsetX * this.m11 + matrix.OffsetY * this.m21 + this.offsetX;
            double offsetY = matrix.OffsetX * this.m12 + matrix.OffsetY * this.m22 + this.offsetY;

            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public void Rotate(double angle)
        {
            // R_theta==[costheta -sintheta; sintheta costheta],
            double theta = angle * Math.PI / 180;

            var rTheta = new Matrix(Math.Cos(theta), Math.Sin(theta),
                                    -Math.Sin(theta), Math.Cos(theta),
                                    0, 0);

            Append(rTheta);
        }

        public void RotateAt(double angle,
            double centerX,
            double centerY)
        {
            Translate(-centerX, -centerY);
            Rotate(angle);
            Translate(centerX, centerY);
        }

        public void RotateAtPrepend(double angle,
            double centerX,
            double centerY)
        {
            Matrix m = Matrix.Identity;
            m.RotateAt(angle, centerX, centerY);
            Prepend(m);
        }

        public void RotatePrepend(double angle)
        {
            Matrix m = Matrix.Identity;
            m.Rotate(angle);
            Prepend(m);
        }

        public void Scale(double scaleX,
            double scaleY)
        {
            var scale = new Matrix(scaleX, 0,
                                   0, scaleY,
                                   0, 0);

            Append(scale);
        }

        public void ScaleAt(double scaleX,
            double scaleY,
            double centerX,
            double centerY)
        {
            Translate(-centerX, -centerY);
            Scale(scaleX, scaleY);
            Translate(centerX, centerY);
        }

        public void ScaleAtPrepend(double scaleX,
            double scaleY,
            double centerX,
            double centerY)
        {
            Matrix m = Matrix.Identity;
            m.ScaleAt(scaleX, scaleY, centerX, centerY);
            Prepend(m);
        }

        public void ScalePrepend(double scaleX,
            double scaleY)
        {
            Matrix m = Matrix.Identity;
            m.Scale(scaleX, scaleY);
            Prepend(m);
        }

        public void SetIdentity()
        {
            m11 = m22 = 1.0;
            m12 = m21 = 0.0;
            offsetX = offsetY = 0.0;
        }

        public void Skew(double skewX,
            double skewY)
        {
            var skewM = new Matrix(1, Math.Tan(skewY * Math.PI / 180),
                                   Math.Tan(skewX * Math.PI / 180), 1,
                                   0, 0);
            Append(skewM);
        }

        public void SkewPrepend(double skewX,
            double skewY)
        {
            Matrix m = Matrix.Identity;
            m.Skew(skewX, skewY);
            Prepend(m);
        }

        public override string ToString()
        {
            if (IsIdentity)
            {
                return "Identity";
            }
            else
            {
                return string.Format("{0},{1},{2},{3},{4},{5}",
                                     m11, m12, m21, m22, offsetX, offsetY);
            }
        }

        public PointF Transform(PointF point)
        {
            return Matrix.Multiply(point, this);
        }

        public void Transform(PointF[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Transform(points[i]);
            }
        }

        public void Translate(double offsetX,
            double offsetY)
        {
            this.offsetX += offsetX;
            this.offsetY += offsetY;
        }

        public void TranslatePrepend(double offsetX,
            double offsetY)
        {
            Matrix m = Matrix.Identity;
            m.Translate(offsetX, offsetY);
            Prepend(m);
        }

        #endregion Public Methods

        #region Internal Static Methods

        internal static PointF Multiply(PointF point, Matrix matrix)
        {
            return new PointF(
                (float)(point.X * matrix.M11 + point.Y * matrix.M21 + matrix.OffsetX),
                (float)(point.X * matrix.M12 + point.Y * matrix.M22 + matrix.OffsetY));
        }

        #endregion Internal Static Methods

        #endregion Methods
    }
}