// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Angle : IEquatable<Angle>, IComparable<Angle>
	{
		readonly float _value;

		public Angle(float value)
		{
			_value = value;
		}

		public static implicit operator Radian(Angle angle)
		{
			Radian result = new Radian(Math.AngleUnitsToRadians(angle._value));
			return result;
		}

		public static implicit operator Degree(Angle angle)
		{
			Degree result = new Degree(Math.AngleUnitsToDegrees(angle._value));
			return result;
		}

		public int CompareTo(Angle other)
		{
			if (_value < other._value) return -1;
			if (_value > other._value) return 1;
			return 0;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Angle is equal to this Angle instance.
		/// </summary>
		/// <param name="other">The Angle to compare this instance to.</param>
		/// <returns>True if the other Angle is equal to this instance; False otherwise.</returns>
		public bool Equals(ref Angle other)
		{
			return _value == other._value;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Angle is equal to this Angle instance.
		/// </summary>
		/// <param name="other">The Angle to compare this instance to.</param>
		/// <returns>True if the other Angle is equal to this instance; False otherwise.</returns>
		public bool Equals(Angle other)
		{
			return Equals(ref other);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Angle instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Angle; False otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			if (!(obj is Angle))
				return false;

			return Equals((Angle)obj);
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
