#pragma once

#include "OgreHardwareBuffer.h"
#include "MogreCommon.h"
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
}