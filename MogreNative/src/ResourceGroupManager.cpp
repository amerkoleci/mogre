#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::ResourceGroupManager* ResourceGroupManager_getSingletonPtr()
	{
		return Ogre::ResourceGroupManager::getSingletonPtr();
	}

	MOGRE_EXPORTS_API void ResourceGroupManager_initialiseAllResourceGroups(Ogre::ResourceGroupManager* _this)
	{
		_this->initialiseAllResourceGroups();
	}

	MOGRE_EXPORTS_API char* ResourceGroupManager_DEFAULT_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME);
	}

	MOGRE_EXPORTS_API char* ResourceGroupManager_INTERNAL_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(Ogre::ResourceGroupManager::INTERNAL_RESOURCE_GROUP_NAME);
	}

	MOGRE_EXPORTS_API char* ResourceGroupManager_AUTODETECT_RESOURCE_GROUP_NAME()
	{
		return CreateOutString(Ogre::ResourceGroupManager::AUTODETECT_RESOURCE_GROUP_NAME);
	}

	MOGRE_EXPORTS_API void ResourceGroupManager_addResourceLocation(Ogre::ResourceGroupManager* _this,
		const char* name, 
		const char* locType,
		const char* resGroup, 
		bool recursive, 
		bool readOnly)
	{
		_this->addResourceLocation(name, locType, resGroup, recursive, readOnly);
	}
}