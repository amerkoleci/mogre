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
#include "OgreMath.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"
#include "MogrePair.h"

namespace Mogre
{
    ref class AxisAlignedBox;
    ref class Matrix4;
    value class Ray;
    value class Sphere;
    value class Plane;
    value class Vector2;
    value class Vector3;
    value class Vector4;
    value class Quaternion;
    value class Degree;

    /** <summary>Wrapper class which indicates a given angle value is in Radians.</summary>
    <remarks>
    Radian values are interchangeable with Degree values, and conversions
    will be done automatically between them.
    </remarks>
    */
    [Serializable]
    public value class Radian : IEquatable<Radian>, IComparable<Radian>
    {
        Real mRad;

    public:
        explicit Radian ( Real r ) : mRad(r) {}
        Radian ( Degree d );
        inline static operator Radian ( Real f ) { return Radian(f); }
        inline static operator Radian ( Degree d );

        property Real ValueDegrees { Real get(); }
        property Real ValueRadians { Real get() { return mRad; } }
        property Real ValueAngleUnits { Real get(); }

        inline static Radian operator + ( Radian l, Radian r ) { return Radian ( l.mRad + r.mRad ); }
        inline static Radian operator + ( Radian l, Degree d );
        inline static Radian operator - (Radian r ) { return Radian(-r.mRad); }
        inline static Radian operator - ( Radian l, Radian r ) { return Radian ( l.mRad - r.mRad ); }
        inline static Radian operator - ( Radian l, Degree d );
        inline static Radian operator * ( Radian l, Real f ) { return Radian ( l.mRad * f ); }
        inline static Radian operator * ( Real f, Radian r ) { return Radian ( r.mRad * f ); }
        inline static Radian operator * ( Radian l, Radian f ) { return Radian ( l.mRad * f.mRad ); }
        inline static Radian operator / ( Radian l, Real f ) { return Radian ( l.mRad / f ); }
        inline static Radian operator / ( Real f, Radian r ) { return Radian ( f / r.mRad ); }

        inline static bool operator <  ( Radian l, Radian r ) { return l.mRad <  r.mRad; }
        inline static bool operator <= ( Radian l, Radian r ) { return l.mRad <= r.mRad; }
        inline static bool operator == ( Radian l, Radian r ) { return l.mRad == r.mRad; }
        inline static bool operator != ( Radian l, Radian r ) { return l.mRad != r.mRad; }
        inline static bool operator >= ( Radian l, Radian r ) { return l.mRad >= r.mRad; }
        inline static bool operator >  ( Radian l, Radian r ) { return l.mRad >  r.mRad; }

        virtual bool Equals(Radian other) { return *this == other; }
        virtual int CompareTo(Radian other)
        {
            if (mRad < other.mRad) return -1;
            if (mRad > other.mRad) return 1;
            return 0;
        }

        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("Radian({0})", mRad);
        }

        inline static operator Ogre::Radian& (Radian& obj)
        {
            return reinterpret_cast<Ogre::Radian&>(obj);
        }
        inline static operator const Radian& ( const Ogre::Radian& obj)
        {
            return reinterpret_cast<const Radian&>(obj);
        }
        inline static operator const Radian& ( const Ogre::Radian* pobj)
        {
            return reinterpret_cast<const Radian&>(*pobj);
        }
    };

    /** <summary>Wrapper class which indicates a given angle value is in Degrees.</summary>
    <remarks>
    Degree values are interchangeable with Radian values, and conversions
    will be done automatically between them.
    </remarks>
    */
    [Serializable]
    public value class Degree : IEquatable<Degree>, IComparable<Degree>
    {
        Real mDeg;

    public:
        explicit Degree ( Real d ) : mDeg(d) {}
        Degree ( Radian r ) : mDeg(r.ValueDegrees) {}
        inline static operator Degree ( Real f ) { return Degree(f); }
        inline static operator Degree ( Radian r ) { return Degree(r.ValueDegrees); }

        property Real ValueDegrees { Real get() { return mDeg; } }
        property Real ValueRadians { Real get(); }
        property Real ValueAngleUnits { Real get(); }

        inline static Degree operator + ( Degree l, Degree d ) { return Degree ( l.mDeg + d.mDeg ); }
        inline static Degree operator + ( Degree l, Radian r ) { return Degree ( l.mDeg + r.ValueDegrees ); }
        inline static Degree operator - (Degree d) { return Degree(-d.mDeg); }
        inline static Degree operator - ( Degree l, Degree d ) { return Degree ( l.mDeg - d.mDeg ); }
        inline static Degree operator - ( Degree l, Radian r ) { return Degree ( l.mDeg - r.ValueDegrees ); }
        inline static Degree operator * ( Degree l, Real f ) { return Degree ( l.mDeg * f ); }
        inline static Degree operator * ( Real f, Degree r ) { return Degree ( r.mDeg * f ); }
        inline static Degree operator * ( Degree l, Degree f ) { return Degree ( l.mDeg * f.mDeg ); }
        inline static Degree operator / ( Degree l, Real f ) { return Degree ( l.mDeg / f ); }
        inline static Degree operator / ( Real f, Degree r ) { return Degree ( f / r.mDeg ); }

        inline static bool operator <  ( Degree l, Degree d ) { return l.mDeg <  d.mDeg; }
        inline static bool operator <= ( Degree l, Degree d ) { return l.mDeg <= d.mDeg; }
        inline static bool operator == ( Degree l, Degree d ) { return l.mDeg == d.mDeg; }
        inline static bool operator != ( Degree l, Degree d ) { return l.mDeg != d.mDeg; }
        inline static bool operator >= ( Degree l, Degree d ) { return l.mDeg >= d.mDeg; }
        inline static bool operator >  ( Degree l, Degree d ) { return l.mDeg >  d.mDeg; }

        virtual bool Equals(Degree other) { return *this == other; }
        virtual int CompareTo(Degree other)
        {
            if (mDeg < other.mDeg) return -1;
            if (mDeg > other.mDeg) return 1;
            return 0;
        }

        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("Degree({0})", mDeg);
        }

        inline static operator Ogre::Degree& (Degree& obj)
        {
            return reinterpret_cast<Ogre::Degree&>(obj);
        }
        inline static operator const Degree& ( const Ogre::Degree& obj)
        {
            return reinterpret_cast<const Degree&>(obj);
        }
        inline static operator const Degree& ( const Ogre::Degree* pobj)
        {
            return reinterpret_cast<const Degree&>(*pobj);
        }
    };

    /** <summary>Wrapper class which identifies a value as the currently default angle
    type, as defined by Math::setAngleUnit.</summary>
    <remarks>
    Angle values will be automatically converted between radians and degrees,
    as appropriate.
    </remarks>
    */
    [Serializable]
    public value class Angle : IEquatable<Angle>, IComparable<Angle>
    {
        Real mAngle;
    public:
        explicit Angle ( Real angle ) : mAngle(angle) {}
        static operator Radian( Angle a );
        static operator Degree( Angle a );

        virtual bool Equals(Angle other) { return mAngle == other.mAngle; }
        virtual int CompareTo(Angle other)
        {
            if (mAngle < other.mAngle) return -1;
            if (mAngle > other.mAngle) return 1;
            return 0;
        }
    };

    // these functions could not be defined within the class definition of class
    // Radian because they required class Degree to be defined
    inline Radian::Radian ( Degree d ) : mRad(d.ValueRadians) {
    }
    inline Radian::operator Radian ( Degree d ) {
        return Radian(d.ValueRadians);
    }
    inline Radian Radian::operator + ( Radian l, Degree d ) {
        return Radian ( l.mRad + d.ValueRadians );
    }
    inline Radian Radian::operator - ( Radian l, Degree d ) {
        return Radian ( l.mRad - d.ValueRadians );
    }

    /** <summary>Class to provide access to common mathematical functions.</summary>
    <remarks>
    Most of the maths functions are aliased versions of the C runtime
    library functions. They are aliased here to provide future
    optimisation opportunities, either from faster RTLs or custom
    math approximations.
    <note>
    This is based on MgcMath.h from
    <a href="http://www.geometrictools.com/">Wild Magic</a>.
    </note>
    </remarks>
    */
    public ref class Math abstract sealed
    {
    public:

        enum class AngleUnit
        {
            AU_DEGREE,
            AU_RADIAN
        };

    protected:
        // angle units used by the api
        static AngleUnit msAngleUnit;

        // Size of the trig tables as determined by constructor.
        static int mTrigTableSize;

        // Radian -> index factor value ( mTrigTableSize / 2 * PI )
        static Real mTrigTableFactor;
        static array<Real>^ mSinTable;
        static array<Real>^ mTanTable;

        /** <summary>Private function to build trig tables.</summary>
        */
        static void buildTrigTables();

        static Real SinTable (Real fValue);
        static Real TanTable (Real fValue);

        static System::Random^ mRandomizer = gcnew System::Random;

    public:
        /** <summary>Static constructor.</summary> */
        static Math();

        /** <summary>Gets or Sets the size of the tables used to implement Sin, Cos, and Tan table lookups.</summary> */
        static property int TrigTableSize
        {
            int get() { return mTrigTableSize; }
            void set(int trigTableSize);
        }

        static inline int IAbs (int iValue) { return ( iValue >= 0 ? iValue : -iValue ); }
        static inline int ICeil (float fValue) { return int(ceil(fValue)); }
        static inline int IFloor (float fValue) { return int(floor(fValue)); }
        static int ISign (int iValue);

        /** <summary>Absolute value function</summary>
        <param name="fValue">The value whose absolute value will be returned.</param>
        */
        static inline Real Abs (Real fValue) { return Real(System::Math::Abs(fValue)); }

        /** <summary>Absolute value function</summary>
        <param name="dValue">The value, in degrees, whose absolute value will be returned.</param>
        */
        static inline Degree Abs (Degree dValue) { return Degree(System::Math::Abs(dValue.ValueDegrees)); }

        /** <summary>Absolute value function</summary>
        <param name="rValue">The value, in radians, whose absolute value will be returned.</param>
        */
        static inline Radian Abs (Radian rValue) { return Radian(System::Math::Abs(rValue.ValueRadians)); }

        /** <summary>Arc cosine function</summary>
        <param name="fValue">The value whose arc cosine will be returned.</param>
        */
        static Radian ACos (Real fValue);

        /** <summary>Arc sine function</summary>
        <param name="fValue">The value whose arc sine will be returned.</param>
        */
        static Radian ASin (Real fValue);

        /** <summary>Arc tangent function</summary>
        <param name="fValue">The value whose arc tangent will be returned.</param>
        */
        static inline Radian ATan (Real fValue) { return Radian(System::Math::Atan(fValue)); }

        /** <summary>Arc tangent between two values function</summary>
        <param name="fY">The first value to calculate the arc tangent with.</param>
        <param name="fX">The second value to calculate the arc tangent with.</param>
        */
        static inline Radian ATan2 (Real fY, Real fX) { return Radian(System::Math::Atan2(fY,fX)); }

        /** <summary>Ceiling function</summary>
        <param name="fValue">The value to round up to the nearest integer.</param>
        <returns>Returns the smallest following integer.</returns>
        */
        static inline Real Ceil (Real fValue) { return Real(System::Math::Ceiling(fValue)); }

        /** <summary>Cosine function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Cos (Radian fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Cos(fValue.ValueRadians)) : SinTable(fValue.ValueRadians + HALF_PI);
        }
        /** <summary>Cosine function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Cos (Radian fValue) {
            return Real(System::Math::Cos(fValue.ValueRadians));
        }
        /** <summary>Cosine function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Cos (Real fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Cos(fValue)) : SinTable(fValue + HALF_PI);
        }
        /** <summary>Cosine function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Cos (Real fValue) {
            return Real(System::Math::Cos(fValue));
        }

        static inline Real Exp (Real fValue) { return Real(System::Math::Exp(fValue)); }

        /** <summary>Floor function</summary>
        <param name="fValue">The value to round down to the nearest integer.</param>
        <returns>Returns the largest previous integer.</returns>
        */
        static inline Real Floor (Real fValue) { return Real(System::Math::Floor(fValue)); }

        static inline Real Log (Real fValue) { return Real(System::Math::Log(fValue)); }

        static inline Real Log2 (Real fValue) { return Real(System::Math::Log(fValue,2.0f)); }

        static inline Real LogN (Real base, Real fValue) { return Real(System::Math::Log(fValue,base)); }

        static inline Real Pow (Real fBase, Real fExponent) { return Real(System::Math::Pow(fBase,fExponent)); }

        static Real Sign (Real fValue);
        static inline Radian Sign ( Radian rValue )
        {
            return Radian(Sign(rValue.ValueRadians));
        }
        static inline Degree Sign ( Degree dValue )
        {
            return Degree(Sign(dValue.ValueDegrees));
        }

        /** <summary>Sine function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Sin (Radian fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Sin(fValue.ValueRadians)) : SinTable(fValue.ValueRadians);
        }
        /** <summary>Sine function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Sin (Radian fValue) {
            return Real(System::Math::Sin(fValue.ValueRadians));
        }
        /** <summary>Sine function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Sin (Real fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Sin(fValue)) : SinTable(fValue);
        }
        /** <summary>Sine function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Sin (Real fValue) {
            return Real(System::Math::Sin(fValue));
        }

        /** <summary>Square function.</summary>
        <param name="fValue">The value to be squared.</param>
        */
        static inline Real Sqr (Real fValue) { return fValue*fValue; }

        /** <summary>Square root function.</summary>
        <param name="fValue">The value whose square root will be calculated.</param>
        */
        static inline Real Sqrt (Real fValue) { return Real(System::Math::Sqrt(fValue)); }

        /** <summary>Square root function.</summary>
        <param name="fValue">The value, in radians, whose square root will be calculated.</param>
        <returns>The square root of the angle in radians.</returns>
        */
        static inline Radian Sqrt (Radian fValue) { return Radian(System::Math::Sqrt(fValue.ValueRadians)); }

        /** <summary>Square root function.</summary>
        <param name="fValue">The value, in degrees, whose square root will be calculated.</param>
        <returns>The square root of the angle in degrees.</returns>
        */
        static inline Degree Sqrt (Degree fValue) { return Degree(System::Math::Sqrt(fValue.ValueDegrees)); }

        /** <summary>Inverse square root i.e. 1 / Sqrt(x), good for vector
        normalisation.</summary>
        <param name="fValue">The value whose inverse square root will be calculated.</param>
        */
        static Real InvSqrt(Real fValue);

        /** <summary>Generate a random number of unit length.</summary>
        <returns>A random number in the range from [0,1].</returns>
        */
        static Real UnitRandom ();

        /** <summary>Generate a random number within the range provided.</summary>
        <param name="fLow">The lower bound of the range.</param>
        <param name="fHigh">The upper bound of the range.</param>
        <returns>A random number in the range from [<paramref name="fLow" />,<paramref name="fHigh" />].</returns>
        */
        static Real RangeRandom (Real fLow, Real fHigh);

        /** <summary>Generate a random number in the range [-1,1].</summary>
        <returns>A random number in the range from [-1,1].</returns>
        */
        static Real SymmetricRandom ();

        /** <summary>Tangent function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Tan (Radian fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Tan(fValue.ValueRadians)) : TanTable(fValue.ValueRadians);
        }
        /** <summary>Tangent function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Tan (Radian fValue) {
            return Real(System::Math::Tan(fValue.ValueRadians));
        }
        /** <summary>Tangent function.</summary>
        <param name="fValue">Angle in radians</param>
        <param name="useTables">If true, uses lookup tables rather than
        calculation - faster but less accurate.</param>
        */
        static inline Real Tan (Real fValue, bool useTables) {
            return (!useTables) ? Real(System::Math::Tan(fValue)) : TanTable(fValue);
        }
        /** <summary>Tangent function.</summary>
        <param name="fValue">Angle in radians</param>
        */
        static inline Real Tan (Real fValue) {
            return Real(System::Math::Tan(fValue));
        }

        static inline Real DegreesToRadians(Real degrees) { return degrees * fDeg2Rad; }
        static inline Real RadiansToDegrees(Real radians) { return radians * fRad2Deg; }

        /** <summary>These functions used to set the assumed angle units (radians or degrees)
        expected when using the Angle type.</summary>
        <remarks>
        You can set this directly after creating a new Root, and also before/after resource creation,
        depending on whether you want the change to affect resource files.
        </remarks>
        */
        static void SetAngleUnit(AngleUnit unit);
        /** <summary>Get the unit being used for angles.</summary> */
        static AngleUnit GetAngleUnit(void);

        /** <summary>Convert from the current AngleUnit to radians.</summary> */
        static Real AngleUnitsToRadians(Real units);
        /** <summary>Convert from radians to the current AngleUnit.</summary> */
        static Real RadiansToAngleUnits(Real radians);
        /** <summary>Convert from the current AngleUnit to degrees.</summary> */
        static Real AngleUnitsToDegrees(Real units);
        /** <summary>Convert from degrees to the current AngleUnit.</summary> */
        static Real DegreesToAngleUnits(Real degrees);

        /** <summary>Checks whether a given point is inside a triangle, in a
        2-dimensional (Cartesian) space.</summary>
        <remarks>
        The vertices of the triangle must be given in either
        trigonometrical (anticlockwise) or inverse trigonometrical
        (clockwise) order.
        </remarks>
        <param name="p">The point.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <returns>
        If the point resides in the triangle, <b>true</b> is
        returned.
        If the point is outside the triangle, <b>false</b> is
        returned.
        </returns>
        */
        static bool PointInTri2D(Vector2 p, Vector2 a,
            Vector2 b, Vector2 c);

        /** <summary>Checks whether a given 3D point is inside a triangle.</summary>
        <remarks>
        The vertices of the triangle must be given in either
        trigonometrical (anticlockwise) or inverse trigonometrical
        (clockwise) order, and the point must be guaranteed to be in the
        same plane as the triangle
        </remarks>
        <param name="p">The point.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="normal">The triangle plane's normal (passed in rather than calculated
        on demand since the callermay already have it)</param>
        <returns>
        If the point resides in the triangle, <b>true</b> is
        returned.
        If the point is outside the triangle, <b>false</b> is
        returned.
        </returns>
        */
        static bool PointInTri3D(Vector3 p, Vector3 a,
            Vector3 b, Vector3 c, Vector3 normal);
        /** <summary>Ray / plane intersection, returns boolean result and distance.</summary> */
        static Pair<bool, Real> Intersects(Ray ray, Plane plane);

        /** <summary>Ray / sphere intersection, returns boolean result and distance.</summary> */
        static Pair<bool, Real> Intersects(Ray ray, Sphere sphere,
            bool discardInside);
        /** <summary>Ray / sphere intersection, returns boolean result and distance.</summary> */
        static Pair<bool, Real> Intersects(Ray ray, Sphere sphere);

        /** <summary>Ray / box intersection, returns boolean result and distance.</summary> */
        static Pair<bool, Real> Intersects(Ray ray, AxisAlignedBox^ box);

        /** <summary>Ray / box intersection, returns boolean result and two intersection distance.</summary>
        <param name="ray">The ray.</param>
        <param name="box">The box.</param>
        <param name="d1">A real pointer to retrieve the near intersection distance
        from the ray origin, maybe <b>null</b> which means don't care
        about the near intersection distance.</param>
        <param name="d2">A real pointer to retrieve the far intersection distance
        from the ray origin, maybe <b>null</b> which means don't care
        about the far intersection distance.</param>
        <returns>
        If the ray is Intersects the box, <b>true</b> is returned, and
        the near intersection distance is return by <i>d1</i>, the
        far intersection distance is return by <i>d2</i>. Guarantee
        <b>0</b> &lt;= <i>d1</i> &lt;= <i>d2</i>.
        If the ray isn't Intersects the box, <b>false</b> is returned, and
        <i>d1</i> and <i>d2</i> is unmodified.
        </returns>
        */
        static bool Intersects(Ray ray, AxisAlignedBox^ box,
            [Out] Real% d1, [Out] Real% d2);

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="normal">The triangle plane's normal (passed in rather than calculated
        on demand since the callermay already have it), doesn't need
        normalised since we don't care.</param>
        <param name="positiveSide">Intersect with "positive side" of the triangle</param>
        <param name="negativeSide">Intersect with "negative side" of the triangle</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c, Vector3 normal,
            bool positiveSide, bool negativeSide);

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="normal">The triangle plane's normal (passed in rather than calculated
        on demand since the callermay already have it), doesn't need
        normalised since we don't care.</param>
        <param name="positiveSide">Intersect with "positive side" of the triangle</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c, Vector3 normal,
            bool positiveSide);

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="normal">The triangle plane's normal (passed in rather than calculated
        on demand since the callermay already have it), doesn't need
        normalised since we don't care.</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c, Vector3 normal );

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="positiveSide">Intersect with "positive side" of the triangle</param>
        <param name="negativeSide">Intersect with "negative side" of the triangle</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c,
            bool positiveSide, bool negativeSide);

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <param name="positiveSide">Intersect with "positive side" of the triangle</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c,
            bool positiveSide);

        /** <summary>Ray / triangle intersection, returns boolean result and distance.</summary>
        <param name="ray">The ray.</param>
        <param name="a">The triangle's first vertex.</param>
        <param name="b">The triangle's second vertex.</param>
        <param name="c">The triangle's third vertex.</param>
        <returns>
        If the ray is Intersects the triangle, a pair of <b>true</b> and the
        distance between intersection point and ray origin returned.
        If the ray isn't Intersects the triangle, a pair of <b>false</b> and
        <b>0</b> returned.
        </returns>
        */
        static Pair<bool, Real> Intersects(Ray ray, Vector3 a,
            Vector3 b, Vector3 c );

        /** <summary>Sphere / box intersection test.</summary> */
        static bool Intersects(Sphere sphere, AxisAlignedBox^ box);

        /** <summary>Plane / box intersection test.</summary> */
        static bool Intersects(Plane plane, AxisAlignedBox^ box);

        /** <summary>Ray / convex plane list intersection test.</summary>
        <param name="ray">The ray to test with</param>
        <param name="planeList">List of planes which form a convex volume</param>
        <param name="normalIsOutside">Does the normal point outside the volume</param>
        */
        static Pair<bool, Real> Intersects(
            Ray ray, Collections::Generic::List<Plane>^ planeList,
            bool normalIsOutside);
        /** <summary>Ray / convex plane list intersection test.</summary>
        <param name="ray">The ray to test with</param>
        <param name="planeList">List of planes which form a convex volume</param>
        <param name="normalIsOutside">Does the normal point outside the volume</param>
        */
        static Pair<bool, Real> Intersects(
            Ray ray, Collections::Generic::IEnumerable<Plane>^ planeList,
            bool normalIsOutside);

        /** <summary>Sphere / plane intersection test.</summary>
        <remarks>NB just do a plane.getDistance(sphere.getCenter()) for more detail!</remarks>
        */
        static bool Intersects(Sphere sphere, Plane plane);

        /** <summary>Compare 2 reals, using tolerance for inaccuracies.</summary>
        */
        static bool RealEqual(Real a, Real b, Real tolerance);
        /** <summary>Compare 2 reals, using tolerance for inaccuracies.</summary>
        */
        static bool RealEqual(Real a, Real b )
        {
            return RealEqual(a, b, Real::Epsilon);
        }

        /** <summary>Calculates the tangent space vector for a given set of positions / texture coords.</summary> */
        static Vector3 CalculateTangentSpaceVector(
            Vector3 position1, Vector3 position2, Vector3 position3,
            Real u1, Real v1, Real u2, Real v2, Real u3, Real v3);

        /** <summary>Build a reflection matrix for the passed in plane.</summary> */
        static Matrix4^ BuildReflectionMatrix(Plane p);
        /** <summary>Calculate a face normal, including the w component which is the offset from the origin.</summary> */
        static Vector4 CalculateFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3);
        /** <summary>Calculate a face normal, no w-information.</summary> */
        static Vector3 CalculateBasicFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3);
        /** <summary>Calculate a face normal without normalize, including the w component which is the offset from the origin.</summary> */
        static Vector4 CalculateFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3);
        /** <summary>Calculate a face normal without normalize, no w-information.</summary> */
        static Vector3 CalculateBasicFaceNormalWithoutNormalize(Vector3 v1, Vector3 v2, Vector3 v3);

        /** <summary>Generates a value based on the gaussian (normal) distribution function
        with the given offset and scale parameters.</summary>
        */
        static Real GaussianDistribution(Real x, Real offset, Real scale);
        /** <summary>Generates a value based on the gaussian (normal) distribution function
        with the given offset and scale parameters.</summary>
        */
        static Real GaussianDistribution(Real x, Real offset)
        {
            return GaussianDistribution(x, offset, 1.0f);
        }
        /** <summary>Generates a value based on the gaussian (normal) distribution function
        with the given offset and scale parameters.</summary>
        */
        static Real GaussianDistribution(Real x)
        {
            return GaussianDistribution(x, 0.0f, 1.0f);
        }

        /** <summary>Clamp a value within an inclusive range.</summary> */
        static Real Clamp(Real val, Real minval, Real maxval)
        {
            assert (minval <= maxval && "Invalid clamp range");
            return System::Math::Max(System::Math::Min(val, maxval), minval);
        }

        static Matrix4^ MakeViewMatrix(Vector3 position, Quaternion orientation, 
            Matrix4^ reflectMatrix);
        static Matrix4^ MakeViewMatrix(Vector3 position, Quaternion orientation);

        /** <summary>Get a bounding radius value from a bounding box.</summary> */
        static Real BoundingRadiusFromAABB(AxisAlignedBox^ aabb);

        literal Real POS_INFINITY = Real::PositiveInfinity;
        literal Real NEG_INFINITY = Real::NegativeInfinity;
        literal Real PI = Real( System::Math::PI );
        literal Real TWO_PI = Real( 2.0 * PI );
        literal Real HALF_PI = Real( 0.5 * PI );
        literal Real fDeg2Rad = PI / Real(180.0);
        literal Real fRad2Deg = Real(180.0) / PI;
    };

    // these functions must be defined down here, because they rely on the
    // angle unit conversion functions in class Math:

    inline Real Radian::ValueDegrees::get()
    {
        return Math::RadiansToDegrees ( mRad );
    }

    inline Real Radian::ValueAngleUnits::get()
    {
        return Math::RadiansToAngleUnits ( mRad );
    }

    inline Real Degree::ValueRadians::get()
    {
        return Math::DegreesToRadians ( mDeg );
    }

    inline Real Degree::ValueAngleUnits::get()
    {
        return Math::DegreesToAngleUnits ( mDeg );
    }

    inline Angle::operator Radian( Angle a )
    {
        return Radian(Math::AngleUnitsToRadians(a.mAngle));
    }

    inline Angle::operator Degree( Angle a )
    {
        return Degree(Math::AngleUnitsToDegrees(a.mAngle));
    }
}