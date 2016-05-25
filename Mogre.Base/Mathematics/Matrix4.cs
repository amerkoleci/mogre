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
		/// The size of the <see cref="Matrix4x4"/> type, in bytes.
		/// </summary>
		public const int SizeInBytes = 64; // 4 * 16

        /// <summary>
        /// Value at row 1 column 1 of the Matrix4x4.
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1 column 2 of the Matrix4x4.
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1 column 3 of the Matrix4x4.
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 1 column 4 of the Matrix4x4.
        /// </summary>
        public float M14;

        /// <summary>
        /// Value at row 2 column 1 of the Matrix4x4.
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2 column 2 of the Matrix4x4.
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2 column 3 of the Matrix4x4.
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 2 column 4 of the Matrix4x4.
        /// </summary>
        public float M24;

        /// <summary>
        /// Value at row 3 column 1 of the Matrix4x4.
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3 column 2 of the Matrix4x4.
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3 column 3 of the Matrix4x4.
        /// </summary>
        public float M33;

        /// <summary>
        /// Value at row 3 column 4 of the Matrix4x4.
        /// </summary>
        public float M34;

        /// <summary>
        /// Value at row 4 column 1 of the Matrix4x4.
        /// </summary>
        public float M41;

        /// <summary>
        /// Value at row 4 column 2 of the Matrix4x4.
        /// </summary>
        public float M42;

        /// <summary>
        /// Value at row 4 column 3 of the Matrix4x4.
        /// </summary>
        public float M43;

        /// <summary>
        /// Value at row 4 column 4 of the Matrix4x4.
        /// </summary>
        public float M44;

        /// <summary>
        /// A <see cref="Matrix4"/> with all of its components set to zero.
        /// </summary>
        public static readonly Matrix4 Zero = new Matrix4();

        /// <summary>
        /// The identity <see cref="Matrix4"/>.
        /// </summary>
        public static readonly Matrix4 Identity = new Matrix4 { M11 = 1.0f, M22 = 1.0f, M33 = 1.0f, M44 = 1.0f };

        /// <summary>
		/// Gets or sets the first row in the Matrix4x4; that is M11, M12, M13, and M14.
		/// </summary>
		public Vector4 Row1
        {
            get { return new Vector4(M11, M12, M13, M14); }
            set { M11 = value.X; M12 = value.Y; M13 = value.Z; M14 = value.W; }
        }

        /// <summary>
        /// Gets or sets the second row in the Matrix4x4; that is M21, M22, M23, and M24.
        /// </summary>
        public Vector4 Row2
        {
            get { return new Vector4(M21, M22, M23, M24); }
            set { M21 = value.X; M22 = value.Y; M23 = value.Z; M24 = value.W; }
        }

        /// <summary>
        /// Gets or sets the third row in the Matrix4x4; that is M31, M32, M33, and M34.
        /// </summary>
        public Vector4 Row3
        {
            get { return new Vector4(M31, M32, M33, M34); }
            set { M31 = value.X; M32 = value.Y; M33 = value.Z; M34 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fourth row in the Matrix4x4; that is M41, M42, M43, and M44.
        /// </summary>
        public Vector4 Row4
        {
            get { return new Vector4(M41, M42, M43, M44); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; M44 = value.W; }
        }

        /// <summary>
		/// Gets or sets the first column in the Matrix4x4; that is M11, M21, M31, and M41.
		/// </summary>
		public Vector4 Column1
        {
            get { return new Vector4(M11, M21, M31, M41); }
            set { M11 = value.X; M21 = value.Y; M31 = value.Z; M41 = value.W; }
        }

        /// <summary>
        /// Gets or sets the second column in the Matrix4x4; that is M12, M22, M32, and M42.
        /// </summary>
        public Vector4 Column2
        {
            get { return new Vector4(M12, M22, M32, M42); }
            set { M12 = value.X; M22 = value.Y; M32 = value.Z; M42 = value.W; }
        }

        /// <summary>
        /// Gets or sets the third column in the Matrix4x4; that is M13, M23, M33, and M43.
        /// </summary>
        public Vector4 Column3
        {
            get { return new Vector4(M13, M23, M33, M43); }
            set { M13 = value.X; M23 = value.Y; M33 = value.Z; M43 = value.W; }
        }

        /// <summary>
        /// Gets or sets the fourth column in the Matrix4x4; that is M14, M24, M34, and M44.
        /// </summary>
        public Vector4 Column4
        {
            get { return new Vector4(M14, M24, M34, M44); }
            set { M14 = value.X; M24 = value.Y; M34 = value.Z; M44 = value.W; }
        }

        /// <summary>
		/// Gets or sets the translation of the Matrix4x4; that is M41, M42, and M43.
		/// </summary>
		public Vector3 Translation
        {
            get { return new Vector3(M41, M42, M43); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; }
        }

        /// <summary>
		/// Gets or sets the scale of the Matrix4x4; that is M11, M22, and M33.
		/// </summary>
		public Vector3 Scale
        {
            get { return new Vector3(M11, M22, M33); }
            set { M11 = value.X; M22 = value.Y; M33 = value.Z; }
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
                    case 0: return M11;
                    case 1: return M12;
                    case 2: return M13;
                    case 3: return M14;
                    case 4: return M21;
                    case 5: return M22;
                    case 6: return M23;
                    case 7: return M24;
                    case 8: return M31;
                    case 9: return M32;
                    case 10: return M33;
                    case 11: return M34;
                    case 12: return M41;
                    case 13: return M42;
                    case 14: return M43;
                    case 15: return M44;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Matrix4x4 run from 0 to 15, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M14 = value; break;
                    case 4: M21 = value; break;
                    case 5: M22 = value; break;
                    case 6: M23 = value; break;
                    case 7: M24 = value; break;
                    case 8: M31 = value; break;
                    case 9: M32 = value; break;
                    case 10: M33 = value; break;
                    case 11: M34 = value; break;
                    case 12: M41 = value; break;
                    case 13: M42 = value; break;
                    case 14: M43 = value; break;
                    case 15: M44 = value; break;
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
        /// Creates an array containing the elements of the Matrix4x4.
        /// </summary>
        /// <returns>A sixteen-element array containing the components of the Matrix4x4.</returns>
        public float[] ToArray()
        {
            return new[] { M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44 };
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Matrix4"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Matrix4(float value)
        {
            M11 = M12 = M13 = M14 =
            M21 = M22 = M23 = M24 =
            M31 = M32 = M33 = M34 =
            M41 = M42 = M43 = M44 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4"/> struct.
        /// </summary>
        /// <param name="m11">The value to assign at row 1 column 1 of the Matrix4x4.</param>
        /// <param name="m12">The value to assign at row 1 column 2 of the Matrix4x4.</param>
        /// <param name="m13">The value to assign at row 1 column 3 of the Matrix4x4.</param>
        /// <param name="m14">The value to assign at row 1 column 4 of the Matrix4x4.</param>
        /// <param name="m21">The value to assign at row 2 column 1 of the Matrix4x4.</param>
        /// <param name="m22">The value to assign at row 2 column 2 of the Matrix4x4.</param>
        /// <param name="m23">The value to assign at row 2 column 3 of the Matrix4x4.</param>
        /// <param name="m24">The value to assign at row 2 column 4 of the Matrix4x4.</param>
        /// <param name="m31">The value to assign at row 3 column 1 of the Matrix4x4.</param>
        /// <param name="m32">The value to assign at row 3 column 2 of the Matrix4x4.</param>
        /// <param name="m33">The value to assign at row 3 column 3 of the Matrix4x4.</param>
        /// <param name="m34">The value to assign at row 3 column 4 of the Matrix4x4.</param>
        /// <param name="m41">The value to assign at row 4 column 1 of the Matrix4x4.</param>
        /// <param name="m42">The value to assign at row 4 column 2 of the Matrix4x4.</param>
        /// <param name="m43">The value to assign at row 4 column 3 of the Matrix4x4.</param>
        /// <param name="m44">The value to assign at row 4 column 4 of the Matrix4x4.</param>
        public Matrix4(float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the components of the Matrix4x4. This must be an array with sixteen elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than sixteen elements.</exception>
        public Matrix4(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 16)
                throw new ArgumentOutOfRangeException("values", "There must be sixteen and only sixteen input values for Matrix4x4.");

            M11 = values[0];
            M12 = values[1];
            M13 = values[2];
            M14 = values[3];

            M21 = values[4];
            M22 = values[5];
            M23 = values[6];
            M24 = values[7];

            M31 = values[8];
            M32 = values[9];
            M33 = values[10];
            M34 = values[11];

            M41 = values[12];
            M42 = values[13];
            M43 = values[14];
            M44 = values[15];
        }

        /// <summary>
		/// Returns a boolean indicating whether the given Matrix4 is equal to this Matrix4 instance.
		/// </summary>
		/// <param name="other">The Matrix4 to compare this instance to.</param>
		/// <returns>True if the other Matrix4 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Matrix4 other)
        {
            return
                other.M11 == M11 &&
                other.M12 == M12 &&
                other.M13 == M13 &&
                other.M14 == M14 &&

                other.M21 == M21 &&
                other.M22 == M22 &&
                other.M23 == M23 &&
                other.M24 == M24 &&

                other.M31 == M31 &&
                other.M32 == M32 &&
                other.M33 == M33 &&
                other.M34 == M34 &&

                other.M41 == M41 &&
                other.M42 == M42 &&
                other.M43 == M43 &&
                other.M44 == M44;
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
            return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
                M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
                M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
                M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
        }
    }
}
