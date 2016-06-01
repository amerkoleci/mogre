// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        public float m00;
        public float m01;
        public float m02;

        public float m10;
        public float m11;
        public float m12;

        public float m20;
        public float m21;
        public float m22;


        /// <summary>
        /// A <see cref="Matrix3"/> with all of its components set to zero.
        /// </summary>
        public static readonly Matrix3 Zero = new Matrix3();

        /// <summary>
        /// The identity <see cref="Matrix3"/>.
        /// </summary>
        public static readonly Matrix3 Identity = new Matrix3(1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
		/// Gets or sets the scale of the Matrix3; that is M11, M22, and M33.
		/// </summary>
		public Vector3 Scale
        {
            get { return new Vector3(m00, m11, m22); }
            set { m00 = value.X; m11 = value.Y; m22 = value.Z; }
        }

        /// <summary>
		/// Gets a value indicating whether this instance is an identity Matrix3.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is an identity Matrix3; otherwise, <c>false</c>.
		/// </value>
		public bool IsIdentity
        {
            get
            {
                return Equals(Identity);
            }
        }

        /// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the Matrix3 component, depending on the index.</value>
		/// <param name="index">The zero-based index of the component to access.</param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 15].</exception>
		public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m02;
                    case 3: return m10;
                    case 4: return m11;
                    case 5: return m12;
                    case 6: return m20;
                    case 7: return m21;
                    case 8: return m22;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Matrix3 run from 0 to 8, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m10 = value; break;
                    case 4: m11 = value; break;
                    case 5: m12 = value; break;
                    case 6: m20 = value; break;
                    case 7: m21 = value; break;
                    case 8: m22 = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Matrix3 run from 0 to 8, inclusive.");
                }
            }
        }

        /// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the Matrix4x4 component, depending on the index.</value>
		/// <param name="row">The row of the Matrix4x4 to access.</param>
		/// <param name="column">The column of the Matrix4x4 to access.</param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="row"/> or <paramref name="column"/>is out of the range [0, 3].</exception>
		public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row > 2)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
                if (column < 0 || column > 2)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 2, inclusive.");

                return this[(row * 3) + column];
            }

            set
            {
                if (row < 0 || row > 2)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 2, inclusive.");
                if (column < 0 || column > 2)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 2, inclusive.");

                this[(row * 3) + column] = value;
            }
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Matrix3"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Matrix3(float value)
        {
            m00 = m01 = m02 =
            m10 = m11 = m12 =
            m20 = m21 = m22 = value;
        }

        public Matrix3(Matrix3 other)
        {
            m00 = other.m00;
            m01 = other.m01;
            m02 = other.m02;
            m10 = other.m10;
            m11 = other.m11;
            m12 = other.m12;
            m20 = other.m20;
            m21 = other.m21;
            m22 = other.m22;
        }

        public Matrix3(float fEntry00, float fEntry01, float fEntry02, float fEntry10, float fEntry11, float fEntry12, float fEntry20, float fEntry21, float fEntry22)
        {
            this.m00 = fEntry00;
            this.m01 = fEntry01;
            this.m02 = fEntry02;
            this.m10 = fEntry10;
            this.m11 = fEntry11;
            this.m12 = fEntry12;
            this.m20 = fEntry20;
            this.m21 = fEntry21;
            this.m22 = fEntry22;
        }

        /// <summary>
		/// Returns a boolean indicating whether the given Matrix3 is equal to this Matrix3 instance.
		/// </summary>
		/// <param name="other">The Matrix3 to compare this instance to.</param>
		/// <returns>True if the other Matrix3 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Matrix3 other)
        {
            return
                other.m00 == m00 &&
                other.m01 == m01 &&
                other.m02 == m02 &&

                other.m10 == m10 &&
                other.m11 == m11 &&
                other.m12 == m12 &&

                other.m20 == m20 &&
                other.m21 == m21 &&
                other.m22 == m22;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Matrix3 is equal to this Matrix3 instance.
        /// </summary>
        /// <param name="other">The Matrix3 to compare this instance to.</param>
        /// <returns>True if the other Matrix3 is equal to this instance; False otherwise.</returns>
        public bool Equals(Matrix3 other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Matrix3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Matrix3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3))
                return false;

            return Equals((Matrix3)obj);
        }

        /// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Matrix3 left, Matrix3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3 left, Matrix3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
        {
            return
                m00.GetHashCode() + m01.GetHashCode() + m02.GetHashCode() +
                m10.GetHashCode() + m11.GetHashCode() + m12.GetHashCode() +
                m20.GetHashCode() + m21.GetHashCode() + m22.GetHashCode();
        }

        /// <summary>
		/// Adds two matricies.
		/// </summary>
		/// <param name="left">The first Matrix3 to add.</param>
		/// <param name="right">The second Matrix3 to add.</param>
		/// <returns>The sum of the two matricies.</returns>
		public static Matrix3 operator +(Matrix3 left, Matrix3 right)
        {
            Matrix3 result = new Matrix3();
            result.m00 = left.m00 + right.m00;
            result.m01 = left.m01 + right.m01;
            result.m02 = left.m02 + right.m02;
            result.m10 = left.m10 + right.m10;
            result.m11 = left.m11 + right.m11;
            result.m12 = left.m12 + right.m12;
            result.m20 = left.m20 + right.m20;
            result.m21 = left.m21 + right.m21;
            result.m22 = left.m22 + right.m22;
            return result;
        }

        /// <summary>
		/// Subtracts two matricies.
		/// </summary>
		/// <param name="left">The first Matrix3 to subtract.</param>
		/// <param name="right">The second Matrix3 to subtract.</param>
		/// <returns>The difference between the two matricies.</returns>
		public static Matrix3 operator -(Matrix3 left, Matrix3 right)
        {
            Matrix3 result = new Matrix3();
            result.m00 = left.m00 - right.m00;
            result.m01 = left.m01 - right.m01;
            result.m02 = left.m02 - right.m02;
            result.m10 = left.m10 - right.m10;
            result.m11 = left.m11 - right.m11;
            result.m12 = left.m12 - right.m12;
            result.m20 = left.m20 - right.m20;
            result.m21 = left.m21 - right.m21;
            result.m22 = left.m22 - right.m22;
            return result;
        }

        /// <summary>
		/// Multiplies two matricies.
		/// </summary>
		/// <param name="left">The first Matrix3 to multiply.</param>
		/// <param name="right">The second Matrix3 to multiply.</param>
		/// <returns>The product of the two matricies.</returns>
		public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            Matrix3 result = new Matrix3();
            result.m00 = left.m00 * right.m00 + left.m01 * right.m10 + left.m02 * right.m20;
            result.m01 = left.m00 * right.m01 + left.m01 * right.m11 + left.m02 * right.m21;
            result.m02 = left.m00 * right.m02 + left.m01 * right.m12 + left.m02 * right.m22;

            result.m10 = left.m10 * right.m00 + left.m11 * right.m10 + left.m12 * right.m20;
            result.m11 = left.m10 * right.m01 + left.m11 * right.m11 + left.m12 * right.m21;
            result.m12 = left.m10 * right.m02 + left.m11 * right.m12 + left.m12 * right.m22;

            result.m20 = left.m20 * right.m00 + left.m21 * right.m10 + left.m22 * right.m20;
            result.m21 = left.m20 * right.m01 + left.m21 * right.m11 + left.m22 * right.m21;
            result.m22 = left.m20 * right.m02 + left.m21 * right.m12 + left.m22 * right.m22;
            return result;
        }

        /// <summary>
		/// Divides two matricies.
		/// </summary>
		/// <param name="left">The first Matrix3 to divide.</param>
		/// <param name="right">The second Matrix4x4 to divide.</param>
		/// <returns>The quotient of the two matricies.</returns>
		public static Matrix3 operator /(Matrix3 left, Matrix3 right)
        {
            Matrix3 result = new Matrix3();
            result.m00 = left.m00 / right.m00;
            result.m01 = left.m01 / right.m01;
            result.m02 = left.m02 / right.m02;
            result.m10 = left.m10 / right.m10;
            result.m11 = left.m11 / right.m11;
            result.m12 = left.m12 / right.m12;
            result.m20 = left.m20 / right.m20;
            result.m21 = left.m21 / right.m21;
            result.m22 = left.m22 / right.m22;
            return result;
        }


        public Vector3 GetColumn(uint iCol)
        {
            unsafe
            {
                fixed (float* ptr = &m00)
                {
                    return new Vector3(*(ptr + iCol), *(ptr + 3 + iCol), *(ptr + 6 + iCol));
                }
            }
        }

        public void SetColumn(uint iCol, Vector3 vec)
        {
            unsafe
            {
                fixed (float* ptr = &m00)
                {
                    *(ptr + iCol) = vec.x;
                    *(ptr + 3 + iCol) = vec.y;
                    *(ptr + 6 + iCol) = vec.z;
                }
            }
        }

        public void FromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
        {
            SetColumn(0, xAxis);
            SetColumn(1, yAxis);
            SetColumn(2, zAxis);
        }
    }
}
