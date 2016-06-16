#include "stdafx.h"
#include "MogrePixelBox.h"

using namespace Mogre;

size_t PixelBox::GetConsecutiveSize()
{
	return PixelUtil::GetMemorySize(box.Width, box.Height, box.Depth, format);
}

/** Return a subvolume of this PixelBox.
@param def	Defines the bounds of the subregion to return
@returns	A pixel box describing the region and the data in it
@remarks	This function does not copy any data, it just returns
a PixelBox object with a data pointer pointing somewhere inside
the data of object.
@throws	Exception(ERR_INVALIDPARAMS) if def is not fully contained
*/
Mogre::PixelBox PixelBox::GetSubVolume(Box def)
{
	if (PixelUtil::IsCompressed(format))
	{
		if (def.left == box.left && def.top == box.top && def.front == box.front &&
			def.right == box.right && def.bottom == box.bottom && def.back == box.back)
		{
			// Entire buffer is being queried
			return *this;
		}
		throw gcnew ArgumentException("Cannot return subvolume of compressed PixelBuffer", "def");
	}

	if (!box.Contains(def))
		throw gcnew ArgumentException("Bounds out of range", "def");

	const size_t elemSize = PixelUtil::GetNumElemBytes(format);
	// Calculate new data origin
	PixelBox rval(def, format, (IntPtr)(void*)(((Ogre::uint8*)(void*)data)
		+ ((def.left - box.left)*elemSize)
		+ ((def.top - box.top)*rowPitch*elemSize)
		+ ((def.front - box.front)*slicePitch*elemSize))
	);

	rval.rowPitch = rowPitch;
	rval.slicePitch = slicePitch;
	rval.format = format;

	return rval;
}