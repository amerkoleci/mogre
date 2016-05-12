using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Mogre
{
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

		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;

		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;

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
	}
}
