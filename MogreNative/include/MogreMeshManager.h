#pragma once

#include "OgreMeshManager.h"
#include "OgreMesh.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class TexturePtr;
	ref class DataStreamPtr;

	public ref class Mesh : public Resource
	{
	public protected:
		Mesh(Ogre::Mesh* obj) : Resource(obj)
		{
		}

		Mesh(intptr_t ptr) : Resource((Ogre::Mesh*)ptr)
		{
		}

		Mesh(Ogre::Resource* obj) : Resource(obj)
		{
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(Mesh);

	internal:
		property Ogre::Mesh* UnmanagedPointer
		{
			Ogre::Mesh* get()
			{
				return static_cast<Ogre::Mesh*>(_native);
			}
		}
	};

	public ref class MeshPtr : public Mesh
	{
	public protected:
		Ogre::MeshPtr* _sharedPtr;

		MeshPtr(Ogre::MeshPtr& sharedPtr) : Mesh(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::MeshPtr(sharedPtr);
		}

		!MeshPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~MeshPtr()
		{
			this->!MeshPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(MeshPtr);

		static MeshPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			return (MeshPtr^)ptr;
		}

		static operator MeshPtr ^ (ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::Mesh*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type Mesh.");
			return gcnew MeshPtr(Ogre::MeshPtr(ptr->_sharedPtr->dynamicCast<Ogre::Mesh>()));
		}

		MeshPtr(Mesh^ obj) : Mesh(obj->_native)
		{
			_sharedPtr = new Ogre::MeshPtr(static_cast<Ogre::Mesh*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			MeshPtr^ clr = dynamic_cast<MeshPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(MeshPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (MeshPtr^ val1, MeshPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (MeshPtr^ val1, MeshPtr^ val2)
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

		property Mesh^ Target
		{
			Mesh^ get()
			{
				return static_cast<Ogre::Mesh*>(_native);
			}
		}
	};

	public ref class MeshLodUsage
	{
	public protected:
		MeshLodUsage(Ogre::MeshLodUsage* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~MeshLodUsage()
		{
			this->!MeshLodUsage();
		}
		!MeshLodUsage()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::MeshLodUsage* _native;
		bool _createdByCLR;

	public:
		MeshLodUsage();

		property String^ manualName
		{
		public:
			String^ get();
		public:
			void set(String^ value);
		}

		property Mogre::MeshPtr^ manualMesh
		{
		public:
			Mogre::MeshPtr^ get();
		public:
			void set(Mogre::MeshPtr^ value);
		}

		/*property Mogre::EdgeData^ edgeData
		{
		public:
			Mogre::EdgeData^ get();
		public:
			void set(Mogre::EdgeData^ value);
		}*/

		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_PLAINWRAPPER(MeshLodUsage);
	};

	public ref class MeshManager : public ResourceManager
	{
	private protected:
		static MeshManager^ _singleton;

	public protected:
		MeshManager(Ogre::MeshManager* obj) : ResourceManager(obj)
		{
		}

	public:

		static property MeshManager^ Singleton
		{
			MeshManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::MeshManager* ptr = Ogre::MeshManager::getSingletonPtr();
					if (ptr) _singleton = gcnew MeshManager(ptr);
				}
				return _singleton;
			}
		}

		property Ogre::Real BoundsPaddingFactor
		{
		public:
			Ogre::Real get();
		public:
			void set(Ogre::Real paddingFactor);
		}

		property bool PrepareAllMeshesForShadowVolumes
		{
		public:
			bool get();
		public:
			void set(bool enable);
		}
	};
}