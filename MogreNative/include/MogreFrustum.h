#pragma once

#include "IDisposable.h"
#include "OgreFrustum.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class Frustum : public MovableObject
	{
	public protected:
		Frustum(intptr_t ptr) : MovableObject(ptr)
		{

		}

	internal:
		property Ogre::Frustum* UnmanagedPointer
		{
			Ogre::Frustum* get();
		}
	};
}