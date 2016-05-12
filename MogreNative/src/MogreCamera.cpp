#include "stdafx.h"
#include "MogreCamera.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Camera* Camera::UnmanagedPointer::get()
{
	return static_cast<Ogre::Camera*>(_native);
}