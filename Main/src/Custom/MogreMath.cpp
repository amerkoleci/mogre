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
#include "MogreStableHeaders.h"

#include "Custom\MogreMath.h"
#include "Custom\MogreVector2.h"
#include "Custom\MogreVector3.h"
#include "Custom\MogrePlane.h"
#include "Custom\MogreRay.h"
#include "Custom\MogreAxisAlignedBox.h"

namespace Mogre
{
    //-----------------------------------------------------------------------
    Math::Math()
    {
        msAngleUnit = AngleUnit::AU_DEGREE;

        mTrigTableSize = 4096;
        mTrigTableFactor = mTrigTableSize / Math::TWO_PI;

        mSinTable = gcnew array<Real>(mTrigTableSize);
        mTanTable = gcnew array<Real>(mTrigTableSize);

        buildTrigTables();
    }
    //-----------------------------------------------------------------------
    void Math::TrigTableSize::set( int trigTableSize )
    {
        mTrigTableSize = trigTableSize;
        mTrigTableFactor = mTrigTableSize / Math::TWO_PI;

        mSinTable = gcnew array<Real>(mTrigTableSize);
        mTanTable = gcnew array<Real>(mTrigTableSize);

        buildTrigTables();
    }
    //-----------------------------------------------------------------------
    void Math::buildTrigTables(void)
    {
        // Build trig lookup tables
        // Could get away with building only PI sized Sin table but simpler this 
        // way. Who cares, it'll ony use an extra 8k of memory anyway and I like 
        // simplicity.
        Real angle;
        for (int i = 0; i < mTrigTableSize; ++i)
        {
            angle = Math::TWO_PI * i / mTrigTableSize;
            mSinTable[i] = sin(angle);
            mTanTable[i] = tan(angle);
        }
    }
    //-----------------------------------------------------------------------	
    Real Math::SinTable (Real fValue)
    {
        // Convert range to index values, wrap if required
        int idx;
        if (fValue >= 0)
        {
            idx = int(fValue * mTrigTableFactor) % mTrigTableSize;
        }
        else
        {
            idx = mTrigTableSize - (int(-fValue * mTrigTableFactor) % mTrigTableSize) - 1;
        }

        return mSinTable[idx];
    }
    //-----------------------------------------------------------------------
    Real Math::TanTable (Real fValue)
    {
        // Convert range to index values, wrap if required
        int idx = int(fValue *= mTrigTableFactor) % mTrigTableSize;
        return mTanTable[idx];
    }
    //-----------------------------------------------------------------------
    int Math::ISign (int iValue)
    {
        return ( iValue > 0 ? +1 : ( iValue < 0 ? -1 : 0 ) );
    }
    //-----------------------------------------------------------------------
    Radian Math::ACos (Real fValue)
    {
        if ( -1.0 < fValue )
        {
            if ( fValue < 1.0 )
                return Radian(System::Math::Acos(fValue));
            else
                return Radian(0.0);
        }
        else
        {
            return Radian(PI);
        }
    }
    //-----------------------------------------------------------------------
    Radian Math::ASin (Real fValue)
    {
        if ( -1.0 < fValue )
        {
            if ( fValue < 1.0 )
                return Radian(System::Math::Asin(fValue));
            else
                return Radian(HALF_PI);
        }
        else
        {
            return Radian(-HALF_PI);
        }
    }
    //-----------------------------------------------------------------------
    Real Math::Sign (Real fValue)
    {
        if ( fValue > 0.0 )
            return 1.0;

        if ( fValue < 0.0 )
            return -1.0;

        return 0.0;
    }
    //-----------------------------------------------------------------------
    Real Math::InvSqrt(Real fValue)
    {
        return (Real)(1 / System::Math::Sqrt(fValue));
    }

    //-----------------------------------------------------------------------
    Real Math::UnitRandom ()
    {
        return (Real)mRandomizer->NextDouble();
    }

    //-----------------------------------------------------------------------
    Real Math::RangeRandom (Real fLow, Real fHigh)
    {
        return (fHigh-fLow)*UnitRandom() + fLow;
    }

    //-----------------------------------------------------------------------
    Real Math::SymmetricRandom ()
    {
        return 2.0f * UnitRandom() - 1.0f;
    }

    //-----------------------------------------------------------------------
    void Math::SetAngleUnit(Math::AngleUnit unit)
    {
        msAngleUnit = unit;
    }
    //-----------------------------------------------------------------------
    Math::AngleUnit Math::GetAngleUnit(void)
    {
        return msAngleUnit;
    }
    //-----------------------------------------------------------------------
    Real Math::AngleUnitsToRadians(Real angleunits)
    {
        if (msAngleUnit == AngleUnit::AU_DEGREE)
            return angleunits * fDeg2Rad;
        else
            return angleunits;
    }

    //-----------------------------------------------------------------------
    Real Math::RadiansToAngleUnits(Real radians)
    {
        if (msAngleUnit == AngleUnit::AU_DEGREE)
            return radians * fRad2Deg;
        else
            return radians;
    }

    //-----------------------------------------------------------------------
    Real Math::AngleUnitsToDegrees(Real angleunits)
    {
        if (msAngleUnit == AngleUnit::AU_RADIAN)
            return angleunits * fRad2Deg;
        else
            return angleunits;
    }

    //-----------------------------------------------------------------------
    Real Math::DegreesToAngleUnits(Real degrees)
    {
        if (msAngleUnit == AngleUnit::AU_RADIAN)
            return degrees * fDeg2Rad;
        else
            return degrees;
    }

    //-----------------------------------------------------------------------
    bool Math::PointInTri2D(Vector2 p, Vector2 a, 
        Vector2 b, Vector2 c)
    {
        // Winding must be consistent from all edges for point to be inside
        Vector2 v1, v2;
        Real dot[3];
        bool zeroDot[3];

        v1 = b - a;
        v2 = p - a;

        // Note we don't care about normalisation here since sign is all we need
        // It means we don't have to worry about magnitude of cross products either
        dot[0] = v1.CrossProduct(v2);
        zeroDot[0] = Math::RealEqual(dot[0], 0.0f, 1e-3);


        v1 = c - b;
        v2 = p - b;

        dot[1] = v1.CrossProduct(v2);
        zeroDot[1] = Math::RealEqual(dot[1], 0.0f, 1e-3);

        // Compare signs (ignore colinear / coincident points)
        if(!zeroDot[0] && !zeroDot[1] 
        && Math::Sign(dot[0]) != Math::Sign(dot[1]))
        {
            return false;
        }

        v1 = a - c;
        v2 = p - c;

        dot[2] = v1.CrossProduct(v2);
        zeroDot[2] = Math::RealEqual(dot[2], 0.0f, 1e-3);
        // Compare signs (ignore colinear / coincident points)
        if((!zeroDot[0] && !zeroDot[2] 
        && Math::Sign(dot[0]) != Math::Sign(dot[2])) ||
            (!zeroDot[1] && !zeroDot[2] 
        && Math::Sign(dot[1]) != Math::Sign(dot[2])))
        {
            return false;
        }


        return true;
    }
    //-----------------------------------------------------------------------
    bool Math::PointInTri3D(Vector3 p, Vector3 a, 
        Vector3 b, Vector3 c, Vector3 normal)
    {
        // Winding must be consistent from all edges for point to be inside
        Vector3 v1, v2;
        Real dot[3];
        bool zeroDot[3];

        v1 = b - a;
        v2 = p - a;

        // Note we don't care about normalisation here since sign is all we need
        // It means we don't have to worry about magnitude of cross products either
        dot[0] = v1.CrossProduct(v2).DotProduct(normal);
        zeroDot[0] = Math::RealEqual(dot[0], 0.0f, 1e-3);


        v1 = c - b;
        v2 = p - b;

        dot[1] = v1.CrossProduct(v2).DotProduct(normal);
        zeroDot[1] = Math::RealEqual(dot[1], 0.0f, 1e-3);

        // Compare signs (ignore colinear / coincident points)
        if(!zeroDot[0] && !zeroDot[1] 
        && Math::Sign(dot[0]) != Math::Sign(dot[1]))
        {
            return false;
        }

        v1 = a - c;
        v2 = p - c;

        dot[2] = v1.CrossProduct(v2).DotProduct(normal);
        zeroDot[2] = Math::RealEqual(dot[2], 0.0f, 1e-3);
        // Compare signs (ignore colinear / coincident points)
        if((!zeroDot[0] && !zeroDot[2] 
        && Math::Sign(dot[0]) != Math::Sign(dot[2])) ||
            (!zeroDot[1] && !zeroDot[2] 
        && Math::Sign(dot[1]) != Math::Sign(dot[2])))
        {
            return false;
        }


        return true;
    }
    //-----------------------------------------------------------------------
    bool Math::RealEqual( Real a, Real b, Real tolerance )
    {
        if (fabs(b-a) <= tolerance)
            return true;
        else
            return false;
    }

    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, Plane plane)
    {

        Real denom = plane.normal.DotProduct(ray.Direction);
        if (Math::Abs(denom) < Real::Epsilon)
        {
            // Parallel
            return Pair<bool, Real>(false, 0);
        }
        else
        {
            Real nom = plane.normal.DotProduct(ray.Origin) + plane.d;
            Real t = -(nom/denom);
            return Pair<bool, Real>(t >= 0, t);
        }

    }
    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, 
        Collections::Generic::List<Plane>^ planes, bool normalIsOutside)
    {
        bool allInside = true;
        Pair<bool, Real> ret;
        ret.first = false;
        ret.second = 0.0f;

        // derive side
        // NB we don't pass directly since that would require Plane::Side in 
        // interface, which results in recursive includes since Math is so fundamental
        Plane::Side outside = normalIsOutside ? Plane::Side::POSITIVE_SIDE : Plane::Side::NEGATIVE_SIDE;

        int count = planes->Count;
        for (int i=0; i < count; i++)
        {
            Plane plane = planes[i];
            // is origin outside?
            if (plane.GetSide(ray.Origin) == outside)
            {
                allInside = false;
                // Test single plane
                Pair<bool, Real> planeRes = 
                    ray.Intersects(plane);
                if (planeRes.first)
                {
                    // Ok, we intersected
                    ret.first = true;
                    // Use the most distant result since convex volume
                    ret.second = System::Math::Max(ret.second, planeRes.second);
                }
            }
        }

        if (allInside)
        {
            // Intersecting at 0 distance since inside the volume!
            ret.first = true;
            ret.second = 0.0f;
        }

        return ret;
    }
    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, 
        Collections::Generic::IEnumerable<Plane>^ planes, bool normalIsOutside)
    {
        bool allInside = true;
        Pair<bool, Real> ret;
        ret.first = false;
        ret.second = 0.0f;

        // derive side
        // NB we don't pass directly since that would require Plane::Side in 
        // interface, which results in recursive includes since Math is so fundamental
        Plane::Side outside = normalIsOutside ? Plane::Side::POSITIVE_SIDE : Plane::Side::NEGATIVE_SIDE;

        for each (Plane plane in planes)
        {
            // is origin outside?
            if (plane.GetSide(ray.Origin) == outside)
            {
                allInside = false;
                // Test single plane
                Pair<bool, Real> planeRes = 
                    ray.Intersects(plane);
                if (planeRes.first)
                {
                    // Ok, we intersected
                    ret.first = true;
                    // Use the most distant result since convex volume
                    ret.second = System::Math::Max(ret.second, planeRes.second);
                }
            }
        }

        if (allInside)
        {
            // Intersecting at 0 distance since inside the volume!
            ret.first = true;
            ret.second = 0.0f;
        }

        return ret;
    }
    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, Sphere sphere, 
        bool discardInside)
    {
        Vector3 raydir = ray.Direction;
        // Adjust ray origin relative to sphere center
        Vector3 rayorig = ray.Origin - sphere.Center;
        Real radius = sphere.Radius;

        // Check origin inside first
        if (rayorig.SquaredLength <= radius*radius && discardInside)
        {
            return Pair<bool, Real>(true, 0);
        }

        // Mmm, quadratics
        // Build coeffs which can be used with std quadratic solver
        // ie t = (-b +/- sqrt(b*b + 4ac)) / 2a
        Real a = raydir.DotProduct(raydir);
        Real b = 2 * rayorig.DotProduct(raydir);
        Real c = rayorig.DotProduct(rayorig) - radius*radius;

        // Calc determinant
        Real d = (b*b) - (4 * a * c);
        if (d < 0)
        {
            // No intersection
            return Pair<bool, Real>(false, 0);
        }
        else
        {
            // BTW, if d=0 there is one intersection, if d > 0 there are 2
            // But we only want the closest one, so that's ok, just use the 
            // '-' version of the solver
            Real t = ( -b - Math::Sqrt(d) ) / (2 * a);
            if (t < 0)
                t = ( -b + Math::Sqrt(d) ) / (2 * a);
            return Pair<bool, Real>(true, t);
        }


    }

    Pair<bool, Real> Math::Intersects(Ray ray, Sphere sphere)
    {
        return Intersects(ray, sphere, true);
    }

    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, AxisAlignedBox^ box)
    {
        if (box->IsNull) return Pair<bool, Real>(false, 0);

        Real lowt = 0.0f;
        Real t;
        bool hit = false;
        Vector3 hitpoint;
        Vector3 min = box->Minimum;
        Vector3 max = box->Maximum;
        Vector3 rayorig = ray.Origin;
        Vector3 raydir = ray.Direction;

        // Check origin inside first
        if ( rayorig > min && rayorig < max )
        {
            return Pair<bool, Real>(true, 0);
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

        return Pair<bool, Real>(hit, lowt);

    }
    //-----------------------------------------------------------------------
    bool Math::Intersects(Ray ray, AxisAlignedBox^ box,
        Real% d1, Real% d2)
    {
        if (box->IsNull)
            return false;

        Vector3 min = box->Minimum;
        Vector3 max = box->Maximum;
        Vector3 rayorig = ray.Origin;
        Vector3 raydir = ray.Direction;

        Vector3 absDir;
        absDir[0] = Math::Abs(raydir[0]);
        absDir[1] = Math::Abs(raydir[1]);
        absDir[2] = Math::Abs(raydir[2]);

        // Sort the axis, ensure check minimise floating error axis first
        int imax = 0, imid = 1, imin = 2;
        if (absDir[0] < absDir[2])
        {
            imax = 2;
            imin = 0;
        }
        if (absDir[1] < absDir[imin])
        {
            imid = imin;
            imin = 1;
        }
        else if (absDir[1] > absDir[imax])
        {
            imid = imax;
            imax = 1;
        }

        Real start = 0, end = Math::POS_INFINITY;

#define _CALC_AXIS(i)                                       \
    do {                                                    \
    Real temp;											\
    Real denom = 1 / raydir[i];                         \
    Real newstart = (min[i] - rayorig[i]) * denom;      \
    Real newend = (max[i] - rayorig[i]) * denom;        \
    if (newstart > newend)								\
        { temp = newend; newend = newstart; newstart = temp; }	\
        if (newstart > end || newend < start) return false; \
        if (newstart > start) start = newstart;             \
        if (newend < end) end = newend;                     \
    } while(0)

        // Check each axis in turn

        _CALC_AXIS(imax);

        if (absDir[imid] < Real::Epsilon)
        {
            // Parallel with middle and minimise axis, check bounds only
            if (rayorig[imid] < min[imid] || rayorig[imid] > max[imid] ||
                rayorig[imin] < min[imin] || rayorig[imin] > max[imin])
                return false;
        }
        else
        {
            _CALC_AXIS(imid);

            if (absDir[imin] < Real::Epsilon)
            {
                // Parallel with minimise axis, check bounds only
                if (rayorig[imin] < min[imin] || rayorig[imin] > max[imin])
                    return false;
            }
            else
            {
                _CALC_AXIS(imin);
            }
        }
#undef _CALC_AXIS

        d1 = start;
        d2 = end;

        return true;
    }
    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c, Vector3 normal,
        bool positiveSide, bool negativeSide)
    {
        //
        // Calculate intersection with plane.
        //
        Real t;
        {
            Real denom = normal.DotProduct(ray.Direction);

            // Check intersect side
            if (denom > + Real::Epsilon)
            {
                if (!negativeSide)
                    return Pair<bool, Real>(false, 0);
            }
            else if (denom < - Real::Epsilon)
            {
                if (!positiveSide)
                    return Pair<bool, Real>(false, 0);
            }
            else
            {
                // Parallel or triangle area is close to zero when
                // the plane normal not normalised.
                return Pair<bool, Real>(false, 0);
            }

            t = normal.DotProduct(a - ray.Origin) / denom;

            if (t < 0)
            {
                // Intersection is behind origin
                return Pair<bool, Real>(false, 0);
            }
        }

        //
        // Calculate the largest area projection plane in X, Y or Z.
        //
        size_t i0, i1;
        {
            Real n0 = Math::Abs(normal[0]);
            Real n1 = Math::Abs(normal[1]);
            Real n2 = Math::Abs(normal[2]);

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
            Real u1 = b[i0] - a[i0];
            Real v1 = b[i1] - a[i1];
            Real u2 = c[i0] - a[i0];
            Real v2 = c[i1] - a[i1];
            Real u0 = t * ray.Direction[i0] + ray.Origin[i0] - a[i0];
            Real v0 = t * ray.Direction[i1] + ray.Origin[i1] - a[i1];

            Real alpha = u0 * v2 - u2 * v0;
            Real beta  = u1 * v0 - u0 * v1;
            Real area  = u1 * v2 - u2 * v1;

            // epsilon to avoid float precision error
            const Real EPSILON = 1e-3f;

            Real tolerance = - EPSILON * area;

            if (area > 0)
            {
                if (alpha < tolerance || beta < tolerance || alpha+beta > area-tolerance)
                    return Pair<bool, Real>(false, 0);
            }
            else
            {
                if (alpha > tolerance || beta > tolerance || alpha+beta < area-tolerance)
                    return Pair<bool, Real>(false, 0);
            }
        }

        return Pair<bool, Real>(true, t);
    }

    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c, Vector3 normal,
        bool positiveSide)
    {
        return Intersects(ray, a, b, c, normal, positiveSide, true);
    }

    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c, Vector3 normal )
    {
        return Intersects(ray, a, b, c, normal, true, true);
    }

    //-----------------------------------------------------------------------
    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c,
        bool positiveSide, bool negativeSide)
    {
        Vector3 normal = CalculateBasicFaceNormalWithoutNormalize(a, b, c);
        return Intersects(ray, a, b, c, normal, positiveSide, negativeSide);
    }

    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c,
        bool positiveSide)
    {
        return Intersects(ray, a, b, c, positiveSide, true);
    }

    Pair<bool, Real> Math::Intersects(Ray ray, Vector3 a,
        Vector3 b, Vector3 c )
    {
        return Intersects(ray, a, b, c, true, true);
    }

    //-----------------------------------------------------------------------
    bool Math::Intersects(Sphere sphere, AxisAlignedBox^ box)
    {
        if (box->IsNull) return false;

        // Use splitting planes
        Vector3 center = sphere.Center;
        Real radius = sphere.Radius;
        Vector3 min = box->Minimum;
        Vector3 max = box->Maximum;

        // just test facing planes, early fail if sphere is totally outside
        if (center.x < min.x && 
            min.x - center.x > radius)
        {
            return false;
        }
        if (center.x > max.x && 
            center.x  - max.x > radius)
        {
            return false;
        }

        if (center.y < min.y && 
            min.y - center.y > radius)
        {
            return false;
        }
        if (center.y > max.y && 
            center.y  - max.y > radius)
        {
            return false;
        }

        if (center.z < min.z && 
            min.z - center.z > radius)
        {
            return false;
        }
        if (center.z > max.z && 
            center.z  - max.z > radius)
        {
            return false;
        }

        // Must intersect
        return true;

    }
    //-----------------------------------------------------------------------
    bool Math::Intersects(Plane plane, AxisAlignedBox^ box)
    {
        if (box->IsNull) return false;

        // Get corners of the box
        array<Vector3>^ pCorners = box->GetAllCorners();


        // Test which side of the plane the corners are
        // Intersection occurs when at least one corner is on the 
        // opposite side to another
        Plane::Side lastSide = plane.GetSide(pCorners[0]);
        for (int corner = 1; corner < 8; ++corner)
        {
            if (plane.GetSide(pCorners[corner]) != lastSide)
            {
                return true;
            }
        }

        return false;
    }
    //-----------------------------------------------------------------------
    bool Math::Intersects(Sphere sphere, Plane plane)
    {
        return (
            Math::Abs(plane.normal.DotProduct(sphere.Center))
            <= sphere.Radius );
    }
    //-----------------------------------------------------------------------
    Vector3 Math::CalculateTangentSpaceVector(
        Vector3 position1, Vector3 position2, Vector3 position3,
        Real u1, Real v1, Real u2, Real v2, Real u3, Real v3)
    {
        //side0 is the vector along one side of the triangle of vertices passed in, 
        //and side1 is the vector along another side. Taking the cross product of these returns the normal.
        Vector3 side0 = position1 - position2;
        Vector3 side1 = position3 - position1;
        //Calculate face normal
        Vector3 normal = side1.CrossProduct(side0);
        normal.Normalise();
        //Now we use a formula to calculate the tangent. 
        Real deltaV0 = v1 - v2;
        Real deltaV1 = v3 - v1;
        Vector3 tangent = deltaV1 * side0 - deltaV0 * side1;
        tangent.Normalise();
        //Calculate binormal
        Real deltaU0 = u1 - u2;
        Real deltaU1 = u3 - u1;
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
    //-----------------------------------------------------------------------
    Matrix4^ Math::BuildReflectionMatrix(Plane p)
    {
        return gcnew Matrix4(
            -2 * p.normal.x * p.normal.x + 1,   -2 * p.normal.x * p.normal.y,       -2 * p.normal.x * p.normal.z,       -2 * p.normal.x * p.d, 
            -2 * p.normal.y * p.normal.x,       -2 * p.normal.y * p.normal.y + 1,   -2 * p.normal.y * p.normal.z,       -2 * p.normal.y * p.d, 
            -2 * p.normal.z * p.normal.x,       -2 * p.normal.z * p.normal.y,       -2 * p.normal.z * p.normal.z + 1,   -2 * p.normal.z * p.d, 
            0,                                  0,                                  0,                                  1);
    }
    //-----------------------------------------------------------------------
    Vector4 Math::CalculateFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 normal = CalculateBasicFaceNormal(v1, v2, v3);
        // Now set up the w (distance of tri from origin
        return Vector4(normal.x, normal.y, normal.z, -(normal.DotProduct(v1)));
    }
    //-----------------------------------------------------------------------
    Vector3 Math::CalculateBasicFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 normal = (v2 - v1).CrossProduct(v3 - v1);
        normal.Normalise();
        return normal;
    }
    //-----------------------------------------------------------------------
    Vector4 Math::CalculateFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 normal = CalculateBasicFaceNormalWithoutNormalize(v1, v2, v3);
        // Now set up the w (distance of tri from origin)
        return Vector4(normal.x, normal.y, normal.z, -(normal.DotProduct(v1)));
    }
    //-----------------------------------------------------------------------
    Vector3 Math::CalculateBasicFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 normal = (v2 - v1).CrossProduct(v3 - v1);
        return normal;
    }
    //-----------------------------------------------------------------------
    Real Math::GaussianDistribution(Real x, Real offset, Real scale)
    {
        Real nom = System::Math::Exp(
            -Math::Sqr(x - offset) / (2 * Math::Sqr(scale)));
        Real denom = scale * Math::Sqrt(2 * Math::PI);

        return nom / denom;

    }
    //---------------------------------------------------------------------
    Matrix4^ Math::MakeViewMatrix(Vector3 position, Quaternion orientation, 
        Matrix4^ reflectMatrix)
    {
        Matrix4^ viewMatrix;

        // View matrix is:
        //
        //  [ Lx  Uy  Dz  Tx  ]
        //  [ Lx  Uy  Dz  Ty  ]
        //  [ Lx  Uy  Dz  Tz  ]
        //  [ 0   0   0   1   ]
        //
        // Where T = -(Transposed(Rot) * Pos)

        // This is most efficiently done using 3x3 Matrices
        Matrix3^ rot = orientation.ToRotationMatrix();

        // Make the translation relative to new axes
        Matrix3^ rotT = rot->Transpose();
        Vector3 trans = -rotT * position;

        // Make final matrix
        viewMatrix = gcnew Matrix4(rotT); // fills upper 3x3
        viewMatrix->m03 = trans.x;
        viewMatrix->m13 = trans.y;
        viewMatrix->m23 = trans.z;

        // Deal with reflections
        if (reflectMatrix)
        {
            viewMatrix = viewMatrix * reflectMatrix;
        }

        return viewMatrix;
    }

    Matrix4^ Math::MakeViewMatrix(Vector3 position, Quaternion orientation)
    {
        return MakeViewMatrix(position, orientation, nullptr);
    }

    //---------------------------------------------------------------------
    Real Math::BoundingRadiusFromAABB(AxisAlignedBox^ aabb)
    {
        Vector3 max = aabb->Maximum;
        Vector3 min = aabb->Minimum;

        Vector3 magnitude = max;
        magnitude.MakeCeil(-max);
        magnitude.MakeCeil(min);
        magnitude.MakeCeil(-min);

        return magnitude.Length;
    }
}