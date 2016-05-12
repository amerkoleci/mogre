#pragma once

#include "OgreEntity.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class Entity : public MovableObject
	{
	public protected:
		Entity(intptr_t ptr) : MovableObject(ptr)
		{

		}

	internal:
		property Ogre::Entity* UnmanagedPointer
		{
			Ogre::Entity* get();
		}
	};
}