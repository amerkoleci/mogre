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

#include "Custom\MogreMatrix4.h"

namespace Mogre
{
    //-----------------------------------------------------------------------
    inline static Real
        MINOR(Matrix4^ m, const size_t r0, const size_t r1, const size_t r2, 
        const size_t c0, const size_t c1, const size_t c2)
    {
        return m[r0, c0] * (m[r1, c1] * m[r2, c2] - m[r2, c1] * m[r1, c2]) -
            m[r0, c1] * (m[r1, c0] * m[r2, c2] - m[r2, c0] * m[r1, c2]) +
            m[r0, c2] * (m[r1, c0] * m[r2, c1] - m[r2, c0] * m[r1, c1]);
    }
    //-----------------------------------------------------------------------
    Matrix4^ Matrix4::Adjoint()
    {
        return gcnew Matrix4( MINOR(this, 1, 2, 3, 1, 2, 3),
            -MINOR(this, 0, 2, 3, 1, 2, 3),
            MINOR(this, 0, 1, 3, 1, 2, 3),
            -MINOR(this, 0, 1, 2, 1, 2, 3),

            -MINOR(this, 1, 2, 3, 0, 2, 3),
            MINOR(this, 0, 2, 3, 0, 2, 3),
            -MINOR(this, 0, 1, 3, 0, 2, 3),
            MINOR(this, 0, 1, 2, 0, 2, 3),

            MINOR(this, 1, 2, 3, 0, 1, 3),
            -MINOR(this, 0, 2, 3, 0, 1, 3),
            MINOR(this, 0, 1, 3, 0, 1, 3),
            -MINOR(this, 0, 1, 2, 0, 1, 3),

            -MINOR(this, 1, 2, 3, 0, 1, 2),
            MINOR(this, 0, 2, 3, 0, 1, 2),
            -MINOR(this, 0, 1, 3, 0, 1, 2),
            MINOR(this, 0, 1, 2, 0, 1, 2));
    }
    //-----------------------------------------------------------------------
    Real Matrix4::Determinant::get()
    {
        return m00 * MINOR(this, 1, 2, 3, 1, 2, 3) -
            m01 * MINOR(this, 1, 2, 3, 0, 2, 3) +
            m02 * MINOR(this, 1, 2, 3, 0, 1, 3) -
            m03 * MINOR(this, 1, 2, 3, 0, 1, 2);
    }
    //-----------------------------------------------------------------------
    Matrix4^ Matrix4::Inverse()
    {
        Real v0 = m20 * m31 - m21 * m30;
        Real v1 = m20 * m32 - m22 * m30;
        Real v2 = m20 * m33 - m23 * m30;
        Real v3 = m21 * m32 - m22 * m31;
        Real v4 = m21 * m33 - m23 * m31;
        Real v5 = m22 * m33 - m23 * m32;

        Real t00 = + (v5 * m11 - v4 * m12 + v3 * m13);
        Real t10 = - (v5 * m10 - v2 * m12 + v1 * m13);
        Real t20 = + (v4 * m10 - v2 * m11 + v0 * m13);
        Real t30 = - (v3 * m10 - v1 * m11 + v0 * m12);

        Real invDet = 1 / (t00 * m00 + t10 * m01 + t20 * m02 + t30 * m03);

        Real d00 = t00 * invDet;
        Real d10 = t10 * invDet;
        Real d20 = t20 * invDet;
        Real d30 = t30 * invDet;

        Real d01 = - (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
        Real d11 = + (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
        Real d21 = - (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
        Real d31 = + (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

        v0 = m10 * m31 - m11 * m30;
        v1 = m10 * m32 - m12 * m30;
        v2 = m10 * m33 - m13 * m30;
        v3 = m11 * m32 - m12 * m31;
        v4 = m11 * m33 - m13 * m31;
        v5 = m12 * m33 - m13 * m32;

        Real d02 = + (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
        Real d12 = - (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
        Real d22 = + (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
        Real d32 = - (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

        v0 = m21 * m10 - m20 * m11;
        v1 = m22 * m10 - m20 * m12;
        v2 = m23 * m10 - m20 * m13;
        v3 = m22 * m11 - m21 * m12;
        v4 = m23 * m11 - m21 * m13;
        v5 = m23 * m12 - m22 * m13;

        Real d03 = - (v5 * m01 - v4 * m02 + v3 * m03) * invDet;
        Real d13 = + (v5 * m00 - v2 * m02 + v1 * m03) * invDet;
        Real d23 = - (v4 * m00 - v2 * m01 + v0 * m03) * invDet;
        Real d33 = + (v3 * m00 - v1 * m01 + v0 * m02) * invDet;

        return gcnew Matrix4(
            d00, d01, d02, d03,
            d10, d11, d12, d13,
            d20, d21, d22, d23,
            d30, d31, d32, d33);
    }
    //-----------------------------------------------------------------------
    void Matrix4::MakeTransform(Vector3 position, Vector3 scale, Quaternion orientation)
    {
        // Ordering:
        //    1. Scale
        //    2. Rotate
        //    3. Translate

        Matrix3^ rot3x3 = orientation.ToRotationMatrix();

        // Set up final matrix with scale, rotation and translation
        m00 = scale.x * rot3x3->m00; m01 = scale.y * rot3x3->m01; m02 = scale.z * rot3x3->m02; m03 = position.x;
        m10 = scale.x * rot3x3->m10; m11 = scale.y * rot3x3->m11; m12 = scale.z * rot3x3->m12; m13 = position.y;
        m20 = scale.x * rot3x3->m20; m21 = scale.y * rot3x3->m21; m22 = scale.z * rot3x3->m22; m23 = position.z;

        // No projection term
        m30 = 0; m31 = 0; m32 = 0; m33 = 1;
    }
    //-----------------------------------------------------------------------
    void Matrix4::MakeInverseTransform(Vector3 position, Vector3 scale, Quaternion orientation)
    {
        // Invert the parameters
        Vector3 invTranslate = -position;
        Vector3 invScale(1 / scale.x, 1 / scale.y, 1 / scale.z);
        Quaternion invRot = orientation.Inverse();

        // Because we're inverting, order is translation, rotation, scale
        // So make translation relative to scale & rotation
        invTranslate = invRot * invTranslate; // rotate
        invTranslate *= invScale; // scale

        // Next, make a 3x3 rotation matrix
        Matrix3^ rot3x3 = invRot.ToRotationMatrix();

        // Set up final matrix with scale, rotation and translation
        m00 = invScale.x * rot3x3->m00; m01 = invScale.x * rot3x3->m01; m02 = invScale.x * rot3x3->m02; m03 = invTranslate.x;
        m10 = invScale.y * rot3x3->m10; m11 = invScale.y * rot3x3->m11; m12 = invScale.y * rot3x3->m12; m13 = invTranslate.y;
        m20 = invScale.z * rot3x3->m20; m21 = invScale.z * rot3x3->m21; m22 = invScale.z * rot3x3->m22; m23 = invTranslate.z;

        // No projection term
        m30 = 0; m31 = 0; m32 = 0; m33 = 1;
    }
    //-----------------------------------------------------------------------
    void Matrix4::Decomposition([Out] Vector3% position, [Out] Vector3% scale, [Out] Quaternion% orientation)
    {
        assert(IsAffine);

        Matrix3^ m3x3 = Extract3x3Matrix();

        Matrix3^ matQ;
        Vector3 vecU;
        m3x3->QDUDecomposition( matQ, scale, vecU ); 

        orientation = Quaternion( matQ );
        position = Vector3( m03, m13, m23 );
    }
    //-----------------------------------------------------------------------
    Matrix4^ Matrix4::InverseAffine()
    {
        if (!IsAffine)
            throw gcnew Exception("This matrix must be affine");

        Real m10 = this->m10, m11 = this->m11, m12 = this->m12;
        Real m20 = this->m20, m21 = this->m21, m22 = this->m22;

        Real t00 = m22 * m11 - m21 * m12;
        Real t10 = m20 * m12 - m22 * m10;
        Real t20 = m21 * m10 - m20 * m11;

        Real m00 = this->m00, m01 = this->m01, m02 = this->m02;

        Real invDet = 1 / (m00 * t00 + m01 * t10 + m02 * t20);

        t00 *= invDet; t10 *= invDet; t20 *= invDet;

        m00 *= invDet; m01 *= invDet; m02 *= invDet;

        Real r00 = t00;
        Real r01 = m02 * m21 - m01 * m22;
        Real r02 = m01 * m12 - m02 * m11;

        Real r10 = t10;
        Real r11 = m00 * m22 - m02 * m20;
        Real r12 = m02 * m10 - m00 * m12;

        Real r20 = t20;
        Real r21 = m01 * m20 - m00 * m21;
        Real r22 = m00 * m11 - m01 * m10;

        Real m03 = this->m03, m13 = this->m13, m23 = this->m23;

        Real r03 = - (r00 * m03 + r01 * m13 + r02 * m23);
        Real r13 = - (r10 * m03 + r11 * m13 + r12 * m23);
        Real r23 = - (r20 * m03 + r21 * m13 + r22 * m23);

        return gcnew Matrix4(
            r00, r01, r02, r03,
            r10, r11, r12, r13,
            r20, r21, r22, r23,
            0,   0,   0,   1);
    }
}