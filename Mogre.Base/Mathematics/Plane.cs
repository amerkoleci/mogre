// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a plane in 3D space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Plane : IEquatable<Plane>
    {
        /// <summary>
        /// The "positive side" of the plane is the half space to which the
        /// plane normal points.The "negative side" is the other half
        /// space.The flag "no side" indicates the plane itself.
        /// </summary>
        public enum Side
        {
            NO_SIDE,
            POSITIVE_SIDE,
            NEGATIVE_SIDE,
            BOTH_SIDE
        }

        /// <summary>
        /// The normal of the plane.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// The distance of the plane.
        /// </summary>
        public float D;

        public Plane(Vector3 normal, float constant)
        {
            Normal = normal;
            D = -constant;
        }

        public Plane(float normalX, float normalY, float normalZ, float d)
        {
            Normal = new Vector3(normalX, normalY, normalZ);
            D = d;
        }

        public Plane(Vector3 normal, Vector3 point)
        {
            Normal = normal;
            D = -normal.Dot(point);
        }

        public Plane(Vector3 point0, Vector3 point1, Vector3 point2) : this()
        {
            Redefine(point0, point1, point2);
        }

        /// <summary>
        /// Redefine this plane based on 3 points.
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public void Redefine(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            Vector3 kEdge1 = point1 - point0;
            Vector3 kEdge2 = point2 - point0;
            Normal = kEdge1.Cross(kEdge2);
            Normal.Normalize();
            D = -Normal.Dot(point0);
        }

        public float GetDistance(Vector3 point)
        {
            return Normal.Dot(point) + D;
        }

        public Side GetSide(Vector3 point)
        {
            var fDistance = GetDistance(point);

            if (fDistance < 0.0f)
                return Side.NEGATIVE_SIDE;

            if (fDistance > 0.0f)
                return Side.POSITIVE_SIDE;

            return Side.NO_SIDE;
        }

        public Side GetSide(AxisAlignedBox box)
        {
            if (box.IsNull)
                return Side.NO_SIDE;

            if (box.IsInfinite)
                return Side.BOTH_SIDE;

            return GetSide(box.Center, box.HalfSize);
        }

        public Side GetSide(Vector3 centre, Vector3 halfSize)
        {
            // Calculate the distance between box centre and the plane
            var dist = GetDistance(centre);

            // Calculate the maximise allows absolute distance for
            // the distance between box centre and plane
            var maxAbsDist = Normal.AbsDotProduct(halfSize);

            if (dist < -maxAbsDist)
                return Side.NEGATIVE_SIDE;

            if (dist > +maxAbsDist)
                return Side.POSITIVE_SIDE;

            return Side.BOTH_SIDE;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Plane left, Plane right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Plane left, Plane right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ D.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Plane"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Plane"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Plane"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Plane other)
        {
            return
                Normal == other.Normal &&
                D == other.D;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Plane"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Plane"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Plane"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Plane other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (!(value is Plane))
                return false;

            var strongValue = (Plane)value;
            return Equals(ref strongValue);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Normal:{0} D:{1}", Normal, D);
        }
    }
}
