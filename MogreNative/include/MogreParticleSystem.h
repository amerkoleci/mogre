#pragma once

#include "OgreParticleSystem.h"
#include "MogreMovableObject.h"

namespace Mogre
{
	public ref class ParticleSystem : public MovableObject
	{
	public protected:
		ParticleSystem(intptr_t ptr) : MovableObject(ptr)
		{

		}

	internal:
		property Ogre::ParticleSystem* UnmanagedPointer
		{
			Ogre::ParticleSystem* get();
		}
	};
}