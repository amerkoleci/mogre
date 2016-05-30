#pragma once

#include "OgreMeshManager.h"
#include "OgreMesh.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"
#include "MogreHardwareBuffer.h"
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

		Mogre::MeshPtr^ Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed);
		Mogre::MeshPtr^ Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed);
		Mogre::MeshPtr^ Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage);
		Mogre::MeshPtr^ Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage);
		Mogre::MeshPtr^ Load(String^ filename, String^ groupName);

		//Mogre::MeshPtr^ CreateManual(String^ name, String^ groupName, Mogre::IManualResourceLoader^ loader);
		Mogre::MeshPtr^ CreateManual(String^ name, String^ groupName);

		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments);
		Mogre::MeshPtr^ CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height);

		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer, int ySegmentsToKeep);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments);
		Mogre::MeshPtr^ CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature);

		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow);
		Mogre::MeshPtr^ CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height);

		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage, bool vbUseShadow, bool ibUseShadow);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage, bool vbUseShadow);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel);
		//Mogre::PatchMeshPtr^ CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height);

		virtual void LoadResource(Mogre::Resource^ res);
	};
}