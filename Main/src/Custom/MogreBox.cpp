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

namespace Mogre
{
    size_t PixelBox::GetConsecutiveSize()
    { 
        return PixelUtil::GetMemorySize(box.Width, box.Height, box.Depth, format); 
    }

    Mogre::PixelBox PixelBox::GetSubVolume(Box def)
    {
        if(PixelUtil::IsCompressed(format))
        {
            if(def.left == box.left && def.top == box.top && def.front == box.front &&
                def.right == box.right && def.bottom == box.bottom && def.back == box.back)
            {
                // Entire buffer is being queried
                return *this;
            }
            throw gcnew ArgumentException("Cannot return subvolume of compressed PixelBuffer", "def");
        }
        if(!box.Contains(def))
            throw gcnew ArgumentException("Bounds out of range", "def");

        const size_t elemSize = PixelUtil::GetNumElemBytes(format);
        // Calculate new data origin
        PixelBox rval(def, format, (IntPtr)(void*) ( ((uint8*)(void*)data) 
            + ((def.left-box.left)*elemSize)
            + ((def.top-box.top)*rowPitch*elemSize)
            + ((def.front-box.front)*slicePitch*elemSize) )
            );		

        rval.rowPitch = rowPitch;
        rval.slicePitch = slicePitch;
        rval.format = format;

        return rval;
    }
}