// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

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
		static readonly Random _randomizer = new Random();

		public static int IAbs(int iValue)
        {
            return (iValue >= 0 ? iValue : -iValue);
        }

        public static int ICeil(float value)
        {
            return (int)System.Math.Ceiling(value);
        }

        public static int IFloor(float value)
        {
            return (int)System.Math.Floor(value);
        }

        public static int ISign(int iValue)
        {
            return (iValue > 0 ? +1 : (iValue < 0 ? -1 : 0));
        }

        public static float Abs(float fValue)
        {
            return System.Math.Abs(fValue);
        }

        public static Radian ATan(float value)
        {
            return new Radian((float)System.Math.Atan(value));
        }

        public static Radian ATan2(float fY, float fX)
        {
            return new Radian((float)System.Math.Atan2(fY, fX));
        }

        public static float Sin(float value)
        {
            return (float)System.Math.Sin(value);
        }

        /// <summary>Sine function. </summary>
        /// <param name="radians">Angle in radians </param>
        public static float Sin(Radian radians)
        {
            return (float)System.Math.Sin(radians.ValueRadians);
        }

        public static float Cos(float value)
        {
            return (float)System.Math.Cos(value);
        }

        public static float Cos(Radian value)
        {
            return (float)System.Math.Cos(value.ValueRadians);
        }

        public static float Sqrt(float value)
        {
            return (float)System.Math.Sqrt(value);
        }

        public static Radian Sqrt(Radian value)
        {
            return new Radian((float)System.Math.Sqrt(value.ValueRadians));
        }

        public static float Sqr(float value)
        {
            return value * value;
        }

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

		public static float UnitRandom()
		{
			return (float)_randomizer.NextDouble();
		}

		public static float RangeRandom(float fLow, float fHigh)
		{
			return (fHigh - fLow) * UnitRandom() + fLow;
		}

		/// <summary>Calculate a face normal, no w-information. </summary>
		public static Vector3 CalculateBasicFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 rkVector = v3 - v1;
            Vector3 result = (v2 - v1).CrossProduct(rkVector);
            result.Normalise();
            return result;
        }

        /// <summary>Calculate a face normal without normalize, no w-information. </summary>
        public static Vector3 CalculateBasicFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 rkVector = v3 - v1;
            return (v2 - v1).CrossProduct(rkVector);
        }

        /// <summary>Calculate a face normal, including the w component which is the offset from the origin. </summary>
        public static Vector4 CalculateFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 vector = CalculateBasicFaceNormal(v1, v2, v3);
            return new Vector4(vector.x, vector.y, vector.z, -vector.DotProduct(v1));
        }

        /// <summary>Calculate a face normal without normalize, including the w component which is the offset from the origin. </summary>
        public static Vector4 CalculateFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 vector = Math.CalculateBasicFaceNormalWithoutNormalize(v1, v2, v3);
            return new Vector4(vector.x, vector.y, vector.z, -vector.DotProduct(v1));
        }

        public static Vector3 CalculateTangentSpaceVector(
            Vector3 position1, Vector3 position2, Vector3 position3,
            float u1, float v1, float u2, float v2, float u3, float v3)
        {
            //side0 is the vector along one side of the triangle of vertices passed in, 
            //and side1 is the vector along another side. Taking the cross product of these returns the normal.
            Vector3 side0 = position1 - position2;
            Vector3 side1 = position3 - position1;
            //Calculate face normal
            Vector3 normal = side1.CrossProduct(side0);
            normal.Normalise();
            //Now we use a formula to calculate the tangent. 
            float deltaV0 = v1 - v2;
            float deltaV1 = v3 - v1;
            Vector3 tangent = deltaV1 * side0 - deltaV0 * side1;
            tangent.Normalise();
            //Calculate binormal
            float deltaU0 = u1 - u2;
            float deltaU1 = u3 - u1;
            Vector3 binormal = deltaU1 * side0 - deltaU0 * side1;
            binormal.Normalise();
            //Now, we take the cross product of the tangents to get a vector which 
            //should point in the same direction as our normal calculated above. 
            //If it points in the opposite direction (the dot product between the normals is less than zero), 
            //then we need to reverse the s and t tangents. 
            //This is because the triangle has been mirrored when going from tangent space to object space.
            //reverse tangents if necessary
            Vector3 tangentCross = tangent.CrossProduct(binormal);
            if (tangentCross.DotProduct(normal) < 0.0f)
            {
                tangent = -tangent;
                binormal = -binormal;
            }

            return tangent;

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

        public static bool Intersects(Sphere sphere, Plane plane)
        {
            Vector3 center = sphere.Center;
            return Abs(plane.normal.DotProduct(center)) <= sphere.Radius;
        }

        public static Tuple<bool, float> Intersects(Ray ray, IList<Plane> planes, bool normalIsOutside)
        {
            bool allInside = true;
            bool hit = false;
            float distance = 0.0f;

            Plane.Side side = normalIsOutside ? Plane.Side.POSITIVE_SIDE : Plane.Side.NEGATIVE_SIDE;

            for (int i = 0; i < planes.Count; i++)
            {
                var plane = planes[i];
                Vector3 origin = ray.Origin;
                if (plane.GetSide(origin) == side)
                {
                    allInside = false;
                    Tuple<bool, float> pair = ray.Intersects(plane);
                    if (pair.Item1)
                    {
                        hit = true;
                        distance = System.Math.Max(distance, pair.Item2);
                    }
                }
            }

            if (allInside)
            {
                return Tuple.Create(true, 0.0f);
            }

            return Tuple.Create(hit, distance);
        }

        public static Tuple<bool, float> Intersects(Ray ray, IEnumerable<Plane> planes, bool normalIsOutside)
        {
            bool allInside = true;
            bool hit = false;
            float distance = 0.0f;

            Plane.Side side = normalIsOutside ? Plane.Side.POSITIVE_SIDE : Plane.Side.NEGATIVE_SIDE;

            foreach (var plane in planes)
            {
                Vector3 origin = ray.Origin;
                if (plane.GetSide(origin) == side)
                {
                    allInside = false;
                    Tuple<bool, float> pair = ray.Intersects(plane);
                    if (pair.Item1)
                    {
                        hit = true;
                        distance = System.Math.Max(distance, pair.Item2);
                    }
                }
            }

            if (allInside)
            {
                return Tuple.Create(true, 0.0f);
            }

            return Tuple.Create(hit, distance);
        }

        public static bool Intersects(Sphere sphere, AxisAlignedBox box)
        {
            if (box.IsNull)
            {
                return false;
            }

            // Use splitting planes
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;
            Vector3 min = box.Minimum;
            Vector3 max = box.Maximum;

            // just test facing planes, early fail if sphere is totally outside
            if (center.x < min.x &&
                min.x - center.x > radius)
            {
                return false;
            }
            if (center.x > max.x &&
                center.x - max.x > radius)
            {
                return false;
            }

            if (center.y < min.y &&
                min.y - center.y > radius)
            {
                return false;
            }
            if (center.y > max.y &&
                center.y - max.y > radius)
            {
                return false;
            }

            if (center.z < min.z &&
                min.z - center.z > radius)
            {
                return false;
            }
            if (center.z > max.z &&
                center.z - max.z > radius)
            {
                return false;
            }

            // Must intersect
            return true;
        }

        public static Tuple<bool, float> Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c)
        {
            return Intersects(ray, a, b, c, true, true);
        }

        public static Tuple<bool, float> Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c, bool positiveSide)
        {
            return Intersects(ray, a, b, c, positiveSide, true);
        }

        public static Tuple<bool, float> Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c, bool positiveSide, bool negativeSide)
        {
            Vector3 normal = CalculateBasicFaceNormalWithoutNormalize(a, b, c);
            return Intersects(ray, a, b, c, normal, positiveSide, negativeSide);
        }

        public static Tuple<bool, float> Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
        {
            return Intersects(ray, a, b, c, normal, true, true);
        }

        public static Tuple<bool, float> Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c, Vector3 normal, bool positiveSide, bool negativeSide)
        {
            //
            // Calculate intersection with plane.
            //
            float t;
            {
                float denom = normal.DotProduct(ray.Direction);

                // Check intersect side
                if (denom > +float.Epsilon)
                {
                    if (!negativeSide)
                        return Tuple.Create(false, 0.0f);
                }
                else if (denom < -float.Epsilon)
                {
                    if (!positiveSide)
                        return Tuple.Create(false, 0.0f);
                }
                else
                {
                    // Parallel or triangle area is close to zero when
                    // the plane normal not normalised.
                    return Tuple.Create(false, 0.0f);
                }

                t = normal.DotProduct(a - ray.Origin) / denom;

                if (t < 0)
                {
                    // Intersection is behind origin
                    return Tuple.Create(false, 0.0f);
                }
            }

            //
            // Calculate the largest area projection plane in X, Y or Z.
            //
            int i0, i1;
            {
                float n0 = Abs(normal[0]);
                float n1 = Abs(normal[1]);
                float n2 = Abs(normal[2]);

                i0 = 1; i1 = 2;
                if (n1 > n2)
                {
                    if (n1 > n0) i0 = 0;
                }
                else
                {
                    if (n2 > n0) i1 = 0;
                }
            }

            //
            // Check the intersection point is inside the triangle.
            //
            {
                float u1 = b[i0] - a[i0];
                float v1 = b[i1] - a[i1];
                float u2 = c[i0] - a[i0];
                float v2 = c[i1] - a[i1];
                float u0 = t * ray.Direction[i0] + ray.Origin[i0] - a[i0];
                float v0 = t * ray.Direction[i1] + ray.Origin[i1] - a[i1];

                float alpha = u0 * v2 - u2 * v0;
                float beta = u1 * v0 - u0 * v1;
                float area = u1 * v2 - u2 * v1;

                // epsilon to avoid float precision error
                const float EPSILON = 1e-3f;

                float tolerance = -EPSILON * area;

                if (area > 0)
                {
                    if (alpha < tolerance || beta < tolerance || alpha + beta > area - tolerance)
                        return Tuple.Create(false, 0.0f);
                }
                else
                {
                    if (alpha > tolerance || beta > tolerance || alpha + beta < area - tolerance)
                        return Tuple.Create(false, 0.0f);
                }
            }

            return Tuple.Create(true, t);
        }

        /// <summary>Ray / sphere intersection, returns boolean result and distance. </summary>
        public static Tuple<bool, float> Intersects(Ray ray, Sphere sphere)
        {
            return Intersects(ray, sphere, true);
        }

        public static Tuple<bool, float> Intersects(Ray ray, Sphere sphere, bool discardInside)
        {
            Vector3 raydir = ray.Direction;
            // Adjust ray origin relative to sphere center
            Vector3 rayorig = ray.Origin - sphere.Center;
            float radius = sphere.Radius;

            // Check origin inside first
            if (rayorig.SquaredLength <= radius * radius && discardInside)
            {
                return Tuple.Create(true, 0.0f);
            }

            // Mmm, quadratics
            // Build coeffs which can be used with std quadratic solver
            // ie t = (-b +/- sqrt(b*b + 4ac)) / 2a
            float a = raydir.DotProduct(raydir);
            float b = 2 * rayorig.DotProduct(raydir);
            float c = rayorig.DotProduct(rayorig) - radius * radius;

            // Calc determinant
            float d = (b * b) - (4 * a * c);
            if (d < 0)
            {
                // No intersection
                return Tuple.Create(false, 0.0f);
            }

            // BTW, if d=0 there is one intersection, if d > 0 there are 2
            // But we only want the closest one, so that's ok, just use the 
            // '-' version of the solver
            float t = (-b - Math.Sqrt(d)) / (2 * a);
            if (t < 0)
                t = (-b + Math.Sqrt(d)) / (2 * a);

            return Tuple.Create(true, t);
        }

        /// <summary>Ray / box intersection, returns boolean result and distance. </summary>
        public static Tuple<bool, float> Intersects(Ray ray, AxisAlignedBox box)
        {
            if (box.IsNull) return Tuple.Create(false, 0.0f);

            float lowt = 0.0f;
            float t;
            bool hit = false;
            Vector3 hitpoint;
            Vector3 min = box.Minimum;
            Vector3 max = box.Maximum;
            Vector3 rayorig = ray.Origin;
            Vector3 raydir = ray.Direction;

            // Check origin inside first
            if (rayorig > min && rayorig < max)
            {
                return Tuple.Create(true, 0.0f);
            }

            // Check each face in turn, only check closest 3
            // Min x
            if (rayorig.x < min.x && raydir.x > 0)
            {
                t = (min.x - rayorig.x) / raydir.x;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.y >= min.y && hitpoint.y <= max.y &&
                        hitpoint.z >= min.z && hitpoint.z <= max.z &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }
            // Max x
            if (rayorig.x > max.x && raydir.x < 0)
            {
                t = (max.x - rayorig.x) / raydir.x;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.y >= min.y && hitpoint.y <= max.y &&
                        hitpoint.z >= min.z && hitpoint.z <= max.z &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }
            // Min y
            if (rayorig.y < min.y && raydir.y > 0)
            {
                t = (min.y - rayorig.y) / raydir.y;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                        hitpoint.z >= min.z && hitpoint.z <= max.z &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }
            // Max y
            if (rayorig.y > max.y && raydir.y < 0)
            {
                t = (max.y - rayorig.y) / raydir.y;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                        hitpoint.z >= min.z && hitpoint.z <= max.z &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }
            // Min z
            if (rayorig.z < min.z && raydir.z > 0)
            {
                t = (min.z - rayorig.z) / raydir.z;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                        hitpoint.y >= min.y && hitpoint.y <= max.y &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }
            // Max z
            if (rayorig.z > max.z && raydir.z < 0)
            {
                t = (max.z - rayorig.z) / raydir.z;
                if (t > 0)
                {
                    // Substitute t back into ray and check bounds and dist
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                        hitpoint.y >= min.y && hitpoint.y <= max.y &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }

            return Tuple.Create(hit, lowt);
        }

        /// <summary>Ray / plane intersection, returns boolean result and distance. </summary>
        public static Tuple<bool, float> Intersects(Ray ray, Plane plane)
        {
            float denom = plane.normal.DotProduct(ray.Direction);
            if (Abs(denom) < float.Epsilon)
            {
                // Parallel
                return Tuple.Create(false, 0.0f);
            }
            else
            {
                float nom = plane.normal.DotProduct(ray.Origin) + plane.d;
                float t = -(nom / denom);
                return Tuple.Create(t >= 0, t);
            }
        }
    }
}
