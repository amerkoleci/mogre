#pragma once

#include "OgreMaterial.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"
#include "Marshalling.h"

namespace Mogre
{
	public ref class Material : public Resource
	{
	public protected:
		Material(Ogre::Material* obj) : Resource(obj)
		{
		}

		Material(intptr_t ptr) : Resource((Ogre::Material*)ptr)
		{
		}

		Material(Ogre::Resource* obj) : Resource(obj)
		{
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Material);

	internal:
		property Ogre::Material* UnmanagedPointer
		{
			Ogre::Material* get()
			{
				return static_cast<Ogre::Material*>(_native);
			}
		}
	};

	public ref class MaterialPtr : public Material
	{
	public protected:
		Ogre::MaterialPtr* _sharedPtr;

		MaterialPtr(Ogre::MaterialPtr& sharedPtr) : Material(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::MaterialPtr(sharedPtr);
		}

		!MaterialPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~MaterialPtr()
		{
			this->!MaterialPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(MaterialPtr);

		static MaterialPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			return (MaterialPtr^)ptr;
		}

		static operator MaterialPtr ^ (ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::Material*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type Material.");
			auto materialPtr = Ogre::MaterialPtr(ptr->_sharedPtr->dynamicCast<Ogre::Material>());
			return gcnew MaterialPtr(materialPtr);
		}

		MaterialPtr(Material^ obj) : Material(obj->_native)
		{
			_sharedPtr = new Ogre::MaterialPtr(static_cast<Ogre::Material*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			MaterialPtr^ clr = dynamic_cast<MaterialPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(MaterialPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (MaterialPtr^ val1, MaterialPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (MaterialPtr^ val1, MaterialPtr^ val2)
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

		property Material^ Target
		{
			Material^ get()
			{
				return static_cast<Ogre::Material*>(_native);
			}
		}
	};

	public ref class MaterialManager : public ResourceManager
	{
	private protected:
		static MaterialManager^ _singleton;

	public protected:
		MaterialManager(Ogre::MaterialManager* obj) : ResourceManager(obj)
		{
		}

	public:

		static property MaterialManager^ Singleton
		{
			MaterialManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::MaterialManager* ptr = Ogre::MaterialManager::getSingletonPtr();
					if (ptr) _singleton = gcnew MaterialManager(ptr);
				}
				return _singleton;
			}
		}
	};
}