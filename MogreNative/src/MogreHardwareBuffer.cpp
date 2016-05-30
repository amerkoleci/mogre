#include "stdafx.h"
#include "MogreHardwareBuffer.h"
#include "MogreRoot.h"
#include "MogreRenderSystem.h"

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


VertexElement::VertexElement(unsigned short source, size_t offset,
	VertexElementType theType, VertexElementSemantic semantic, unsigned short index)
{
	data.mSource = source;
	data.mOffset = offset;
	data.mType = theType;
	data.mSemantic = semantic;
	data.mIndex = index;
}
//-----------------------------------------------------------------------------
size_t VertexElement::Size::get()
{
	return GetTypeSize(data.mType);
}
//-----------------------------------------------------------------------------
size_t VertexElement::GetTypeSize(VertexElementType etype)
{
	switch (etype)
	{
	case VertexElementType::VET_COLOUR:
	case VertexElementType::VET_COLOUR_ABGR:
	case VertexElementType::VET_COLOUR_ARGB:
		return sizeof(Ogre::RGBA);
	case VertexElementType::VET_FLOAT1:
		return sizeof(float);
	case VertexElementType::VET_FLOAT2:
		return sizeof(float) * 2;
	case VertexElementType::VET_FLOAT3:
		return sizeof(float) * 3;
	case VertexElementType::VET_FLOAT4:
		return sizeof(float) * 4;
	case VertexElementType::VET_SHORT1:
		return sizeof(short);
	case VertexElementType::VET_SHORT2:
		return sizeof(short) * 2;
	case VertexElementType::VET_SHORT3:
		return sizeof(short) * 3;
	case VertexElementType::VET_SHORT4:
		return sizeof(short) * 4;
	case VertexElementType::VET_UBYTE4:
		return sizeof(unsigned char) * 4;
	}
	return 0;
}
//-----------------------------------------------------------------------------
unsigned short VertexElement::GetTypeCount(VertexElementType etype)
{
	switch (etype)
	{
	case VertexElementType::VET_COLOUR:
	case VertexElementType::VET_COLOUR_ABGR:
	case VertexElementType::VET_COLOUR_ARGB:
		return 1;
	case VertexElementType::VET_FLOAT1:
		return 1;
	case VertexElementType::VET_FLOAT2:
		return 2;
	case VertexElementType::VET_FLOAT3:
		return 3;
	case VertexElementType::VET_FLOAT4:
		return 4;
	case VertexElementType::VET_SHORT1:
		return 1;
	case VertexElementType::VET_SHORT2:
		return 2;
	case VertexElementType::VET_SHORT3:
		return 3;
	case VertexElementType::VET_SHORT4:
		return 4;
	case VertexElementType::VET_UBYTE4:
		return 4;
	}

	throw gcnew ArgumentException("Invalid type", "etype");
}
//-----------------------------------------------------------------------------
VertexElementType VertexElement::MultiplyTypeCount(VertexElementType baseType,
	unsigned short count)
{
	switch (baseType)
	{
	case VertexElementType::VET_FLOAT1:
		switch (count)
		{
		case 1:
			return VertexElementType::VET_FLOAT1;
		case 2:
			return VertexElementType::VET_FLOAT2;
		case 3:
			return VertexElementType::VET_FLOAT3;
		case 4:
			return VertexElementType::VET_FLOAT4;
		default:
			break;
		}
		break;
	case VertexElementType::VET_SHORT1:
		switch (count)
		{
		case 1:
			return VertexElementType::VET_SHORT1;
		case 2:
			return VertexElementType::VET_SHORT2;
		case 3:
			return VertexElementType::VET_SHORT3;
		case 4:
			return VertexElementType::VET_SHORT4;
		default:
			break;
		}
		break;
	default:
		break;
	}

	throw gcnew ArgumentException("Invalid base type", "baseType");
}
//--------------------------------------------------------------------------
VertexElementType VertexElement::BestColourVertexElementType::get()
{
	// Use the current render system to determine if possible
	if (Root::Singleton && Root::Singleton->RenderSystem)
	{
		return Root::Singleton->RenderSystem->ColourVertexElementType;
	}
	else
	{
		// We can't know the specific type right now, so pick a type
		// based on platform
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
		return VertexElementType::VET_COLOUR_ARGB; // prefer D3D format on windows
#else
		return VertexElementType::VET_COLOUR_ABGR; // prefer GL format on everything else
#endif

	}
}

void VertexElement::ConvertColourValue(VertexElementType srcType, VertexElementType dstType, Ogre::uint32% ptr)
{
	if (srcType == dstType)
		return;

	// Conversion between ARGB and ABGR is always a case of flipping R/B
	ptr =
		((ptr & 0x00FF0000) >> 16) | ((ptr & 0x000000FF) << 16) | (ptr & 0xFF00FF00);
}

Ogre::uint32 VertexElement::ConvertColourValue(ColourValue src, VertexElementType dst)
{
	switch (dst)
	{
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
	default:
#endif
	case VertexElementType::VET_COLOUR_ARGB:
		return FromColor4(src).getAsARGB();
#if OGRE_PLATFORM != OGRE_PLATFORM_WIN32
	default:
#endif
	case VertexElementType::VET_COLOUR_ABGR:
		return FromColor4(src).getAsABGR();
	};

}

VertexElementType VertexElement::GetBaseType(VertexElementType multiType)
{
	switch (multiType)
	{
	case VertexElementType::VET_FLOAT1:
	case VertexElementType::VET_FLOAT2:
	case VertexElementType::VET_FLOAT3:
	case VertexElementType::VET_FLOAT4:
		return VertexElementType::VET_FLOAT1;
	case VertexElementType::VET_COLOUR:
		return VertexElementType::VET_COLOUR;
	case VertexElementType::VET_COLOUR_ABGR:
		return VertexElementType::VET_COLOUR_ABGR;
	case VertexElementType::VET_COLOUR_ARGB:
		return VertexElementType::VET_COLOUR_ARGB;
	case VertexElementType::VET_SHORT1:
	case VertexElementType::VET_SHORT2:
	case VertexElementType::VET_SHORT3:
	case VertexElementType::VET_SHORT4:
		return VertexElementType::VET_SHORT1;
	case VertexElementType::VET_UBYTE4:
		return VertexElementType::VET_UBYTE4;
	};
	// To keep compiler happy
	return VertexElementType::VET_FLOAT1;
}
