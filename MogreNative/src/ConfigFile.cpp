#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::ConfigFile* ConfigFile_new()
	{
		return new Ogre::ConfigFile();
	}

	MOGRE_EXPORTS_API void ConfigFile_delete(Ogre::ConfigFile* _this)
	{
		SafeDelete(_this);
	}
}