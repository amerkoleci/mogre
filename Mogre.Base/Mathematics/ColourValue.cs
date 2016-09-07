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
	/// Represents a color using Red, Green, Blue, and Alpha stored as four float
	/// values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ColourValue : IEquatable<ColourValue>, IFormattable
	{
		/// <summary>
		/// The red component of the color.
		/// </summary>
		public float R;

		/// <summary>
		/// The green component of the color.
		/// </summary>
		public float G;

		/// <summary>
		/// The blue component of the color.
		/// </summary>
		public float B;

		/// <summary>
		/// The alpha component of the color.
		/// </summary>
		public float A;

		/// <summary>
		/// A <see cref="ColourValue"/> with all of its components set to zero.
		/// </summary>
		public static readonly ColourValue Zero = new ColourValue();

		/// <summary>
		/// A <see cref="ColourValue"/> with all of its components set to zero.
		/// </summary>
		public static readonly ColourValue ZERO = new ColourValue();

		/// <summary>
		/// A Black <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue Black = new ColourValue(0.0f, 0.0f, 0.0f, 1.0f);

		/// <summary>
		/// A TransparentBlack <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue TransparentBlack = new ColourValue(0.0f, 0.0f, 0.0f, 0.0f);

		/// <summary>
		/// A White <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue White = new ColourValue(1.0f, 1.0f, 1.0f, 1.0f);

		/// <summary>
		/// A Red <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue Red = new ColourValue(1.0f, 0.0f, 0.0f);

		/// <summary>
		/// A Green <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue Green = new ColourValue(0.0f, 1.0f, 0.0f);

		/// <summary>
		/// A Blue <see cref="ColourValue"/>.
		/// </summary>
		public static readonly ColourValue Blue = new ColourValue(0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public ColourValue(float value)
		{
			R = G = B = A = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Alir.Color4"/> struct.
		/// </summary>
		/// <param name="red">The red component of the color.</param>
		/// <param name="green">The green component of the color.</param>
		/// <param name="blue">The blue component of the color.</param>
		public ColourValue(float red, float green, float blue)
		{
			R = red;
			G = green;
			B = blue;
			A = 1.0f;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="red">The red component of the color.</param>
		/// <param name="green">The green component of the color.</param>
		/// <param name="blue">The blue component of the color.</param>
		/// <param name="alpha">The alpha component of the color.</param>
		public ColourValue(float red, float green, float blue, float alpha)
		{
			R = red;
			G = green;
			B = blue;
			A = alpha;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="argb">A packed integer containing all four color components.</param>
		public ColourValue(int argb)
		{
			A = ((argb >> 24) & 255) / 255.0f;
			R = ((argb >> 16) & 255) / 255.0f;
			G = ((argb >> 8) & 255) / 255.0f;
			B = (argb & 255) / 255.0f;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="red">The red component of the color.</param>
		/// <param name="green">The green component of the color.</param>
		/// <param name="blue">The blue component of the color.</param>
		public ColourValue(int red, int green, int blue)
		{
			R = red / 255.0f;
			G = green / 255.0f;
			B = blue / 255.0f;
			A = 1.0f;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="red">The red component of the color.</param>
		/// <param name="green">The green component of the color.</param>
		/// <param name="blue">The blue component of the color.</param>
		/// <param name="alpha">The alpha component of the color.</param>
		public ColourValue(int red, int green, int blue, int alpha)
		{
			R = red / 255.0f;
			G = green / 255.0f;
			B = blue / 255.0f;
			A = alpha / 255.0f;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColourValue"/> struct.
		/// </summary>
		/// <param name="values">The values to assign to the red, green, blue, and alpha components of the color. This must be an array with four elements.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
		public ColourValue(float[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != 4)
				throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Color4.");

			R = values[0];
			G = values[1];
			B = values[2];
			A = values[3];
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(ColourValue left, ColourValue right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(ColourValue left, ColourValue right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = R.GetHashCode();
				hashCode = (hashCode * 397) ^ G.GetHashCode();
				hashCode = (hashCode * 397) ^ B.GetHashCode();
				hashCode = (hashCode * 397) ^ A.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="ColourValue"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="ColourValue"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="ColourValue"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(ref ColourValue other)
		{
			return
				A == other.A &&
				R == other.R &&
				G == other.G &&
				B == other.B;
		}

		/// <summary>
		/// Determines whether the specified <see cref="ColourValue"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="ColourValue"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="ColourValue"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(ColourValue other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			if (!(value is ColourValue))
				return false;

			var strongValue = (ColourValue)value;
			return Equals(ref strongValue);
		}

		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Red:{0} Green:{1} Blue:{2} Alpha:{3}", R, G, B, A);
		}

		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>
		/// A <see cref="string"/> that represents this instance.
		/// </returns>
		public string ToString(string format)
		{
			if (format == null)
				return ToString();

			return string.Format(CultureInfo.CurrentCulture, "Red:{0} Green:{1} Blue:{2} Alpha:{3}", R.ToString(format, CultureInfo.CurrentCulture),
				G.ToString(format, CultureInfo.CurrentCulture), B.ToString(format, CultureInfo.CurrentCulture), A.ToString(format, CultureInfo.CurrentCulture));
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
			return string.Format(formatProvider, "Red:{0} Green:{1} Blue:{2} Alpha:{3}", R, G, B, A);
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
				return ToString(formatProvider);

			return string.Format(formatProvider, "Red:{0} Green:{1} Blue:{2} Alpha:{3}", R.ToString(format, formatProvider),
				G.ToString(format, formatProvider), B.ToString(format, formatProvider), A.ToString(format, formatProvider));
		}
	}
}
