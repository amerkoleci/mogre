#include "stdafx.h"


static char* CreateOutString(const string& str)
{
	char* result = new char[str.length() + 1];
	strcpy(result, str.c_str());
	return result;
}

static char* CreateOutString(const char* str)
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
		NameValuePairList params;
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

	MOGRE_EXPORTS_API Ogre::SceneManager* Root_createSceneManager(Ogre::Root* _this, const char* typeName, uint32_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, const char* instanceName)
	{
		return _this->createSceneManager(typeName, numWorkerThreads, threadedCullingMethod, instanceName);
	}

	MOGRE_EXPORTS_API Ogre::SceneManager* Root_createSceneManager2(Ogre::Root* _this, SceneTypeMask typeMask, uint32_t numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, const char* instanceName)
	{
		return _this->createSceneManager(typeMask, numWorkerThreads, threadedCullingMethod, instanceName);
	}

	MOGRE_EXPORTS_API Ogre::CompositorManager2* Root_getCompositorManager2(Ogre::Root* _this)
	{
		return _this->getCompositorManager2();
	}
}