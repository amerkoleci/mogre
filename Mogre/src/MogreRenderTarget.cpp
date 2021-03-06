#include "stdafx.h"
#include "MogreRenderTarget.h"
#include "MogreViewport.h"

using namespace Mogre;

Mogre::Viewport^ RenderTargetViewportEvent_NativePtr::source::get()
{
	return _native->source;
}

Mogre::RenderTarget^ RenderTargetEvent_NativePtr::source::get()
{
	return _native->source;
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

unsigned int RenderTarget::Width::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getWidth();
}

unsigned int RenderTarget::Height::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getHeight();
}

unsigned int RenderTarget::ColourDepth::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getColourDepth();
}

bool RenderTarget::IsActive::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->isActive();
}

bool RenderTarget::IsPrimary::get()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->isPrimary();
}


void RenderTarget::IsActive::set(bool state)
{
	static_cast<Ogre::RenderTarget*>(_native)->setActive(state);
}

void RenderTarget::GetMetrics([Out] unsigned int% width, [Out] unsigned int% height, [Out] unsigned int% colourDepth)
{
	pin_ptr<unsigned int> p_width = &width;
	pin_ptr<unsigned int> p_height = &height;
	pin_ptr<unsigned int> p_colourDepth = &colourDepth;

	static_cast<Ogre::RenderTarget*>(_native)->getMetrics(*p_width, *p_height, *p_colourDepth);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top, float width, float height)
{
	return gcnew Mogre::Viewport(_native->addViewport(left, top, width, height));
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top, float width)
{
	return gcnew Mogre::Viewport(
		_native->addViewport(left, top, width)
	);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left, float top)
{
	return gcnew Mogre::Viewport(
		_native->addViewport(left, top)
	);
}

Mogre::Viewport^ RenderTarget::AddViewport(float left)
{
	return gcnew Mogre::Viewport(
		_native->addViewport(left)
	);
}

Mogre::Viewport^ RenderTarget::AddViewport()
{
	return gcnew Mogre::Viewport(
		_native->addViewport()
	);
}

Mogre::Viewport^ RenderTarget::GetViewport(unsigned short index)
{
	return _native->getViewport(index);
}

void RenderTarget::RemoveViewport(Mogre::Viewport^ viewport)
{
	_native->removeViewport(viewport);
}

void RenderTarget::RemoveAllViewports()
{
	_native->removeAllViewports();
}


Mogre::RenderTarget::FrameStats^ RenderTarget::GetStatistics()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->getStatistics();
}

void RenderTarget::ResetStatistics()
{
	static_cast<Ogre::RenderTarget*>(_native)->resetStatistics();
}

void RenderTarget::GetCustomAttribute(String^ name, void* pData)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::RenderTarget*>(_native)->getCustomAttribute(o_name, pData);
}

void RenderTarget::RemoveAllListeners()
{
	static_cast<Ogre::RenderTarget*>(_native)->removeAllListeners();
}

void RenderTarget::WriteContentsToFile(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<Ogre::RenderTarget*>(_native)->writeContentsToFile(o_filename);
}

String^ RenderTarget::WriteContentsToTimestampedFile(String^ filenamePrefix, String^ filenameSuffix)
{
	DECLARE_NATIVE_STRING(o_filenamePrefix, filenamePrefix);
	DECLARE_NATIVE_STRING(o_filenameSuffix, filenameSuffix);

	return TO_CLR_STRING(static_cast<Ogre::RenderTarget*>(_native)->writeContentsToTimestampedFile(o_filenamePrefix, o_filenameSuffix));
}

bool RenderTarget::RequiresTextureFlipping()
{
	return static_cast<const Ogre::RenderTarget*>(_native)->requiresTextureFlipping();
}

Ogre::RenderTarget* RenderTarget::UnmanagedPointer::get()
{
	return _native;
}

void RenderTexture::WriteContentsToFile(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<Ogre::RenderTexture*>(_native)->writeContentsToFile(o_filename);
}

void MultiRenderTarget::BindSurface(size_t attachment, Mogre::RenderTexture^ target)
{
	static_cast<Ogre::MultiRenderTarget*>(_native)->bindSurface(attachment, target);
}

void MultiRenderTarget::UnbindSurface(size_t attachment)
{
	static_cast<Ogre::MultiRenderTarget*>(_native)->unbindSurface(attachment);
}

void MultiRenderTarget::WriteContentsToFile(String^ filename)
{
	DECLARE_NATIVE_STRING(o_filename, filename);

	static_cast<Ogre::MultiRenderTarget*>(_native)->writeContentsToFile(o_filename);
}