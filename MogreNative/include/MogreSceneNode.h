#pragma once

#include "OgreSceneNode.h"
#include "MogreNode.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class MovableObject;

	public ref class SceneNode : public Node
	{
	public:
		ref class ObjectVec;

	public:
		INC_DECLARE_STLVECTOR(ObjectVec, Mogre::MovableObject^, Ogre::MovableObject*, public:, private:);
		INC_DECLARE_ITERATOR(ObjectIterator, Ogre::SceneNode::ObjectIterator, Mogre::SceneNode::ObjectVec, Mogre::MovableObject^, Ogre::MovableObject*);

	public protected:
		SceneNode(Ogre::SceneNode* obj) : Node(obj)
		{

		}

		SceneNode(IntPtr ptr) : Node(ptr)
		{

		}

	public:

		property Mogre::SceneNode^ ParentSceneNode
		{
		public:
			Mogre::SceneNode^ get();
		}

		void AttachObject(Mogre::MovableObject^ obj);

		unsigned short NumAttachedObjects();

		Mogre::MovableObject^ GetAttachedObject(unsigned short index);

		Mogre::MovableObject^ GetAttachedObject(String^ name);

		void DetachObject(Mogre::MovableObject^ obj);

		void DetachAllObjects();



		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate);
		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate);
		Mogre::SceneNode^ CreateChildSceneNode(SceneMemoryMgrTypes sceneType);
		Mogre::SceneNode^ CreateChildSceneNode();

		void Yaw(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo);
		void Yaw(Mogre::Radian angle);

		void SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector);
		void SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo);
		void SetDirection(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void SetDirection(Mogre::Vector3 vec, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector);
		void SetDirection(Mogre::Vector3 vec, Mogre::Node::TransformSpace relativeTo);
		void SetDirection(Mogre::Vector3 vec);

		void LookAt(Mogre::Vector3 targetPoint, Mogre::Node::TransformSpace relativeTo, Mogre::Vector3 localDirectionVector);
		void LookAt(Mogre::Vector3 targetPoint, Mogre::Node::TransformSpace relativeTo);

		void SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 localDirectionVector, Mogre::Vector3 offset);
		void SetAutoTracking(bool enabled, Mogre::SceneNode^ target, Mogre::Vector3 localDirectionVector);
		void SetAutoTracking(bool enabled, Mogre::SceneNode^ target);
		void SetAutoTracking(bool enabled);

		void SetVisible(bool visible, bool cascade);
		void SetVisible(bool visible);

		void FlipVisibility(bool cascade);
		void FlipVisibility();

		Mogre::SceneNode::ObjectIterator^ GetAttachedObjectIterator();

		void RemoveAndDestroyChild(SceneNode^ node);
		void RemoveAndDestroyAllChildren();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SceneNode);

	internal:
		property Ogre::SceneNode* UnmanagedPointer
		{
			Ogre::SceneNode* get();
		}
	};
}