#include "stdafx.h"
#include "MogreViewport.h"
#include "MogreRenderTarget.h"
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

int Viewport::ActualHeight::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getActualHeight();
}

int Viewport::ActualLeft::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getActualLeft();
}

int Viewport::ActualTop::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getActualTop();
}

int Viewport::ActualWidth::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getActualWidth();
}

Ogre::Real Viewport::Height::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getHeight();
}

Ogre::Real Viewport::Left::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getLeft();
}

String^ Viewport::MaterialScheme::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Viewport*>(_native)->getMaterialScheme());
}

void Viewport::MaterialScheme::set(String^ schemeName)
{
	DECLARE_NATIVE_STRING(o_schemeName, schemeName);

	static_cast<Ogre::Viewport*>(_native)->setMaterialScheme(o_schemeName);
}

bool Viewport::OverlaysEnabled::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getOverlaysEnabled();
}

void Viewport::OverlaysEnabled::set(bool enabled)
{
	static_cast<Ogre::Viewport*>(_native)->setOverlaysEnabled(enabled);
}

String^ Viewport::RenderQueueInvocationSequenceName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Viewport*>(_native)->getRenderQueueInvocationSequenceName());
}

void Viewport::RenderQueueInvocationSequenceName::set(String^ sequenceName)
{
	DECLARE_NATIVE_STRING(o_sequenceName, sequenceName);

	static_cast<Ogre::Viewport*>(_native)->setRenderQueueInvocationSequenceName(o_sequenceName);
}

bool Viewport::SkiesEnabled::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getSkiesEnabled();
}

void Viewport::SkiesEnabled::set(bool enabled)
{
	static_cast<Ogre::Viewport*>(_native)->setSkiesEnabled(enabled);
}

Mogre::RenderTarget^ Viewport::Target::get()
{
	return _native->getTarget();
}

Ogre::Real Viewport::Top::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getTop();
}

Ogre::uint Viewport::VisibilityMask::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getVisibilityMask();
}

Ogre::Real Viewport::Width::get()
{
	return static_cast<const Ogre::Viewport*>(_native)->getWidth();
}

void Viewport::_updateDimensions()
{
	_native->_updateDimensions();
}

void Viewport::SetDimensions(Ogre::Real left, Ogre::Real top, Ogre::Real width, Ogre::Real height)
{
	_native->setDimensions(left, top, width, height);
}

void Viewport::GetActualDimensions([Out] int% left, [Out] int% top, [Out] int% width, [Out] int% height)
{
	pin_ptr<int> p_left = &left;
	pin_ptr<int> p_top = &top;
	pin_ptr<int> p_width = &width;
	pin_ptr<int> p_height = &height;

	_native->getActualDimensions(*p_left, *p_top, *p_width, *p_height);
}

void Viewport::Clear()
{
	_native->clear();
}

void Viewport::Clear(unsigned int buffers)
{
	_native->clear(buffers);
}

void Viewport::Clear(unsigned int buffers, Mogre::ColourValue colour)
{
	_native->clear(buffers, FromColor4(colour));
}

void Viewport::Clear(unsigned int buffers, Mogre::ColourValue colour, Ogre::Real depth)
{
	_native->clear(buffers, FromColor4(colour), depth);
}

void Viewport::Clear(unsigned int buffers, Mogre::ColourValue colour, Ogre::Real depth, unsigned short stencil)
{
	_native->clear(buffers, FromColor4(colour), depth, stencil);
}