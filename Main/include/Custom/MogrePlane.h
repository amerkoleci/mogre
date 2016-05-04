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
// This file is based on material originally from:
// Geometric Tools, LLC
// Copyright (c) 1998-2010
// Distributed under the Boost Software License, Version 1.0.
// http://www.boost.org/LICENSE_1_0.txt
// http://www.geometrictools.com/License/Boost/LICENSE_1_0.txt

#pragma once

#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgrePlane.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Custom\MogreVector3.h"

namespace Mogre
{
    /** <summary>Defines a plane in 3D space.</summary>
    <remarks>
    A plane is defined in 3D space by the equation
    Ax + By + Cz + D = 0
    <para/>
    This equates to a vector (the normal of the plane, whose x, y
    and z components equate to the coefficients A, B and C
    respectively), and a constant (D) which is the distance along
    the normal you have to go to move the plane back to the origin.
    </remarks>
    */
    [Serializable]
    public value class Plane : IEquatable<Plane>
    {
    public:
        inline static operator Ogre::Plane& (Plane& obj)
        {
            return reinterpret_cast<Ogre::Plane&>(obj);
        }
        inline static operator const Plane& ( const Ogre::Plane& obj)
        {
            return reinterpret_cast<const Plane&>(obj);
        }
        inline static operator const Plane& ( const Ogre::Plane* pobj)
        {
            return reinterpret_cast<const Plane&>(*pobj);
        }

        /** <summary>Construct a plane through a normal, and a distance to move the plane along the normal.</summary>*/
        Plane (Vector3 rkNormal, Real fConstant);
        /** <summary>Construct a plane using the 4 constants directly</summary> **/
        Plane (Real a, Real b, Real c, Real d);
        Plane (Vector3 rkNormal, Vector3 rkPoint);
        Plane (Vector3 rkPoint0, Vector3 rkPoint1,
            Vector3 rkPoint2);

        /** <summary>The "positive side" of the plane is the half space to which the
        plane normal points. The "negative side" is the other half
        space. The flag "no side" indicates the plane itself.</summary>
        */
        enum class Side
        {
            NO_SIDE = Ogre::Plane::NO_SIDE,
            POSITIVE_SIDE = Ogre::Plane::POSITIVE_SIDE,
            NEGATIVE_SIDE = Ogre::Plane::NEGATIVE_SIDE,
            BOTH_SIDE = Ogre::Plane::BOTH_SIDE
        };

        Side GetSide (Vector3 rkPoint);

        /**
        <summary>Returns the side where the alignedBox is. The flag BOTH_SIDE indicates an intersecting box.
        One corner ON the plane is sufficient to consider the box and the plane intersecting.</summary>
        */
        Side GetSide (AxisAlignedBox^ rkBox);

        /** <summary>Returns which side of the plane that the given box lies on.
        The box is defined as centre/half-size pairs for effectively.</summary>
        <param name="centre">The centre of the box.</param>
        <param name="halfSize">The half-size of the box.</param>
        <returns>
        POSITIVE_SIDE if the box complete lies on the "positive side" of the plane,
        NEGATIVE_SIDE if the box complete lies on the "negative side" of the plane,
        and BOTH_SIDE if the box intersects the plane.
        </returns>
        */
        Side GetSide (Vector3 centre, Vector3 halfSize);

        /** <summary>This is a pseudodistance. The sign of the return value is
        positive if the point is on the positive side of the plane,
        negative if the point is on the negative side, and zero if the
        point is on the plane.</summary>
        <remarks>
        The absolute value of the return value is the true distance only
        when the plane normal is a unit length vector.
        </remarks>
        */
        Real GetDistance (Vector3 rkPoint);

        /** <summary>Redefine this plane based on 3 points.</summary> */
        void Redefine(Vector3 rkPoint0, Vector3 rkPoint1,
            Vector3 rkPoint2);

        /** <summary>Redefine this plane based on a normal and a point.</summary> */
        void Redefine(Vector3 rkNormal, Vector3 rkPoint);

        /** <summary>Project a vector onto the plane.</summary> 
        <remarks>This gives you the element of the input vector that is perpendicular 
        to the normal of the plane. You can get the element which is parallel
        to the normal of the plane by subtracting the result of this method
        from the original vector, since parallel + perpendicular = original.</remarks>
        <param name="v">The input vector</param>
        */
        Vector3 ProjectVector(Vector3 v);

        /** <summary>Normalises the plane.</summary>
        <remarks>
        This method normalises the plane's normal and the length scale of d
        is as well.
        <note>
        This function will not crash for zero-sized vectors, but there
        will be no changes made to their components.
        </note>
        </remarks>
        <returns>The previous length of the plane's normal.</returns>
        */
        Real Normalise();

        Vector3 normal;
        Real d;
        // Comparison operator
        static bool operator==(Plane lhs, Plane rhs)
        {
            return (rhs.d == lhs.d && rhs.normal == lhs.normal);
        }

        static bool operator!=(Plane lhs, Plane rhs)
        {
            return (rhs.d != lhs.d || rhs.normal != lhs.normal);
        }

        virtual bool Equals(Plane other) { return *this == other; }

        virtual String^ ToString() override;
    };
}