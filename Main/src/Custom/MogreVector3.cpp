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

#include "Custom\MogreVector3.h"
#include "Custom\MogreQuaternion.h"

namespace Mogre
{
    Vector3 Vector3::RandomDeviant(
        Radian angle,
        Vector3 up )
    {
        Vector3 newUp;

        if (up == Vector3::ZERO)
        {
            // Generate an up vector
            newUp = this->Perpendicular;
        }
        else
        {
            newUp = up;
        }

        // Rotate up vector by random amount around this
        Quaternion q;
        q.FromAngleAxis( Radian(Math::UnitRandom() * Math::TWO_PI), *this );
        newUp = q * newUp;

        // Finally rotate this by given angle around randomised up
        q.FromAngleAxis( angle, newUp );
        return q * (*this);
    }

    Quaternion Vector3::GetRotationTo(Vector3 dest, Vector3 fallbackAxis)
    {
        // Based on Stan Melax's article in Game Programming Gems
        Quaternion q;
        // Copy, since cannot modify local
        Vector3 v0 = *this;
        Vector3 v1 = dest;
        v0.Normalise();
        v1.Normalise();

        Real d = v0.DotProduct(v1);
        // If dot == 1, vectors are the same
        if (d >= 1.0f)
        {
            return Quaternion::IDENTITY;
        }

        if (d < (1e-6f - 1.0f))
        {
            if (fallbackAxis != Vector3::ZERO)
            {
                // rotate 180 degrees about the fallback axis
                q.FromAngleAxis(Radian(Math::PI), fallbackAxis);
            }
            else
            {
                // Generate an axis
                Vector3 axis = Vector3::UNIT_X.CrossProduct(*this);
                if (axis.IsZeroLength) // pick another if colinear
                    axis = Vector3::UNIT_Y.CrossProduct(*this);
                axis.Normalise();
                q.FromAngleAxis(Radian(Math::PI), axis);
            }
        }
        else
        {
            Real s = Math::Sqrt( (1+d)*2 );
            Real invs = 1 / s;

            Vector3 c = v0.CrossProduct(v1);

            q.x = c.x * invs;
            q.y = c.y * invs;
            q.z = c.z * invs;
            q.w = s * 0.5f;
            q.Normalise();
        }
        return q;
    }

    Quaternion Vector3::GetRotationTo(Vector3 dest)
    {
        return GetRotationTo(dest, Vector3::ZERO);
    }
}