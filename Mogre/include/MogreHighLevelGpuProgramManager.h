#pragma once

#include "OgreHighLevelGpuProgram.h"
#include "OgreHighLevelGpuProgramManager.h"
#include "MogreGpuProgramManager.h"
#include "MogreResourceManager.h"

#ifdef LoadImage
#	undef LoadImage
#endif

namespace Mogre
{
	public ref class HighLevelGpuProgram : public GpuProgram
	{
	public protected:
		HighLevelGpuProgram(Ogre::HighLevelGpuProgram* obj) : GpuProgram(obj)
		{
		}

	public:
		property Mogre::GpuNamedConstants_NativePtr ConstantDefinitions
		{
		public:
			Mogre::GpuNamedConstants_NativePtr get();
		}

		Mogre::GpuProgramParametersSharedPtr^ CreateParameters();

		Mogre::GpuProgram^ _getBindingDelegate();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(HighLevelGpuProgram);
	};

	public ref class HighLevelGpuProgramPtr : public HighLevelGpuProgram
	{
	public protected:
		Ogre::HighLevelGpuProgramPtr* _sharedPtr;

		HighLevelGpuProgramPtr(Ogre::HighLevelGpuProgramPtr& sharedPtr) : HighLevelGpuProgram(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::HighLevelGpuProgramPtr(sharedPtr);
		}

		!HighLevelGpuProgramPtr()
		{
			if (_sharedPtr != 0)
			{
				if (_sharedPtr->useCount() > 1)
				{
					delete _sharedPtr;
				}
				_sharedPtr = 0;
			}
		}

		~HighLevelGpuProgramPtr()
		{
			this->!HighLevelGpuProgramPtr();
		}

	public:
		static operator HighLevelGpuProgramPtr ^ (const Ogre::HighLevelGpuProgramPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			Ogre::HighLevelGpuProgramPtr wrapperPtr = Ogre::HighLevelGpuProgramPtr(ptr);
			wrapperPtr.setUseCount(wrapperPtr.useCount() + 1);
			return gcnew HighLevelGpuProgramPtr(wrapperPtr);
		}

		static operator Ogre::HighLevelGpuProgramPtr& (HighLevelGpuProgramPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::HighLevelGpuProgramPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::HighLevelGpuProgramPtr* (HighLevelGpuProgramPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		static HighLevelGpuProgramPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::HighLevelGpuProgram*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type HighLevelGpuProgram.");
			Ogre::HighLevelGpuProgramPtr highLevelGpuProgramPtr = ptr->_sharedPtr->staticCast<Ogre::HighLevelGpuProgram>();
			return gcnew HighLevelGpuProgramPtr(highLevelGpuProgramPtr);
		}

		static operator HighLevelGpuProgramPtr ^ (ResourcePtr^ ptr)
		{
			HighLevelGpuProgramPtr^ res = FromResourcePtr(ptr);
			// invalidate previous pointer and return converted pointer
			delete ptr;
			return res;
		}

		HighLevelGpuProgramPtr(HighLevelGpuProgram^ obj) : HighLevelGpuProgram(static_cast<Ogre::HighLevelGpuProgram*>(obj->_native))
		{
			_sharedPtr = new Ogre::HighLevelGpuProgramPtr(static_cast<Ogre::HighLevelGpuProgram*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			HighLevelGpuProgramPtr^ clr = dynamic_cast<HighLevelGpuProgramPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(HighLevelGpuProgramPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (HighLevelGpuProgramPtr^ val1, HighLevelGpuProgramPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (HighLevelGpuProgramPtr^ val1, HighLevelGpuProgramPtr^ val2)
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

		property HighLevelGpuProgram^ Target
		{
			HighLevelGpuProgram^ get()
			{
				return static_cast<Ogre::HighLevelGpuProgram*>(_native);
			}
		}
	};

	public ref class HighLevelGpuProgramManager : public ResourceManager
	{
	private protected:
		static HighLevelGpuProgramManager^ _singleton;

	public protected:
		HighLevelGpuProgramManager(Ogre::HighLevelGpuProgramManager* obj) : ResourceManager(obj)
		{
		}

	public:
		HighLevelGpuProgramManager();

		static property HighLevelGpuProgramManager^ Singleton
		{
			HighLevelGpuProgramManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::HighLevelGpuProgramManager* ptr = Ogre::HighLevelGpuProgramManager::getSingletonPtr();
					if (ptr) _singleton = gcnew HighLevelGpuProgramManager(ptr);
				}
				return _singleton;
			}
		}

		Mogre::HighLevelGpuProgramPtr^ CreateProgram(String^ name, String^ groupName, String^ language, Mogre::GpuProgramType gptype);
	};
}