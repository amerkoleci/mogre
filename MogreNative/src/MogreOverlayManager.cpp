#include "stdafx.h"
#include "MogreOverlayManager.h"

using namespace Mogre;

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