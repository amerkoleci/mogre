/*
-----------------------------------------------------------------------------
This source file is part of OGRE
    (Object-oriented Graphics Rendering Engine) ported to C++/CLI
For the latest info, see http://www.ogre3d.org/

Copyright (c) 2000-2012 Torus Knot Software Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/
#pragma once

#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgreVector2.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Custom\MogreMath.h"

namespace Mogre
{
    /** <summary>Standard 2-dimensional vector.</summary>
    <remarks>
    A direction in 2D space represented as distances along the 2
    orthogonal axes (x, y). Note that positions, directions and
    scaling factors can be represented by a vector, depending on how
    you interpret the values.
    </remarks>
    */
    [Serializable]
    public value class Vector2 : IEquatable<Vector2>
    {
    public:
        inline static operator Ogre::Vector2& (Vector2& obj)
        {
            return reinterpret_cast<Ogre::Vector2&>(obj);
        }
        inline static operator const Vector2& ( const Ogre::Vector2& obj)
        {
            return reinterpret_cast<const Vector2&>(obj);
        }
        inline static operator const Vector2& ( const Ogre::Vector2* pobj)
        {
            return reinterpret_cast<const Vector2&>(*pobj);
        }

        Real x, y;

    public:
        inline Vector2(Real fX, Real fY )
            : x( fX ), y( fY )
        {
        }

        inline explicit Vector2( Real scaler )
            : x( scaler), y( scaler )
        {
        }

        inline explicit Vector2( array<Real>^ afCoordinate )
            : x( afCoordinate[0] ),
              y( afCoordinate[1] )
        {
        }

        inline explicit Vector2( array<int>^ afCoordinate )
        {
            x = (Real)afCoordinate[0];
            y = (Real)afCoordinate[1];
        }

        inline explicit Vector2( Real* const r )
            : x( r[0] ), y( r[1] )
        {
        }

        property Real default[int]
        {
            inline Real get(int i)
            {
                assert( i < 2 );

                return *(&x+i);
            }

            inline void set(int i, Real value)
            {
                assert( i < 2 );

                *(&x+i) = value;
            }
        }

        inline static bool operator == ( Vector2 lvec, Vector2 rkVector )
        {
            return ( lvec.x == rkVector.x && lvec.y == rkVector.y );
        }

        inline static bool operator != ( Vector2 lvec, Vector2 rkVector )
        {
            return ( lvec.x != rkVector.x || lvec.y != rkVector.y  );
        }

        virtual bool Equals(Vector2 other) { return *this == other; }

        // arithmetic operations
        inline static Vector2 operator + ( Vector2 lvec, Vector2 rkVector )
        {
            return Vector2(
                lvec.x + rkVector.x,
                lvec.y + rkVector.y);
        }

        inline static Vector2 operator - ( Vector2 lvec, Vector2 rkVector )
        {
            return Vector2(
                lvec.x - rkVector.x,
                lvec.y - rkVector.y);
        }

        inline static Vector2 operator * ( Vector2 lvec, Real fScalar )
        {
            return Vector2(
                lvec.x * fScalar,
                lvec.y * fScalar);
        }

        inline static Vector2 operator * ( Vector2 lhs, Vector2 rhs)
        {
            return Vector2(
                lhs.x * rhs.x,
                lhs.y * rhs.y);
        }

        inline static Vector2 operator / ( Vector2 lvec, Real fScalar )
        {
            assert( fScalar != 0.0 );

            Real fInv = 1.0f / fScalar;

            return Vector2(
                lvec.x * fInv,
                lvec.y * fInv);
        }

        inline static Vector2 operator / ( Vector2 lhs, Vector2 rhs)
        {
            return Vector2(
                lhs.x / rhs.x,
                lhs.y / rhs.y);
        }

        inline static Vector2 operator - ( Vector2 vec )
        {
            return Vector2(-vec.x, -vec.y);
        }

        inline static Vector2 operator * ( Real fScalar, Vector2 rkVector )
        {
            return Vector2(
                fScalar * rkVector.x,
                fScalar * rkVector.y);
        }

        inline static Vector2 operator / ( Real fScalar, Vector2 rkVector )
        {
            return Vector2(
                fScalar / rkVector.x,
                fScalar / rkVector.y);
        }

        inline static Vector2 operator + (Vector2 lhs, Real rhs)
        {
            return Vector2(
                lhs.x + rhs,
                lhs.y + rhs);
        }

        inline static Vector2 operator + (Real lhs, Vector2 rhs)
        {
            return Vector2(
                lhs + rhs.x,
                lhs + rhs.y);
        }

        inline static Vector2 operator - (Vector2 lhs, Real rhs)
        {
            return Vector2(
                lhs.x - rhs,
                lhs.y - rhs);
        }

        inline static Vector2 operator - (Real lhs, Vector2 rhs)
        {
            return Vector2(
                lhs - rhs.x,
                lhs - rhs.y);
        }

        /** <summary>Returns the length (magnitude) of the vector.</summary>
        <remarks>
        <note type="warning">
        This operation requires a square root and is expensive in
        terms of CPU operations. If you don't need to know the exact
        length (e.g. for just comparing lengths) use squaredLength()
        instead.
        </note>
        </remarks>
        */
        property Real Length
        {
            inline Real get()
            {
                return System::Math::Sqrt( x * x + y * y );
            }
        }

        /** <summary>Returns the square of the length(magnitude) of the vector.</summary>
        <remarks>
        This  method is for efficiency - calculating the actual
        length of a vector requires a square root, which is expensive
        in terms of the operations required. This method returns the
        square of the length of the vector, i.e. the same as the
        length but before the square root is taken. Use this if you
        want to find the longest / shortest vector without incurring
        the square root.
        </remarks>
        */
        property Real SquaredLength
        {
            inline Real get()
            {
                return x * x + y * y;
            }
        }
        /** <summary>Returns the distance to another vector.</summary>
        <remarks>
        <note type="warning">
        This operation requires a square root and is expensive in
        terms of CPU operations. If you don't need to know the exact
        distance (e.g. for just comparing distances) use squaredDistance()
        instead.
        </note>
        </remarks>
        */
        inline Real Distance(Vector2 rhs)
        {
            return (*this - rhs).Length;
        }

        /** <summary>Returns the square of the distance to another vector.</summary>
        <remarks>
        This method is for efficiency - calculating the actual
        distance to another vector requires a square root, which is
        expensive in terms of the operations required. This method
        returns the square of the distance to another vector, i.e.
        the same as the distance but before the square root is taken.
        Use this if you want to find the longest / shortest distance
        without incurring the square root.
        </remarks>
        */
        inline Real SquaredDistance(Vector2 rhs)
        {
            return (*this - rhs).SquaredLength;
        }

        /** <summary>Calculates the dot (scalar) product of this vector with another.</summary>
        <remarks>
        The dot product can be used to calculate the angle between 2
        vectors. If both are unit vectors, the dot product is the
        cosine of the angle; otherwise the dot product must be
        divided by the product of the lengths of both vectors to get
        the cosine of the angle. This result can further be used to
        calculate the distance of a point from a plane.
        </remarks>
        <param name="vec">Vector with which to calculate the dot product (together
        with this one).</param>
        <returns>A float representing the dot product value.</returns>
        */
        inline Real DotProduct(Vector2 vec)
        {
            return x * vec.x + y * vec.y;
        }

        /** <summary>Normalises the vector.</summary>
        <remarks>
        This method normalises the vector such that it's
        length / magnitude is 1. The result is called a unit vector.
        <note>
        This function will not crash for zero-sized vectors, but there
        will be no changes made to their components.
        </note>
        </remarks>
        <returns>The previous length of the vector.</returns>
        */
        inline Real Normalise()
        {
            Real fLength = System::Math::Sqrt( x * x + y * y);

            // Will also work for zero-sized vectors, but will change nothing
            // We're not using epsilons because we don't need to.
            // Read http://www.ogre3d.org/forums/viewtopic.php?f=4&t=61259
            if ( fLength > Real(0.0f) )
            {
                Real fInvLength = 1.0f / fLength;
                x *= fInvLength;
                y *= fInvLength;
            }

            return fLength;
        }

        /** <summary>Returns a vector at a point half way between this and the passed
        in vector.</summary>
        */
        inline Vector2 MidPoint( Vector2 vec )
        {
            return Vector2(
                ( x + vec.x ) * 0.5f,
                ( y + vec.y ) * 0.5f );
        }

        /** <summary>Returns true if the vector's scalar components are all greater
        that the ones of the vector it is compared against.</summary>
        */
        inline static bool operator < ( Vector2 lhs, Vector2 rhs )
        {
            if( lhs.x < rhs.x && lhs.y < rhs.y )
                return true;
            return false;
        }

        /** <summary>Returns true if the vector's scalar components are all smaller
        that the ones of the vector it is compared against.</summary>
        */
        inline static bool operator > ( Vector2 lhs, Vector2 rhs )
        {
            if( lhs.x > rhs.x && lhs.y > rhs.y )
                return true;
            return false;
        }

        /** <summary>Sets this vector's components to the minimum of its own and the
        ones of the passed in vector.</summary>
        <remarks>
        'Minimum' in this case means the combination of the lowest
        value of x, y and z from both vectors. Lowest is taken just
        numerically, not magnitude, so -1 &lt; 0.
        </remarks>
        */
        inline void MakeFloor( Vector2 cmp )
        {
            if( cmp.x < x ) x = cmp.x;
            if( cmp.y < y ) y = cmp.y;
        }

        /** <summary>Sets this vector's components to the maximum of its own and the
        ones of the passed in vector.</summary>
        <remarks>
        'Maximum' in this case means the combination of the highest
        value of x, y and z from both vectors. Highest is taken just
        numerically, not magnitude, so 1 &gt; -3.
        </remarks>
        */
        inline void MakeCeil( Vector2 cmp )
        {
            if( cmp.x > x ) x = cmp.x;
            if( cmp.y > y ) y = cmp.y;
        }

        /** <summary>Generates a vector perpendicular to this vector (eg an 'up' vector).</summary>
        <remarks>
        This method will return a vector which is perpendicular to this
        vector. There are an infinite number of possibilities but this
        method will guarantee to generate one of them. If you need more
        control you should use the Quaternion class.
        </remarks>
        */
        property Vector2 Perpendicular
        {
            inline Vector2 get()
            {
                return Vector2 (-y, x);
            }
        }
        /** <summary>Calculates the 2 dimensional cross-product of 2 vectors, which results
        in a single floating point value which is 2 times the area of the triangle.</summary>
        */
        inline Real CrossProduct( Vector2 rkVector )
        {
            return x * rkVector.y - y * rkVector.x;
        }

        /** <summary>Generates a new random vector which deviates from this vector by a
        given angle in a random direction.</summary>
        <remarks>
        This method assumes that the random number generator has already
        been seeded appropriately.
        </remarks>
        <param name="angle">The angle at which to deviate in radians</param>
        <returns>
        A random vector which deviates from this vector by angle. This
        vector will not be normalised, normalise it if you wish
        afterwards.
        </returns>
        */
        inline Vector2 RandomDeviant(Real angle)
        {

            angle *=  Math::UnitRandom() * Math::TWO_PI;
            Real cosa = System::Math::Cos(angle);
            Real sina = System::Math::Sin(angle);
            return  Vector2(cosa * x - sina * y,
                sina * x + cosa * y);
        }

        /** <summary>Returns true if this vector is zero length.</summary> */
        property bool IsZeroLength
        {
            inline bool get()
            {
                Real sqlen = (x * x) + (y * y);
                return (sqlen < (1e-06 * 1e-06));

            }
        }

        /** <summary>As normalise, except that this vector is unaffected and the
        normalised vector is returned as a copy.</summary> */
        property Vector2 NormalisedCopy
        {
            inline Vector2 get()
            {
                Vector2 ret = *this;
                ret.Normalise();
                return ret;
            }
        }

        /** <summary>Calculates a reflection vector to the plane with the given normal .</summary>
        <remarks> NB assumes 'this' is pointing AWAY FROM the plane, invert if it is not.</remarks>
        */
        inline Vector2 Reflect(Vector2 normal)
        {
            return Vector2( *this - ( 2 * this->DotProduct(normal) * normal ) );
        }

        /// <summary>Check whether this vector contains valid values</summary>
        property bool IsNaN
        {
            inline bool get()
            {
                return Real::IsNaN(x) || Real::IsNaN(y);
            }
        }

        /** <summary>Gets the angle between 2 vectors.</summary>
        <remarks>
        Vectors do not have to be unit-length but must represent directions.
        </remarks>
        */
        inline Radian AngleBetween(Vector2 other)
        {
            Real lenProduct = Length * other.Length;
            // Divide by zero check
            if(lenProduct < 1e-6f)
                lenProduct = 1e-6f;

            Real f = DotProduct(other) / lenProduct;

            f = Math::Clamp(f, (Real)-1.0, (Real)1.0);
            return Math::ACos(f);
        }

        /** <summary>Gets the oriented angle between 2 vectors.</summary>
        <remarks>
        Vectors do not have to be unit-length but must represent directions.
        The angle is comprised between 0 and 2 PI.
        </remarks>
        */
        inline Radian AngleTo(Vector2 other)
        {
            Radian angle = AngleBetween(other);

            if (CrossProduct(other)<0)
                angle = (Radian)Math::TWO_PI - angle;

            return angle;
        }

        // special points
        static initonly Vector2 ZERO = Vector2( 0, 0);
        static initonly Vector2 UNIT_X = Vector2( 1, 0);
        static initonly Vector2 UNIT_Y = Vector2( 0, 1);
        static initonly Vector2 NEGATIVE_UNIT_X = Vector2( -1,  0);
        static initonly Vector2 NEGATIVE_UNIT_Y = Vector2(  0, -1);
        static initonly Vector2 UNIT_SCALE = Vector2(1, 1);

        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("Vector2({0}, {1})", x, y);
        }
    };
}