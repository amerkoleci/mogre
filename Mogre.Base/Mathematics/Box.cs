// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Box : IEquatable<Box>
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
		public int front;
		public int back;

		public int Width
		{
			get { return right - left; }
		}

		public int Height
		{
			get { return bottom - top; }
		}

		public int Depth
		{
			get { return back - front; }
		}

		public Box(int left, int top, int front, int right, int bottom, int back)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
			this.front = front;
			this.back = back;
		}

		public Box(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
			this.front = 0;
			this.back = 1;
		}

		public bool Contains(Box other)
		{
			return
				other.left >= left && other.top >= top && other.front >= front &&
				other.right <= right && other.bottom <= bottom && other.back <= back;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Box is equal to this Box instance.
		/// </summary>
		/// <param name="other">The Box to compare this instance to.</param>
		/// <returns>True if the other Box is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Box other)
		{
			return
				left == other.left &&
				top == other.top &&
				right == other.right &&
				bottom == other.bottom &&
				back == other.back &&
				front == other.front;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Box is equal to this Box instance.
		/// </summary>
		/// <param name="other">The Box to compare this instance to.</param>
		/// <returns>True if the other Box is equal to this instance; False otherwise.</returns>
		public bool Equals(Box other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Box instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Box; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Box))
				return false;

			return Equals((Box)obj);
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
			hash = HashCodeHelper.CombineHashCodes(hash, front.GetHashCode());
			hash = HashCodeHelper.CombineHashCodes(hash, back.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Box left, Box right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Box left, Box right)
		{
			return !left.Equals(right);
		}
	}
}
