#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API SceneNode* SceneManager_getRootSceneNode(Ogre::SceneManager* _this)
	{
		return _this->getRootSceneNode();
	}

	MOGRE_EXPORTS_API Entity* SceneManager_createEntity(Ogre::SceneManager* _this, const char* meshName, const char* groupName, SceneMemoryMgrTypes sceneType)
	{
		return _this->createEntity(meshName, groupName, sceneType);
	}

	MOGRE_EXPORTS_API Camera* SceneManager_createCamera(Ogre::SceneManager* _this, const char* name, bool notShadowCaster, bool forCubemapping)
	{
		return _this->createCamera(name, notShadowCaster, forCubemapping);
	}
}