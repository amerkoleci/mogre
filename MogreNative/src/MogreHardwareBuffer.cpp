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