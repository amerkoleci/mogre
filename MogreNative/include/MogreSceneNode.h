#pragma once

#include "OgreSceneNode.h"
#include "MogreNode.h"
#include "MogreCommon.h"

namespace Mogre
{
	public ref class SceneNode : public Node
	{
	public protected:
		SceneNode(intptr_t ptr) : Node(ptr)
		{

		}

	public:
		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate);
		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate);
		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType);
		Mogre::SceneNode^ CreateChildSceneNode();

	internal:
		property Ogre::SceneNode* UnmanagedPointer
		{
			Ogre::SceneNode* get();
		}
	};
}