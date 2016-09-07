// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct FloatRect : IEquatable<FloatRect>
	{
		public float left;
		public float top;
		public float right;
		public float bottom;

		public float Width
		{
			get { return right - left; }
		}

		public float Height
		{
			get { return bottom - top; }
		}

		public FloatRect(float left, float top, float right, float bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public bool Inside(float x, float y)
		{
			return x >= left && x <= right && y >= top && y <= bottom;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given FloatRect is equal to this FloatRect instance.
		/// </summary>
		/// <param name="other">The FloatRect to compare this instance to.</param>
		/// <returns>True if the other FloatRect is equal to this instance; False otherwise.</returns>
		public bool Equals(ref FloatRect other)
		{
			return left == other.left && top == other.top && right == other.right && bottom == other.bottom;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given FloatRect is equal to this FloatRect instance.
		/// </summary>
		/// <param name="other">The FloatRect to compare this instance to.</param>
		/// <returns>True if the other FloatRect is equal to this instance; False otherwise.</returns>
		public bool Equals(FloatRect other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this FloatRect instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this FloatRect; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is FloatRect))
				return false;

			return Equals((FloatRect)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash = left.GetHashCode();
			hash = HashCodeHelper.CombineHashCodes(hash, top.GetHashCode());
			hash = HashCodeHelper.CombineHashCodes(hash, right.GetHashCode());
			hash = HashCodeHelper.CombineHashCodes(hash, bottom.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(FloatRect left, FloatRect right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(FloatRect left, FloatRect right)
		{
			return !left.Equals(right);
		}
	}
}
