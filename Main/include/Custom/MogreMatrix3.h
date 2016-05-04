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
// NB All code adapted from Wild Magic 0.2 Matrix math (free source code)
// http://www.geometrictools.com/

// NOTE.  The (x,y,z) coordinate system is assumed to be right-handed.
// Coordinate axis rotation matrices are of the form
//   RX =    1       0       0
//           0     cos(t) -sin(t)
//           0     sin(t)  cos(t)
// where t > 0 indicates a counterclockwise rotation in the yz-plane
//   RY =  cos(t)    0     sin(t)
//           0       1       0
//        -sin(t)    0     cos(t)
// where t > 0 indicates a counterclockwise rotation in the zx-plane
//   RZ =  cos(t) -sin(t)    0
//         sin(t)  cos(t)    0
//           0       0       1
// where t > 0 indicates a counterclockwise rotation in the xy-plane.

#pragma once

#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgreMatrix3.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"
#include "Custom\MogreVector3.h"

namespace Mogre
{
    /** <summary>A 3x3 matrix which can represent rotations around axes.</summary>
    <remarks>
    <note>
    <b>All the code is adapted from the Wild Magic 0.2 Matrix
    library (http://www.geometrictools.com/).</b>
    </note>
    <para>
    The coordinate system is assumed to be <b>right-handed</b>.
    </para>
    </remarks>
    */
    [Serializable]
    [StructLayout(LayoutKind::Explicit)]
    public ref class Matrix3
    {
    public:
        [Serializable]
        [StructLayout(LayoutKind::Sequential)]
        value struct NativeValue
        {
            Real m00, m01, m02;
            Real m10, m11, m12;
            Real m20, m21, m22;

            property Real default[int,int]
            {
                inline Real get(int row, int col)
                {
                    return *(&m00 + ((3*row) + col)); 
                }

                inline void set (int row, int col, Real value)
                {
                    *(&m00 + ((3*row) + col)) = value;
                }
            }
        };

        static operator Matrix3^ (const Ogre::Matrix3& matrix)
        {
            Matrix3^ mat = gcnew Matrix3;
            pin_ptr<Real> pdest = &mat->m00;
            memcpy(pdest,matrix[0],9*sizeof(Real));
            return mat;
        }

    public:
        inline Matrix3 () {};
        inline explicit Matrix3 (array<Real,2>^ arr)
        {
            pin_ptr<Real> pdest = &m00;
            pin_ptr<Real> psrc = &arr[0,0];
            memcpy(pdest,psrc,9*sizeof(Real));
        }
        inline Matrix3 (Matrix3^ rkMatrix)
        {
            pin_ptr<Real> pdest = &m00;
            pin_ptr<Real> psrc = &rkMatrix->m00;
            memcpy(pdest,psrc,9*sizeof(Real));
        }
        Matrix3 (Real fEntry00, Real fEntry01, Real fEntry02,
            Real fEntry10, Real fEntry11, Real fEntry12,
            Real fEntry20, Real fEntry21, Real fEntry22)
        {
            m00 = fEntry00;
            m01 = fEntry01;
            m02 = fEntry02;
            m10 = fEntry10;
            m11 = fEntry11;
            m12 = fEntry12;
            m20 = fEntry20;
            m21 = fEntry21;
            m22 = fEntry22;
        }

        property Real default[int,int]
        {
            inline Real get(int row, int col)
            {
                return *(&m00 + ((3*row) + col)); 
            }

            inline void set (int row, int col, Real value)
            {
                *(&m00 + ((3*row) + col)) = value;
            }
        }

        Vector3 GetColumn (size_t iCol);
        void SetColumn(size_t iCol, Vector3 vec);
        void FromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis);

        // assignment and comparison
        inline Matrix3^ operator= (Matrix3^ rkMatrix)
        {
            pin_ptr<Real> pdest = &m00;
            pin_ptr<Real> psrc = &rkMatrix->m00;
            memcpy(pdest,psrc,9*sizeof(Real));
            return this;
        }
        static bool operator== (Matrix3^ lmat, Matrix3^ rkMatrix);
        inline static bool operator!= (Matrix3^ lmat, Matrix3^ rkMatrix)
        {
            return !(lmat == rkMatrix);
        }

        virtual bool Equals(Object^ obj) override
        {
            Matrix3^ clr = dynamic_cast<Matrix3^>(obj);
            if (clr == CLR_NULL)
            {
                return false;
            }

            return (this == clr);
        }

        bool Equals(Matrix3^ obj)
        {
            return (this == obj);
        }

        // arithmetic operations
        static Matrix3^ operator+ (Matrix3^ lmat, Matrix3^ rkMatrix);
        static Matrix3^ operator- (Matrix3^ lmat, Matrix3^ rkMatrix);
        static Matrix3^ operator* (Matrix3^ lmat, Matrix3^ rkMatrix);
        static Matrix3^ operator- (Matrix3^ mat);

        // matrix * vector [3x3 * 3x1 = 3x1]
        static Vector3 operator* (Matrix3^ lmat, Vector3 rkVector);

        // vector * matrix [1x3 * 3x3 = 1x3]
        static Vector3 operator* (Vector3 rkVector,
            Matrix3^ rkMatrix);

        // matrix * scalar
        static Matrix3^ operator* (Matrix3^ lmat, Real fScalar);

        // scalar * matrix
        static Matrix3^ operator* (Real fScalar, Matrix3^ rkMatrix);

        // utilities
        Matrix3^ Transpose();
        bool Inverse ([Out] Matrix3^% rkInverse, Real fTolerance);
        bool Inverse ([Out] Matrix3^% rkInverse)
        {
            return Inverse(rkInverse, 1e-06);
        }
        Matrix3^ Inverse (Real fTolerance);
        Matrix3^ Inverse ()
        {
            return Inverse(1e-06);
        }
        property Real Determinant
        {
            Real get();
        }

        // singular value decomposition
        void SingularValueDecomposition ([Out] Matrix3^% rkL, [Out] Vector3% rkS,
            [Out] Matrix3^% rkR);
        void SingularValueComposition (Matrix3^ rkL,
            Vector3 rkS, Matrix3^ rkR);

        /// <summary>Gram-Schmidt orthonormalization (applied to columns of rotation matrix)</summary>
        void Orthonormalize ();

        /// <summary>Orthogonal Q, diagonal D, upper triangular U stored as (u01,u02,u12)</summary>
        void QDUDecomposition ([Out] Matrix3^% rkQ, [Out] Vector3% rkD,
            [Out] Vector3% rkU);

        property Real SpectralNorm
        {
            Real get();
        }

        // matrix must be orthonormal
        void ToAngleAxis ([Out] Vector3% rkAxis, [Out] Radian% rfAngle);
        inline void ToAngleAxis ([Out] Vector3% rkAxis, [Out] Degree% rfAngle) {
            Radian r;
            ToAngleAxis ( rkAxis, r );
            rfAngle = r;
        }
        void FromAngleAxis (Vector3 rkAxis, Radian fRadians);

        // The matrix must be orthonormal.  The decomposition is yaw*pitch*roll
        // where yaw is rotation about the Up vector, pitch is rotation about the
        // Right axis, and roll is rotation about the Direction axis.
        bool ToEulerAnglesXYZ ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        bool ToEulerAnglesXZY ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        bool ToEulerAnglesYXZ ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        bool ToEulerAnglesYZX ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        bool ToEulerAnglesZXY ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        bool ToEulerAnglesZYX ([Out] Radian% rfYAngle, [Out] Radian% rfPAngle,
            [Out] Radian% rfRAngle);
        void FromEulerAnglesXYZ (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        void FromEulerAnglesXZY (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        void FromEulerAnglesYXZ (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        void FromEulerAnglesYZX (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        void FromEulerAnglesZXY (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        void FromEulerAnglesZYX (Radian fYAngle, Radian fPAngle, Radian fRAngle);
        /// <summary>Eigensolver, matrix must be symmetric</summary>
        void EigenSolveSymmetric (array<Real>^ afEigenvalue,
            array<Vector3>^ akEigenvector);

        static void TensorProduct (Vector3 rkU, Vector3 rkV,
            [Out] Matrix3^% rkProduct);

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

        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("Matrix3({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                m00, m01, m02, m10, m11, m12, m20, m21, m22);
        }

        static initonly Real EPSILON = 1e-06;
        static initonly Matrix3^ ZERO = gcnew Matrix3(0,0,0,0,0,0,0,0,0);
        static initonly Matrix3^ IDENTITY = gcnew Matrix3(1,0,0,0,1,0,0,0,1);

        [FieldOffset(0)]
        NativeValue value;

        [FieldOffset(0)]
        Real m00;
        [FieldOffset(1*sizeof(Real))]
        Real m01;
        [FieldOffset(2*sizeof(Real))]
        Real m02;
        [FieldOffset(3*sizeof(Real))]
        Real m10;
        [FieldOffset(4*sizeof(Real))]
        Real m11;
        [FieldOffset(5*sizeof(Real))]
        Real m12;
        [FieldOffset(6*sizeof(Real))]
        Real m20;
        [FieldOffset(7*sizeof(Real))]
        Real m21;
        [FieldOffset(8*sizeof(Real))]
        Real m22;

    protected public:
        // support for eigensolver
        void Tridiagonal (Real afDiag[3], Real afSubDiag[3]);
        bool QLAlgorithm (Real afDiag[3], Real afSubDiag[3]);

        // support for singular value decomposition
        static initonly Real msSvdEpsilon = 1e-04;
        static initonly unsigned int msSvdMaxIterations = 32;
        static void Bidiagonalize (Matrix3^ kA, Matrix3^ kL,
            Matrix3^ kR);
        static void GolubKahanStep (Matrix3^ kA, Matrix3^ kL,
            Matrix3^ kR);

        // support for spectral norm
        static Real MaxCubicRoot (Real afCoeff[3]);
    };
}