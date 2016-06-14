#pragma once

#include "OgreVertexIndexData.h"
#include "MogreHardwareBuffer.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class BufferUsageList;
	ref class Const_BufferUsageList;

	public ref class VertexData : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		value class HardwareAnimationData_NativePtr
		{
		private protected:
			Ogre::VertexData::HardwareAnimationData* _native;

			//Public Declarations
		public:
			property Mogre::Real parametric
			{
			public:
				Mogre::Real get();
			public:
				void set(Mogre::Real value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(VertexData::HardwareAnimationData_NativePtr, Ogre::VertexData::HardwareAnimationData);


			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static HardwareAnimationData_NativePtr Create();

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}
		};

		INC_DECLARE_STLVECTOR(HardwareAnimationDataList, Mogre::VertexData::HardwareAnimationData_NativePtr, Ogre::VertexData::HardwareAnimationData, public:, private:);

	public protected:
		Mogre::VertexData::HardwareAnimationDataList^ _hwAnimationDataList;
		Mogre::VertexBufferBinding^ _vertexBufferBinding;
		Mogre::VertexDeclaration^ _vertexDeclaration;

		VertexData(Ogre::VertexData* obj) 
			: _native(obj)
			, _createdByCLR(false)
			, _vertexBufferBinding(nullptr)
			, _vertexDeclaration(nullptr)
		{
		}

		VertexData(intptr_t ptr) 
			: _native((Ogre::VertexData*)ptr)
			, _createdByCLR(false)
			, _vertexBufferBinding(nullptr)
			, _vertexDeclaration(nullptr)
		{
		}

		Ogre::VertexData* _native;
		bool _createdByCLR;

	public:
		~VertexData();
	protected:
		!VertexData();
	public:
		VertexData();

		property Mogre::VertexDeclaration^ vertexDeclaration
		{
		public:
			Mogre::VertexDeclaration^ get();
		public:
			void set(Mogre::VertexDeclaration^ value);
		}

		property Mogre::VertexBufferBinding^ vertexBufferBinding
		{
		public:
			Mogre::VertexBufferBinding^ get();
		public:
			void set(Mogre::VertexBufferBinding^ value);
		}

		property size_t vertexStart
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property size_t vertexCount
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property Mogre::VertexData::HardwareAnimationDataList^ hwAnimationDataList
		{
		public:
			Mogre::VertexData::HardwareAnimationDataList^ get();
		}

		property size_t hwAnimDataItemsUsed
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property Mogre::HardwareVertexBufferSharedPtr^ hardwareShadowVolWBuffer
		{
		public:
			Mogre::HardwareVertexBufferSharedPtr^ get();
		public:
			void set(Mogre::HardwareVertexBufferSharedPtr^ value);
		}

		Mogre::VertexData^ Clone(bool copyData);
		Mogre::VertexData^ Clone();

		void PrepareForShadowVolume();

		void ReorganiseBuffers(Mogre::VertexDeclaration^ newDeclaration, Mogre::Const_BufferUsageList^ bufferUsage);

		void ReorganiseBuffers(Mogre::VertexDeclaration^ newDeclaration);

		void CloseGapsInBindings();

		void RemoveUnusedBuffers();

		void ConvertPackedColour(Mogre::VertexElementType srcType, Mogre::VertexElementType destType);

		void AllocateHardwareAnimationElements(Ogre::ushort count, bool animateNormals);

		property bool IsDisposed
		{
			virtual bool get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(VertexData);
	};

	public ref class IndexData : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public protected:
		IndexData(Ogre::IndexData* obj) : _native(obj), _createdByCLR(false)
		{
		}

		IndexData(intptr_t ptr) : _native((Ogre::IndexData*)ptr), _createdByCLR(false)
		{
		}

		Ogre::IndexData* _native;
		bool _createdByCLR;

	public:
		~IndexData();
	protected:
		!IndexData();

	public:
		IndexData();

		property Mogre::HardwareIndexBufferSharedPtr^ indexBuffer
		{
		public:
			Mogre::HardwareIndexBufferSharedPtr^ get();
		public:
			void set(Mogre::HardwareIndexBufferSharedPtr^ value);
		}

		property size_t indexStart
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property size_t indexCount
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		Mogre::IndexData^ Clone(bool copyData);
		Mogre::IndexData^ Clone();

		void OptimiseVertexCacheTriList();

		property bool IsDisposed
		{
			virtual bool get();
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(IndexData);
	};

	INC_DECLARE_STLVECTOR(BufferUsageList, Mogre::HardwareBuffer::Usage, Ogre::HardwareBuffer::Usage, public, private);
}