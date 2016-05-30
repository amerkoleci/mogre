#pragma once

#include "OgreManualObject.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class ManualObject : public MovableObject
	{
	public protected:
		ManualObject(intptr_t ptr) : MovableObject(ptr)
		{

		}

	internal:
		property Ogre::ManualObject* UnmanagedPointer
		{
			Ogre::ManualObject* get();
		}
	};
}