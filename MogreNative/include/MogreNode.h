#pragma once
#include "OgreNode.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"

namespace Mogre
{
	public ref class Node : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class ChildNodeMap;

		enum class TransformSpace
		{
			TS_LOCAL = Ogre::Node::TS_LOCAL,
			TS_PARENT = Ogre::Node::TS_PARENT,
			TS_WORLD = Ogre::Node::TS_WORLD,
			Local = Ogre::Node::TS_LOCAL,
			Parent = Ogre::Node::TS_PARENT,
			World = Ogre::Node::TS_WORLD
		};

		//#define STLDECL_MANAGEDKEY String^
		//#define STLDECL_MANAGEDVALUE Mogre::Node^
		//#define STLDECL_NATIVEKEY Ogre::String
		//#define STLDECL_NATIVEVALUE Ogre::Node*
		//		INC_DECLARE_STLHASHMAP(ChildNodeMap, STLDECL_MANAGEDKEY, STLDECL_MANAGEDVALUE, STLDECL_NATIVEKEY, STLDECL_NATIVEVALUE, public:, private:);
		//#undef STLDECL_MANAGEDKEY
		//#undef STLDECL_MANAGEDVALUE
		//#undef STLDECL_NATIVEKEY
		//#undef STLDECL_NATIVEVALUE

				//INC_DECLARE_MAP_ITERATOR(ChildNodeIterator, Ogre::Node::NodeVecIterator, Mogre::Node::ChildNodeMap, Mogre::Node^, Ogre::Node*, String^, Ogre::String);

	internal:
		Ogre::Node* _native;
		bool _createdByCLR;

	public protected:
		Node(Ogre::Node* obj) : _native(obj)
		{
		}

		Node(intptr_t ptr) : _native((Ogre::Node*)ptr)
		{

		}

	public:
		~Node();
	protected:
		!Node();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property bool InheritOrientation
		{
		public:
			bool get();
		public:
			void set(bool inherit);
		}

		property bool InheritScale
		{
		public:
			bool get();
		public:
			void set(bool inherit);
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property bool IsStatic
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property Mogre::Quaternion Orientation
		{
		public:
			Mogre::Quaternion get();
		public:
			void set(Mogre::Quaternion q);
		}

		property Mogre::Node^ Parent
		{
		public:
			Mogre::Node^ get();
		}

		property Ogre::uint16 DepthLevel
		{
		public:
			Ogre::uint16 get();
		}

		property Mogre::Vector3 Position
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 pos);
		}

		property Mogre::Vector3 Scale
		{
		public:
			Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 value);
		}

		property Mogre::Quaternion DerivedOrientation
		{
		public:
			virtual Mogre::Quaternion get();
		public:
			void set(Mogre::Quaternion value);
		}

		property Mogre::Vector3 DerivedPosition
		{
		public:
			virtual Mogre::Vector3 get();
		public:
			void set(Mogre::Vector3 value);
		}

		void SetOrientation(Ogre::Real w, Ogre::Real x, Ogre::Real y, Ogre::Real z);
		void ResetOrientation();
		void SetPosition(Ogre::Real x, Ogre::Real y, Ogre::Real z);
		void SetScale(Ogre::Real x, Ogre::Real y, Ogre::Real z);
		void SetScale(Mogre::Vector3 scale);
		Mogre::Vector3 GetScale();

		void ScaleVector(Mogre::Vector3 scale);
		void ScaleXYZ(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Translate(Mogre::Vector3 d, Mogre::Node::TransformSpace relativeTo);
		void Translate(Mogre::Vector3 d);

		void Translate(Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo);
		void Translate(Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Translate(Mogre::Matrix3^ axes, Mogre::Vector3 move, Mogre::Node::TransformSpace relativeTo);
		void Translate(Mogre::Matrix3^ axes, Mogre::Vector3 move);

		void Translate(Mogre::Matrix3^ axes, Ogre::Real x, Ogre::Real y, Ogre::Real z, Mogre::Node::TransformSpace relativeTo);
		void Translate(Mogre::Matrix3^ axes, Ogre::Real x, Ogre::Real y, Ogre::Real z);

		void Roll(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo);
		void Roll(Mogre::Radian angle);

		void Pitch(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo);
		void Pitch(Mogre::Radian angle);

		void Yaw(Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo);
		void Yaw(Mogre::Radian angle);

		void Rotate(Mogre::Vector3 axis, Mogre::Radian angle, Mogre::Node::TransformSpace relativeTo);
		void Rotate(Mogre::Vector3 axis, Mogre::Radian angle);

		void Rotate(Mogre::Quaternion q, Mogre::Node::TransformSpace relativeTo);
		void Rotate(Mogre::Quaternion q);

		Mogre::Node^ CreateChild(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate, Mogre::Quaternion rotate);
		Mogre::Node^ CreateChild(SceneMemoryMgrTypes sceneType, Mogre::Vector3 translate);
		Mogre::Node^ CreateChild(SceneMemoryMgrTypes sceneType);
		Mogre::Node^ CreateChild();

		void AddChild(Mogre::Node^ child);

		unsigned short NumChildren();

		Mogre::Node^ GetChild(unsigned short index);

		//Mogre::Node::ChildNodeIterator^ GetChildIterator();

		void RemoveChild(Mogre::Node^ child);

		void RemoveAllChildren();

		Mogre::Quaternion _getDerivedOrientation();

		Mogre::Vector3 _getDerivedPosition();

		Mogre::Vector3 _getDerivedScale();

		Mogre::Matrix4^ _getFullTransform();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Node);

	public:
		property Ogre::Node* NativePtr
		{
			Ogre::Node* get() { return _native; }
		}

	internal:
		property Ogre::Node* UnmanagedPointer
		{
			Ogre::Node* get();
		}
	};
}