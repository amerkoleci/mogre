#include "stdafx.h"
#include "MogreViewport.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Viewport* Viewport::UnmanagedPointer::get()
{
	return _native;
}