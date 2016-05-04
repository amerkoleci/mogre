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
#include "OgreColourValue.h"
#pragma managed(pop)
#pragma warning(pop)
#include "Prerequisites.h"

namespace Mogre
{
    typedef Ogre::RGBA RGBA;
    typedef Ogre::ARGB ARGB;
    typedef Ogre::ABGR ABGR;
    typedef Ogre::BGRA BGRA;

    /** <summary>Class representing colour.</summary>
    <remarks>
    Colour is represented as 4 components, each of which is a
    floating-point value from 0.0 to 1.0.
    <para/>
    The 3 'normal' colour components are red, green and blue, a higher
    number indicating greater amounts of that component in the colour.
    The forth component is the 'alpha' value, which represents
    transparency. In this case, 0.0 is completely transparent and 1.0 is
    fully opaque.
    </remarks>
    */
    [Serializable]
    [StructLayout(LayoutKind::Sequential)]
    public value class ColourValue : IEquatable<ColourValue>
    {
    public:
        inline static operator Ogre::ColourValue& (ColourValue& obj)
        {
            return reinterpret_cast<Ogre::ColourValue&>(obj);
        }
        inline static operator const ColourValue& ( const Ogre::ColourValue& obj)
        {
            return reinterpret_cast<const ColourValue&>(obj);
        }
        inline static operator const ColourValue& ( const Ogre::ColourValue* pobj)
        {
            return reinterpret_cast<const ColourValue&>(*pobj);
        }

        static initonly ColourValue ZERO = ColourValue(0.0,0.0,0.0,0.0);
        static initonly ColourValue Black = ColourValue(0.0,0.0,0.0);
        static initonly ColourValue White = ColourValue(1.0,1.0,1.0);
        static initonly ColourValue Red = ColourValue(1.0,0.0,0.0);
        static initonly ColourValue Green = ColourValue(0.0,1.0,0.0);
        static initonly ColourValue Blue = ColourValue(0.0,0.0,1.0);

        explicit ColourValue( float red,
            float green,
            float blue,
            float alpha ) : r(red), g(green), b(blue), a(alpha)
        { }
        explicit ColourValue( float red,
            float green,
            float blue ) : r(red), g(green), b(blue), a(1.0f)
        { }

        static bool operator==(ColourValue lhs, ColourValue rhs);
        static bool operator!=(ColourValue lhs, ColourValue rhs);

        virtual bool Equals(ColourValue other) { return *this == other; }

        float r,g,b,a;

        /** <summary>Retrieves colour as RGBA.</summary>
        */
        RGBA GetAsRGBA(void);

        /** <summary>Retrieves colour as ARGB.</summary>
        */
        ARGB GetAsARGB(void);

        /** <summary>Retrieves colour as BGRA.</summary>
        */
        BGRA GetAsBGRA(void);

        /** <summary>Retrieves colours as ABGR</summary> */
        ABGR GetAsABGR(void);

        /** <summary>Sets colour as RGBA.</summary>
        */
        void SetAsRGBA(const RGBA val);

        /** <summary>Sets colour as ARGB.</summary>
        */
        void SetAsARGB(const ARGB val);

        /** <summary>Sets colour as BGRA.</summary>
        */
        void SetAsBGRA(const BGRA val);

        /** <summary>Sets colour as ABGR.</summary>
        */
        void SetAsABGR(const ABGR val);

        /** <summary>Clamps colour value to the range [0, 1].</summary>
        */
        void Saturate(void)
        {
            if (r < 0)
                r = 0;
            else if (r > 1)
                r = 1;

            if (g < 0)
                g = 0;
            else if (g > 1)
                g = 1;

            if (b < 0)
                b = 0;
            else if (b > 1)
                b = 1;

            if (a < 0)
                a = 0;
            else if (a > 1)
                a = 1;
        }

        /** <summary>As saturate, except that this colour value is unaffected and
        the saturated colour value is returned as a copy.</summary> */
        ColourValue SaturateCopy(void)
        {
            ColourValue ret = *this;
            ret.Saturate();
            return ret;
        }

        property float default[int]
        {
            inline float get(int i)
            {
                assert( i < 4 );

                return *(&r+i);
            }

            inline void set(int i, float value)
            {
                assert( i < 4 );

                *(&r+i) = value;
            }
        }

        // arithmetic operations
        inline static ColourValue operator + ( ColourValue lcol, ColourValue rkVector )
        {
            ColourValue kSum;

            kSum.r = lcol.r + rkVector.r;
            kSum.g = lcol.g + rkVector.g;
            kSum.b = lcol.b + rkVector.b;
            kSum.a = lcol.a + rkVector.a;

            return kSum;
        }

        inline static ColourValue operator - ( ColourValue lcol, ColourValue rkVector )
        {
            ColourValue kDiff;

            kDiff.r = lcol.r - rkVector.r;
            kDiff.g = lcol.g - rkVector.g;
            kDiff.b = lcol.b - rkVector.b;
            kDiff.a = lcol.a - rkVector.a;

            return kDiff;
        }

        inline static ColourValue operator * (ColourValue lcol, const float fScalar )
        {
            ColourValue kProd;

            kProd.r = fScalar*lcol.r;
            kProd.g = fScalar*lcol.g;
            kProd.b = fScalar*lcol.b;
            kProd.a = fScalar*lcol.a;

            return kProd;
        }

        inline static ColourValue operator * ( ColourValue lcol, ColourValue rhs)
        {
            ColourValue kProd;

            kProd.r = rhs.r * lcol.r;
            kProd.g = rhs.g * lcol.g;
            kProd.b = rhs.b * lcol.b;
            kProd.a = rhs.a * lcol.a;

            return kProd;
        }

        inline static ColourValue operator / ( ColourValue lcol, ColourValue rhs)
        {
            ColourValue kProd;

            kProd.r = lcol.r / rhs.r;
            kProd.g = lcol.g / rhs.g;
            kProd.b = lcol.b / rhs.b;
            kProd.a = lcol.a / rhs.a;

            return kProd;
        }

        inline static ColourValue operator / (ColourValue lcol, const float fScalar )
        {
            assert( fScalar != 0.0 );

            ColourValue kDiv;

            float fInv = 1.0f / fScalar;
            kDiv.r = lcol.r * fInv;
            kDiv.g = lcol.g * fInv;
            kDiv.b = lcol.b * fInv;
            kDiv.a = lcol.a * fInv;

            return kDiv;
        }

        inline static ColourValue operator * (const float fScalar, ColourValue rkVector )
        {
            ColourValue kProd;

            kProd.r = fScalar * rkVector.r;
            kProd.g = fScalar * rkVector.g;
            kProd.b = fScalar * rkVector.b;
            kProd.a = fScalar * rkVector.a;

            return kProd;
        }

        /** <summary>Set a colour value from Hue, Saturation and Brightness.</summary>
        <param name="hue">Hue value, scaled to the [0,1] range as opposed to the 0-360</param>
        <param name="saturation">Saturation level, [0,1]</param>
        <param name="brightness">Brightness level, [0,1]</param>
        */
        void SetHSB(Real hue, Real saturation, Real brightness);

        /** <summary>Convert the current colour to Hue, Saturation and Brightness values.</summary>
        <param name="hue">Output hue value, scaled to the [0,1] range as opposed to the 0-360</param>
        <param name="saturation">Output saturation level, [0,1]</param>
        <param name="brightness">Output brightness level, [0,1]</param>
        */
        void GetHSB([Out] Real% hue, [Out] Real% saturation, [Out] Real% brightness);

        /// <inheritdoc />
        virtual System::String^ ToString() override
        {
            return System::String::Format("ColourValue({0}, {1}, {2}, {3})", r, g, b, a);
        }
    };
}