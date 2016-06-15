#include "stdafx.h"
#include "MogreHardwareBuffer.h"
#include "MogreRoot.h"
#include "MogreRenderSystem.h"
#include "MogreRenderTarget.h"
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

Mogre::RenderTexture^ HardwarePixelBuffer::GetRenderTarget(size_t slice)
{
	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->getRenderTarget(slice);
}

Mogre::RenderTexture^ HardwarePixelBuffer::GetRenderTarget()
{
	return static_cast<Ogre::HardwarePixelBuffer*>(_native)->getRenderTarget();
}


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


CPP_DECLARE_STLLIST_READONLY(VertexDeclaration::, VertexElementList, Mogre::VertexElement^, Ogre::VertexElement);

//Public Declarations
VertexDeclaration::VertexDeclaration()
{
	_createdByCLR = true;
	_native = new Ogre::VertexDeclaration();
	ObjectTable::Add((IntPtr)_native, this, nullptr);
}

VertexDeclaration::~VertexDeclaration()
{
	this->!VertexDeclaration();
}

VertexDeclaration::!VertexDeclaration()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0)
	{
		ObjectTable::Remove((IntPtr)_native);
		delete _native;
		_native = 0;
	}

	OnDisposed(this, nullptr);
}

size_t VertexDeclaration::ElementCount::get()
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->getElementCount();
}

unsigned short VertexDeclaration::MaxSource::get()
{
	return static_cast<const Ogre::VertexDeclaration*>(_native)->getMaxSource();
}

Mogre::VertexDeclaration::Const_VertexElementList^ VertexDeclaration::GetElements()
{
	return static_cast<const Ogre::VertexDeclaration*>(_native)->getElements();
}

Mogre::VertexElement^ VertexDeclaration::GetElement(unsigned short index)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->getElement(index);
}

void VertexDeclaration::Sort()
{
	static_cast<Ogre::VertexDeclaration*>(_native)->sort();
}

void VertexDeclaration::CloseGapsInSource()
{
	static_cast<Ogre::VertexDeclaration*>(_native)->closeGapsInSource();
}

Mogre::VertexDeclaration^ VertexDeclaration::GetAutoOrganisedDeclaration(bool skeletalAnimation, bool vertexAnimation, bool vertexAnimationNormals)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->getAutoOrganisedDeclaration(skeletalAnimation, vertexAnimation, vertexAnimationNormals);
}

Mogre::VertexElement^ VertexDeclaration::AddElement(unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->addElement(source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic, index);
}
Mogre::VertexElement^ VertexDeclaration::AddElement(unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->addElement(source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic);
}

Mogre::VertexElement^ VertexDeclaration::InsertElement(unsigned short atPosition, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->insertElement(atPosition, source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic, index);
}
Mogre::VertexElement^ VertexDeclaration::InsertElement(unsigned short atPosition, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->insertElement(atPosition, source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic);
}

void VertexDeclaration::RemoveElement(unsigned short elem_index)
{
	static_cast<Ogre::VertexDeclaration*>(_native)->removeElement(elem_index);
}

void VertexDeclaration::RemoveElement(Mogre::VertexElementSemantic semantic, unsigned short index)
{
	static_cast<Ogre::VertexDeclaration*>(_native)->removeElement((Ogre::VertexElementSemantic)semantic, index);
}
void VertexDeclaration::RemoveElement(Mogre::VertexElementSemantic semantic)
{
	static_cast<Ogre::VertexDeclaration*>(_native)->removeElement((Ogre::VertexElementSemantic)semantic);
}

void VertexDeclaration::RemoveAllElements()
{
	static_cast<Ogre::VertexDeclaration*>(_native)->removeAllElements();
}

void VertexDeclaration::ModifyElement(unsigned short elem_index, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index)
{
	static_cast<Ogre::VertexDeclaration*>(_native)->modifyElement(elem_index, source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic, index);
}
void VertexDeclaration::ModifyElement(unsigned short elem_index, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic)
{
	static_cast<Ogre::VertexDeclaration*>(_native)->modifyElement(elem_index, source, offset, (Ogre::VertexElementType)theType, (Ogre::VertexElementSemantic)semantic);
}

Mogre::VertexElement^ VertexDeclaration::FindElementBySemantic(Mogre::VertexElementSemantic sem, unsigned short index)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->findElementBySemantic((Ogre::VertexElementSemantic)sem, index);
}
Mogre::VertexElement^ VertexDeclaration::FindElementBySemantic(Mogre::VertexElementSemantic sem)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->findElementBySemantic((Ogre::VertexElementSemantic)sem);
}

Mogre::VertexDeclaration::Const_VertexElementList^ VertexDeclaration::FindElementsBySource(unsigned short source)
{
	return Mogre::VertexDeclaration::VertexElementList::ByValue(static_cast<Ogre::VertexDeclaration*>(_native)->findElementsBySource(source))->ReadOnlyInstance;
}

size_t VertexDeclaration::GetVertexSize(unsigned short source)
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->getVertexSize(source);
}

Mogre::VertexDeclaration^ VertexDeclaration::Clone()
{
	return static_cast<Ogre::VertexDeclaration*>(_native)->clone();
}

bool VertexDeclaration::Equals(Object^ obj)
{
	VertexDeclaration^ clr = dynamic_cast<VertexDeclaration^>(obj);
	if (clr == CLR_NULL)
	{
		return false;
	}

	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (clr->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *(static_cast<Ogre::VertexDeclaration*>(_native)) == *(static_cast<Ogre::VertexDeclaration*>(clr->_native));
}

bool VertexDeclaration::Equals(VertexDeclaration^ obj)
{
	if (obj == CLR_NULL)
	{
		return false;
	}

	if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
	if (obj->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'obj' is null.");

	return *(static_cast<Ogre::VertexDeclaration*>(_native)) == *(static_cast<Ogre::VertexDeclaration*>(obj->_native));
}

bool VertexDeclaration::operator ==(VertexDeclaration^ obj1, VertexDeclaration^ obj2)
{
	if ((Object^)obj1 == (Object^)obj2) return true;
	if ((Object^)obj1 == nullptr || (Object^)obj2 == nullptr) return false;

	return obj1->Equals(obj2);
}

bool VertexDeclaration::operator !=(VertexDeclaration^ obj1, VertexDeclaration^ obj2)
{
	return !(obj1 == obj2);
}


bool VertexDeclaration::VertexElementLess(Mogre::VertexElement^ e1, Mogre::VertexElement^ e2)
{
	pin_ptr<Ogre::VertexElement> p_e1 = interior_ptr<Ogre::VertexElement>(&e1->data);
	pin_ptr<Ogre::VertexElement> p_e2 = interior_ptr<Ogre::VertexElement>(&e2->data);

	return Ogre::VertexDeclaration::vertexElementLess(*p_e1, *p_e2);
}


CPP_DECLARE_STLMAP(VertexBufferBinding::, VertexBufferBindingMap, unsigned short, Mogre::HardwareVertexBufferSharedPtr^, unsigned short, Ogre::HardwareVertexBufferSharedPtr);
CPP_DECLARE_STLMAP(VertexBufferBinding::, BindingIndexMap, Mogre::ushort, Mogre::ushort, Ogre::ushort, Ogre::ushort);

VertexBufferBinding::VertexBufferBinding()
{
	_createdByCLR = true;
	_native = new Ogre::VertexBufferBinding();
	ObjectTable::Add((IntPtr)_native, this, nullptr);
}

VertexBufferBinding::~VertexBufferBinding()
{
	this->!VertexBufferBinding();
}

VertexBufferBinding::!VertexBufferBinding()
{
	OnDisposing(this, nullptr);

	if (IsDisposed)
		return;

	if (_createdByCLR && _native != 0)
	{
		ObjectTable::Remove((IntPtr)_native);
		delete _native; _native = 0;
	}

	OnDisposed(this, nullptr);
}

size_t VertexBufferBinding::BufferCount::get()
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->getBufferCount();
}

bool VertexBufferBinding::HasGaps::get()
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->hasGaps();
}

unsigned short VertexBufferBinding::LastBoundIndex::get()
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->getLastBoundIndex();
}

unsigned short VertexBufferBinding::NextIndex::get()
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->getNextIndex();
}

void VertexBufferBinding::SetBinding(unsigned short index, Mogre::HardwareVertexBufferSharedPtr^ buffer)
{
	static_cast<Ogre::VertexBufferBinding*>(_native)->setBinding(index, (const Ogre::HardwareVertexBufferSharedPtr&)buffer);
}

void VertexBufferBinding::UnsetBinding(unsigned short index)
{
	static_cast<Ogre::VertexBufferBinding*>(_native)->unsetBinding(index);
}

void VertexBufferBinding::UnsetAllBindings()
{
	static_cast<Ogre::VertexBufferBinding*>(_native)->unsetAllBindings();
}

Mogre::VertexBufferBinding::Const_VertexBufferBindingMap^ VertexBufferBinding::GetBindings()
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->getBindings();
}

Mogre::HardwareVertexBufferSharedPtr^ VertexBufferBinding::GetBuffer(unsigned short index)
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->getBuffer(index);
}

bool VertexBufferBinding::IsBufferBound(unsigned short index)
{
	return static_cast<const Ogre::VertexBufferBinding*>(_native)->isBufferBound(index);
}

void VertexBufferBinding::CloseGaps(Mogre::VertexBufferBinding::BindingIndexMap^ bindingIndexMap)
{
	static_cast<Ogre::VertexBufferBinding*>(_native)->closeGaps(bindingIndexMap);
}

// ----------------- HardwareBufferManager -----------------

Mogre::HardwareVertexBufferSharedPtr^ HardwareBufferManager::CreateVertexBuffer(size_t vertexSize, size_t numVerts, Mogre::HardwareBuffer::Usage usage, bool useShadowBuffer)
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createVertexBuffer(vertexSize, numVerts, (Ogre::HardwareBuffer::Usage)usage, useShadowBuffer);
}
Mogre::HardwareVertexBufferSharedPtr^ HardwareBufferManager::CreateVertexBuffer(size_t vertexSize, size_t numVerts, Mogre::HardwareBuffer::Usage usage)
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createVertexBuffer(vertexSize, numVerts, (Ogre::HardwareBuffer::Usage)usage);
}

Mogre::HardwareIndexBufferSharedPtr^ HardwareBufferManager::CreateIndexBuffer(Mogre::HardwareIndexBuffer::IndexType itype, size_t numIndexes, Mogre::HardwareBuffer::Usage usage, bool useShadowBuffer)
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createIndexBuffer((Ogre::HardwareIndexBuffer::IndexType)itype, numIndexes, (Ogre::HardwareBuffer::Usage)usage, useShadowBuffer);
}

Mogre::HardwareIndexBufferSharedPtr^ HardwareBufferManager::CreateIndexBuffer(Mogre::HardwareIndexBuffer::IndexType itype, size_t numIndexes, Mogre::HardwareBuffer::Usage usage)
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createIndexBuffer((Ogre::HardwareIndexBuffer::IndexType)itype, numIndexes, (Ogre::HardwareBuffer::Usage)usage);
}

Mogre::VertexDeclaration^ HardwareBufferManager::CreateVertexDeclaration()
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createVertexDeclaration();
}

void HardwareBufferManager::DestroyVertexDeclaration(Mogre::VertexDeclaration^ decl)
{
	static_cast<Ogre::HardwareBufferManager*>(_native)->destroyVertexDeclaration(decl);
}

Mogre::VertexBufferBinding^ HardwareBufferManager::CreateVertexBufferBinding()
{
	return static_cast<Ogre::HardwareBufferManager*>(_native)->createVertexBufferBinding();
}

void HardwareBufferManager::DestroyVertexBufferBinding(Mogre::VertexBufferBinding^ binding)
{
	static_cast<Ogre::HardwareBufferManager*>(_native)->destroyVertexBufferBinding(binding);
}

void HardwareBufferManager::RegisterVertexBufferSourceAndCopy(Mogre::HardwareVertexBufferSharedPtr^ sourceBuffer, Mogre::HardwareVertexBufferSharedPtr^ copy)
{
	static_cast<Ogre::HardwareBufferManager*>(_native)->registerVertexBufferSourceAndCopy((const Ogre::HardwareVertexBufferSharedPtr&)sourceBuffer, (const Ogre::HardwareVertexBufferSharedPtr&)copy);
}

void HardwareBufferManager::ReleaseVertexBufferCopy(Mogre::HardwareVertexBufferSharedPtr^ bufferCopy)
{
	static_cast<Ogre::HardwareBufferManager*>(_native)->releaseVertexBufferCopy((const Ogre::HardwareVertexBufferSharedPtr&)bufferCopy);
}

void HardwareBufferManager::TouchVertexBufferCopy(Mogre::HardwareVertexBufferSharedPtr^ bufferCopy)
{
	static_cast<Ogre::HardwareBufferManager*>(_native)->touchVertexBufferCopy((const Ogre::HardwareVertexBufferSharedPtr&)bufferCopy);
}