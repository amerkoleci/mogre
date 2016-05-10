#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::TextureManager* TextureManager_getSingletonPtr()
	{
		return Ogre::TextureManager::getSingletonPtr();
	}

	MOGRE_EXPORTS_API uint32_t TextureManager_getDefaultNumMipmaps(Ogre::TextureManager* _this)
	{
		return _this->getDefaultNumMipmaps();
	}

	MOGRE_EXPORTS_API void TextureManager_setDefaultNumMipmaps(Ogre::TextureManager* _this, uint32_t value)
	{
		_this->setDefaultNumMipmaps(value);
	}
}