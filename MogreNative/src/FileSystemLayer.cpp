#include "stdafx.h"
#include "OgreFileSystemLayer.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::FileSystemLayer* FileSystemLayer_new()
	{
		return OGRE_NEW_T(Ogre::FileSystemLayer, Ogre::MEMCATEGORY_GENERAL)(OGRE_VERSION_NAME);
	}

	MOGRE_EXPORTS_API void FileSystemLayer_delete(Ogre::FileSystemLayer* _this)
	{
		OGRE_DELETE_T(_this, FileSystemLayer, Ogre::MEMCATEGORY_GENERAL);
	}
}