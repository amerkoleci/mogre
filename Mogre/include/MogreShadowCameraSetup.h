#pragma once

#include "OgreShadowCameraSetup.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class SceneManager;
	ref class Camera;
	ref class Light;

	public ref class ShadowCameraSetup : public IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	internal:
		Ogre::ShadowCameraSetup* _native;
		bool _createdByCLR;

	public protected:
		ShadowCameraSetup(Ogre::ShadowCameraSetup* obj) : _native(obj)
		{
		}

	public:
		~ShadowCameraSetup();
	protected:
		!ShadowCameraSetup();

	public:
		property bool IsDisposed
		{
			virtual bool get() { return _native == nullptr; }
		}

		void GetShadowCamera(Mogre::SceneManager^ sm, Mogre::Camera^ cam, Mogre::Light^ light, Mogre::Camera^ texCam, size_t iteration);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(ShadowCameraSetup);
	};

	//################################################################
	//DefaultShadowCameraSetup
	//################################################################

	public ref class DefaultShadowCameraSetup : public ShadowCameraSetup
	{
		//Internal Declarations
	public protected:
		DefaultShadowCameraSetup(Ogre::DefaultShadowCameraSetup* obj) : ShadowCameraSetup(obj)
		{
		}


		//Public Declarations
	public:
		DefaultShadowCameraSetup();

		void GetShadowCamera(Mogre::SceneManager^ sm, Mogre::Camera^ cam, Mogre::Light^ light, Mogre::Camera^ texCam, size_t iteration);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(DefaultShadowCameraSetup);
	};

	public ref class ShadowCameraSetupPtr : public ShadowCameraSetup
	{
	public protected:
		Ogre::ShadowCameraSetupPtr* _sharedPtr;

		ShadowCameraSetupPtr(Ogre::ShadowCameraSetupPtr& sharedPtr) : ShadowCameraSetup(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::ShadowCameraSetupPtr(sharedPtr);
		}

		!ShadowCameraSetupPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~ShadowCameraSetupPtr()
		{
			this->!ShadowCameraSetupPtr();
		}

	public:
		static operator ShadowCameraSetupPtr ^ (const Ogre::ShadowCameraSetupPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew ShadowCameraSetupPtr(*(new Ogre::ShadowCameraSetupPtr(ptr)));
		}

		static operator Ogre::ShadowCameraSetupPtr& (ShadowCameraSetupPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::ShadowCameraSetupPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::ShadowCameraSetupPtr* (ShadowCameraSetupPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		ShadowCameraSetupPtr(ShadowCameraSetup^ obj) : ShadowCameraSetup(obj->_native)
		{
			_sharedPtr = new Ogre::ShadowCameraSetupPtr(static_cast<Ogre::ShadowCameraSetup*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			ShadowCameraSetupPtr^ clr = dynamic_cast<ShadowCameraSetupPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(ShadowCameraSetupPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (ShadowCameraSetupPtr^ val1, ShadowCameraSetupPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (ShadowCameraSetupPtr^ val1, ShadowCameraSetupPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property ShadowCameraSetup^ Target
		{
			ShadowCameraSetup^ get()
			{
				return static_cast<Ogre::ShadowCameraSetup*>(_native);
			}
		}
	};
}