#include "stdafx.h"
#include "MogreRenderQueue.h"

using namespace Mogre;

RenderQueue::~RenderQueue()
{
	this->!RenderQueue();
}

RenderQueue::!RenderQueue()
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