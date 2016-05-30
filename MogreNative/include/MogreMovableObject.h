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
					return _native == nullptr;
				}
			}
		};

	internal:
		Ogre::MovableObject* _native;
		bool _createdByCLR;

	public protected:
		MovableObject(intptr_t ptr) : _native((Ogre::MovableObject*)ptr)
		{

		}

		MovableObject(Ogre::MovableObject* ptr) : _native(ptr)
		{

		}

		~MovableObject();
		!MovableObject();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property Ogre::Real WorldRadius
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

		property String^ MovableType
		{
		public:
			String^ get();
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

		void AddQueryFlags(Ogre::uint32 flags);
		void RemoveQueryFlags(unsigned long flags);

		void AddVisibilityFlags(Ogre::uint32 flags);
		void RemoveVisibilityFlags(Ogre::uint32 flags);

		void SetListener(Mogre::MovableObject::IListener^ listener);
		//Mogre::MovableObject::IListener^ GetListener();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(MovableObject);

	internal:
		property Ogre::MovableObject* UnmanagedPointer
		{
			Ogre::MovableObject* get();
		}
	};
}