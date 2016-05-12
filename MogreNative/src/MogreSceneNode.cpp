#include "stdafx.h"
#include "MogreSceneNode.h"
#include "Marshalling.h"
#include "ObjectTable.h"
using namespace Mogre;

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate)
{
	auto native = (intptr_t)static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode(
		(Ogre::SceneMemoryMgrTypes)sceneType, 
		FromVector3(translate),
		FromQuaternion(rotate)
		);

	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>(native);
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate)
{
	auto native = (intptr_t)static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode((Ogre::SceneMemoryMgrTypes)sceneType, FromVector3(translate));
	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>(native);
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType)
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>((intptr_t)static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode((Ogre::SceneMemoryMgrTypes)sceneType));
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode()
{
	return ObjectTable::GetOrCreateObject<Mogre::SceneNode^>((intptr_t)static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode());
}

Ogre::SceneNode* SceneNode::UnmanagedPointer::get()
{
	return static_cast<Ogre::SceneNode*>(_native);
}