#include "stdafx.h"
#include "MogreEntity.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::Entity* Entity::UnmanagedPointer::get()
{
	return static_cast<Ogre::Entity*>(_native);
}