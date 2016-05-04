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
#include "OgreRay.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"
#include "Custom\MogreMath.h"
#include "Custom\MogreVector3.h"
#include "Custom\MogrePlane.h"
#include "MogreSphere.h"
#include "MogrePlaneBoundedVolume.h"

namespace Mogre
{
    /** <summary>Representation of a ray in space, i.e. a line with an origin and direction.</summary> */
    [Serializable]
    public value class Ray
    {
    protected:
        Vector3 mOrigin;
        Vector3 mDirection;
    public:
        inline static operator Ogre::Ray& (Ray& obj)
        {
            return reinterpret_cast<Ogre::Ray&>(obj);
        }
        inline static operator const Ray& ( const Ogre::Ray& obj)
        {
            return reinterpret_cast<const Ray&>(obj);
        }
        inline static operator const Ray& ( const Ogre::Ray* pobj)
        {
            return reinterpret_cast<const Ray&>(*pobj);
        }

        Ray(Vector3 origin, Vector3 direction)
            :mOrigin(origin), mDirection(direction) {}

        /** <summary>Gets or Sets the origin of the ray.</summary> */
        property Vector3 Origin
        {
            void set(Vector3 origin) {mOrigin = origin;} 
            Vector3 get() {return mOrigin;} 
        }

        /** <summary>Gets or Sets the direction of the ray.</summary> */
        property Vector3 Direction
        {
            void set(Vector3 dir) {mDirection = dir;} 
            Vector3 get() {return mDirection;} 
        }

        /** <summary>Gets the position of a point <paramref name="t"/> units along the ray.</summary> */
        Vector3 GetPoint(Real t) { 
            return Vector3(mOrigin + (mDirection * t));
        }

        /** <summary>Gets the position of a point t units along the ray.</summary> */
        static Vector3 operator*(Ray r, Real t) { 
            return r.GetPoint(t);
        }

        /** <summary>Tests whether this ray intersects the given plane.</summary>
        <returns>A pair structure where the first element indicates whether
        an intersection occurs, and if true, the second element will
        indicate the distance along the ray at which it intersects. 
        This can be converted to a point in space by calling getPoint().
        </returns>
        */
        Pair<bool, Real> Intersects(Plane p)
        {
            return Math::Intersects(*this, p);
        }
        /** <summary>Tests whether this ray intersects the given plane bounded volume.</summary>
        <returns>A pair structure where the first element indicates whether
        an intersection occurs, and if true, the second element will
        indicate the distance along the ray at which it intersects. 
        This can be converted to a point in space by calling getPoint().
        </returns>
        */
        Pair<bool, Real> Intersects(PlaneBoundedVolume^ p)
        {
            return Math::Intersects(*this, p->planes, p->outside == Plane::Side::POSITIVE_SIDE);
        }
        /** <summary>Tests whether this ray intersects the given sphere.</summary>
        <returns> A pair structure where the first element indicates whether
        an intersection occurs, and if true, the second element will
        indicate the distance along the ray at which it intersects. 
        This can be converted to a point in space by calling getPoint().
        </returns>
        */
        Pair<bool, Real> Intersects(Sphere s)
        {
            return Math::Intersects(*this, s);
        }
        /** <summary>Tests whether this ray intersects the given box.</summary>
        <returns>A pair structure where the first element indicates whether
        an intersection occurs, and if true, the second element will
        indicate the distance along the ray at which it intersects. 
        This can be converted to a point in space by calling getPoint().
        </returns>
        */
        Pair<bool, Real> Intersects(AxisAlignedBox^ box)
        {
            return Math::Intersects(*this, box);
        }

    };
}