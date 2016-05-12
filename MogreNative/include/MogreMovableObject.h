#pragma once

#include "OgreMovableObject.h"

namespace Mogre
{
	public ref class MovableObject// : public ShadowCaster, public IAnimableObject
	{
	internal:
		Ogre::MovableObject* _native;

	public protected:
		MovableObject(intptr_t ptr) : _native((Ogre::MovableObject*)ptr)
		{

		}

	internal:
		property Ogre::MovableObject* UnmanagedPointer
		{
			Ogre::MovableObject* get();
		}
	};
}