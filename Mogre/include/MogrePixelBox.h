#pragma once

#include "OgreCommon.h"
#include "MogrePixelFormat.h"
#include "Marshalling.h"

namespace Mogre
{
	/** A primitive describing a volume (3D), image (2D) or line (1D) of pixels in memory.
	In case of a rectangle, depth must be 1.
	Pixels are stored as a succession of "depth" slices, each containing "height" rows of
	"width" pixels.
	*/
	public value class PixelBox
	{
	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_VALUECLASS(PixelBox);

		Box box;

		/** Constructor providing extents in the form of a Box object. This constructor
		assumes the pixel data is laid out consecutively in memory. (this
		means row after row, slice after slice, with no space in between)
		@param extents	    Extents of the region defined by data
		@param pixelFormat	Format of this buffer
		@param pixelData	Pointer to the actual data
		*/
		PixelBox(Box extents, PixelFormat pixelFormat, IntPtr pixelData) :
			box(extents), data(pixelData), format(pixelFormat)
		{
			setConsecutive();
		}

		PixelBox(Box extents, PixelFormat pixelFormat) :
			box(extents), data(0), format(pixelFormat)
		{
			setConsecutive();
		}
		/** Constructor providing width, height and depth. This constructor
		assumes the pixel data is laid out consecutively in memory. (this
		means row after row, slice after slice, with no space in between)
		@param width	    Width of the region
		@param height	    Height of the region
		@param depth	    Depth of the region
		@param pixelFormat	Format of this buffer
		@param pixelData    Pointer to the actual data
		*/
		PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat, IntPtr pixelData) :
			box(Box(0, 0, 0, width, height, depth)),
			data(pixelData), format(pixelFormat)
		{
			setConsecutive();
		}

		PixelBox(size_t width, size_t height, size_t depth, PixelFormat pixelFormat) :
			box(Box(0, 0, 0, width, height, depth)),
			data(0), format(pixelFormat)
		{
			setConsecutive();
		}

		/// The data pointer 
		IntPtr data;
		/// The pixel format 
		PixelFormat format;
		/** Number of elements between the leftmost pixel of one row and the left
		pixel of the next. This value must always be equal to getWidth() (consecutive)
		for compressed formats.
		*/
		size_t rowPitch;
		/** Number of elements between the top left pixel of one (depth) slice and
		the top left pixel of the next. This can be a negative value. Must be a multiple of
		rowPitch. This value must always be equal to getWidth()*getHeight() (consecutive)
		for compressed formats.
		*/
		size_t slicePitch;

		/** Set the rowPitch and slicePitch so that the buffer is laid out consecutive
		in memory.
		*/
		void setConsecutive()
		{
			rowPitch = box.Width;
			slicePitch = box.Width*box.Height;
		}
		/**	Get the number of elements between one past the rightmost pixel of
		one row and the leftmost pixel of the next row. (IE this is zero if rows
		are consecutive).
		*/
		property size_t RowSkip
		{
			size_t get() { return rowPitch - box.Width; }
		}

		/** Get the number of elements between one past the right bottom pixel of
		one slice and the left top pixel of the next slice. (IE this is zero if slices
		are consecutive).
		*/
		property size_t SliceSkip
		{
			size_t get() { return slicePitch - (box.Height * rowPitch); }
		}

		/** Return whether this buffer is laid out consecutive in memory (ie the pitches
		are equal to the dimensions)
		*/
		property bool IsConsecutive
		{
			bool get()
			{
				return rowPitch == box.Width && slicePitch == box.Width*box.Height;
			}
		}
		/** Return the size (in bytes) this image would take if it was
		laid out consecutive in memory
		*/
		size_t GetConsecutiveSize();

		/** Return a subvolume of this PixelBox.
		@param def	Defines the bounds of the subregion to return
		@returns	A pixel box describing the region and the data in it
		@remarks	This function does not copy any data, it just returns
		a PixelBox object with a data pointer pointing somewhere inside
		the data of object.
		@throws	Exception(ERR_INVALIDPARAMS) if def is not fully contained
		*/
		PixelBox GetSubVolume(Box def);
	};
}