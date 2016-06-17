#pragma once

#include "OgreMeshManager.h"
#include "OgreMesh.h"
#include "MogreResource.h"
#include "MogreResourceManager.h"
#include "MogreHardwareBuffer.h"
#include "MogreAnimation.h"
#include "MogreRenderOperation.h"
#include "MogreCommon.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class TexturePtr;
	ref class DataStreamPtr;
	ref class VertexData;
	ref class IndexData;
	ref class Mesh;
	ref class MeshPtr;

	public ref class SubMesh : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		Ogre::SubMesh* _native;
		bool _createdByCLR;

		SubMesh(Ogre::SubMesh* obj) : _native(obj)
		{
		}

		SubMesh(IntPtr ptr) : _native((Ogre::SubMesh*)ptr.ToPointer())
		{
		}

	public:
		~SubMesh();
	protected:
		!SubMesh();
	public:
		property bool IsDisposed
		{
			virtual bool get();
		}

		property bool useSharedVertices
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property Mogre::RenderOperation::OperationTypes operationType
		{
		public:
			Mogre::RenderOperation::OperationTypes get();
		public:
			void set(Mogre::RenderOperation::OperationTypes value);
		}

		property Mogre::VertexData^ vertexData
		{
		public:
			Mogre::VertexData^ get();
		public:
			void set(Mogre::VertexData^ value);
		}

		property Mogre::IndexData^ indexData
		{
		public:
			Mogre::IndexData^ get();
		public:
			void set(Mogre::IndexData^ value);
		}

		property Mogre::Mesh^ parent
		{
		public:
			Mogre::Mesh^ get();
		public:
			void set(Mogre::Mesh^ value);
		}

		property String^ MaterialName
		{
		public:
			String^ get();
		public:
			void set(String^ matName);
		}

		property Mogre::VertexAnimationType VertexAnimationType
		{
		public:
			Mogre::VertexAnimationType get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(SubMesh);
	};

	public ref class Mesh : public Resource
	{
	public:
		INC_DECLARE_STLVECTOR(IndexMap, unsigned short, unsigned short, public:, private:);

	private protected:
		Mogre::Mesh::IndexMap^ _sharedBlendIndexToBoneIndexMap;

	public protected:
		Mesh(Ogre::Mesh* obj) : Resource(obj)
		{
		}

		Mesh(IntPtr ptr) : Resource((Ogre::Mesh*)ptr.ToPointer())
		{
		}

	public:
		//Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader);
		Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual);
		Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group);

		property Mogre::VertexData^ sharedVertexData
		{
		public:
			Mogre::VertexData^ get();
		public:
			void set(Mogre::VertexData^ value);
		}

		property Mogre::Mesh::IndexMap^ sharedBlendIndexToBoneIndexMap
		{
		public:
			Mogre::Mesh::IndexMap^ get();
		}

		property bool AutoBuildEdgeLists
		{
		public:
			bool get();
		public:
			void set(bool autobuild);
		}

		property Mogre::Real BoundingSphereRadius
		{
		public:
			Mogre::Real get();
		}

		property Mogre::AxisAlignedBox^ Bounds
		{
		public:
			Mogre::AxisAlignedBox^ get();
		}

		property bool HasSkeleton
		{
		public:
			bool get();
		}

		property bool HasVertexAnimation
		{
		public:
			bool get();
		}

		property Mogre::HardwareBuffer::Usage IndexBufferUsage
		{
		public:
			Mogre::HardwareBuffer::Usage get();
		}

		property bool IsEdgeListBuilt
		{
		public:
			bool get();
		}

		property bool IsIndexBufferShadowed
		{
		public:
			bool get();
		}

		property bool IsPreparedForShadowVolumes
		{
		public:
			bool get();
		}

		property bool IsVertexBufferShadowed
		{
		public:
			bool get();
		}

		property unsigned short NumAnimations
		{
		public:
			unsigned short get();
		}

		property Mogre::ushort NumLodLevels
		{
		public:
			Mogre::ushort get();
		}

		property unsigned short NumSubMeshes
		{
		public:
			unsigned short get();
		}

		property size_t PoseCount
		{
		public:
			size_t get();
		}

		property Mogre::VertexAnimationType SharedVertexDataAnimationType
		{
		public:
			Mogre::VertexAnimationType get();
		}

		property String^ SkeletonName
		{
		public:
			String^ get();
		public:
			void set(String^ skelName);
		}

		property Mogre::HardwareBuffer::Usage VertexBufferUsage
		{
		public:
			Mogre::HardwareBuffer::Usage get();
		}

		Mogre::SubMesh^ CreateSubMesh();

		Mogre::SubMesh^ CreateSubMesh(String^ name);

		void NameSubMesh(String^ name, Mogre::ushort index);

		Mogre::ushort _getSubMeshIndex(String^ name);

		Mogre::SubMesh^ GetSubMesh(unsigned short index);

		Mogre::SubMesh^ GetSubMesh(String^ name);

		//Mogre::Mesh::SubMeshIterator^ GetSubMeshIterator();

		Mogre::MeshPtr^ Clone(String^ newName, String^ newGroup);
		Mogre::MeshPtr^ Clone(String^ newName);

		void BuildEdgeList();
		void FreeEdgeList();
		void PrepareForShadowVolume();

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
				if (_sharedPtr->useCount() > 1)
				{
					delete _sharedPtr;
				}

				_sharedPtr = 0;
			}
		}

		~MeshPtr()
		{
			this->!MeshPtr();
		}

	public:
		static operator MeshPtr ^ (const Ogre::MeshPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew MeshPtr(const_cast<Ogre::MeshPtr&>(ptr));
		}

		static operator Ogre::MeshPtr& (MeshPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::MeshPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::MeshPtr* (MeshPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

		static MeshPtr^ FromResourcePtr(ResourcePtr^ ptr)
		{
			if (CLR_NULL == ptr) return nullptr;
			void* castptr = dynamic_cast<Ogre::Mesh*>(ptr->_native);
			if (castptr == 0) throw gcnew InvalidCastException("The underlying type of the ResourcePtr object is not of type Mesh.");
			Ogre::MeshPtr meshPtr = ptr->_sharedPtr->staticCast<Ogre::Mesh>();
			return gcnew MeshPtr(meshPtr);
		}

		static operator MeshPtr ^ (ResourcePtr^ ptr)
		{
			MeshPtr^ res = FromResourcePtr(ptr);
			// invalidate previous pointer and return converted pointer
			delete ptr;
			return res;
		}

		MeshPtr(Mesh^ obj) : Mesh(obj->UnmanagedPointer)
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

		Mogre::MeshPtr^ GetByName(String^ name);
		Mogre::MeshPtr^ GetByName(String^ name, String^ groupName);

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