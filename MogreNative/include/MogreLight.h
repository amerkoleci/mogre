#pragma once

#include "OgreLight.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class Light : public MovableObject
	{
	public:
		enum class LightTypes
		{
			LT_POINT = Ogre::Light::LT_POINT,
			LT_DIRECTIONAL = Ogre::Light::LT_DIRECTIONAL,
			LT_SPOTLIGHT = Ogre::Light::LT_SPOTLIGHT,
			Point = Ogre::Light::LT_POINT,
			Directional = Ogre::Light::LT_DIRECTIONAL,
			Spotlight = Ogre::Light::LT_SPOTLIGHT
		};

	public protected:
		Light(intptr_t ptr) : MovableObject(ptr)
		{

		}

	internal:
		property Ogre::Light* UnmanagedPointer
		{
			Ogre::Light* get();
		}
	};
}