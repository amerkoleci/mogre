#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API ResourceGroupManager* ResourceGroupManager_getSingletonPtr()
	{
		return ResourceGroupManager::getSingletonPtr();
	}

	MOGRE_EXPORTS_API void ResourceGroupManager_initialiseAllResourceGroups(ResourceGroupManager* _this)
	{
		_this->initialiseAllResourceGroups();
	}
}