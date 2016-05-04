#pragma once

#pragma warning(push, 0)
#pragma managed(push, off)
#include "OgreRectangle.h"
#pragma managed(pop)
#pragma warning(pop)

namespace Mogre
{
    [Serializable]
    public value class Rectangle
    {
    public:
        Real left;
        Real top;
        Real right;
        Real bottom;

        inline bool Inside(Real x, Real y) { return x >= left && x <= right && y >= top && y <= bottom; }

        inline static operator Ogre::Rectangle& (Rectangle& obj)
        {
            return reinterpret_cast<Ogre::Rectangle&>(obj);
        }
        inline static operator const Rectangle& ( const Ogre::Rectangle& obj)
        {
            return reinterpret_cast<const Rectangle&>(obj);
        }
        inline static operator const Rectangle& ( const Ogre::Rectangle* pobj)
        {
            return reinterpret_cast<const Rectangle&>(*pobj);
        }
    };
}