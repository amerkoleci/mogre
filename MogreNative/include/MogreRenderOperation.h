#pragma once

#include "OgreRenderOperation.h"
#include "Marshalling.h"

namespace Mogre
{
	ref class MeshPtr;

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
			OT_TRIANGLE_FAN = Ogre::RenderOperation::OT_TRIANGLE_FAN
		};

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

		DEFINE_MANAGED_NATIVE_CONVERSIONS(RenderOperation);
	};
}