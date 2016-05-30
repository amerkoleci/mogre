// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Mogre
{
    public static class Math
    {
        public enum AngleUnit
        {
            AU_DEGREE = 0,
            AU_RADIAN = 1,
            Degree = 0,
            Radian = 1
        }

        /// <summary>
		/// The value for which all absolute numbers smaller than are considered equal to zero.
		/// </summary>
		public const float ZeroTolerance = 1e-6f; // Value a 8x higher than 1.19209290E-07F

        /// <summary>
		/// A value specifying the approximation of π which is 180 degrees.
		/// </summary>
		public const float Pi = (float)PI;

        /// <summary>
        /// A value specifying the approximation of 2π which is 360 degrees.
        /// </summary>
        public const float TwoPi = (float)(2 * PI);

        /// <summary>
        /// A value specifying the approximation of π/2 which is 90 degrees.
        /// </summary>
        public const float PiOverTwo = (float)(PI / 2);

        /// <summary>
        /// A value specifying the approximation of π/4 which is 45 degrees.
        /// </summary>
        public const float PiOverFour = (float)(PI / 4);

        public static readonly float RadiansPerDegree = Pi / 180.0f;

        public static readonly float DegreesPerRadian = 180.0f / Pi;

        public const float POS_INFINITY = float.PositiveInfinity;

        public const float NEG_INFINITY = float.NegativeInfinity;

        public const float PI = 3.14159274f;

        public const float TWO_PI = 6.28318548f;

        public const float HALF_PI = 1.57079637f;

        public const float fDeg2Rad = 0.0174532924f;

        public const float fRad2Deg = 57.2957764f;

        public static AngleUnit CurrentAngleUnit = AngleUnit.Degree;

        public static float DegreesToRadians(float degrees)
        {
            return degrees * fDeg2Rad;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * fRad2Deg;
        }

        public static float RadiansToAngleUnits(float radians)
        {
            if (CurrentAngleUnit == AngleUnit.Degree)
                return radians * fRad2Deg;

            return radians;
        }

        public static float DegreesToAngleUnits(float degrees)
        {
            if (CurrentAngleUnit == AngleUnit.Radian)
                return degrees * fDeg2Rad;

            return degrees;
        }

        public static float AngleUnitsToRadians(float angleunits)
        {
            if (CurrentAngleUnit == AngleUnit.Degree)
                return angleunits * fDeg2Rad;

            return angleunits;
        }

        public static float AngleUnitsToDegrees(float angleunits)
        {
            if (CurrentAngleUnit == AngleUnit.Radian)
                return angleunits * fRad2Deg;

            return angleunits;
        }

        /// <summary>
		/// Determines whether the specified value is close to zero (0.0f).
		/// </summary>
		/// <param name="a">The floating value.</param>
		/// <returns><c>true</c> if the specified value is close to zero (0.0f); otherwise, <c>false</c>.</returns>
		public static bool IsZero(float a)
        {
            return System.Math.Abs(a) < ZeroTolerance;
        }

        /// <summary>
        /// Determines whether the specified value is close to one (1.0f).
        /// </summary>
        /// <param name="a">The floating value.</param>
        /// <returns><c>true</c> if the specified value is close to one (1.0f); otherwise, <c>false</c>.</returns>
        public static bool IsOne(float a)
        {
            return IsZero(a - 1.0f);
        }

        public static bool Intersects(Plane plane, AxisAlignedBox box)
        {
            if (box.IsNull) return false;

            // Get corners of the box
            var corners = box.GetAllCorners();

            // Test which side of the plane the corners are
            // Intersection occurs when at least one corner is on the 
            // opposite side to another
            Plane.Side lastSide = plane.GetSide(corners[0]);
            for (int corner = 1; corner < 8; ++corner)
            {
                if (plane.GetSide(corners[corner]) != lastSide)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
