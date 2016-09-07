// Copyright (C) Amer Koleci
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
	/// Represents a two dimensional mathematical vector.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		/// <summary>
		/// The size of the <see cref="Vector2"/> type, in bytes.
		/// </summary>
		public const int SizeInBytes = 8;

		/// <summary>
		/// A <see cref="Vector2"/> with all of its components set to zero.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2();

		/// <summary>
		/// The X unit <see cref="Vector2"/> (1, 0).
		/// </summary>
		public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

		/// <summary>
		/// The Y unit <see cref="Vector2"/> (0, 1).
		/// </summary>
		public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

		/// <summary>
		/// A <see cref="Vector2"/> with all of its components set to one.
		/// </summary>
		public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

		public static Vector2 ZERO = new Vector2();

		public static Vector2 UNIT_X = new Vector2(1.0f, 0.0f);

		public static Vector2 UNIT_Y = new Vector2(0.0f, 1.0f);

		public static Vector2 NEGATIVE_UNIT_X = new Vector2(-1.0f, 0.0f);

		public static Vector2 NEGATIVE_UNIT_Y = new Vector2(0.0f, -1.0f);

		public static Vector2 UNIT_SCALE = new Vector2(1.0f, 1.0f);

		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;

		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;

		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float x { get { return X; } set { X = value; } }

		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float y { get { return Y; } set { Y = value; } }

		/// <summary>Returns true if this vector is zero length. </summary>
		public bool IsZeroLength
		{
			get
			{
				float length = X * X + Y * Y;
				return length < Math.ZeroTolerance;
			}
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return X;
					case 1: return Y;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
			}
			set
			{
				switch (index)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					default: throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
				}
			}
		}

		/// <summary>
		/// Returns the length (magnitude) of the vector. This operation requires a square root and is expensive in terms of CPU operations. 
		/// If you don't need to know the exact length (e.g. for just comparing lengths) use squaredLength() instead. 
		/// </summary>
		public float Length
		{
			get
			{
				return (float)System.Math.Sqrt(X * X + Y * Y);
			}
		}

		/// <summary>
		/// Returns the square of the length(magnitude) of the vector. 
		/// This method is for efficiency - calculating the actual length of a vector requires a square root, which is expensive in terms of the operations required. 
		/// This method returns the square of the length of the vector, i.e. the same as the length but before the square root is taken. 
		/// Use this if you want to find the longest / shortest vector without incurring the square root. 
		/// </summary>
		public float SquaredLength
		{
			get
			{
				return X * X + Y * Y;
			}
		}

		public Vector2 Perpendicular
		{
			get
			{
				return new Vector2(-Y, X);
			}
		}

		/// <summary>
		/// As normalise, except that this vector is unaffected and the normalised vector is returned as a copy. 
		/// </summary>
		public Vector2 NormalisedCopy
		{
			get
			{
				Vector2 result = this;
				result.Normalise();
				return result;
			}
		}

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>
		public Vector2(float value) : this(value, value)
		{
		}

		/// <summary>
		/// Constructs a vector with the given individual elements.
		/// </summary>
		/// <param name="x">The X component.</param>
		/// <param name="y">The Y component.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2"/> struct.
		/// </summary>
		/// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with two elements.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than two elements.</exception>
		public Vector2(float[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			if (values.Length != 2)
				throw new ArgumentOutOfRangeException("values", "Vector2 needs at least two inputs");

			X = values[0];
			Y = values[1];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector2"/> struct.
		/// </summary>
		/// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with two elements.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than two elements.</exception>
		public Vector2(int[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			if (values.Length != 2)
				throw new ArgumentOutOfRangeException("values", "Vector2 needs at least two inputs");

			X = values[0];
			Y = values[1];
		}

		/// <summary>
		/// Returns a String representing this Vector2 instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return ToString("G", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector2 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector2 instance, using the specified format to format individual elements 
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			var sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			sb.Append('<');
			sb.Append(X.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(Y.ToString(format, formatProvider));
			sb.Append('>');
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
			return hash;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector2 is equal to this Vector2 instance.
		/// </summary>
		/// <param name="other">The Vector2 to compare this instance to.</param>
		/// <returns>True if the other Vector2 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Vector2 other)
		{
			return
				X == other.X &&
				Y == other.Y;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector2 is equal to this Vector2 instance.
		/// </summary>
		/// <param name="other">The Vector2 to compare this instance to.</param>
		/// <returns>True if the other Vector2 is equal to this instance; False otherwise.</returns>
		//[JitIntrinsic]
		public bool Equals(Vector2 other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector2 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector2; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Vector2))
				return false;

			return Equals((Vector2)obj);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are not equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are not equal; False if they are equal.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !(left == right);
		}

		public static Vector2 operator +(float scalar, Vector2 vector)
		{
			var result = new Vector2(scalar);
			result += vector;
			return result;
		}

		public static Vector2 operator +(Vector2 vector, float scalar)
		{
			var result = new Vector2(scalar);
			result += vector;
			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X + right.X, left.Y + right.Y);
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X - right.X, left.Y - right.Y);
		}

		/// <summary>
		/// Negates a given vector.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 value)
		{
			return new Vector2(-value.X, -value.Y);
		}

		public static Vector2 operator -(float scalar, Vector2 vector)
		{
			var result = new Vector2(scalar);
			result -= vector;
			return result;
		}

		public static Vector2 operator -(Vector2 vector, float scalar)
		{
			var rkVector = new Vector2(scalar);
			return vector - rkVector;
		}


		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(float scalar, Vector2 value)
		{
			return new Vector2(value.X * scalar, value.Y * scalar);
		}

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 value, float scalar)
		{
			return new Vector2(value.X * scalar, value.Y * scalar);
		}

		/// <summary>
		/// Modulates a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left">The first vector to modulate.</param>
		/// <param name="right">The second vector to modulate.</param>
		/// <returns>The modulated vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X * right.X, left.Y * right.Y);
		}

		/// <summary>
		/// Divides the first vector by the second.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 left, Vector2 right)
		{
			return new Vector2(left.X / right.X, left.Y / right.Y);
		}

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 value, float scalar)
		{
			return new Vector2(value.X / scalar, value.Y / scalar);
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2"/> to <see cref="Vector3"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector2 value)
		{
			return new Vector3(value, 0.0f);
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2"/> to <see cref="Vector4"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector4(Vector2 value)
		{
			return new Vector4(value, 0.0f, 0.0f);
		}

		public static bool operator <(Vector2 lhs, Vector2 rhs)
		{
			return lhs.X < rhs.X && lhs.Y < rhs.Y;
		}

		public static bool operator >(Vector2 lhs, Vector2 rhs)
		{
			return lhs.X > rhs.X && lhs.Y > rhs.Y;
		}

		/// <summary>Normalises the vector. This method normalises the vector such that it's length / magnitude is 1. The result is called a unit vector. This function will not crash for zero-sized vectors, but there will be no changes made to their components. The previous length of the vector. </summary>
		public float Normalise()
		{
			var length = Length;
			if (length > Math.ZeroTolerance)
			{
				var inv = 1.0f / length;
				X *= inv;
				Y *= inv;
			}

			return length;
		}

		public float CrossProduct(Vector2 vector)
		{
			return X * vector.Y - Y * vector.X;
		}

		public float DotProduct(Vector2 vector)
		{
			return X * vector.X + Y * vector.Y;
		}

		/// <summary>Calculates a reflection vector to the plane with the given normal . NB assumes 'this' is pointing AWAY FROM the plane, invert if it is not. </summary>
		public Vector2 Reflect(Vector2 normal)
		{
			Vector2 rkVector = 2.0f * DotProduct(normal) * normal;
			return this - rkVector;
		}

		/// <summary>Sets this vector's components to the maximum of its own and the ones of the passed in vector. 'Maximum' in this case means the combination of the highest value of x, y and z from both vectors. Highest is taken just numerically, not magnitude, so 1 &gt; -3. </summary>
		public void MakeCeil(Vector2 cmp)
		{
			if (cmp.X > X)
			{
				X = cmp.X;
			}

			if (cmp.Y > Y)
			{
				Y = cmp.Y;
			}
		}

		/// <summary>Sets this vector's components to the minimum of its own and the ones of the passed in vector. 'Minimum' in this case means the combination of the lowest value of x, y and z from both vectors. Lowest is taken just numerically, not magnitude, so -1 &lt; 0. </summary>
		public void MakeFloor(Vector2 cmp)
		{
			if (cmp.X < X)
			{
				X = cmp.X;
			}

			if (cmp.Y < Y)
			{
				Y = cmp.Y;
			}
		}

		/// <summary>
		/// Returns a vector at a point half way between this and the passed in vector.
		/// </summary>
		public Vector2 MidPoint(Vector2 vec)
		{
			return new Vector2(X + vec.X * 0.5f, Y + vec.Y * 0.5f);
		}
	}
}
