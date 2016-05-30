#include "stdafx.h"
#include "MogreMeshManager.h"

using namespace Mogre;

MeshLodUsage::MeshLodUsage()
{
	_createdByCLR = true;
	_native = new Ogre::MeshLodUsage();
}

String^ MeshLodUsage::manualName::get()
{
	return TO_CLR_STRING(static_cast<Ogre::MeshLodUsage*>(_native)->manualName);
}

void MeshLodUsage::manualName::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	static_cast<Ogre::MeshLodUsage*>(_native)->manualName = o_value;
}

Mogre::MeshPtr^ MeshLodUsage::manualMesh::get()
{
	return static_cast<Ogre::MeshLodUsage*>(_native)->manualMesh;
}

void MeshLodUsage::manualMesh::set(Mogre::MeshPtr^ value)
{
	static_cast<Ogre::MeshLodUsage*>(_native)->manualMesh = (Ogre::MeshPtr)value;
}

/*Mogre::EdgeData^ MeshLodUsage::edgeData::get()
{
	return static_cast<Ogre::MeshLodUsage*>(_native)->edgeData;
}

void MeshLodUsage::edgeData::set(Mogre::EdgeData^ value)
{
	static_cast<Ogre::MeshLodUsage*>(_native)->edgeData = value;
}*/

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