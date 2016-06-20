#pragma once

#include "OgreRenderOperation.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class MeshPtr;
	ref class VertexData;
	ref class IndexData;

	public ref class RenderOperation
	{
	public:
		enum class OperationTypes
		{
			OT_POINT_LIST = Ogre::RenderOperation::OT_POINT_LIST,
			OT_LINE_LIST = Ogre::RenderOperation::OT_LINE_LIST,
			OT_LINE_STRIP = Ogre::RenderOperation::OT_LINE_STRIP,
			OT_TRIANGLE_LIST = Ogre::RenderOperation::OT_TRIANGLE_LIST,
			OT_TRIANGLE_STRIP = Ogre::RenderOperation::OT_TRIANGLE_STRIP,
			OT_TRIANGLE_FAN = Ogre::RenderOperation::OT_TRIANGLE_FAN,
			OT_PATCH_1_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_1_CONTROL_POINT,
			OT_PATCH_2_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_2_CONTROL_POINT,
			OT_PATCH_3_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_3_CONTROL_POINT,
			OT_PATCH_4_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_4_CONTROL_POINT,
			OT_PATCH_5_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_5_CONTROL_POINT,
			OT_PATCH_6_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_6_CONTROL_POINT,
			OT_PATCH_7_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_7_CONTROL_POINT,
			OT_PATCH_8_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_8_CONTROL_POINT,
			OT_PATCH_9_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_9_CONTROL_POINT,
			OT_PATCH_10_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_10_CONTROL_POINT,
			OT_PATCH_11_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_11_CONTROL_POINT,
			OT_PATCH_12_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_12_CONTROL_POINT,
			OT_PATCH_13_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_13_CONTROL_POINT,
			OT_PATCH_14_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_14_CONTROL_POINT,
			OT_PATCH_15_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_15_CONTROL_POINT,
			OT_PATCH_16_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_16_CONTROL_POINT,
			OT_PATCH_17_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_17_CONTROL_POINT,
			OT_PATCH_18_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_18_CONTROL_POINT,
			OT_PATCH_19_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_19_CONTROL_POINT,
			OT_PATCH_20_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_20_CONTROL_POINT,
			OT_PATCH_21_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_21_CONTROL_POINT,
			OT_PATCH_22_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_22_CONTROL_POINT,
			OT_PATCH_23_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_23_CONTROL_POINT,
			OT_PATCH_24_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_24_CONTROL_POINT,
			OT_PATCH_25_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_25_CONTROL_POINT,
			OT_PATCH_26_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_26_CONTROL_POINT,
			OT_PATCH_27_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_27_CONTROL_POINT,
			OT_PATCH_28_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_28_CONTROL_POINT,
			OT_PATCH_29_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_29_CONTROL_POINT,
			OT_PATCH_30_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_30_CONTROL_POINT,
			OT_PATCH_31_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_31_CONTROL_POINT,
			OT_PATCH_32_CONTROL_POINT = Ogre::RenderOperation::OT_PATCH_32_CONTROL_POINT
		};

	private:
		Mogre::VertexData^ _vertexData;
		Mogre::IndexData^ _indexData;

	public protected:
		RenderOperation(Ogre::RenderOperation* obj) : _native(obj), _createdByCLR(false)
		{
		}

		~RenderOperation()
		{
			if (_createdByCLR &&_native)
			{
				delete _native;
				_native = 0;
			}
		}

		Ogre::RenderOperation* _native;
		bool _createdByCLR;

	public:
		RenderOperation();

		property Mogre::VertexData^ vertexData
		{
		public:
			Mogre::VertexData^ get();
		public:
			void set(Mogre::VertexData^ value);
		}

		property Mogre::RenderOperation::OperationTypes operationType
		{
		public:
			Mogre::RenderOperation::OperationTypes get();
		public:
			void set(Mogre::RenderOperation::OperationTypes value);
		}

		property bool useIndexes
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property Mogre::IndexData^ indexData
		{
		public:
			Mogre::IndexData^ get();
		public:
			void set(Mogre::IndexData^ value);
		}

		/*property Mogre::IRenderable^ srcRenderable
		{
		public:
			Mogre::IRenderable^ get();
		}*/

		property size_t numberOfInstances
		{
		public:
			size_t get();
		public:
			void set(size_t value);
		}

		property bool renderToVertexBuffer
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		property bool useGlobalInstancingVertexBufferIsAvailable
		{
		public:
			bool get();
		public:
			void set(bool value);
		}

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderOperation);
	};
}