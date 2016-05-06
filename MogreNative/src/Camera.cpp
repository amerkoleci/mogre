#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API float Camera_getNearClipDistance(Ogre::Camera* _this)
	{
		return _this->getNearClipDistance();
	}

	MOGRE_EXPORTS_API void Camera_setNearClipDistance(Ogre::Camera* _this, float value)
	{
		_this->setNearClipDistance(value);
	}

	MOGRE_EXPORTS_API float Camera_getFarClipDistance(Ogre::Camera* _this)
	{
		return _this->getFarClipDistance();
	}

	MOGRE_EXPORTS_API void Camera_setFarClipDistance(Ogre::Camera* _this, float value)
	{
		_this->setFarClipDistance(value);
	}
}