#pragma once

#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgreCommon.h"
#pragma managed(pop)
#pragma warning(pop)

namespace Mogre
{

    [Serializable]
    public value class FloatRect
    {
    public:
        inline static operator Ogre::FloatRect& (FloatRect& obj)
        {
            return reinterpret_cast<Ogre::FloatRect&>(obj);
        }
        inline static operator const FloatRect& ( const Ogre::FloatRect& obj)
        {
            return reinterpret_cast<const FloatRect&>(obj);
        }
        inline static operator const FloatRect& ( const Ogre::FloatRect* pobj)
        {
            return reinterpret_cast<const FloatRect&>(*pobj);
        }

        float left, top, right, bottom;

        FloatRect( float l, float t, float r, float b )
            : left( l ), top( t ), right( r ), bottom( b )
        {
        }
        property float Width
        {
            float get()
            {
                return right - left;
            }
        }
        property float Height
        {
            float get()
            {
                return bottom - top;
            }
        }
    };

    [Serializable]
    public value class RealRect
    {
    public:
        inline static operator Ogre::RealRect& (RealRect& obj)
        {
            return reinterpret_cast<Ogre::RealRect&>(obj);
        }
        inline static operator const RealRect& ( const Ogre::RealRect& obj)
        {
            return reinterpret_cast<const RealRect&>(obj);
        }
        inline static operator const RealRect& ( const Ogre::RealRect* pobj)
        {
            return reinterpret_cast<const RealRect&>(*pobj);
        }

        Mogre::Real left, top, right, bottom;

        RealRect( float l, float t, float r, float b )
            : left( l ), top( t ), right( r ), bottom( b )
        {
        }
        property Mogre::Real Width
        {
            Mogre::Real get()
            {
                return right - left;
            }
        }
        property Mogre::Real Height
        {
            Mogre::Real get()
            {
                return bottom - top;
            }
        }
    };

    [Serializable]
    public value class Rect
    {
    public:
        inline static operator Ogre::Rect& (Rect& obj)
        {
            return reinterpret_cast<Ogre::Rect&>(obj);
        }
        inline static operator const Rect& ( const Ogre::Rect& obj)
        {
            return reinterpret_cast<const Rect&>(obj);
        }
        inline static operator const Rect& ( const Ogre::Rect* pobj)
        {
            return reinterpret_cast<const Rect&>(*pobj);
        }

        long left, top, right, bottom;

        Rect( long l, long t, long r, long b )
            : left( l ), top( t ), right( r ), bottom( b )
        {
        }
        property long Width
        {
            long get()
            {
                return right - left;
            }
        }
        property long Height
        {
            long get()
            {
                return bottom - top;
            }
        }
    };



}