// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Represents a four dimensional mathematical quaternion in Ogre3D format (w, x, y, z).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="Quaternion"/> type, in bytes.
        /// </summary>
        public const int SizeInBytes = 16;

        /// <summary>
        /// A <see cref="Quaternion"/> with all of its components set to zero.
        /// </summary>
        public static readonly Quaternion Zero = new Quaternion();

        /// <summary>
        /// The identity <see cref="Quaternion"/> (1, 0, 0, 0).
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);

        public static Quaternion ZERO = new Quaternion();

        public static Quaternion IDENTITY = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// The W component of the quaternion.
        /// </summary>
        public float W;

        /// <summary>
        /// The X component of the quaternion.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the quaternion.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the quaternion.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public float w { get { return W; } set { W = value; } }

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public float x { get { return X; } set { X = value; } }

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public float y { get { return Y; } set { Y = value; } }

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public float z { get { return Z; } set { Z = value; } }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return W;
                    case 1: return X;
                    case 2: return Y;
                    case 3: return Z;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: W = value; break;
                    case 1: X = value; break;
                    case 2: Y = value; break;
                    case 3: Z = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
                }
            }
        }

        public float Norm
        {
            get
            {
                return W * W + X * X + Y * Y + Z * Z;
            }
        }

        /// <summary>Calculate the local pitch element of this quaternion </summary>
        public Radian Pitch
        {
            get
            {
                return Math.ATan2((2.0f * Y * Z) + (W * X), (W * W) - (X * X) - (Y * Y) + (Z * Z));
            }
        }

        /// <summary>Calculate the local roll element of this quaternion. </summary>
        public Radian Roll
        {
            get
            {
                return Math.ATan2((2.0f * X * Y) + (W * Z), (W * W) + (X * X) - (Y * Y) - (Z * Z));
            }
        }

        /// <summary>Calculate the local yaw element of this quaternion </summary>
        public Radian Yaw
        {
            get
            {
                return new Radian((float)System.Math.Asin((-2.0f * X * Z) - (W * Y)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="w">The w that will be assigned to w-component.</param>
        public Quaternion(float w)
        {
            W = w;
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the components.</param>
        public Quaternion(Vector4 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public Quaternion(Vector3 value, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
        /// <param name="z">Initial value for the Z component of the quaternion.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public Quaternion(Vector2 value, float z, float w)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="w">Initial value for the X component of the quaternion.</param>
        /// <param name="x">Initial value for the Y component of the quaternion.</param>
        /// <param name="y">Initial value for the Z component of the quaternion.</param>
        /// <param name="z">Initial value for the W component of the quaternion.</param>
        public Quaternion(float w, float x, float y, float z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z, and W components of the quaternion. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Quaternion(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            X = values[0];
            Y = values[1];
            Z = values[2];
            W = values[3];
        }

        /// <summary>Construct a quaternion from 3 orthonormal local axes. </summary>
        public Quaternion(Vector3 xaxis, Vector3 yaxis, Vector3 zaxis) : this()
        {
            FromAxes(xaxis, yaxis, zaxis);
        }

        public Quaternion(Radian angle, Vector3 axis) : this()
        {
            FromAngleAxis(angle, axis);
        }

        /// <summary>Construct a quaternion from a rotation matrix. </summary>
        public Quaternion(Matrix3 rot) : this()
        {
            FromRotationMatrix(rot);
        }


        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        /// <remarks>
        /// <see cref="Quaternion.LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public float Length()
        {
            return (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        /// <remarks>
        /// This method may be preferred to <see cref="Quaternion.Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public float LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Quaternion is equal to this Quaternion instance.
        /// </summary>
        /// <param name="other">The Quaternion to compare this instance to.</param>
        /// <returns>True if the other Quaternion is equal to this instance; False otherwise.</returns>
        public bool Equals(ref Quaternion other)
        {
            return
                X == other.X &&
                Y == other.Y &&
                Z == other.Z &&
                W == other.W;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Quaternion is equal to this Quaternion instance.
        /// </summary>
        /// <param name="other">The Quaternion to compare this instance to.</param>
        /// <returns>True if the other Quaternion is equal to this instance; False otherwise.</returns>
        public bool Equals(Quaternion other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Returns a String representing this Quaternion instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Quaternion instance, using the specified format to format individual elements.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a String representing this Quaternion instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator + " ";
            sb.Append(W.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(Y.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(Z.ToString(format, formatProvider));
            return sb.ToString();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int hash = X.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, Z.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, W.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Quaternion instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Quaternion; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion))
                return false;
            return Equals((Quaternion)obj);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        public float Normalise()
        {
            float length = Length();
            if (!Math.IsZero(length))
            {
                float inverse = 1.0f / length;
                X *= inverse;
                Y *= inverse;
                Z *= inverse;
                W *= inverse;
            }

            return length;
        }

        public static Vector3 operator *(Quaternion q, Vector3 v)
        {
            // nVidia SDK implementation
            Vector3 uv, uuv;
            Vector3 qvec = new Vector3(q.X, q.Y, q.Z);
            uv = qvec.CrossProduct(v);
            uuv = qvec.CrossProduct(uv);
            uv *= (2.0f * q.w);
            uuv *= 2.0f;

            return v + uv + uuv;
        }

        public static bool operator <(Quaternion lhs, Quaternion rhs)
        {
            return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z && lhs.W < rhs.W;
        }

        public static bool operator >(Quaternion lhs, Quaternion rhs)
        {
            return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z && lhs.W < rhs.W;
        }

        public void FromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
        {
            FromRotationMatrix(new Matrix3
            {
                m00 = xAxis.x,
                m10 = xAxis.y,
                m20 = xAxis.z,
                m01 = yAxis.x,
                m11 = yAxis.y,
                m21 = yAxis.z,
                m02 = zAxis.x,
                m12 = zAxis.y,
                m22 = zAxis.z
            });
        }

        public void FromAxes(Vector3[] akAxis)
        {
            Matrix3 matrix = new Matrix3();
            for (var index = 0; index < 3; index++)
            {
                matrix[0, index] = akAxis[index].X;
                matrix[1, index] = akAxis[index].Y;
                matrix[2, index] = akAxis[index].Z;
            }

            FromRotationMatrix(matrix);
        }

        public unsafe void FromRotationMatrix(Matrix3 rotationMatrix)
        {
            float fTrace = rotationMatrix.m00 + rotationMatrix.m11 + rotationMatrix.m22;
            float fRoot;

            if (fTrace > 0.0)
            {
                // |w| > 1/2, may as well choose w > 1/2
                fRoot = (float)System.Math.Sqrt(fTrace + 1.0f);  // 2w
                w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;  // 1/(4w)
                x = (rotationMatrix.m21 - rotationMatrix.m12) * fRoot;
                y = (rotationMatrix.m02 - rotationMatrix.m20) * fRoot;
                z = (rotationMatrix.m10 - rotationMatrix.m01) * fRoot;
            }
            else
            {
                // |w| <= 1/2
                var s_iNext = new int[] { 1, 2, 0 };
                int i = 0;
                if (rotationMatrix.m11 > rotationMatrix.m00)
                    i = 1;
                if (rotationMatrix.m22 > rotationMatrix[i, i])
                    i = 2;
                int j = s_iNext[i];
                int k = s_iNext[j];

                fRoot = (float)System.Math.Sqrt(rotationMatrix[i, i] - rotationMatrix[j, j] - rotationMatrix[k, k] + 1.0);

                this[i + 1] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                w = (rotationMatrix[k, j] - rotationMatrix[j, k]) * fRoot;
                this[j + 1] = (rotationMatrix[j, i] + rotationMatrix[i, j]) * fRoot;
                this[k + 1] = (rotationMatrix[k, i] + rotationMatrix[i, k]) * fRoot;
            }
        }

        public void FromAngleAxis(Radian rfAngle, Vector3 rkAxis)
        {
            Radian halfAngle = 0.5f * rfAngle;
            float fSin = Math.Sin(halfAngle);
            W = Math.Cos(halfAngle);
            X = fSin * rkAxis.X;
            Y = fSin * rkAxis.Y;
            Z = fSin * rkAxis.Z;
        }

        public float Dot(Quaternion quaternion)
        {
            return w * quaternion.w + x * quaternion.x + y * quaternion.y + z * quaternion.z;
        }
    }
}
