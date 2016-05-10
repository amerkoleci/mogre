#include "stdafx.h"
#include "Root.h"
#include "Util.h"
#include "ObjectTable.h"

using namespace Mogre;

static Root::Root()
{
	_instantiated = false;
}

Root::Root(String^ pluginFileName, String^ configFileName, String^ logFileName)
{

	Create(
		Util::ToUnmanagedString(pluginFileName), 
		Util::ToUnmanagedString(configFileName),
		Util::ToUnmanagedString(logFileName)
	);
}

Root::Root(String^ pluginFileName, String^ configFileName)
{

	Create(
		Util::ToUnmanagedString(pluginFileName),
		Util::ToUnmanagedString(configFileName),
		"Ogre.log"
	);
}

Root::Root(String^ pluginFileName)
{

	Create(
		Util::ToUnmanagedString(pluginFileName),
		"ogre.cfg",
		"Ogre.log"
	);
}

Root::Root()
{
	Create("plugins" OGRE_BUILD_SUFFIX ".cfg", "ogre.cfg", "Ogre.log");
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

	OGRE_DELETE _root;
	_root = nullptr;

	_instantiated = false;

	OnDisposed(this, nullptr);
}

bool Root::IsDisposed::get()
{
	return (_root == nullptr);
}

void Root::Create(const char* pluginFileName, const char* configFileName, const char* logFileName)
{
	//if (checkRuntimeFiles)
	//	RuntimeFileChecks::Check();
	Init();

	_root = OGRE_NEW Ogre::Root(
		pluginFileName,
		configFileName,
		logFileName);

	if (_root == nullptr)
		throw gcnew Exception("Failed to create root instance");

	PostInit();
}

void Root::Init()
{
	//if (_instantiated)
	//	throw gcnew MogreAlreadyInstantiatedException("The physics core object has already been instantiated. Check Physics.Instantiated before calling this ctor.");

	_instantiated = true;
}

void Root::PostInit()
{
	ObjectTable::Add((intptr_t)_root, this, nullptr);
}

void Root::Shutdown()
{
	_root->shutdown();
}

void Root::StartRendering()
{
	_root->startRendering();
}

bool Root::RenderOneFrame()
{
	return _root->renderOneFrame();
}

bool Root::RenderOneFrame(float timeSinceLastFrame)
{
	return _root->renderOneFrame(Ogre::Real(timeSinceLastFrame));
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

extern "C"
{
	MOGRE_EXPORTS_API void Mogre_DeleteString(char* str)
	{
		delete[] str;
	}

	MOGRE_EXPORTS_API Ogre::Root* Root_new(const char* pluginFileName, const char* configFileName, const char* logFileName)
	{
		return new Ogre::Root(pluginFileName, configFileName, logFileName);
	}

	MOGRE_EXPORTS_API void Root_delete(Ogre::Root* _this)
	{
		SafeDelete(_this);
	}

	MOGRE_EXPORTS_API void Root_shutdown(Ogre::Root* _this)
	{
		_this->shutdown();
	}

	MOGRE_EXPORTS_API void Root_startRendering(Ogre::Root* _this)
	{
		_this->startRendering();
	}

	MOGRE_EXPORTS_API Ogre::RenderSystem* Root_getRenderSystemByName(Ogre::Root* _this, const char* name)
	{
		return _this->getRenderSystemByName(name);
	}

	MOGRE_EXPORTS_API Ogre::RenderSystem* Root_getRenderSystem(Ogre::Root* _this)
	{
		return _this->getRenderSystem();
	}

	MOGRE_EXPORTS_API void Root_setRenderSystem(Ogre::Root* _this, Ogre::RenderSystem* value)
	{
		_this->setRenderSystem(value);
	}

	MOGRE_EXPORTS_API Ogre::RenderWindow* Root_initialise(Ogre::Root* _this, bool autoCreateWindow, const char* name)
	{
		return _this->initialise(autoCreateWindow, name);
	}

	MOGRE_EXPORTS_API Ogre::RenderWindow* Root_initialise2(Ogre::Root* _this, bool autoCreateWindow, const char* name, const char* caps)
	{
		return _this->initialise(autoCreateWindow, name, caps);
	}

	MOGRE_EXPORTS_API Ogre::RenderWindow* Root_createRenderWindow(Ogre::Root* _this, const char* name, uint32_t width, uint32_t height, bool fullscreen)
	{
		return _this->createRenderWindow(name, width, height, fullscreen);
	}

	MOGRE_EXPORTS_API Ogre::RenderWindow* Root_createRenderWindow2(Ogre::Root* _this, const char* name, uint32_t width, uint32_t height, bool fullscreen, HWND handle)
	{
		Ogre::NameValuePairList params;
		params["externalWindowHandle"] = Ogre::StringConverter::toString((size_t)handle);
		return _this->createRenderWindow(name, width, height, fullscreen, &params);
	}

	MOGRE_EXPORTS_API bool Root_renderOneFrame(Ogre::Root* _this)
	{
		return _this->renderOneFrame();
	}

	MOGRE_EXPORTS_API bool Root_renderOneFrame2(Ogre::Root* _this, float timeSinceLastFrame)
	{
		return _this->renderOneFrame(timeSinceLastFrame);
	}

	MOGRE_EXPORTS_API Ogre::SceneManager* Root_createSceneManager(Ogre::Root* _this, const char* typeName, uint32_t numWorkerThreads, Ogre::InstancingThreadedCullingMethod threadedCullingMethod, const char* instanceName)
	{
		return _this->createSceneManager(typeName, numWorkerThreads, threadedCullingMethod, instanceName);
	}

	MOGRE_EXPORTS_API Ogre::SceneManager* Root_createSceneManager2(Ogre::Root* _this, Ogre::SceneTypeMask typeMask, uint32_t numWorkerThreads, Ogre::InstancingThreadedCullingMethod threadedCullingMethod, const char* instanceName)
	{
		return _this->createSceneManager(typeMask, numWorkerThreads, threadedCullingMethod, instanceName);
	}

	MOGRE_EXPORTS_API Ogre::CompositorManager2* Root_getCompositorManager2(Ogre::Root* _this)
	{
		return _this->getCompositorManager2();
	}
}