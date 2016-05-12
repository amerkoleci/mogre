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

Ogre::SceneManager* SceneManager::UnmanagedPointer::get()
{
	return _native;
}