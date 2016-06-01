#include "stdafx.h"
#include "MogreOverlayManager.h"
#include "MogreSceneManager.h"

using namespace Mogre;

OverlaySystem::OverlaySystem(SceneManager^ sceneManager)
	: _sceneManager(sceneManager)
{
	_createdByCLR = true;
	_native = OGRE_NEW Ogre::OverlaySystem();
	GetPointerOrNull(sceneManager)->addRenderQueueListener(_native);
}

OverlaySystem::~OverlaySystem()
{
	this->!OverlaySystem();
}

OverlaySystem::!OverlaySystem()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_native)
	{
		GetPointerOrNull(_sceneManager)->removeRenderQueueListener(_native);
	}

	if (_createdByCLR && _native)
	{
		OGRE_DELETE _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

OverlayManager::OverlayManager()
{
	_createdByCLR = true;
	_native = new Ogre::OverlayManager();
}

OverlayManager::~OverlayManager()
{
	this->!OverlayManager();
}

OverlayManager::!OverlayManager()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

Ogre::Real OverlayManager::LoadingOrder::get()
{
	return _native->getLoadingOrder();
}

Ogre::Real OverlayManager::ViewportAspectRatio::get()
{
	return _native->getViewportAspectRatio();
}

int OverlayManager::ViewportHeight::get()
{
	return _native->getViewportHeight();
}

int OverlayManager::ViewportWidth::get()
{
	return _native->getViewportWidth();
}