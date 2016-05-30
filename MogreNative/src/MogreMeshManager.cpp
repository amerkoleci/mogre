#include "stdafx.h"
#include "MogreMeshManager.h"

using namespace Mogre;

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
