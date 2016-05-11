#include "stdafx.h"
#include "MogreRenderTarget.h"
#include "Marshalling.h"

using namespace Mogre;

RenderTarget::RenderTarget()
{

}

RenderTarget::~RenderTarget()
{
	this->!RenderTarget();
}

RenderTarget::!RenderTarget()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	/*if (_renderTargetListener != 0)
	{
		if (_native != 0) static_cast<Ogre::RenderTarget*>(_native)->removeListener(_renderTargetListener);
		delete _renderTargetListener; _renderTargetListener = 0;
	}*/

	if (_createdByCLR && _native) { delete _native; _native = 0; }

	OnDisposed(this, nullptr);
}

bool RenderTarget::IsDisposed::get()
{
	return (_native == nullptr);
}