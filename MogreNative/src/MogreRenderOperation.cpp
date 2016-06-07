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
	return static_cast<Ogre::RenderOperation*>(_native)->vertexData;
}

void RenderOperation::vertexData::set(Mogre::VertexData^ value)
{
	static_cast<Ogre::RenderOperation*>(_native)->vertexData = value;
}

Mogre::RenderOperation::OperationTypes RenderOperation::operationType::get()
{
	return (Mogre::RenderOperation::OperationTypes)static_cast<Ogre::RenderOperation*>(_native)->operationType;
}
void RenderOperation::operationType::set(Mogre::RenderOperation::OperationTypes value)
{
	static_cast<Ogre::RenderOperation*>(_native)->operationType = (Ogre::RenderOperation::OperationType)value;
}

bool RenderOperation::useIndexes::get()
{
	return static_cast<Ogre::RenderOperation*>(_native)->useIndexes;
}

void RenderOperation::useIndexes::set(bool value)
{
	static_cast<Ogre::RenderOperation*>(_native)->useIndexes = value;
}

Mogre::IndexData^ RenderOperation::indexData::get()
{
	return static_cast<Ogre::RenderOperation*>(_native)->indexData;
}
void RenderOperation::indexData::set(Mogre::IndexData^ value)
{
	static_cast<Ogre::RenderOperation*>(_native)->indexData = value;
}

//Mogre::IRenderable^ RenderOperation::srcRenderable::get()
//{
//	return static_cast<Ogre::RenderOperation*>(_native)->srcRenderable;
//}

size_t RenderOperation::numberOfInstances::get()
{
	return static_cast<Ogre::RenderOperation*>(_native)->numberOfInstances;
}

void RenderOperation::numberOfInstances::set(size_t value)
{
	static_cast<Ogre::RenderOperation*>(_native)->numberOfInstances = value;
}

bool RenderOperation::renderToVertexBuffer::get()
{
	return static_cast<Ogre::RenderOperation*>(_native)->renderToVertexBuffer;
}

void RenderOperation::renderToVertexBuffer::set(bool value)
{
	static_cast<Ogre::RenderOperation*>(_native)->renderToVertexBuffer = value;
}

bool RenderOperation::useGlobalInstancingVertexBufferIsAvailable::get()
{
	return static_cast<Ogre::RenderOperation*>(_native)->useGlobalInstancingVertexBufferIsAvailable;
}

void RenderOperation::useGlobalInstancingVertexBufferIsAvailable::set(bool value)
{
	static_cast<Ogre::RenderOperation*>(_native)->useGlobalInstancingVertexBufferIsAvailable = value;
}