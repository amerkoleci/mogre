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

#include "Custom\MogreMatrix3.h"

namespace Mogre
{
    //-----------------------------------------------------------------------
    Vector3 Matrix3::GetColumn (size_t iCol)
    {
        assert( iCol < 3 );
        interior_ptr<Real> ptr = &m00;
        return Vector3(*(ptr + iCol), *(ptr + 3 + iCol), *(ptr + 6 + iCol));
    }
    //-----------------------------------------------------------------------
    void Matrix3::SetColumn(size_t iCol, Vector3 vec)
    {
        assert( iCol < 3 );
        interior_ptr<Real> ptr = &m00;
        *(ptr + iCol) = vec.x;
        *(ptr + 3 + iCol) = vec.y;
        *(ptr + 6 + iCol) = vec.z;

    }
    //-----------------------------------------------------------------------
    void Matrix3::FromAxes(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
    {
        SetColumn(0,xAxis);
        SetColumn(1,yAxis);
        SetColumn(2,zAxis);

    }

    //-----------------------------------------------------------------------
    bool Matrix3::operator== (Matrix3^ left, Matrix3^ right)
    {
        if ((Object^)left == (Object^)right) return true;
        if ((Object^)left == nullptr || (Object^)right == nullptr) return false;

        if (
            left->m00 == right->m00 && left->m01 == right->m01 && left->m02 == right->m02 &&
            left->m10 == right->m10 && left->m11 == right->m11 && left->m12 == right->m12 &&
            left->m20 == right->m20 && left->m21 == right->m21 && left->m22 == right->m22) {

                return true;
        }

        return false;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator+ (Matrix3^ lmat, Matrix3^ rkMatrix)
    {
        Matrix3^ kSum = gcnew Matrix3;
        for (size_t iRow = 0; iRow < 3; iRow++)
        {
            for (size_t iCol = 0; iCol < 3; iCol++)
            {
                kSum[iRow,iCol] = lmat[iRow,iCol] +
                    rkMatrix[iRow,iCol];
            }
        }
        return kSum;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator- (Matrix3^ lmat, Matrix3^ rkMatrix)
    {
        Matrix3^ kDiff = gcnew Matrix3;
        for (size_t iRow = 0; iRow < 3; iRow++)
        {
            for (size_t iCol = 0; iCol < 3; iCol++)
            {
                kDiff[iRow,iCol] = lmat[iRow,iCol] -
                    rkMatrix[iRow,iCol];
            }
        }
        return kDiff;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator* (Matrix3^ left, Matrix3^ right)
    {
        Matrix3^ result = gcnew Matrix3();

        result->m00 = left->m00 * right->m00 + left->m01 * right->m10 + left->m02 * right->m20;
        result->m01 = left->m00 * right->m01 + left->m01 * right->m11 + left->m02 * right->m21;
        result->m02 = left->m00 * right->m02 + left->m01 * right->m12 + left->m02 * right->m22;

        result->m10 = left->m10 * right->m00 + left->m11 * right->m10 + left->m12 * right->m20;
        result->m11 = left->m10 * right->m01 + left->m11 * right->m11 + left->m12 * right->m21;
        result->m12 = left->m10 * right->m02 + left->m11 * right->m12 + left->m12 * right->m22;

        result->m20 = left->m20 * right->m00 + left->m21 * right->m10 + left->m22 * right->m20;
        result->m21 = left->m20 * right->m01 + left->m21 * right->m11 + left->m22 * right->m21;
        result->m22 = left->m20 * right->m02 + left->m21 * right->m12 + left->m22 * right->m22;

        return result;
    }
    //-----------------------------------------------------------------------
    Vector3 Matrix3::operator* (Matrix3^ matrix, Vector3 vector)
    {
        Vector3 product;

        product.x = matrix->m00 * vector.x + matrix->m01 * vector.y + matrix->m02 * vector.z;
        product.y = matrix->m10 * vector.x + matrix->m11 * vector.y + matrix->m12 * vector.z;
        product.z = matrix->m20 * vector.x + matrix->m21 * vector.y + matrix->m22 * vector.z;

        return product;
    }
    //-----------------------------------------------------------------------
    Vector3 Matrix3::operator* (Vector3 vector, Matrix3^ matrix)
    {
        Vector3 product;

        product.x = vector.x * matrix->m00 + vector.y * matrix->m10 + vector.z * matrix->m20;
        product.y = vector.x * matrix->m01 + vector.y * matrix->m11 + vector.z * matrix->m21;
        product.z = vector.x * matrix->m02 + vector.y * matrix->m12 + vector.z * matrix->m22;

        return product;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator- (Matrix3^ matrix)
    {
        Matrix3^ result = gcnew Matrix3();

        result->m00 = -matrix->m00;
        result->m01 = -matrix->m01;
        result->m02 = -matrix->m02;
        result->m10 = -matrix->m10;
        result->m11 = -matrix->m11;
        result->m12 = -matrix->m12;
        result->m20 = -matrix->m20;
        result->m21 = -matrix->m21;
        result->m22 = -matrix->m22;

        return result;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator* (Matrix3^ matrix, Real scalar)
    {
        Matrix3^ result = gcnew Matrix3();

        result->m00 = matrix->m00 * scalar;
        result->m01 = matrix->m01 * scalar;
        result->m02 = matrix->m02 * scalar;
        result->m10 = matrix->m10 * scalar;
        result->m11 = matrix->m11 * scalar;
        result->m12 = matrix->m12 * scalar;
        result->m20 = matrix->m20 * scalar;
        result->m21 = matrix->m21 * scalar;
        result->m22 = matrix->m22 * scalar;

        return result;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::operator* (Real scalar, Matrix3^ matrix)
    {
        Matrix3^ result = gcnew Matrix3();

        result->m00 = matrix->m00 * scalar;
        result->m01 = matrix->m01 * scalar;
        result->m02 = matrix->m02 * scalar;
        result->m10 = matrix->m10 * scalar;
        result->m11 = matrix->m11 * scalar;
        result->m12 = matrix->m12 * scalar;
        result->m20 = matrix->m20 * scalar;
        result->m21 = matrix->m21 * scalar;
        result->m22 = matrix->m22 * scalar;

        return result;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::Transpose ()
    {
        return gcnew Matrix3(m00, m10, m20,
            m01, m11, m21,
            m02, m12, m22);
    }
    //-----------------------------------------------------------------------
    bool Matrix3::Inverse (Matrix3^% rkInverse, Real fTolerance)
    {
        // Invert a 3x3 using cofactors.  This is about 8 times faster than
        // the Numerical Recipes code which uses Gaussian elimination.

        rkInverse = gcnew Matrix3;

        rkInverse->m00 = m11*m22 -
            m12*m21;
        rkInverse->m01 = m02*m21 -
            m01*m22;
        rkInverse->m02 = m01*m12 -
            m02*m11;
        rkInverse->m10 = m12*m20 -
            m10*m22;
        rkInverse->m11 = m00*m22 -
            m02*m20;
        rkInverse->m12 = m02*m10 -
            m00*m12;
        rkInverse->m20 = m10*m21 -
            m11*m20;
        rkInverse->m21 = m01*m20 -
            m00*m21;
        rkInverse->m22 = m00*m11 -
            m01*m10;

        Real fDet =
            m00*rkInverse->m00 +
            m01*rkInverse->m10+
            m02*rkInverse->m20;

        if ( System::Math::Abs(fDet) <= fTolerance )
            return false;

        Real fInvDet = 1.0f/fDet;
        for (size_t iRow = 0; iRow < 3; iRow++)
        {
            for (size_t iCol = 0; iCol < 3; iCol++)
                rkInverse[iRow, iCol] *= fInvDet;
        }

        return true;
    }
    //-----------------------------------------------------------------------
    Matrix3^ Matrix3::Inverse (Real fTolerance)
    {
        Matrix3^ kInverse;
        Inverse(kInverse,fTolerance);
        return kInverse;
    }
    //-----------------------------------------------------------------------
    Real Matrix3::Determinant::get()
    {
        Real fCofactor00 = m11*m22 -
            m12*m21;
        Real fCofactor10 = m12*m20 -
            m10*m22;
        Real fCofactor20 = m10*m21 -
            m11*m20;

        Real fDet =
            m00*fCofactor00 +
            m01*fCofactor10 +
            m02*fCofactor20;

        return fDet;
    }
    //-----------------------------------------------------------------------
    void Matrix3::Bidiagonalize (Matrix3^ kA, Matrix3^ kL,
        Matrix3^ kR)
    {
        Real afV[3], afW[3];
        Real fLength, fSign, fT1, fInvT1, fT2;
        bool bIdentity;

        // map first column to (*,0,0)
        fLength = System::Math::Sqrt(kA->m00*kA->m00 + kA->m10*kA->m10 +
            kA->m20*kA->m20);
        if ( fLength > 0.0 )
        {
            fSign = (kA->m00 > 0.0f ? 1.0f : -1.0f);
            fT1 = kA->m00 + fSign*fLength;
            fInvT1 = 1.0f/fT1;
            afV[1] = kA->m10*fInvT1;
            afV[2] = kA->m20*fInvT1;

            fT2 = -2.0f/(1.0f+afV[1]*afV[1]+afV[2]*afV[2]);
            afW[0] = fT2*(kA->m00+kA->m10*afV[1]+kA->m20*afV[2]);
            afW[1] = fT2*(kA->m01+kA->m11*afV[1]+kA->m21*afV[2]);
            afW[2] = fT2*(kA->m02+kA->m12*afV[1]+kA->m22*afV[2]);
            kA->m00 += afW[0];
            kA->m01 += afW[1];
            kA->m02 += afW[2];
            kA->m11 += afV[1]*afW[1];
            kA->m12 += afV[1]*afW[2];
            kA->m21 += afV[2]*afW[1];
            kA->m22 += afV[2]*afW[2];

            kL->m00 = 1.0f+fT2;
            kL->m01 = kL->m10 = fT2*afV[1];
            kL->m02 = kL->m20 = fT2*afV[2];
            kL->m11 = 1.0f+fT2*afV[1]*afV[1];
            kL->m12 = kL->m21 = fT2*afV[1]*afV[2];
            kL->m22 = 1.0f+fT2*afV[2]*afV[2];
            bIdentity = false;
        }
        else
        {
            kL = Matrix3::IDENTITY;
            bIdentity = true;
        }

        // map first row to (*,*,0)
        fLength = System::Math::Sqrt(kA->m01*kA->m01+kA->m02*kA->m02);
        if ( fLength > 0.0 )
        {
            fSign = (kA->m01 > 0.0f ? 1.0f : -1.0f);
            fT1 = kA->m01 + fSign*fLength;
            afV[2] = kA->m02/fT1;

            fT2 = -2.0f/(1.0f+afV[2]*afV[2]);
            afW[0] = fT2*(kA->m01+kA->m02*afV[2]);
            afW[1] = fT2*(kA->m11+kA->m12*afV[2]);
            afW[2] = fT2*(kA->m21+kA->m22*afV[2]);
            kA->m01 += afW[0];
            kA->m11 += afW[1];
            kA->m12 += afW[1]*afV[2];
            kA->m21 += afW[2];
            kA->m22 += afW[2]*afV[2];

            kR->m00 = 1.0;
            kR->m01 = kR->m10 = 0.0;
            kR->m02 = kR->m20 = 0.0;
            kR->m11 = 1.0f+fT2;
            kR->m12 = kR->m21 = fT2*afV[2];
            kR->m22 = 1.0f+fT2*afV[2]*afV[2];
        }
        else
        {
            kR = Matrix3::IDENTITY;
        }

        // map second column to (*,*,0)
        fLength = System::Math::Sqrt(kA->m11*kA->m11+kA->m21*kA->m21);
        if ( fLength > 0.0 )
        {
            fSign = (kA->m11 > 0.0f ? 1.0f : -1.0f);
            fT1 = kA->m11 + fSign*fLength;
            afV[2] = kA->m21/fT1;

            fT2 = -2.0f/(1.0f+afV[2]*afV[2]);
            afW[1] = fT2*(kA->m11+kA->m21*afV[2]);
            afW[2] = fT2*(kA->m12+kA->m22*afV[2]);
            kA->m11 += afW[1];
            kA->m12 += afW[2];
            kA->m22 += afV[2]*afW[2];

            Real fA = 1.0f+fT2;
            Real fB = fT2*afV[2];
            Real fC = 1.0f+fB*afV[2];

            if ( bIdentity )
            {
                kL->m00 = 1.0;
                kL->m01 = kL->m10 = 0.0;
                kL->m02 = kL->m20 = 0.0;
                kL->m11 = fA;
                kL->m12 = kL->m21 = fB;
                kL->m22 = fC;
            }
            else
            {
                for (int iRow = 0; iRow < 3; iRow++)
                {
                    Real fTmp0 = kL[iRow, 1];
                    Real fTmp1 = kL[iRow, 2];
                    kL[iRow, 1] = fA*fTmp0+fB*fTmp1;
                    kL[iRow, 2] = fB*fTmp0+fC*fTmp1;
                }
            }
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::GolubKahanStep (Matrix3^ kA, Matrix3^ kL,
        Matrix3^ kR)
    {
        Real fT11 = kA->m01*kA->m01+kA->m11*kA->m11;
        Real fT22 = kA->m12*kA->m12+kA->m22*kA->m22;
        Real fT12 = kA->m11*kA->m12;
        Real fTrace = fT11+fT22;
        Real fDiff = fT11-fT22;
        Real fDiscr = System::Math::Sqrt(fDiff*fDiff+4.0f*fT12*fT12);
        Real fRoot1 = 0.5f*(fTrace+fDiscr);
        Real fRoot2 = 0.5f*(fTrace-fDiscr);

        // adjust right
        Real fY = kA->m00 - (System::Math::Abs(fRoot1-fT22) <=
            System::Math::Abs(fRoot2-fT22) ? fRoot1 : fRoot2);
        Real fZ = kA->m01;
        Real fInvLength = Math::InvSqrt(fY*fY+fZ*fZ);
        Real fSin = fZ*fInvLength;
        Real fCos = -fY*fInvLength;

        Real fTmp0 = kA->m00;
        Real fTmp1 = kA->m01;
        kA->m00 = fCos*fTmp0-fSin*fTmp1;
        kA->m01 = fSin*fTmp0+fCos*fTmp1;
        kA->m10 = -fSin*kA->m11;
        kA->m11 *= fCos;

        size_t iRow;
        for (iRow = 0; iRow < 3; iRow++)
        {
            fTmp0 = kR[0, iRow];
            fTmp1 = kR[1, iRow];
            kR[0, iRow] = fCos*fTmp0-fSin*fTmp1;
            kR[1, iRow] = fSin*fTmp0+fCos*fTmp1;
        }

        // adjust left
        fY = kA->m00;
        fZ = kA->m10;
        fInvLength = Math::InvSqrt(fY*fY+fZ*fZ);
        fSin = fZ*fInvLength;
        fCos = -fY*fInvLength;

        kA->m00 = fCos*kA->m00-fSin*kA->m10;
        fTmp0 = kA->m01;
        fTmp1 = kA->m11;
        kA->m01 = fCos*fTmp0-fSin*fTmp1;
        kA->m11 = fSin*fTmp0+fCos*fTmp1;
        kA->m02 = -fSin*kA->m12;
        kA->m12 *= fCos;

        size_t iCol;
        for (iCol = 0; iCol < 3; iCol++)
        {
            fTmp0 = kL[iCol, 0];
            fTmp1 = kL[iCol, 1];
            kL[iCol, 0] = fCos*fTmp0-fSin*fTmp1;
            kL[iCol, 1] = fSin*fTmp0+fCos*fTmp1;
        }

        // adjust right
        fY = kA->m01;
        fZ = kA->m02;
        fInvLength = Math::InvSqrt(fY*fY+fZ*fZ);
        fSin = fZ*fInvLength;
        fCos = -fY*fInvLength;

        kA->m01 = fCos*kA->m01-fSin*kA->m02;
        fTmp0 = kA->m11;
        fTmp1 = kA->m12;
        kA->m11 = fCos*fTmp0-fSin*fTmp1;
        kA->m12 = fSin*fTmp0+fCos*fTmp1;
        kA->m21 = -fSin*kA->m22;
        kA->m22 *= fCos;

        for (iRow = 0; iRow < 3; iRow++)
        {
            fTmp0 = kR[1, iRow];
            fTmp1 = kR[2, iRow];
            kR[1, iRow] = fCos*fTmp0-fSin*fTmp1;
            kR[2, iRow] = fSin*fTmp0+fCos*fTmp1;
        }

        // adjust left
        fY = kA->m11;
        fZ = kA->m21;
        fInvLength = Math::InvSqrt(fY*fY+fZ*fZ);
        fSin = fZ*fInvLength;
        fCos = -fY*fInvLength;

        kA->m11 = fCos*kA->m11-fSin*kA->m21;
        fTmp0 = kA->m12;
        fTmp1 = kA->m22;
        kA->m12 = fCos*fTmp0-fSin*fTmp1;
        kA->m22 = fSin*fTmp0+fCos*fTmp1;

        for (iCol = 0; iCol < 3; iCol++)
        {
            fTmp0 = kL[iCol, 1];
            fTmp1 = kL[iCol, 2];
            kL[iCol, 1] = fCos*fTmp0-fSin*fTmp1;
            kL[iCol, 2] = fSin*fTmp0+fCos*fTmp1;
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::SingularValueDecomposition (Matrix3^% kL, Vector3% kS,
        Matrix3^% kR)
    {
        // temas: currently unused
        //const int iMax = 16;
        size_t iRow, iCol;

        kL = gcnew Matrix3;
        kR = gcnew Matrix3;

        Matrix3^ kA = this;
        Bidiagonalize(kA,kL,kR);

        for (unsigned int i = 0; i < msSvdMaxIterations; i++)
        {
            Real fTmp, fTmp0, fTmp1;
            Real fSin0, fCos0, fTan0;
            Real fSin1, fCos1, fTan1;

            bool bTest1 = (System::Math::Abs(kA->m01) <=
                msSvdEpsilon*(System::Math::Abs(kA->m00)+System::Math::Abs(kA->m11)));
            bool bTest2 = (System::Math::Abs(kA->m12) <=
                msSvdEpsilon*(System::Math::Abs(kA->m11)+System::Math::Abs(kA->m22)));
            if ( bTest1 )
            {
                if ( bTest2 )
                {
                    kS[0] = kA->m00;
                    kS[1] = kA->m11;
                    kS[2] = kA->m22;
                    break;
                }
                else
                {
                    // 2x2 closed form factorization
                    fTmp = (kA->m11*kA->m11 - kA->m22*kA->m22 +
                        kA->m12*kA->m12)/(kA->m12*kA->m22);
                    fTan0 = 0.5f*(fTmp+System::Math::Sqrt(fTmp*fTmp + 4.0f));
                    fCos0 = Math::InvSqrt(1.0f+fTan0*fTan0);
                    fSin0 = fTan0*fCos0;

                    for (iCol = 0; iCol < 3; iCol++)
                    {
                        fTmp0 = kL[iCol, 1];
                        fTmp1 = kL[iCol, 2];
                        kL[iCol, 1] = fCos0*fTmp0-fSin0*fTmp1;
                        kL[iCol, 2] = fSin0*fTmp0+fCos0*fTmp1;
                    }

                    fTan1 = (kA->m12-kA->m22*fTan0)/kA->m11;
                    fCos1 = Math::InvSqrt(1.0f+fTan1*fTan1);
                    fSin1 = -fTan1*fCos1;

                    for (iRow = 0; iRow < 3; iRow++)
                    {
                        fTmp0 = kR[1, iRow];
                        fTmp1 = kR[2, iRow];
                        kR[1, iRow] = fCos1*fTmp0-fSin1*fTmp1;
                        kR[2, iRow] = fSin1*fTmp0+fCos1*fTmp1;
                    }

                    kS[0] = kA->m00;
                    kS[1] = fCos0*fCos1*kA->m11 -
                        fSin1*(fCos0*kA->m12-fSin0*kA->m22);
                    kS[2] = fSin0*fSin1*kA->m11 +
                        fCos1*(fSin0*kA->m12+fCos0*kA->m22);
                    break;
                }
            }
            else
            {
                if ( bTest2 )
                {
                    // 2x2 closed form factorization
                    fTmp = (kA->m00*kA->m00 + kA->m11*kA->m11 -
                        kA->m01*kA->m01)/(kA->m01*kA->m11);
                    fTan0 = 0.5f*(-fTmp+System::Math::Sqrt(fTmp*fTmp + 4.0f));
                    fCos0 = Math::InvSqrt(1.0f+fTan0*fTan0);
                    fSin0 = fTan0*fCos0;

                    for (iCol = 0; iCol < 3; iCol++)
                    {
                        fTmp0 = kL[iCol, 0];
                        fTmp1 = kL[iCol, 1];
                        kL[iCol, 0] = fCos0*fTmp0-fSin0*fTmp1;
                        kL[iCol, 1] = fSin0*fTmp0+fCos0*fTmp1;
                    }

                    fTan1 = (kA->m01-kA->m11*fTan0)/kA->m00;
                    fCos1 = Math::InvSqrt(1.0f+fTan1*fTan1);
                    fSin1 = -fTan1*fCos1;

                    for (iRow = 0; iRow < 3; iRow++)
                    {
                        fTmp0 = kR[0, iRow];
                        fTmp1 = kR[1, iRow];
                        kR[0, iRow] = fCos1*fTmp0-fSin1*fTmp1;
                        kR[1, iRow] = fSin1*fTmp0+fCos1*fTmp1;
                    }

                    kS[0] = fCos0*fCos1*kA->m00 -
                        fSin1*(fCos0*kA->m01-fSin0*kA->m11);
                    kS[1] = fSin0*fSin1*kA->m00 +
                        fCos1*(fSin0*kA->m01+fCos0*kA->m11);
                    kS[2] = kA->m22;
                    break;
                }
                else
                {
                    GolubKahanStep(kA,kL,kR);
                }
            }
        }

        // positize diagonal
        for (iRow = 0; iRow < 3; iRow++)
        {
            if ( kS[iRow] < 0.0 )
            {
                kS[iRow] = -kS[iRow];
                for (iCol = 0; iCol < 3; iCol++)
                    kR[iRow, iCol] = -kR[iRow, iCol];
            }
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::SingularValueComposition (Matrix3^ kL,
        Vector3 kS, Matrix3^ kR)
    {
        size_t iRow, iCol;
        Matrix3^ kTmp = gcnew Matrix3;

        // product S*R
        for (iRow = 0; iRow < 3; iRow++)
        {
            for (iCol = 0; iCol < 3; iCol++)
                kTmp[iRow, iCol] = kS[iRow]*kR[iRow, iCol];
        }

        // product L*S*R
        for (iRow = 0; iRow < 3; iRow++)
        {
            for (iCol = 0; iCol < 3; iCol++)
            {
                this[iRow, iCol] = 0.0;
                for (int iMid = 0; iMid < 3; iMid++)
                    this[iRow, iCol] += kL[iRow, iMid]*kTmp[iMid, iCol];
            }
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::Orthonormalize ()
    {
        // Algorithm uses Gram-Schmidt orthogonalization.  If 'this' matrix is
        // M = [m0|m1|m2], then orthonormal output matrix is Q = [q0|q1|q2],
        //
        //   q0 = m0/|m0|
        //   q1 = (m1-(q0*m1)q0)/|m1-(q0*m1)q0|
        //   q2 = (m2-(q0*m2)q0-(q1*m2)q1)/|m2-(q0*m2)q0-(q1*m2)q1|
        //
        // where |V| indicates length of vector V and A*B indicates dot
        // product of vectors A and B.

        // compute q0
        Real fInvLength = Math::InvSqrt(m00*m00
            + m10*m10 +
            m20*m20);

        m00 *= fInvLength;
        m10 *= fInvLength;
        m20 *= fInvLength;

        // compute q1
        Real fDot0 =
            m00*m01 +
            m10*m11 +
            m20*m21;

        m01 -= fDot0*m00;
        m11 -= fDot0*m10;
        m21 -= fDot0*m20;

        fInvLength = Math::InvSqrt(m01*m01 +
            m11*m11 +
            m21*m21);

        m01 *= fInvLength;
        m11 *= fInvLength;
        m21 *= fInvLength;

        // compute q2
        Real fDot1 =
            m01*m02 +
            m11*m12 +
            m21*m22;

        fDot0 =
            m00*m02 +
            m10*m12 +
            m20*m22;

        m02 -= fDot0*m00 + fDot1*m01;
        m12 -= fDot0*m10 + fDot1*m11;
        m22 -= fDot0*m20 + fDot1*m21;

        fInvLength = Math::InvSqrt(m02*m02 +
            m12*m12 +
            m22*m22);

        m02 *= fInvLength;
        m12 *= fInvLength;
        m22 *= fInvLength;
    }
    //-----------------------------------------------------------------------
    void Matrix3::QDUDecomposition (Matrix3^% kQ,
        Vector3% kD, Vector3% kU)
    {
        kQ = gcnew Matrix3;

        // Factor M = QR = QDU where Q is orthogonal, D is diagonal,
        // and U is upper triangular with ones on its diagonal.  Algorithm uses
        // Gram-Schmidt orthogonalization (the QR algorithm).
        //
        // If M = [ m0 | m1 | m2 ] and Q = [ q0 | q1 | q2 ], then
        //
        //   q0 = m0/|m0|
        //   q1 = (m1-(q0*m1)q0)/|m1-(q0*m1)q0|
        //   q2 = (m2-(q0*m2)q0-(q1*m2)q1)/|m2-(q0*m2)q0-(q1*m2)q1|
        //
        // where |V| indicates length of vector V and A*B indicates dot
        // product of vectors A and B.  The matrix R has entries
        //
        //   r00 = q0*m0  r01 = q0*m1  r02 = q0*m2
        //   r10 = 0      r11 = q1*m1  r12 = q1*m2
        //   r20 = 0      r21 = 0      r22 = q2*m2
        //
        // so D = diag(r00,r11,r22) and U has entries u01 = r01/r00,
        // u02 = r02/r00, and u12 = r12/r11.

        // Q = rotation
        // D = scaling
        // U = shear

        // D stores the three diagonal entries r00, r11, r22
        // U stores the entries U[0] = u01, U[1] = u02, U[2] = u12

        // build orthogonal matrix Q
        Real fInvLength = m00*m00 + m10*m10 + m20*m20;
        if (!Math::RealEqual(fInvLength,0)) fInvLength = Math::InvSqrt(fInvLength);

        kQ->m00 = m00*fInvLength;
        kQ->m10 = m10*fInvLength;
        kQ->m20 = m20*fInvLength;

        Real fDot = kQ->m00*m01 + kQ->m10*m11 +
            kQ->m20*m21;
        kQ->m01 = m01-fDot*kQ->m00;
        kQ->m11 = m11-fDot*kQ->m10;
        kQ->m21 = m21-fDot*kQ->m20;
        fInvLength = kQ->m01*kQ->m01 + kQ->m11*kQ->m11 + kQ->m21*kQ->m21;
        if (!Math::RealEqual(fInvLength,0)) fInvLength = Math::InvSqrt(fInvLength);

        kQ->m01 *= fInvLength;
        kQ->m11 *= fInvLength;
        kQ->m21 *= fInvLength;

        fDot = kQ->m00*m02 + kQ->m10*m12 +
            kQ->m20*m22;
        kQ->m02 = m02-fDot*kQ->m00;
        kQ->m12 = m12-fDot*kQ->m10;
        kQ->m22 = m22-fDot*kQ->m20;
        fDot = kQ->m01*m02 + kQ->m11*m12 +
            kQ->m21*m22;
        kQ->m02 -= fDot*kQ->m01;
        kQ->m12 -= fDot*kQ->m11;
        kQ->m22 -= fDot*kQ->m21;
        fInvLength = kQ->m02*kQ->m02 + kQ->m12*kQ->m12 + kQ->m22*kQ->m22;
        if (!Math::RealEqual(fInvLength,0)) fInvLength = Math::InvSqrt(fInvLength);

        kQ->m02 *= fInvLength;
        kQ->m12 *= fInvLength;
        kQ->m22 *= fInvLength;

        // guarantee that orthogonal matrix has determinant 1 (no reflections)
        Real fDet = kQ->m00*kQ->m11*kQ->m22 + kQ->m01*kQ->m12*kQ->m20 +
            kQ->m02*kQ->m10*kQ->m21 - kQ->m02*kQ->m11*kQ->m20 -
            kQ->m01*kQ->m10*kQ->m22 - kQ->m00*kQ->m12*kQ->m21;

        if ( fDet < 0.0 )
        {
            for (size_t iRow = 0; iRow < 3; iRow++)
                for (size_t iCol = 0; iCol < 3; iCol++)
                    kQ[iRow, iCol] = -kQ[iRow, iCol];
        }

        // build "right" matrix R
        Matrix3^ kR = gcnew Matrix3;
        kR->m00 = kQ->m00*m00 + kQ->m10*m10 +
            kQ->m20*m20;
        kR->m01 = kQ->m00*m01 + kQ->m10*m11 +
            kQ->m20*m21;
        kR->m11 = kQ->m01*m01 + kQ->m11*m11 +
            kQ->m21*m21;
        kR->m02 = kQ->m00*m02 + kQ->m10*m12 +
            kQ->m20*m22;
        kR->m12 = kQ->m01*m02 + kQ->m11*m12 +
            kQ->m21*m22;
        kR->m22 = kQ->m02*m02 + kQ->m12*m12 +
            kQ->m22*m22;

        // the scaling component
        kD[0] = kR->m00;
        kD[1] = kR->m11;
        kD[2] = kR->m22;

        // the shear component
        Real fInvD0 = 1.0f/kD[0];
        kU[0] = kR->m01*fInvD0;
        kU[1] = kR->m02*fInvD0;
        kU[2] = kR->m12/kD[1];
    }
    //-----------------------------------------------------------------------
    Real Matrix3::MaxCubicRoot (Real afCoeff[3])
    {
        // Spectral norm is for A^T*A, so characteristic polynomial
        // P(x) = c[0]+c[1]*x+c[2]*x^2+x^3 has three positive real roots.
        // This yields the assertions c[0] < 0 and c[2]*c[2] >= 3*c[1].

        // quick out for uniform scale (triple root)
        const Real fOneThird = 1.0/3.0;
        const Real fEpsilon = 1e-06;
        Real fDiscr = afCoeff[2]*afCoeff[2] - 3.0f*afCoeff[1];
        if ( fDiscr <= fEpsilon )
            return -fOneThird*afCoeff[2];

        // Compute an upper bound on roots of P(x).  This assumes that A^T*A
        // has been scaled by its largest entry.
        Real fX = 1.0;
        Real fPoly = afCoeff[0]+fX*(afCoeff[1]+fX*(afCoeff[2]+fX));
        if ( fPoly < 0.0 )
        {
            // uses a matrix norm to find an upper bound on maximum root
            fX = System::Math::Abs(afCoeff[0]);
            Real fTmp = 1.0f+System::Math::Abs(afCoeff[1]);
            if ( fTmp > fX )
                fX = fTmp;
            fTmp = 1.0f+System::Math::Abs(afCoeff[2]);
            if ( fTmp > fX )
                fX = fTmp;
        }

        // Newton's method to find root
        Real fTwoC2 = 2.0f*afCoeff[2];
        for (int i = 0; i < 16; i++)
        {
            fPoly = afCoeff[0]+fX*(afCoeff[1]+fX*(afCoeff[2]+fX));
            if ( System::Math::Abs(fPoly) <= fEpsilon )
                return fX;

            Real fDeriv = afCoeff[1]+fX*(fTwoC2+3.0f*fX);
            fX -= fPoly/fDeriv;
        }

        return fX;
    }
    //-----------------------------------------------------------------------
    Real Matrix3::SpectralNorm::get()
    {
        Matrix3^ kP = gcnew Matrix3;
        size_t iRow, iCol;
        Real fPmax = 0.0;
        for (iRow = 0; iRow < 3; iRow++)
        {
            for (iCol = 0; iCol < 3; iCol++)
            {
                kP[iRow, iCol] = 0.0;
                for (int iMid = 0; iMid < 3; iMid++)
                {
                    kP[iRow, iCol] +=
                        this[iMid, iRow]*this[iMid, iCol];
                }
                if ( kP[iRow, iCol] > fPmax )
                    fPmax = kP[iRow, iCol];
            }
        }

        Real fInvPmax = 1.0f/fPmax;
        for (iRow = 0; iRow < 3; iRow++)
        {
            for (iCol = 0; iCol < 3; iCol++)
                kP[iRow, iCol] *= fInvPmax;
        }

        Real afCoeff[3];
        afCoeff[0] = -(kP->m00*(kP->m11*kP->m22-kP->m12*kP->m21) +
            kP->m01*(kP->m20*kP->m12-kP->m10*kP->m22) +
            kP->m02*(kP->m10*kP->m21-kP->m20*kP->m11));
        afCoeff[1] = kP->m00*kP->m11-kP->m01*kP->m10 +
            kP->m00*kP->m22-kP->m02*kP->m20 +
            kP->m11*kP->m22-kP->m12*kP->m21;
        afCoeff[2] = -(kP->m00+kP->m11+kP->m22);

        Real fRoot = MaxCubicRoot(afCoeff);
        Real fNorm = System::Math::Sqrt(fPmax*fRoot);
        return fNorm;
    }
    //-----------------------------------------------------------------------
    void Matrix3::ToAngleAxis (Vector3% rkAxis, Radian% rfRadians)
    {
        // Let (x,y,z) be the unit-length axis and let A be an angle of rotation.
        // The rotation matrix is R = I + sin(A)*P + (1-cos(A))*P^2 where
        // I is the identity and
        //
        //       +-        -+
        //   P = |  0 -z +y |
        //       | +z  0 -x |
        //       | -y +x  0 |
        //       +-        -+
        //
        // If A > 0, R represents a counterclockwise rotation about the axis in
        // the sense of looking from the tip of the axis vector towards the
        // origin.  Some algebra will show that
        //
        //   cos(A) = (trace(R)-1)/2  and  R - R^t = 2*sin(A)*P
        //
        // In the event that A = pi, R-R^t = 0 which prevents us from extracting
        // the axis through P.  Instead note that R = I+2*P^2 when A = pi, so
        // P^2 = (R-I)/2.  The diagonal entries of P^2 are x^2-1, y^2-1, and
        // z^2-1.  We can solve these for axis (x,y,z).  Because the angle is pi,
        // it does not matter which sign you choose on the square roots.

        Real fTrace = m00 + m11 + m22;
        Real fCos = 0.5f*(fTrace-1.0f);
        rfRadians = Mogre::Math::ACos(fCos);  // in [0,PI]

        if ( rfRadians > Radian(0.0) )
        {
            if ( rfRadians < Radian(Math::PI) )
            {
                rkAxis.x = m21-m12;
                rkAxis.y = m02-m20;
                rkAxis.z = m10-m01;
                rkAxis.Normalise();
            }
            else
            {
                // angle is PI
                float fHalfInverse;
                if ( m00 >= m11 )
                {
                    // r00 >= r11
                    if ( m00 >= m22 )
                    {
                        // r00 is maximum diagonal term
                        rkAxis.x = 0.5f*System::Math::Sqrt(m00 -
                            m11 - m22 + 1.0f);
                        fHalfInverse = 0.5f/rkAxis.x;
                        rkAxis.y = fHalfInverse*m01;
                        rkAxis.z = fHalfInverse*m02;
                    }
                    else
                    {
                        // r22 is maximum diagonal term
                        rkAxis.z = 0.5f*System::Math::Sqrt(m22 -
                            m00 - m11 + 1.0f);
                        fHalfInverse = 0.5f/rkAxis.z;
                        rkAxis.x = fHalfInverse*m02;
                        rkAxis.y = fHalfInverse*m12;
                    }
                }
                else
                {
                    // r11 > r00
                    if ( m11 >= m22 )
                    {
                        // r11 is maximum diagonal term
                        rkAxis.y = 0.5f*System::Math::Sqrt(m11 -
                            m00 - m22 + 1.0f);
                        fHalfInverse  = 0.5f/rkAxis.y;
                        rkAxis.x = fHalfInverse*m01;
                        rkAxis.z = fHalfInverse*m12;
                    }
                    else
                    {
                        // r22 is maximum diagonal term
                        rkAxis.z = 0.5f*System::Math::Sqrt(m22 -
                            m00 - m11 + 1.0f);
                        fHalfInverse = 0.5f/rkAxis.z;
                        rkAxis.x = fHalfInverse*m02;
                        rkAxis.y = fHalfInverse*m12;
                    }
                }
            }
        }
        else
        {
            // The angle is 0 and the matrix is the identity.  Any axis will
            // work, so just use the x-axis.
            rkAxis.x = 1.0;
            rkAxis.y = 0.0;
            rkAxis.z = 0.0;
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromAngleAxis (Vector3 rkAxis, Radian fRadians)
    {
        Real fCos = Math::Cos(fRadians);
        Real fSin = Math::Sin(fRadians);
        Real fOneMinusCos = 1.0f-fCos;
        Real fX2 = rkAxis.x*rkAxis.x;
        Real fY2 = rkAxis.y*rkAxis.y;
        Real fZ2 = rkAxis.z*rkAxis.z;
        Real fXYM = rkAxis.x*rkAxis.y*fOneMinusCos;
        Real fXZM = rkAxis.x*rkAxis.z*fOneMinusCos;
        Real fYZM = rkAxis.y*rkAxis.z*fOneMinusCos;
        Real fXSin = rkAxis.x*fSin;
        Real fYSin = rkAxis.y*fSin;
        Real fZSin = rkAxis.z*fSin;

        m00 = fX2*fOneMinusCos+fCos;
        m01 = fXYM-fZSin;
        m02 = fXZM+fYSin;
        m10 = fXYM+fZSin;
        m11 = fY2*fOneMinusCos+fCos;
        m12 = fYZM-fXSin;
        m20 = fXZM-fYSin;
        m21 = fYZM+fXSin;
        m22 = fZ2*fOneMinusCos+fCos;
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesXYZ (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz          -cy*sz           sy
        //        cz*sx*sy+cx*sz  cx*cz-sx*sy*sz -cy*sx
        //       -cx*cz*sy+sx*sz  cz*sx+cx*sy*sz  cx*cy

        rfPAngle = Radian(Mogre::Math::ASin(m02));
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(-m12,m22);
                rfRAngle = System::Math::Atan2(-m01,m00);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(m10,m11);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(m10,m11);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesXZY (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz          -sz              cz*sy
        //        sx*sy+cx*cy*sz  cx*cz          -cy*sx+cx*sy*sz
        //       -cx*sy+cy*sx*sz  cz*sx           cx*cy+sx*sy*sz

        rfPAngle = Mogre::Math::ASin(-m01);
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(m21,m11);
                rfRAngle = System::Math::Atan2(m02,m00);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(-m20,m22);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(-m20,m22);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesYXZ (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz+sx*sy*sz  cz*sx*sy-cy*sz  cx*sy
        //        cx*sz           cx*cz          -sx
        //       -cz*sy+cy*sx*sz  cy*cz*sx+sy*sz  cx*cy

        rfPAngle = Mogre::Math::ASin(-m12);
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(m02,m22);
                rfRAngle = System::Math::Atan2(m10,m11);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(-m01,m00);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(-m01,m00);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesYZX (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz           sx*sy-cx*cy*sz  cx*sy+cy*sx*sz
        //        sz              cx*cz          -cz*sx
        //       -cz*sy           cy*sx+cx*sy*sz  cx*cy-sx*sy*sz

        rfPAngle = Mogre::Math::ASin(m10);
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(-m20,m00);
                rfRAngle = System::Math::Atan2(-m12,m11);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(m21,m22);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(m21,m22);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesZXY (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz-sx*sy*sz -cx*sz           cz*sy+cy*sx*sz
        //        cz*sx*sy+cy*sz  cx*cz          -cy*cz*sx+sy*sz
        //       -cx*sy           sx              cx*cy

        rfPAngle = Mogre::Math::ASin(m21);
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(-m01,m11);
                rfRAngle = System::Math::Atan2(-m20,m22);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(m02,m00);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(m02,m00);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::ToEulerAnglesZYX (Radian% rfYAngle, Radian% rfPAngle,
        Radian% rfRAngle)
    {
        // rot =  cy*cz           cz*sx*sy-cx*sz  cx*cz*sy+sx*sz
        //        cy*sz           cx*cz+sx*sy*sz -cz*sx+cx*sy*sz
        //       -sy              cy*sx           cx*cy

        rfPAngle = Mogre::Math::ASin(-m20);
        if ( rfPAngle < Radian(Math::HALF_PI) )
        {
            if ( rfPAngle > Radian(-Math::HALF_PI) )
            {
                rfYAngle = System::Math::Atan2(m10,m00);
                rfRAngle = System::Math::Atan2(m21,m22);
                return true;
            }
            else
            {
                // WARNING.  Not a unique solution.
                Radian fRmY = System::Math::Atan2(-m01,m02);
                rfRAngle = Radian(0.0);  // any angle works
                rfYAngle = rfRAngle - fRmY;
                return false;
            }
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRpY = System::Math::Atan2(-m01,m02);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = fRpY - rfRAngle;
            return false;
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesXYZ (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        *this = kXMat*(kYMat*kZMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesXZY (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        *this = kXMat*(kZMat*kYMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesYXZ (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        *this = kYMat*(kXMat*kZMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesYZX (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        *this = kYMat*(kZMat*kXMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesZXY (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        *this = kZMat*(kXMat*kYMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::FromEulerAnglesZYX (Radian fYAngle, Radian fPAngle,
        Radian fRAngle)
    {
        Real fCos, fSin;

        fCos = Math::Cos(fYAngle);
        fSin = Math::Sin(fYAngle);
        Matrix3^ kZMat = gcnew Matrix3(fCos,-fSin,0.0,fSin,fCos,0.0,0.0,0.0,1.0);

        fCos = Math::Cos(fPAngle);
        fSin = Math::Sin(fPAngle);
        Matrix3^ kYMat = gcnew Matrix3(fCos,0.0,fSin,0.0,1.0,0.0,-fSin,0.0,fCos);

        fCos = Math::Cos(fRAngle);
        fSin = Math::Sin(fRAngle);
        Matrix3^ kXMat = gcnew Matrix3(1.0,0.0,0.0,0.0,fCos,-fSin,0.0,fSin,fCos);

        *this = kZMat*(kYMat*kXMat);
    }
    //-----------------------------------------------------------------------
    void Matrix3::Tridiagonal (Real afDiag[3], Real afSubDiag[3])
    {
        // Householder reduction T = Q^t M Q
        //   Input:
        //     mat, symmetric 3x3 matrix M
        //   Output:
        //     mat, orthogonal matrix Q
        //     diag, diagonal entries of T
        //     subd, subdiagonal entries of T (T is symmetric)

        Real fA = m00;
        Real fB = m01;
        Real fC = m02;
        Real fD = m11;
        Real fE = m12;
        Real fF = m22;

        afDiag[0] = fA;
        afSubDiag[2] = 0.0;
        if ( System::Math::Abs(fC) >= EPSILON )
        {
            Real fLength = System::Math::Sqrt(fB*fB+fC*fC);
            Real fInvLength = 1.0f/fLength;
            fB *= fInvLength;
            fC *= fInvLength;
            Real fQ = 2.0f*fB*fE+fC*(fF-fD);
            afDiag[1] = fD+fC*fQ;
            afDiag[2] = fF-fC*fQ;
            afSubDiag[0] = fLength;
            afSubDiag[1] = fE-fB*fQ;
            m00 = 1.0;
            m01 = 0.0;
            m02 = 0.0;
            m10 = 0.0;
            m11 = fB;
            m12 = fC;
            m20 = 0.0;
            m21 = fC;
            m22 = -fB;
        }
        else
        {
            afDiag[1] = fD;
            afDiag[2] = fF;
            afSubDiag[0] = fB;
            afSubDiag[1] = fE;
            m00 = 1.0;
            m01 = 0.0;
            m02 = 0.0;
            m10 = 0.0;
            m11 = 1.0;
            m12 = 0.0;
            m20 = 0.0;
            m21 = 0.0;
            m22 = 1.0;
        }
    }
    //-----------------------------------------------------------------------
    bool Matrix3::QLAlgorithm (Real afDiag[3], Real afSubDiag[3])
    {
        // QL iteration with implicit shifting to reduce matrix from tridiagonal
        // to diagonal

        for (int i0 = 0; i0 < 3; i0++)
        {
            const unsigned int iMaxIter = 32;
            unsigned int iIter;
            for (iIter = 0; iIter < iMaxIter; iIter++)
            {
                int i1;
                for (i1 = i0; i1 <= 1; i1++)
                {
                    Real fSum = System::Math::Abs(afDiag[i1]) +
                        System::Math::Abs(afDiag[i1+1]);
                    if ( System::Math::Abs(afSubDiag[i1]) + fSum == fSum )
                        break;
                }
                if ( i1 == i0 )
                    break;

                Real fTmp0 = (afDiag[i0+1]-afDiag[i0])/(2.0f*afSubDiag[i0]);
                Real fTmp1 = System::Math::Sqrt(fTmp0*fTmp0+1.0f);
                if ( fTmp0 < 0.0 )
                    fTmp0 = afDiag[i1]-afDiag[i0]+afSubDiag[i0]/(fTmp0-fTmp1);
                else
                    fTmp0 = afDiag[i1]-afDiag[i0]+afSubDiag[i0]/(fTmp0+fTmp1);
                Real fSin = 1.0;
                Real fCos = 1.0;
                Real fTmp2 = 0.0;
                for (int i2 = i1-1; i2 >= i0; i2--)
                {
                    Real fTmp3 = fSin*afSubDiag[i2];
                    Real fTmp4 = fCos*afSubDiag[i2];
                    if ( System::Math::Abs(fTmp3) >= System::Math::Abs(fTmp0) )
                    {
                        fCos = fTmp0/fTmp3;
                        fTmp1 = System::Math::Sqrt(fCos*fCos+1.0f);
                        afSubDiag[i2+1] = fTmp3*fTmp1;
                        fSin = 1.0f/fTmp1;
                        fCos *= fSin;
                    }
                    else
                    {
                        fSin = fTmp3/fTmp0;
                        fTmp1 = System::Math::Sqrt(fSin*fSin+1.0f);
                        afSubDiag[i2+1] = fTmp0*fTmp1;
                        fCos = 1.0f/fTmp1;
                        fSin *= fCos;
                    }
                    fTmp0 = afDiag[i2+1]-fTmp2;
                    fTmp1 = (afDiag[i2]-fTmp0)*fSin+2.0f*fTmp4*fCos;
                    fTmp2 = fSin*fTmp1;
                    afDiag[i2+1] = fTmp0+fTmp2;
                    fTmp0 = fCos*fTmp1-fTmp4;

                    for (int iRow = 0; iRow < 3; iRow++)
                    {
                        fTmp3 = this[iRow, i2+1];
                        this[iRow, i2+1] = fSin*this[iRow, i2] +
                            fCos*fTmp3;
                        this[iRow, i2] = fCos*this[iRow, i2] -
                            fSin*fTmp3;
                    }
                }
                afDiag[i0] -= fTmp2;
                afSubDiag[i0] = fTmp0;
                afSubDiag[i1] = 0.0;
            }

            if ( iIter == iMaxIter )
            {
                // should not get here under normal circumstances
                return false;
            }
        }

        return true;
    }
    //-----------------------------------------------------------------------
    void Matrix3::EigenSolveSymmetric (array<Real>^ afEigenvalue,
        array<Vector3>^ akEigenvector)
    {
        Matrix3^ kMatrix = this;
        Real afSubDiag[3];
        pin_ptr<Real> p_afEigenvalue = &afEigenvalue[0];
        kMatrix->Tridiagonal(p_afEigenvalue,afSubDiag);
        kMatrix->QLAlgorithm(p_afEigenvalue,afSubDiag);

        p_afEigenvalue = nullptr;

        for (size_t i = 0; i < 3; i++)
        {
            akEigenvector[i].x = kMatrix[0, i];
            akEigenvector[i].y = kMatrix[1, i];
            akEigenvector[i].z = kMatrix[2, i];
        }

        // make eigenvectors form a right--handed system
        Vector3 kCross = akEigenvector[1].CrossProduct(akEigenvector[2]);
        Real fDet = akEigenvector[0].DotProduct(kCross);
        if ( fDet < 0.0 )
        {
            akEigenvector[2].x = - akEigenvector[2].x;
            akEigenvector[2].y = - akEigenvector[2].y;
            akEigenvector[2].z = - akEigenvector[2].z;
        }
    }
    //-----------------------------------------------------------------------
    void Matrix3::TensorProduct (Vector3 rkU, Vector3 rkV,
        Matrix3^% rkProduct)
    {
        rkProduct = gcnew Matrix3;

        for (size_t iRow = 0; iRow < 3; iRow++)
        {
            for (size_t iCol = 0; iCol < 3; iCol++)
                rkProduct[iRow, iCol] = rkU[iRow]*rkV[iCol];
        }
    }
    //-----------------------------------------------------------------------
}