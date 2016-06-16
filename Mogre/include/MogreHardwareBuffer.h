#pragma once

#include "OgreHardwareBuffer.h"
#include "OgreHardwareIndexBuffer.h"
#include "OgreHardwareVertexBuffer.h"
#include "OgreHardwarePixelBuffer.h"
#include "OgreHardwareBufferManager.h"
#include "MogreCommon.h"
#include "MogrePixelFormat.h"
#include "MogreTextureManager.h"
#include "Marshalling.h"
#include "STLContainerWrappers.h"

namespace Mogre
{
	ref class RenderTexture;

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
		HardwareBuffer(IntPtr ptr) : _native((Ogre::HardwareBuffer*)ptr.ToPointer())
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

		HardwareIndexBuffer(IntPtr ptr) : HardwareBuffer(ptr)
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
		static operator HardwareIndexBufferSharedPtr ^ (const Ogre::HardwareIndexBufferSharedPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew HardwareIndexBufferSharedPtr(*(new Ogre::HardwareIndexBufferSharedPtr(ptr)));
		}

		static operator Ogre::HardwareIndexBufferSharedPtr& (HardwareIndexBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::HardwareIndexBufferSharedPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::HardwareIndexBufferSharedPtr* (HardwareIndexBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

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

		HardwareVertexBuffer(IntPtr ptr) : HardwareBuffer(ptr)
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
		static operator HardwareVertexBufferSharedPtr ^ (const Ogre::HardwareVertexBufferSharedPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew HardwareVertexBufferSharedPtr(*(new Ogre::HardwareVertexBufferSharedPtr(ptr)));
		}

		static operator Ogre::HardwareVertexBufferSharedPtr& (HardwareVertexBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::HardwareVertexBufferSharedPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::HardwareVertexBufferSharedPtr* (HardwareVertexBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

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

		HardwarePixelBuffer(IntPtr ptr) : HardwareBuffer(ptr)
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

		Mogre::RenderTexture^ GetRenderTarget(size_t slice);
		Mogre::RenderTexture^ GetRenderTarget();

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
		static operator HardwarePixelBufferSharedPtr ^ (const Ogre::HardwarePixelBufferSharedPtr& ptr)
		{
			if (ptr.isNull()) return nullptr;
			return gcnew HardwarePixelBufferSharedPtr(*(new Ogre::HardwarePixelBufferSharedPtr(ptr)));
		}

		static operator Ogre::HardwarePixelBufferSharedPtr& (HardwarePixelBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return Ogre::HardwarePixelBufferSharedPtr();
			return *(t->_sharedPtr);
		}

		static operator Ogre::HardwarePixelBufferSharedPtr* (HardwarePixelBufferSharedPtr^ t)
		{
			if (CLR_NULL == t) return nullptr;
			return t->_sharedPtr;
		}

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

	public ref class VertexElement
	{
	protected public:
		// A value class to map the fields of the Ogre::VertexElement
		[StructLayout(LayoutKind::Sequential)]
		value struct Data
		{
			/// The source vertex buffer, as bound to an index using VertexBufferBinding
			unsigned short mSource;
			/// The offset in the buffer that this element starts at
			size_t mOffset;
			/// The type of element
			VertexElementType mType;
			/// The meaning of the element
			VertexElementSemantic mSemantic;
			/// Index of the item, only applicable for some elements like texture coords
			unsigned short mIndex;
		};

	public:
		static operator VertexElement ^ (const Ogre::VertexElement& elem)
		{
			VertexElement^ clrelem = gcnew VertexElement;
			pin_ptr<Ogre::VertexElement> ptr = interior_ptr<Ogre::VertexElement>(&clrelem->data);
			*ptr = elem;
			return clrelem;
		}

		static operator VertexElement ^ (const Ogre::VertexElement* pelem)
		{
			if (pelem)
				return (VertexElement^)*pelem;
			else
				return nullptr;
		}

		static operator Ogre::VertexElement(VertexElement^ elem)
		{
			pin_ptr<Ogre::VertexElement> ptr = interior_ptr<Ogre::VertexElement>(&elem->data);
			return *ptr;
		}

	internal:
		VertexElement()
		{
		}

	protected public:
		Data data;

	public:
		/// Constructor, should not be called directly, call VertexDeclaration::addElement
		VertexElement(unsigned short source, size_t offset, VertexElementType theType,
			VertexElementSemantic semantic, unsigned short index);
		/// Gets the vertex buffer index from where this element draws it's values
		property unsigned short Source
		{
			unsigned short get() { return data.mSource; }
		}
		/// Gets the offset into the buffer where this element starts
		property size_t Offset
		{
			size_t get() { return data.mOffset; }
		}
		/// Gets the data format of this element
		property VertexElementType Type
		{
			VertexElementType get() { return data.mType; }
		}
		/// Gets the meaning of this element
		property VertexElementSemantic Semantic
		{
			VertexElementSemantic get() { return data.mSemantic; }
		}
		/// Gets the index of this element, only applicable for repeating elements
		property unsigned short Index
		{
			unsigned short get() { return data.mIndex; }
		}
		/// Gets the size of this element in bytes
		property size_t Size
		{
			size_t get();
		}
		/// Utility method for helping to calculate offsets
		static size_t GetTypeSize(VertexElementType etype);
		/// Utility method which returns the count of values in a given type
		static unsigned short GetTypeCount(VertexElementType etype);
		/** Simple converter function which will turn a single-value type into a
		multi-value type based on a parameter.
		*/
		static VertexElementType MultiplyTypeCount(VertexElementType baseType, unsigned short count);
		/** Simple converter function which will a type into it's single-value
		equivalent - makes switches on type easier.
		*/
		static VertexElementType GetBaseType(VertexElementType multiType);

		/** Utility method for converting colour from
		one packed 32-bit colour type to another.
		@param srcType The source type
		@param dstType The destination type
		@param ptr Read / write value to change
		*/
		static void ConvertColourValue(VertexElementType srcType,
			VertexElementType dstType, Ogre::uint32% ptr);

		/** Utility method for converting colour to
		a packed 32-bit colour type.
		@param src source colour
		@param dst The destination type
		*/
		static Ogre::uint32 ConvertColourValue(ColourValue src,
			VertexElementType dst);

		/** Utility method to get the most appropriate packed colour vertex element format. */
		property VertexElementType BestColourVertexElementType
		{
			static VertexElementType get();
		}

		virtual bool Equals(Object^ obj) override
		{
			VertexElement^ clr = dynamic_cast<VertexElement^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}

			return (this == clr);
		}

		bool Equals(VertexElement^ obj)
		{
			return (this == obj);
		}

		inline static bool operator== (VertexElement^ lhs, VertexElement^ rhs)
		{
			if ((Object^)lhs == (Object^)rhs) return true;
			if ((Object^)lhs == nullptr || (Object^)rhs == nullptr) return false;

			if (lhs->data.mType != rhs->data.mType ||
				lhs->data.mIndex != rhs->data.mIndex ||
				lhs->data.mOffset != rhs->data.mOffset ||
				lhs->data.mSemantic != rhs->data.mSemantic ||
				lhs->data.mSource != rhs->data.mSource)
				return false;
			else
				return true;

		}
		inline static bool operator!= (VertexElement^ lhs, VertexElement^ rhs)
		{
			return !(lhs == rhs);
		}

		/** Adjusts a pointer to the base of a vertex to point at this element.
		@remarks
		This variant is for void pointers, passed as a parameter because we can't
		rely on covariant return types.
		@param pBase Pointer to the start of a vertex in this buffer.
		@param pElem Pointer to a pointer which will be set to the start of this element.
		*/
		inline void BaseVertexPointerToElement(void* pBase, void** pElem)
		{
			// The only way we can do this is to cast to char* in order to use byte offset
			// then cast back to void*.
			*pElem = static_cast<void*>(
				static_cast<unsigned char*>(pBase) + data.mOffset);
		}
		/** Adjusts a pointer to the base of a vertex to point at this element.
		@remarks
		This variant is for float pointers, passed as a parameter because we can't
		rely on covariant return types.
		@param pBase Pointer to the start of a vertex in this buffer.
		@param pElem Pointer to a pointer which will be set to the start of this element.
		*/
		inline void BaseVertexPointerToElement(void* pBase, float** pElem)
		{
			// The only way we can do this is to cast to char* in order to use byte offset
			// then cast back to float*. However we have to go via void* because casting
			// directly is not allowed
			*pElem = static_cast<float*>(
				static_cast<void*>(
					static_cast<unsigned char*>(pBase) + data.mOffset));
		}

		/** Adjusts a pointer to the base of a vertex to point at this element.
		@remarks
		This variant is for RGBA pointers, passed as a parameter because we can't
		rely on covariant return types.
		@param pBase Pointer to the start of a vertex in this buffer.
		@param pElem Pointer to a pointer which will be set to the start of this element.
		*/
		inline void BaseVertexPointerToElement(void* pBase, Ogre::RGBA** pElem)
		{
			*pElem = static_cast<Ogre::RGBA*>(
				static_cast<void*>(
					static_cast<unsigned char*>(pBase) + data.mOffset));
		}
		/** Adjusts a pointer to the base of a vertex to point at this element.
		@remarks
		This variant is for char pointers, passed as a parameter because we can't
		rely on covariant return types.
		@param pBase Pointer to the start of a vertex in this buffer.
		@param pElem Pointer to a pointer which will be set to the start of this element.
		*/
		inline void BaseVertexPointerToElement(void* pBase, unsigned char** pElem)
		{
			*pElem = static_cast<unsigned char*>(pBase) + data.mOffset;
		}

		/** Adjusts a pointer to the base of a vertex to point at this element.
		@remarks
		This variant is for ushort pointers, passed as a parameter because we can't
		rely on covariant return types.
		@param pBase Pointer to the start of a vertex in this buffer.
		@param pElem Pointer to a pointer which will be set to the start of this element.
		*/
		inline void BaseVertexPointerToElement(void* pBase, unsigned short** pElem)
		{
			*pElem = static_cast<unsigned short*>(pBase) + data.mOffset;
		}
	};

	//################################################################
	//VertexDeclaration
	//################################################################

	public ref class VertexDeclaration : public IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class VertexElementList;

		INC_DECLARE_STLLIST_READONLY(VertexElementList, Mogre::VertexElement^, Ogre::VertexElement, public:, private:);

	internal:
		Ogre::VertexDeclaration* _native;
		bool _createdByCLR;

		//Internal Declarations
	public protected:
		VertexDeclaration(Ogre::VertexDeclaration* obj) : _native(obj)
		{
		}

		VertexDeclaration(IntPtr ptr) : _native((Ogre::VertexDeclaration*)ptr.ToPointer())
		{

		}

		~VertexDeclaration();
		!VertexDeclaration();

	public:
		VertexDeclaration();

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property size_t ElementCount
		{
		public:
			size_t get();
		}

		property unsigned short MaxSource
		{
		public:
			unsigned short get();
		}

		Mogre::VertexDeclaration::Const_VertexElementList^ GetElements();

		Mogre::VertexElement^ GetElement(unsigned short index);

		void Sort();

		void CloseGapsInSource();

		Mogre::VertexDeclaration^ GetAutoOrganisedDeclaration(bool skeletalAnimation, bool vertexAnimation, bool vertexAnimationNormals);

		Mogre::VertexElement^ AddElement(unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index);
		Mogre::VertexElement^ AddElement(unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic);

		Mogre::VertexElement^ InsertElement(unsigned short atPosition, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index);
		Mogre::VertexElement^ InsertElement(unsigned short atPosition, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic);

		void RemoveElement(unsigned short elem_index);

		void RemoveElement(Mogre::VertexElementSemantic semantic, unsigned short index);
		void RemoveElement(Mogre::VertexElementSemantic semantic);

		void RemoveAllElements();

		void ModifyElement(unsigned short elem_index, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic, unsigned short index);
		void ModifyElement(unsigned short elem_index, unsigned short source, size_t offset, Mogre::VertexElementType theType, Mogre::VertexElementSemantic semantic);

		Mogre::VertexElement^ FindElementBySemantic(Mogre::VertexElementSemantic sem, unsigned short index);
		Mogre::VertexElement^ FindElementBySemantic(Mogre::VertexElementSemantic sem);

		Mogre::VertexDeclaration::Const_VertexElementList^ FindElementsBySource(unsigned short source);

		size_t GetVertexSize(unsigned short source);

		Mogre::VertexDeclaration^ Clone();

		virtual bool Equals(Object^ obj) override;
		bool Equals(VertexDeclaration^ obj);
		static bool operator == (VertexDeclaration^ obj1, VertexDeclaration^ obj2);
		static bool operator != (VertexDeclaration^ obj1, VertexDeclaration^ obj2);

		void CopyTo(VertexDeclaration^ dest)
		{
			if (_native == NULL) throw gcnew Exception("The underlying native object for the caller is null.");
			if (dest->_native == NULL) throw gcnew ArgumentException("The underlying native object for parameter 'dest' is null.");

			*(dest->_native) = *_native;
		}

		static bool VertexElementLess(Mogre::VertexElement^ e1, Mogre::VertexElement^ e2);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(VertexDeclaration);
	};

	//################################################################
	//VertexBufferBinding
	//################################################################

	public ref class VertexBufferBinding : public IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class VertexBufferBindingMap;
		ref class BindingIndexMap;

		INC_DECLARE_STLMAP(VertexBufferBindingMap, unsigned short, Mogre::HardwareVertexBufferSharedPtr^, unsigned short, Ogre::HardwareVertexBufferSharedPtr, public:, private:);
		INC_DECLARE_STLMAP(BindingIndexMap, Mogre::ushort, Mogre::ushort, Ogre::ushort, Ogre::ushort, public:, private:);

	internal:
		Ogre::VertexBufferBinding* _native;
		bool _createdByCLR;

	public protected:
		VertexBufferBinding(IntPtr ptr) : _native((Ogre::VertexBufferBinding*)ptr.ToPointer())
		{

		}

		VertexBufferBinding(Ogre::VertexBufferBinding* obj) : _native(obj)
		{
		}

		~VertexBufferBinding();
		!VertexBufferBinding();

	public:
		VertexBufferBinding();

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		property size_t BufferCount
		{
		public:
			size_t get();
		}

		property bool HasGaps
		{
		public:
			bool get();
		}

		property unsigned short LastBoundIndex
		{
		public:
			unsigned short get();
		}

		property unsigned short NextIndex
		{
		public:
			unsigned short get();
		}

		void SetBinding(unsigned short index, Mogre::HardwareVertexBufferSharedPtr^ buffer);

		void UnsetBinding(unsigned short index);

		void UnsetAllBindings();

		Mogre::VertexBufferBinding::Const_VertexBufferBindingMap^ GetBindings();

		Mogre::HardwareVertexBufferSharedPtr^ GetBuffer(unsigned short index);

		bool IsBufferBound(unsigned short index);

		void CloseGaps(Mogre::VertexBufferBinding::BindingIndexMap^ bindingIndexMap);

		DEFINE_MANAGED_NATIVE_CONVERSIONS(VertexBufferBinding);
	};

	public ref class HardwareBufferManager
	{
	public:
		enum class BufferLicenseType
		{
			BLT_MANUAL_RELEASE = Ogre::HardwareBufferManager::BLT_MANUAL_RELEASE,
			BLT_AUTOMATIC_RELEASE = Ogre::HardwareBufferManager::BLT_AUTOMATIC_RELEASE
		};

	private protected:
		static HardwareBufferManager^ _singleton;
		Ogre::HardwareBufferManager* _native;
		bool _createdByCLR;

		//Internal Declarations
	public protected:
		HardwareBufferManager(Ogre::HardwareBufferManager* obj) : _native(obj)
		{
		}

	public:

		static property HardwareBufferManager^ Singleton
		{
			HardwareBufferManager^ get()
			{
				if (_singleton == CLR_NULL)
				{
					Ogre::HardwareBufferManager* ptr = Ogre::HardwareBufferManager::getSingletonPtr();
					if (ptr) _singleton = gcnew HardwareBufferManager(ptr);
				}
				return _singleton;
			}
		}

		Mogre::HardwareVertexBufferSharedPtr^ CreateVertexBuffer(size_t vertexSize, size_t numVerts, Mogre::HardwareBuffer::Usage usage, bool useShadowBuffer);
		Mogre::HardwareVertexBufferSharedPtr^ CreateVertexBuffer(size_t vertexSize, size_t numVerts, Mogre::HardwareBuffer::Usage usage);

		Mogre::HardwareIndexBufferSharedPtr^ CreateIndexBuffer(Mogre::HardwareIndexBuffer::IndexType itype, size_t numIndexes, Mogre::HardwareBuffer::Usage usage, bool useShadowBuffer);
		Mogre::HardwareIndexBufferSharedPtr^ CreateIndexBuffer(Mogre::HardwareIndexBuffer::IndexType itype, size_t numIndexes, Mogre::HardwareBuffer::Usage usage);

		Mogre::VertexDeclaration^ CreateVertexDeclaration();

		void DestroyVertexDeclaration(Mogre::VertexDeclaration^ decl);

		Mogre::VertexBufferBinding^ CreateVertexBufferBinding();

		void DestroyVertexBufferBinding(Mogre::VertexBufferBinding^ binding);

		void RegisterVertexBufferSourceAndCopy(Mogre::HardwareVertexBufferSharedPtr^ sourceBuffer, Mogre::HardwareVertexBufferSharedPtr^ copy);

		void ReleaseVertexBufferCopy(Mogre::HardwareVertexBufferSharedPtr^ bufferCopy);

		void TouchVertexBufferCopy(Mogre::HardwareVertexBufferSharedPtr^ bufferCopy);
	};
}