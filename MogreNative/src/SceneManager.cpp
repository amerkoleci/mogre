#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Camera* SceneManager_createCamera(Ogre::SceneManager* _this, const char* name, bool notShadowCaster, bool forCubemapping)
	{
		return _this->createCamera(name, notShadowCaster, forCubemapping);
	}
}