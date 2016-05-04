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
#include "OgreAxisAlignedBox.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"
#include "Custom\MogreVector3.h"
#include "Custom\MogreMatrix4.h"
#include "MogreSphere.h"

namespace Mogre {

    /** <summary>A 3D box aligned with the x/y/z axes.</summary>
    <remarks>
    This class represents a simple box which is aligned with the
    axes. Internally it only stores 2 points as the extremeties of
    the box, one which is the minima of all 3 axes, and the other
    which is the maxima of all 3 axes. This class is typically used
    for an axis-aligned bounding box (AABB) for collision and
    visibility determination.
    </remarks>
    */
    [Serializable]
    public ref class AxisAlignedBox
    {
    public:
        static operator AxisAlignedBox^ (const Ogre::AxisAlignedBox& box)
        {
            AxisAlignedBox^ clr_box = gcnew AxisAlignedBox(box.getMinimum(), box.getMaximum());
            if (box.isNull())
                clr_box->SetNull();
            else if (box.isInfinite())
                clr_box->SetInfinite();

            return clr_box;
        }

        static operator Ogre::AxisAlignedBox (AxisAlignedBox^ box)
        {
            Ogre::AxisAlignedBox o_box((Ogre::Vector3&)box->mMinimum, (Ogre::Vector3&)box->mMaximum);
            if (box->IsNull)
                o_box.setNull();
            else if (box->IsInfinite)
                o_box.setInfinite();

            return o_box;
        }

        enum class Extent
        {
            EXTENT_NULL,
            EXTENT_FINITE,
            EXTENT_INFINITE
        };

    protected:
        Vector3 mMinimum;
        Vector3 mMaximum;
        Extent mExtent;

        [NonSerialized]
        array<Vector3>^ mCorners;

    public:
        /*
           1-----2
          /|    /|
         / |   / |
        5-----4  |
        |  0--|--3
        | /   | /
        |/    |/
        6-----7
        */
        enum class CornerEnum {
            FAR_LEFT_BOTTOM = Ogre::AxisAlignedBox::FAR_LEFT_BOTTOM,
            FAR_LEFT_TOP = Ogre::AxisAlignedBox::FAR_LEFT_TOP,
            FAR_RIGHT_TOP = Ogre::AxisAlignedBox::FAR_RIGHT_TOP,
            FAR_RIGHT_BOTTOM = Ogre::AxisAlignedBox::FAR_RIGHT_BOTTOM,
            NEAR_RIGHT_BOTTOM = Ogre::AxisAlignedBox::NEAR_RIGHT_BOTTOM,
            NEAR_LEFT_BOTTOM = Ogre::AxisAlignedBox::NEAR_LEFT_BOTTOM,
            NEAR_LEFT_TOP = Ogre::AxisAlignedBox::NEAR_LEFT_TOP,
            NEAR_RIGHT_TOP = Ogre::AxisAlignedBox::NEAR_RIGHT_TOP
        };
        inline AxisAlignedBox()
        {
            // Default to a null box 
            SetMinimum( -0.5, -0.5, -0.5 );
            SetMaximum( 0.5, 0.5, 0.5 );
            mExtent = Extent::EXTENT_NULL;
        }

        inline AxisAlignedBox(Extent e)
        {
            SetMinimum( -0.5, -0.5, -0.5 );
            SetMaximum( 0.5, 0.5, 0.5 );
            mExtent = e;
        }

        inline AxisAlignedBox(AxisAlignedBox^ rkBox)
        {
            if (rkBox->IsNull)
                SetNull();
            else if (rkBox->IsInfinite)
                SetInfinite();
            else
                SetExtents( rkBox->mMinimum, rkBox->mMaximum );
        }

        inline AxisAlignedBox( Vector3 min, Vector3 max )
        {
            SetExtents( min, max );
        }

        inline AxisAlignedBox(
            Real mx, Real my, Real mz,
            Real Mx, Real My, Real Mz )
        {
            SetExtents( mx, my, mz, Mx, My, Mz );
        }


        /** <summary>Gets or Sets the minimum corner of the box.</summary>
        */
        property Vector3 Minimum
        {
            inline Vector3 get() { return mMinimum; }
            inline void set( Vector3 vec )
            {
                mExtent = Extent::EXTENT_FINITE;
                mMinimum = vec;
            }
        }

        /** <summary>Gets or Sets the maximum corner of the box.</summary>
        */
        property Vector3 Maximum
        { 
            inline Vector3 get() { return mMaximum; }
            inline void set( Vector3 vec )
            {
                mExtent = Extent::EXTENT_FINITE;
                mMaximum = vec;
            }
        }

        /** <summary>Sets the minimum corner of the box.</summary>
        */
        inline void SetMinimum( Vector3 vec )
        {
            mExtent = Extent::EXTENT_FINITE;
            mMinimum = vec;
        }

        /** <summary>Sets the minimum corner of the box.</summary>
        */
        inline void SetMinimum( Real x, Real y, Real z )
        {
            mExtent = Extent::EXTENT_FINITE;
            mMinimum.x = x;
            mMinimum.y = y;
            mMinimum.z = z;
        }

        /** <summary>Changes the X component of the minimum corner of the box</summary>
        */
        inline void SetMinimumX(Real x)
        {
            mMinimum.x = x;
        }

        /** <summary>Changes the Y component of the minimum corner of the box</summary>
        */
        inline void SetMinimumY(Real y)
        {
            mMinimum.y = y;
        }

        /** <summary>Changes the Z component of the minimum corner of the box</summary>
        */
        inline void SetMinimumZ(Real z)
        {
            mMinimum.z = z;
        }

        /** <summary>Sets the maximum corner of the box.</summary>
        */
        inline void SetMaximum( Vector3 vec )
        {
            mExtent = Extent::EXTENT_FINITE;
            mMaximum = vec;
        }

        /** <summary>Sets the maximum corner of the box.</summary>
        */
        inline void SetMaximum( Real x, Real y, Real z )
        {
            mExtent = Extent::EXTENT_FINITE;
            mMaximum.x = x;
            mMaximum.y = y;
            mMaximum.z = z;
        }

        /** <summary>Changes the X component of the maximum corner of the box</summary>
        */
        inline void SetMaximumX( Real x )
        {
            mMaximum.x = x;
        }

        /** <summary>Changes the Y component of the maximum corner of the box</summary>
        */
        inline void SetMaximumY( Real y )
        {
            mMaximum.y = y;
        }

        /** <summary>Changes the Z component of the maximum corner of the box</summary>
        */
        inline void SetMaximumZ( Real z )
        {
            mMaximum.z = z;
        }

        /** <summary>Sets both minimum and maximum extents at once.</summary>
        */
        inline void SetExtents( Vector3 min, Vector3 max )
        {
            mExtent = Extent::EXTENT_FINITE;
            mMinimum = min;
            mMaximum = max;
        }

        /** <summary>Sets both minimum and maximum extents at once.</summary>
        */
        inline void SetExtents(
            Real mx, Real my, Real mz,
            Real Mx, Real My, Real Mz )
        {
            mExtent = Extent::EXTENT_FINITE;

            mMinimum.x = mx;
            mMinimum.y = my;
            mMinimum.z = mz;

            mMaximum.x = Mx;
            mMaximum.y = My;
            mMaximum.z = Mz;

        }

        /** <summary>Returns an array of 8 corner points, useful for
        collision vs. non-aligned objects.</summary>
        <remarks>
        If the order of these corners is important, they are as
        follows: The 4 points of the minimum Z face (note that
        because Ogre uses right-handed coordinates, the minimum Z is
        at the 'back' of the box) starting with the minimum point of
        all, then anticlockwise around this face (if you are looking
        onto the face from outside the box). Then the 4 points of the
        maximum Z face, starting with maximum point of all, then
        anticlockwise around this face (looking onto the face from
        outside the box). Like this:
        <pre>
           1-----2
          /|    /|
         / |   / |
        5-----4  |
        |  0--|--3
        | /   | /
        |/    |/
        6-----7
        </pre>
        <note>as this implementation uses a static member, make sure to use your own copy !</note>
        </remarks>
        */
        inline array<Vector3>^ GetAllCorners()
        {
            if (mExtent != Extent::EXTENT_FINITE)
                throw gcnew System::Exception("Can't get corners of a null or infinite AAB");

            // The order of these items is, using right-handed co-ordinates:
            // Minimum Z face, starting with Min(all), then anticlockwise
            //   around face (looking onto the face)
            // Maximum Z face, starting with Max(all), then anticlockwise
            //   around face (looking onto the face)
            // Only for optimization/compatibility.
            if (!mCorners)
                mCorners = gcnew array<Vector3>(8);

            mCorners[0] = mMinimum;
            mCorners[1].x = mMinimum.x; mCorners[1].y = mMaximum.y; mCorners[1].z = mMinimum.z;
            mCorners[2].x = mMaximum.x; mCorners[2].y = mMaximum.y; mCorners[2].z = mMinimum.z;
            mCorners[3].x = mMaximum.x; mCorners[3].y = mMinimum.y; mCorners[3].z = mMinimum.z;

            mCorners[4] = mMaximum;
            mCorners[5].x = mMinimum.x; mCorners[5].y = mMaximum.y; mCorners[5].z = mMaximum.z;
            mCorners[6].x = mMinimum.x; mCorners[6].y = mMinimum.y; mCorners[6].z = mMaximum.z;
            mCorners[7].x = mMaximum.x; mCorners[7].y = mMinimum.y; mCorners[7].z = mMaximum.z;

            return mCorners;
        }

        /** <summary>Gets the position of one of the corners</summary>
        */
        Vector3 GetCorner(CornerEnum cornerToGet)
        {
            switch(cornerToGet)
            {
            case CornerEnum::FAR_LEFT_BOTTOM:
                return mMinimum;
            case CornerEnum::FAR_LEFT_TOP:
                return Vector3(mMinimum.x, mMaximum.y, mMinimum.z);
            case CornerEnum::FAR_RIGHT_TOP:
                return Vector3(mMaximum.x, mMaximum.y, mMinimum.z);
            case CornerEnum::FAR_RIGHT_BOTTOM:
                return Vector3(mMaximum.x, mMinimum.y, mMinimum.z);
            case CornerEnum::NEAR_RIGHT_BOTTOM:
                return Vector3(mMaximum.x, mMinimum.y, mMaximum.z);
            case CornerEnum::NEAR_LEFT_BOTTOM:
                return Vector3(mMinimum.x, mMinimum.y, mMaximum.z);
            case CornerEnum::NEAR_LEFT_TOP:
                return Vector3(mMinimum.x, mMaximum.y, mMaximum.z);
            case CornerEnum::NEAR_RIGHT_TOP:
                return mMaximum;
            default:
                return Vector3();
            }
        }

        virtual String^ ToString() override
        {
            switch (mExtent)
            {
            case Extent::EXTENT_NULL:
                return "AxisAlignedBox(null)";

            case Extent::EXTENT_FINITE:
                return "AxisAlignedBox(min=" + mMinimum + ", max=" + mMaximum + ")";

            case Extent::EXTENT_INFINITE:
                return "AxisAlignedBox(infinite)";

            default: // shut up compiler
                throw gcnew Exception("Should never reach here" );
            }
        }

        /** <summary>Merges the passed in box into the current box. The result is the
        box which encompasses both.</summary>
        */
        void Merge( AxisAlignedBox^ rhs )
        {
            // Do nothing if rhs null, or this is infinite
            if ((rhs->mExtent == Extent::EXTENT_NULL) || (mExtent == Extent::EXTENT_INFINITE))
            {
                return;
            }
            // Otherwise if rhs is infinite, make this infinite, too
            else if (rhs->mExtent == Extent::EXTENT_INFINITE)
            {
                mExtent = Extent::EXTENT_INFINITE;
            }
            // Otherwise if current null, just take rhs
            else if (mExtent == Extent::EXTENT_NULL)
            {
                SetExtents(rhs->mMinimum, rhs->mMaximum);
            }
            // Otherwise merge
            else
            {
                Vector3 min = mMinimum;
                Vector3 max = mMaximum;
                max.MakeCeil(rhs->mMaximum);
                min.MakeFloor(rhs->mMinimum);

                SetExtents(min, max);
            }

        }

        /** <summary>Extends the box to encompass the specified point (if needed).</summary>
        */
        inline void Merge( Vector3 point )
        {
            switch (mExtent)
            {
            case Extent::EXTENT_NULL: // if null, use this point
                SetExtents(point, point);
                return;

            case Extent::EXTENT_FINITE:
                mMaximum.MakeCeil(point);
                mMinimum.MakeFloor(point);
                return;

            case Extent::EXTENT_INFINITE: // if infinite, makes no difference
                return;
            }

            throw gcnew Exception(" Should never reach here" );
        }

        /** <summary>Transforms the box according to the matrix supplied.</summary>
        <remarks>
        By calling this method you get the axis-aligned box which
        surrounds the transformed version of this box. Therefore each
        corner of the box is transformed by the matrix, then the
        extents are mapped back onto the axes to produce another
        AABB. Useful when you have a local AABB for an object which
        is then transformed.
        </remarks>
        */
        inline void Transform( Matrix4^ matrix )
        {
            // Do nothing if current null or infinite
            if( mExtent != Extent::EXTENT_FINITE )
                return;

            Vector3 oldMin, oldMax, currentCorner;

            // Getting the old values so that we can use the existing merge method.
            oldMin = mMinimum;
            oldMax = mMaximum;

            // reset
            SetNull();

            // We sequentially compute the corners in the following order :
            // 0, 6, 5, 1, 2, 4 ,7 , 3
            // This sequence allows us to only change one member at a time to get at all corners.

            // For each one, we transform it using the matrix
            // Which gives the resulting point and merge the resulting point.

            // First corner 
            // min min min
            currentCorner = oldMin;
            Merge( matrix * currentCorner );

            // min,min,max
            currentCorner.z = oldMax.z;
            Merge( matrix * currentCorner );

            // min max max
            currentCorner.y = oldMax.y;
            Merge( matrix * currentCorner );

            // min max min
            currentCorner.z = oldMin.z;
            Merge( matrix * currentCorner );

            // max max min
            currentCorner.x = oldMax.x;
            Merge( matrix * currentCorner );

            // max max max
            currentCorner.z = oldMax.z;
            Merge( matrix * currentCorner );

            // max min max
            currentCorner.y = oldMin.y;
            Merge( matrix * currentCorner );

            // max min min
            currentCorner.z = oldMin.z;
            Merge( matrix * currentCorner ); 
        }

        /** <summary>Transforms the box according to the affine matrix supplied.</summary>
        <remarks>
        By calling this method you get the axis-aligned box which
        surrounds the transformed version of this box. Therefore each
        corner of the box is transformed by the matrix, then the
        extents are mapped back onto the axes to produce another
        AABB. Useful when you have a local AABB for an object which
        is then transformed.
        <note>
        The matrix must be an affine matrix. <see cref="Matrix4::IsAffine"/>.
        </note>
        </remarks>
        */
        void TransformAffine(Matrix4^ m)
        {
            if (!m->IsAffine)
                throw gcnew ArgumentException("Matrix should be Affine", "m");

            // Do nothing if current null or infinite
            if ( mExtent != Extent::EXTENT_FINITE )
                return;

            Vector3 centre = Center;
            Vector3 halfSize = HalfSize;

            Vector3 newCentre = m->TransformAffine(centre);
            Vector3 newHalfSize(
                Math::Abs(m->m00) * halfSize.x + Math::Abs(m->m01) * halfSize.y + Math::Abs(m->m02) * halfSize.z, 
                Math::Abs(m->m10) * halfSize.x + Math::Abs(m->m11) * halfSize.y + Math::Abs(m->m12) * halfSize.z,
                Math::Abs(m->m20) * halfSize.x + Math::Abs(m->m21) * halfSize.y + Math::Abs(m->m22) * halfSize.z);

            SetExtents(newCentre - newHalfSize, newCentre + newHalfSize);
        }

        /** <summary>Sets the box to a 'null' value i.e. not a box.</summary>
        */
        inline void SetNull()
        {
            mExtent = Extent::EXTENT_NULL;
        }

        /** <summary>Returns true if the box is null i.e. empty.</summary>
        */
        property bool IsNull
        {
            inline bool get() { return (mExtent == Extent::EXTENT_NULL); }
        }

        /** <summary>Returns true if the box is finite.</summary>
        */
        property bool IsFinite
        {
            bool get() { return (mExtent == Extent::EXTENT_FINITE); }
        }

        /** <summary>Sets the box to 'infinite'</summary>
        */
        inline void SetInfinite()
        {
            mExtent = Extent::EXTENT_INFINITE;
        }

        /** <summary>Returns true if the box is infinite.</summary>
        */
        property bool IsInfinite
        {
            bool get() { return (mExtent == Extent::EXTENT_INFINITE); }
        }

        /** <summary>Returns whether or not this box intersects another.</summary> */
        inline bool Intersects(AxisAlignedBox^ b2)
        {
            // Early-fail for nulls
            if (this->IsNull || b2->IsNull)
                return false;

            // Early-success for infinites
            if (this->IsInfinite || b2->IsInfinite)
                return true;

            // Use up to 6 separating planes
            if (mMaximum.x < b2->mMinimum.x)
                return false;
            if (mMaximum.y < b2->mMinimum.y)
                return false;
            if (mMaximum.z < b2->mMinimum.z)
                return false;

            if (mMinimum.x > b2->mMaximum.x)
                return false;
            if (mMinimum.y > b2->mMaximum.y)
                return false;
            if (mMinimum.z > b2->mMaximum.z)
                return false;

            // otherwise, must be intersecting
            return true;

        }

        /// <summary>Calculate the area of intersection of this box and another</summary>
        inline AxisAlignedBox^ Intersection(AxisAlignedBox^ b2)
        {
            if (this->IsNull || b2->IsNull)
            {
                return gcnew AxisAlignedBox();
            }
            else if (this->IsInfinite)
            {
                return gcnew AxisAlignedBox(b2);
            }
            else if (b2->IsInfinite)
            {
                return gcnew AxisAlignedBox(this);
            }

            Vector3 intMin = mMinimum;
            Vector3 intMax = mMaximum;

            intMin.MakeCeil(b2->Minimum);
            intMax.MakeFloor(b2->Maximum);

            // Check intersection isn't null
            if (intMin.x < intMax.x &&
                intMin.y < intMax.y &&
                intMin.z < intMax.z)
            {
                return gcnew AxisAlignedBox(intMin, intMax);
            }

            return gcnew AxisAlignedBox();
        }

        /// <summary>Calculate the volume of this box</summary>
        Real Volume()
        {
            switch (mExtent)
            {
            case Extent::EXTENT_NULL:
                return 0.0f;

            case Extent::EXTENT_FINITE:
                {
                    Vector3 diff = mMaximum - mMinimum;
                    return diff.x * diff.y * diff.z;
                }

            case Extent::EXTENT_INFINITE:
                return Math::POS_INFINITY;

            default: // shut up compiler
                throw gcnew Exception("Should never reach here" );
            }
        }

        /** <summary>Scales the AABB by the vector given.</summary> */
        inline void Scale(Vector3 s)
        {
            // Do nothing if current null or infinite
            if (mExtent != Extent::EXTENT_FINITE)
                return;

            // NB assumes centered on origin
            Vector3 min = mMinimum * s;
            Vector3 max = mMaximum * s;
            SetExtents(min, max);
        }

        /** <summary>Tests whether this box intersects a sphere.</summary> */
        bool Intersects(Sphere s)
        {
            return Math::Intersects(s, this); 
        }
        /** <summary>Tests whether this box intersects a plane.</summary> */
        bool Intersects(Plane p)
        {
            return Math::Intersects(p, this);
        }
        /** <summary>Tests whether the vector point is within this box.</summary> */
        bool Intersects(Vector3 v)
        {
            switch (mExtent)
            {
            case Extent::EXTENT_NULL:
                return false;

            case Extent::EXTENT_FINITE:
                return(v.x >= mMinimum.x  &&  v.x <= mMaximum.x  && 
                    v.y >= mMinimum.y  &&  v.y <= mMaximum.y  && 
                    v.z >= mMinimum.z  &&  v.z <= mMaximum.z);

            case Extent::EXTENT_INFINITE:
                return true;

            default: // shut up compiler
                throw gcnew Exception("Should never reach here" );
            }
        }
        /// <summary>Gets the centre of the box</summary>
        property Vector3 Center
        {
            Vector3 get()
            {
                if (mExtent != Extent::EXTENT_FINITE)
                    throw gcnew Exception("Can't get center of a null or infinite AAB");

                return Vector3(
                    (mMaximum.x + mMinimum.x) * 0.5f,
                    (mMaximum.y + mMinimum.y) * 0.5f,
                    (mMaximum.z + mMinimum.z) * 0.5f);
            }
        }
        /// <summary>Gets the size of the box</summary>
        property Vector3 Size
        {
            Vector3 get()
            {
                switch (mExtent)
                {
                case Extent::EXTENT_NULL:
                    return Vector3::ZERO;

                case Extent::EXTENT_FINITE:
                    return mMaximum - mMinimum;

                case Extent::EXTENT_INFINITE:
                    return Vector3(
                        Math::POS_INFINITY,
                        Math::POS_INFINITY,
                        Math::POS_INFINITY);

                default: // shut up compiler
                    throw gcnew Exception("Should never reach here" );
                }
            }
        }
        /// <summary>Gets the half-size of the box</summary>
        property Vector3 HalfSize
        {
            Vector3 get()
            {
                switch (mExtent)
                {
                case Extent::EXTENT_NULL:
                    return Vector3::ZERO;

                case Extent::EXTENT_FINITE:
                    return (mMaximum - mMinimum) * 0.5;

                case Extent::EXTENT_INFINITE:
                    return Vector3(
                        Math::POS_INFINITY,
                        Math::POS_INFINITY,
                        Math::POS_INFINITY);

                default: // shut up compiler
                    throw gcnew Exception("Should never reach here" );
                }
            }
        }

        /** <summary>Tests whether the given point contained by this box.</summary>
        */
        bool Contains(Vector3 v)
        {
            if (IsNull)
                return false;
            if (IsInfinite)
                return true;

            return mMinimum.x <= v.x && v.x <= mMaximum.x &&
                mMinimum.y <= v.y && v.y <= mMaximum.y &&
                mMinimum.z <= v.z && v.z <= mMaximum.z;
        }

        /** <summary>Returns the minimum distance between a given point and any part of the box.</summary> */
        Real Distance(Vector3 v)
        {
            if (Contains(v))
                return 0;
            else
            {
                Real maxDist = Real::MinValue;

                if (v.x < mMinimum.x)
                    maxDist = System::Math::Max(maxDist, mMinimum.x - v.x);
                if (v.y < mMinimum.y)
                    maxDist = System::Math::Max(maxDist, mMinimum.y - v.y);
                if (v.z < mMinimum.z)
                    maxDist = System::Math::Max(maxDist, mMinimum.z - v.z);

                if (v.x > mMaximum.x)
                    maxDist = System::Math::Max(maxDist, v.x - mMaximum.x);
                if (v.y > mMaximum.y)
                    maxDist = System::Math::Max(maxDist, v.y - mMaximum.y);
                if (v.z > mMaximum.z)
                    maxDist = System::Math::Max(maxDist, v.z - mMaximum.z);

                return maxDist;
            }
        }

        /** <summary>Tests whether another box contained by this box.</summary>
        */
        bool Contains(AxisAlignedBox^ other)
        {
            if (other->IsNull || this->IsInfinite)
                return true;

            if (this->IsNull || other->IsInfinite)
                return false;

            return this->mMinimum.x <= other->mMinimum.x &&
                this->mMinimum.y <= other->mMinimum.y &&
                this->mMinimum.z <= other->mMinimum.z &&
                other->mMaximum.x <= this->mMaximum.x &&
                other->mMaximum.y <= this->mMaximum.y &&
                other->mMaximum.z <= this->mMaximum.z;
        }

        /** <summary>Tests 2 boxes for equality.</summary>
        */
        virtual bool Equals(Object^ obj) override
        {
            AxisAlignedBox^ box = dynamic_cast<AxisAlignedBox^>(obj);
            if (box)
                return this->Equals(box);
            else
                return false;
        }

        bool Equals(AxisAlignedBox^ rhs)
        {
            if (this->mExtent != rhs->mExtent)
                return false;

            if (!this->IsFinite)
                return true;

            return this->mMinimum == rhs->mMinimum &&
                this->mMaximum == rhs->mMaximum;
        }

        // special values
        static initonly AxisAlignedBox^ BOX_NULL = gcnew AxisAlignedBox( );
        static initonly AxisAlignedBox^ BOX_INFINITE = gcnew AxisAlignedBox( Extent::EXTENT_INFINITE );

    };

} // namespace Mogre
