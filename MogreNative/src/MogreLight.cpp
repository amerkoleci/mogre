#include "stdafx.h"
#include "MogreLight.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Light* Light::UnmanagedPointer::get()
{
	return static_cast<Ogre::Light*>(_native);
}