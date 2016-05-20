#include "stdafx.h"
#include "MogreMeshManager.h"

using namespace Mogre;

Ogre::Real MeshManager::BoundsPaddingFactor::get()
{
	return static_cast<Ogre::MeshManager*>(_native)->getBoundsPaddingFactor();
}

void MeshManager::BoundsPaddingFactor::set(Ogre::Real paddingFactor)
{
	static_cast<Ogre::MeshManager*>(_native)->setBoundsPaddingFactor(paddingFactor);
}

bool MeshManager::PrepareAllMeshesForShadowVolumes::get()
{
	return static_cast<Ogre::MeshManager*>(_native)->getPrepareAllMeshesForShadowVolumes();
}

void MeshManager::PrepareAllMeshesForShadowVolumes::set(bool enable)
{
	static_cast<Ogre::MeshManager*>(_native)->setPrepareAllMeshesForShadowVolumes(enable);
}