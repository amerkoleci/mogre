#include "stdafx.h"
#include "MogreVertexIndexData.h"

using namespace Mogre;


Mogre::Real VertexData::HardwareAnimationData_NativePtr::parametric::get()
{
	return _native->parametric;
}
void VertexData::HardwareAnimationData_NativePtr::parametric::set(Mogre::Real value)
{
	_native->parametric = value;
}


Mogre::VertexData::HardwareAnimationData_NativePtr VertexData::HardwareAnimationData_NativePtr::Create()
{
	HardwareAnimationData_NativePtr ptr;
	ptr._native = new Ogre::VertexData::HardwareAnimationData();
	return ptr;
}

CPP_DECLARE_STLVECTOR(VertexData::, HardwareAnimationDataList, Mogre::VertexData::HardwareAnimationData_NativePtr, Ogre::VertexData::HardwareAnimationData);

VertexData::VertexData()
{
	_createdByCLR = true;
	_native = new Ogre::VertexData();
	if (!ObjectTable::Contains((intptr_t)_native))
	{
		ObjectTable::Add((intptr_t)_native, this, nullptr);
	}
}

VertexData::~VertexData()
{
	this->!VertexData();
}

VertexData::!VertexData()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR &&_native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool VertexData::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::VertexDeclaration^ VertexData::vertexDeclaration::get()
{
	return _native->vertexDeclaration;
}

void VertexData::vertexDeclaration::set(Mogre::VertexDeclaration^ value)
{
	_native->vertexDeclaration = value;
}

Mogre::VertexBufferBinding^ VertexData::vertexBufferBinding::get()
{
	return _native->vertexBufferBinding;
}

void VertexData::vertexBufferBinding::set(Mogre::VertexBufferBinding^ value)
{
	_native->vertexBufferBinding = value;
}

size_t VertexData::vertexStart::get()
{
	return _native->vertexStart;
}

void VertexData::vertexStart::set(size_t value)
{
	_native->vertexStart = value;
}

size_t VertexData::vertexCount::get()
{
	return _native->vertexCount;
}

void VertexData::vertexCount::set(size_t value)
{
	_native->vertexCount = value;
}

Mogre::VertexData::HardwareAnimationDataList^ VertexData::hwAnimationDataList::get()
{
	return (CLR_NULL == _hwAnimationDataList) ? (_hwAnimationDataList = _native->hwAnimationDataList) : _hwAnimationDataList;
}

size_t VertexData::hwAnimDataItemsUsed::get()
{
	return _native->hwAnimDataItemsUsed;
}

void VertexData::hwAnimDataItemsUsed::set(size_t value)
{
	_native->hwAnimDataItemsUsed = value;
}

Mogre::HardwareVertexBufferSharedPtr^ VertexData::hardwareShadowVolWBuffer::get()
{
	return _native->hardwareShadowVolWBuffer;
}

void VertexData::hardwareShadowVolWBuffer::set(Mogre::HardwareVertexBufferSharedPtr^ value)
{
	_native->hardwareShadowVolWBuffer = (Ogre::HardwareVertexBufferSharedPtr)value;
}

Mogre::VertexData^ VertexData::Clone(bool copyData)
{
	return _native->clone(copyData);
}

Mogre::VertexData^ VertexData::Clone()
{
	return _native->clone();
}

void VertexData::PrepareForShadowVolume()
{
	_native->prepareForShadowVolume();
}

void VertexData::ReorganiseBuffers(Mogre::VertexDeclaration^ newDeclaration, Mogre::Const_BufferUsageList^ bufferUsage)
{
	_native->reorganiseBuffers(newDeclaration, bufferUsage);
}

void VertexData::ReorganiseBuffers(Mogre::VertexDeclaration^ newDeclaration)
{
	_native->reorganiseBuffers(newDeclaration);
}

void VertexData::CloseGapsInBindings()
{
	_native->closeGapsInBindings();
}

void VertexData::RemoveUnusedBuffers()
{
	_native->removeUnusedBuffers();
}

void VertexData::ConvertPackedColour(Mogre::VertexElementType srcType, Mogre::VertexElementType destType)
{
	_native->convertPackedColour((Ogre::VertexElementType)srcType, (Ogre::VertexElementType)destType);
}

void VertexData::AllocateHardwareAnimationElements(Mogre::ushort count, bool animateNormals)
{
	_native->allocateHardwareAnimationElements(count, animateNormals);
}

//  ------------------  IndexData

IndexData::IndexData()
{
	_createdByCLR = true;
	_native = new Ogre::IndexData();
	if (!ObjectTable::Contains((intptr_t)_native))
	{
		ObjectTable::Add((intptr_t)_native, this, nullptr);
	}
}

IndexData::~IndexData()
{
	this->!IndexData();
}

IndexData::!IndexData()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR &&_native)
	{
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

bool IndexData::IsDisposed::get()
{
	return (_native == nullptr);
}

Mogre::HardwareIndexBufferSharedPtr^ IndexData::indexBuffer::get()
{
	return _native->indexBuffer;
}

void IndexData::indexBuffer::set(Mogre::HardwareIndexBufferSharedPtr^ value)
{
	_native->indexBuffer = (Ogre::HardwareIndexBufferSharedPtr)value;
}

size_t IndexData::indexStart::get()
{
	return _native->indexStart;
}
void IndexData::indexStart::set(size_t value)
{
	_native->indexStart = value;
}

size_t IndexData::indexCount::get()
{
	return _native->indexCount;
}

void IndexData::indexCount::set(size_t value)
{
	_native->indexCount = value;
}

Mogre::IndexData^ IndexData::Clone(bool copyData)
{
	return _native->clone(copyData);
}
Mogre::IndexData^ IndexData::Clone()
{
	return _native->clone();
}

void IndexData::OptimiseVertexCacheTriList()
{
	static_cast<Ogre::IndexData*>(_native)->optimiseVertexCacheTriList();
}

CPP_DECLARE_STLVECTOR(, BufferUsageList, Mogre::HardwareBuffer::Usage, Ogre::HardwareBuffer::Usage);