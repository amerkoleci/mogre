#pragma once

#include "OgreWindowEventUtilities.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class RenderWindow;

	public interface class IWindowEventListener
	{
		virtual Ogre::WindowEventListener* _GetNativePtr();


		inline static operator Ogre::WindowEventListener* (IWindowEventListener^ t)
		{
			return (t == CLR_NULL) ? 0 : t->_GetNativePtr();
		}

	public:

		virtual void WindowMoved(Mogre::RenderWindow^ rw);
		virtual void WindowResized(Mogre::RenderWindow^ rw);
		virtual void WindowClosed(Mogre::RenderWindow^ rw);
		virtual void WindowFocusChange(Mogre::RenderWindow^ rw);

	};

	//################################################################
	//IWindowEventListener
	//################################################################

	public ref class WindowEventListener : public IMogreDisposable, public IWindowEventListener
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::WindowEventListener* _native;
		bool _createdByCLR;

	public protected:
		WindowEventListener(Ogre::WindowEventListener* obj) : _native(obj)
		{
		}

		virtual Ogre::WindowEventListener* _IWindowEventListener_GetNativePtr() = IWindowEventListener::_GetNativePtr;

	public:
		~WindowEventListener();
	protected:
		!WindowEventListener();

		//Public Declarations
	public:
		WindowEventListener();

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}


		//[Implementation::MethodIndex(0)]
		virtual void WindowMoved(Mogre::RenderWindow^ rw);

		//[Implementation::MethodIndex(1)]
		virtual void WindowResized(Mogre::RenderWindow^ rw);

		//[Implementation::MethodIndex(2)]
		virtual void WindowClosed(Mogre::RenderWindow^ rw);

		//[Implementation::MethodIndex(3)]
		virtual void WindowFocusChange(Mogre::RenderWindow^ rw);
	};


	//################################################################
	//WindowEventUtilities
	//################################################################

	public ref class WindowEventUtilities
	{
	public:
		ref class WindowEventListeners;
		ref class Windows;

#define STLDECL_MANAGEDTYPE Mogre::RenderWindow^
#define STLDECL_NATIVETYPE Ogre::RenderWindow*
		INC_DECLARE_STLVECTOR(Windows, Mogre::RenderWindow^, Ogre::RenderWindow*, public:, private:);
#undef STLDECL_MANAGEDTYPE
#undef STLDECL_NATIVETYPE

		//Internal Declarations
	public protected:
		WindowEventUtilities(Ogre::WindowEventUtilities* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~WindowEventUtilities()
		{
			this->!WindowEventUtilities();
		}

		!WindowEventUtilities()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::WindowEventUtilities* _native;
		bool _createdByCLR;


		//Public Declarations
	public:
		WindowEventUtilities();


		static void MessagePump();

		static void AddWindowEventListener(Mogre::RenderWindow^ window, Mogre::IWindowEventListener^ listener);

		static void RemoveWindowEventListener(Mogre::RenderWindow^ window, Mogre::IWindowEventListener^ listener);

		static void _addRenderWindow(Mogre::RenderWindow^ window);

		static void _removeRenderWindow(Mogre::RenderWindow^ window);

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(WindowEventUtilities);
	};

	//################################################################
	//WindowEventListener_Proxy
	//################################################################

	class WindowEventListener_Proxy : public Ogre::WindowEventListener
	{
	public:
		friend ref class Mogre::WindowEventListener;

		bool* _overriden;

		gcroot<Mogre::WindowEventListener^> _managed;

		WindowEventListener_Proxy(Mogre::WindowEventListener^ managedObj) :
			_managed(managedObj)
		{
		}

		virtual void windowMoved(Ogre::RenderWindow* rw) override;

		virtual void windowResized(Ogre::RenderWindow* rw) override;

		virtual void windowClosed(Ogre::RenderWindow* rw) override;

		virtual void windowFocusChange(Ogre::RenderWindow* rw) override;
	};
}