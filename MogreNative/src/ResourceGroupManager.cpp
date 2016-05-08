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

	MOGRE_EXPORTS_API char* ResourceGroupManager_DEFAULT_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME);
	}

	MOGRE_EXPORTS_API char* ResourceGroupManager_INTERNAL_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME);
	}

	MOGRE_EXPORTS_API char* ResourceGroupManager_AUTODETECT_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);
	}
}