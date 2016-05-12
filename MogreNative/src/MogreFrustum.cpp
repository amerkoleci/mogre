#include "stdafx.h"
#include "MogreFrustum.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Frustum* Frustum::UnmanagedPointer::get()
{
	return static_cast<Ogre::Frustum*>(_native);
}