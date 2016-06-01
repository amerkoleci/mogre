#include "stdafx.h"
#include "MogreMeshManager.h"
#include "MogreVertexIndexData.h"

using namespace Mogre;


// ------------- SubMesh
SubMesh::~SubMesh()
{
	this->!SubMesh();
}

SubMesh::!SubMesh()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool SubMesh::IsDisposed::get()
{
	return (_native == nullptr);
}

bool SubMesh::useSharedVertices::get()
{
	return _native->useSharedVertices;
}

void SubMesh::useSharedVertices::set(bool value)
{
	_native->useSharedVertices = value;
}

Mogre::RenderOperation::OperationTypes SubMesh::operationType::get()
{
	return (Mogre::RenderOperation::OperationTypes)_native->operationType;
}

void SubMesh::operationType::set(Mogre::RenderOperation::OperationTypes value)
{
	_native->operationType = (Ogre::RenderOperation::OperationType)value;
}

Mogre::VertexData^ SubMesh::vertexData::get()
{
	return _native->vertexData;
}

void SubMesh::vertexData::set(Mogre::VertexData^ value)
{
	_native->vertexData = value;
}

Mogre::IndexData^ SubMesh::indexData::get()
{
	return _native->indexData;
}

void SubMesh::indexData::set(Mogre::IndexData^ value)
{
	_native->indexData = value;
}

//Mogre::SubMesh::IndexMap^ SubMesh::blendIndexToBoneIndexMap::get()
//{
//	return (CLR_NULL == _blendIndexToBoneIndexMap) ? (_blendIndexToBoneIndexMap = static_cast<Ogre::SubMesh*>(_native)->blendIndexToBoneIndexMap) : _blendIndexToBoneIndexMap;
//}
//
//SubMesh::STLVector_Vector3^ SubMesh::extremityPoints::get()
//{
//	return (CLR_NULL == _extremityPoints) ? (_extremityPoints = static_cast<Ogre::SubMesh*>(_native)->extremityPoints) : _extremityPoints;
//}

Mogre::Mesh^ SubMesh::parent::get()
{
	return _native->parent;
}

void SubMesh::parent::set(Mogre::Mesh^ value)
{
	_native->parent = value;
}

//bool SubMesh::HasTextureAliases::get()
//{
//	return static_cast<const Ogre::SubMesh*>(_native)->hasTextureAliases();
//}
//
//bool SubMesh::IsMatInitialised::get()
//{
//	return static_cast<const Ogre::SubMesh*>(_native)->isMatInitialised();
//}

String^ SubMesh::MaterialName::get()
{
	return TO_CLR_STRING(_native->getMaterialName());
}

void SubMesh::MaterialName::set(String^ matName)
{
	DECLARE_NATIVE_STRING(o_matName, matName);

	_native->setMaterialName(o_matName);
}

//size_t SubMesh::TextureAliasCount::get()
//{
//	return static_cast<const Ogre::SubMesh*>(_native)->getTextureAliasCount();
//}

Mogre::VertexAnimationType SubMesh::VertexAnimationType::get()
{
	return (Mogre::VertexAnimationType)_native->getVertexAnimationType();
}

CPP_DECLARE_STLVECTOR(Mesh::, IndexMap, unsigned short, unsigned short);

//Mesh::Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual, Mogre::IManualResourceLoader^ loader) : Resource((CLRObject*)0)
//{
//	_createdByCLR = true;
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_group, group);
//
//	_native = new Ogre::Mesh(creator, o_name, handle, o_group, isManual, loader);
//	ObjectTable::Add((intptr_t)_native, this, nullptr);
//}

Mesh::Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group, bool isManual) : Resource((Ogre::Resource*)0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	_native = new Ogre::Mesh(creator, o_name, handle, o_group, isManual);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Mesh::Mesh(Mogre::ResourceManager^ creator, String^ name, Mogre::ResourceHandle handle, String^ group) : Resource((Ogre::Resource*)0)
{
	_createdByCLR = true;
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_group, group);
	_native = new Ogre::Mesh(creator, o_name, handle, o_group);
	ObjectTable::Add((intptr_t)_native, this, nullptr);
}

Mogre::VertexData^ Mesh::sharedVertexData::get()
{
	return static_cast<Ogre::Mesh*>(_native)->sharedVertexData;
}
void Mesh::sharedVertexData::set(Mogre::VertexData^ value)
{
	static_cast<Ogre::Mesh*>(_native)->sharedVertexData = value;
}

Mogre::Mesh::IndexMap^ Mesh::sharedBlendIndexToBoneIndexMap::get()
{
	if (CLR_NULL == _sharedBlendIndexToBoneIndexMap)
	{
		/*Ogre::vector<unsigned short> vector;
			static_cast<Ogre::Mesh*>(_native)->sharedBlendIndexToBoneIndexMap.begin(),
			static_cast<Ogre::Mesh*>(_native)->sharedBlendIndexToBoneIndexMap.end());
		_sharedBlendIndexToBoneIndexMap = gcnew Mogre::Mesh::IndexMap(vector);*/
	}
	
	return _sharedBlendIndexToBoneIndexMap;
}

bool Mesh::AutoBuildEdgeLists::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getAutoBuildEdgeLists();
}
void Mesh::AutoBuildEdgeLists::set(bool autobuild)
{
	static_cast<Ogre::Mesh*>(_native)->setAutoBuildEdgeLists(autobuild);
}

Mogre::Real Mesh::BoundingSphereRadius::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getBoundingSphereRadius();
}

Mogre::AxisAlignedBox^ Mesh::Bounds::get()
{
	return  ToAxisAlignedBounds(static_cast<const Ogre::Mesh*>(_native)->getBounds());
}

bool Mesh::HasSkeleton::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->hasSkeleton();
}

bool Mesh::HasVertexAnimation::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->hasVertexAnimation();
}

Mogre::HardwareBuffer::Usage Mesh::IndexBufferUsage::get()
{
	return (Mogre::HardwareBuffer::Usage)static_cast<const Ogre::Mesh*>(_native)->getIndexBufferUsage();
}

bool Mesh::IsEdgeListBuilt::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->isEdgeListBuilt();
}

bool Mesh::IsIndexBufferShadowed::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->isIndexBufferShadowed();
}

bool Mesh::IsPreparedForShadowVolumes::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->isPreparedForShadowVolumes();
}

bool Mesh::IsVertexBufferShadowed::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->isVertexBufferShadowed();
}

unsigned short Mesh::NumAnimations::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getNumAnimations();
}

Mogre::ushort Mesh::NumLodLevels::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getNumLodLevels();
}

unsigned short Mesh::NumSubMeshes::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getNumSubMeshes();
}

size_t Mesh::PoseCount::get()
{
	return static_cast<const Ogre::Mesh*>(_native)->getPoseCount();
}

Mogre::VertexAnimationType Mesh::SharedVertexDataAnimationType::get()
{
	return (Mogre::VertexAnimationType)static_cast<const Ogre::Mesh*>(_native)->getSharedVertexDataAnimationType();
}

String^ Mesh::SkeletonName::get()
{
	return TO_CLR_STRING(static_cast<const Ogre::Mesh*>(_native)->getSkeletonName());
}

void Mesh::SkeletonName::set(String^ skelName)
{
	DECLARE_NATIVE_STRING(o_skelName, skelName);

	static_cast<Ogre::Mesh*>(_native)->setSkeletonName(o_skelName);
}

Mogre::HardwareBuffer::Usage Mesh::VertexBufferUsage::get()
{
	return (Mogre::HardwareBuffer::Usage)static_cast<const Ogre::Mesh*>(_native)->getVertexBufferUsage();
}

Mogre::SubMesh^ Mesh::CreateSubMesh()
{
	return static_cast<Ogre::Mesh*>(_native)->createSubMesh();
}

Mogre::SubMesh^ Mesh::CreateSubMesh(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<Ogre::Mesh*>(_native)->createSubMesh(o_name);
}

void Mesh::NameSubMesh(String^ name, Mogre::ushort index)
{
	DECLARE_NATIVE_STRING(o_name, name);

	static_cast<Ogre::Mesh*>(_native)->nameSubMesh(o_name, index);
}

Mogre::ushort Mesh::_getSubMeshIndex(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::Mesh*>(_native)->_getSubMeshIndex(o_name);
}

Mogre::SubMesh^ Mesh::GetSubMesh(unsigned short index)
{
	return static_cast<const Ogre::Mesh*>(_native)->getSubMesh(index);
}

Mogre::SubMesh^ Mesh::GetSubMesh(String^ name)
{
	DECLARE_NATIVE_STRING(o_name, name);

	return static_cast<const Ogre::Mesh*>(_native)->getSubMesh(o_name);
}

//Mogre::Mesh::SubMeshIterator^ Mesh::GetSubMeshIterator()
//{
//	return static_cast<Ogre::Mesh*>(_native)->getSubMeshIterator();
//}

Mogre::MeshPtr^ Mesh::Clone(String^ newName, String^ newGroup)
{
	DECLARE_NATIVE_STRING(o_newName, newName);
	DECLARE_NATIVE_STRING(o_newGroup, newGroup);

	return static_cast<Ogre::Mesh*>(_native)->clone(o_newName, o_newGroup);
}

Mogre::MeshPtr^ Mesh::Clone(String^ newName)
{
	DECLARE_NATIVE_STRING(o_newName, newName);

	return static_cast<Ogre::Mesh*>(_native)->clone(o_newName);
}

void Mesh::BuildEdgeList()
{
	static_cast<Ogre::Mesh*>(_native)->buildEdgeList();
}

void Mesh::FreeEdgeList()
{
	static_cast<Ogre::Mesh*>(_native)->freeEdgeList();
}

void Mesh::PrepareForShadowVolume()
{
	static_cast<Ogre::Mesh*>(_native)->prepareForShadowVolume();
}

// ------------- MeshLodUsage

MeshLodUsage::MeshLodUsage()
{
	_createdByCLR = true;
	_native = new Ogre::MeshLodUsage();
}

String^ MeshLodUsage::manualName::get()
{
	return TO_CLR_STRING(static_cast<Ogre::MeshLodUsage*>(_native)->manualName);
}

void MeshLodUsage::manualName::set(String^ value)
{
	DECLARE_NATIVE_STRING(o_value, value);

	static_cast<Ogre::MeshLodUsage*>(_native)->manualName = o_value;
}

Mogre::MeshPtr^ MeshLodUsage::manualMesh::get()
{
	return static_cast<Ogre::MeshLodUsage*>(_native)->manualMesh;
}

void MeshLodUsage::manualMesh::set(Mogre::MeshPtr^ value)
{
	static_cast<Ogre::MeshLodUsage*>(_native)->manualMesh = (Ogre::MeshPtr)value;
}

/*Mogre::EdgeData^ MeshLodUsage::edgeData::get()
{
	return static_cast<Ogre::MeshLodUsage*>(_native)->edgeData;
}

void MeshLodUsage::edgeData::set(Mogre::EdgeData^ value)
{
	static_cast<Ogre::MeshLodUsage*>(_native)->edgeData = value;
}*/

Ogre::Real MeshManager::BoundsPaddingFactor::get()
{
	return static_cast<Ogre::MeshManager*>(_native)->getBoundsPaddingFactor();
}

void MeshManager::BoundsPaddingFactor::set(Ogre::Real paddingFactor)
{
	static_cast<Ogre::MeshManager*>(_native)->setBoundsPaddingFactor(paddingFactor);
}

bool MeshManager::PrepareAllMeshesForShadowVolumes::get()
{
	return static_cast<Ogre::MeshManager*>(_native)->getPrepareAllMeshesForShadowVolumes();
}

void MeshManager::PrepareAllMeshesForShadowVolumes::set(bool enable)
{
	static_cast<Ogre::MeshManager*>(_native)->setPrepareAllMeshesForShadowVolumes(enable);
}

//generic<typename T> where T : value class
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, array<T>^ controlPointArray, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage, bool vbUseShadow, bool ibUseShadow)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//	pin_ptr<T> buf = &controlPointArray[0];
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, buf, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide, (Ogre::HardwareBuffer::Usage)vbUsage, (Ogre::HardwareBuffer::Usage)ibUsage, vbUseShadow, ibUseShadow);
//}

Mogre::MeshPtr^ MeshManager::Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->load(o_filename, o_groupName, (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexBufferShadowed, indexBufferShadowed);
}

Mogre::MeshPtr^ MeshManager::Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->load(o_filename, o_groupName, (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexBufferShadowed);
}

Mogre::MeshPtr^ MeshManager::Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->load(o_filename, o_groupName, (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::Load(String^ filename, String^ groupName, Mogre::HardwareBuffer::Usage vertexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->load(o_filename, o_groupName, (Ogre::HardwareBuffer::Usage)vertexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::Load(String^ filename, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_filename, filename);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->load(o_filename, o_groupName);
}

//Mogre::MeshPtr^ MeshManager::CreateManual(String^ name, String^ groupName, Mogre::IManualResourceLoader^ loader)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createManual(o_name, o_groupName, loader);
//}

Mogre::MeshPtr^ MeshManager::CreateManual(String^ name, String^ groupName)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createManual(o_name, o_groupName);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer, indexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector));
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets, uTile);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals, int numTexCoordSets)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals, numTexCoordSets);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments, bool normals)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments, normals);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments, int ysegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments, ysegments);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, int xsegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height, xsegments);
}

Mogre::MeshPtr^ MeshManager::CreatePlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createPlane(o_name, o_groupName, FromPlane(plane), width, height);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer, int ySegmentsToKeep)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer, indexShadowBuffer, ySegmentsToKeep);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer, indexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation, Mogre::HardwareBuffer::Usage vertexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation), (Ogre::HardwareBuffer::Usage)vertexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector, Mogre::Quaternion orientation)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector), FromQuaternion(orientation));
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile, Mogre::Vector3 upVector)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile, FromVector3(upVector));
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile, Mogre::Real vTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile, vTile);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real uTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets, uTile);
}
Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals, int numTexCoordSets)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals, numTexCoordSets);
}
Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments, bool normals)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments, normals);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments, int ysegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments, ysegments);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature, int xsegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature, xsegments);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedIllusionPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real curvature)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedIllusionPlane(o_name, o_groupName, FromPlane(plane), width, height, curvature);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer, bool indexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer, indexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage, bool vertexShadowBuffer)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage, vertexShadowBuffer);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage, Mogre::HardwareBuffer::Usage indexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage, (Ogre::HardwareBuffer::Usage)indexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector, Mogre::HardwareBuffer::Usage vertexBufferUsage)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile, FromVector3(upVector), (Ogre::HardwareBuffer::Usage)vertexBufferUsage);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile, Mogre::Vector3 upVector)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile, FromVector3(upVector));
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile, Mogre::Real yTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile, yTile);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets, Mogre::Real xTile)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets, xTile);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals, int numTexCoordSets)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals, numTexCoordSets);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments, bool normals)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments, normals);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments, int ysegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments, ysegments);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow, int xsegments)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow, xsegments);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height, Mogre::Real bow)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height, bow);
}

Mogre::MeshPtr^ MeshManager::CreateCurvedPlane(String^ name, String^ groupName, Mogre::Plane plane, Mogre::Real width, Mogre::Real height)
{
	DECLARE_NATIVE_STRING(o_name, name);
	DECLARE_NATIVE_STRING(o_groupName, groupName);

	return static_cast<Ogre::MeshManager*>(_native)->createCurvedPlane(o_name, o_groupName, FromPlane(plane), width, height);
}

//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage, bool vbUseShadow, bool ibUseShadow)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide, (Ogre::HardwareBuffer::Usage)vbUsage, (Ogre::HardwareBuffer::Usage)ibUsage, vbUseShadow, ibUseShadow);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage, bool vbUseShadow)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide, (Ogre::HardwareBuffer::Usage)vbUsage, (Ogre::HardwareBuffer::Usage)ibUsage, vbUseShadow);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage, Mogre::HardwareBuffer::Usage ibUsage)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide, (Ogre::HardwareBuffer::Usage)vbUsage, (Ogre::HardwareBuffer::Usage)ibUsage);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide, Mogre::HardwareBuffer::Usage vbUsage)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide, (Ogre::HardwareBuffer::Usage)vbUsage);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel, Mogre::PatchSurface::VisibleSide visibleSide)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel, (Ogre::PatchSurface::VisibleSide)visibleSide);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel, size_t vMaxSubdivisionLevel)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel, vMaxSubdivisionLevel);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height, size_t uMaxSubdivisionLevel)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height, uMaxSubdivisionLevel);
//}
//Mogre::PatchMeshPtr^ MeshManager::CreateBezierPatch(String^ name, String^ groupName, void* controlPointBuffer, Mogre::VertexDeclaration^ declaration, size_t width, size_t height)
//{
//	DECLARE_NATIVE_STRING(o_name, name);
//	DECLARE_NATIVE_STRING(o_groupName, groupName);
//
//	return static_cast<Ogre::MeshManager*>(_native)->createBezierPatch(o_name, o_groupName, controlPointBuffer, declaration, width, height);
//}

void MeshManager::LoadResource(Mogre::Resource^ res)
{
	static_cast<Ogre::MeshManager*>(_native)->loadResource(GetPointerOrNull(res));
}
