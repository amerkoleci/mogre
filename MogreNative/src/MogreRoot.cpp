#include "stdafx.h"
#include "Marshalling.h"
#include "ObjectTable.h"
#include "MogreRoot.h"
#include "MogreRenderSystem.h"
#include "MogreRenderWindow.h"

using namespace Mogre;

Root::Root(String^ pluginFileName, String^ configFileName, String^ logFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	DECLARE_NATIVE_STRING(o_configFileName, configFileName);
	DECLARE_NATIVE_STRING(o_logFileName, logFileName);
	_native = new Ogre::Root(o_pluginFileName, o_configFileName, o_logFileName);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Root::Root(String^ pluginFileName, String^ configFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	DECLARE_NATIVE_STRING(o_configFileName, configFileName);
	_native = new Ogre::Root(o_pluginFileName, o_configFileName);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Root::Root(String^ pluginFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	_native = new Ogre::Root(o_pluginFileName);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Root::Root()
{
	_createdByCLR = true;
	_native = new Ogre::Root();
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Root::~Root()
{
	this->!Root();
}

Root::!Root()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	_native = Ogre::Root::getSingletonPtr();

	// Collect all SharedPtr objects that are waiting for finalization
	System::GC::Collect();
	System::GC::WaitForPendingFinalizers();
	System::GC::Collect();

	if (_frameListener != 0)
	{
		if (_native != 0) static_cast<Ogre::Root*>(_native)->removeFrameListener(_frameListener);
		delete _frameListener; _frameListener = 0;
	}

	if (_createdByCLR && _native) { delete _native; _native = 0; }
	_singleton = nullptr;

	OnDisposed(this, nullptr);
}

bool Root::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::RenderSystem^ Root::RenderSystem::get()
{
	return ObjectTable::TryGetObject<Mogre::RenderSystem^>((intptr_t)_native->getRenderSystem());
}

void Root::RenderSystem::set(Mogre::RenderSystem^ system)
{
	_native->setRenderSystem(GetPointerOrNull(system));
}

void Root::Shutdown()
{
	_native->shutdown();
}

void Root::StartRendering()
{
	_native->startRendering();
}

bool Root::RenderOneFrame()
{
	return _native->renderOneFrame();
}

bool Root::RenderOneFrame(float timeSinceLastFrame)
{
	return _native->renderOneFrame(Ogre::Real(timeSinceLastFrame));
}

Mogre::RenderSystem^ Root::GetRenderSystemByName(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::RenderSystem^>((intptr_t)_native->getRenderSystemByName(o_name));
}

Mogre::RenderWindow^ Root::Initialise(bool autoCreateWindow, String^ windowTitle)
{
	DECLARE_NATIVE_STRING(o_windowTitle, windowTitle);

	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->initialise(autoCreateWindow, o_windowTitle));
}

Mogre::RenderWindow^ Root::Initialise(bool autoCreateWindow)
{
	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->initialise(autoCreateWindow));
}


char* CreateOutString(const string& str)
{
	char* result = new char[str.length() + 1];
	strcpy(result, str.c_str());
	return result;
}

char* CreateOutString(const char* str)
{
	char* result = new char[strlen(str) + 1];
	strcpy(result, str);
	return result;
}