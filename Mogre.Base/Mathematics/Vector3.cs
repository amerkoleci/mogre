// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Globalization;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Represents a three dimensional mathematical vector.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		/// <summary>
		/// The size of the <see cref="Vector3"/> type, in bytes.
		/// </summary>
		public const int SizeInBytes = 12;

		/// <summary>
		/// A <see cref="Vector3"/> with all of its components set to zero.
		/// </summary>
		public static readonly Vector3 Zero = new Vector3();

		/// <summary>
		/// The X unit <see cref="Vector3"/> (1, 0, 0).
		/// </summary>
		public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);

		/// <summary>
		/// The Y unit <see cref="Vector3"/> (0, 1, 0).
		/// </summary>
		public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);

		/// <summary>
		/// The Z unit <see cref="Vector3"/> (0, 0, 1).
		/// </summary>
		public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Returns a unit vector designating up (0, 1, 0).
		/// </summary>
		public static readonly Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);

		/// <summary>
		/// Returns a unit Vector3 designating down (0, -1, 0).
		/// </summary>
		public static readonly Vector3 Down = new Vector3(0.0f, -1.0f, 0.0f);

		/// <summary>
		/// Returns a unit Vector3 designating forward in a right-handed coordinate system(0, 0, -1).
		/// </summary>
		public static readonly Vector3 Forward = new Vector3(0.0f, 0.0f, -1.0f);

		/// <summary>
		/// Returns a unit Vector3 designating backward in a right-handed coordinate system (0, 0, 1).
		/// </summary>
		public static readonly Vector3 Backward = new Vector3(0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Returns a unit Vector3 pointing to the right (1, 0, 0).
		/// </summary>
		public static readonly Vector3 Right = new Vector3(1.0f, 0.0f, 0.0f);

		/// <summary>
		/// Returns a unit Vector3 designating left (-1, 0, 0).
		/// </summary>
		public static readonly Vector3 Left = new Vector3(-1.0f, 0.0f, 0.0f);

		/// <summary>
		/// A <see cref="Vector3"/> with all of its components set to one.
		/// </summary>
		public static readonly Vector3 One = new Vector3(1.0f, 1.0f, 1.0f);

		/// <summary>
		/// The Negative X unit <see cref="Vector3"/> (-1, 0, 0).
		/// </summary>
		public static readonly Vector3 NegativeUnitX = new Vector3(-1.0f, 0.0f, 0.0f);

		/// <summary>
		/// The Negative Y unit <see cref="Vector3"/> (0, -1, 0).
		/// </summary>
		public static readonly Vector3 NegativeUnitY = new Vector3(0.0f, -1.0f, 0.0f);

		/// <summary>
		/// The Negative Z unit <see cref="Vector3"/> (0, 0, -1).
		/// </summary>
		public static readonly Vector3 NegativeUnitZ = new Vector3(0.0f, 0.0f, -1.0f);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Vector3 PositiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Vector3 NegativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		public static Vector3 ZERO = new Vector3();

		public static Vector3 UNIT_X = new Vector3(1.0f, 0.0f, 0.0f);

		public static Vector3 UNIT_Y = new Vector3(0.0f, 1.0f, 0.0f);

		public static Vector3 UNIT_Z = new Vector3(0.0f, 0.0f, 1.0f);

		public static Vector3 NEGATIVE_UNIT_X = new Vector3(-1.0f, 0.0f, 0.0f);

		public static Vector3 NEGATIVE_UNIT_Y = new Vector3(0.0f, -1.0f, 0.0f);

		public static Vector3 NEGATIVE_UNIT_Z = new Vector3(0.0f, 0.0f, -1.0f);

		public static Vector3 UNIT_SCALE = new Vector3(1.0f, 1.0f, 1.0f);

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
		/// <value>The value of the X, Y, or Z component, depending on the index.</value>
		/// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, and 2 for the Z component.</param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 2].</exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return X;
					case 1: return Y;
					case 2: return Z;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
			}

			set
			{
				switch (index)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					default: throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Vector3(float value)
		{
			X = value;
			Y = value;
			Z = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3"/> struct.
		/// </summary>
		/// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		public Vector3(Vector2 value, float z)
		{
			X = value.X;
			Y = value.Y;
			Z = z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3"/> struct.
		/// </summary>
		/// <param name="values">The values to assign to the X, Y, and Z components of the vector. This must be an array with three elements.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements.</exception>
		public Vector3(float[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != 3)
				throw new ArgumentOutOfRangeException("values", "Vector3 needs at least three inputs");

			X = values[0];
			Y = values[1];
			Z = values[2];
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		/// <remarks>
		/// and speed is of the essence.
		/// </remarks>
		public float Length
		{
			get
			{
				return (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
			}
		}

		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		/// <remarks>
		/// and speed is of the essence.
		/// </remarks>
		public float SquaredLength
		{
			get
			{
				return (X * X) + (Y * Y) + (Z * Z);
			}
		}

		/// <summary>
		/// Calculates the dot Product operation on two vectors.
		/// <remarks>
		/// A dot product of two vectors v1 and v2 equals to |v1|*|v2|*cos(fi)
		/// where fi is the angle between the vectors and |v1| and |v2| are the vector lengths.
		/// For unit vectors (whose length is one) the dot product will obviously be just cos(fi).
		/// For example, if the unit vectors are parallel the result is cos(0) = 1.0f,
		/// if they are perpendicular the result is cos(PI/2) = 0.0f.
		/// The dot product may be calculated on vectors with any length however.
		/// A zero vector is treated as perpendicular to any vector (result is 0.0f).
		/// </remarks>
		/// </summary>
		/// <param name="vec">The vector to perform the Dot Product against.</param>
		/// <returns>Products of vector lengths and cosine of the angle between them. </returns>
		public float Dot(Vector3 vec)
		{
			return (X * vec.X) + (Y * vec.Y) + (Z * vec.Z);
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
		public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
		{
			result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector3 left, Vector3 right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
		}

		/// <summary>
		/// Turns the current vector into a unit vector.
		/// </summary>
		/// <remarks>
		/// The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </remarks>
		public float Normalise()
		{
			float length = Length;
			if (!Math.IsZero(length))
			{
				float inv = 1.0f / length;
				X *= inv;
				Y *= inv;
				Z *= inv;
			}

			return length;
		}

		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <param name="result">When the method completes, contains the normalized vector.</param>
		public static void Normalise(ref Vector3 value, out Vector3 result)
		{
			result = value;
			result.Normalise();
		}

		/// <summary>
		/// Converts the vector into a unit vector.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 Normalise(Vector3 value)
		{
			value.Normalise();
			return value;
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="vector">A vector to perform the Cross Product against.</param>
		/// <returns>When the method completes, contains he cross product of the two vectors.</returns>
		public Vector3 Cross(Vector3 vector)
		{
			return new Vector3(
				(Y * vector.Z) - (Z * vector.Y),
				(Z * vector.X) - (X * vector.Z),
				(X * vector.Y) - (Y * vector.X));
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <param name="result">When the method completes, contains he cross product of the two vectors.</param>
		public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
			result = new Vector3(
				(left.Y * right.Z) - (left.Z * right.Y),
				(left.Z * right.X) - (left.X * right.Z),
				(left.X * right.Y) - (left.Y * right.X));
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="left">First source vector.</param>
		/// <param name="right">Second source vector.</param>
		/// <returns>The cross product of the two vectors.</returns>
		public static Vector3 Cross(Vector3 left, Vector3 right)
		{
			Vector3 result;
			Cross(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <param name="result">When the method completes, contains the distance between the two vectors.</param>
		/// <remarks>
		/// <see cref="Vector3.DistanceSquared(ref Vector3, ref Vector3, out float)"/> may be preferred when only the relative distance is needed
		/// and speed is of the essence.
		/// </remarks>
		public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;

			result = (float)System.Math.Sqrt((x * x) + (y * y) + (z * z));
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The distance between the two vectors.</returns>
		/// <remarks>
		/// <see cref="Vector3.DistanceSquared(Vector3, Vector3)"/> may be preferred when only the relative distance is needed
		/// and speed is of the essence.
		/// </remarks>
		public static float Distance(Vector3 value1, Vector3 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;

			return (float)System.Math.Sqrt((x * x) + (y * y) + (z * z));
		}

		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
		/// <remarks>Distance squared is the value before taking the square root. 
		/// Distance squared can often be used in place of distance if relative comparisons are being made. 
		/// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
		/// compare the distance between A and B to the distance between A and C. Calculating the two distances 
		/// involves two square roots, which are computationally expensive. However, using distance squared 
		/// provides the same information and avoids calculating two square roots.
		/// </remarks>
		public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;

			result = (x * x) + (y * y) + (z * z);
		}

		/// <summary>
		/// Calculates the squared distance between two vectors.
		/// </summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The squared distance between the two vectors.</returns>
		/// <remarks>Distance squared is the value before taking the square root. 
		/// Distance squared can often be used in place of distance if relative comparisons are being made. 
		/// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
		/// compare the distance between A and B to the distance between A and C. Calculating the two distances 
		/// involves two square roots, which are computationally expensive. However, using distance squared 
		/// provides the same information and avoids calculating two square roots.
		/// </remarks>
		public static float DistanceSquared(Vector3 value1, Vector3 value2)
		{
			float x = value1.X - value2.X;
			float y = value1.Y - value2.Y;
			float z = value1.Z - value2.Z;

			return (x * x) + (y * y) + (z * z);
		}

		public void MakeFloor(Vector3 cmp)
		{
			if (cmp.x < x) x = cmp.x;
			if (cmp.y < y) y = cmp.y;
			if (cmp.z < z) z = cmp.z;
		}

		public void MakeCeil(Vector3 cmp)
		{
			if (cmp.x > x) x = cmp.x;
			if (cmp.y > y) y = cmp.y;
			if (cmp.z > z) z = cmp.z;
		}

		public float AbsDotProduct(Vector3 vector)
		{
			return System.Math.Abs(x * vector.x) + System.Math.Abs(y * vector.y) + System.Math.Abs(z * vector.z);
		}

		public Vector3 CrossProduct(Vector3 vector)
		{
			Vector3 result;

			result.X = y * vector.z - z * vector.y;
			result.Y = z * vector.x - x * vector.z;
			result.Z = x * vector.y - y * vector.x;

			return result;
		}

		public float DotProduct(Vector3 vector)
		{
			return X * vector.X + Y * vector.Y + Z * vector.Z;
		}

		/// <summary>
		/// Causes negative members to become positive
		/// </summary>
		public void MakeAbs()
		{
			x = Math.Abs(x);
			y = Math.Abs(y);
			z = Math.Abs(z);
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="left">The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		/// <summary>
		/// Assert a vector (return it unchanged).
		/// </summary>
		/// <param name="value">The vector to assert (unchange).</param>
		/// <returns>The asserted (unchanged) vector.</returns>
		public static Vector3 operator +(Vector3 value)
		{
			return value;
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="left">The first vector to subtract.</param>
		/// <param name="right">The second vector to subtract.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		public static Vector3 operator -(Vector3 value)
		{
			return new Vector3(-value.X, -value.Y, -value.Z);
		}

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator *(float scalar, Vector3 value)
		{
			return new Vector3(value.X * scalar, value.Y * scalar, value.Z * scalar);
		}

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator *(Vector3 value, float scalar)
		{
			return new Vector3(value.X * scalar, value.Y * scalar, value.Z * scalar);
		}

		/// <summary>
		///	Scales a Vector3 by another vector.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector3 operator *(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scalar">The amount by which to scale the vector.</param>
		/// <returns>The scaled vector.</returns>
		public static Vector3 operator /(Vector3 value, float scalar)
		{
			return new Vector3(value.X / scalar, value.Y / scalar, value.Z / scalar);
		}

		/// <summary>
		///		Used when a Vector3 is divided by another vector.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector3 operator /(Vector3 left, Vector3 right)
		{
			return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

		public static bool operator <(Vector3 lhs, Vector3 rhs)
		{
			return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
		}

		public static bool operator >(Vector3 lhs, Vector3 rhs)
		{
			return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector3 is equal to this Vector3 instance.
		/// </summary>
		/// <param name="other">The Vector3 to compare this instance to.</param>
		/// <returns>True if the other Vector3 is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Vector3 other)
		{
			return X == other.X &&
				   Y == other.Y &&
				   Z == other.Z;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector3 is equal to this Vector3 instance.
		/// </summary>
		/// <param name="other">The Vector3 to compare this instance to.</param>
		/// <returns>True if the other Vector3 is equal to this instance; False otherwise.</returns>
		public bool Equals(Vector3 other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector3"/> to <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector2(Vector3 value)
		{
			return new Vector2(value.X, value.Y);
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector3"/> to <see cref="Vector4"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Vector4(Vector3 value)
		{
			return new Vector4(value, 0.0f);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", X, Y, Z);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			if (format == null)
				return ToString();

			return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}",
				X.ToString(format, CultureInfo.CurrentCulture),
				Y.ToString(format, CultureInfo.CurrentCulture),
				Z.ToString(format, CultureInfo.CurrentCulture));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", X, Y, Z);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (format == null)
				ToString(formatProvider);

			return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}",
				X.ToString(format, formatProvider),
				Y.ToString(format, formatProvider),
				Z.ToString(format, formatProvider));
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			int hash = X.GetHashCode();
			hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
			hash = HashCodeHelper.CombineHashCodes(hash, Z.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Vector3))
				return false;
			return Equals((Vector3)obj);
		}
	}
}
