#pragma once

#include "Overlay/OgreOverlaySystem.h"
#include "Overlay/OgreOverlayManager.h"
#include "MogreCommon.h"
#include "MogreStringVector.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class SceneManager;

	public ref class OverlaySystem : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	private:
		SceneManager^ _sceneManager;

	public protected:
		Ogre::OverlaySystem* _native;
		bool _createdByCLR;

		OverlaySystem(intptr_t ptr) : _native((Ogre::OverlaySystem*)ptr)
		{

		}

		OverlaySystem(Ogre::OverlaySystem* obj) : _native(obj)
		{

		}

	public:
		OverlaySystem(SceneManager^ sceneManager);

		~OverlaySystem();
	protected:
		!OverlaySystem();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

	internal:
		property Ogre::OverlaySystem* UnmanagedPointer
		{
			Ogre::OverlaySystem* get() { return _native; }
		}
	};

	public ref class OverlayManager : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		static OverlayManager^ _singleton;
		Ogre::OverlayManager* _native;
		bool _createdByCLR;

		OverlayManager(intptr_t ptr) : _native((Ogre::OverlayManager*)ptr)
		{

		}

		OverlayManager(Ogre::OverlayManager* obj) : _native(obj)
		{

		}

	public:
		~OverlayManager();
	protected:
		!OverlayManager();

	public:
		OverlayManager();

		static property OverlayManager^ Singleton
		{
			OverlayManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::OverlayManager* ptr = Ogre::OverlayManager::getSingletonPtr();
					if (ptr) _singleton = gcnew OverlayManager(ptr);
				}
				return _singleton;
			}
		}

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property Ogre::Real LoadingOrder
		{
		public:
			Ogre::Real get();
		}

		property Ogre::Real ViewportAspectRatio
		{
		public:
			Ogre::Real get();
		}

		property int ViewportHeight
		{
		public:
			int get();
		}

		property int ViewportWidth
		{
		public:
			int get();
		}

	internal:
		property Ogre::OverlayManager* UnmanagedPointer
		{
			Ogre::OverlayManager* get() { return _native; }
		}
	};
}