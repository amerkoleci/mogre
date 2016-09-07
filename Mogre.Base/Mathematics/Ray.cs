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
	/// Representation of a ray in space, ie a line with an origin and direction.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Ray : IEquatable<Ray>, IFormattable
	{
		public Vector3 Origin;
		public Vector3 Direction;

		/// <summary>
		/// Initializes a new instance of the <see cref="Ray"/> struct.
		/// </summary>
		public Ray(Vector3 origin, Vector3 direction)
		{
			Origin = origin;
			Direction = direction;
		}

		public Vector3 GetPoint(float t)
		{
			return Origin + (Direction * t);
		}

		/// <summary>Tests whether this ray intersects the given plane. A pair structure where the first element indicates whether an intersection occurs, and if true, the second element will indicate the distance along the ray at which it intersects. This can be converted to a point in space by calling getPoint(). </summary>
		public Tuple<bool, float> Intersects(AxisAlignedBox box)
		{
			return Math.Intersects(this, box);
		}

		/// <summary>Tests whether this ray intersects the given plane. A pair structure where the first element indicates whether an intersection occurs, and if true, the second element will indicate the distance along the ray at which it intersects. This can be converted to a point in space by calling getPoint(). </summary>
		public Tuple<bool, float> Intersects(Sphere s)
		{
			return Math.Intersects(this, s);
		}

		/// <summary>Tests whether this ray intersects the given plane. A pair structure where the first element indicates whether an intersection occurs, and if true, the second element will indicate the distance along the ray at which it intersects. This can be converted to a point in space by calling getPoint(). </summary>
		public Tuple<bool, float> Intersects(Plane p)
		{
			return Math.Intersects(this, p);
		}

		/// <summary>Tests whether this ray intersects the given plane. A pair structure where the first element indicates whether an intersection occurs, and if true, the second element will indicate the distance along the ray at which it intersects. This can be converted to a point in space by calling getPoint(). </summary>
		//public Pair<bool, float> Intersects(PlaneBoundedVolume p)
		//{
		//    byte normalIsOutside;
		//    if (p.outside == Plane.Side.POSITIVE_SIDE)
		//    {
		//        normalIsOutside = 1;
		//    }
		//    else
		//    {
		//        normalIsOutside = 0;
		//    }
		//    return Math.Intersects(this, p.planes, normalIsOutside != 0);
		//}

		/// <summary>
		/// Returns a boolean indicating whether the given Ray is equal to this Ray instance.
		/// </summary>
		/// <param name="other">The Ray to compare this instance to.</param>
		/// <returns>True if the other Ray is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Ray other)
		{
			return
				Origin.Equals(ref other.Origin) &&
				Direction.Equals(other.Direction);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Ray is equal to this Ray instance.
		/// </summary>
		/// <param name="other">The Ray to compare this instance to.</param>
		/// <returns>True if the other Ray is equal to this instance; False otherwise.</returns>
		public bool Equals(Ray other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a String representing this Ray instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			return ToString("G", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Ray instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Ray instance, using the specified format to format individual elements 
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			var sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator + " ";
			sb.Append(Origin.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(Direction.ToString(format, formatProvider));
			return sb.ToString();
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash = Origin.GetHashCode();
			hash = HashCodeHelper.CombineHashCodes(hash, Direction.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Ray instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Ray; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Ray))
				return false;
			return Equals((Ray)obj);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Ray left, Ray right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Ray left, Ray right)
		{
			return !left.Equals(right);
		}
	}
}
