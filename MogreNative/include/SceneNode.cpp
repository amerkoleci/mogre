#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API Ogre::SceneNode* SceneNode_createChildSceneNode(Ogre::SceneNode* _this, Ogre::SceneMemoryMgrTypes sceneType, const Ogre::Vector3& translate, const Ogre::Quaternion& rotate)
	{
		return _this->createChildSceneNode(sceneType, translate, rotate);
	}
}