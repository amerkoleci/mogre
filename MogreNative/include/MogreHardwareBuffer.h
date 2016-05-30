#pragma once

#include "OgreHardwareBuffer.h"
#include "OgreHardwareIndexBuffer.h"
#include "OgreHardwareVertexBuffer.h"
#include "OgreHardwarePixelBuffer.h"
#include "MogreCommon.h"
#include "MogrePixelFormat.h"
#include "MogreTextureManager.h"
#include "Marshalling.h"

namespace Mogre
{
	public ref class HardwareBuffer : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		enum class Usage
		{
			HBU_STATIC = Ogre::HardwareBuffer::HBU_STATIC,
			HBU_DYNAMIC = Ogre::HardwareBuffer::HBU_DYNAMIC,
			HBU_WRITE_ONLY = Ogre::HardwareBuffer::HBU_WRITE_ONLY,
			HBU_DISCARDABLE = Ogre::HardwareBuffer::HBU_DISCARDABLE,
			HBU_STATIC_WRITE_ONLY = Ogre::HardwareBuffer::HBU_STATIC_WRITE_ONLY,
			HBU_DYNAMIC_WRITE_ONLY = Ogre::HardwareBuffer::HBU_DYNAMIC_WRITE_ONLY,
			HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE = Ogre::HardwareBuffer::HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE
		};

		enum class LockOptions
		{
			HBL_NORMAL = Ogre::HardwareBuffer::HBL_NORMAL,
			HBL_DISCARD = Ogre::HardwareBuffer::HBL_DISCARD,
			HBL_READ_ONLY = Ogre::HardwareBuffer::HBL_READ_ONLY,
			HBL_NO_OVERWRITE = Ogre::HardwareBuffer::HBL_NO_OVERWRITE
		};

	internal:
		Ogre::HardwareBuffer* _native;
		bool _createdByCLR;

	public protected:
		HardwareBuffer(intptr_t ptr) : _native((Ogre::HardwareBuffer*)ptr)
		{

		}

		HardwareBuffer(Ogre::HardwareBuffer* ptr) : _native(ptr)
		{

		}

		~HardwareBuffer();
		!HardwareBuffer();

	public:
		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property bool HasShadowBuffer
		{
		public:
			bool get();
		}

		property bool IsLocked
		{
		public:
			bool get();
		}

		property bool IsSystemMemory
		{
		public:
			bool get();
		}

		property size_t SizeInBytes
		{
		public:
			size_t get();
		}

		void* Lock(size_t offset, size_t length, Mogre::HardwareBuffer::LockOptions options);

		void* Lock(Mogre::HardwareBuffer::LockOptions options);

		void Unlock();

		void ReadData(size_t offset, size_t length, void* pDest);

		void WriteData(size_t offset, size_t length, const void* pSource, bool discardWholeBuffer);
		void WriteData(size_t offset, size_t length, const void* pSource);

		void CopyData(Mogre::HardwareBuffer^ srcBuffer, size_t srcOffset, size_t dstOffset, size_t length, bool discardWholeBuffer);
		void CopyData(Mogre::HardwareBuffer^ srcBuffer, size_t srcOffset, size_t dstOffset, size_t length);

		void _updateFromShadow();

		Mogre::HardwareBuffer::Usage GetUsage();

		void SuppressHardwareUpdate(bool suppress);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(HardwareBuffer);

	internal:
		property Ogre::HardwareBuffer* UnmanagedPointer
		{
			Ogre::HardwareBuffer* get();
		}
	};

	public ref class HardwareIndexBuffer : public HardwareBuffer
	{
	public:
		enum class IndexType
		{
			IT_16BIT = Ogre::HardwareIndexBuffer::IT_16BIT,
			IT_32BIT = Ogre::HardwareIndexBuffer::IT_32BIT
		};

	public protected:
		HardwareIndexBuffer(Ogre::HardwareIndexBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwareIndexBuffer(Ogre::HardwareBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwareIndexBuffer(intptr_t ptr) : HardwareBuffer(ptr)
		{

		}

	public:
		property size_t IndexSize
		{
		public:
			size_t get();
		}

		property size_t NumIndexes
		{
		public:
			size_t get();
		}

		property Mogre::HardwareIndexBuffer::IndexType Type
		{
		public:
			Mogre::HardwareIndexBuffer::IndexType get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(HardwareIndexBuffer);
	};

	public ref class HardwareIndexBufferSharedPtr : public HardwareIndexBuffer
	{
	public protected:
		Ogre::HardwareIndexBufferSharedPtr* _sharedPtr;

		HardwareIndexBufferSharedPtr(Ogre::HardwareIndexBufferSharedPtr& sharedPtr) : HardwareIndexBuffer(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::HardwareIndexBufferSharedPtr(sharedPtr);
		}

		!HardwareIndexBufferSharedPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~HardwareIndexBufferSharedPtr()
		{
			this->!HardwareIndexBufferSharedPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(HardwareIndexBufferSharedPtr);

		HardwareIndexBufferSharedPtr(HardwareIndexBuffer^ obj) : HardwareIndexBuffer(obj->_native)
		{
			_sharedPtr = new Ogre::HardwareIndexBufferSharedPtr(static_cast<Ogre::HardwareIndexBuffer*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			HardwareIndexBufferSharedPtr^ clr = dynamic_cast<HardwareIndexBufferSharedPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(HardwareIndexBufferSharedPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (HardwareIndexBufferSharedPtr^ val1, HardwareIndexBufferSharedPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (HardwareIndexBufferSharedPtr^ val1, HardwareIndexBufferSharedPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property HardwareIndexBuffer^ Target
		{
			HardwareIndexBuffer^ get()
			{
				return static_cast<Ogre::HardwareIndexBuffer*>(_native);
			}
		}
	};

	public enum class VertexElementType
	{
		VET_FLOAT1 = Ogre::VET_FLOAT1,
		VET_FLOAT2 = Ogre::VET_FLOAT2,
		VET_FLOAT3 = Ogre::VET_FLOAT3,
		VET_FLOAT4 = Ogre::VET_FLOAT4,
		VET_COLOUR = Ogre::VET_COLOUR,
		VET_SHORT1 = Ogre::VET_SHORT1,
		VET_SHORT2 = Ogre::VET_SHORT2,
		VET_SHORT3 = Ogre::VET_SHORT3,
		VET_SHORT4 = Ogre::VET_SHORT4,
		VET_UBYTE4 = Ogre::VET_UBYTE4,
		VET_COLOUR_ARGB = Ogre::VET_COLOUR_ARGB,
		VET_COLOUR_ABGR = Ogre::VET_COLOUR_ABGR
	};

	public enum class VertexElementSemantic
	{
		VES_POSITION = Ogre::VES_POSITION,
		VES_BLEND_WEIGHTS = Ogre::VES_BLEND_WEIGHTS,
		VES_BLEND_INDICES = Ogre::VES_BLEND_INDICES,
		VES_NORMAL = Ogre::VES_NORMAL,
		VES_DIFFUSE = Ogre::VES_DIFFUSE,
		VES_SPECULAR = Ogre::VES_SPECULAR,
		VES_TEXTURE_COORDINATES = Ogre::VES_TEXTURE_COORDINATES,
		VES_BINORMAL = Ogre::VES_BINORMAL,
		VES_TANGENT = Ogre::VES_TANGENT
	};

	//################################################################
	//HardwareVertexBuffer
	//################################################################

	public ref class HardwareVertexBuffer : public HardwareBuffer
	{
	public protected:
		HardwareVertexBuffer(Ogre::HardwareVertexBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwareVertexBuffer(Ogre::HardwareBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwareVertexBuffer(intptr_t ptr) : HardwareBuffer(ptr)
		{

		}

	public:
		property size_t NumVertices
		{
		public:
			size_t get();
		}

		property size_t VertexSize
		{
		public:
			size_t get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(HardwareVertexBuffer);
	};

	public ref class HardwareVertexBufferSharedPtr : public HardwareVertexBuffer
	{
	public protected:
		Ogre::HardwareVertexBufferSharedPtr* _sharedPtr;

		HardwareVertexBufferSharedPtr(Ogre::HardwareVertexBufferSharedPtr& sharedPtr) : HardwareVertexBuffer(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::HardwareVertexBufferSharedPtr(sharedPtr);
		}

		!HardwareVertexBufferSharedPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~HardwareVertexBufferSharedPtr()
		{
			this->!HardwareVertexBufferSharedPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(HardwareVertexBufferSharedPtr);

		HardwareVertexBufferSharedPtr(HardwareVertexBuffer^ obj) : HardwareVertexBuffer(obj->_native)
		{
			_sharedPtr = new Ogre::HardwareVertexBufferSharedPtr(static_cast<Ogre::HardwareVertexBuffer*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			HardwareVertexBufferSharedPtr^ clr = dynamic_cast<HardwareVertexBufferSharedPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(HardwareVertexBufferSharedPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (HardwareVertexBufferSharedPtr^ val1, HardwareVertexBufferSharedPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (HardwareVertexBufferSharedPtr^ val1, HardwareVertexBufferSharedPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property HardwareVertexBuffer^ Target
		{
			HardwareVertexBuffer^ get()
			{
				return static_cast<Ogre::HardwareVertexBuffer*>(_native);
			}
		}
	};

	ref class HardwarePixelBufferSharedPtr;

	public ref class HardwarePixelBuffer : public HardwareBuffer
	{
	public protected:
		HardwarePixelBuffer(Ogre::HardwarePixelBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwarePixelBuffer(Ogre::HardwareBuffer* obj) : HardwareBuffer(obj)
		{
		}

		HardwarePixelBuffer(intptr_t ptr) : HardwareBuffer(ptr)
		{

		}


	public:
		property Mogre::PixelBox CurrentLock
		{
		public:
			Mogre::PixelBox get();
		}

		property size_t Depth
		{
		public:
			size_t get();
		}

		property Mogre::PixelFormat Format
		{
		public:
			Mogre::PixelFormat get();
		}

		property size_t Height
		{
		public:
			size_t get();
		}

		property size_t Width
		{
		public:
			size_t get();
		}

		Mogre::PixelBox Lock(Mogre::Image::Box lockBox, Mogre::HardwareBuffer::LockOptions options);

		void* Lock(size_t offset, size_t length, Mogre::HardwareBuffer::LockOptions options);

		void ReadData(size_t offset, size_t length, void* pDest);

		void WriteData(size_t offset, size_t length, const void* pSource, bool discardWholeBuffer);
		void WriteData(size_t offset, size_t length, const void* pSource);

		void Blit(Mogre::HardwarePixelBufferSharedPtr^ src, Mogre::Image::Box srcBox, Mogre::Image::Box dstBox);

		void Blit(Mogre::HardwarePixelBufferSharedPtr^ src);

		void BlitFromMemory(Mogre::PixelBox src, Mogre::Image::Box dstBox);

		void BlitFromMemory(Mogre::PixelBox src);

		void BlitToMemory(Mogre::Image::Box srcBox, Mogre::PixelBox dst);

		void BlitToMemory(Mogre::PixelBox dst);

		//Mogre::RenderTexture^ GetRenderTarget(size_t slice);
		//Mogre::RenderTexture^ GetRenderTarget();

		DEFINE_MANAGED_NATIVE_CONVERSIONS(HardwarePixelBuffer);
	};

	public ref class HardwarePixelBufferSharedPtr : public HardwarePixelBuffer
	{
	public protected:
		Ogre::HardwarePixelBufferSharedPtr* _sharedPtr;

		HardwarePixelBufferSharedPtr(Ogre::HardwarePixelBufferSharedPtr& sharedPtr) : HardwarePixelBuffer(sharedPtr.getPointer())
		{
			_sharedPtr = new Ogre::HardwarePixelBufferSharedPtr(sharedPtr);
		}

		!HardwarePixelBufferSharedPtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}

		~HardwarePixelBufferSharedPtr()
		{
			this->!HardwarePixelBufferSharedPtr();
		}

	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR(HardwarePixelBufferSharedPtr);

		HardwarePixelBufferSharedPtr(HardwarePixelBuffer^ obj) : HardwarePixelBuffer(obj->_native)
		{
			_sharedPtr = new Ogre::HardwarePixelBufferSharedPtr(static_cast<Ogre::HardwarePixelBuffer*>(obj->_native));
		}

		virtual bool Equals(Object^ obj) override
		{
			HardwarePixelBufferSharedPtr^ clr = dynamic_cast<HardwarePixelBufferSharedPtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (_native == clr->_native);
		}
		bool Equals(HardwarePixelBufferSharedPtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}

			return (_native == obj->_native);
		}

		static bool operator == (HardwarePixelBufferSharedPtr^ val1, HardwarePixelBufferSharedPtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}

		static bool operator != (HardwarePixelBufferSharedPtr^ val1, HardwarePixelBufferSharedPtr^ val2)
		{
			return !(val1 == val2);
		}

		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>(_native);
		}

		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}

		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}

		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}

		property HardwarePixelBuffer^ Target
		{
			HardwarePixelBuffer^ get()
			{
				return static_cast<Ogre::HardwarePixelBuffer*>(_native);
			}
		}
	};
}