#include "stdafx.h"
#include "MogreWindowEventUtilities.h"
#include "MogreRenderWindow.h"

using namespace Mogre;

Ogre::WindowEventListener* WindowEventListener::_IWindowEventListener_GetNativePtr()
{
	return static_cast<Ogre::WindowEventListener*>(static_cast<WindowEventListener_Proxy*>(_native));
}

WindowEventListener::WindowEventListener() 
{
	_createdByCLR = true;
	Type^ thisType = this->GetType();
	WindowEventListener_Proxy* proxy = new WindowEventListener_Proxy(this);
	//proxy->_overriden = Implementation::SubclassingManager::Instance->GetOverridenMethodsArrayPointer(thisType, WindowEventListener::typeid, 4);
	_native = proxy;
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

WindowEventListener::~WindowEventListener()
{
	this->!WindowEventListener();
}

WindowEventListener::!WindowEventListener()
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

void WindowEventListener::WindowMoved(Mogre::RenderWindow^ rw)
{
	static_cast<WindowEventListener_Proxy*>(_native)->windowMoved(rw);
}

void WindowEventListener::WindowResized(Mogre::RenderWindow^ rw)
{
	static_cast<WindowEventListener_Proxy*>(_native)->windowResized(rw);
}

void WindowEventListener::WindowClosed(Mogre::RenderWindow^ rw)
{
	static_cast<WindowEventListener_Proxy*>(_native)->windowClosed(rw);
}

void WindowEventListener::WindowFocusChange(Mogre::RenderWindow^ rw)
{
	static_cast<WindowEventListener_Proxy*>(_native)->windowFocusChange(rw);
}

//################################################################
//WindowEventUtilities
//################################################################

#define STLDECL_MANAGEDTYPE Mogre::RenderWindow^
#define STLDECL_NATIVETYPE Ogre::RenderWindow*
CPP_DECLARE_STLVECTOR(WindowEventUtilities::, Windows, STLDECL_MANAGEDTYPE, STLDECL_NATIVETYPE);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE

WindowEventUtilities::WindowEventUtilities()
{
	_createdByCLR = true;
	_native = new Ogre::WindowEventUtilities();
}

void WindowEventUtilities::MessagePump()
{
	Ogre::WindowEventUtilities::messagePump();
}

void WindowEventUtilities::AddWindowEventListener(Mogre::RenderWindow^ window, Mogre::IWindowEventListener^ listener)
{
	Ogre::WindowEventUtilities::addWindowEventListener(window, listener);
}

void WindowEventUtilities::RemoveWindowEventListener(Mogre::RenderWindow^ window, Mogre::IWindowEventListener^ listener)
{
	Ogre::WindowEventUtilities::removeWindowEventListener(window, listener);
}

void WindowEventUtilities::_addRenderWindow(Mogre::RenderWindow^ window)
{
	Ogre::WindowEventUtilities::_addRenderWindow(window);
}

void WindowEventUtilities::_removeRenderWindow(Mogre::RenderWindow^ window)
{
	Ogre::WindowEventUtilities::_removeRenderWindow(window);
}

void WindowEventListener_Proxy::windowMoved(Ogre::RenderWindow* rw)
{
	//if (_overriden[0])
	{
		_managed->WindowMoved(rw);
	}
	//else
	//	WindowEventListener::windowMoved(rw);
}

void WindowEventListener_Proxy::windowResized(Ogre::RenderWindow* rw)
{
	//if (_overriden[1])
	{
		_managed->WindowResized(rw);
	}
	//else
	//	WindowEventListener::windowResized(rw);
}

void WindowEventListener_Proxy::windowClosed(Ogre::RenderWindow* rw)
{
	//if (_overriden[2])
	{
		_managed->WindowClosed(rw);
	}
	//else
	//	WindowEventListener::windowClosed(rw);
}

void WindowEventListener_Proxy::windowFocusChange(Ogre::RenderWindow* rw)
{
	//if (_overriden[3])
	{
		_managed->WindowFocusChange(rw);
	}
	//else
	//	WindowEventListener::windowFocusChange(rw);
}