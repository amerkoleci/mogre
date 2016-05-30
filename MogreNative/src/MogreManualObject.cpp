#include "stdafx.h"
#include "MogreManualObject.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::ManualObject* ManualObject::UnmanagedPointer::get()
{
	return static_cast<Ogre::ManualObject*>(_native);
}