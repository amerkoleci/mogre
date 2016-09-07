// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Degree : IEquatable<Degree>, IComparable<Degree>
	{
		readonly float _value;

		public float ValueAngleUnits
		{
			get
			{
				return Math.DegreesToAngleUnits(_value);
			}
		}

		public float ValueRadians
		{
			get
			{
				return Math.DegreesToRadians(_value);
			}
		}

		public float ValueDegrees
		{
			get
			{
				return _value;
			}
		}

		public Degree(Radian r)
		{
			_value = r.ValueDegrees;
		}

		public Degree(float d)
		{
			_value = d;
		}

		public static implicit operator Degree(Radian r)
		{
			Degree result = new Degree(r.ValueDegrees);
			return result;
		}

		public static implicit operator Degree(float f)
		{
			Degree result = new Degree(f);
			return result;
		}

		public static Degree operator +(Degree l, Radian r)
		{
			Degree result = new Degree(l._value + r.ValueDegrees);
			return result;
		}

		public static Degree operator +(Degree l, Degree d)
		{
			Degree result = new Degree(l._value + d._value);
			return result;
		}

		public static Degree operator -(Degree l, Radian r)
		{
			Degree result = new Degree(l._value - r.ValueDegrees);
			return result;
		}

		public static Degree operator -(Degree l, Degree d)
		{
			Degree result = new Degree(l._value - d._value);
			return result;
		}

		public static Degree operator -(Degree d)
		{
			Degree result = new Degree(-d._value);
			return result;
		}

		public static Degree operator *(Degree l, Degree f)
		{
			Degree result = new Degree(l._value * f._value);
			return result;
		}

		public static Degree operator *(float f, Degree r)
		{
			Degree result = new Degree(r._value * f);
			return result;
		}

		public static Degree operator *(Degree l, float f)
		{
			Degree result = new Degree(l._value * f);
			return result;
		}

		public static Degree operator /(float f, Degree r)
		{
			Degree result = new Degree(f / r._value);
			return result;
		}

		public static Degree operator /(Degree l, float f)
		{
			Degree result = new Degree(l._value / f);
			return result;
		}

		public static bool operator <(Degree l, Degree d)
		{
			return l._value < d._value;
		}

		public static bool operator <=(Degree l, Degree d)
		{
			return l._value <= d._value;
		}

		public static bool operator ==(Degree l, Degree d)
		{
			return l._value == d._value;
		}

		public static bool operator !=(Degree l, Degree d)
		{
			return l._value != d._value;
		}

		public static bool operator >=(Degree l, Degree d)
		{
			return l._value >= d._value;
		}

		public static bool operator >(Degree l, Degree d)
		{
			return l._value > d._value;
		}

		public int CompareTo(Degree other)
		{
			if (_value < other._value) return -1;
			if (_value > other._value) return 1;
			return 0;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Degree is equal to this Degree instance.
		/// </summary>
		/// <param name="other">The Degree to compare this instance to.</param>
		/// <returns>True if the other Degree is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Degree other)
		{
			return _value == other._value;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Degree is equal to this Degree instance.
		/// </summary>
		/// <param name="other">The Degree to compare this instance to.</param>
		/// <returns>True if the other Degree is equal to this instance; False otherwise.</returns>
		public bool Equals(Degree other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Degree instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Degree; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Degree))
				return false;

			return Equals((Degree)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}
}
