#include "stdafx.h"
#include "MogreRenderTarget.h"
#include "MogreViewport.h"

using namespace Mogre;

Mogre::Viewport^ RenderTargetViewportEvent_NativePtr::source::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)_native->source);
}

Mogre::RenderTarget^ RenderTargetEvent_NativePtr::source::get()
{
	return ObjectTable::GetOrCreateObject<Mogre::RenderTarget^>((intptr_t)_native->source);
}

void RenderTargetListener_Director::preRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
{
	if (doCallForPreRenderTargetUpdate)
	{
		_receiver->PreRenderTargetUpdate(evt);
	}
}

void RenderTargetListener_Director::postRenderTargetUpdate(const Ogre::RenderTargetEvent& evt)
{
	if (doCallForPostRenderTargetUpdate)
	{
		_receiver->PostRenderTargetUpdate(evt);
	}
}

void RenderTargetListener_Director::preViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
{
	if (doCallForPreViewportUpdate)
	{
		_receiver->PreViewportUpdate(evt);
	}
}

void RenderTargetListener_Director::postViewportUpdate(const Ogre::RenderTargetViewportEvent& evt)
{
	if (doCallForPostViewportUpdate)
	{
		_receiver->PostViewportUpdate(evt);
	}
}

void RenderTargetListener_Director::viewportAdded(const Ogre::RenderTargetViewportEvent& evt)
{
	if (doCallForViewportAdded)
	{
		_receiver->ViewportAdded(evt);
	}
}

void RenderTargetListener_Director::viewportRemoved(const Ogre::RenderTargetViewportEvent& evt)
{
	if (doCallForViewportRemoved)
	{
		_receiver->ViewportRemoved(evt);
	}
}

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

	if (_renderTargetListener != 0)
	{
		if (_native != 0) _native->removeListener(_renderTargetListener);
		delete _renderTargetListener; _renderTargetListener = 0;
	}

	OnDisposed(this, nullptr);
}

bool RenderTarget::IsDisposed::get()
{
	return (_native == nullptr);
}

Ogre::uchar RenderTarget::Priority::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getPriority();
}

void RenderTarget::Priority::set(Ogre::uchar priority)
{
	static_cast<Ogre::RenderTarget*>(_native)->setPriority(priority);
}

size_t RenderTarget::TriangleCount::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getTriangleCount();
}

size_t RenderTarget::BatchCount::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getBatchCount();
}

unsigned short RenderTarget::NumViewports::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getNumViewports();
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top, float width, float height)
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->addViewport(left, top, width, height)
		);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top, float width)
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->addViewport(left, top, width)
		);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top)
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->addViewport(left, top)
		);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left)
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->addViewport(left)
		);
}

Mogre::Viewport^ RenderTarget::AddViewport()
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->addViewport()
		);
}

Mogre::Viewport^ RenderTarget::GetViewport(unsigned short index)
{
	return ObjectTable::GetOrCreateObject<Mogre::Viewport^>((intptr_t)
		static_cast<Ogre::RenderTarget*>(_native)->getViewport(index)
		);
}

void RenderTarget::RemoveViewport(Mogre::Viewport^ viewport)
{
	static_cast<Ogre::RenderTarget*>(_native)->removeViewport(GetPointerOrNull(viewport));
}

void RenderTarget::RemoveAllViewports()
{
	static_cast<Ogre::RenderTarget*>(_native)->removeAllViewports();
}


Mogre::RenderTarget::FrameStats^ RenderTarget::GetStatistics()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getStatistics();
}

void RenderTarget::ResetStatistics()
{
	static_cast<Ogre::RenderTarget*>(_native)->resetStatistics();
}

Ogre::RenderTarget* RenderTarget::UnmanagedPointer::get()
{
	return _native;
}