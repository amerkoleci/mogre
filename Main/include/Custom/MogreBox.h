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
#include "OgreCommon.h"
#pragma managed(pop)
#pragma warning(pop)

namespace Mogre
{
    value class Box;
    value class PixelBox;
}

#include "MogrePixelFormat.h"

namespace Mogre
{
    /** <summary>Structure used to define a box in a 3-D integer space.
    Note that the left, top, and front edges are included but the right, 
    bottom and top ones are not.</summary>
    */
    [Serializable]
    public value class Box
    {
    public:
        inline static operator Ogre::Box& (Box& obj)
        {
            return reinterpret_cast<Ogre::Box&>(obj);
        }
        inline static operator const Box& ( const Ogre::Box& obj)
        {
            return reinterpret_cast<const Box&>(obj);
        }
        inline static operator const Box& ( const Ogre::Box* pobj)
        {
            return reinterpret_cast<const Box&>(*pobj);
        }

        size_t left, top, right, bottom, front, back;
        /** <summary>Define a box from left, top, right and bottom coordinates
        This box will have depth one (front=0 and back=1).</summary>
        <param name="l">x value of left edge</param>
        <param name="t">y value of top edge</param>
        <param name="r">x value of right edge</param>
        <param name="b">y value of bottom edge</param>
        <remarks>
        <note>Note that the left, top, and front edges are included 
        but the right, bottom and top ones are not.</note>
        </remarks>
        */
        Box( size_t l, size_t t, size_t r, size_t b ):
            left(l),
            top(t),
            right(r),
            bottom(b),
            front(0),
            back(1)
        {
            assert(right >= left && bottom >= top && back >= front);
        }
        /** <summary>Define a box from left, top, front, right, bottom and back
        coordinates.</summary>
        <param name="l">x value of left edge</param>
        <param name="t">y value of top edge</param>
        <param name="ff">z value of front edge</param>
        <param name="r">x value of right edge</param>
        <param name="b">y value of bottom edge</param>
        <param name="bb">z value of back edge</param>
        <remarks>
        <note>Note that the left, top, and front edges are included 
        but the right, bottom and top ones are not.</note>
        </remarks>
        */
        Box( size_t l, size_t t, size_t ff, size_t r, size_t b, size_t bb ):
            left(l),
            top(t),
            right(r),
            bottom(b),
            front(ff),
            back(bb)
        {
            assert(right >= left && bottom >= top && back >= front);
        }

        /// <summary>Return true if the other box is a part of this one</summary>
        bool Contains(Box def)
        {
            return (def.left >= left && def.top >= top && def.front >= front &&
                def.right <= right && def.bottom <= bottom && def.back <= back);
        }

        /// <summary>Get the width of this box</summary>
        property size_t Width
        {
            size_t get() { return right-left; }
        }

        /// <summary>Get the height of this box</summary>
        property size_t Height
        {
            size_t get() { return bottom-top; }
        }

        /// <summary>Get the depth of this box</summary>
        property size_t Depth
        {
            size_t get() { return back-front; }
        }
    };


    /** <summary>A primitive describing a volume (3D), image (2D) or line (1D) of pixels in memory.</summary>
    <remarks>
    In case of a rectangle, depth must be 1. 
    Pixels are stored as a succession of "depth" slices, each containing "height" rows of 
    "width" pixels.
    </remarks>
    */
    public value class PixelBox
    {
    public:
        inline static operator Ogre::PixelBox& (PixelBox& obj)
        {
            return reinterpret_cast<Ogre::PixelBox&>(obj);
        }
        inline static operator const PixelBox& ( const Ogre::PixelBox& obj)
        {
            return reinterpret_cast<const PixelBox&>(obj);
        }
        inline static operator const PixelBox& ( const Ogre::PixelBox* pobj)
        {
            return reinterpret_cast<const PixelBox&>(*pobj);
        }

        Box box;

        /** <summary>Constructor providing extents in the form of a Box object. This constructor
        assumes the pixel data is laid out consecutively in memory. (this
        means row after row, slice after slice, with no space in between)</summary>
        <param name="extents">Extents of the region defined by data</param>
        <param name="pixelFormat">Format of this buffer</param>
        <param name="pixelData">Pointer to the actual data</param>
        */
        PixelBox(Box extents, PixelFormat pixelFormat, IntPtr pixelData):
        box(extents), data(pixelData), format(pixelFormat)
        {
            setConsecutive();
        }

        /** <summary>Constructor providing extents in the form of a Box object. This constructor
        assumes the pixel data is laid out consecutively in memory. (this
        means row after row, slice after slice, with no space in between)</summary>
        <param name="extents">Extents of the region defined by data</param>
        <param name="pixelFormat">Format of this buffer</param>
        */
        PixelBox(Box extents, PixelFormat pixelFormat):
        box(extents), data(0), format(pixelFormat)
        {
            setConsecutive();
        }

        /** <summary>Constructor providing width, height and depth. This constructor
        assumes the pixel data is laid out consecutively in memory. (this
        means row after row, slice after slice, with no space in between)</summary>
        <param name="width">Width of the region</param>
        <param name="height">Height of the region</param>
        <param name="depth">Depth of the region</param>
        <param name="pixelFormat">Format of this buffer</param>
        <param name="pixelData">Pointer to the actual data</param>
        */
        PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat, IntPtr pixelData):
        box( Box(0, 0, 0, width, height, depth) ),
            data(pixelData), format(pixelFormat)
        {
            setConsecutive();
        }

        /** <summary>Constructor providing width, height and depth. This constructor
        assumes the pixel data is laid out consecutively in memory. (this
        means row after row, slice after slice, with no space in between)</summary>
        <param name="width">Width of the region</param>
        <param name="height">Height of the region</param>
        <param name="depth">Depth of the region</param>
        <param name="pixelFormat">Format of this buffer</param>
        */
        PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat):
        box( Box(0, 0, 0, width, height, depth) ),
            data(0), format(pixelFormat)
        {
            setConsecutive();
        }

        /// <summary>The data pointer</summary>
        IntPtr data;
        /// <summary>The pixel format</summary>
        PixelFormat format;
        /** <summary>Number of elements between the leftmost pixel of one row and the left
        pixel of the next. This value must always be equal to getWidth() (consecutive) 
        for compressed formats.</summary>
        */
        size_t rowPitch;
        /** <summary>Number of elements between the top left pixel of one (depth) slice and 
        the top left pixel of the next. This can be a negative value. Must be a multiple of
        rowPitch. This value must always be equal to getWidth()*getHeight() (consecutive) 
        for compressed formats.</summary>
        */
        size_t slicePitch;

        /** <summary>Set the rowPitch and slicePitch so that the buffer is laid out consecutive 
        in memory.</summary>
        */
        void setConsecutive()
        {
            rowPitch = box.Width;
            slicePitch = box.Width*box.Height;
        }
        /** <summary>Get the number of elements between one past the rightmost pixel of 
        one row and the leftmost pixel of the next row. (IE this is zero if rows
        are consecutive).</summary>
        */
        property size_t RowSkip
        {
            size_t get() { return rowPitch - box.Width; }
        }

        /** <summary>Get the number of elements between one past the right bottom pixel of
        one slice and the left top pixel of the next slice. (IE this is zero if slices
        are consecutive).</summary>
        */
        property size_t SliceSkip
        {
            size_t get() { return slicePitch - (box.Height * rowPitch); }
        }

        /** <summary>Return whether this buffer is laid out consecutive in memory (ie the pitches
        are equal to the dimensions)</summary>
        */
        property bool IsConsecutive
        {
            bool get()
            { 
                return rowPitch == box.Width && slicePitch == box.Width*box.Height; 
            }
        }
        /** <summary>Return the size (in bytes) this image would take if it was
        laid out consecutive in memory</summary>
        */
        size_t GetConsecutiveSize();

        /** <summary>Return a subvolume of this PixelBox.</summary>
        <param name="def">Defines the bounds of the subregion to return</param>
        <returns>A pixel box describing the region and the data in it</returns>
        <remarks>This function does not copy any data, it just returns
        a PixelBox object with a data pointer pointing somewhere inside 
        the data of object.</remarks>
        <exception cref="ArgumentException">if def is not fully contained</exception>
        */
        PixelBox GetSubVolume(Box def);
    };
}