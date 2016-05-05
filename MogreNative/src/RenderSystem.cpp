#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API void RenderSystem_setConfigOption(Ogre::RenderSystem* _this, const char* name, const char* value)
	{
		_this->setConfigOption(name, value);
	}
}