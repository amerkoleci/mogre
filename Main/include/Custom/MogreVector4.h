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
#include "OgreVector4.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Custom\MogreVector3.h"

namespace Mogre
{
    /** <summary>4-dimensional homogeneous vector.</summary>
    */
    [Serializable]
    public value class Vector4 : IEquatable<Vector4>
    {
    public:
        inline static operator Ogre::Vector4& (Vector4& obj)
        {
            return reinterpret_cast<Ogre::Vector4&>(obj);
        }
        inline static operator const Vector4& ( const Ogre::Vector4& obj)
        {
            return reinterpret_cast<const Vector4&>(obj);
        }
        inline static operator const Vector4& ( const Ogre::Vector4* pobj)
        {
            return reinterpret_cast<const Vector4&>(*pobj);
        }

        Real x, y, z, w;

        inline Vector4( Real fX, Real fY, Real fZ, Real fW )
            : x( fX ), y( fY ), z( fZ ), w( fW)
        {
        }

        inline explicit Vector4( array<Real>^ afCoordinate )
            : x( afCoordinate[0] ),
              y( afCoordinate[1] ),
              z( afCoordinate[2] ),
              w( afCoordinate[3] )
        {
        }

        inline explicit Vector4( array<int>^ afCoordinate )
        {
            x = (Real)afCoordinate[0];
            y = (Real)afCoordinate[1];
            z = (Real)afCoordinate[2];
            w = (Real)afCoordinate[3];
        }

        inline explicit Vector4( Real* const r )
            : x( r[0] ), y( r[1] ), z( r[2] ), w( r[3] )
        {
        }

        inline explicit Vector4( Real scaler )
            : x( scaler )
            , y( scaler )
            , z( scaler )
            , w( scaler )
        {
        }

        property Real default[int]
        {
            inline Real get(int i)
            {
                assert( i < 4 );

                return *(&x+i);
            }

            inline void set(int i, Real value)
            {
                assert( i < 4 );

                *(&x+i) = value;
            }
        }

        inline static bool operator == ( Vector4 lvec, Vector4 rkVector )
        {
            return ( lvec.x == rkVector.x &&
                lvec.y == rkVector.y &&
                lvec.z == rkVector.z &&
                lvec.w == rkVector.w );
        }

        inline static bool operator != ( Vector4 lvec, Vector4 rkVector )
        {
            return ( lvec.x != rkVector.x ||
                lvec.y != rkVector.y ||
                lvec.z != rkVector.z ||
                lvec.w != rkVector.w );
        }

        virtual bool Equals(Vector4 other) { return *this == other; }

        inline static operator Vector4 (Vector3 rhs)
        {
            Vector4 vec;

            vec.x = rhs.x;
            vec.y = rhs.y;
            vec.z = rhs.z;
            vec.w = 1.0f;
            return vec;
        }

        // arithmetic operations
        inline static Vector4 operator + ( Vector4 lvec, Vector4 rkVector )
        {
            return Vector4(
                lvec.x + rkVector.x,
                lvec.y + rkVector.y,
                lvec.z + rkVector.z,
                lvec.w + rkVector.w);
        }

        inline static Vector4 operator - ( Vector4 lvec, Vector4 rkVector )
        {
            return Vector4(
                lvec.x - rkVector.x,
                lvec.y - rkVector.y,
                lvec.z - rkVector.z,
                lvec.w - rkVector.w);
        }

        inline static Vector4 operator * ( Vector4 lvec, Real fScalar )
        {
            return Vector4(
                lvec.x * fScalar,
                lvec.y * fScalar,
                lvec.z * fScalar,
                lvec.w * fScalar);
        }

        inline static Vector4 operator * ( Vector4 lvec, Vector4 rhs)
        {
            return Vector4(
                rhs.x * lvec.x,
                rhs.y * lvec.y,
                rhs.z * lvec.z,
                rhs.w * lvec.w);
        }

        inline static Vector4 operator / ( Vector4 lvec, Real fScalar )
        {
            assert( fScalar != 0.0 );

            Real fInv = 1.0f / fScalar;

            return Vector4(
                lvec.x * fInv,
                lvec.y * fInv,
                lvec.z * fInv,
                lvec.w * fInv);
        }

        inline static Vector4 operator / ( Vector4 lvec, Vector4 rhs)
        {
            return Vector4(
                lvec.x / rhs.x,
                lvec.y / rhs.y,
                lvec.z / rhs.z,
                lvec.w / rhs.w);
        }

        inline static Vector4 operator - (Vector4 vec)
        {
            return Vector4(-vec.x, -vec.y, -vec.z, -vec.w);
        }

        inline static Vector4 operator * ( Real fScalar, Vector4 rkVector )
        {
            return Vector4(
                fScalar * rkVector.x,
                fScalar * rkVector.y,
                fScalar * rkVector.z,
                fScalar * rkVector.w);
        }

        inline static Vector4 operator / ( Real fScalar, Vector4 rkVector )
        {
            return Vector4(
                fScalar / rkVector.x,
                fScalar / rkVector.y,
                fScalar / rkVector.z,
                fScalar / rkVector.w);
        }

        inline static Vector4 operator + (Vector4 lhs, Real rhs)
        {
            return Vector4(
                lhs.x + rhs,
                lhs.y + rhs,
                lhs.z + rhs,
                lhs.w + rhs);
        }

        inline static Vector4 operator + (Real lhs, Vector4 rhs)
        {
            return Vector4(
                lhs + rhs.x,
                lhs + rhs.y,
                lhs + rhs.z,
                lhs + rhs.w);
        }

        inline static Vector4 operator - (Vector4 lhs, Real rhs)
        {
            return Vector4(
                lhs.x - rhs,
                lhs.y - rhs,
                lhs.z - rhs,
                lhs.w - rhs);
        }

        inline static Vector4 operator - (Real lhs, Vector4 rhs)
        {
            return Vector4(
                lhs - rhs.x,
                lhs - rhs.y,
                lhs - rhs.z,
                lhs - rhs.w);
        }

        /** <summary>Calculates the dot (scalar) product of this vector with another.</summary>
        <param name="vec">Vector with which to calculate the dot product (together
        with this one).</param>
        <returns>A float representing the dot product value.</returns>
        */
        inline Real DotProduct(Vector4 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z + w * vec.w;
        }
        /// <summary>Check whether this vector contains valid values</summary>
        property bool IsNaN
        {
            inline bool get()
            {
                return Real::IsNaN(x) || Real::IsNaN(y) || Real::IsNaN(z) || Real::IsNaN(w);
            }
        }
        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("Vector4({0}, {1}, {2}, {3})", x, y, z, w);
        }
        // special
        static initonly Vector4 ZERO = Vector4(0, 0, 0, 0);
    };
}