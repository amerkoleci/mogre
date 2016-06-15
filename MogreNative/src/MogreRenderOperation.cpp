#include "stdafx.h"
#include "MogreRenderOperation.h"
#include "MogreVertexIndexData.h"

using namespace Mogre;

RenderOperation::RenderOperation()
{
	_createdByCLR = true;
	_native = new Ogre::RenderOperation();
}

Mogre::VertexData^ RenderOperation::vertexData::get()
{
	ReturnCachedObjectGcnew(Mogre::VertexData, _vertexData, _native->vertexData);
}

void RenderOperation::vertexData::set(Mogre::VertexData^ value)
{
	_vertexData = value;
	_native->vertexData = value;
}

Mogre::RenderOperation::OperationTypes RenderOperation::operationType::get()
{
	return (Mogre::RenderOperation::OperationTypes)_native->operationType;
}

void RenderOperation::operationType::set(Mogre::RenderOperation::OperationTypes value)
{
	_native->operationType = (Ogre::RenderOperation::OperationType)value;
}

bool RenderOperation::useIndexes::get()
{
	return _native->useIndexes;
}

void RenderOperation::useIndexes::set(bool value)
{
	_native->useIndexes = value;
}

Mogre::IndexData^ RenderOperation::indexData::get()
{
	ReturnCachedObjectGcnew(Mogre::IndexData, _indexData, _native->indexData);
}

void RenderOperation::indexData::set(Mogre::IndexData^ value)
{
	_indexData = value;
	_native->indexData = value;
}

//Mogre::IRenderable^ RenderOperation::srcRenderable::get()
//{
//	return static_cast<Ogre::RenderOperation*>(_native)->srcRenderable;
//}

size_t RenderOperation::numberOfInstances::get()
{
	return _native->numberOfInstances;
}

void RenderOperation::numberOfInstances::set(size_t value)
{
	_native->numberOfInstances = value;
}

bool RenderOperation::renderToVertexBuffer::get()
{
	return _native->renderToVertexBuffer;
}

void RenderOperation::renderToVertexBuffer::set(bool value)
{
	_native->renderToVertexBuffer = value;
}

bool RenderOperation::useGlobalInstancingVertexBufferIsAvailable::get()
{
	return _native->useGlobalInstancingVertexBufferIsAvailable;
}

void RenderOperation::useGlobalInstancingVertexBufferIsAvailable::set(bool value)
{
	_native->useGlobalInstancingVertexBufferIsAvailable = value;
}