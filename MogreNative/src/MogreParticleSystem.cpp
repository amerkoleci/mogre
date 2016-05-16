#include "stdafx.h"
#include "MogreParticleSystem.h"
#include "Marshalling.h"

using namespace Mogre;

Ogre::ParticleSystem* ParticleSystem::UnmanagedPointer::get()
{
	return static_cast<Ogre::ParticleSystem*>(_native);
}