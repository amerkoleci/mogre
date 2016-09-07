// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Rect : IEquatable<Rect>
	{
		public int left;
		public int top;
		public int right;
		public int bottom;

		public int Width
		{
			get { return right - left; }
		}

		public int Height
		{
			get { return bottom - top; }
		}

		public Rect(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public bool Inside(int x, int y)
		{
			return x >= left && x <= right && y >= top && y <= bottom;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Rect is equal to this Rect instance.
		/// </summary>
		/// <param name="other">The Rect to compare this instance to.</param>
		/// <returns>True if the other Rect is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Rect other)
		{
			return left == other.left && top == other.top && right == other.right && bottom == other.bottom;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Rect is equal to this Rect instance.
		/// </summary>
		/// <param name="other">The Rect to compare this instance to.</param>
		/// <returns>True if the other Rect is equal to this instance; False otherwise.</returns>
		public bool Equals(Rect other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Rect instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Rect; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Rect))
				return false;

			return Equals((Rect)obj);
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
		public static bool operator ==(Rect left, Rect right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Rect left, Rect right)
		{
			return !left.Equals(right);
		}
	}
}
