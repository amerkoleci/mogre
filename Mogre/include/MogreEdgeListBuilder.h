#pragma once

#include "OgreEdgeListBuilder.h"
#include "MogreCommon.h"
#include "STLContainerWrappers.h"
#include "IteratorWrapper.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class Log;
	ref class VertexData;
	ref class HardwareVertexBufferSharedPtr;

	public ref class EdgeData : IMogreDisposable
	{
	public:
		/// <summary>Raised before any disposing is performed.</summary>
		virtual event EventHandler^ OnDisposing;
		/// <summary>Raised once all disposing is performed.</summary>
		virtual event EventHandler^ OnDisposed;

	public:
		ref class TriangleFaceNormalList;
		ref class TriangleLightFacingList;
		ref class TriangleList;
		ref class EdgeList;
		ref class EdgeGroupList;

		//################################################################
		//Edge_NativePtr
		//################################################################

	public:
		value class Edge_NativePtr
		{
		private protected:
			Ogre::EdgeData::Edge* _native;

			//Public Declarations
		public:
			property size_t* triIndex
			{
			public:
				size_t* get();
			}

			property size_t* vertIndex
			{
			public:
				size_t* get();
			}

			property size_t* sharedVertIndex
			{
			public:
				size_t* get();
			}

			property bool degenerate
			{
			public:
				bool get();
			public:
				void set(bool value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(EdgeData::Edge_NativePtr, Ogre::EdgeData::Edge);

			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static Edge_NativePtr Create();

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}

		};

		//################################################################
		//EdgeGroup_NativePtr
		//################################################################

	public:
		value class EdgeGroup_NativePtr
		{
			//Private Declarations
		private protected:
			Ogre::EdgeData::EdgeGroup* _native;

			//Public Declarations
		public:
			property size_t vertexSet
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property Mogre::VertexData^ vertexData
			{
			public:
				Mogre::VertexData^ get();
			}

			property size_t triStart
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property size_t triCount
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property Mogre::EdgeData::EdgeList^ edges
			{
			public:
				Mogre::EdgeData::EdgeList^ get();
			public:
				void set(Mogre::EdgeData::EdgeList^ value);
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(EdgeData::EdgeGroup_NativePtr, Ogre::EdgeData::EdgeGroup);

			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static EdgeGroup_NativePtr Create();

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}
		};

		//################################################################
		//Triangle_NativePtr
		//################################################################

	public:
		value class Triangle_NativePtr
		{
		private protected:
			Ogre::EdgeData::Triangle* _native;

		public:
			property size_t indexSet
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property size_t vertexSet
			{
			public:
				size_t get();
			public:
				void set(size_t value);
			}

			property size_t* vertIndex
			{
			public:
				size_t* get();
			}

			property size_t* sharedVertIndex
			{
			public:
				size_t* get();
			}

			DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_NATIVEPTRVALUECLASS(EdgeData::Triangle_NativePtr, Ogre::EdgeData::Triangle);

			property IntPtr NativePtr
			{
				IntPtr get() { return (IntPtr)_native; }
			}

			static Triangle_NativePtr Create();

			void DestroyNativePtr()
			{
				if (_native) { delete _native; _native = 0; }
			}

			property bool IsNull
			{
				bool get() { return (_native == 0); }
			}

			//Protected Declarations
		protected public:

		};

	public: INC_DECLARE_STLVECTOR(TriangleFaceNormalList, Mogre::Vector4, Ogre::Vector4, public:, private:);
	public: INC_DECLARE_STLVECTOR(TriangleLightFacingList, char, char, public:, private:);
	public: INC_DECLARE_STLVECTOR(TriangleList, Mogre::EdgeData::Triangle_NativePtr, Ogre::EdgeData::Triangle, public:, private:);
	public: INC_DECLARE_STLVECTOR(EdgeList, Mogre::EdgeData::Edge_NativePtr, Ogre::EdgeData::Edge, public:, private:);
	public: INC_DECLARE_STLVECTOR(EdgeGroupList, Mogre::EdgeData::EdgeGroup_NativePtr, Ogre::EdgeData::EdgeGroup, public:, private:);

	private protected:
		//Cached fields
		Mogre::EdgeData::TriangleList^ _triangles;
		Mogre::EdgeData::TriangleLightFacingList^ _triangleLightFacings;
		Mogre::EdgeData::EdgeGroupList^ _edgeGroups;

	public protected:
		EdgeData(IntPtr obj) : _native((Ogre::EdgeData*)obj.ToPointer()), _createdByCLR(false)
		{
		}

		EdgeData(Ogre::EdgeData* obj) : _native(obj), _createdByCLR(false)
		{
		}

		Ogre::EdgeData* _native;
		bool _createdByCLR;

	public:
		~EdgeData();
	protected:
		!EdgeData();

	public:
		EdgeData();

		property Mogre::EdgeData::TriangleList^ triangles
		{
		public:
			Mogre::EdgeData::TriangleList^ get();
		}

		property Mogre::EdgeData::TriangleLightFacingList^ triangleLightFacings
		{
		public:
			Mogre::EdgeData::TriangleLightFacingList^ get();
		}

		property Mogre::EdgeData::EdgeGroupList^ edgeGroups
		{
		public:
			Mogre::EdgeData::EdgeGroupList^ get();
		}

		property bool isClosed
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		void UpdateTriangleLightFacing(Mogre::Vector4 lightPos);
		void UpdateFaceNormals(size_t vertexSet, Mogre::HardwareVertexBufferSharedPtr^ positionBuffer);

		void Log(Mogre::Log^ log);

		property bool IsDisposed
		{
			virtual bool get()
			{
				return _native == nullptr;
			}
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(EdgeData);

	internal:
		property Ogre::EdgeData* UnmanagedPointer
		{
			Ogre::EdgeData* get()
			{
				return _native;
			}
		}
	};
}