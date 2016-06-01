// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4 : IEquatable<Matrix4>
    {
        /// <summary>
		/// The size of the <see cref="Matrix4"/> type, in bytes.
		/// </summary>
		public const int SizeInBytes = 64; // 4 * 16

        public float m00;
        public float m01;
        public float m02;
        public float m03;

        public float m10;
        public float m11;
        public float m12;
        public float m13;

        public float m20;
        public float m21;
        public float m22;
        public float m23;

        public float m30;
        public float m31;
        public float m32;
        public float m33;

        /// <summary>
        /// A <see cref="Matrix4"/> with all of its components set to zero.
        /// </summary>
        public static readonly Matrix4 Zero = new Matrix4();

        /// <summary>
        /// The identity <see cref="Matrix4"/>.
        /// </summary>
        public static readonly Matrix4 Identity = new Matrix4 { m00 = 1.0f, m11 = 1.0f, m22 = 1.0f, m33 = 1.0f };


        /// <summary>
		/// Gets or sets the translation of the Matrix4x4; that is M41, M42, and M43.
		/// </summary>
		public Vector3 Translation
        {
            get { return new Vector3(m03, m13, m23); }
            set { m03 = value.X; m13 = value.Y; m23 = value.Z; }
        }

        /// <summary>
		/// Gets or sets the scale of the Matrix4x4; that is M11, M22, and M33.
		/// </summary>
		public Vector3 Scale
        {
            get { return new Vector3(m00, m11, m22); }
            set { m00 = value.X; m11 = value.Y; m22 = value.Z; }
        }

        /// <summary>
		/// Gets a value indicating whether this instance is an identity Matrix4x4.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is an identity Matrix4x4; otherwise, <c>false</c>.
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
		/// <value>The value of the Matrix4x4 component, depending on the index.</value>
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
                    case 3: return m03;
                    case 4: return m10;
                    case 5: return m11;
                    case 6: return m12;
                    case 7: return m13;
                    case 8: return m20;
                    case 9: return m21;
                    case 10: return m22;
                    case 11: return m23;
                    case 12: return m30;
                    case 13: return m31;
                    case 14: return m32;
                    case 15: return m33;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Matrix4x4 run from 0 to 15, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m03 = value; break;
                    case 4: m10 = value; break;
                    case 5: m11 = value; break;
                    case 6: m12 = value; break;
                    case 7: m13 = value; break;
                    case 8: m20 = value; break;
                    case 9: m21 = value; break;
                    case 10: m22 = value; break;
                    case 11: m23 = value; break;
                    case 12: m30 = value; break;
                    case 13: m31 = value; break;
                    case 14: m32 = value; break;
                    case 15: m33 = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Matrix4x4 run from 0 to 15, inclusive.");
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
                if (row < 0 || row > 3)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");
                if (column < 0 || column > 3)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");

                return this[(row * 4) + column];
            }

            set
            {
                if (row < 0 || row > 3)
                    throw new ArgumentOutOfRangeException("row", "Rows and columns for matrices run from 0 to 3, inclusive.");
                if (column < 0 || column > 3)
                    throw new ArgumentOutOfRangeException("column", "Rows and columns for matrices run from 0 to 3, inclusive.");

                this[(row * 4) + column] = value;
            }
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Matrix4"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Matrix4(float value)
        {
            m00 = m01 = m02 = m03 =
            m10 = m11 = m12 = m13 =
            m20 = m21 = m22 = m23 =
            m30 = m31 = m32 = m33 = value;
        }

        public Matrix4(float fm00, float fm01, float fm02, float fm03, float fm10, float fm11, float fm12, float fm13, float fm20, float fm21, float fm22, float fm23, float fm30, float fm31, float fm32, float fm33)
        {
            this.m00 = fm00;
            this.m01 = fm01;
            this.m02 = fm02;
            this.m03 = fm03;
            this.m10 = fm10;
            this.m11 = fm11;
            this.m12 = fm12;
            this.m13 = fm13;
            this.m20 = fm20;
            this.m21 = fm21;
            this.m22 = fm22;
            this.m23 = fm23;
            this.m30 = fm30;
            this.m31 = fm31;
            this.m32 = fm32;
            this.m33 = fm33;
        }

        /// <summary>
		/// Determines the sum of two matrices.
		/// </summary>
		/// <param name="left">The first Matrix4x4 to add.</param>
		/// <param name="right">The second Matrix4x4 to add.</param>
		/// <param name="result">When the method completes, contains the sum of the two matrices.</param>
		public static void Add(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.m00 = left.m00 + right.m00;
            result.m01 = left.m01 + right.m01;
            result.m02 = left.m02 + right.m02;
            result.m03 = left.m03 + right.m03;
            result.m10 = left.m10 + right.m10;
            result.m11 = left.m11 + right.m11;
            result.m12 = left.m12 + right.m12;
            result.m13 = left.m13 + right.m13;
            result.m20 = left.m20 + right.m20;
            result.m21 = left.m21 + right.m21;
            result.m22 = left.m22 + right.m22;
            result.m23 = left.m23 + right.m23;
            result.m30 = left.m30 + right.m30;
            result.m31 = left.m31 + right.m31;
            result.m32 = left.m32 + right.m32;
            result.m33 = left.m33 + right.m33;
        }

        /// <summary>
        /// Determines the sum of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to add.</param>
        /// <param name="right">The second Matrix4x4 to add.</param>
        /// <returns>The sum of the two matrices.</returns>
        public static Matrix4 Add(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to subtract.</param>
        /// <param name="right">The second Matrix4x4 to subtract.</param>
        /// <param name="result">When the method completes, contains the difference between the two matrices.</param>
        public static void Subtract(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.m00 = left.m00 - right.m00;
            result.m01 = left.m01 - right.m01;
            result.m02 = left.m02 - right.m02;
            result.m03 = left.m03 - right.m03;
            result.m10 = left.m10 - right.m10;
            result.m11 = left.m11 - right.m11;
            result.m12 = left.m12 - right.m12;
            result.m13 = left.m13 - right.m13;
            result.m20 = left.m20 - right.m20;
            result.m21 = left.m21 - right.m21;
            result.m22 = left.m22 - right.m22;
            result.m23 = left.m23 - right.m23;
            result.m30 = left.m30 - right.m30;
            result.m31 = left.m31 - right.m31;
            result.m32 = left.m32 - right.m32;
            result.m33 = left.m33 - right.m33;
        }

        /// <summary>
        /// Determines the difference between two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to subtract.</param>
        /// <param name="right">The second Matrix4x4 to subtract.</param>
        /// <returns>The difference between the two matrices.</returns>
        public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled Matrix4x4.</param>
        public static void Multiply(ref Matrix4 left, float scalar, out Matrix4 result)
        {
            result.m00 = left.m00 - scalar;
            result.m01 = left.m01 - scalar;
            result.m02 = left.m02 - scalar;
            result.m03 = left.m03 - scalar;
            result.m10 = left.m10 - scalar;
            result.m11 = left.m11 - scalar;
            result.m12 = left.m12 - scalar;
            result.m13 = left.m13 - scalar;
            result.m20 = left.m20 - scalar;
            result.m21 = left.m21 - scalar;
            result.m22 = left.m22 - scalar;
            result.m23 = left.m23 - scalar;
            result.m30 = left.m30 - scalar;
            result.m31 = left.m31 - scalar;
            result.m32 = left.m32 - scalar;
            result.m33 = left.m33 - scalar;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4 Multiply(Matrix4 left, float scalar)
        {
            Matrix4 result;
            Multiply(ref left, scalar, out result);
            return result;
        }

        /// <summary>
        /// Determines the product of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Matrix4x4 to multiply.</param>
        /// <param name="result">The product of the two matrices.</param>
        public static void Multiply(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.m00 = (left.m00 * right.m00) + (left.m01 * right.m10) + (left.m02 * right.m20) + (left.m03 * right.m30);
            result.m01 = (left.m00 * right.m01) + (left.m01 * right.m11) + (left.m02 * right.m21) + (left.m03 * right.m31);
            result.m02 = (left.m00 * right.m02) + (left.m01 * right.m12) + (left.m02 * right.m22) + (left.m03 * right.m32);
            result.m03 = (left.m00 * right.m03) + (left.m01 * right.m13) + (left.m02 * right.m23) + (left.m03 * right.m33);
            result.m10 = (left.m10 * right.m00) + (left.m11 * right.m10) + (left.m12 * right.m20) + (left.m13 * right.m30);
            result.m11 = (left.m10 * right.m01) + (left.m11 * right.m11) + (left.m12 * right.m21) + (left.m13 * right.m31);
            result.m12 = (left.m10 * right.m02) + (left.m11 * right.m12) + (left.m12 * right.m22) + (left.m13 * right.m32);
            result.m13 = (left.m10 * right.m03) + (left.m11 * right.m13) + (left.m12 * right.m23) + (left.m13 * right.m33);
            result.m20 = (left.m20 * right.m00) + (left.m21 * right.m10) + (left.m22 * right.m20) + (left.m23 * right.m30);
            result.m21 = (left.m20 * right.m01) + (left.m21 * right.m11) + (left.m22 * right.m21) + (left.m23 * right.m31);
            result.m22 = (left.m20 * right.m02) + (left.m21 * right.m12) + (left.m22 * right.m22) + (left.m23 * right.m32);
            result.m23 = (left.m20 * right.m03) + (left.m21 * right.m13) + (left.m22 * right.m23) + (left.m23 * right.m33);
            result.m30 = (left.m30 * right.m00) + (left.m31 * right.m10) + (left.m32 * right.m20) + (left.m33 * right.m30);
            result.m31 = (left.m30 * right.m01) + (left.m31 * right.m11) + (left.m32 * right.m21) + (left.m33 * right.m31);
            result.m32 = (left.m30 * right.m02) + (left.m31 * right.m12) + (left.m32 * right.m22) + (left.m33 * right.m32);
            result.m33 = (left.m30 * right.m03) + (left.m31 * right.m13) + (left.m32 * right.m23) + (left.m33 * right.m33);
        }

        /// <summary>
		/// Scales a Matrix4x4 by the given value.
		/// </summary>
		/// <param name="left">The Matrix4x4 to scale.</param>
		/// <param name="scalar">The amount by which to scale.</param>
		/// <param name="result">When the method completes, contains the scaled Matrix4x4.</param>
		public static void Divide(ref Matrix4 left, float scalar, out Matrix4 result)
        {
            float inv = 1.0f / scalar;

            result.m00 = left.m00 * inv;
            result.m01 = left.m01 * inv;
            result.m02 = left.m02 * inv;
            result.m03 = left.m03 * inv;
            result.m10 = left.m10 * inv;
            result.m11 = left.m11 * inv;
            result.m12 = left.m12 * inv;
            result.m13 = left.m13 * inv;
            result.m20 = left.m20 * inv;
            result.m21 = left.m21 * inv;
            result.m22 = left.m22 * inv;
            result.m23 = left.m23 * inv;
            result.m30 = left.m30 * inv;
            result.m31 = left.m31 * inv;
            result.m32 = left.m32 * inv;
            result.m33 = left.m33 * inv;
        }

        /// <summary>
        /// Scales a Matrix4x4 by the given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4 Divide(Matrix4 left, float scalar)
        {
            Matrix4 result;
            Divide(ref left, scalar, out result);
            return result;
        }

        /// <summary>
        /// Determines the quotient of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <param name="result">When the method completes, contains the quotient of the two matrices.</param>
        public static void Divide(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.m00 = left.m00 / right.m00;
            result.m01 = left.m01 / right.m01;
            result.m02 = left.m02 / right.m02;
            result.m03 = left.m03 / right.m03;
            result.m10 = left.m10 / right.m10;
            result.m11 = left.m11 / right.m11;
            result.m12 = left.m12 / right.m12;
            result.m13 = left.m13 / right.m13;
            result.m20 = left.m20 / right.m20;
            result.m21 = left.m21 / right.m21;
            result.m22 = left.m22 / right.m22;
            result.m23 = left.m23 / right.m23;
            result.m30 = left.m30 / right.m30;
            result.m31 = left.m31 / right.m31;
            result.m32 = left.m32 / right.m32;
            result.m33 = left.m33 / right.m33;
        }

        /// <summary>
        /// Determines the quotient of two matrices.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <returns>The quotient of the two matrices.</returns>
        public static Matrix4 Divide(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Divide(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Negates a Matrix4x4.
        /// </summary>
        /// <param name="value">The Matrix4x4 to be negated.</param>
        /// <param name="result">When the method completes, contains the negated Matrix4x4.</param>
        public static void Negate(ref Matrix4 value, out Matrix4 result)
        {
            result.m00 = -value.m00;
            result.m01 = -value.m01;
            result.m02 = -value.m02;
            result.m03 = -value.m03;
            result.m10 = -value.m10;
            result.m11 = -value.m11;
            result.m12 = -value.m12;
            result.m13 = -value.m13;
            result.m20 = -value.m20;
            result.m21 = -value.m21;
            result.m22 = -value.m22;
            result.m23 = -value.m23;
            result.m30 = -value.m30;
            result.m31 = -value.m31;
            result.m32 = -value.m32;
            result.m33 = -value.m33;
        }

        /// <summary>
		/// Adds two matricies.
		/// </summary>
		/// <param name="left">The first Matrix4x4 to add.</param>
		/// <param name="right">The second Matrix4x4 to add.</param>
		/// <returns>The sum of the two matricies.</returns>
		public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Assert a Matrix4x4 (return it unchanged).
        /// </summary>
        /// <param name="value">The Matrix4x4 to assert (unchange).</param>
        /// <returns>The asserted (unchanged) Matrix4x4.</returns>
        public static Matrix4 operator +(Matrix4 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two matricies.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to subtract.</param>
        /// <param name="right">The second Matrix4x4 to subtract.</param>
        /// <returns>The difference between the two matricies.</returns>
        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Negates a Matrix4x4.
        /// </summary>
        /// <param name="value">The Matrix4x4 to negate.</param>
        /// <returns>The negated Matrix4x4.</returns>
        public static Matrix4 operator -(Matrix4 value)
        {
            Matrix4 result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by a given value.
        /// </summary>
        /// <param name="right">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4 operator *(float scalar, Matrix4 right)
        {
            Matrix4 result;
            Multiply(ref right, scalar, out result);
            return result;
        }


        /// <summary>
        /// Scales a Matrix4x4 by a given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4 operator *(Matrix4 left, float scalar)
        {
            Matrix4 result;
            Multiply(ref left, scalar, out result);
            return result;
        }

        /// <summary>
        /// Multiplies two matricies.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Matrix4x4 to multiply.</param>
        /// <returns>The product of the two matricies.</returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a Matrix4x4 by a given value.
        /// </summary>
        /// <param name="left">The Matrix4x4 to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled Matrix4x4.</returns>
        public static Matrix4 operator /(Matrix4 left, float scalar)
        {
            Matrix4 result;
            Divide(ref left, scalar, out result);
            return result;
        }

        /// <summary>
        /// Divides two matricies.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to divide.</param>
        /// <param name="right">The second Matrix4x4 to divide.</param>
        /// <returns>The quotient of the two matricies.</returns>
        public static Matrix4 operator /(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Divide(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies with vector.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Vector3 to multiply.</param>
        /// <returns>The product.</returns>
        public static Vector3 operator *(Matrix4 left, Vector3 right)
        {
            Vector3 result = new Vector3();

            float fInvW = 1.0f / (left.m30 * right.x + left.m31 * right.y + left.m32 * right.z + left.m33);

            result.x = (left.m00 * right.x + left.m01 * right.y + left.m02 * right.z + left.m03) * fInvW;
            result.y = (left.m10 * right.x + left.m11 * right.y + left.m12 * right.z + left.m13) * fInvW;
            result.z = (left.m20 * right.x + left.m21 * right.y + left.m22 * right.z + left.m23) * fInvW;

            return result;
        }

        /// <summary>
        /// Multiplies with vector.
        /// </summary>
        /// <param name="left">The first Matrix4x4 to multiply.</param>
        /// <param name="right">The second Vector3 to multiply.</param>
        /// <returns>The product.</returns>
        public static Vector4 operator *(Matrix4 left, Vector4 right)
        {
            return new Vector4(
                   left.m00 * right.x + left.m01 * right.y + left.m02 * right.z + left.m03 * right.w,
                   left.m10 * right.x + left.m11 * right.y + left.m12 * right.z + left.m13 * right.w,
                   left.m20 * right.x + left.m21 * right.y + left.m22 * right.z + left.m23 * right.w,
                   left.m30 * right.x + left.m31 * right.y + left.m32 * right.z + left.m33 * right.w
                   );
        }

        /// <summary>
		/// Returns a boolean indicating whether the given Matrix4 is equal to this Matrix4 instance.
		/// </summary>
		/// <param name="other">The Matrix4 to compare this instance to.</param>
		/// <returns>True if the other Matrix4 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Matrix4 other)
        {
            return
                other.m00 == m00 &&
                other.m01 == m01 &&
                other.m02 == m02 &&
                other.m03 == m03 &&

                other.m10 == m10 &&
                other.m11 == m11 &&
                other.m12 == m12 &&
                other.m13 == m13 &&

                other.m20 == m20 &&
                other.m21 == m21 &&
                other.m22 == m22 &&
                other.m23 == m23 &&

                other.m30 == m30 &&
                other.m31 == m31 &&
                other.m32 == m32 &&
                other.m33 == m33;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Matrix4 is equal to this Matrix4 instance.
        /// </summary>
        /// <param name="other">The Matrix4 to compare this instance to.</param>
        /// <returns>True if the other Matrix4 is equal to this instance; False otherwise.</returns>
        public bool Equals(Matrix4 other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Matrix4 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Matrix4; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4))
                return false;

            return Equals((Matrix4)obj);
        }

        /// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Matrix4 left, Matrix4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix4 left, Matrix4 right)
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
                m00.GetHashCode() + m01.GetHashCode() + m02.GetHashCode() + m03.GetHashCode() +
                m10.GetHashCode() + m11.GetHashCode() + m12.GetHashCode() + m13.GetHashCode() +
                m20.GetHashCode() + m21.GetHashCode() + m22.GetHashCode() + m23.GetHashCode() +
                m30.GetHashCode() + m31.GetHashCode() + m32.GetHashCode() + m33.GetHashCode();
        }
    }
}
