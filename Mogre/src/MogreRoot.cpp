#include "stdafx.h"
#include "MogreRoot.h"
#include "MogreRenderSystem.h"
#include "MogreRenderWindow.h"
#include "MogreCompositorManager2.h"
#include "MogreSceneManager.h"

using namespace Mogre;

Root::Root(String^ pluginFileName, String^ configFileName, String^ logFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	DECLARE_NATIVE_STRING(o_configFileName, configFileName);
	DECLARE_NATIVE_STRING(o_logFileName, logFileName);
	_native = new Ogre::Root(o_pluginFileName, o_configFileName, o_logFileName);
}

Root::Root(String^ pluginFileName, String^ configFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	DECLARE_NATIVE_STRING(o_configFileName, configFileName);
	_native = new Ogre::Root(o_pluginFileName, o_configFileName);
}

Root::Root(String^ pluginFileName)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_pluginFileName, pluginFileName);
	_native = new Ogre::Root(o_pluginFileName);
}

Root::Root()
{
	_createdByCLR = true;
	_native = new Ogre::Root();
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

	Mogre::TextureManager::Singleton->Shutdown();

	// Collect all SharedPtr objects that are waiting for finalization
	System::GC::Collect();
	System::GC::WaitForPendingFinalizers();
	System::GC::Collect();

	if (_frameListener != 0)
	{
		if (_native != 0) static_cast<Ogre::Root*>(_native)->removeFrameListener(_frameListener);
		delete _frameListener; _frameListener = 0;
	}

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	_singleton = nullptr;

	OnDisposed(this, nullptr);
}

bool Root::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::Timer^ Root::Timer::get()
{
	ReturnCachedObjectGcnew(Mogre::Timer, _timer, _native->getTimer());
}

Mogre::FrameStats^ Root::GetFrameStats()
{
	return _native->getFrameStats();
}


Mogre::CompositorManager2^ Root::CompositorManager2::get()
{
	ReturnCachedObjectGcnew(Mogre::CompositorManager2, _compositorManager2, _native->getCompositorManager2());
}

Mogre::RenderWindow^ Root::AutoCreatedWindow::get()
{
	return _native->getAutoCreatedWindow();
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
	ReturnCachedObjectGcnew(Mogre::RenderSystem, _activeRenderSystem, _native->getRenderSystem());
}

void Root::RenderSystem::set(Mogre::RenderSystem^ system)
{
	_native->setRenderSystem(system);
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

Mogre::Const_RenderSystemList^ Root::GetAvailableRenderers()
{
	return _native->getAvailableRenderers();
}

Mogre::SceneManager^ Root::CreateSceneManager(String^ typeName, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return gcnew Mogre::SceneManager(_native->createSceneManager(o_typeName, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod, o_instanceName));
}

Mogre::SceneManager^ Root::CreateSceneManager(String^ typeName, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod)
{
	DECLARE_NATIVE_STRING(o_typeName, typeName);

	return gcnew Mogre::SceneManager(_native->createSceneManager(o_typeName, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod));
}

Mogre::SceneManager^ Root::CreateSceneManager(SceneType typeMask, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return gcnew Mogre::SceneManager(_native->createSceneManager((Ogre::SceneTypeMask)typeMask, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod, o_instanceName));
}

Mogre::SceneManager^ Root::CreateSceneManager(SceneType typeMask, size_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod)
{
	return gcnew Mogre::SceneManager(_native->createSceneManager((Ogre::SceneTypeMask)typeMask, numWorkerThreads, (Ogre::InstancingThreadedCullingMethod)threadedCullingMethod));
}

void Root::DestroySceneManager(Mogre::SceneManager^ sceneManager)
{
	Ogre::SceneManager* nativeSceneManager = sceneManager;
	delete sceneManager;
	sceneManager = nullptr;
	_native->destroySceneManager(nativeSceneManager);
}

Mogre::SceneManager^ Root::GetSceneManager(String^ instanceName)
{
	DECLARE_NATIVE_STRING(o_instanceName, instanceName);

	return _native->getSceneManager(o_instanceName);
}

Mogre::SceneManagerEnumerator::SceneManagerIterator^ Root::GetSceneManagerIterator()
{
	return _native->getSceneManagerIterator();
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

	return _native->getRenderSystemByName(o_name);
}

Mogre::RenderWindow^ Root::Initialise(bool autoCreateWindow, String^ windowTitle)
{
	DECLARE_NATIVE_STRING(o_windowTitle, windowTitle);

	return _native->initialise(autoCreateWindow, o_windowTitle);
}

Mogre::RenderWindow^ Root::Initialise(bool autoCreateWindow)
{
	return _native->initialise(autoCreateWindow);
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

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height, bool fullScreen, Mogre::Const_NameValuePairList^ miscParams)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::RenderWindow(_native->createRenderWindow(o_name, width, height, fullScreen, miscParams));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height, bool fullScreen)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::RenderWindow(_native->createRenderWindow(o_name, width, height, fullScreen));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, unsigned int width, unsigned int height)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return gcnew Mogre::RenderWindow(_native->createRenderWindow(o_name, width, height, false));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, IntPtr handle, unsigned int width, unsigned int height, bool fullScreen)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Ogre::NameValuePairList params;
	params["externalWindowHandle"] = Ogre::StringConverter::toString((size_t)handle.ToInt64());

	return gcnew Mogre::RenderWindow(_native->createRenderWindow(o_name, width, height, fullScreen, &params));
}

Mogre::RenderWindow^ Root::CreateRenderWindow(String^ name, IntPtr handle, unsigned int width, unsigned int height)
{
	DECLARE_NATIVE_STRING(o_name, name);
	Ogre::NameValuePairList params;
	params["externalWindowHandle"] = Ogre::StringConverter::toString((size_t)handle.ToInt64());

	return gcnew Mogre::RenderWindow(_native->createRenderWindow(o_name, width, height, false, &params));
}

void Root::DetachRenderTarget(Mogre::RenderTarget^ target)
{
	_native->detachRenderTarget(GetUnmanagedNullable(target));
}

void Root::DetachRenderTarget(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	_native->detachRenderTarget(o_name);
}

Mogre::RenderTarget^ Root::GetRenderTarget(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return _native->getRenderTarget(o_name);
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

void Root::_updateAllRenderTargets()
{
	static_cast<Ogre::Root*>(_native)->_updateAllRenderTargets();
}

CPP_DECLARE_STLMAP(, UnaryOptionList, String^, bool, Ogre::String, bool);
CPP_DECLARE_STLMAP(, BinaryOptionList, String^, String^, Ogre::String, Ogre::String);
CPP_DECLARE_STLMAP(, NameValuePairList, String^, String^, Ogre::String, Ogre::String);
CPP_DECLARE_STLVECTOR(, RenderSystemList, Mogre::RenderSystem^, Ogre::RenderSystem*);
CPP_DECLARE_STLMAP(, AliasTextureNamePairList, String^, String^, Ogre::String, Ogre::String);