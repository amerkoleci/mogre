#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API TextureManager* TextureManager_getSingletonPtr()
	{
		return TextureManager::getSingletonPtr();
	}

	MOGRE_EXPORTS_API uint32_t TextureManager_getDefaultNumMipmaps(TextureManager* _this)
	{
		return _this->getDefaultNumMipmaps();
	}

	MOGRE_EXPORTS_API void TextureManager_setDefaultNumMipmaps(TextureManager* _this, uint32_t value)
	{
		_this->setDefaultNumMipmaps(value);
	}
}