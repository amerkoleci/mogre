#include "stdafx.h"
#include "MogreMovableObject.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::MovableObject* MovableObject::UnmanagedPointer::get()
{
	return _native;
}