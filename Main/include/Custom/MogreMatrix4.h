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
#include "OgreMatrix4.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"
#include "Custom\MogreMatrix3.h"
#include "Custom\MogreQuaternion.h"
#include "Custom\MogreVector3.h"
#include "MogreVector4.h"
#include "Custom\MogrePlane.h"

namespace Mogre
{
    /** <summary>Class encapsulating a standard 4x4 homogenous matrix.</summary>
    <remarks>
    OGRE uses column vectors when applying matrix multiplications,
    This means a vector is represented as a single column, 4-row
    matrix. This has the effect that the tranformations implemented
    by the matrices happens right-to-left e.g. if vector V is to be
    transformed by M1 then M2 then M3, the calculation would be
    M3 * M2 * M1 * V. The order that matrices are concatenated is
    vital since matrix multiplication is not cummatative, i.e. you
    can get a different result if you concatenate in the wrong order.
    <para/>
    The use of column vectors and right-to-left ordering is the
    standard in most mathematical texts, and id the same as used in
    OpenGL. It is, however, the opposite of Direct3D, which has
    inexplicably chosen to differ from the accepted standard and uses
    row vectors and left-to-right matrix multiplication.
    <para/>
    OGRE deals with the differences between D3D and OpenGL etc.
    internally when operating through different render systems. OGRE
    users only need to conform to standard maths conventions, i.e.
    right-to-left matrix multiplication, (OGRE transposes matrices it
    passes to D3D to compensate).
    <para/>
    The generic form M * V which shows the layout of the matrix 
    entries is shown below:
    <pre>
    [ m00  m01  m02  m03 ]   {x}
    | m10  m11  m12  m13 | * {y}
    | m20  m21  m22  m23 |   {z}
    [ m30  m31  m32  m33 ]   {1}
    </pre>
    </remarks>
    */
    [Serializable]
    [StructLayout(LayoutKind::Explicit)]
    public ref class Matrix4
    {
    public:
        [Serializable]
        [StructLayout(LayoutKind::Sequential)]
        value struct NativeValue
        {
            Real m00, m01, m02, m03;
            Real m10, m11, m12, m13;
            Real m20, m21, m22, m23;
            Real m30, m31, m32, m33;

            property Real default[int,int]
            {
                inline Real get(int row, int col)
                {
                    assert( row < 4 && col < 4  );
                    return *(&m00 + ((4*row) + col)); 
                }

                inline void set (int row, int col, Real value)
                {
                    assert( row < 4 && col < 4 );
                    *(&m00 + ((4*row) + col)) = value;
                }
            }
        };

        static operator Matrix4^ (const Ogre::Matrix4& matrix)
        {
            Matrix4^ mat = gcnew Matrix4;
            pin_ptr<Real> pdest = &mat->m00;
            memcpy(pdest,matrix[0],16*sizeof(Real));
            return mat;
        }

    public:
        [FieldOffset(0)]
        NativeValue value;

        [FieldOffset(0)]
        Real m00;
        [FieldOffset(1*sizeof(Real))]
        Real m01;
        [FieldOffset(2*sizeof(Real))]
        Real m02;
        [FieldOffset(3*sizeof(Real))]
        Real m03;
        [FieldOffset(4*sizeof(Real))]
        Real m10;
        [FieldOffset(5*sizeof(Real))]
        Real m11;
        [FieldOffset(6*sizeof(Real))]
        Real m12;
        [FieldOffset(7*sizeof(Real))]
        Real m13;
        [FieldOffset(8*sizeof(Real))]
        Real m20;
        [FieldOffset(9*sizeof(Real))]
        Real m21;
        [FieldOffset(10*sizeof(Real))]
        Real m22;
        [FieldOffset(11*sizeof(Real))]
        Real m23;
        [FieldOffset(12*sizeof(Real))]
        Real m30;
        [FieldOffset(13*sizeof(Real))]
        Real m31;
        [FieldOffset(14*sizeof(Real))]
        Real m32;
        [FieldOffset(15*sizeof(Real))]
        Real m33;

        /** <summary>Default constructor.</summary>
        <remarks>
        <note>
        It does <b>NOT</b> initialize the matrix for efficiency.
        </note>
        </remarks>
        */
        inline Matrix4()
        {
        }
        inline Matrix4 (Matrix4^ rkMatrix)
        {
            pin_ptr<Real> pdest = &m00;
            pin_ptr<Real> psrc = &rkMatrix->m00;
            memcpy(pdest,psrc,16*sizeof(Real));
        }

        inline Matrix4(
            Real fm00, Real fm01, Real fm02, Real fm03,
            Real fm10, Real fm11, Real fm12, Real fm13,
            Real fm20, Real fm21, Real fm22, Real fm23,
            Real fm30, Real fm31, Real fm32, Real fm33 )
        {
            m00 = fm00;
            m01 = fm01;
            m02 = fm02;
            m03 = fm03;
            m10 = fm10;
            m11 = fm11;
            m12 = fm12;
            m13 = fm13;
            m20 = fm20;
            m21 = fm21;
            m22 = fm22;
            m23 = fm23;
            m30 = fm30;
            m31 = fm31;
            m32 = fm32;
            m33 = fm33;
        }

        /** <summary>Creates a standard 4x4 transformation matrix with a zero translation part from a rotation/scaling 3x3 matrix.</summary>
        */
        inline Matrix4(Matrix3^ m3x3)
        {
            operator=(IDENTITY);
            operator=(m3x3);
        }

        /** <summary>Creates a standard 4x4 transformation matrix with a zero translation part from a rotation/scaling Quaternion.</summary>
        */
        inline Matrix4(Quaternion rot)
        {
            Matrix3^ m3x3 = rot.ToRotationMatrix();
            operator=(IDENTITY);
            operator=(m3x3);
        }


        property Real default[int,int]
        {
            inline Real get(int row, int col)
            {
                assert( row < 4 && col < 4  );
                return *(&m00 + ((4*row) + col)); 
            }

            inline void set (int row, int col, Real value)
            {
                assert( row < 4 && col < 4 );
                *(&m00 + ((4*row) + col)) = value;
            }
        }

        inline Matrix4^ Concatenate(Matrix4^ m2)
        {
            Matrix4^ r = gcnew Matrix4;
            r->m00 = m00 * m2->m00 + m01 * m2->m10 + m02 * m2->m20 + m03 * m2->m30;
            r->m01 = m00 * m2->m01 + m01 * m2->m11 + m02 * m2->m21 + m03 * m2->m31;
            r->m02 = m00 * m2->m02 + m01 * m2->m12 + m02 * m2->m22 + m03 * m2->m32;
            r->m03 = m00 * m2->m03 + m01 * m2->m13 + m02 * m2->m23 + m03 * m2->m33;

            r->m10 = m10 * m2->m00 + m11 * m2->m10 + m12 * m2->m20 + m13 * m2->m30;
            r->m11 = m10 * m2->m01 + m11 * m2->m11 + m12 * m2->m21 + m13 * m2->m31;
            r->m12 = m10 * m2->m02 + m11 * m2->m12 + m12 * m2->m22 + m13 * m2->m32;
            r->m13 = m10 * m2->m03 + m11 * m2->m13 + m12 * m2->m23 + m13 * m2->m33;

            r->m20 = m20 * m2->m00 + m21 * m2->m10 + m22 * m2->m20 + m23 * m2->m30;
            r->m21 = m20 * m2->m01 + m21 * m2->m11 + m22 * m2->m21 + m23 * m2->m31;
            r->m22 = m20 * m2->m02 + m21 * m2->m12 + m22 * m2->m22 + m23 * m2->m32;
            r->m23 = m20 * m2->m03 + m21 * m2->m13 + m22 * m2->m23 + m23 * m2->m33;

            r->m30 = m30 * m2->m00 + m31 * m2->m10 + m32 * m2->m20 + m33 * m2->m30;
            r->m31 = m30 * m2->m01 + m31 * m2->m11 + m32 * m2->m21 + m33 * m2->m31;
            r->m32 = m30 * m2->m02 + m31 * m2->m12 + m32 * m2->m22 + m33 * m2->m32;
            r->m33 = m30 * m2->m03 + m31 * m2->m13 + m32 * m2->m23 + m33 * m2->m33;

            return r;
        }

        /** <summary>Matrix concatenation using '*'.</summary>
        */
        inline static Matrix4^ operator * ( Matrix4^ m1, Matrix4^ m2 )
        {
            return m1->Concatenate( m2 );
        }

        /** <summary>Vector transformation using '*'.</summary>
        <remarks>
        Transforms the given 3-D vector by the matrix, projecting the 
        result back into <i>w</i> = 1.
        <note>
        This means that the initial <i>w</i> is considered to be 1.0,
        and then all the tree elements of the resulting 3-D vector are
        divided by the resulting <i>w</i>.
        </note>
        </remarks>
        */
        inline static Vector3 operator * ( Matrix4^ mat, Vector3 v )
        {
            Vector3 r;

            Real fInvW = 1.0f / ( mat->m30 * v.x + mat->m31 * v.y + mat->m32 * v.z + mat->m33 );

            r.x = ( mat->m00 * v.x + mat->m01 * v.y + mat->m02 * v.z + mat->m03 ) * fInvW;
            r.y = ( mat->m10 * v.x + mat->m11 * v.y + mat->m12 * v.z + mat->m13 ) * fInvW;
            r.z = ( mat->m20 * v.x + mat->m21 * v.y + mat->m22 * v.z + mat->m23 ) * fInvW;

            return r;
        }
        inline static Vector4 operator * (Matrix4^ mat, Vector4 v)
        {
            return Vector4(
                mat->m00 * v.x + mat->m01 * v.y + mat->m02 * v.z + mat->m03 * v.w, 
                mat->m10 * v.x + mat->m11 * v.y + mat->m12 * v.z + mat->m13 * v.w,
                mat->m20 * v.x + mat->m21 * v.y + mat->m22 * v.z + mat->m23 * v.w,
                mat->m30 * v.x + mat->m31 * v.y + mat->m32 * v.z + mat->m33 * v.w
                );
        }
        inline static Plane operator * (Matrix4^ mat, Plane p)
        {
            Plane ret;
            Matrix4^ invTrans = mat->Inverse()->Transpose();
            ret.normal.x =
                invTrans->m00 * p.normal.x + invTrans->m01 * p.normal.y + invTrans->m02 * p.normal.z;
            ret.normal.y = 
                invTrans->m10 * p.normal.x + invTrans->m11 * p.normal.y + invTrans->m12 * p.normal.z;
            ret.normal.z = 
                invTrans->m20 * p.normal.x + invTrans->m21 * p.normal.y + invTrans->m22 * p.normal.z;
            Vector3 pt = p.normal * -p.d;
            pt = mat * pt;
            ret.d = - pt.DotProduct(ret.normal);
            return ret;
        }


        /** <summary>Matrix addition.</summary>
        */
        inline static Matrix4^ operator + ( Matrix4^ m1, Matrix4^ m2 )
        {
            Matrix4^ r = gcnew Matrix4;

            r->m00 = m1->m00 + m2->m00;
            r->m01 = m1->m01 + m2->m01;
            r->m02 = m1->m02 + m2->m02;
            r->m03 = m1->m03 + m2->m03;

            r->m10 = m1->m10 + m2->m10;
            r->m11 = m1->m11 + m2->m11;
            r->m12 = m1->m12 + m2->m12;
            r->m13 = m1->m13 + m2->m13;

            r->m20 = m1->m20 + m2->m20;
            r->m21 = m1->m21 + m2->m21;
            r->m22 = m1->m22 + m2->m22;
            r->m23 = m1->m23 + m2->m23;

            r->m30 = m1->m30 + m2->m30;
            r->m31 = m1->m31 + m2->m31;
            r->m32 = m1->m32 + m2->m32;
            r->m33 = m1->m33 + m2->m33;

            return r;
        }

        /** <summary>Matrix subtraction.</summary>
        */
        inline static Matrix4^ operator - ( Matrix4^ m1, Matrix4^ m2 )
        {
            Matrix4^ r = gcnew Matrix4;
            r->m00 = m1->m00 - m2->m00;
            r->m01 = m1->m01 - m2->m01;
            r->m02 = m1->m02 - m2->m02;
            r->m03 = m1->m03 - m2->m03;

            r->m10 = m1->m10 - m2->m10;
            r->m11 = m1->m11 - m2->m11;
            r->m12 = m1->m12 - m2->m12;
            r->m13 = m1->m13 - m2->m13;

            r->m20 = m1->m20 - m2->m20;
            r->m21 = m1->m21 - m2->m21;
            r->m22 = m1->m22 - m2->m22;
            r->m23 = m1->m23 - m2->m23;

            r->m30 = m1->m30 - m2->m30;
            r->m31 = m1->m31 - m2->m31;
            r->m32 = m1->m32 - m2->m32;
            r->m33 = m1->m33 - m2->m33;

            return r;
        }

        /** <summary>Tests 2 matrices for equality.</summary>
        */
        inline static bool operator == ( Matrix4^ m1, Matrix4^ m2 )
        {
            if ((Object^)m1 == (Object^)m2) return true;
            if ((Object^)m1 == nullptr || (Object^)m2 == nullptr) return false;

            if( 
                m1->m00 != m2->m00 || m1->m01 != m2->m01 || m1->m02 != m2->m02 || m1->m03 != m2->m03 ||
                m1->m10 != m2->m10 || m1->m11 != m2->m11 || m1->m12 != m2->m12 || m1->m13 != m2->m13 ||
                m1->m20 != m2->m20 || m1->m21 != m2->m21 || m1->m22 != m2->m22 || m1->m23 != m2->m23 ||
                m1->m30 != m2->m30 || m1->m31 != m2->m31 || m1->m32 != m2->m32 || m1->m33 != m2->m33 )
                return false;
            return true;
        }

        /** <summary>Tests 2 matrices for inequality.</summary>
        */
        inline static bool operator != ( Matrix4^ m1, Matrix4^ m2 )
        {
            if ((Object^)m1 == (Object^)m2) return false;
            if ((Object^)m1 == nullptr || (Object^)m2 == nullptr) return true;

            if( 
                m1->m00 != m2->m00 || m1->m01 != m2->m01 || m1->m02 != m2->m02 || m1->m03 != m2->m03 ||
                m1->m10 != m2->m10 || m1->m11 != m2->m11 || m1->m12 != m2->m12 || m1->m13 != m2->m13 ||
                m1->m20 != m2->m20 || m1->m21 != m2->m21 || m1->m22 != m2->m22 || m1->m23 != m2->m23 ||
                m1->m30 != m2->m30 || m1->m31 != m2->m31 || m1->m32 != m2->m32 || m1->m33 != m2->m33 )
                return true;
            return false;
        }

        virtual bool Equals(Object^ obj) override
        {
            Matrix4^ clr = dynamic_cast<Matrix4^>(obj);
            if (clr == CLR_NULL)
            {
                return false;
            }

            return (this == clr);
        }

        bool Equals(Matrix4^ obj)
        {
            return (this == obj);
        }

        /** <summary>Assignment from 3x3 matrix.</summary>
        */
        inline void operator = ( Matrix3^ mat3 )
        {
            m00 = mat3->m00; m01 = mat3->m01; m02 = mat3->m02;
            m10 = mat3->m10; m11 = mat3->m11; m12 = mat3->m12;
            m20 = mat3->m20; m21 = mat3->m21; m22 = mat3->m22;
        }

        inline void operator = (Matrix4^ rkMatrix)
        {
            pin_ptr<Real> pdest = &m00;
            pin_ptr<Real> psrc = &rkMatrix->m00;
            memcpy(pdest,psrc,16*sizeof(Real));
        }

        inline Matrix4^ Transpose(void)
        {
            return gcnew Matrix4(m00, m10, m20, m30,
                m01, m11, m21, m31,
                m02, m12, m22, m32,
                m03, m13, m23, m33);
        }

        /*
        -----------------------------------------------------------------------
        Translation Transformation
        -----------------------------------------------------------------------
        */
        /** <summary>Sets the translation transformation part of the matrix.</summary>
        */
        inline void SetTrans( Vector3 v )
        {
            m03 = v.x;
            m13 = v.y;
            m23 = v.z;
        }

        /** <summary>Extracts the translation transformation part of the matrix.</summary>
        */
        inline Vector3 GetTrans()
        {
            return Vector3(m03, m13, m23);
        }


        /** <summary>Builds a translation matrix</summary>
        */
        inline void MakeTrans( Vector3 v )
        {
            m00 = 1.0; m01 = 0.0; m02 = 0.0; m03 = v.x;
            m10 = 0.0; m11 = 1.0; m12 = 0.0; m13 = v.y;
            m20 = 0.0; m21 = 0.0; m22 = 1.0; m23 = v.z;
            m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
        }

        /** <summary>Builds a translation matrix</summary>
        */
        inline void MakeTrans( Real tx, Real ty, Real tz )
        {
            m00 = 1.0; m01 = 0.0; m02 = 0.0; m03 = tx;
            m10 = 0.0; m11 = 1.0; m12 = 0.0; m13 = ty;
            m20 = 0.0; m21 = 0.0; m22 = 1.0; m23 = tz;
            m30 = 0.0; m31 = 0.0; m32 = 0.0; m33 = 1.0;
        }

        /** <summary>Gets a translation matrix.</summary>
        */
        inline static Matrix4^ GetTrans( Vector3 v )
        {
            Matrix4^ r = gcnew Matrix4;

            r->m00 = 1.0; r->m01 = 0.0; r->m02 = 0.0; r->m03 = v.x;
            r->m10 = 0.0; r->m11 = 1.0; r->m12 = 0.0; r->m13 = v.y;
            r->m20 = 0.0; r->m21 = 0.0; r->m22 = 1.0; r->m23 = v.z;
            r->m30 = 0.0; r->m31 = 0.0; r->m32 = 0.0; r->m33 = 1.0;

            return r;
        }

        /** <summary>Gets a translation matrix - variation for not using a vector.</summary>
        */
        inline static Matrix4^ GetTrans( Real t_x, Real t_y, Real t_z )
        {
            Matrix4^ r = gcnew Matrix4;

            r->m00 = 1.0; r->m01 = 0.0; r->m02 = 0.0; r->m03 = t_x;
            r->m10 = 0.0; r->m11 = 1.0; r->m12 = 0.0; r->m13 = t_y;
            r->m20 = 0.0; r->m21 = 0.0; r->m22 = 1.0; r->m23 = t_z;
            r->m30 = 0.0; r->m31 = 0.0; r->m32 = 0.0; r->m33 = 1.0;

            return r;
        }

        /*
        -----------------------------------------------------------------------
        Scale Transformation
        -----------------------------------------------------------------------
        */
        /** <summary>Sets the scale part of the matrix.</summary>
        */
        inline void SetScale( Vector3 v )
        {
            m00 = v.x;
            m11 = v.y;
            m22 = v.z;
        }

        /** <summary>Gets a scale matrix.</summary>
        */
        inline static Matrix4^ GetScale( Vector3 v )
        {
            Matrix4^ r = gcnew Matrix4;
            r->m00 = v.x; r->m01 = 0.0; r->m02 = 0.0; r->m03 = 0.0;
            r->m10 = 0.0; r->m11 = v.y; r->m12 = 0.0; r->m13 = 0.0;
            r->m20 = 0.0; r->m21 = 0.0; r->m22 = v.z; r->m23 = 0.0;
            r->m30 = 0.0; r->m31 = 0.0; r->m32 = 0.0; r->m33 = 1.0;

            return r;
        }

        /** <summary>Gets a scale matrix - variation for not using a vector.</summary>
        */
        inline static Matrix4^ GetScale( Real s_x, Real s_y, Real s_z )
        {
            Matrix4^ r = gcnew Matrix4;
            r->m00 = s_x; r->m01 = 0.0; r->m02 = 0.0; r->m03 = 0.0;
            r->m10 = 0.0; r->m11 = s_y; r->m12 = 0.0; r->m13 = 0.0;
            r->m20 = 0.0; r->m21 = 0.0; r->m22 = s_z; r->m23 = 0.0;
            r->m30 = 0.0; r->m31 = 0.0; r->m32 = 0.0; r->m33 = 1.0;

            return r;
        }

        /** <summary>Extracts the rotation / scaling part of the Matrix as a 3x3 matrix.</summary>
        */
        inline Matrix3^ Extract3x3Matrix()
        {
            Matrix3^ m3x3 = gcnew Matrix3;

            m3x3->m00 = m00;
            m3x3->m01 = m01;
            m3x3->m02 = m02;
            m3x3->m10 = m10;
            m3x3->m11 = m11;
            m3x3->m12 = m12;
            m3x3->m20 = m20;
            m3x3->m21 = m21;
            m3x3->m22 = m22;

            return m3x3;
        }

        /** <summary>Determines if this matrix involves a scaling.</summary> */
        property bool HasScale
        {
            inline bool get()
            {
                // check magnitude of column vectors (==local axes)
                Real t = m00 * m00 + m10 * m10 + m20 * m20;
                if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                    return true;
                t = m01 * m01 + m11 * m11 + m21 * m21;
                if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                    return true;
                t = m02 * m02 + m12 * m12 + m22 * m22;
                if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                    return true;

                return false;
            }
        }

        /** <summary>Determines if this matrix involves a negative scaling.</summary> */
        property bool HasNegativeScale
        {
            inline bool get()
            {
                return Determinant < 0;
            }
        }

        /** <summary>Extracts the rotation / scaling part as a quaternion from the Matrix.</summary>
        */
        inline Quaternion ExtractQuaternion()
        {
            Matrix3^ m3x3 = Extract3x3Matrix();
            return Quaternion(m3x3);
        }

        static initonly Matrix4^ ZERO = gcnew Matrix4 (
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0 );
        static initonly Matrix4^ ZEROAFFINE = gcnew Matrix4 (
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 1 );
        static initonly Matrix4^ IDENTITY = gcnew Matrix4 (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1 );
        /** <summary>Useful little matrix which takes 2D clipspace {-1, 1} to {0,1}
        and inverts the Y.</summary> */
        static initonly Matrix4^ CLIPSPACE2DTOIMAGESPACE = gcnew Matrix4 (
            0.5,    0,  0, 0.5, 
              0, -0.5,  0, 0.5, 
              0,    0,  1,   0,
              0,    0,  0,   1);

        inline static Matrix4^ operator*(Matrix4^ mat, Real scalar)
        {
            return gcnew Matrix4(
                scalar*mat->m00, scalar*mat->m01, scalar*mat->m02, scalar*mat->m03,
                scalar*mat->m10, scalar*mat->m11, scalar*mat->m12, scalar*mat->m13,
                scalar*mat->m20, scalar*mat->m21, scalar*mat->m22, scalar*mat->m23,
                scalar*mat->m30, scalar*mat->m31, scalar*mat->m32, scalar*mat->m33);
        }

        virtual String^ ToString() override
        {
            String^ str = "Matrix4(";
            for (size_t i = 0; i < 4; ++i)
            {
                str += " row" + (unsigned)i + "{";
                for(size_t j = 0; j < 4; ++j)
                {
                    str += this[i, j] + " ";
                }
                str += "}";
            }
            str += ")";
            return str;
        }

        Matrix4^ Adjoint();
        property Real Determinant
        {
            Real get();
        }
        Matrix4^ Inverse();

        /** <summary>Building a Matrix4 from orientation / scale / position.</summary>
        <remarks>
        Transform is performed in the order scale, rotate, translation, i.e. translation is independent
        of orientation axes, scale does not affect size of translation, rotation and scaling are always
        centered on the origin.
        </remarks>
        */
        void MakeTransform(Vector3 position, Vector3 scale, Quaternion orientation);

        /** <summary>Building an inverse Matrix4 from orientation / scale / position.</summary>
        <remarks>
        As makeTransform except it build the inverse given the same data as makeTransform, so
        performing -translation, -rotate, 1/scale in that order.
        </remarks>
        */
        void MakeInverseTransform(Vector3 position, Vector3 scale, Quaternion orientation);

        /** <summary>Decompose a Matrix4 to orientation / scale / position.</summary>
        */
        void Decomposition([Out] Vector3% position, [Out] Vector3% scale, [Out] Quaternion% orientation);

        /** <summary>Check whether or not the matrix is affine matrix.</summary>
        <remarks>
        An affine matrix is a 4x4 matrix with row 3 equal to (0, 0, 0, 1),
        e.g. no projective coefficients.
        </remarks>
        */
        property bool IsAffine
        {
            inline bool get() { return m30 == 0 && m31 == 0 && m32 == 0 && m33 == 1; }
        }

        /** <summary>Returns the inverse of the affine matrix.</summary>
        <remarks>
        <note>
        The matrix must be an affine matrix. <see cref="IsAffine"/>.
        </note>
        </remarks>
        */
        Matrix4^ InverseAffine();

        /** <summary>Concatenate two affine matrix.</summary>
        <remarks>
        <note>
        The matrices must be affine matrix. <see cref="IsAffine"/>.
        </note>
        </remarks>
        */
        Matrix4^ ConcatenateAffine(Matrix4^ m2)
        {
            if (!IsAffine || !m2->IsAffine)
                throw gcnew Exception("This matrix and m2 matrix must be affine");

            return gcnew Matrix4(
                m00 * m2->m00 + m01 * m2->m10 + m02 * m2->m20,
                m00 * m2->m01 + m01 * m2->m11 + m02 * m2->m21,
                m00 * m2->m02 + m01 * m2->m12 + m02 * m2->m22,
                m00 * m2->m03 + m01 * m2->m13 + m02 * m2->m23 + m03,

                m10 * m2->m00 + m11 * m2->m10 + m12 * m2->m20,
                m10 * m2->m01 + m11 * m2->m11 + m12 * m2->m21,
                m10 * m2->m02 + m11 * m2->m12 + m12 * m2->m22,
                m10 * m2->m03 + m11 * m2->m13 + m12 * m2->m23 + m13,

                m20 * m2->m00 + m21 * m2->m10 + m22 * m2->m20,
                m20 * m2->m01 + m21 * m2->m11 + m22 * m2->m21,
                m20 * m2->m02 + m21 * m2->m12 + m22 * m2->m22,
                m20 * m2->m03 + m21 * m2->m13 + m22 * m2->m23 + m23,

                0, 0, 0, 1);
        }

        /** <summary>3-D Vector transformation specially for affine matrix.</summary>
        <remarks>
        Transforms the given 3-D vector by the matrix, projecting the 
        result back into <i>w</i> = 1.
        <note>
        The matrix must be an affine matrix. <see cref="IsAffine"/>.
        </note>
        </remarks>
        */
        inline Vector3 TransformAffine(Vector3 v)
        {
            if (!IsAffine)
                throw gcnew Exception("This matrix must be affine");

            return Vector3(
                m00 * v.x + m01 * v.y + m02 * v.z + m03, 
                m10 * v.x + m11 * v.y + m12 * v.z + m13,
                m20 * v.x + m21 * v.y + m22 * v.z + m23);
        }

        /** <summary>4-D Vector transformation specially for affine matrix.</summary>
        <remarks>
        <note>
        The matrix must be an affine matrix. <see cref="IsAffine"/>.
        </note>
        </remarks>
        */
        inline Vector4 TransformAffine(Vector4 v)
        {
            if (!IsAffine)
                throw gcnew Exception("This matrix must be affine");

            return Vector4(
                m00 * v.x + m01 * v.y + m02 * v.z + m03 * v.w, 
                m10 * v.x + m11 * v.y + m12 * v.z + m13 * v.w,
                m20 * v.x + m21 * v.y + m22 * v.z + m23 * v.w,
                v.w);
        }

        /* operators need to be static members to get recognized by the CLR
        */
        inline static Vector4 operator * (Vector4 v, Matrix4^ mat)
        {
            return Vector4(
                v.x*mat->m00 + v.y*mat->m10 + v.z*mat->m20 + v.w*mat->m30,
                v.x*mat->m01 + v.y*mat->m11 + v.z*mat->m21 + v.w*mat->m31,
                v.x*mat->m02 + v.y*mat->m12 + v.z*mat->m22 + v.w*mat->m32,
                v.x*mat->m03 + v.y*mat->m13 + v.z*mat->m23 + v.w*mat->m33
                );
        }
    };
}