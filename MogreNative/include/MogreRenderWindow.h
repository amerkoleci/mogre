#pragma once

#include "IDisposable.h"
#include "OgreRenderWindow.h"
#include "MogreRenderTarget.h"

namespace Mogre
{
	public ref class RenderWindow : public RenderTarget
	{
	public protected:
		RenderWindow(intptr_t ptr) : RenderTarget(ptr)
		{

		}

	internal:
		property Ogre::RenderWindow* UnmanagedPointer
		{
			Ogre::RenderWindow* get();
		}
	};
}