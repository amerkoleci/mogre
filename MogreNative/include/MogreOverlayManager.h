#pragma once

#include "Overlay/OgreOverlaySystem.h"
#include "Overlay/OgreOverlayManager.h"
#include "Overlay/OgreOverlay.h"
#include "Overlay/OgreOverlayElement.h"
#include "MogreCommon.h"
#include "MogreStringVector.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class SceneManager;

	public enum class GuiVerticalAlignment
	{
		GVA_TOP = Ogre::GVA_TOP,
		GVA_CENTER = Ogre::GVA_CENTER,
		GVA_BOTTOM = Ogre::GVA_BOTTOM
	};

	public enum class GuiHorizontalAlignment
	{
		GHA_LEFT = Ogre::GHA_LEFT,
		GHA_CENTER = Ogre::GHA_CENTER,
		GHA_RIGHT = Ogre::GHA_RIGHT
	};

	public enum class GuiMetricsMode
	{
		GMM_RELATIVE = Ogre::GMM_RELATIVE,
		GMM_PIXELS = Ogre::GMM_PIXELS,
		GMM_RELATIVE_ASPECT_ADJUSTED = Ogre::GMM_RELATIVE_ASPECT_ADJUSTED
	};

	public ref class OverlayElement : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::OverlayElement* _native;
		bool _createdByCLR;

		OverlayElement(intptr_t ptr) : _native((Ogre::OverlayElement*)ptr)
		{

		}

		OverlayElement(Ogre::OverlayElement* obj) : _native(obj)
		{

		}

	public:
		~OverlayElement();
	protected:
		!OverlayElement();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

	internal:
		property Ogre::OverlayElement* UnmanagedPointer
		{
			Ogre::OverlayElement* get() { return _native; }
		}
	};

	public ref class Overlay : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::Overlay* _native;
		bool _createdByCLR;

		Overlay(intptr_t ptr) : _native((Ogre::Overlay*)ptr)
		{

		}

		Overlay(Ogre::Overlay* obj) : _native(obj)
		{

		}

	public:
		~Overlay();
	protected:
		!Overlay();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

	internal:
		property Ogre::Overlay* UnmanagedPointer
		{
			Ogre::Overlay* get() { return _native; }
		}
	};

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