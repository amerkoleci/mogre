#pragma once

#include "OgreViewport.h"

namespace Mogre
{
	public ref class Viewport
	{
	internal:
		Ogre::Viewport* _native;

	private:
		Viewport(Ogre::Viewport* obj) : _native(obj)
		{
		}

	public protected:
		Viewport(intptr_t ptr) : _native((Ogre::Viewport*)ptr)
		{

		}

	internal:
		property Ogre::Viewport* UnmanagedPointer
		{
			Ogre::Viewport* get();
		}
	};
}