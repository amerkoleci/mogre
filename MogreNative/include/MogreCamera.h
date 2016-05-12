#pragma once

#include "IDisposable.h"
#include "OgreCamera.h"
#include "MogreFrustum.h"

namespace Mogre
{
	public ref class Camera : public Frustum
	{
	public protected:
		Camera(intptr_t ptr) : Frustum(ptr)
		{

		}

	internal:
		property Ogre::Camera* UnmanagedPointer
		{
			Ogre::Camera* get();
		}
	};
}