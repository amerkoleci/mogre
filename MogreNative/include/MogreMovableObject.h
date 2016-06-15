#pragma once

#include "OgreMovableObject.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Node;
	ref class SceneNode;

	public ref class MovableObject : IMogreDisposable// : public ShadowCaster, public IAnimableObject
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

		interface class IListener
		{
			virtual Ogre::MovableObject::Listener* _GetNativePtr();
		public:
			virtual void ObjectDestroyed(Mogre::MovableObject^ param1);
			virtual void ObjectAttached(Mogre::MovableObject^ param1);
			virtual void ObjectDetached(Mogre::MovableObject^ param1);
		};

		ref class Listener : public IMogreDisposable, public Mogre::MovableObject::IListener
		{
		public:
			/// <summary>Raised before any disposing is performed.</summary>
			virtual event EventHandler^ OnDisposing;
			/// <summary>Raised once all disposing is performed.</summary>
			virtual event EventHandler^ OnDisposed;

		internal:
			Ogre::MovableObject::Listener* _native;
			bool _createdByCLR;

		public protected:
			Listener(Ogre::MovableObject::Listener* obj) : _native(obj)
			{
			}

			virtual Ogre::MovableObject::Listener* _IListener_GetNativePtr() = IListener::_GetNativePtr;
		public:
			Listener();

			~Listener();
			!Listener();

			virtual void ObjectDestroyed(Mogre::MovableObject^ param1);
			virtual void ObjectAttached(Mogre::MovableObject^ param1);
			virtual void ObjectDetached(Mogre::MovableObject^ param1);

			property bool IsDisposed
			{
				virtual bool get()
				{
					return _native;
				}
			}
		};

	internal:
		Ogre::MovableObject* _native;
		bool _preventDelete;

	private:
		bool _isDisposed;

	public protected:
		MovableObject(IntPtr ptr)
		{
			UnmanagedPointer = (Ogre::MovableObject*)ptr.ToPointer();
		}

		MovableObject(Ogre::MovableObject* native)
		{
			UnmanagedPointer = native;
		}

		~MovableObject();
		!MovableObject();

	internal:
		static MovableObject^ GetManaged(Ogre::MovableObject* native);

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _isDisposed;
			}
		}

		property Mogre::Aabb LocalAabb
		{
		public:
			Mogre::Aabb get();
		public:
			void set(Mogre::Aabb enabled);
		}

		property Mogre::Aabb WorldAabb
		{
		public:
			Mogre::Aabb get();
		}

		property Mogre::Aabb WorldAabbUpdated
		{
		public:
			Mogre::Aabb get();
		}

		property Ogre::Real WorldRadius
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real WorldRadiusUpdated
		{
		public:
			Ogre::Real get();
		}

		property bool CastShadows
		{
		public:
			bool get();
		public:
			void set(bool enabled);
		}

		property Ogre::uint32 DefaultQueryFlags
		{
		public:
			static Ogre::uint32 get();
		public:
			static void set(Ogre::uint32 flags);
		}

		property Ogre::uint32 DefaultVisibilityFlags
		{
		public:
			static Ogre::uint32 get();
		public:
			static void set(Ogre::uint32 flags);
		}

		property bool IsAttached
		{
		public:
			bool get();
		}

		property bool IsStatic
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property String^ MovableType
		{
		public:
			String^ get();
		}

		property Ogre::IdType Id
		{
		public:
			Ogre::IdType get();
		}

		property String^ Name
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property Mogre::Node^ ParentNode
		{
		public:
			Mogre::Node^ get();
		}

		property Mogre::SceneNode^ ParentSceneNode
		{
		public:
			Mogre::SceneNode^ get();
		}

		property Ogre::uint32 QueryFlags
		{
		public:
			Ogre::uint32 get();
		public:
			void set(Ogre::uint32 flags);
		}

		property Ogre::Real RenderingDistance
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real dist);
		}

		property Ogre::Real RenderingMinPixelSize
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real dist);
		}

		property Ogre::uint8 RenderQueueGroup
		{
		public:
			Ogre::uint8 get();
		public:
			void set(Ogre::uint8 queueID);
		}

		property Ogre::uint32 VisibilityFlags
		{
		public:
			Ogre::uint32 get();
		public:
			void set(Ogre::uint32 flags);
		}

		property bool Visible
		{
		public:
			bool get();
		public:
			void set(bool visible);
		}


	protected:
		Object^ _userObject;
	public:
		property Object^ UserObject
		{
			Object^ get() { return _userObject; }
			void set(Object^ obj) { _userObject = obj; }
		}

		void AddQueryFlags(Ogre::uint32 flags);
		void RemoveQueryFlags(unsigned long flags);

		void AddVisibilityFlags(Ogre::uint32 flags);
		void RemoveVisibilityFlags(Ogre::uint32 flags);

		void SetListener(Mogre::MovableObject::IListener^ listener);
		//Mogre::MovableObject::IListener^ GetListener();

		void DetachFromParent();

		bool IsVisible();

		DEFINE_MANAGED_NATIVE_CONVERSIONS_GET_MANAGED(MovableObject);

	public:
		property Ogre::MovableObject* NativePtr
		{
			Ogre::MovableObject* get() { return UnmanagedPointer; }
		}

	internal:
		property Ogre::MovableObject* UnmanagedPointer
		{
			virtual Ogre::MovableObject* get();
			void set(Ogre::MovableObject* value);
		}
	};
}