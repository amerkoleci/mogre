#include "stdafx.h"

extern "C"
{
	MOGRE_EXPORTS_API SceneNode* SceneNode_createChildSceneNode(Ogre::SceneNode* _this, SceneMemoryMgrTypes sceneType, const Vector3& translate, const Quaternion& rotate)
	{
		return _this->createChildSceneNode(sceneType, translate, rotate);
	}
}