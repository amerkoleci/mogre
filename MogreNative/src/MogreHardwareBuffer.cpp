#include "stdafx.h"
#include "MogreHardwareBuffer.h"

using namespace Mogre;

HardwareBuffer::~HardwareBuffer()
{
	this->!HardwareBuffer();
}

HardwareBuffer::!HardwareBuffer()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0)
	{
		delete _native; _native = 0;
	}

	OnDisposed(this, nullptr);
}

bool HardwareBuffer::HasShadowBuffer::get()
{
	return static_cast<const Ogre::HardwareBuffer*>(_native)->hasShadowBuffer();
}

bool HardwareBuffer::IsLocked::get()
{
	return static_cast<const Ogre::HardwareBuffer*>(_native)->isLocked();
}

bool HardwareBuffer::IsSystemMemory::get()
{
	return static_cast<const Ogre::HardwareBuffer*>(_native)->isSystemMemory();
}

size_t HardwareBuffer::SizeInBytes::get()
{
	return static_cast<const Ogre::HardwareBuffer*>(_native)->getSizeInBytes();
}

void* HardwareBuffer::Lock(size_t offset, size_t length, Mogre::HardwareBuffer::LockOptions options)
{
	return static_cast<Ogre::HardwareBuffer*>(_native)->lock(offset, length, (Ogre::HardwareBuffer::LockOptions)options);
}

void* HardwareBuffer::Lock(Mogre::HardwareBuffer::LockOptions options)
{
	return static_cast<Ogre::HardwareBuffer*>(_native)->lock((Ogre::HardwareBuffer::LockOptions)options);
}

void HardwareBuffer::Unlock()
{
	static_cast<Ogre::HardwareBuffer*>(_native)->unlock();
}

void HardwareBuffer::ReadData(size_t offset, size_t length, void* pDest)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->readData(offset, length, pDest);
}

void HardwareBuffer::WriteData(size_t offset, size_t length, const void* pSource, bool discardWholeBuffer)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->writeData(offset, length, pSource, discardWholeBuffer);
}
void HardwareBuffer::WriteData(size_t offset, size_t length, const void* pSource)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->writeData(offset, length, pSource);
}

void HardwareBuffer::CopyData(Mogre::HardwareBuffer^ srcBuffer, size_t srcOffset, size_t dstOffset, size_t length, bool discardWholeBuffer)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->copyData(srcBuffer, srcOffset, dstOffset, length, discardWholeBuffer);
}
void HardwareBuffer::CopyData(Mogre::HardwareBuffer^ srcBuffer, size_t srcOffset, size_t dstOffset, size_t length)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->copyData(srcBuffer, srcOffset, dstOffset, length);
}

void HardwareBuffer::_updateFromShadow()
{
	static_cast<Ogre::HardwareBuffer*>(_native)->_updateFromShadow();
}

Mogre::HardwareBuffer::Usage HardwareBuffer::GetUsage()
{
	return (Mogre::HardwareBuffer::Usage)static_cast<const Ogre::HardwareBuffer*>(_native)->getUsage();
}

void HardwareBuffer::SuppressHardwareUpdate(bool suppress)
{
	static_cast<Ogre::HardwareBuffer*>(_native)->suppressHardwareUpdate(suppress);
}

Ogre::HardwareBuffer* HardwareBuffer::UnmanagedPointer::get()
{
	return _native;
}

size_t HardwareIndexBuffer::IndexSize::get()
{
	return static_cast<const Ogre::HardwareIndexBuffer*>(_native)->getIndexSize();
}

size_t HardwareIndexBuffer::NumIndexes::get()
{
	return static_cast<const Ogre::HardwareIndexBuffer*>(_native)->getNumIndexes();
}

Mogre::HardwareIndexBuffer::IndexType HardwareIndexBuffer::Type::get()
{
	return (Mogre::HardwareIndexBuffer::IndexType)static_cast<const Ogre::HardwareIndexBuffer*>(_native)->getType();
}

size_t HardwareVertexBuffer::NumVertices::get()
{
	return static_cast<const Ogre::HardwareVertexBuffer*>(_native)->getNumVertices();
}

size_t HardwareVertexBuffer::VertexSize::get()
{
	return static_cast<const Ogre::HardwareVertexBuffer*>(_native)->getVertexSize();
}

Mogre::PixelBox HardwarePixelBuffer::CurrentLock::get()
{
	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->getCurrentLock();
}

size_t HardwarePixelBuffer::Depth::get()
{
	return static_cast<const Ogre::HardwarePixelBuffer*>(_native)->getDepth();
}

Mogre::PixelFormat HardwarePixelBuffer::Format::get()
{
	return (Mogre::PixelFormat)static_cast<const Ogre::HardwarePixelBuffer*>(_native)->getFormat();
}

size_t HardwarePixelBuffer::Height::get()
{
	return static_cast<const Ogre::HardwarePixelBuffer*>(_native)->getHeight();
}

size_t HardwarePixelBuffer::Width::get()
{
	return static_cast<const Ogre::HardwarePixelBuffer*>(_native)->getWidth();
}

Mogre::PixelBox HardwarePixelBuffer::Lock(Mogre::Image::Box lockBox, Mogre::HardwareBuffer::LockOptions options)
{
	Ogre::Image::Box ogreLockBox(lockBox.left, lockBox.top, lockBox.front, lockBox.right, lockBox.bottom, lockBox.back);
	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->lock(ogreLockBox, (Ogre::HardwareBuffer::LockOptions)options);
}

void* HardwarePixelBuffer::Lock(size_t offset, size_t length, Mogre::HardwareBuffer::LockOptions options)
{
	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->lock(offset, length, (Ogre::HardwareBuffer::LockOptions)options);
}

void HardwarePixelBuffer::ReadData(size_t offset, size_t length, void* pDest)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->readData(offset, length, pDest);
}

void HardwarePixelBuffer::WriteData(size_t offset, size_t length, const void* pSource, bool discardWholeBuffer)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->writeData(offset, length, pSource, discardWholeBuffer);
}
void HardwarePixelBuffer::WriteData(size_t offset, size_t length, const void* pSource)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->writeData(offset, length, pSource);
}

void HardwarePixelBuffer::Blit(Mogre::HardwarePixelBufferSharedPtr^ src, Mogre::Image::Box srcBox, Mogre::Image::Box dstBox)
{
	Ogre::Image::Box ogreSrcBox(srcBox.left, srcBox.top, srcBox.front, srcBox.right, srcBox.bottom, srcBox.back);
	Ogre::Image::Box ogreDstBox(dstBox.left, dstBox.top, dstBox.front, dstBox.right, dstBox.bottom, dstBox.back);
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blit((const Ogre::HardwarePixelBufferSharedPtr&)src, ogreSrcBox, ogreDstBox);
}

void HardwarePixelBuffer::Blit(Mogre::HardwarePixelBufferSharedPtr^ src)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blit((const Ogre::HardwarePixelBufferSharedPtr&)src);
}

void HardwarePixelBuffer::BlitFromMemory(Mogre::PixelBox src, Mogre::Image::Box dstBox)
{
	Ogre::Image::Box ogreDstBox(dstBox.left, dstBox.top, dstBox.front, dstBox.right, dstBox.bottom, dstBox.back);
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blitFromMemory(src, ogreDstBox);
}

void HardwarePixelBuffer::BlitFromMemory(Mogre::PixelBox src)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blitFromMemory(src);
}

void HardwarePixelBuffer::BlitToMemory(Mogre::Image::Box srcBox, Mogre::PixelBox dst)
{
	Ogre::Image::Box ogreSrcBox(srcBox.left, srcBox.top, srcBox.front, srcBox.right, srcBox.bottom, srcBox.back);
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blitToMemory(ogreSrcBox, dst);
}

void HardwarePixelBuffer::BlitToMemory(Mogre::PixelBox dst)
{
	static_cast<Ogre::HardwarePixelBuffer*>(_native)->blitToMemory(dst);
}

//Mogre::RenderTexture^ HardwarePixelBuffer::GetRenderTarget(size_t slice)
//{
//	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->getRenderTarget(slice);
//}
//
//Mogre::RenderTexture^ HardwarePixelBuffer::GetRenderTarget()
//{
//	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->getRenderTarget();
//}
