using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Mogre
{
	/// <summary>
	/// Represents a three dimensional mathematical vector.
	/// </summary>
	[DataContract(Name = "Vector3")]
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
		public float Length()
		{
			return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
		}

		/// <summary>
		/// Calculates the squared length of the vector.
		/// </summary>
		/// <returns>The squared length of the vector.</returns>
		/// <remarks>
		/// and speed is of the essence.
		/// </remarks>
		public float LengthSquared()
		{
			return (X * X) + (Y * Y) + (Z * Z);
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
