#include "stdafx.h"
#include "Marshalling.h"
#include "ObjectTable.h"
#include "MogreSceneManager.h"

using namespace Mogre;

SceneManager::~SceneManager()
{
	this->!SceneManager();
}

SceneManager::!SceneManager()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native) { delete _native; _native = 0; }

	OnDisposed(this, nullptr);
}

bool SceneManager::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name)
{
	return CreateCamera(name, true, false);
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name, bool notShadowCaster)
{
	return CreateCamera(name, notShadowCaster, false);
}

Mogre::Camera^ SceneManager::CreateCamera(String^ name, bool notShadowCaster, bool forCubemapping)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Camera^>((intptr_t)_native->createCamera(o_name, notShadowCaster, forCubemapping));
}

Mogre::Camera^ SceneManager::FindCamera(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::Camera^>((intptr_t)_native->findCamera(o_name));
}

void SceneManager::DestroyCamera(Mogre::Camera^ camera)
{
	_native->destroyCamera(GetPointerOrNull(camera));
}

void SceneManager::DestroyAllCameras()
{
	_native->destroyAllCameras();
}

Ogre::SceneManager* SceneManager::UnmanagedPointer::get()
{
	return _native;
}

