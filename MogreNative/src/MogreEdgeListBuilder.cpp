#include "stdafx.h"
#include "MogreEdgeListBuilder.h"
#include "MogreVertexIndexData.h"
#include "MogreHardwareBuffer.h"
#include "MogreLog.h"

using namespace Mogre;

size_t* EdgeData::Edge_NativePtr::triIndex::get()
{
	return _native->triIndex;
}

size_t* EdgeData::Edge_NativePtr::vertIndex::get()
{
	return _native->vertIndex;
}

size_t* EdgeData::Edge_NativePtr::sharedVertIndex::get()
{
	return _native->sharedVertIndex;
}

bool EdgeData::Edge_NativePtr::degenerate::get()
{
	return _native->degenerate;
}
void EdgeData::Edge_NativePtr::degenerate::set(bool value)
{
	_native->degenerate = value;
}

Mogre::EdgeData::Edge_NativePtr EdgeData::Edge_NativePtr::Create()
{
	Edge_NativePtr ptr;
	ptr._native = new Ogre::EdgeData::Edge();
	return ptr;
}

size_t EdgeData::EdgeGroup_NativePtr::vertexSet::get()
{
	return _native->vertexSet;
}
void EdgeData::EdgeGroup_NativePtr::vertexSet::set(size_t value)
{
	_native->vertexSet = value;
}

Mogre::VertexData^ EdgeData::EdgeGroup_NativePtr::vertexData::get()
{
	return _native->vertexData;
}

size_t EdgeData::EdgeGroup_NativePtr::triStart::get()
{
	return _native->triStart;
}
void EdgeData::EdgeGroup_NativePtr::triStart::set(size_t value)
{
	_native->triStart = value;
}

size_t EdgeData::EdgeGroup_NativePtr::triCount::get()
{
	return _native->triCount;
}
void EdgeData::EdgeGroup_NativePtr::triCount::set(size_t value)
{
	_native->triCount = value;
}

Mogre::EdgeData::EdgeList^ EdgeData::EdgeGroup_NativePtr::edges::get()
{
	return Mogre::EdgeData::EdgeList::ByValue(_native->edges);
}
void EdgeData::EdgeGroup_NativePtr::edges::set(Mogre::EdgeData::EdgeList^ value)
{
	_native->edges = value;
}


Mogre::EdgeData::EdgeGroup_NativePtr EdgeData::EdgeGroup_NativePtr::Create()
{
	EdgeGroup_NativePtr ptr;
	ptr._native = new Ogre::EdgeData::EdgeGroup();
	return ptr;
}

size_t EdgeData::Triangle_NativePtr::indexSet::get()
{
	return _native->indexSet;
}
void EdgeData::Triangle_NativePtr::indexSet::set(size_t value)
{
	_native->indexSet = value;
}

size_t EdgeData::Triangle_NativePtr::vertexSet::get()
{
	return _native->vertexSet;
}
void EdgeData::Triangle_NativePtr::vertexSet::set(size_t value)
{
	_native->vertexSet = value;
}

size_t* EdgeData::Triangle_NativePtr::vertIndex::get()
{
	return _native->vertIndex;
}

size_t* EdgeData::Triangle_NativePtr::sharedVertIndex::get()
{
	return _native->sharedVertIndex;
}


Mogre::EdgeData::Triangle_NativePtr EdgeData::Triangle_NativePtr::Create()
{
	Triangle_NativePtr ptr;
	ptr._native = new Ogre::EdgeData::Triangle();
	return ptr;
}

CPP_DECLARE_STLVECTOR(EdgeData::, TriangleFaceNormalList, Mogre::Vector4, Ogre::Vector4);
CPP_DECLARE_STLVECTOR(EdgeData::, TriangleLightFacingList, char, char);
CPP_DECLARE_STLVECTOR(EdgeData::, TriangleList, Mogre::EdgeData::Triangle_NativePtr, Ogre::EdgeData::Triangle);
CPP_DECLARE_STLVECTOR(EdgeData::, EdgeList, Mogre::EdgeData::Edge_NativePtr, Ogre::EdgeData::Edge);
CPP_DECLARE_STLVECTOR(EdgeData::, EdgeGroupList, Mogre::EdgeData::EdgeGroup_NativePtr, Ogre::EdgeData::EdgeGroup);

EdgeData::EdgeData()
{
	_createdByCLR = true;
	_native = new Ogre::EdgeData();
}

EdgeData::~EdgeData()
{
	this->!EdgeData();
}

EdgeData::!EdgeData()
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

Mogre::EdgeData::TriangleList^ EdgeData::triangles::get()
{
	return (CLR_NULL == _triangles) ? (_triangles = static_cast<Ogre::EdgeData*>(_native)->triangles) : _triangles;
}

Mogre::EdgeData::TriangleLightFacingList^ EdgeData::triangleLightFacings::get()
{
	return (CLR_NULL == _triangleLightFacings) ? (_triangleLightFacings = static_cast<Ogre::EdgeData*>(_native)->triangleLightFacings) : _triangleLightFacings;
}

Mogre::EdgeData::EdgeGroupList^ EdgeData::edgeGroups::get()
{
	return (CLR_NULL == _edgeGroups) ? (_edgeGroups = static_cast<Ogre::EdgeData*>(_native)->edgeGroups) : _edgeGroups;
}

bool EdgeData::isClosed::get()
{
	return static_cast<Ogre::EdgeData*>(_native)->isClosed;
}
void EdgeData::isClosed::set(bool value)
{
	static_cast<Ogre::EdgeData*>(_native)->isClosed = value;
}

void EdgeData::UpdateTriangleLightFacing(Mogre::Vector4 lightPos)
{
	static_cast<Ogre::EdgeData*>(_native)->updateTriangleLightFacing(FromVector4(lightPos));
}

void EdgeData::UpdateFaceNormals(size_t vertexSet, Mogre::HardwareVertexBufferSharedPtr^ positionBuffer)
{
	static_cast<Ogre::EdgeData*>(_native)->updateFaceNormals(vertexSet, (const Ogre::HardwareVertexBufferSharedPtr&)positionBuffer);
}

void EdgeData::Log(Mogre::Log^ log)
{
	static_cast<Ogre::EdgeData*>(_native)->log(GetPointerOrNull(log));
}