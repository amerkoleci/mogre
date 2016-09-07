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
	/// A sphere primitive, mostly used for bounds checking. 
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Sphere : IEquatable<Sphere>, IFormattable
	{
		public float Radius;
		public Vector3 Center;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sphere"/> struct.
		/// </summary>
		public Sphere(Vector3 center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public bool Intersects(Sphere other)
		{
			return (other.Center - Center).Length <=
				(other.Radius + Radius);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Sphere is equal to this Sphere instance.
		/// </summary>
		/// <param name="other">The Sphere to compare this instance to.</param>
		/// <returns>True if the other Sphere is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Sphere other)
		{
			return
				Center.Equals(ref other.Center) &&
				Radius == other.Radius;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Sphere is equal to this Sphere instance.
		/// </summary>
		/// <param name="other">The Sphere to compare this instance to.</param>
		/// <returns>True if the other Sphere is equal to this instance; False otherwise.</returns>
		public bool Equals(Sphere other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a String representing this Sphere instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return ToString("G", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Sphere instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Sphere instance, using the specified format to format individual elements 
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			var sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator + " ";
			sb.Append(Center.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(Radius.ToString(format, formatProvider));
			return sb.ToString();
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash = Center.GetHashCode();
			hash = HashCodeHelper.CombineHashCodes(hash, Radius.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Sphere instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Sphere; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Sphere))
				return false;
			return Equals((Sphere)obj);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Sphere left, Sphere right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Sphere left, Sphere right)
		{
			return !left.Equals(right);
		}
	}
}
