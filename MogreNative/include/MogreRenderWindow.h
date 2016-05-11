#pragma once

#include "IDisposable.h"
#include "OgreRenderWindow.h"
#include "MogreRenderTarget.h"

namespace Mogre
{
	public ref class RenderWindow : public RenderTarget
	{
	internal:
		Ogre::RenderWindow* _native;

	public protected:
		RenderWindow(intptr_t ptr) : _native((Ogre::RenderWindow*)ptr)
		{

		}

	internal:
		property Ogre::RenderWindow* UnmanagedPointer
		{
			Ogre::RenderWindow* get();
		}
	};
}