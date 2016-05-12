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

Mogre::RenderWindow^ Root::AutoCreatedWindow::get()
{
	return ObjectTable::TryGetObject<Mogre::RenderWindow^>((intptr_t)_native->getAutoCreatedWindow());
}

unsigned int Root::DisplayMonitorCount::get()
{
	return _native->getDisplayMonitorCount();
}

Ogre::Real Root::FrameSmoothingPeriod::get()
{
	return _native->getFrameSmoothingPeriod();
}

void Root::FrameSmoothingPeriod::set(Ogre::Real period)
{
	_native->setFrameSmoothingPeriod(period);
}

bool Root::IsInitialised::get()
{
	return _native->isInitialised();
}

bool Root::BlendIndicesGpuRedundant::get()
{
	return _native->isBlendIndicesGpuRedundant();
}

void Root::BlendWeightsGpuRedundant::set(bool value)
{
	_native->setBlendIndicesGpuRedundant(value);
}

bool Root::BlendWeightsGpuRedundant::get()
{
	return _native->isBlendWeightsGpuRedundant();
}

void Root::BlendIndicesGpuRedundant::set(bool value)
{
	_native->setBlendWeightsGpuRedundant(value);
}

Ogre::Real Root::DefaultMinPixelSize::get()
{
	return _native->getDefaultMinPixelSize();
}

void Root::DefaultMinPixelSize::set(Ogre::Real value)
{
	_native->setDefaultMinPixelSize(value);
}

Mogre::RenderSystem^ Root::RenderSystem::get()
{
	return ObjectTable::TryGetObject<Mogre::RenderSystem^>((intptr_t)_native->getRenderSystem());
}

void Root::RenderSystem::set(Mogre::RenderSystem^ system)
{
	_native->setRenderSystem(GetPointerOrNull(system));
}

void Root::SaveConfig()
{
	_native->saveConfig();
}

bool Root::RestoreConfig()
{
	return _native->restoreConfig();
}

bool Root::ShowConfigDialog()
{
	return _native->showConfigDialog();
}

Mogre::SceneManager^ Root::CreateSceneManager(String^ typeName, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)_native->createSceneManager(o_typeName, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod, o_instanceName));
}

Mogre::SceneManager^ Root::CreateSceneManager(String^ typeName, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)_native->createSceneManager(o_typeName, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod));
}

Mogre::SceneManager^ Root::CreateSceneManager(SceneType typeMask, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)_native->createSceneManager((Ogre::SceneTypeMask)typeMask, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod, o_instanceName));
}

Mogre::SceneManager^ Root::CreateSceneManager(SceneType typeMask, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod)
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)_native->createSceneManager((Ogre::SceneTypeMask)typeMask, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod));
}

void Root::DestroySceneManager(Mogre::SceneManager^ sceneManager)
{
	_native->destroySceneManager(GetPointerOrNull(sceneManager));
}

Mogre::SceneManager^ Root::GetSceneManager(String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return ObjectTable::GetOrCreateObject<Mogre::SceneManager^>((intptr_t)_native->getSceneManager(o_instanceName));
}

String^ Root::GetErrorDescription(long errorNumber)
{
	return TO_CLR_STRING(_native->getErrorDescription(errorNumber));
}

void Root::QueueEndRendering()
{
	_native->queueEndRendering();
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

void Root::Shutdown()
{
	_native->shutdown();
}

void Root::AddResourceLocation(String^ name, String^ locType, String^ groupName, bool recursive)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->addResourceLocation(o_name, o_locType, o_groupName, recursive);
}

void Root::AddResourceLocation(String^ name, String^ locType, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->addResourceLocation(o_name, o_locType, o_groupName);
}

void Root::AddResourceLocation(String^ name, String^ locType)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_locType, locType);

	_native->addResourceLocation(o_name, o_locType);
}

void Root::RemoveResourceLocation(String^ name, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	_native->removeResourceLocation(o_name, o_groupName);
}

void Root::RemoveResourceLocation(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);;

	_native->removeResourceLocation(o_name);
}

/*Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height, bool fullScreen, Mogre::Const_NameValuePairList^ miscParams)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->createRenderWindow(o_name, width, height, fullScreen, miscParams);
}*/

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height, bool fullScreen)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->createRenderWindow(o_name, width, height, fullScreen));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->createRenderWindow(o_name, width, height, false));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, IntPtr handle, unsigned int width, unsigned int height, bool fullScreen)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Ogre::NameValuePairList params;
	params["externalWindowHandle"] = Ogre::StringConverter::toString((size_t)handle.ToInt64());

	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->createRenderWindow(o_name, width, height, fullScreen, &params));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, IntPtr handle, unsigned int width, unsigned int height)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Ogre::NameValuePairList params;
	params["externalWindowHandle"] = Ogre::StringConverter::toString((size_t)handle.ToInt64());

	return ObjectTable::GetOrCreateObject<Mogre::RenderWindow^>((intptr_t)_native->createRenderWindow(o_name, width, height, false, &params));
}

void Root::DetachRenderTarget(Mogre::RenderTarget^ target)
{
	_native->detachRenderTarget(GetPointerOrNull(target));
}

void Root::DetachRenderTarget(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->detachRenderTarget(o_name);
}

Mogre::RenderTarget^ Root::GetRenderTarget(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return ObjectTable::GetOrCreateObject<Mogre::RenderTarget^>((intptr_t)_native->getRenderTarget(o_name));
}

void Root::LoadPlugin(String^ pluginName)
{
	DECLARE_NATIVE_STRING(o_pluginName, pluginName);

	_native->loadPlugin(o_pluginName);
}

void Root::UnloadPlugin(String^ pluginName)
{
	DECLARE_NATIVE_STRING(o_pluginName, pluginName);

	_native->unloadPlugin(o_pluginName);
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