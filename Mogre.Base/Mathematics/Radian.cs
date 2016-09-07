// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Radian : IEquatable<Radian>, IComparable<Radian>
	{
		readonly float _value;

		public float ValueAngleUnits
		{
			get
			{
				return Math.RadiansToAngleUnits(_value);
			}
		}

		public float ValueRadians
		{
			get
			{
				return _value;
			}
		}

		public float ValueDegrees
		{
			get
			{
				return Math.RadiansToDegrees(_value);
			}
		}

		public Radian(Degree d)
		{
			_value = d.ValueRadians;
		}

		public Radian(float r)
		{
			_value = r;
		}

		public static implicit operator Radian(Degree d)
		{
			Radian result = new Radian(d.ValueRadians);
			return result;
		}

		public static implicit operator Radian(float f)
		{
			Radian result = new Radian(f);
			return result;
		}

		public static Radian operator +(Radian l, Degree d)
		{
			Radian result = new Radian((float)((double)l._value + (double)d.ValueRadians));
			return result;
		}

		public static Radian operator +(Radian l, Radian r)
		{
			Radian result = new Radian(l._value + r._value);
			return result;
		}

		public static Radian operator -(Radian l, Degree d)
		{
			Radian result = new Radian(l._value - d.ValueRadians);
			return result;
		}

		public static Radian operator -(Radian l, Radian r)
		{
			Radian result = new Radian(l._value - r._value);
			return result;
		}

		public static Radian operator -(Radian r)
		{
			Radian result = new Radian(-r._value);
			return result;
		}

		public static Radian operator *(Radian l, Radian f)
		{
			Radian result = new Radian(l._value * f._value);
			return result;
		}

		public static Radian operator *(float f, Radian r)
		{
			Radian result = new Radian(r._value * f);
			return result;
		}

		public static Radian operator *(Radian l, float f)
		{
			Radian result = new Radian(l._value * f);
			return result;
		}

		public static Radian operator /(float f, Radian r)
		{
			Radian result = new Radian(f / r._value);
			return result;
		}

		public static Radian operator /(Radian l, float f)
		{
			Radian result = new Radian(l._value / f);
			return result;
		}

		public static bool operator <(Radian l, Radian r)
		{
			return l._value < r._value;
		}

		public static bool operator <=(Radian l, Radian r)
		{
			return l._value <= r._value;
		}

		public static bool operator ==(Radian l, Radian r)
		{
			return l._value == r._value;
		}

		public static bool operator !=(Radian l, Radian r)
		{
			return l._value != r._value;
		}

		public static bool operator >=(Radian l, Radian r)
		{
			return l._value >= r._value;
		}

		public static bool operator >(Radian l, Radian r)
		{
			return l._value > r._value;
		}

		public int CompareTo(Radian other)
		{
			if (_value < other._value) return -1;
			if (_value > other._value) return 1;
			return 0;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Radian is equal to this Radian instance.
		/// </summary>
		/// <param name="other">The Radian to compare this instance to.</param>
		/// <returns>True if the other Radian is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Radian other)
		{
			return _value == other._value;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Radian is equal to this Radian instance.
		/// </summary>
		/// <param name="other">The Radian to compare this instance to.</param>
		/// <returns>True if the other Radian is equal to this instance; False otherwise.</returns>
		public bool Equals(Radian other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Radian instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Radian; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Radian))
				return false;

			return Equals((Radian)obj);
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
