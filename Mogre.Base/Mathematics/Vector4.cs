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
	/// Represents a four dimensional mathematical vector.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector4 : IEquatable<Vector4>, IFormattable
	{
		/// <summary>
		/// The size of the <see cref="Vector4"/> type, in bytes.
		/// </summary>
		public const int SizeInBytes = 16;

		/// <summary>
		/// A <see cref="Vector4"/> with all of its components set to zero.
		/// </summary>
		public static readonly Vector4 Zero = new Vector4();

		/// <summary>
		/// The X unit <see cref="Vector4"/> (1, 0, 0, 0).
		/// </summary>
		public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

		/// <summary>
		/// The Y unit <see cref="Vector4"/> (0, 1, 0, 0).
		/// </summary>
		public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

		/// <summary>
		/// The Z unit <see cref="Vector4"/> (0, 0, 1, 0).
		/// </summary>
		public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

		/// <summary>
		/// The W unit <see cref="Vector4"/> (0, 0, 0, 1).
		/// </summary>
		public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

		/// <summary>
		/// A <see cref="Vector4"/> with all of its components set to one.
		/// </summary>
		public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

		public static Vector4 ZERO = new Vector4();

		/// <summary>
		/// The X component of the vector.
		/// </summary>

		public float X;

		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;

		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public float Z;

		/// <summary>
		/// The W component of the vector.
		/// </summary>
		public float W;

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
		/// The W component of the vector.
		/// </summary>
		public float w { get { return W; } set { W = value; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector4(float value)
		{
			X = value;
			Y = value;
			Z = value;
			W = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(Vector3 value, float w)
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
			W = w;
		}

		/// <summary>Creates a new instance of Vector4.</summary>
		/// <param name="value">Value to initialize X,Y,Z.</param>
		public Vector4(Vector3 value)
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
			W = 0.0f;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4(Vector2 value, float z, float w)
		{
			X = value.X;
			Y = value.Y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4"/> struct.
		/// </summary>
		/// <param name="values">The values to assign to the X, Y, Z, and W components of the vector. This must be an array with four elements.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
		public Vector4(float[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != 4)
				throw new ArgumentOutOfRangeException("values", "Vector4 needs at least fours inputs");

			X = values[0];
			Y = values[1];
			Z = values[2];
			W = values[3];
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		/// <remarks>
		/// <see cref="Vector4.LengthSquared"/> may be preferred when only the relative length is needed
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
		/// This method may be preferred to <see cref="Vector4.Length"/> when only a relative length is needed
		/// and speed is of the essence.
		/// </remarks>
		public float LengthSquared()
		{
			return (X * X) + (Y * Y) + (Z * Z) + (W * W);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector4 is equal to this Vector4 instance.
		/// </summary>
		/// <param name="other">The Vector4 to compare this instance to.</param>
		/// <returns>True if the other Vector4 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Vector4 other)
		{
			return
				X == other.X &&
				Y == other.Y &&
				Z == other.Z &&
				W == other.W;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector4 is equal to this Vector4 instance.
		/// </summary>
		/// <param name="other">The Vector4 to compare this instance to.</param>
		/// <returns>True if the other Vector4 is equal to this instance; False otherwise.</returns>
		public bool Equals(Vector4 other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a String representing this Vector4 instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return ToString("G", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector4 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector4 instance, using the specified format to format individual elements 
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			var sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator + " ";
			sb.Append(this.X.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(this.Y.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(this.Z.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(this.W.ToString(format, formatProvider));
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
		/// Returns a boolean indicating whether the given Object is equal to this Vector4 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector4; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Vector4))
				return false;
			return Equals((Vector4)obj);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Vector4 left, Vector4 right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Vector4 left, Vector4 right)
		{
			return !left.Equals(right);
		}

		public static bool operator <(Vector4 lhs, Vector4 rhs)
		{
			return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z && lhs.W < rhs.W;
		}

		public static bool operator >(Vector4 lhs, Vector4 rhs)
		{
			return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z && lhs.W < rhs.W;
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector2(Vector4 value)
		{
			return new Vector2(value.X, value.Y);
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector4"/> to <see cref="Vector3"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector3(Vector4 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}
	}
}
