// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Aabb : IEquatable<Aabb>
	{
		public Vector3 Center;
		public Vector3 HalfSize;

		public Vector3 Minimum
		{
			get { return Center - HalfSize; }
		}

		public Vector3 Maximum
		{
			get { return Center + HalfSize; }
		}

		public Vector3 Size
		{
			get { return HalfSize * 2.0f; }
		}

		public float Volume
		{
			get
			{
				Vector3 size = HalfSize * 2.0f;
				return size.x * size.y * size.z; // w * h * d
			}
		}

		public float Radius
		{
			get
			{
				return Math.Sqrt(HalfSize.DotProduct(HalfSize));
			}
		}

		public float RadiusOrigin
		{
			get
			{
				Vector3 v = Center;
				v.MakeAbs();
				v += HalfSize;
				return v.Length;
			}
		}

		public static readonly Aabb BOX_ZERO = new Aabb(Vector3.ZERO, Vector3.ZERO);
		public static readonly Aabb BOX_NULL = new Aabb(Vector3.ZERO, new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
		public static readonly Aabb BOX_INFINITE = new Aabb(Vector3.ZERO, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));

		public Aabb(AxisAlignedBox box)
		{
			Center = box.Center;
			HalfSize = box.HalfSize;
		}

		public Aabb(Vector3 center, Vector3 halfSize)
		{
			Center = center;
			HalfSize = halfSize;
		}

		/// <summary>
		/// Sets both minimum and maximum extents at once.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public void SetExtents(Vector3 min, Vector3 max)
		{
			Debug.Assert(
				min.x <= max.x && min.y <= max.y && min.z <= max.z,
				"The minimum corner of the box must be less than or equal to maximum corner");
			Center = (max + min) * 0.5f;
			HalfSize = (max - min) * 0.5f;
		}

		public static Aabb NewFromExtents(Vector3 min, Vector3 max)
		{
			Debug.Assert(min.x <= max.x && min.y <= max.y && min.z <= max.z,
				"The minimum corner of the box must be less than or equal to maximum corner");

			Aabb result = new Aabb
			{
				Center = (max + min) * 0.5f,
				HalfSize = (max - min) * 0.5f
			};

			return result;
		}

		public bool Contains(Aabb other)
		{
			Vector3 dist = Center - other.Center;

			// In theory, "abs( dist.x ) < mHalfSize - other.mHalfSize" should be more pipeline-
			// friendly because the processor can do the subtraction while the abs() is being performed,
			// however that variation won't handle the case where both boxes are infinite (will produce
			// nan instead and return false, when it should return true)

			//TODO: Profile whether '&&' or '&' is faster. Probably varies per architecture.
			return (Math.Abs(dist.x) + other.HalfSize.x <= HalfSize.x) &
					(Math.Abs(dist.y) + other.HalfSize.y <= HalfSize.y) &
					(Math.Abs(dist.z) + other.HalfSize.z <= HalfSize.z);
		}

		public bool Contains(Vector3 vector)
		{
			Vector3 dist = Center - vector;

			// ( abs( dist.x ) <= mHalfSize.x &&
			//   abs( dist.y ) <= mHalfSize.y &&
			//   abs( dist.z ) <= mHalfSize.z )
			return
				(Math.Abs(dist.x) <= HalfSize.x) &
				(Math.Abs(dist.y) <= HalfSize.y) &
				(Math.Abs(dist.z) <= HalfSize.z);
		}

		public bool Intersects(Aabb other)
		{
			Vector3 dist = Center - other.Center;
			Vector3 sumHalfSizes = HalfSize + other.HalfSize;

			// ( abs( center.x - center2.x ) <= halfSize.x + halfSize2.x &&
			//   abs( center.y - center2.y ) <= halfSize.y + halfSize2.y &&
			//   abs( center.z - center2.z ) <= halfSize.z + halfSize2.z )
			//TODO: Profile whether '&&' or '&' is faster. Probably varies per architecture.
			return (Math.Abs(dist.x) <= sumHalfSizes.x) &
					(Math.Abs(dist.y) <= sumHalfSizes.y) &
					(Math.Abs(dist.z) <= sumHalfSizes.z);
		}

		public float Distance(Vector3 vector)
		{
			Vector3 dist = Center - vector;

			// x = abs( dist.x ) - halfSize.x
			// y = abs( dist.y ) - halfSize.y
			// z = abs( dist.z ) - halfSize.z
			// return max( min( x, y, z ), 0 ); //Return minimum between xyz, clamp to zero

			dist.x = Math.Abs(dist.x) - HalfSize.x;
			dist.y = Math.Abs(dist.y) - HalfSize.y;
			dist.z = Math.Abs(dist.z) - HalfSize.z;

			return System.Math.Max(System.Math.Min(System.Math.Min(dist.x, dist.y), dist.z), 1.0f);
		}

		public void Merge(Aabb rhs)
		{
			Vector3 max = Center + HalfSize;
			max.MakeCeil(rhs.Center + rhs.HalfSize);

			Vector3 min = Center - HalfSize;
			min.MakeFloor(rhs.Center - rhs.HalfSize);

			if (float.IsInfinity(max.x) == false &&
				float.IsInfinity(max.y) == false &&
				float.IsInfinity(max.z) == false)
			{
				Center = (max + min) * 0.5f;
			}
			HalfSize = (max - min) * 0.5f;
		}

		public void Merge(Vector3 points)
		{
			Vector3 max = Center + HalfSize;
			max.MakeCeil(points);

			Vector3 min = Center - HalfSize;
			min.MakeFloor(points);

			if (float.IsInfinity(max.x) == false &&
			   float.IsInfinity(max.y) == false &&
			   float.IsInfinity(max.z) == false)
			{
				Center = (max + min) * 0.5f;
			}

			HalfSize = (max - min) * 0.5f;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Box is equal to this Box instance.
		/// </summary>
		/// <param name="other">The Box to compare this instance to.</param>
		/// <returns>True if the other Box is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Aabb other)
		{
			return
				Center.Equals(other.Center) &&
				HalfSize.Equals(other.HalfSize);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Box is equal to this Box instance.
		/// </summary>
		/// <param name="other">The Box to compare this instance to.</param>
		/// <returns>True if the other Box is equal to this instance; False otherwise.</returns>
		public bool Equals(Aabb other)
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
			if (!(obj is Aabb))
				return false;

			return Equals((Aabb)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash = Center.GetHashCode();
			hash = HashCodeHelper.CombineHashCodes(hash, HalfSize.GetHashCode());
			return hash;
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Aabb left, Aabb right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Aabb left, Aabb right)
		{
			return !left.Equals(right);
		}
	}
}
