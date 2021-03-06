#include "stdafx.h"
#include "MogreSceneNode.h"
#include "MogreMovableObject.h"
#include "Marshalling.h"

using namespace Mogre;

CPP_DECLARE_STLVECTOR(SceneNode::, ObjectVec, Mogre::MovableObject^, Ogre::MovableObject*);
CPP_DECLARE_ITERATOR(SceneNode::, ObjectIterator, Ogre::SceneNode::ObjectIterator, Mogre::SceneNode::ObjectVec, Mogre::MovableObject^, Ogre::MovableObject*, );

Mogre::SceneNode^ SceneNode::ParentSceneNode::get()
{
	return (Mogre::SceneNode^)Node::GetManaged(static_cast<const Ogre::SceneNode*>(_native)->getParentSceneNode());
}

void SceneNode::AttachObject(Mogre::MovableObject^ obj)
{
	static_cast<Ogre::SceneNode*>(_native)->attachObject(obj);
}

size_t SceneNode::NumAttachedObjects()
{
	return static_cast<const Ogre::SceneNode*>(_native)->numAttachedObjects();
}

Mogre::MovableObject^ SceneNode::GetAttachedObject(size_t index)
{
	return MovableObject::GetManaged(static_cast<Ogre::SceneNode*>(_native)->getAttachedObject(index));
}

Mogre::MovableObject^ SceneNode::GetAttachedObject(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return MovableObject::GetManaged(static_cast<Ogre::SceneNode*>(_native)->getAttachedObject(o_name));
}

void SceneNode::DetachObject(Mogre::MovableObject^ obj)
{
	static_cast<Ogre::SceneNode*>(_native)->detachObject(obj);
}

void SceneNode::DetachAllObjects()
{
	static_cast<Ogre::SceneNode*>(_native)->detachAllObjects();
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate)
{
	auto native = static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode(
		(Ogre::SceneMemoryMgrTypes)sceneType,
		FromVector3(translate),
		FromQuaternion(rotate)
	);

	return gcnew Mogre::SceneNode(native);
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate)
{
	auto native = static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode((Ogre::SceneMemoryMgrTypes)sceneType, FromVector3(translate));
	return gcnew Mogre::SceneNode(native);
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode(SceneMemoryMgrTypes sceneType)
{
	auto native = static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode((Ogre::SceneMemoryMgrTypes)sceneType);
	return gcnew Mogre::SceneNode(native);
}

Mogre::SceneNode^ SceneNode::CreateChildSceneNode()
{
	auto native = static_cast<Ogre::SceneNode*>(_native)->createChildSceneNode();
	return gcnew Mogre::SceneNode(native);
}

void SceneNode::Yaw(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::SceneNode*>(_native)->yaw(Ogre::Radian(angle.ValueRadians), (Ogre::Node::TransformSpace)relativeTo);
}

void SceneNode::Yaw(Mogre::Radian angle)
{
	static_cast<Ogre::SceneNode*>(_native)->yaw(Ogre::Radian(angle.ValueRadians));
}

void SceneNode::SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(x, y, z, (Ogre::Node::TransformSpace)relativeTo, FromVector3(localDirectionVector));
}

void SceneNode::SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(x, y, z, (Ogre::Node::TransformSpace)relativeTo);
}

void SceneNode::SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(x, y, z);
}

void SceneNode::SetDirection(Mogre::Vector3 vec, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(FromVector3(vec), (Ogre::Node::TransformSpace)relativeTo, FromVector3(localDirectionVector));
}

void SceneNode::SetDirection(Mogre::Vector3 vec, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(FromVector3(vec), (Ogre::Node::TransformSpace)relativeTo);
}
void SceneNode::SetDirection(Mogre::Vector3 vec)
{
	static_cast<Ogre::SceneNode*>(_native)->setDirection(FromVector3(vec));
}

void SceneNode::LookAt(Mogre::Vector3 targetPoint, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector)
{
	static_cast<Ogre::SceneNode*>(_native)->lookAt(FromVector3(targetPoint), (Ogre::Node::TransformSpace)relativeTo, FromVector3(localDirectionVector));
}

void SceneNode::LookAt(Mogre::Vector3 targetPoint, Mogre::Node::TransformSpace relativeTo)
{
	static_cast<Ogre::SceneNode*>(_native)->lookAt(FromVector3(targetPoint), (Ogre::Node::TransformSpace)relativeTo);
}

void SceneNode::SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 localDirectionVector, Mogre::Vector3 offset)
{
	static_cast<Ogre::SceneNode*>(_native)->setAutoTracking(enabled, target, FromVector3(localDirectionVector), FromVector3(offset));
}

void SceneNode::SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 localDirectionVector)
{
	static_cast<Ogre::SceneNode*>(_native)->setAutoTracking(enabled, target, FromVector3(localDirectionVector));
}

void SceneNode::SetAutoTracking(bool enabled, Mogre::SceneNode^ target)
{
	static_cast<Ogre::SceneNode*>(_native)->setAutoTracking(enabled, target);
}

void SceneNode::SetAutoTracking(bool enabled)
{
	static_cast<Ogre::SceneNode*>(_native)->setAutoTracking(enabled);
}

void SceneNode::SetVisible(bool visible, bool cascade)
{
	static_cast<Ogre::SceneNode*>(_native)->setVisible(visible, cascade);
}

void SceneNode::SetVisible(bool visible)
{
	static_cast<Ogre::SceneNode*>(_native)->setVisible(visible);
}

void SceneNode::FlipVisibility(bool cascade)
{
	static_cast<Ogre::SceneNode*>(_native)->flipVisibility(cascade);
}

void SceneNode::FlipVisibility()
{
	static_cast<Ogre::SceneNode*>(_native)->flipVisibility();
}

Mogre::SceneNode::ObjectIterator^ SceneNode::GetAttachedObjectIterator()
{
	return static_cast<Ogre::SceneNode*>(_native)->getAttachedObjectIterator();
}

void SceneNode::RemoveAndDestroyChild(SceneNode^ node)
{
	static_cast<Ogre::SceneNode*>(_native)->removeAndDestroyChild(node);
}

void SceneNode::RemoveAndDestroyAllChildren()
{
	static_cast<Ogre::SceneNode*>(_native)->removeAndDestroyAllChildren();
}