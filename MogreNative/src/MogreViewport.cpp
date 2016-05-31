#include "stdafx.h"
#include "MogreViewport.h"
#include "Marshalling.h"

using namespace Mogre;

Viewport::~Viewport()
{
	this->!Viewport();
}

Viewport::!Viewport()
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

Ogre::Viewport* Viewport::UnmanagedPointer::get()
{
	return _native;
}