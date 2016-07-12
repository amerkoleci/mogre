#include "stdafx.h"
#include "MogreRenderQueue.h"

using namespace Mogre;

// RenderQueueGroup
RenderQueueGroup::~RenderQueueGroup()
{
	this->!RenderQueueGroup();
}

RenderQueueGroup::!RenderQueueGroup()
{
	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}
}


// RenderQueue
RenderQueue::~RenderQueue()
{
	this->!RenderQueue();
}

RenderQueue::!RenderQueue()
{
	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}
}

Ogre::uint8 RenderQueue::DefaultQueueGroup::get()
{
	return _native->getDefaultQueueGroup();
}

void RenderQueue::DefaultQueueGroup::set(Ogre::uint8 grp)
{
	_native->setDefaultQueueGroup(grp);
}

Ogre::ushort RenderQueue::DefaultRenderablePriority::get()
{
	return _native->getDefaultRenderablePriority();
}

void RenderQueue::DefaultRenderablePriority::set(Mogre::ushort priority)
{
	_native->setDefaultRenderablePriority(priority);
}

Mogre::RenderQueueGroup^ RenderQueue::GetQueueGroup(Ogre::uint8 qid)
{
	return _native->getQueueGroup(qid);
}