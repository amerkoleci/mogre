#pragma once

#include "Overlay/OgreOverlayManager.h"
#include "MogreCommon.h"
#include "MogreStringVector.h"
#include "Marshalling.h"

namespace Mogre
{
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

	internal:
		property Ogre::OverlayManager* UnmanagedPointer
		{
			Ogre::OverlayManager* get() { return _native; }
		}
	};
}